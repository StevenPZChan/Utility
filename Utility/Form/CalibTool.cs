using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using HalconDotNet;

namespace Utility.Form
{
    /// <summary>
    /// 相机标定控件 使用方法：先构造一个函数HObject MySnapshot()，目的是获得单帧图像， 然后订阅事件[CalibTool对象].SnapshotEvent +=
    /// MySnapshot(); 可选：回调函数中调用[CalibTool对象].DisplayContinuous(ho_Image);
    /// </summary>
    [ToolboxBitmap(typeof(CalibTool), "hdevelop_icon.ico")]
    public partial class CalibTool : UserControl
    {
        #region Fields
        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple CameraParameters;
        /// <summary>
        /// 相机外参，未计算标定板补偿
        /// </summary>
        public HPose CameraPose;
        private readonly List<HImage> _images = new List<HImage>();
        private int width, height;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalibTool()
        {
            InitializeComponent();
            this._images.Clear();
            Disposed += (sender, e) => {
                foreach (HImage i in this._images)
                    i.Dispose();
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// 标定误差，pixel
        /// </summary>
        public double Errors { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// 采集图像事件，需要订阅相机采集图像的函数
        /// </summary>
        [Description("采集图像事件")]
        public event Func<HImage> SnapshotEvent;
        #endregion

        #region Methods
        /// <summary>
        /// 需要在相机回调函数中传入图像HObject对象
        /// </summary>
        /// <param name="ho_Image">图像HObject对象</param>
        public void DisplayContinuous(HImage ho_Image)
        {
            ho_Image.GetImageSize(out this.width, out this.height);
            HSystem.SetSystem("width", this.width);
            HSystem.SetSystem("height", this.height);
            this.hWinContinuous.HalconWindow.SetPart(0, 0, this.height - 1, this.width - 1);
            this.hWinContinuous.HalconWindow.DispObj(ho_Image);
            if (this.cTextBoxDesc.Text == "")
                return;

            HRegion ho_Calib = ho_Image.FindCaltab(this.cTextBoxDesc.Text, 3, 112, 5);
            this.hWinContinuous.HalconWindow.DispObj(ho_Calib);
            ho_Calib.Dispose();
        }

        private void buttonCalib_Click(object sender, EventArgs e)
        {
            if (this.cTextBoxDesc.Text == "")
            {
                MessageBox.Show("请选择标定描述文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int referenceIndex = this.listBox1.SelectedIndex;
                var hv_StartParameters = new HTuple();
                var hv_TmpCtrl_FindCalObjParNames = new HTuple();
                var hv_TmpCtrl_FindCalObjParValues = new HTuple();
                hv_StartParameters[0] = 0.001 * float.Parse(this.cTextBoxFocus.Text);
                hv_StartParameters[1] = 0;
                hv_StartParameters[2] = 1e-6 * float.Parse(this.cTextBoxSx.Text);
                hv_StartParameters[3] = 1e-6 * float.Parse(this.cTextBoxSy.Text);
                hv_StartParameters[4] = this.width / 2;
                hv_StartParameters[5] = this.height / 2;
                hv_StartParameters[6] = this.width;
                hv_StartParameters[7] = this.height;
                hv_TmpCtrl_FindCalObjParNames[0] = "gap_tolerance";
                hv_TmpCtrl_FindCalObjParNames[1] = "alpha";
                hv_TmpCtrl_FindCalObjParNames[2] = "skip_find_caltab";
                hv_TmpCtrl_FindCalObjParValues[0] = 1;
                hv_TmpCtrl_FindCalObjParValues[1] = 1;
                hv_TmpCtrl_FindCalObjParValues[2] = "false";

                using (var hv_CalibHandle = new HCalibData("calibration_object", 1, 1))
                {
                    hv_CalibHandle.SetCalibDataCamParam(0, "area_scan_division", hv_StartParameters);
                    hv_CalibHandle.SetCalibDataCalibObject(0, this.cTextBoxDesc.Text);
                    for (var i = 0; i < this._images.Count; i++)
                        hv_CalibHandle.FindCalibObject(this._images[i], 0, 0, i, hv_TmpCtrl_FindCalObjParNames, hv_TmpCtrl_FindCalObjParValues);

                    hv_CalibHandle.SetCalibData("camera", new HTuple(0), "excluded_settings", new HTuple("sx", "sy", "cx", "cy"));
                    this.Errors = hv_CalibHandle.CalibrateCameras();
                    this.CameraParameters = hv_CalibHandle.GetCalibData("camera", 0, "params");
                    this.CameraPose = new HPose(hv_CalibHandle.GetCalibData("calib_obj_pose", new HTuple(0, referenceIndex), new HTuple("pose")));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"出现错误：{ex.Message}，请检查标定数据！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.buttonSave.Enabled = this.Errors > 0 && this.Errors < 1;
            if (this.Errors < 1)
                MessageBox.Show($"标定成功！误差为{this.Errors:f}pixel，请保存！", "标定成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            if (SnapshotEvent == null)
                return;

            HImage ho_Image = SnapshotEvent.Invoke();
            int num = this._images.Count;
            this._images.Add(ho_Image);
            this.listBox1.Items.Add($"图像 {num + 1:d2}");
            this.listBox1.SelectedIndex = num;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int selected = this.listBox1.SelectedIndex;
            if (selected > -1)
            {
                this._images.RemoveAt(selected);
                this.listBox1.Items.RemoveAt(this.listBox1.Items.Count - 1);
            }

            if (selected > 0)
                this.listBox1.SelectedIndex = selected - 1;
        }

        private void buttonDesc_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.openFileDialog1.ShowDialog())
                this.cTextBoxDesc.Text = this.openFileDialog1.FileName;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == this.folderBrowserDialog1.ShowDialog())
                this.cTextBoxResult.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.cTextBoxResult.Text == "")
            {
                MessageBox.Show("请选择保存路径！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            HMisc.WriteCamPar(this.CameraParameters, this.cTextBoxResult.Text + @"\campar.cal");
            this.CameraPose.WritePose(this.cTextBoxResult.Text + @"\campose.dat");
            MessageBox.Show("保存成功！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonDelete.Enabled = this.listBox1.Items.Count > 0;
            this.buttonCalib.Enabled = this.listBox1.Items.Count > 3;
            if (this.listBox1.SelectedIndex <= -1)
                return;

            this.hWinPic.SetFullImagePart(this._images[this.listBox1.SelectedIndex]);
            this.hWinPic.HalconWindow.DispObj(this._images[this.listBox1.SelectedIndex]);
        }
        #endregion
    }
}
