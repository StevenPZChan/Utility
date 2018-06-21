using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace Utility.Files
{
    /// <summary>
    /// 图像文件操作类
    /// </summary>
    public static class PicFile
    {
        /// <summary>
        /// 保存截图到文件
        /// </summary>
        /// <param name="c">截图控件对象</param>
        /// <param name="width">图像宽，-1时为保持控件宽</param>
        /// <param name="height">图像高，-1时为保持控件高</param>
        /// <param name="filename">文件名</param>
        /// <param name="quality">图像质量（限tiff和jpg等有损压缩格式）</param>
        public static void SavePic(this Control c, int width, int height, string filename, long quality = 95L)
        {
            width = width == -1 ? c.Width : width;
            height = height == -1 ? c.Height : height;
            using (Bitmap bitsmall = new Bitmap(width, height))
            {
                Graphics newg = Graphics.FromImage(bitsmall);
                newg.InterpolationMode = InterpolationMode.Bicubic;
                newg.SmoothingMode = SmoothingMode.HighQuality;
                Bitmap bit = new Bitmap(c.Width, c.Height);//实例化一个和窗体一样大的bitmap
                Graphics g = Graphics.FromImage(bit);
                g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
                g.CopyFromScreen(c.Left, c.Top, 0, 0, new Size(c.Width, c.Height));//保存整个窗体为图片
                newg.DrawImage(bit, new Rectangle(0, 0, width, height), new Rectangle(0, 0, c.Width, c.Height), GraphicsUnit.Pixel);
                bitsmall.SavePic(filename, quality);
            }
        }

        /// <summary>
        /// 保存位图数据到图像文件
        /// </summary>
        /// <param name="bitmap">位图数据</param>
        /// <param name="filename">文件名</param>
        /// <param name="quality">图像质量（限tiff和jpg等有损压缩格式）</param>
        public static void SavePic(this Bitmap bitmap, string filename, long quality = 95L)
        {
            string f = filename.Substring(0, filename.LastIndexOf(@"\"));
            if (!Directory.Exists(f))
                Directory.CreateDirectory(f);
            string subfix = filename.Substring(filename.LastIndexOf("."));
            EncoderParameters eps = new EncoderParameters(1);
            EncoderParameter ep;
            switch (subfix.ToLower())
            {
                case ".bmp":
                case ".dib":
                case ".rle":
                    bitmap.Save(filename, ImageFormat.Bmp);
                    break;
                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".jfif":
                    ep = new EncoderParameter(Encoder.Quality, quality);//质量等级95%
                    eps.Param[0] = ep;
                    bitmap.Save(filename, ImageCodecInfo.GetImageEncoders()[1], eps);
                    break;
                case ".gif":
                    bitmap.Save(filename, ImageFormat.Gif);
                    break;
                case ".tif":
                case ".tiff":
                    ep = new EncoderParameter(Encoder.Quality, quality);//质量等级95%
                    eps.Param[0] = ep;
                    bitmap.Save(filename, ImageCodecInfo.GetImageEncoders()[3], eps);
                    break;
                case ".png":
                    bitmap.Save(filename, ImageFormat.Png);
                    break;
                default:
                    ep = new EncoderParameter(Encoder.Quality, quality);//质量等级95%
                    eps.Param[0] = ep;
                    bitmap.Save(filename + ".jpg", ImageCodecInfo.GetImageEncoders()[1], eps);
                    break;
            }
        }
    }
}
