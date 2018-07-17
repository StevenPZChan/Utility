using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace Utility.Cameras
{
    /// <summary>
    /// The Imaging Source相机类扩展
    /// </summary>
    public static class TISCamExtends
    {
        #region Methods
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
                bitmap.Save(ms, ImageFormat.Png);
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
        #endregion
    }
}
