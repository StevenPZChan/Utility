using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

using Basler.Pylon;

namespace Utility.Cameras
{
    /// <summary>
    /// Basler相机辅助类
    /// </summary>
    [ToolboxBitmap(typeof(BaslerCam), "Basler.ico")]
    public class BaslerCam : Component
    {
        #region Fields
        private Camera _camera;
        #endregion

        #region Constructors
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
            this.DeviceName = device;
            this.Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 数据包延时
        /// </summary>
        [Category("相机参数"), Description("数据包延时"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long DataDelay
        {
            get { return this._camera.Parameters[PLCamera.GevSCPD].GetValue(); }
            set { this._camera.Parameters[PLCamera.GevSCPD].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 相机帧率
        /// </summary>
        [Category("相机参数"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double DeviceFrameRate
        {
            get { return this._camera.Parameters[PLCamera.AcquisitionFrameRateAbs].GetValue(); }
            set { this._camera.Parameters[PLCamera.AcquisitionFrameRateAbs].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }

        /// <summary>
        /// 相机自定义名称，可附加到回调函数
        /// </summary>
        [Browsable(true), Category("相机参数"), Description("相机自定义名称，可附加到回调函数")]
        public string DeviceName { get; set; }

        /// <summary>
        /// 曝光开关
        /// </summary>
        [Category("相机参数"), Description("曝光开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExposureAuto
        {
            get { return this._camera.Parameters[PLCamera.ExposureAuto].GetValue() == PLCamera.ExposureAuto.Continuous; }
            set { this._camera.Parameters[PLCamera.ExposureAuto].TrySetValue(value ? PLCamera.ExposureAuto.Continuous : PLCamera.ExposureAuto.Off); }
        }

        /// <summary>
        /// 曝光时间，us
        /// </summary>
        [Category("相机参数"), Description("曝光时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ExposureTime
        {
            get { return this._camera.Parameters[PLCamera.ExposureTimeRaw].GetValue(); }
            set { this._camera.Parameters[PLCamera.ExposureTimeRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 相机帧率设置开关
        /// </summary>
        [Category("相机参数"), Description("相机帧率设置开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool FrameRateEnable
        {
            get { return this._camera.Parameters[PLCamera.AcquisitionFrameRateEnable].GetValue(); }
            set { this._camera.Parameters[PLCamera.AcquisitionFrameRateEnable].TrySetValue(value); }
        }

        /// <summary>
        /// 增益开关
        /// </summary>
        [Category("相机参数"), Description("增益开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GainAuto
        {
            get { return this._camera.Parameters[PLCamera.GainAuto].GetValue() == PLCamera.GainAuto.Continuous; }
            set { this._camera.Parameters[PLCamera.GainAuto].TrySetValue(value ? PLCamera.GainAuto.Continuous : PLCamera.GainAuto.Off); }
        }

        /// <summary>
        /// 增益值，0.01dB
        /// </summary>
        [Category("相机参数"), Description("增益值"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long GainValue
        {
            get { return this._camera.Parameters[PLCamera.GainRaw].GetValue(); }
            set { this._camera.Parameters[PLCamera.GainRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 灰度系数
        /// </summary>
        [Category("相机参数"), Description("灰度系数"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Gamma
        {
            get { return this._camera.Parameters[PLCamera.Gamma].GetValue(); }
            set { this._camera.Parameters[PLCamera.Gamma].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }

        /// <summary>
        /// 灰度系数开关
        /// </summary>
        [Category("相机参数"), Description("灰度系数开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GammaEnable
        {
            get { return this._camera.Parameters[PLCamera.GammaEnable].GetValue(); }
            set { this._camera.Parameters[PLCamera.GammaEnable].TrySetValue(value); }
        }

        /// <summary>
        /// 图像高
        /// </summary>
        [Category("相机参数"), Description("图像高"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ImageHeight
        {
            get { return this._camera.Parameters[PLCamera.Height].GetValue(); }
            set { this._camera.Parameters[PLCamera.Height].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 图像宽
        /// </summary>
        [Category("相机参数"), Description("图像宽"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long ImageWidth
        {
            get { return this._camera.Parameters[PLCamera.Width].GetValue(); }
            set { this._camera.Parameters[PLCamera.Width].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 相机编号
        /// </summary>
        [Category("相机参数"), Description("相机编号")]
        public int Index { get; set; }

        /// <summary>
        /// 缓冲区大小
        /// </summary>
        [Category("相机参数"), Description("缓冲区大小"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long PacketSize
        {
            get { return this._camera.Parameters[PLCamera.GevSCPSPacketSize].GetValue(); }
            set { this._camera.Parameters[PLCamera.GevSCPSPacketSize].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 图像格式
        /// </summary>
        [Category("相机参数"), Description("图像格式"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PixelFormat
        {
            get { return this._camera.Parameters[PLCamera.PixelFormat].GetValue(); }
            set { this._camera.Parameters[PLCamera.PixelFormat].TrySetValue(value); }
        }

        /// <summary>
        /// 锐度
        /// </summary>
        [Category("相机参数"), Description("锐度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long Sharpness
        {
            get { return this._camera.Parameters[PLCamera.SharpnessEnhancementRaw].GetValue(); }
            set { this._camera.Parameters[PLCamera.SharpnessEnhancementRaw].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 锐度开关
        /// </summary>
        [Category("相机参数"), Description("锐度开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool SharpnessEnable
        {
            get { return this._camera.Parameters[PLCamera.DemosaicingMode].GetValue() == PLCamera.DemosaicingMode.BaslerPGI; }
            set { this._camera.Parameters[PLCamera.DemosaicingMode].TrySetValue(value ? PLCamera.DemosaicingMode.BaslerPGI : PLCamera.DemosaicingMode.Simple); }
        }

        /// <summary>
        /// 传输延时
        /// </summary>
        [Category("相机参数"), Description("传输延时"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public long TransferDelay
        {
            get { return this._camera.Parameters[PLCamera.GevSCFTD].GetValue(); }
            set { this._camera.Parameters[PLCamera.GevSCFTD].TrySetValue(value, IntegerValueCorrection.Nearest); }
        }

        /// <summary>
        /// 触发延迟时间，us
        /// </summary>
        [Category("相机参数"), Description("触发延迟时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TriggerDelay
        {
            get { return this._camera.Parameters[PLCamera.TriggerDelayAbs].GetValue(); }
            set { this._camera.Parameters[PLCamera.TriggerDelayAbs].TrySetValue(value, FloatValueCorrection.ClipToRange); }
        }

        /// <summary>
        /// 硬触发开关
        /// </summary>
        [Category("相机参数"), Description("硬触发开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerEnable
        {
            get { return this._camera.Parameters[PLCamera.TriggerMode].GetValue() == PLCamera.TriggerMode.On; }
            set { this._camera.Parameters[PLCamera.TriggerMode].TrySetValue(value ? PLCamera.TriggerMode.On : PLCamera.TriggerMode.Off); }
        }

        /// <summary>
        /// 硬触发极性
        /// </summary>
        [Category("相机参数"), Description("硬触发极性（是否上升沿）"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerPolarity
        {
            get { return this._camera.Parameters[PLCamera.TriggerActivation].GetValue() == PLCamera.TriggerActivation.RisingEdge; }
            set
            {
                this._camera.Parameters[PLCamera.TriggerActivation]
                   .TrySetValue(value ? PLCamera.TriggerActivation.RisingEdge : PLCamera.TriggerActivation.FallingEdge);
            }
        }

        /// <summary>
        /// 触发选择器
        /// </summary>
        [Category("相机参数"), Description("触发选择器"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TriggerSelector
        {
            get { return this._camera.Parameters[PLCamera.TriggerSelector].GetValue(); }
            set { this._camera.Parameters[PLCamera.TriggerSelector].TrySetValue(value); }
        }

        /// <summary>
        /// 触发源
        /// </summary>
        [Category("相机参数"), Description("触发源"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TriggerSource
        {
            get { return this._camera.Parameters[PLCamera.TriggerSource].GetValue(); }
            set { this._camera.Parameters[PLCamera.TriggerSource].TrySetValue(value); }
        }

        /// <summary>
        /// 相机连接状态
        /// </summary>
        public bool IsConnected => this._camera?.IsConnected ?? false;
        /// <summary>
        /// 相机采集状态
        /// </summary>
        public bool IsGrabbing => this._camera?.StreamGrabber.IsGrabbing ?? false;
        /// <summary>
        /// 相机序列号
        /// </summary>
        [Category("相机参数"), Description("相机序列号")]
        public string SerialNumber => this._camera.CameraInfo[CameraInfoKey.SerialNumber];
        internal bool SharpnessAvailable => !this._camera.Parameters[PLCamera.DemosaicingMode].IsEmpty;
        #endregion

        #region Events
        /// <summary>
        /// 错误处理事件
        /// </summary>
        public event EventHandler<Exception> Error;

        /// <summary>
        /// 采集停止时事件
        /// </summary>
        public event EventHandler<GrabStopEventArgs> GrabStopped;

        /// <summary>
        /// 获取图像时事件
        /// </summary>
        public event EventHandler<ImageGrabbedEventArgs> ImageGrabbed;
        #endregion

        #region Methods
        /// <summary>
        /// 关闭相机
        /// </summary>
        public void CloseCamera()
        {
            if (this._camera == null)
                return;

            this._camera.StreamGrabber.Stop();
            this._camera.Close();
            this._camera.Dispose();
            this._camera = null;
        }

        /// <summary>
        /// 获取指定信息
        /// </summary>
        /// <param name="key">信息名</param>
        /// <returns>信息值</returns>
        public string GetCameraInfo(string key) => this._camera.CameraInfo[key];

        /// <summary>
        /// 获取指定属性
        /// </summary>
        /// <param name="property">属性名</param>
        /// <returns>属性值，需要强制转换，对应不同种类属性分别为bool,ICommandParameter,string(enum),double,long,string</returns>
        public object GetCameraParam(object property)
        {
            var vals = new Dictionary<Type, Func<object>> {
                [typeof(BooleanName)] = () => this._camera.Parameters[(BooleanName)property].GetValue(),
                [typeof(CommandName)] = () => this._camera.Parameters[(CommandName)property],
                [typeof(EnumName)] = () => this._camera.Parameters[(EnumName)property].GetValue(),
                [typeof(FloatName)] = () => this._camera.Parameters[(FloatName)property].GetValue(),
                [typeof(IntegerName)] = () => this._camera.Parameters[(IntegerName)property].GetValue(),
                [typeof(StringName)] = () => this._camera.Parameters[(StringName)property].GetValue(),
                [typeof(string)] = () => ((IEnumParameter)this._camera.Parameters[property.ToString()]).GetValue()
            };

            return vals.ContainsKey(property.GetType()) ? vals[property.GetType()]() : vals[typeof(string)]();
        }

        /// <summary>
        /// 获取Enum类型属性的可设定值
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="vals">可设定值</param>
        public void GetCameraParamValues(EnumName property, ref IList<string> vals)
        {
            IEnumerable<string> temp = this._camera.Parameters[property].GetAllValues();
            vals.Clear();
            foreach (string t in temp)
                //if (camera.Parameters[property].CanSetValue(t))
                vals.Add(t);
        }

        /// <summary>
        /// 获取Float类型属性的可设定范围
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="increment">增长值</param>
        public void GetCameraParamValues(FloatName property, out double min, out double max, out double increment)
        {
            min = this._camera.Parameters[property].GetMinimum();
            max = this._camera.Parameters[property].GetMaximum();
            double? temp = this._camera.Parameters[property].GetIncrement();
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
        public void GetCameraParamValues(IntegerName property, out long min, out long max, out long increment)
        {
            min = this._camera.Parameters[property].GetMinimum();
            max = this._camera.Parameters[property].GetMaximum();
            increment = this._camera.Parameters[property].GetIncrement();
        }

        /// <summary>
        /// 获取String类型属性的最大可设定长度
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="maxLen">最大字符长度</param>
        public void GetCameraParamValues(StringName property, out int maxLen) => maxLen = this._camera.Parameters[property].GetMaxLength();

        /// <summary>
        /// 开始采集
        /// </summary>
        public void LiveStart()
        {
            if (this._camera == null || this._camera.StreamGrabber.IsGrabbing || !this._camera.IsConnected)
                return;

            this._camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
            this._camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void LiveStop()
        {
            if (this._camera == null)
                return;

            if (this._camera.IsConnected && this._camera.StreamGrabber.IsGrabbing)
                this._camera.StreamGrabber.Stop();
        }

        /// <summary>
        /// 从文件读取并设置相机参数
        /// </summary>
        /// <param name="deviceStateFile">文件地址</param>
        /// <returns>是否成功读取</returns>
        public bool LoadCameraParams(string deviceStateFile)
        {
            if (!File.Exists(deviceStateFile))
                return false;

            try
            {
                //camera.Parameters.Load(deviceStateFile, ParameterPath.CameraDevice);
                XDocument cameraInfo = XDocument.Load(deviceStateFile);
                CloseCamera();
                this._camera = new Camera(cameraInfo.Element("Properties")?.Element("DeviceInfo")?.Attribute("SerialNumber")?.Value);
                //camera.CameraOpened += Configuration.AcquireContinuous;
                this._camera.Open();

                IEnumerable<XElement> items = cameraInfo.Element("Properties")?.Elements("Item");
                if (items != null)
                    foreach (XElement i in items)
                        this._camera.Parameters[i.Element("Property")?.Value].ParseAndSetValue(i.Element("Value")?.Value);

                this._camera.StreamGrabber.UserData = this.DeviceName;
                this._camera.StreamGrabber.ImageGrabbed += RaiseImageGrabbedEvent;
                this._camera.StreamGrabber.GrabStopped += RaiseGrabStoppedEvent;

                //CloseCamera();
                return this._camera.IsConnected;
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(ex);

                return false;
            }
        }

        /// <summary>
        /// 储存相机参数到文件
        /// </summary>
        /// <param name="deviceStateFile">文件地址</param>
        /// <returns>是否成功储存</returns>
        public bool SaveCameraParams(string deviceStateFile)
        {
            //camera.Parameters.Save(deviceStateFile, ParameterPath.CameraDevice);
            if (this._camera == null)
                return false;

            try
            {
                var cameraInfo = new XDocument();
                cameraInfo.Add(new XComment("DeviceState"));
                var root = new XElement("Properties");

                var parameters = new List<object> {
                    PLCamera.AcquisitionFrameRateEnable, PLCamera.AcquisitionFrameRateAbs, PLCamera.PixelFormat, PLCamera.Width, PLCamera.Height,
                    PLCamera.ExposureAuto, PLCamera.ExposureTimeRaw, PLCamera.GainAuto, PLCamera.GainRaw, PLCamera.GammaEnable, PLCamera.Gamma,
                    PLCamera.TriggerSelector, PLCamera.TriggerMode, PLCamera.TriggerSource, PLCamera.TriggerActivation, PLCamera.TriggerDelayAbs,
                    PLCamera.GevSCPSPacketSize, PLCamera.GevSCPD, PLCamera.GevSCFTD
                };
                if (this.SharpnessAvailable)
                    parameters.AddRange(new object[] {PLCamera.DemosaicingMode, PLCamera.SharpnessEnhancementRaw});
                foreach (object p in parameters)
                    root.Add(ParamItem(p));
                root.Add(new XElement("DeviceInfo", new XAttribute("Name", this._camera.CameraInfo[CameraInfoKey.FullName]),
                    new XAttribute("CustomName", this._camera.CameraInfo[CameraInfoKey.FriendlyName]),
                    new XAttribute("SerialNumber", this._camera.CameraInfo[CameraInfoKey.SerialNumber])));
                cameraInfo.Add(root);

                cameraInfo.Save(deviceStateFile);

                return true;
            }
            catch (Exception ex)
            {
                RaiseErrorEvent(ex);

                return false;
            }
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
                [typeof(BooleanName)] = () => this._camera.Parameters[(BooleanName)property].TrySetValue((bool)value),
                [typeof(CommandName)] = () => this._camera.Parameters[(CommandName)property].TryExecute(),
                [typeof(EnumName)] = () => this._camera.Parameters[(EnumName)property].TrySetValue((string[])value),
                [typeof(FloatName)] = () => this._camera.Parameters[(FloatName)property].TrySetValue((double)value, FloatValueCorrection.ClipToRange),
                [typeof(IntegerName)] = () => this._camera.Parameters[(IntegerName)property].TrySetValue((long)value, IntegerValueCorrection.Nearest),
                [typeof(StringName)] = () => this._camera.Parameters[(StringName)property].TrySetValue((string)value),
                [typeof(string)] = () => ((IEnumParameter)this._camera.Parameters[property.ToString()]).TrySetValue((string[])value)
            };

            return actions.ContainsKey(property.GetType()) ? actions[property.GetType()]() : actions[typeof(string)]();
        }

        /// <summary>
        /// 获取单帧
        /// </summary>
        public void SnapShot()
        {
            if (!this._camera.IsConnected)
                return;

            string triggerSource = this.TriggerSource;
            this.TriggerSource = PLCamera.TriggerSource.Software;
            this._camera.WaitForFrameTriggerReady(100, TimeoutHandling.Return);
            this._camera.ExecuteSoftwareTrigger();
            this.TriggerSource = triggerSource;
        }

        internal void SetCamera(Camera camera) => this._camera = camera;

        private XElement ParamItem(object property)
        {
            string p, v;
            if (property is BooleanName)
            {
                p = this._camera.Parameters[(BooleanName)property].FullName;
                v = this._camera.Parameters[(BooleanName)property].GetValue().ToString().ToLower();
            }
            else if (property is CommandName)
            {
                p = this._camera.Parameters[(CommandName)property].FullName;
                v = this._camera.Parameters[(CommandName)property].ToString();
            }
            else if (property is FloatName)
            {
                p = this._camera.Parameters[(FloatName)property].FullName;
                v = this._camera.Parameters[(FloatName)property].GetValue().ToString(CultureInfo.InvariantCulture);
            }
            else if (property is IntegerName)
            {
                p = this._camera.Parameters[(IntegerName)property].FullName;
                v = this._camera.Parameters[(IntegerName)property].GetValue().ToString();
            }
            else if (property is StringName)
            {
                p = this._camera.Parameters[(StringName)property].FullName;
                v = this._camera.Parameters[(StringName)property].GetValue();
            }
            else
                try
                {
                    p = ((IEnumParameter)this._camera.Parameters[property.ToString()]).FullName;
                    v = ((IEnumParameter)this._camera.Parameters[property.ToString()]).GetValue();
                }
                catch
                {
                    return null;
                }

            var item = new XElement("Item");
            var prop = new XElement("Property", p);
            var val = new XElement("Value", v);
            item.Add(prop);
            item.Add(val);

            return item;
        }

        private void RaiseErrorEvent(Exception e) => Error?.Invoke(this, e);

        private void RaiseGrabStoppedEvent(object sender, GrabStopEventArgs e) => GrabStopped?.Invoke(sender, e);

        private void RaiseImageGrabbedEvent(object sender, ImageGrabbedEventArgs e) => ImageGrabbed?.Invoke(sender, e);
        #endregion
    }
}
