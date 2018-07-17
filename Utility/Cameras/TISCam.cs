using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Linq;

using TIS.Imaging;
using TIS.Imaging.VCDHelpers;

namespace Utility.Cameras
{
    /// <summary>
    /// The Imaging Source相机辅助类
    /// </summary>
    public class TISCam : ICImagingControl
    {
        #region Fields
        private VCDButtonProperty _softTrigger;
        private VCDSimpleProperty _vcdProp;
        #endregion

        #region Constructors
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TISCam() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="device">相机自定义名</param>
        /// <param name="index">相机自定义序号</param>
        public TISCam(string device, int index = 0)
        {
            this.DeviceName = device;
            this.Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 亮度
        /// </summary>
        [Category("相机参数"), Description("亮度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Brightness
        {
            get
            {
                return ((VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Brightness, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range))
                   .Value;
            }
            set
            {
                var brightness =
                    (VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Brightness, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                brightness.Value = Math.Min(brightness.RangeMax, Math.Max(brightness.RangeMin, value));
            }
        }

        /// <summary>
        /// 对比度
        /// </summary>
        [Category("相机参数"), Description("对比度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Contrast
        {
            get
            {
                return ((VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Contrast, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                var contrast = (VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Contrast, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                contrast.Value = Math.Min(contrast.RangeMax, Math.Max(contrast.RangeMin, value));
            }
        }

        /// <summary>
        /// 相机帧率
        /// </summary>
        [Category("相机参数"), Description("相机帧率"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new float DeviceFrameRate
        {
            get { return base.DeviceFrameRate; }
            set
            {
                if (this.LiveVideoRunning)
                    base.LiveStop();
                base.DeviceFrameRate = value;
                base.LiveStart();
            }
        }

        /// <summary>
        /// 相机自定义名称，可附加到回调函数
        /// </summary>
        [Browsable(true), Category("相机参数"), Description("相机自定义名称，可附加到回调函数")]
        public string DeviceName
        {
            get { return this.Tag?.ToString() ?? ""; }
            set { this.Tag = value; }
        }

        /// <summary>
        /// 曝光开关
        /// </summary>
        [Category("相机参数"), Description("曝光开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExposureAuto
        {
            get
            {
                return ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure, VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch))
                   .Switch;
            }
            set
            {
                ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure, VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch =
                    value;
            }
        }

        /// <summary>
        /// 曝光时间，s
        /// </summary>
        [Category("相机参数"), Description("曝光时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ExposureValue
        {
            get
            {
                return ((VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure, VCDIDs.VCDElement_Value,
                    VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                var exposure = (VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure, VCDIDs.VCDElement_Value,
                    VCDIDs.VCDInterface_AbsoluteValue);
                exposure.Value = Math.Min(exposure.RangeMax, Math.Max(exposure.RangeMin, value));
            }
        }

        /// <summary>
        /// 增益开关
        /// </summary>
        [Category("相机参数"), Description("增益开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GainAuto
        {
            get
            {
                return ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain, VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain, VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch = value;
            }
        }

        /// <summary>
        /// 增益值
        /// </summary>
        [Category("相机参数"), Description("增益值"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double GainValue
        {
            get
            {
                return ((VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain, VCDIDs.VCDElement_Value,
                    VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                var gain = (VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain, VCDIDs.VCDElement_Value,
                    VCDIDs.VCDInterface_AbsoluteValue);
                gain.Value = Math.Min(gain.RangeMax, Math.Max(gain.RangeMin, value));
            }
        }

        /// <summary>
        /// 灰度系数
        /// </summary>
        [Category("相机参数"), Description("灰度系数"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Gamma
        {
            get
            {
                return ((VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gamma, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                var gamma = (VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gamma, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                gamma.Value = Math.Min(gamma.RangeMax, Math.Max(gamma.RangeMin, value));
            }
        }

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
            get
            {
                string serialNumber;
                this.Devices[0].GetSerialNumber(out serialNumber);

                return serialNumber;
            }
        }

        /// <summary>
        /// 锐度
        /// </summary>
        [Category("相机参数"), Description("锐度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Sharpness
        {
            get
            {
                return ((VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Sharpness, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range))
                   .Value;
            }
            set
            {
                var sharpness = (VCDRangeProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Sharpness, VCDIDs.VCDElement_Value,
                    VCDIDs.VCDInterface_Range);
                sharpness.Value = Math.Min(sharpness.RangeMax, Math.Max(sharpness.RangeMin, value));
            }
        }

        /// <summary>
        /// 触发去抖动时间，us
        /// </summary>
        [Category("相机参数"), Description("触发去抖动时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TriggerDebounceTime
        {
            get
            {
                return ((VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerDebounceTime,
                    VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                var debounce = (VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerDebounceTime,
                    VCDIDs.VCDInterface_AbsoluteValue);
                debounce.Value = Math.Min(debounce.RangeMax, Math.Max(debounce.RangeMin, value));
            }
        }

        /// <summary>
        /// 触发延迟时间，us
        /// </summary>
        [Category("相机参数"), Description("触发延迟时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TriggerDelay
        {
            get
            {
                return ((VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerDelay,
                    VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                var delay = (VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerDelay,
                    VCDIDs.VCDInterface_AbsoluteValue);
                delay.Value = Math.Min(delay.RangeMax, Math.Max(delay.RangeMin, value));
            }
        }

        /// <summary>
        /// 硬触发开关
        /// </summary>
        [Category("相机参数"), Description("硬触发开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerEnable
        {
            get
            {
                return ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Switch))
                   .Switch;
            }
            set
            {
                ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Switch)).Switch =
                    value;
            }
        }

        /// <summary>
        /// 触发噪声抑制时间，us
        /// </summary>
        [Category("相机参数"), Description("触发噪声抑制时间"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TriggerNoiseSuppressionTime
        {
            get
            {
                return ((VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerNoiseSuppressionTime,
                    VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                var noise = (VCDAbsoluteValueProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerNoiseSuppressionTime, VCDIDs.VCDInterface_AbsoluteValue);
                noise.Value = Math.Min(noise.RangeMax, Math.Max(noise.RangeMin, value));
            }
        }

        /// <summary>
        /// 硬触发极性
        /// </summary>
        [Category("相机参数"), Description("硬触发极性（是否上升沿）"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool TriggerPolarity
        {
            get
            {
                return ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerPolarity,
                    VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_TriggerPolarity,
                    VCDIDs.VCDInterface_Switch)).Switch = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 获得亮度的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camBright">当前值</param>
        public void GetCamBrightMaxMin(out int camMin, out int camMax, out int camBright)
        {
            camMin = this._vcdProp.RangeMin(VCDIDs.VCDID_Brightness);
            camMax = this._vcdProp.RangeMax(VCDIDs.VCDID_Brightness);
            camBright = this._vcdProp.RangeValue[VCDIDs.VCDID_Brightness];
        }

        /// <summary>
        /// 获取相机已获取图像
        /// </summary>
        /// <returns>已获取图像Bitmap</returns>
        public Bitmap GetCameraBitmap() => this.ImageActiveBuffer.Bitmap;

        /// <summary>
        /// 获取相机信息
        /// </summary>
        /// <param name="camBright">高度</param>
        /// <param name="camGain">增益</param>
        /// <param name="camExpose">曝光</param>
        /// <param name="camFrameRate">帧率</param>
        public void GetCameraParam(out float camBright, out float camGain, out float camExpose, out float camFrameRate)
        {
            camBright = this._vcdProp.RangeValue[VCDIDs.VCDID_Brightness];
            camExpose = this._vcdProp.RangeValue[VCDIDs.VCDID_Exposure];
            camGain = this._vcdProp.RangeValue[VCDIDs.VCDID_Gain];
            camFrameRate = this.DeviceFrameRate;
        }

        /// <summary>
        /// 获取曝光的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camExpose">当前值</param>
        public void GetCamExposeMaxMin(out double camMin, out double camMax, out double camExpose)
        {
            camMin = this._vcdProp.RangeMin(VCDIDs.VCDID_Exposure);
            camMax = this._vcdProp.RangeMax(VCDIDs.VCDID_Exposure);
            camExpose = this._vcdProp.RangeValue[VCDIDs.VCDID_Exposure];
        }

        /// <summary>
        /// 获得增益的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camGain">当前值</param>
        public void GetCamGainMaxMin(out double camMin, out double camMax, out double camGain)
        {
            camMin = this._vcdProp.RangeMin(VCDIDs.VCDID_Gain);
            camMax = this._vcdProp.RangeMax(VCDIDs.VCDID_Gain);
            camGain = this._vcdProp.RangeValue[VCDIDs.VCDID_Gain];
        }

        /// <summary>
        /// 获取相机已获取图像
        /// </summary>
        /// <returns>已获取图像bytes</returns>
        public byte[] GetImageByteData()
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                this.ImageActiveBuffer.Bitmap.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();

                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                return Encoding.Default.GetBytes(ex.Message);
            }
            finally
            {
                ms?.Close();
            }
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        /// <param name="deviceStateFile">相机参数储存文件路径</param>
        /// <returns>是否成功打开相机</returns>
        public bool InitCam(string deviceStateFile)
        {
            try
            {
                LoadDeviceStateFromFile(deviceStateFile, true);
            }
            catch
            {
                ShowDeviceSettingsDialog();
            }

            if (!this.DeviceValid)
                return this.DeviceValid;

            this._vcdProp = VCDSimpleModule.GetSimplePropertyContainer(this.VCDPropertyItems);
            //软触发
            this._softTrigger =
                (VCDButtonProperty)this.VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode, VCDIDs.VCDElement_SoftwareTrigger, VCDIDs.VCDInterface_Button);
            this.GainAuto = false;
            this.ExposureAuto = false;
            SaveDeviceStateToFile(deviceStateFile);

            return this.DeviceValid;
        }

        /// <summary>
        /// 相机开始采集图像
        /// </summary>
        public new void LiveStart()
        {
            if (this.LiveVideoRunning)
                return;

            if (this.DeviceValid)
                base.LiveStart();
        }

        /// <summary>
        /// 相机暂停采集图像
        /// </summary>
        public new void LiveStop()
        {
            if (this.LiveVideoRunning)
                base.LiveStop();
        }

        /// <summary>
        /// 储存设备设置到文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>是否储存成功</returns>
        public new bool SaveDeviceStateToFile(string filename)
        {
            var cameraInfo = new XDocument();
            var root = new XElement("device_state", new XAttribute("libver", "3.4"), new XAttribute("filemajor", "1"), new XAttribute("fileminor", "0"));
            var deviceInfo = new XElement("device", new XAttribute("name", this.Device), new XAttribute("base_name", this.Devices[0].Name),
                new XAttribute("unique_name", $"{this.Devices[0].Name} {this.SerialNumber}"));
            deviceInfo.Add(new XElement("videoformat", this.VideoFormat));
            deviceInfo.Add(new XElement("fps", this.DeviceFrameRate.ToString("g")));

            var properties = new XElement("vcdpropertyitems");
            var vals = new Dictionary<string, Func<object, string>> {
                [VCDIDs.VCDInterface_AbsoluteValue] = i => ((VCDAbsoluteValueProperty)i).Value.ToString(CultureInfo.InvariantCulture),
                [VCDIDs.VCDInterface_MapStrings] = i => ((VCDMapStringsProperty)i).Value.ToString(),
                [VCDIDs.VCDInterface_Range] = i => ((VCDRangeProperty)i).Value.ToString(),
                [VCDIDs.VCDInterface_Switch] = i => ((VCDSwitchProperty)i).Switch ? "1" : "0"
            };
            foreach (VCDPropertyItem item in this.VCDPropertyItems)
            {
                var itemInfo = new XElement("item", new XAttribute("guid", item.ItemID), new XAttribute("name", item.Name));
                foreach (VCDPropertyElement elem in item.Elements)
                {
                    var e = new XElement("element", new XAttribute("guid", elem.ElementID), new XAttribute("name", elem.Name));
                    foreach (VCDPropertyInterface itf in elem)
                        if (itf.InterfaceID != VCDIDs.VCDInterface_Button)
                            e.Add(new XElement("itf", new XAttribute("guid", itf.InterfaceID), new XAttribute("value", vals[itf.InterfaceID](itf))));
                    itemInfo.Add(e);
                }

                properties.Add(itemInfo);
            }

            deviceInfo.Add(properties);
            root.Add(deviceInfo);
            cameraInfo.Add(root);

            cameraInfo.Save(filename);

            return true;
        }

        /// <summary>
        /// 打开属性对话框并保存设置到文件
        /// </summary>
        /// <param name="deviceStateFile">文件路径</param>
        public void SetCamParadlg(string deviceStateFile)
        {
            ShowPropertyDialog();
            SaveDeviceStateToFile(deviceStateFile);
        }

        /// <summary>
        /// 相机单帧，软触发
        /// </summary>
        public void SnapShot() => this._softTrigger.Push();
        #endregion
    }
}
