using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

using Basler.Pylon;

namespace Utility.Cameras
{
    /// <summary>
    /// Basler相机属性设置控件
    /// </summary>
    [ToolboxBitmap(typeof(BaslerCamProperty), "Basler.ico")]
    public partial class BaslerCamProperty : UserControl
    {
        #region Fields
        private IList<string> serialNumber = new List<string>(), displayName = new List<string>();
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaslerCamProperty()
        {
            InitializeComponent();
            this.tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(this.tableLayoutPanel1, true, null);
            this.tableLayoutPanel2.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(this.tableLayoutPanel2, true, null);
            this.tableLayoutPanel3.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(this.tableLayoutPanel3, true, null);
            this.tableLayoutPanel4.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(this.tableLayoutPanel4, true, null);
            this.tableLayoutPanel5.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
              ?.SetValue(this.tableLayoutPanel5, true, null);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Basler相机对象 使用方法：在控件显示之前对其赋对象引用，参数修改后调用ConfirmChanging()方法，之后取出Camera对象即可
        /// </summary>
        public BaslerCam Camera { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 确认属性修改
        /// </summary>
        public void ConfirmChanging()
        {
            if (!this.Camera.IsConnected)
                return;

            bool isgrabbing = this.Camera.IsGrabbing;
            if (isgrabbing)
                this.Camera.LiveStop();

            this.Camera.FrameRateEnable = this.fpsEnable.Checked;
            this.Camera.DeviceFrameRate = double.Parse(this.fpsValue.Text);
            this.Camera.PixelFormat = this.pixelFormat.Text;
            this.Camera.ImageWidth = long.Parse(this.imageWidth.Text);
            this.Camera.ImageHeight = long.Parse(this.imageHeight.Text);

            this.Camera.ExposureAuto = this.exposureAuto.Checked;
            this.Camera.ExposureTime = long.Parse(this.exposureTime.Text);
            this.Camera.GainAuto = this.gainAuto.Checked;
            this.Camera.GainValue = long.Parse(this.gainValue.Text);
            this.Camera.GammaEnable = this.gammaEnable.Checked;
            this.Camera.Gamma = double.Parse(this.gammaValue.Text);
            if (this.Camera.SharpnessAvailable)
            {
                this.Camera.SharpnessEnable = this.sharpnessEnable.Checked;
                this.Camera.Sharpness = long.Parse(this.sharpnessValue.Text);
            }

            this.Camera.TriggerSelector = this.triggerSelector.Text;
            this.Camera.TriggerEnable = this.triggerEnable.Checked;
            this.Camera.TriggerSource = this.triggerSource.Text;
            this.Camera.TriggerPolarity = this.triggerPolarity.Checked;
            this.Camera.TriggerDelay = double.Parse(this.triggerDelay.Text);

            this.Camera.PacketSize = long.Parse(this.packetSize.Text);
            this.Camera.DataDelay = long.Parse(this.dataDelay.Text);
            this.Camera.TransferDelay = long.Parse(this.transferDelay.Text);

            if (isgrabbing)
                this.Camera.LiveStart();
        }

        private void BaslerCamProperty_Load(object sender, EventArgs e)
        {
            if (this.Camera == null)
                return;

            int ind = this.Camera.GetConnectedCameras(ref this.serialNumber, ref this.displayName);
            this.cameraSelector.Items.Clear();
            foreach (string name in this.displayName)
                this.cameraSelector.Items.Add(name);
            if (ind != -1)
                this.cameraSelector.Text = this.cameraSelector.Items[ind].ToString();
        }

        private void cameraSelector_TextChanged(object sender, EventArgs e)
        {
            if (this.cameraSelector.Text == "")
                SetEnable(false);
            else
            {
                if (this.Camera.SerialNumber != this.serialNumber[this.cameraSelector.SelectedIndex])
                {
                    this.Camera.CloseCamera();
                    var newcamera = new Camera(this.serialNumber[this.cameraSelector.SelectedIndex]);
                    this.Camera.SetCamera(newcamera);
                    newcamera.Open();
                }

                this.fpsEnable.Checked = this.Camera.FrameRateEnable;
                this.fpsValue.Text = this.Camera.DeviceFrameRate.ToString("f");
                IList<string> pixelFormats = new List<string>();
                this.Camera.GetCameraParamValues(PLCamera.PixelFormat, ref pixelFormats);
                this.pixelFormat.Items.Clear();
                foreach (string format in pixelFormats)
                    this.pixelFormat.Items.Add(format);
                this.pixelFormat.Text = this.Camera.PixelFormat;
                this.imageWidth.Text = this.Camera.ImageWidth.ToString();
                this.imageHeight.Text = this.Camera.ImageHeight.ToString();

                this.exposureAuto.Checked = this.Camera.ExposureAuto;
                this.exposureTime.Text = this.Camera.ExposureTime.ToString();
                this.gainAuto.Checked = this.Camera.GainAuto;
                this.gainValue.Text = this.Camera.GainValue.ToString();
                this.gammaEnable.Checked = this.Camera.GammaEnable;
                this.gammaValue.Text = this.Camera.Gamma.ToString("f");
                if (this.Camera.SharpnessAvailable)
                {
                    this.sharpnessEnable.Checked = this.Camera.SharpnessEnable;
                    this.sharpnessValue.Text = this.Camera.Sharpness.ToString();
                }

                IList<string> triggerSelectors = new List<string>();
                this.Camera.GetCameraParamValues(PLCamera.TriggerSelector, ref triggerSelectors);
                this.triggerSelector.Items.Clear();
                foreach (string selector in triggerSelectors)
                    this.triggerSelector.Items.Add(selector);
                this.triggerSelector.Text = this.Camera.TriggerSelector;
                this.triggerEnable.Checked = this.Camera.TriggerEnable;
                IList<string> triggerSources = new List<string>();
                this.Camera.GetCameraParamValues(PLCamera.TriggerSource, ref triggerSources);
                this.triggerSource.Items.Clear();
                foreach (string source in triggerSources)
                    this.triggerSource.Items.Add(source);
                this.triggerSource.Text = this.Camera.TriggerSource;
                this.triggerPolarity.Checked = this.Camera.TriggerPolarity;
                this.triggerDelay.Text = this.Camera.TriggerDelay.ToString(CultureInfo.InvariantCulture);

                this.packetSize.Text = this.Camera.PacketSize.ToString();
                this.dataDelay.Text = this.Camera.DataDelay.ToString();
                this.transferDelay.Text = this.Camera.TransferDelay.ToString();
                SetEnable(true);
            }
        }

        private void exposureAuto_CheckedChanged(object sender, EventArgs e) => this.exposureTime.Enabled = !this.exposureAuto.Checked;

        private void fpsEnable_CheckedChanged(object sender, EventArgs e) => this.fpsValue.Enabled = this.fpsEnable.Checked;

        private void gainAuto_CheckedChanged(object sender, EventArgs e) => this.gainValue.Enabled = !this.gainAuto.Checked;

        private void gammaEnable_CheckedChanged(object sender, EventArgs e) => this.gammaValue.Enabled = this.gammaEnable.Checked;

        private void SetEnable(bool enable)
        {
            this.fpsEnable.Enabled = enable;
            this.fpsValue.Enabled = this.fpsEnable.Checked && enable;
            this.pixelFormat.Enabled = enable;
            this.imageWidth.Enabled = enable;
            this.imageHeight.Enabled = enable;
            this.exposureAuto.Enabled = enable;
            this.exposureTime.Enabled = !this.exposureAuto.Checked && enable;
            this.gainAuto.Enabled = enable;
            this.gainValue.Enabled = !this.gainAuto.Checked && enable;
            this.gammaEnable.Enabled = enable;
            this.gammaValue.Enabled = this.gammaEnable.Checked && enable;
            if (this.Camera.SharpnessAvailable)
            {
                this.sharpnessEnable.Enabled = enable;
                this.sharpnessValue.Enabled = this.sharpnessEnable.Checked && enable;
            }

            this.triggerSelector.Enabled = enable;
            this.triggerEnable.Enabled = enable;
            this.triggerSource.Enabled = enable;
            this.triggerPolarity.Enabled = enable;
            this.triggerDelay.Enabled = enable;
            this.packetSize.Enabled = enable;
            this.dataDelay.Enabled = enable;
            this.transferDelay.Enabled = enable;
        }

        private void sharpnessEnable_CheckedChanged(object sender, EventArgs e) => this.sharpnessValue.Enabled = this.sharpnessEnable.Checked;
        #endregion
    }
}
