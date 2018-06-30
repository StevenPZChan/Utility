using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using HalconDotNet;

namespace Utility.Form
{
    /// <summary>
    /// 相机标定控件
    /// 使用方法：先构造一个函数HObject MySnapshot()，目的是获得单帧图像，
    /// 然后订阅事件[CalibTool对象].SnapshotEvent += MySnapshot();
    /// 可选：回调函数中调用[CalibTool对象].DisplayContinuous(ho_Image);
    /// </summary>
    [ToolboxBitmap(typeof(CalibTool), "hdevelop_icon.ico")]
    public partial class CalibTool : UserControl
    {
        /// <summary>
        /// 采集图像事件，需要订阅相机采集图像的函数
        /// </summary>
        [Description("采集图像事件")]
        public event Func<HImage> SnapshotEvent;

        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple CameraParameters;
        /// <summary>
        /// 相机外参，未计算标定板补偿
        /// </summary>
        public HPose CameraPose;
        /// <summary>
        /// 标定误差，pixel
        /// </summary>
        public double Errors { get; private set; }

        private List<HImage> Images = new List<HImage>();
        private int width, height;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalibTool()
        {
            InitializeComponent();
            Images.Clear();
            this.Disposed += (sender, e) => {
                foreach (HImage i in Images)
                    i.Dispose();
            };
        }

        /// <summary>
        /// 需要在相机回调函数中传入图像HObject对象
        /// </summary>
        /// <param name="ho_Image">图像HObject对象</param>
        public void DisplayContinuous(HImage ho_Image)
        {
            ho_Image.GetImageSize(out width, out height);
            HSystem.SetSystem("width", width);
            HSystem.SetSystem("height", height);
            hWinContinuous.HalconWindow.SetPart(0, 0, height - 1, width - 1);
            hWinContinuous.HalconWindow.DispObj(ho_Image);
            if (cTextBoxDesc.Text == "")
                return;
            HRegion ho_Calib = ho_Image.FindCaltab(cTextBoxDesc.Text, 3, 112, 5);
            hWinContinuous.HalconWindow.DispObj(ho_Calib);
            ho_Calib.Dispose();
        }

        private void buttonDesc_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog())
                cTextBoxDesc.Text = openFileDialog1.FileName;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
                cTextBoxResult.Text = folderBrowserDialog1.SelectedPath;
        }

        private void buttonCapture_Click(object sender, EventArgs e)
        {
            if (SnapshotEvent == null)
                return;
            HImage ho_Image = SnapshotEvent.Invoke();
            int num = Images.Count;
            Images.Add(ho_Image);
            listBox1.Items.Add($"图像 {(num + 1):d2}");
            listBox1.SelectedIndex = num;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            int selected = listBox1.SelectedIndex;
            if (selected > -1)
            {
                Images.RemoveAt(selected);
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            }
            if (selected > 0)
                listBox1.SelectedIndex = selected - 1;
        }

        private void buttonCalib_Click(object sender, EventArgs e)
        {
            if (cTextBoxDesc.Text == "")
            {
                MessageBox.Show("请选择标定描述文件！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int ReferenceIndex = listBox1.SelectedIndex;
                HTuple hv_StartParameters = new HTuple();
                HTuple hv_TmpCtrl_FindCalObjParNames = new HTuple();
                HTuple hv_TmpCtrl_FindCalObjParValues = new HTuple();
                hv_StartParameters[0] = 0.001 * float.Parse(cTextBoxFocus.Text);
                hv_StartParameters[1] = 0;
                hv_StartParameters[2] = 1e-6 * float.Parse(cTextBoxSx.Text);
                hv_StartParameters[3] = 1e-6 * float.Parse(cTextBoxSy.Text);
                hv_StartParameters[4] = width / 2;
                hv_StartParameters[5] = height / 2;
                hv_StartParameters[6] = width;
                hv_StartParameters[7] = height;
                hv_TmpCtrl_FindCalObjParNames[0] = "gap_tolerance";
                hv_TmpCtrl_FindCalObjParNames[1] = "alpha";
                hv_TmpCtrl_FindCalObjParNames[2] = "skip_find_caltab";
                hv_TmpCtrl_FindCalObjParValues[0] = 1;
                hv_TmpCtrl_FindCalObjParValues[1] = 1;
                hv_TmpCtrl_FindCalObjParValues[2] = "false";

                HCalibData hv_CalibHandle = new HCalibData("calibration_object", 1, 1);
                hv_CalibHandle.SetCalibDataCamParam(0, "area_scan_division", hv_StartParameters);
                hv_CalibHandle.SetCalibDataCalibObject(0, cTextBoxDesc.Text);
                for (int i = 0; i < Images.Count; i++)
                    hv_CalibHandle.FindCalibObject(Images[i], 0, 0, i, hv_TmpCtrl_FindCalObjParNames, hv_TmpCtrl_FindCalObjParValues);

                hv_CalibHandle.SetCalibData("camera", new HTuple(0), "excluded_settings", new HTuple("sx", "sy", "cx", "cy"));
                Errors = hv_CalibHandle.CalibrateCameras();
                CameraParameters = hv_CalibHandle.GetCalibData("camera", 0, "params");
                CameraPose = new HPose(hv_CalibHandle.GetCalibData("calib_obj_pose", new HTuple(0, ReferenceIndex), new HTuple("pose")));
                hv_CalibHandle.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"出现错误：{ex.Message}，请检查标定数据！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            buttonSave.Enabled = Errors > 0 && Errors < 1;
            if (Errors < 1)
                MessageBox.Show($"标定成功！误差为{Errors:f}pixel，请保存！", "标定成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (cTextBoxResult.Text == "")
            {
                MessageBox.Show("请选择保存路径！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HMisc.WriteCamPar(CameraParameters, cTextBoxResult.Text + @"\campar.cal");
            CameraPose.WritePose(cTextBoxResult.Text + @"\campose.dat");
            MessageBox.Show("保存成功！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDelete.Enabled = listBox1.Items.Count > 0;
            buttonCalib.Enabled = listBox1.Items.Count > 3;
            if (listBox1.SelectedIndex > -1)
            {
                hWinPic.SetFullImagePart(Images[listBox1.SelectedIndex]);
                hWinPic.HalconWindow.DispObj(Images[listBox1.SelectedIndex]);
            }
        }
    }
}
