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
        #region Fields
        private Border3DStyle border3DStyle;
        private Color borderColor;
        private bool borderIsSingleMode;
        private ToolStripStatusLabelBorderSides borderSide;
        private int cornerRadius;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelEx()
        {
            this.borderIsSingleMode = true;
            this.borderColor = Color.Black;
            this.border3DStyle = Border3DStyle.Etched;
            this.borderSide = ToolStripStatusLabelBorderSides.None;
            this.cornerRadius = 5;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 边框三维样式
        /// </summary>
        [Category("外观Ex"), DefaultValue(Border3DStyle.Etched), Description("边框三维样式。仅当边框为三维模式时有效")]
        public Border3DStyle Border3DStyle
        {
            get { return this.border3DStyle; }
            set
            {
                if (this.border3DStyle == value)
                    return;

                this.border3DStyle = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Category("外观Ex"), DefaultValue(typeof(Color), "Black"), Description("边框颜色。仅当边框为单色模式时有效")]
        public Color BorderColor
        {
            get { return this.borderColor; }
            set
            {
                if (this.borderColor == value)
                    return;

                this.borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 指定边框是否为单色模式
        /// </summary>
        [Category("外观Ex"), DefaultValue(true), Description("指定边框是否为单色模式。false代表三维模式")]
        public bool BorderIsSingleMode
        {
            get { return this.borderIsSingleMode; }
            set
            {
                if (this.borderIsSingleMode == value)
                    return;

                this.borderIsSingleMode = value;
                Invalidate();
            }
        }

        //之所以不直接用Border3DSide是因为这货不被设计器支持，没法灵活选择位置组合
        /// <summary>
        /// 边框位置
        /// </summary>
        [Category("外观Ex"), DefaultValue(ToolStripStatusLabelBorderSides.None), Description("边框位置。可自由启用各个方位的边框")]
        public ToolStripStatusLabelBorderSides BorderSide
        {
            get { return this.borderSide; }
            set
            {
                if (this.borderSide == value)
                    return;

                this.borderSide = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 圆角半径
        /// </summary>
        [Category("外观Ex"), DefaultValue(5), Description("圆角半径。仅当边框全部绘制时有效")]
        public int CornerRadius
        {
            get { return this.cornerRadius; }
            set
            {
                if (this.cornerRadius == value)
                    return;

                this.cornerRadius = value;
                Invalidate();
            }
        }
        #endregion

        #region Methods
        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.BorderStyle != BorderStyle.None || this.BorderSide == ToolStripStatusLabelBorderSides.None)
                return;

            using (Graphics g = e.Graphics)
            {
                //单色模式
                if (this.BorderIsSingleMode)
                    using (var pen = new Pen(this.BorderColor))
                    {
                        //若是四条边都启用，则直接画矩形
                        if (this.BorderSide == ToolStripStatusLabelBorderSides.All)
                        {
                            int radiusMax = Math.Min(this.Width / 2, this.Height / 2);
                            int radius = Math.Min(radiusMax, this.cornerRadius);
                            if (radius <= 0)
                                return;

                            g.DrawArc(pen, 0, 0, 2 * radius, 2 * radius, 180, 90);
                            g.DrawLine(pen, radius, 0, this.Width - radius - 1, 0);
                            g.DrawArc(pen, this.Width - 2 * radius - 1, 0, 2 * radius, 2 * radius, -90, 90);
                            g.DrawLine(pen, this.Width - 1, radius, this.Width - 1, this.Height - radius - 1);
                            g.DrawArc(pen, this.Width - 2 * radius - 1, this.Height - 2 * radius - 1, 2 * radius, 2 * radius, 0, 90);
                            g.DrawLine(pen, radius, this.Height - 1, this.Width - radius - 1, this.Height - 1);
                            g.DrawArc(pen, 0, this.Height - 2 * radius - 1, 2 * radius, 2 * radius, 90, 90);
                            g.DrawLine(pen, 0, radius, 0, this.Height - radius - 1);
                        }
                        else //否则分别绘制线条
                        {
                            if ((this.BorderSide & ToolStripStatusLabelBorderSides.Top) == ToolStripStatusLabelBorderSides.Top)
                                g.DrawLine(pen, 0, 0, this.Width - 1, 0);

                            if ((this.BorderSide & ToolStripStatusLabelBorderSides.Right) == ToolStripStatusLabelBorderSides.Right)
                                g.DrawLine(pen, this.Width - 1, 0, this.Width - 1, this.Height - 1);

                            if ((this.BorderSide & ToolStripStatusLabelBorderSides.Bottom) == ToolStripStatusLabelBorderSides.Bottom)
                                g.DrawLine(pen, 0, this.Height - 1, this.Width - 1, this.Height - 1);

                            if ((this.BorderSide & ToolStripStatusLabelBorderSides.Left) == ToolStripStatusLabelBorderSides.Left)
                                g.DrawLine(pen, 0, 0, 0, this.Height - 1);
                        }
                    }
                else //三维模式
                    ControlPaint.DrawBorder3D(g, this.ClientRectangle, this.Border3DStyle, (Border3DSide)this.BorderSide);
            }
        }
        #endregion
    }
}
