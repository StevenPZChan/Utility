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
        /// <param name="width">图像宽</param>
        /// <param name="height">图像高</param>
        /// <param name="path">文件名（含路径不含扩展名）</param>
        public static void SavePic(this Control c, int width, int height, string path)
        {
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
                EncoderParameters eps = new EncoderParameters(1);
                EncoderParameter ep = new EncoderParameter(Encoder.Quality, 95L);//质量等级95%
                eps.Param[0] = ep;

                string f = path.Substring(0, path.LastIndexOf(@"\"));
                if (!Directory.Exists(f))
                    Directory.CreateDirectory(f);
                bitsmall.Save(path + ".jpg", ImageCodecInfo.GetImageEncoders()[1], eps);
            }
        }
    }
}
