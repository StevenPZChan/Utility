#define BASLER
#define TIS

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using HalconDotNet;
#if BASLER
using Basler.Pylon;
#endif
#if TIS
using TIS.Imaging;

#endif

namespace Utility.Cameras
{
    /// <summary>
    /// 相机回调函数生成Halcon对象转换类
    /// </summary>
    public static class HalconCameraConverter
    {
        #region Fields
        private static readonly Action<object, HImage, string> OnImageReceived = RaisedImageReceived;
        #endregion

        #region Events
        /// <summary>
        /// 图像处理函数触发的事件
        /// </summary>
        public static event EventHandler<ImageDataEventArgs> ImageReceived;
        #endregion

        #region Methods
#if TIS
        /// <summary>
        /// The Imaging Source相机回调函数 使用方法： ICImagingControl icImagingControl = new
        /// ICImagingControl(); icImagingControl.MemoryCurrentGrabberColorformat =
        /// ICImagingControlColorformats.IC****; icImagingControl.LiveCaptureContinuous = true;
        /// icImagingControl.ImageAvailable += HalconCameraConverter.ImageAvailable;
        /// HalconCameraConverter.ImageReceived += YourImageProcessFunction; icImagingControl.Tag = deviceName;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            ICImagingControlColorformats colorformat = ((ICImagingControl)sender).MemoryCurrentGrabberColorformat;
            var ho_Image = new HImage();
            switch (colorformat)
            {
                case ICImagingControlColorformats.ICY800:
                    ho_Image = new HImage("byte", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, e.ImageBuffer.GetImageDataPtr());
                    break;

                case ICImagingControlColorformats.ICY8:
                    var ho_ImageIn = new HImage("byte", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, e.ImageBuffer.GetImageDataPtr());
                    ho_Image = ho_ImageIn.MirrorImage("row");
                    ho_ImageIn.Dispose();
                    break;

                case ICImagingControlColorformats.ICRGB24:
                    ho_Image.GenImageInterleaved(e.ImageBuffer.GetImageDataPtr(), "bgr", e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, -1, "byte",
                        e.ImageBuffer.Size.Width, e.ImageBuffer.Size.Height, 0, 0, -1, 0);
                    break;

                case ICImagingControlColorformats.IncompatibleColorformat: break;
                case ICImagingControlColorformats.ICRGB32:                 break;
                case ICImagingControlColorformats.ICRGB565:                break;
                case ICImagingControlColorformats.ICRGB555:                break;
                case ICImagingControlColorformats.ICUYVY:                  break;
                case ICImagingControlColorformats.ICYGB1:                  break;
                case ICImagingControlColorformats.ICYGB0:                  break;
                case ICImagingControlColorformats.ICBY8:                   break;
                case ICImagingControlColorformats.ICY16:                   break;
                case ICImagingControlColorformats.ICRGB64:                 break;
                default:
                    ho_Image.GenEmptyObj();
                    break;
            }

            OnImageReceived.BeginInvoke(sender, ho_Image, ((ICImagingControl)sender).Tag.ToString(), EndingImageReceived, ho_Image);
        }
#endif
#if BASLER
        /// <summary>
        /// Basler相机回调函数 使用方法： Camera camera = new Camera(); camera.CameraOpened +=
        /// Configuration.AcquireContinuous; camera.StreamGrabber.ImageGrabbed +=
        /// HalconCameraConverter.OnImageGrabbed; HalconCameraConverter.ImageReceived +=
        /// YourImageProcessFunction; camera.Open(); camera.StreamGrabber.UserData = deviceName;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnImageGrabbed(object sender, ImageGrabbedEventArgs e)
        {
            IGrabResult grabResult = e.GrabResult;
            if (!grabResult.GrabSucceeded)
            {
                Task.Run(() => { throw new InvalidOperationException($"Balser camera error {grabResult.ErrorCode}: {grabResult.ErrorDescription}"); });
                return;
            }

            HImage ho_Image;
            using (var bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb))
            {
                // Lock the bits of the bitmap.
                BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                // Place the pointer to the buffer of the bitmap.
                var converter = new PixelDataConverter {OutputPixelFormat = PixelType.Mono8};
                IntPtr ptrBmp = bmpData.Scan0;
                converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); //Exception handling TODO
                bitmap.UnlockBits(bmpData);
                ho_Image = new HImage("byte", grabResult.Width, grabResult.Height, ptrBmp);
            }

            OnImageReceived.BeginInvoke(sender, ho_Image, grabResult.StreamGrabberUserData.ToString(), EndingImageReceived, ho_Image);
        }
#endif
        private static void EndingImageReceived(IAsyncResult ar)
        {
            var ho_Image = (HImage)ar.AsyncState;
            if (ho_Image.IsInitialized())
                ho_Image.Dispose();
        }

        private static void RaisedImageReceived(object sender, HImage image, string info) => ImageReceived?.Invoke(sender, new ImageDataEventArgs(image, info));
        #endregion
    }

    /// <inheritdoc />
    /// <summary>
    /// 包含HObject图像对象和相机信息的事件信息类
    /// </summary>
    public class ImageDataEventArgs : EventArgs
    {
        #region Fields
        /// <summary>
        /// 相机信息
        /// </summary>
        public readonly string DeviceInfo;
        /// <summary>
        /// HObject图像对象
        /// </summary>
        public readonly HImage Image;
        #endregion

        #region Constructors
        /// <inheritdoc />
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="image">HObject图像对象</param>
        /// <param name="info">相机信息</param>
        public ImageDataEventArgs(HImage image, string info)
        {
            this.Image = image;
            this.DeviceInfo = info;
        }
        #endregion
    }
}
