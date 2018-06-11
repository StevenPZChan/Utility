#define BASLER
#define TIS
using System;
using HalconDotNet;

namespace Utility.Cameras
{
    /// <summary>
    /// 相机回调函数生成Halcon对象转换类
    /// </summary>
    public static class HalconCameraConverter
    {
        /// <summary>
        /// 图像处理函数触发的事件
        /// </summary>
        public static event EventHandler<ImageDataEventArgs> ImageReceived;
        private static Action<object, HImage, string> OnImageReceived = RaisedImageReceived;
#if BASLER
        /// <summary>
        /// Basler相机回调函数
        /// 使用方法：
        /// Camera camera = new Camera();
        /// camera.CameraOpened += Configuration.AcquireContinuous;
        /// camera.StreamGrabber.ImageGrabbed += HalconCameraConverter.OnImageGrabbed;
        /// HalconCameraConverter.ImageReceived += YourImageProcessFunction;
        /// camera.Open();
        /// camera.StreamGrabber.UserData = deviceName;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnImageGrabbed(object sender, Basler.Pylon.ImageGrabbedEventArgs e)
        {
            Basler.Pylon.IGrabResult grabResult = e.GrabResult;
            HImage ho_Image;
            using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(grabResult.Width, grabResult.Height,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                // Lock the bits of the bitmap.
                System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);
                // Place the pointer to the buffer of the bitmap.
                Basler.Pylon.PixelDataConverter converter = new Basler.Pylon.PixelDataConverter();
                converter.OutputPixelFormat = Basler.Pylon.PixelType.Mono8;
                IntPtr ptrBmp = bmpData.Scan0;
                converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                bitmap.UnlockBits(bmpData);
                ho_Image = new HImage("byte", grabResult.Width, grabResult.Height, ptrBmp);
            }
            OnImageReceived.BeginInvoke(sender, ho_Image, grabResult.StreamGrabberUserData.ToString(), EndingImageReceived, ho_Image);
        }
#endif

#if TIS
        /// <summary>
        /// The Imaging Source相机回调函数
        /// 使用方法：
        /// ICImagingControl icImagingControl = new ICImagingControl();
        /// icImagingControl.MemoryCurrentGrabberColorformat = ICImagingControlColorformats.IC****;
        /// icImagingControl.LiveCaptureContinuous = true;
        /// icImagingControl.ImageAvailable += HalconCameraConverter.ImageAvailable;
        /// HalconCameraConverter.ImageReceived += YourImageProcessFunction;
        /// icImagingControl.Tag = deviceName;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ImageAvailable(object sender, TIS.Imaging.ICImagingControl.ImageAvailableEventArgs e)
        {
            TIS.Imaging.ICImagingControlColorformats colorformat = ((TIS.Imaging.ICImagingControl)sender).MemoryCurrentGrabberColorformat;
            HImage ho_Image = new HImage();
            if (colorformat == TIS.Imaging.ICImagingControlColorformats.ICY800)
                ho_Image = new HImage("byte", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, e.ImageBuffer.GetImageDataPtr());
            else if (colorformat == TIS.Imaging.ICImagingControlColorformats.ICY8)
            {
                HImage ho_ImageIn = new HImage("byte", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, e.ImageBuffer.GetImageDataPtr());
                ho_Image = ho_ImageIn.MirrorImage("row");
                ho_ImageIn.Dispose();
            }
            else if (colorformat == TIS.Imaging.ICImagingControlColorformats.ICRGB24)
                ho_Image.GenImageInterleaved(e.ImageBuffer.GetImageDataPtr(), "bgr", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, -1,
                    "byte", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, 0, 0, -1, 0);
            else
                ho_Image.GenEmptyObj();
            OnImageReceived.BeginInvoke(sender, ho_Image, ((TIS.Imaging.ICImagingControl)sender).Tag.ToString(), EndingImageReceived, ho_Image);
        }
#endif

        private static void RaisedImageReceived(object sender, HImage image, string info)
        {
            ImageReceived?.Invoke(sender, new ImageDataEventArgs(image, info));
        }

        private static void EndingImageReceived(IAsyncResult ar)
        {
            ((HImage)ar.AsyncState).Dispose();
        }
    }

    /// <summary>
    /// 包含HObject图像对象和相机信息的事件信息类
    /// </summary>
    public class ImageDataEventArgs : EventArgs
    {
        /// <summary>
        /// HObject图像对象
        /// </summary>
        public HImage Image;
        /// <summary>
        /// 相机信息
        /// </summary>
        public string DeviceInfo;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="image">HObject图像对象</param>
        /// <param name="info">相机信息</param>
        public ImageDataEventArgs(HImage image, string info)
        {
            Image = image;
            DeviceInfo = info;
        }
    }
}
