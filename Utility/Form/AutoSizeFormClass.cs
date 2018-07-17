using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Utility.Form
{
    /// <summary>
    /// 窗体控件自动大小类
    /// </summary>
    public class AutoSizeFormClass
    {
        #region Fields
        //(2).声明 1个对象
        //注意这里不能使用控件列表记录 List nCtrl;，因为控件的关联性，记录的始终是当前的大小。
        //      public List oldCtrl= new List();//这里将西文的大于小于号都过滤掉了，只能改为中文的，使用中要改回西文
        private readonly Dictionary<string, ControlRect> oldCtrl = new Dictionary<string, ControlRect>();
        private float _wScale = 1, _hScale = 1;
        #endregion

        #region Methods
        /// <summary>
        /// (3.2)控件自适应大小,
        /// </summary>
        /// <param name="mForm">控件对象</param>
        public void ControlAutoSize(Control mForm)
        {
            if (this.oldCtrl.Count == 0)
            {
                //*如果在窗体的Form1_Load中，记录控件原始的大小和位置，正常没有问题，但要加入皮肤就会出现问题，因为有些控件如dataGridView的的子控件还没有完成，个数少
                //*要在窗体的Form1_SizeChanged中，第一次改变大小时，记录控件原始的大小和位置,这里所有控件的子控件都已经形成
                this.oldCtrl.Clear();
                var cR = new ControlRect(0, 0, mForm.PreferredSize.Width, mForm.PreferredSize.Height, mForm.Font.Size);
                this.oldCtrl.Add(mForm.GetType() + mForm.Name, cR); //第一个为"窗体本身",只加入一次即可

                AddControl(mForm); //窗体内其余控件可能嵌套其它控件(比如panel),故单独抽出以便递归调用
            }

            this._wScale = mForm.Width / (float)this.oldCtrl[mForm.GetType() + mForm.Name].Width;   //新旧窗体之间的比例，与最早的旧窗体
            this._hScale = mForm.Height / (float)this.oldCtrl[mForm.GetType() + mForm.Name].Height; //.Height;
            AutoScaleControl(mForm, this._wScale, this._hScale);                                    //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
        }

        //(3). 创建两个函数
        /// <summary>
        /// (3.1)记录窗体和其控件的初始位置和大小,
        /// </summary>
        /// <param name="mForm">窗体对象</param>
        public void ControllInitializeSize(Control mForm)
        {
            this.oldCtrl.Clear();
            var cR = new ControlRect(mForm.Left, mForm.Top, mForm.Width, mForm.Height, mForm.Font.Size);
            this.oldCtrl.Add(mForm.GetType() + mForm.Name, cR); //第一个为"窗体本身",只加入一次即可

            AddControl(mForm); //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用

            //this.WindowState = (System.Windows.Forms.FormWindowState)(2);//记录完控件的初始位置和大小后，再最大化
            //0 - Normalize , 1 - Minimize,2- Maximize
        }

        private void AddControl(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            { //**放在这里，是先记录控件的子控件，后记录控件本身
                //if (c.Controls.Count > 0)
                //    AddControl(c);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                var objCtrl = new ControlRect(c.Left, c.Top, c.Width, c.Height, c.Font.Size);
                this.oldCtrl.Add(c.GetType() + c.Name, objCtrl);
                //**放在这里，是先记录控件本身，后记录控件的子控件
                if (c.Controls.Count > 0)
                    AddControl(c); //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            }
        }

        private void AutoScaleControl(Control ctl, float wScale, float hScale)
        {
            //int ctrlNo = 1;//第1个是窗体自身的 Left,Top,Width,Height，所以窗体控件从ctrlNo=1开始
            foreach (Control c in ctl.Controls)
            { //**放在这里，是先缩放控件的子控件，后缩放控件本身
                //if (c.Controls.Count > 0)
                //   AutoScaleControl(c, wScale, hScale);//窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                int ctrLeft0 = this.oldCtrl[c.GetType() + c.Name].Left;
                int ctrTop0 = this.oldCtrl[c.GetType() + c.Name].Top;
                int ctrWidth0 = this.oldCtrl[c.GetType() + c.Name].Width;
                int ctrHeight0 = this.oldCtrl[c.GetType() + c.Name].Height;
                float ctrFont0 = this.oldCtrl[c.GetType() + c.Name].FontSize;
                //c.Left = (int)((ctrLeft0 - wLeft0) * wScale) + wLeft1;//新旧控件之间的线性比例
                //c.Top = (int)((ctrTop0 - wTop0) * h) + wTop1;
                c.Left = (int)(ctrLeft0 * wScale);     //新旧控件之间的线性比例。控件位置只相对于窗体，所以不能加 + wLeft1
                c.Top = (int)(ctrTop0 * hScale);       //
                c.Width = (int)(ctrWidth0 * wScale);   //只与最初的大小相关，所以不能与现在的宽度相乘 (int)(c.Width * w);
                c.Height = (int)(ctrHeight0 * hScale); //
                c.Font = new Font(c.Font.Name, ctrFont0 * Math.Min(wScale, hScale), c.Font.Style, c.Font.Unit);
                //**放在这里，是先缩放控件本身，后缩放控件的子控件
                if (c.Controls.Count > 0)
                    AutoScaleControl(c, wScale, hScale); //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            }
        }
        #endregion

        #region Nested type: ControlRect
        //(1).声明结构,只记录窗体和其控件的初始位置和大小。
        private struct ControlRect
        {
            #region Fields
            public readonly float FontSize;
            public readonly int Height;
            public readonly int Left;
            public readonly int Top;
            public readonly int Width;
            #endregion Fields

            #region Constructors
            public ControlRect(int left, int top, int width, int height, float fontSize)
            {
                this.Left = left;
                this.Top = top;
                this.Width = width;
                this.Height = height;
                this.FontSize = fontSize;
            }
            #endregion Constructors
        }
        #endregion
    }
}
