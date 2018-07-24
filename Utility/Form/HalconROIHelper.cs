using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using HalconDotNet;

namespace Utility.Form
{
    /// <summary>
    /// Halcon画ROI辅助工具，适应选择按钮和显示选项，控件的Click事件会在每次单个按钮按下后触发
    /// </summary>
    [ToolboxBitmap(typeof(HalconROIHelper), "hdevelop_icon.ico")]
    public partial class HalconROIHelper : UserControl
    {
        #region ShapeTypes Enum
        /// <summary>
        /// 区域按钮枚举
        /// </summary>
        [Editor(typeof(ShapeTypesEditor), typeof(UITypeEditor)), Flags]
        public enum ShapeTypes
        {
            /// <summary>
            /// 不包含按钮
            /// </summary>
            None = 0,
            /// <summary>
            /// 只有绘制圆形
            /// </summary>
            Circle = 1,
            /// <summary>
            /// 只有绘制椭圆
            /// </summary>
            Ellipse = 2,
            /// <summary>
            /// 只有绘制轴平行矩形
            /// </summary>
            Rectangle1 = 4,
            /// <summary>
            /// 只有绘制旋转矩形
            /// </summary>
            Rectangle2 = 8,
            /// <summary>
            /// 只有减去圆形
            /// </summary>
            DCircle = 16,
            /// <summary>
            /// 只有减去椭圆
            /// </summary>
            DEllipse = 32,
            /// <summary>
            /// 只有减去轴平行矩形
            /// </summary>
            DRectangle1 = 64,
            /// <summary>
            /// 只有减去旋转矩形
            /// </summary>
            DRectangle2 = 128,
            /// <summary>
            /// 包含全部按钮
            /// </summary>
            All = 255
        }
        #endregion

        #region Fields
        private readonly DataRow linedata;
        private readonly DataTable regiondata;
        private bool line;
        private ShapeTypes region;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public HalconROIHelper()
        {
            InitializeComponent();
            this.LineButton = true;
            this.RegionButton = ShapeTypes.All;
            this.DispAll = false;
            this.DispLine = true;
            this.DispRegion = true;
            this.HwControl = null;
            this.LineColor = "white";
            this.RegionColor = "green";
            this.AddColor = "red";
            this.SubColor = "blue";

            var dt = new DataTable();
            dt.Columns.Add("Row1", typeof(double));
            dt.Columns.Add("Column1", typeof(double));
            dt.Columns.Add("Row2", typeof(double));
            dt.Columns.Add("Column2", typeof(double));
            this.linedata = dt.NewRow();
            this.regiondata = new DataTable();
            this.regiondata.Columns.Add("Row", typeof(double));
            this.regiondata.Columns.Add("Column", typeof(double));
            this.regiondata.Columns.Add("Phi", typeof(double));
            this.regiondata.Columns.Add("Ra", typeof(double));
            this.regiondata.Columns.Add("Rb", typeof(double));
            this.regiondata.Columns.Add("Type", typeof(string));
            this.regiondata.Columns.Add("ToAdd", typeof(bool));
        }
        #endregion

        #region Properties
        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("red"), Description("设置增加区域显示颜色")]
        public string AddColor { get; set; }
        /// <summary>
        /// 是否显示绘制的所有图形
        /// </summary>
        [Category("显示"), DefaultValue(false), Description("是否显示绘制的所有图形")]
        public bool DispAll { get; set; }
        /// <summary>
        /// 是否显示绘制的线段
        /// </summary>
        [Category("显示"), DefaultValue(true), Description("是否显示绘制的线段")]
        public bool DispLine { get; set; }
        /// <summary>
        /// 是否显示绘制的最终区域
        /// </summary>
        [Category("显示"), DefaultValue(true), Description("是否显示绘制的最终区域")]
        public bool DispRegion { get; set; }
        /// <summary>
        /// 设置绘制和显示的窗口
        /// </summary>
        public HWindowControl HwControl { get; set; }

