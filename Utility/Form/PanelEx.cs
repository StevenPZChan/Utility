using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Utility.Form
{
    /// <summary>
    /// 可设置边框样式的Panel
    /// </summary>
    [ToolboxBitmap(typeof(Panel))]
    public class PanelEx : Panel
    {
        private Color borderColor;
        private Border3DStyle border3DStyle;
        private ToolStripStatusLabelBorderSides borderSide;
        private bool borderIsSingleMode;
        private int cornerRadius;

        /// <summary>
        /// 指定边框是否为单色模式
        /// </summary>
        [Category("外观Ex"), DefaultValue(true), Description("指定边框是否为单色模式。false代表三维模式")]
        public bool BorderIsSingleMode
        {
            get { return borderIsSingleMode; }
            set
            {
                if (borderIsSingleMode == value) { return; }
                borderIsSingleMode = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("外观Ex"), DefaultValue(typeof(Color), "Black"), Description("边框颜色。仅当边框为单色模式时有效")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor == value) { return; }
                borderColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 边框三维样式
        /// </summary>
        [Category("外观Ex"), DefaultValue(Border3DStyle.Etched), Description("边框三维样式。仅当边框为三维模式时有效")]
        public Border3DStyle Border3DStyle
        {
            get { return border3DStyle; }
            set
            {
                if (border3DStyle == value) { return; }
                border3DStyle = value;
                this.Invalidate();
            }
        }

        //之所以不直接用Border3DSide是因为这货不被设计器支持，没法灵活选择位置组合
        /// <summary>
        /// 边框位置
        /// </summary>
        [Category("外观Ex"), DefaultValue(ToolStripStatusLabelBorderSides.None), Description("边框位置。可自由启用各个方位的边框")]
        public ToolStripStatusLabelBorderSides BorderSide
        {
            get { return borderSide; }
            set
            {
                if (borderSide == value) { return; }
                borderSide = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        [Category("外观Ex"), DefaultValue(5), Description("圆角半径。仅当边框全部绘制时有效")]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if (cornerRadius == value) { return; }
                cornerRadius = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelEx()
            : base()
        {
            this.borderIsSingleMode = true;
            this.borderColor = Color.Black;
            this.border3DStyle = Border3DStyle.Etched;
            this.borderSide = ToolStripStatusLabelBorderSides.None;
            this.cornerRadius = 5;
        }

        /// <summary>
        /// Paint事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.BorderStyle != BorderStyle.None
                || BorderSide == ToolStripStatusLabelBorderSides.None)
            { return; }

            using (Graphics g = e.Graphics)
            {
                //单色模式
                if (this.BorderIsSingleMode)
                {
                    using (Pen pen = new Pen(BorderColor))
                    {
                        //若是四条边都启用，则直接画矩形
                        if (BorderSide == ToolStripStatusLabelBorderSides.All)
                        {
                            int radius_max = Math.Min(this.Width / 2, this.Height / 2);
                            int radius = Math.Min(radius_max, cornerRadius);
                            if (radius > 0)
                            {
                                g.DrawArc(pen, 0, 0, 2 * radius, 2 * radius, 180, 90);
                                g.DrawLine(pen, radius, 0, this.Width - radius - 1, 0);
                                g.DrawArc(pen, this.Width - 2 * radius - 1, 0, 2 * radius, 2 * radius, -90, 90);
                                g.DrawLine(pen, this.Width - 1, radius, this.Width - 1, this.Height - radius - 1);
                                g.DrawArc(pen, this.Width - 2 * radius - 1, this.Height - 2 * radius - 1, 2 * radius, 2 * radius, 0, 90);
                                g.DrawLine(pen, radius, this.Height - 1, this.Width - radius - 1, this.Height - 1);
                                g.DrawArc(pen, 0, this.Height - 2 * radius - 1, 2 * radius, 2 * radius, 90, 90);
                                g.DrawLine(pen, 0, radius, 0, this.Height - radius - 1);
                            }
                        }
                        else //否则分别绘制线条
                        {
                            if ((BorderSide & ToolStripStatusLabelBorderSides.Top) == ToolStripStatusLabelBorderSides.Top)
                            {
                                g.DrawLine(pen, 0, 0, this.Width - 1, 0);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Right) == ToolStripStatusLabelBorderSides.Right)
                            {
                                g.DrawLine(pen, this.Width - 1, 0, this.Width - 1, this.Height - 1);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Bottom) == ToolStripStatusLabelBorderSides.Bottom)
                            {
                                g.DrawLine(pen, 0, this.Height - 1, this.Width - 1, this.Height - 1);
                            }

                            if ((BorderSide & ToolStripStatusLabelBorderSides.Left) == ToolStripStatusLabelBorderSides.Left)
                            {
                                g.DrawLine(pen, 0, 0, 0, this.Height - 1);
                            }
                        }
                    }
                }
                else //三维模式
                {
                    ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.Border3DStyle, (Border3DSide)BorderSide); //这儿要将ToolStripStatusLabelBorderSides转换为Border3DSide
                }
            }
        }
    }
}