using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using Basler.Pylon;

namespace Utility.Cameras
{
    /// <summary>
    /// Basler相机属性设置控件
    /// </summary>
    public partial class BaslerCamProperty : UserControl
    {
        /// <summary>
        /// Basler相机对象
        /// 使用方法：在控件显示之前对其赋对象引用，参数修改后调用ConfirmChanging()方法，之后取出Camera对象即可
        /// </summary>
        public BaslerCam Camera { get; set; }

        private IList<string> serialNumber, displayName;
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaslerCamProperty()
        {
            InitializeComponent();
            tableLayoutPanel1.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null);
            tableLayoutPanel2.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tableLayoutPanel2, true, null);
            tableLayoutPanel3.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tableLayoutPanel3, true, null);
            tableLayoutPanel4.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tableLayoutPanel4, true, null);
            tableLayoutPanel5.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(tableLayoutPanel5, true, null);

            serialNumber = new List<string>();
            displayName = new List<string>();
        }

        /// <summary>
        /// 确认属性修改
        /// </summary>
        public void ConfirmChanging()
        {
            if (!Camera.IsConnected)
                return;

            bool isgrabbing = Camera.IsGrabbing;
            if (isgrabbing)
                Camera.LiveStop();

            Camera.FrameRateEnable = fpsEnable.Checked;
            Camera.DeviceFrameRate = double.Parse(fpsValue.Text);
            Camera.PixelFormat = pixelFormat.Text;
            Camera.ImageWidth = long.Parse(imageWidth.Text);
            Camera.ImageHeight = long.Parse(imageHeight.Text);

            Camera.ExposureAuto = exposureAuto.Checked;
            Camera.ExposureTime = long.Parse(exposureTime.Text);
            Camera.GainAuto = gainAuto.Checked;
            Camera.GainValue = long.Parse(gainValue.Text);
            Camera.GammaEnable = gammaEnable.Checked;
            Camera.Gamma = double.Parse(gammaValue.Text);
            if (Camera.SharpnessAvailable)
            {
                Camera.SharpnessEnable = sharpnessEnable.Checked;
                Camera.Sharpness = long.Parse(sharpnessValue.Text);
            }

            Camera.TriggerSelector = triggerSelector.Text;
            Camera.TriggerEnable = triggerEnable.Checked;
            Camera.TriggerSource = triggerSource.Text;
            Camera.TriggerPolarity = triggerPolarity.Checked;
            Camera.TriggerDelay = double.Parse(triggerDelay.Text);

            Camera.PacketSize = long.Parse(packetSize.Text);
            Camera.DataDelay = long.Parse(dataDelay.Text);
            Camera.TransferDelay = long.Parse(transferDelay.Text);

            if (isgrabbing)
                Camera.LiveStart();
        }

        private void BaslerCamProperty_Load(object sender, EventArgs e)
        {
            if (Camera == null)
                return;
            int ind = Camera.GetConnectedCameras(ref serialNumber, ref displayName);
            cameraSelector.Items.Clear();
            foreach (var name in displayName)
                cameraSelector.Items.Add(name);
            if (ind != -1)
                cameraSelector.Text = cameraSelector.Items[ind].ToString();
        }

        private void cameraSelector_TextChanged(object sender, EventArgs e)
        {
            if (cameraSelector.Text == "")
                SetEnable(false);
            else
            {
                if (Camera.SerialNumber != serialNumber[cameraSelector.SelectedIndex])
                {
                    Camera.CloseCamera();
                    Camera newcamera = new Camera(serialNumber[cameraSelector.SelectedIndex]);
                    Camera.SetCamera(newcamera);
                    newcamera.Open();
                }

                fpsEnable.Checked = Camera.FrameRateEnable;
                fpsValue.Text = Camera.DeviceFrameRate.ToString("f");
                IList<string> pixelFormats = new List<string>();
                Camera.GetCameraParamValues(PLCamera.PixelFormat, ref pixelFormats);
                pixelFormat.Items.Clear();
                foreach (var format in pixelFormats)
                    pixelFormat.Items.Add(format);
                pixelFormat.Text = Camera.PixelFormat;
                imageWidth.Text = Camera.ImageWidth.ToString();
                imageHeight.Text = Camera.ImageHeight.ToString();

                exposureAuto.Checked = Camera.ExposureAuto;
                exposureTime.Text = Camera.ExposureTime.ToString();
                gainAuto.Checked = Camera.GainAuto;
                gainValue.Text = Camera.GainValue.ToString();
                gammaEnable.Checked = Camera.GammaEnable;
                gammaValue.Text = Camera.Gamma.ToString("f");
                if (Camera.SharpnessAvailable)
                {
                    sharpnessEnable.Checked = Camera.SharpnessEnable;
                    sharpnessValue.Text = Camera.Sharpness.ToString();
                }

                IList<string> triggerSelectors = new List<string>();
                Camera.GetCameraParamValues(PLCamera.TriggerSelector, ref triggerSelectors);
                triggerSelector.Items.Clear();
                foreach (var selector in triggerSelectors)
                    triggerSelector.Items.Add(selector);
                triggerSelector.Text = Camera.TriggerSelector;
                triggerEnable.Checked = Camera.TriggerEnable;
                IList<string> triggerSources = new List<string>();
                Camera.GetCameraParamValues(PLCamera.TriggerSource, ref triggerSources);
                triggerSource.Items.Clear();
                foreach (var source in triggerSources)
                    triggerSource.Items.Add(source);
                triggerSource.Text = Camera.TriggerSource;
                triggerPolarity.Checked = Camera.TriggerPolarity;
                triggerDelay.Text = Camera.TriggerDelay.ToString();

                packetSize.Text = Camera.PacketSize.ToString();
                dataDelay.Text = Camera.DataDelay.ToString();
                transferDelay.Text = Camera.TransferDelay.ToString();
                SetEnable(true);
            }
        }

        private void fpsEnable_CheckedChanged(object sender, EventArgs e)
        {
            fpsValue.Enabled = fpsEnable.Checked;
        }

        private void exposureAuto_CheckedChanged(object sender, EventArgs e)
        {
            exposureTime.Enabled = !exposureAuto.Checked;
        }

        private void gainAuto_CheckedChanged(object sender, EventArgs e)
        {
            gainValue.Enabled = !gainAuto.Checked;
        }

        private void gammaEnable_CheckedChanged(object sender, EventArgs e)
        {
            gammaValue.Enabled = gammaEnable.Checked;
        }

        private void sharpnessEnable_CheckedChanged(object sender, EventArgs e)
        {
            sharpnessValue.Enabled = sharpnessEnable.Checked;
        }

        private void SetEnable(bool enable)
        {
            fpsEnable.Enabled = enable;
            fpsValue.Enabled = fpsEnable.Checked && enable;
            pixelFormat.Enabled = enable;
            imageWidth.Enabled = enable;
            imageHeight.Enabled = enable;
            exposureAuto.Enabled = enable;
            exposureTime.Enabled = !exposureAuto.Checked && enable;
            gainAuto.Enabled = enable;
            gainValue.Enabled = !gainAuto.Checked && enable;
            gammaEnable.Enabled = enable;
            gammaValue.Enabled = gammaEnable.Checked && enable;
            if (Camera.SharpnessAvailable)
            {
                sharpnessEnable.Enabled = enable;
                sharpnessValue.Enabled = sharpnessEnable.Checked && enable;
            }
            triggerSelector.Enabled = enable;
            triggerEnable.Enabled = enable;
            triggerSource.Enabled = enable;
            triggerPolarity.Enabled = enable;
            triggerDelay.Enabled = enable;
            packetSize.Enabled = enable;
            dataDelay.Enabled = enable;
            transferDelay.Enabled = enable;
        }
    }

    /// <summary>
    /// Basler相机辅助类
    /// </summary>
    public class BaslerCam : Component
    {
        #region 属性
        /// <summary>
        /// 相机自定义名称，可附加到回调函数
        /// </summary>
        [Browsable(true), Category("相机参数"), Description("相机自定义名称，可附加到回调函数")]
        public string DeviceName { get; set; }
        /// <summary>
        /// 相机编号
        /// </summary>
        [Category("相机参数"), Description("相机编号")]
        public int Index { get; set; }
        /// <summary>
        /// 相机序列号
        /// </summary>
        [Category("相机参数"), Description("相机序列号")]
        public string SerialNumber
        {
            get { return camera.CameraInfo[CameraInfoKey.SerialNumber]; }
        }
        /// <summary>
        /// 相机连接状态
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (camera == null)
                    return false;
                return camera.IsConnected;
            }
        }
        /// <summary>
        /// 相机采集状态
        /// </summary>
        public bool IsGrabbing
        {
            get
            {
                if (camera == null)
                    return false;
                return camera.StreamGrabber.IsGrabbing;
            }
        }
        /// <summary>
        /// 相机帧率设置开关
        /// </summary>
        [Category("相机参数"), Description("相机帧率设置开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FrameRateEnable
        {
            get { return camera.Parameters[PLCamera.AcquisitionFrameRateEnable].GetValue(); }
            set { camera.Parameters[PLCamera.AcquisitionFrameRateEnable].TrySetValue(value); }
        }
        /// <summary>
        /// 相机帧率
        /// </summary>
        [Category("相机参数"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double DeviceFrameRate
        {
            get { return camera.Parameters[PLCamera.AcquisitionFrameRateAbs].GetValue(); }
            set { camera.Parameters[PLCamera.AcquisitionFrameRateAbs].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }
        /// <summary>
        /// 图像格式
        /// </summary>
        [Category("相机参数"), Description("图像格式"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PixelFormat
        {
            get { return camera.Parameters[PLCamera.PixelFormat].GetValue(); }
            set { camera.Parameters[PLCamera.PixelFormat].TrySetValue(value); }
        }
        /// <summary>
        /// 图像宽
        /// </summary>
        [Category("相机参数"), Description("图像宽"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ImageWidth
        {
            get { return camera.Parameters[PLCamera.Width].GetValue(); }
            set { camera.Parameters[PLCamera.Width].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 图像高
        /// </summary>
        [Category("相机参数"), Description("图像高"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ImageHeight
        {
            get { return camera.Parameters[PLCamera.Height].GetValue(); }
            set { camera.Parameters[PLCamera.Height].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        internal bool SharpnessAvailable
        {
            get { return !camera.Parameters[PLCamera.DemosaicingMode].IsEmpty; }
        }
        /// <summary>
        /// 锐度开关
        /// </summary>
        [Category("相机参数"), Description("锐度开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SharpnessEnable
        {
            get { return camera.Parameters[PLCamera.DemosaicingMode].GetValue() == PLCamera.DemosaicingMode.BaslerPGI; }
            set { camera.Parameters[PLCamera.DemosaicingMode].TrySetValue(value ? PLCamera.DemosaicingMode.BaslerPGI : PLCamera.DemosaicingMode.Simple); }
        }
        /// <summary>
        /// 锐度
        /// </summary>
        [Category("相机参数"), Description("锐度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long Sharpness
        {
            get { return camera.Parameters[PLCamera.SharpnessEnhancementRaw].GetValue(); }
            set { camera.Parameters[PLCamera.SharpnessEnhancementRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 灰度系数开关
        /// </summary>
        [Category("相机参数"), Description("灰度系数开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GammaEnable
        {
            get { return camera.Parameters[PLCamera.GammaEnable].GetValue(); }
            set { camera.Parameters[PLCamera.GammaEnable].TrySetValue(value); }
        }
        /// <summary>
        /// 灰度系数
        /// </summary>
        [Category("相机参数"), Description("灰度系数"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Gamma
        {
            get { return camera.Parameters[PLCamera.Gamma].GetValue(); }
            set { camera.Parameters[PLCamera.Gamma].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }
        /// <summary>
        /// 增益开关
        /// </summary>
        [Category("相机参数"), Description("增益开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GainAuto
        {
            get { return camera.Parameters[PLCamera.GainAuto].GetValue() == PLCamera.GainAuto.Continuous; }
            set { camera.Parameters[PLCamera.GainAuto].TrySetValue(value ? PLCamera.GainAuto.Continuous : PLCamera.GainAuto.Off); }
        }
        /// <summary>
        /// 增益值，0.01dB
        /// </summary>
        [Category("相机参数"), Description("增益值"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long GainValue
        {
            get { return camera.Parameters[PLCamera.GainRaw].GetValue(); }
            set { camera.Parameters[PLCamera.GainRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 曝光开关
        /// </summary>
        [Category("相机参数"), Description("曝光开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExposureAuto
        {
            get { return camera.Parameters[PLCamera.ExposureAuto].GetValue() == PLCamera.ExposureAuto.Continuous; }
            set { camera.Parameters[PLCamera.ExposureAuto].TrySetValue(value ? PLCamera.ExposureAuto.Continuous : PLCamera.ExposureAuto.Off); }
        }
        /// <summary>
        /// 曝光时间，us
        /// </summary>
        [Category("相机参数"), Description("曝光时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ExposureTime
        {
            get { return camera.Parameters[PLCamera.ExposureTimeRaw].GetValue(); }
            set { camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 硬触发开关
        /// </summary>
        [Category("相机参数"), Description("硬触发开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerEnable
        {
            get { return camera.Parameters[PLCamera.TriggerMode].GetValue() == PLCamera.TriggerMode.On; }
            set { camera.Parameters[PLCamera.TriggerMode].TrySetValue(value ? PLCamera.TriggerMode.On : PLCamera.TriggerMode.Off); }
        }
        /// <summary>
        /// 硬触发极性
        /// </summary>
        [Category("相机参数"), Description("硬触发极性（是否上升沿）"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerPolarity
        {
            get { return camera.Parameters[PLCamera.TriggerActivation].GetValue() == PLCamera.TriggerActivation.RisingEdge; }
            set { camera.Parameters[PLCamera.TriggerActivation].TrySetValue(value ? PLCamera.TriggerActivation.RisingEdge : PLCamera.TriggerActivation.FallingEdge); }
        }
        /// <summary>
        /// 触发延迟时间，us
        /// </summary>
        [Category("相机参数"), Description("触发延迟时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TriggerDelay
        {
            get { return camera.Parameters[PLCamera.TriggerDelayAbs].GetValue(); }
            set { camera.Parameters[PLCamera.TriggerDelayAbs].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }
        /// <summary>
        /// 触发选择器
        /// </summary>
        [Category("相机参数"), Description("触发选择器"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TriggerSelector
        {
            get { return camera.Parameters[PLCamera.TriggerSelector].GetValue(); }
            set { camera.Parameters[PLCamera.TriggerSelector].TrySetValue(value); }
        }
        /// <summary>
        /// 触发源
        /// </summary>
        [Category("相机参数"), Description("触发源"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TriggerSource
        {
            get { return camera.Parameters[PLCamera.TriggerSource].GetValue(); }
            set { camera.Parameters[PLCamera.TriggerSource].TrySetValue(value); }
        }
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        [Category("相机参数"), Description("缓冲区大小"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long PacketSize
        {
            get { return camera.Parameters[PLCamera.GevSCPSPacketSize].GetValue(); }
            set { camera.Parameters[PLCamera.GevSCPSPacketSize].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 数据包延时
        /// </summary>
        [Category("相机参数"), Description("数据包延时"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long DataDelay
        {
            get { return camera.Parameters[PLCamera.GevSCPD].GetValue(); }
            set { camera.Parameters[PLCamera.GevSCPD].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        /// <summary>
        /// 传输延时
        /// </summary>
        [Category("相机参数"), Description("传输延时"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long TransferDelay
        {
            get { return camera.Parameters[PLCamera.GevSCFTD].GetValue(); }
            set { camera.Parameters[PLCamera.GevSCFTD].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 获取图像时事件
        /// </summary>
        public event EventHandler<ImageGrabbedEventArgs> ImageGrabbed;
        /// <summary>
        /// 采集停止时事件
        /// </summary>
        public event EventHandler<GrabStopEventArgs> GrabStopped;
        /// <summary>
        /// 错误处理事件
        /// </summary>
        public event EventHandler<Exception> Error;

        private void RaiseImageGrabbedEvent(object sender, ImageGrabbedEventArgs e)
        {
            ImageGrabbed?.Invoke(sender, e);
        }

        private void RaiseGrabStoppedEvent(object sender, GrabStopEventArgs e)
        {
            GrabStopped?.Invoke(sender, e);
        }

        private void RaiseErrorEvent(Exception e)
        {
            Error?.Invoke(this, e);
        }
        #endregion

        private Camera camera;

        #region 构造函数
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public BaslerCam() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device">相机名</param>
        /// <param name="index">相机编号</param>
        public BaslerCam(string device, int index = 0)
        {
            DeviceName = device;
            Index = index;
        }

        internal void SetCamera(Camera camera)
        {
            this.camera = camera;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 从文件读取并设置相机参数
        /// </summary>
        /// <param name="deviceStateFile">文件地址</param>
        /// <returns>是否成功读取</returns>
        public bool LoadCameraParams(string deviceStateFile)
        {
            XDocument cameraInfo = new XDocument();
            if (!File.Exists(deviceStateFile))
                return false;

            try
            {
                //camera.Parameters.Load(deviceStateFile, ParameterPath.CameraDevice);
                cameraInfo = XDocument.Load(deviceStateFile);
                CloseCamera();
                camera = new Camera(cameraInfo.Element("Properties").Element("DeviceInfo").Attribute("SerialNumber").Value);
                //camera.CameraOpened += Configuration.AcquireContinuous;
                camera.Open();

                IEnumerable<XElement> items = cameraInfo.Element("Properties").Elements("Item");
                foreach (XElement i in items)
                    camera.Parameters[i.Element("Property").Value].ParseAndSetValue(i.Element("Value").Value);

                camera.StreamGrabber.UserData = DeviceName;
                camera.StreamGrabber.ImageGrabbed += RaiseImageGrabbedEvent;
                camera.StreamGrabber.GrabStopped += RaiseGrabStoppedEvent;
                //CloseCamera();
                return camera.IsConnected;
            }
            catch (Exception e)
            {
                RaiseErrorEvent(e);
                return false;
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        public void LiveStart()
        {
            if (camera == null || camera.StreamGrabber.IsGrabbing)
                return;
            if (camera.IsConnected)
            {
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
        }

        /// <summary>
        /// 获取单帧
        /// </summary>
        public void SnapShot()
        {
            if (camera.IsConnected)
            {
                string triggerSource = TriggerSource;
                TriggerSource = PLCamera.TriggerSource.Software;
                camera.WaitForFrameTriggerReady(100, TimeoutHandling.Return);
                camera.ExecuteSoftwareTrigger();
                TriggerSource = triggerSource;
            }
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void LiveStop()
        {
            if (camera == null)
                return;
            if (camera.IsConnected && camera.StreamGrabber.IsGrabbing)
                camera.StreamGrabber.Stop();
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public void CloseCamera()
        {
            if (camera != null)
            {
                camera.StreamGrabber.Stop();
                camera.Close();
                camera.Dispose();
                camera = null;
            }
        }

        /// <summary>
        /// 获取指定信息
        /// </summary>
        /// <param name="key">信息名</param>
        /// <returns>信息值</returns>
        public string GetCameraInfo(string key)
        {
            return camera.CameraInfo[key];
        }

        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <param name="property">属性名</param>
        /// <returns>属性值，需要强制转换，对应不同种类属性分别为bool,ICommandParameter,string(enum),double,long,string</returns>
        public object GetCameraParam(object property)
        {
            var vals = new Dictionary<Type, Func<object>> {
                {typeof(BooleanName), () => camera.Parameters[(BooleanName)property].GetValue() },
                {typeof(CommandName), () => camera.Parameters[(CommandName)property] },
                {typeof(EnumName), () => camera.Parameters[(EnumName)property].GetValue() },
                {typeof(FloatName), () => camera.Parameters[(FloatName)property].GetValue() },
                {typeof(IntegerName), () => camera.Parameters[(IntegerName)property].GetValue() },
                {typeof(StringName), () => camera.Parameters[(StringName)property].GetValue() },
                {typeof(string), () => ((IEnumParameter)camera.Parameters[property.ToString()]).GetValue() }
            };
            if (vals.ContainsKey(property.GetType()))
                return vals[property.GetType()]();
            else
                return vals[typeof(string)]();
        }

        /// <summary>
        /// 获取Enum类型属性的可设定值
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="vals">可设定值</param>
        public void GetCameraParamValues(EnumName property, ref IList<string> vals)
        {
            var temp = camera.Parameters[property].GetAllValues();
            vals.Clear();
            foreach (var t in temp)
            {
                //if (camera.Parameters[property].CanSetValue(t))
                vals.Add(t);
            }
        }

        /// <summary>
        /// 获取Float类型属性的可设定范围
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="increment">增长值</param>
        public void GetCameraParamValues(FloatName property, ref double min, ref double max, ref double increment)
        {
            min = camera.Parameters[property].GetMinimum();
            max = camera.Parameters[property].GetMaximum();
            var temp = camera.Parameters[property].GetIncrement();
            if (temp != null)
                increment = (double)temp;
            else
                increment = 0;
        }

        /// <summary>
        /// 获取Int类型属性的可设定范围
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="increment">增长值</param>
        public void GetCameraParamValues(IntegerName property, ref long min, ref long max, ref long increment)
        {
            min = camera.Parameters[property].GetMinimum();
            max = camera.Parameters[property].GetMaximum();
            increment = camera.Parameters[property].GetIncrement();
        }

        /// <summary>
        /// 获取String类型属性的最大可设定长度
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="maxLen">最大字符长度</param>
        public void GetCameraParamValues(StringName property, ref int maxLen)
        {
            maxLen = camera.Parameters[property].GetMaxLength();
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>是否成功设置</returns>
        public bool SetCameraParam(object property, object value)
        {
            var actions = new Dictionary<Type, Func<bool>> {
                {typeof(BooleanName), () => camera.Parameters[(BooleanName)property].TrySetValue((bool)value) },
                {typeof(CommandName), () => camera.Parameters[(CommandName)property].TryExecute() },
                {typeof(EnumName), () => camera.Parameters[(EnumName)property].TrySetValue((string[])value) },
                {typeof(FloatName), () => camera.Parameters[(FloatName)property].TrySetValue((double)value, FloatValueCorrection.ClipToRange) },
                {typeof(IntegerName), () => camera.Parameters[(IntegerName)property].TrySetValue((long)value, IntegerValueCorrection.Nearest) },
                {typeof(StringName), () => camera.Parameters[(StringName)property].TrySetValue((string)value) },
                {typeof(string), () => ((IEnumParameter)camera.Parameters[property.ToString()]).TrySetValue((string[])value) }
            };
            if (actions.ContainsKey(property.GetType()))
                return actions[property.GetType()]();
            else
                return actions[typeof(string)]();
        }

        /// <summary>
        /// 储存相机参数到文件
        /// </summary>
        /// <param name="deviceStateFile">文件地址</param>
        /// <returns>是否成功储存</returns>
        public bool SaveCameraParams(string deviceStateFile)
        {
            //camera.Parameters.Save(deviceStateFile, ParameterPath.CameraDevice);

            try
            {
                XDocument cameraInfo = new XDocument();
                cameraInfo.Add(new XComment("DeviceState"));
                XElement root = new XElement("Properties");

                List<object> parameters = new List<object>() {
                    PLCamera.AcquisitionFrameRateEnable,
                    PLCamera.AcquisitionFrameRateAbs,
                    PLCamera.PixelFormat,
                    PLCamera.Width,
                    PLCamera.Height,
                    PLCamera.ExposureAuto,
                    PLCamera.ExposureTimeRaw,
                    PLCamera.GainAuto,
                    PLCamera.GainRaw,
                    PLCamera.GammaEnable,
                    PLCamera.Gamma,
                    PLCamera.TriggerSelector,
                    PLCamera.TriggerMode,
                    PLCamera.TriggerSource,
                    PLCamera.TriggerActivation,
                    PLCamera.TriggerDelayAbs,
                    PLCamera.GevSCPSPacketSize,
                    PLCamera.GevSCPD,
                    PLCamera.GevSCFTD,
                };
                if (SharpnessAvailable)
                    parameters.AddRange(new object[] { PLCamera.DemosaicingMode, PLCamera.SharpnessEnhancementRaw });
                foreach (object p in parameters)
                    root.Add(ParamItem(p));
                root.Add(new XElement("DeviceInfo", new XAttribute("Name", camera.CameraInfo[CameraInfoKey.FullName].ToString()),
                        new XAttribute("CustomName", camera.CameraInfo[CameraInfoKey.FriendlyName].ToString()),
                        new XAttribute("SerialNumber", camera.CameraInfo[CameraInfoKey.SerialNumber].ToString())));
                cameraInfo.Add(root);

                cameraInfo.Save(deviceStateFile);
                return true;
            }
            catch (Exception e)
            {
                RaiseErrorEvent(e);
                return false;
            }
        }
        #endregion

        private XElement ParamItem(object property)
        {
            string p, v;
            if (property is BooleanName)
            {
                p = camera.Parameters[(BooleanName)property].FullName;
                v = camera.Parameters[(BooleanName)property].GetValue().ToString().ToLower();
            }
            else if (property is CommandName)
            {
                p = camera.Parameters[(CommandName)property].FullName;
                v = camera.Parameters[(CommandName)property].ToString();
            }
            else if (property is FloatName)
            {
                p = camera.Parameters[(FloatName)property].FullName;
                v = camera.Parameters[(FloatName)property].GetValue().ToString();
            }
            else if (property is IntegerName)
            {
                p = camera.Parameters[(IntegerName)property].FullName;
                v = camera.Parameters[(IntegerName)property].GetValue().ToString();
            }
            else if (property is StringName)
            {
                p = camera.Parameters[(StringName)property].FullName;
                v = camera.Parameters[(StringName)property].GetValue();
            }
            else
            {
                try
                {
                    p = ((IEnumParameter)camera.Parameters[property.ToString()]).FullName;
                    v = ((IEnumParameter)camera.Parameters[property.ToString()]).GetValue();
                }
                catch
                {
                    return null;
                }
            }

            XElement item = new XElement("Item");
            XElement prop = new XElement("Property", p);
            XElement val = new XElement("Value", v);
            item.Add(prop);
            item.Add(val);
            return item;
        }
    }

    /// <summary>
    /// Basler相机辅助类扩展
    /// </summary>
    public static class BaslerCamExtends
    {
        /// <summary>
        /// 获取当前连接的所有相机名字
        /// </summary>
        /// <param name="serial">序列号</param>
        /// <param name="display">显示名</param>
        public static void GetConnectedCameras(ref IList<string> serial, ref IList<string> display)
        {
            serial.Clear();
            display.Clear();
            foreach (var c in CameraFinder.Enumerate())
            {
                string s = c[CameraInfoKey.SerialNumber];
                string n = c[CameraInfoKey.FriendlyName];
                serial.Add(s);
                display.Add(n + "(" + s + ")");
            }
        }

        /// <summary>
        /// 获取当前连接的所有相机名字并返回所选择的相机的索引
        /// </summary>
        /// <param name="bc">所选择的相机</param>
        /// <param name="serial">序列号</param>
        /// <param name="display">显示名</param>
        /// <returns>索引</returns>
        public static int GetConnectedCameras(this BaslerCam bc, ref IList<string> serial, ref IList<string> display)
        {
            GetConnectedCameras(ref serial, ref display);
            if (!bc.IsConnected)
                return -1;
            else
                return serial.IndexOf(bc.SerialNumber);
        }
    }
}