        /// <summary>
        /// 是否包含画线按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(true), Description("是否包含画线按钮")]
        public bool LineButton
        {
            get { return this.line; }
            set
            {
                if (this.line == value)
                    return;

                this.line = value;
                OnPaint();
            }
        }

        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("white"), Description("设置线段显示颜色")]
        public string LineColor { get; set; }

        /// <summary>
        /// 包含的区域按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(ShapeTypes.All), Description("包含的区域按钮"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ShapeTypes RegionButton
        {
            get { return this.region; }
            set
            {
                if (this.region == value)
                    return;

                this.region = value;
                OnPaint();
            }
        }

        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("green"), Description("设置合成区域显示颜色")]
        public string RegionColor { get; set; }
        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("blue"), Description("设置减去区域显示颜色")]
        public string SubColor { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 从DataTable数据获取最终区域
        /// </summary>
        /// <param name="data">DataTable</param>
        /// <returns>最终区域</returns>
        public HRegion GenCompRegion(DataTable data)
        {
            var hRegion = new HRegion();
            hRegion.GenEmptyRegion();
            foreach (DataRow dr in data.Rows)
            {
                HRegion hr, htemp;
                hr = GetRegion(dr);
                if ((bool)dr["ToAdd"])
                    htemp = hRegion | hr;
                else
                    htemp = hRegion / hr;
                hr.Dispose();

                hRegion.Dispose();
                hRegion = htemp.CopyObj(1, -1);
                htemp.Dispose();
            }

            return hRegion;
        }

        /// <summary>
        /// 从DataRow得到线段
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>线段</returns>
        public HXLDCont GetLine(DataRow dr) =>
            new HXLDCont(new HTuple((double)dr["Row1"], (double)dr["Row2"]), new HTuple((double)dr["column1"], (double)dr["column2"]));

        /// <summary>
        /// 获取线段数据
        /// </summary>
        /// <returns>线段数据DataRow(点1行、点1列、点2行、点2列)</returns>
        public DataRow GetLineData() => this.linedata;

        /// <summary>
        /// 从DataRow数据获取区域
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>区域</returns>
        public HRegion GetRegion(DataRow dr)
        {
            var type = dr["Type"] as string;
            var hRegion = new HRegion();
            switch (type)
            {
                case "Ellipse":
                    hRegion.GenEllipse((double)dr["Row"], (double)dr["Column"], (double)dr["Phi"], (double)dr["Ra"], (double)dr["Rb"]);
                    break;
                case "Rectangle":
                    hRegion.GenRectangle2((double)dr["Row"], (double)dr["Column"], (double)dr["Phi"], (double)dr["Ra"], (double)dr["Rb"]);
                    break;
            }

            return hRegion;
        }

        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <returns>区域数据DataTable(中心行、中心列、角度、半长轴、半短轴、椭圆or矩形、加or减)</returns>
        public DataTable GetRegionData() => this.regiondata;

        /// <summary>
        /// 重置已绘制图形
        /// </summary>
        public void Reset() => this.regiondata.Clear();

        /// <summary>
        /// 撤销上一步操作
        /// </summary>
        public void Undo()
        {
            int num = this.regiondata.Rows.Count;
            if (num == 0)
                return;

            this.regiondata.Rows.RemoveAt(num - 1);
            if (!this.DispRegion)
                return;

            HRegion hRegion = GenCompRegion(this.regiondata);
            this.HwControl.HalconWindow.SetColor(this.RegionColor);
            this.HwControl.HalconWindow.DispObj(hRegion);
            hRegion.Dispose();
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            if (this.HwControl != null)
            {
                double row1, column1, row2, column2;
                this.HwControl.HalconWindow.SetColor(this.LineColor);
                this.HwControl.Focus();
                this.HwControl.HalconWindow.DrawLine(out row1, out column1, out row2, out column2);
                this.linedata["Row1"] = row1;
                this.linedata["Column1"] = column1;
                this.linedata["Row2"] = row2;
                this.linedata["Column2"] = column2;

                if (this.DispLine)
                {
                    HXLDCont hLine = GetLine(this.linedata);
                    this.HwControl.HalconWindow.SetColor(this.LineColor);
                    this.HwControl.HalconWindow.DispObj(hLine);
                    hLine.Dispose();
                }
            }

            OnClick(e);
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            if (this.HwControl != null)
            {
                double row, column, phi, ra, rb;
                var btn = (Button)sender;
                this.HwControl.HalconWindow.SetColor(this.RegionColor);
                this.HwControl.Focus();
                if (btn == this.btnDrawCircle)
                {
                    this.HwControl.HalconWindow.DrawCircle(out row, out column, out ra);
                    this.regiondata.Rows.Add(row, column, 0, ra, ra, "Ellipse", true);
                }
                else if (btn == this.btnDrawEllipse)
                {
                    this.HwControl.HalconWindow.DrawEllipse(out row, out column, out phi, out ra, out rb);
                    this.regiondata.Rows.Add(row, column, phi, ra, rb, "Ellipse", true);
                }
                else if (btn == this.btnDrawRec1)
                {
                    double row1, column1, row2, column2;
                    this.HwControl.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
                    this.regiondata.Rows.Add((row1 + row2) / 2, (column1 + column2) / 2, 0, (column2 - column1) / 2, (row2 - row1) / 2, "Rectangle", true);
                }
                else if (btn == this.btnDrawRec2)
                {
                    this.HwControl.HalconWindow.DrawRectangle2(out row, out column, out phi, out ra, out rb);
                    this.regiondata.Rows.Add(row, column, phi, ra, rb, "Rectangle", true);
                }
                else if (btn == this.btnDiffCircle)
                {
                    this.HwControl.HalconWindow.DrawCircle(out row, out column, out ra);
                    this.regiondata.Rows.Add(row, column, 0, ra, ra, "Ellipse", false);
                }
                else if (btn == this.btnDiffEllipse)
                {
                    this.HwControl.HalconWindow.DrawEllipse(out row, out column, out phi, out ra, out rb);
                    this.regiondata.Rows.Add(row, column, phi, ra, rb, "Ellipse", false);
                }
                else if (btn == this.btnDiffRec1)
                {
                    double row1, column1, row2, column2;
                    this.HwControl.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
                    this.regiondata.Rows.Add((row1 + row2) / 2, (column1 + column2) / 2, 0, (column2 - column1) / 2, (row2 - row1) / 2, "Rectangle", false);
                }
                else if (btn == this.btnDiffRec2)
                {
                    this.HwControl.HalconWindow.DrawRectangle2(out row, out column, out phi, out ra, out rb);
                    this.regiondata.Rows.Add(row, column, phi, ra, rb, "Rectangle", false);
                }

                if (this.DispAll)
                {
                    DataRow dr = this.regiondata.Rows[this.regiondata.Rows.Count - 1];
                    if ((bool)dr["ToAdd"])
                        this.HwControl.HalconWindow.SetColor(this.AddColor);
                    else
                        this.HwControl.HalconWindow.SetColor(this.SubColor);
                    HRegion hRegion = GetRegion(dr);
                    this.HwControl.HalconWindow.DispObj(hRegion);
                    hRegion.Dispose();
                }

                if (this.DispRegion)
                {
                    HRegion hRegion = GenCompRegion(this.regiondata);
                    this.HwControl.HalconWindow.SetColor(this.RegionColor);
                    this.HwControl.HalconWindow.DispObj(hRegion);
                    hRegion.Dispose();
                }
            }

            OnClick(e);
        }

        private void OnPaint()
        {
            int num = (this.line ? 1 : 0) + Regex.Matches(Convert.ToString((int)this.region, 2), @"1").Count;

            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.Controls.Clear();
            this.tableLayoutPanel1.ColumnCount = num;
            this.tableLayoutPanel1.ColumnStyles.Clear();
            for (var i = 0; i < num; i++)
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / num));
            var current = 0;
            if (this.line)
                this.tableLayoutPanel1.Controls.Add(this.btnLine, current++, 0);
            if ((this.region & ShapeTypes.Circle) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDrawCircle, current++, 0);
            if ((this.region & ShapeTypes.Ellipse) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDrawEllipse, current++, 0);
            if ((this.region & ShapeTypes.Rectangle1) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDrawRec1, current++, 0);
            if ((this.region & ShapeTypes.Rectangle2) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDrawRec2, current++, 0);
            if ((this.region & ShapeTypes.DCircle) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDiffCircle, current++, 0);
            if ((this.region & ShapeTypes.DEllipse) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDiffEllipse, current++, 0);
            if ((this.region & ShapeTypes.DRectangle1) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDiffRec1, current++, 0);
            if ((this.region & ShapeTypes.DRectangle2) > 0)
                this.tableLayoutPanel1.Controls.Add(this.btnDiffRec2, current, 0);
            this.tableLayoutPanel1.ResumeLayout(true);
        }
        #endregion

        #region Nested type: SelectShapes
        private class SelectShapes : UserControl
        {
            #region Fields
            private readonly HalconROIHelper helper;
            private readonly CheckBox[] sub = new CheckBox[8];
            private FlowLayoutPanel flowLayoutPanel1;
            private CheckBox total;
            #endregion

            #region Constructors
            public SelectShapes(HalconROIHelper helper)
            {
                InitializeComponent();
                this.helper = helper;
                Load += (sender, e) => this.ShapeTypes = helper.RegionButton;
                Leave += (sender, e) => {
                    var r = 0;
                    for (var i = 0; i < 8; i++)
                        r += this.sub[i].Checked ? 1 << i : 0;
                    helper.RegionButton = (ShapeTypes)r;
                };
            }
            #endregion

            #region Properties
            private ShapeTypes ShapeTypes
            {
                set
                {
                    this.total.Checked = value == ShapeTypes.All;
                    for (var i = 0; i < 8; i++)
                        this.sub[i].Checked = (value & (ShapeTypes)(1 << i)) > 0;
                }
            }
            #endregion

            #region Methods
            private void InitializeComponent()
            {
                this.flowLayoutPanel1 = new FlowLayoutPanel {BackColor = SystemColors.Control, Dock = DockStyle.Fill};
                this.Controls.Add(this.flowLayoutPanel1);
                this.Height = 225;

                this.total = new CheckBox {Text = "全部"};
                this.total.CheckedChanged += (sender, e) => {
                    foreach (CheckBox cb in this.sub)
                        cb.Checked = this.total.Checked;
                };
                this.flowLayoutPanel1.Controls.Add(this.total);

                string[] text = {"绘制圆形", "绘制椭圆", "绘制轴平行矩形", "绘制旋转矩形", "减去圆形", "减去椭圆", "减去轴平行矩形", "减去旋转矩形"};
                for (var i = 0; i < 8; i++)
                {
                    this.sub[i] = new CheckBox {Text = text[i], Margin = new Padding(24, 0, 0, 0)};
                    this.flowLayoutPanel1.Controls.Add(this.sub[i]);
                }
            }
            #endregion
        }
        #endregion

        #region Nested type: ShapeTypesEditor
        internal class ShapeTypesEditor : UITypeEditor
        {
            #region Methods
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (context?.Instance == null || provider == null)
                    return base.EditValue(context, provider, value);

                var editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService == null)
                    return base.EditValue(context, provider, value);

                var control = (HalconROIHelper)context.Instance;
                editorService.DropDownControl(new SelectShapes(control));
                value = control.RegionButton;
                context.PropertyDescriptor?.SetValue(context.Instance, value);

                return base.EditValue(context, provider, value);
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) =>
                context?.Instance != null ? UITypeEditorEditStyle.DropDown : base.GetEditStyle(context);
            #endregion
        }
        #endregion
    }
}
