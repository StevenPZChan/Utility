using System.Collections.Generic;

using Basler.Pylon;

namespace Utility.Cameras
{
    /// <summary>
    /// Basler相机辅助类扩展
    /// </summary>
    public static class BaslerCamExtends
    {
        #region Methods
        /// <summary>
        /// 获取当前连接的所有相机名字
        /// </summary>
        /// <param name="serial">序列号</param>
        /// <param name="display">显示名</param>
        public static void GetConnectedCameras(ref IList<string> serial, ref IList<string> display)
        {
            serial.Clear();
            display.Clear();
            foreach (ICameraInfo c in CameraFinder.Enumerate())
            {
                string s = c[CameraInfoKey.SerialNumber];
                string n = c[CameraInfoKey.FriendlyName];
                serial.Add(s);
                display.Add($"{n}({s})");
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

            return serial.IndexOf(bc.SerialNumber);
        }
        #endregion
    }
}
