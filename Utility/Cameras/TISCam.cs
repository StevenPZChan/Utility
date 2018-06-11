using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
        #region 属性
        /// <summary>
        /// 相机自定义名称，可附加到回调函数
        /// </summary>
        [Browsable(true), Category("相机参数"), Description("相机自定义名称，可附加到回调函数")]
        public string DeviceName
        {
            get { return Tag == null ? "" : Tag.ToString(); }
            set { Tag = value; }
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
                string serialNumber = "";
                Devices[0].GetSerialNumber(out serialNumber);
                return serialNumber;
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
                if (LiveVideoRunning)
                {
                    base.LiveStop();
                }
                base.DeviceFrameRate = value;
                base.LiveStart();
            }
        }
        /// <summary>
        /// 亮度
        /// </summary>
        [Category("相机参数"), Description("亮度"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Brightness
        {
            get
            {
                return ((VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Brightness,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                VCDRangeProperty brightness = (VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Brightness,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
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
                return ((VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Contrast,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                VCDRangeProperty contrast = (VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Contrast,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                contrast.Value = Math.Min(contrast.RangeMax, Math.Max(contrast.RangeMin, value));
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
                return ((VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Sharpness,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                VCDRangeProperty sharpness = (VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Sharpness,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                sharpness.Value = Math.Min(sharpness.RangeMax, Math.Max(sharpness.RangeMin, value));
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
                return ((VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gamma,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range)).Value;
            }
            set
            {
                VCDRangeProperty gamma = (VCDRangeProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gamma,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Range);
                gamma.Value = Math.Min(gamma.RangeMax, Math.Max(gamma.RangeMin, value));
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
                return ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain,
                    VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain,
                    VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch = value;
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
                return ((VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                VCDAbsoluteValueProperty gain = (VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_AbsoluteValue);
                gain.Value = Math.Min(gain.RangeMax, Math.Max(gain.RangeMin, value));
            }
        }
        /// <summary>
        /// 曝光开关
        /// </summary>
        [Category("相机参数"), Description("曝光开关"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool ExposureAuto
        {
            get
            {
                return ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure,
                    VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure,
                    VCDIDs.VCDElement_Auto, VCDIDs.VCDInterface_Switch)).Switch = value;
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
                return ((VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                VCDAbsoluteValueProperty exposure = (VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_AbsoluteValue);
                exposure.Value = Math.Min(exposure.RangeMax, Math.Max(exposure.RangeMin, value));
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
                return ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_Value, VCDIDs.VCDInterface_Switch)).Switch = value;
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
                return ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerPolarity, VCDIDs.VCDInterface_Switch)).Switch;
            }
            set
            {
                ((VCDSwitchProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerPolarity, VCDIDs.VCDInterface_Switch)).Switch = value;
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
                return ((VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerDelay, VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                VCDAbsoluteValueProperty delay = (VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerDelay, VCDIDs.VCDInterface_AbsoluteValue);
                delay.Value = Math.Min(delay.RangeMax, Math.Max(delay.RangeMin, value));
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
                return ((VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerDebounceTime, VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                VCDAbsoluteValueProperty debounce = (VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerDebounceTime, VCDIDs.VCDInterface_AbsoluteValue);
                debounce.Value = Math.Min(debounce.RangeMax, Math.Max(debounce.RangeMin, value));
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
                return ((VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerNoiseSuppressionTime, VCDIDs.VCDInterface_AbsoluteValue)).Value;
            }
            set
            {
                VCDAbsoluteValueProperty noise = (VCDAbsoluteValueProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_TriggerNoiseSuppressionTime, VCDIDs.VCDInterface_AbsoluteValue);
                noise.Value = Math.Min(noise.RangeMax, Math.Max(noise.RangeMin, value));
            }
        }
        #endregion

        private VCDSimpleProperty VCDProp;
        private VCDButtonProperty SoftTrigger;
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
            DeviceName = device;
            Index = index;
        }

        #region 公共方法
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
            catch (Exception)
            {
                ShowDeviceSettingsDialog();
            }
            if (DeviceValid)
            {
                VCDProp = VCDSimpleModule.GetSimplePropertyContainer(VCDPropertyItems);
                //软触发
                SoftTrigger = (VCDButtonProperty)VCDPropertyItems.FindInterface(VCDIDs.VCDID_TriggerMode,
                    VCDIDs.VCDElement_SoftwareTrigger, VCDIDs.VCDInterface_Button);
                GainAuto = false;
                ExposureAuto = false;
                SaveDeviceStateToFile(deviceStateFile);
            }

            return DeviceValid;
        }

        /// <summary>
        /// 相机开始采集图像
        /// </summary>
        public new void LiveStart()
        {
            if (LiveVideoRunning)
                return;
            if (DeviceValid)
                base.LiveStart();
        }

        /// <summary>
        /// 相机暂停采集图像
        /// </summary>
        public new void LiveStop()
        {
            if (LiveVideoRunning)
                base.LiveStop();
        }

        /// <summary>
        /// 相机单帧，软触发
        /// </summary>
        public void SnapShot()
        {
            SoftTrigger.Push();
        }

        /// <summary>
        /// 获取相机已获取图像
        /// </summary>
        /// <returns>已获取图像Bitmap</returns>
        public Bitmap GetCameraBitmap()
        {
            return ImageActiveBuffer.Bitmap;
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
                ImageActiveBuffer.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = new byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                return System.Text.Encoding.Default.GetBytes(ex.Message);
            }
            finally
            {
                ms.Close();
            }
        }

        /// <summary>
        /// 获取相机信息
        /// </summary>
        /// <param name="CamBright">高度</param>
        /// <param name="camGain">增益</param>
        /// <param name="CamExpose">曝光</param>
        /// <param name="camFrameRate">帧率</param>
        public void GetCameraParam(out float CamBright, out float camGain, out float CamExpose, out float camFrameRate)
        {
            CamBright = VCDProp.RangeValue[VCDIDs.VCDID_Brightness];
            CamExpose = VCDProp.RangeValue[VCDIDs.VCDID_Exposure];
            camGain = VCDProp.RangeValue[VCDIDs.VCDID_Gain];
            camFrameRate = DeviceFrameRate;
        }

        /// <summary>
        /// 获得亮度的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camBright">当前值</param>
        public void GetCamBrightMaxMin(out int camMin, out int camMax, out int camBright)
        {
            camMin = VCDProp.RangeMin(VCDIDs.VCDID_Brightness);
            camMax = VCDProp.RangeMax(VCDIDs.VCDID_Brightness);
            camBright = VCDProp.RangeValue[VCDIDs.VCDID_Brightness];
        }

        /// <summary>
        /// 获得增益的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camGain">当前值</param>
        public void GetCamGainMaxMin(out double camMin, out double camMax, out double camGain)
        {
            camMin = VCDProp.RangeMin(VCDIDs.VCDID_Gain);
            camMax = VCDProp.RangeMax(VCDIDs.VCDID_Gain);
            camGain = VCDProp.RangeValue[VCDIDs.VCDID_Gain];
        }

        /// <summary>
        /// 获取曝光的最大最小值
        /// </summary>
        /// <param name="camMin">最小值</param>
        /// <param name="camMax">最大值</param>
        /// <param name="camExpose">当前值</param>
        public void GetCamExposeMaxMin(out double camMin, out double camMax, out double camExpose)
        {
            camMin = VCDProp.RangeMin(VCDIDs.VCDID_Exposure);
            camMax = VCDProp.RangeMax(VCDIDs.VCDID_Exposure);
            camExpose = VCDProp.RangeValue[VCDIDs.VCDID_Exposure];
        }

        /// <summary>
        /// 储存设备设置到文件
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>是否储存成功</returns>
        public new bool SaveDeviceStateToFile(string filename)
        {
            XDocument cameraInfo = new XDocument();
            XElement root = new XElement("device_state", new XAttribute("libver", "3.4"), new XAttribute("filemajor", "1"), new XAttribute("fileminor", "0"));
            string serialNumber = "";
            Devices[0].GetSerialNumber(out serialNumber);
            XElement deviceInfo = new XElement("device", new XAttribute("name", Device),
                new XAttribute("base_name", Devices[0].Name), new XAttribute("unique_name", Devices[0].Name + " " + serialNumber));
            deviceInfo.Add(new XElement("videoformat", VideoFormat));
            deviceInfo.Add(new XElement("fps", DeviceFrameRate.ToString("g")));

            XElement properties = new XElement("vcdpropertyitems");
            var vals = new Dictionary<string, Func<object, string>> {
                {VCDIDs.VCDInterface_AbsoluteValue, (i) => ((VCDAbsoluteValueProperty)i).Value.ToString() },
                {VCDIDs.VCDInterface_MapStrings, (i) => ((VCDMapStringsProperty)i).Value.ToString() },
                {VCDIDs.VCDInterface_Range, (i) => ((VCDRangeProperty)i).Value.ToString() },
                {VCDIDs.VCDInterface_Switch, (i) => ((VCDSwitchProperty)i).Switch ? "1" : "0" }
            };
            foreach (VCDPropertyItem item in VCDPropertyItems)
            {
                XElement itemInfo = new XElement("item", new XAttribute("guid", item.ItemID), new XAttribute("name", item.Name));
                foreach (VCDPropertyElement elem in item.Elements)
                {
                    XElement e = new XElement("element", new XAttribute("guid", elem.ElementID), new XAttribute("name", elem.Name));
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
        #endregion
    }

    /// <summary>
    /// The Imaging Source相机类扩展
    /// </summary>
    public static class TISCamExtends
    {
        /// <summary>
        /// Bitmap图像转化为bytes
        /// </summary>
        /// <param name="bitmap">Bitmap图像</param>
        /// <returns>bytes图像</returns>
        public static byte[] Bitmap2Byte(this Bitmap bitmap)
        {
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteImage = new byte[ms.Length];
                byteImage = ms.ToArray();
                return byteImage;
            }
            catch (ArgumentNullException ex)
            {
                return System.Text.Encoding.Default.GetBytes(ex.Message);
            }
            finally
            {
                ms.Close();
            }
        }
    }
}
