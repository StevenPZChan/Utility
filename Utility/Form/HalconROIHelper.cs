using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
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

        /// <summary>
        /// 是否包含画线按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(true), Description("是否包含画线按钮")]
        public bool LineButton
        {
            get { return line; }
            set
            {
                if (line == value)
                    return;
                line = value;
                this.OnPaint();
            }
        }
        /// <summary>
        /// 包含的区域按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(ShapeTypes.All), Description("包含的区域按钮"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ShapeTypes RegionButton
        {
            get { return region; }
            set
            {
                if (region == value)
                    return;
                region = value;
                this.OnPaint();
            }
        }
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
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("white"), Description("设置线段显示颜色")]
        public string LineColor { get; set; }
        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("green"), Description("设置合成区域显示颜色")]
        public string RegionColor { get; set; }
        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("red"), Description("设置增加区域显示颜色")]
        public string AddColor { get; set; }
        /// <summary>
        /// 设置显示颜色，默认：线白、区域绿、红增蓝减
        /// </summary>
        [Category("显示"), DefaultValue("blue"), Description("设置减去区域显示颜色")]
        public string SubColor { get; set; }

        /// <summary>
        /// 设置绘制和显示的窗口
        /// </summary>
        public HWindowControl HWControl { get; set; }

        private bool line;
        private ShapeTypes region;
        private DataRow linedata;
        private DataTable regiondata;
        /// <summary>
        /// 构造函数
        /// </summary>
        public HalconROIHelper()
        {
            InitializeComponent();
            LineButton = true;
            RegionButton = ShapeTypes.All;
            DispAll = false;
            DispLine = true;
            DispRegion = true;
            HWControl = null;
            LineColor = "white";
            RegionColor = "green";
            AddColor = "red";
            SubColor = "blue";

            DataTable dt = new DataTable();
            dt.Columns.Add("Row1", typeof(double));
            dt.Columns.Add("Column1", typeof(double));
            dt.Columns.Add("Row2", typeof(double));
            dt.Columns.Add("Column2", typeof(double));
            linedata = dt.NewRow();
            regiondata = new DataTable();
            regiondata.Columns.Add("Row", typeof(double));
            regiondata.Columns.Add("Column", typeof(double));
            regiondata.Columns.Add("Phi", typeof(double));
            regiondata.Columns.Add("Ra", typeof(double));
            regiondata.Columns.Add("Rb", typeof(double));
            regiondata.Columns.Add("Type", typeof(string));
            regiondata.Columns.Add("ToAdd", typeof(bool));
        }

        /// <summary>
        /// 重置已绘制图形
        /// </summary>
        public void Reset()
        {
            regiondata.Clear();
        }

        /// <summary>
        /// 获取线段数据
        /// </summary>
        /// <returns>线段数据DataRow(点1行、点1列、点2行、点2列)</returns>
        public DataRow GetLineData()
        {
            return linedata;
        }

        /// <summary>
        /// 获取区域数据
        /// </summary>
        /// <returns>区域数据DataTable(中心行、中心列、角度、半长轴、半短轴、椭圆or矩形、加or减)</returns>
        public DataTable GetRegionData()
        {
            return regiondata;
        }

        /// <summary>
        /// 从DataRow得到线段
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>线段</returns>
        public HXLDCont GetLine(DataRow dr)
        {
            return new HXLDCont(new HTuple((double)dr["Row1"], (double)dr["Row2"]), new HTuple((double)dr["column1"], (double)dr["column2"]));
        }
        /// <summary>
        /// 从DataRow数据获取区域
        /// </summary>
        /// <param name="dr">DataRow</param>
        /// <returns>区域</returns>
        public HRegion GetRegion(DataRow dr)
        {
            string type = dr["Type"] as string;
            HRegion hRegion = new HRegion();
            if (type == "Ellipse")
                hRegion.GenEllipse((double)dr["Row"], (double)dr["Column"], (double)dr["Phi"], (double)dr["Ra"], (double)dr["Rb"]);
            else if (type == "Rectangle")
                hRegion.GenRectangle2((double)dr["Row"], (double)dr["Column"], (double)dr["Phi"], (double)dr["Ra"], (double)dr["Rb"]);
            return hRegion;
        }

        /// <summary>
        /// 从DataTable数据获取最终区域
        /// </summary>
        /// <param name="data">DataTable</param>
        /// <returns>最终区域</returns>
        public HRegion GenCompRegion(DataTable data)
        {
            HRegion hRegion = new HRegion();
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
                hRegion = htemp.Clone();
                htemp.Dispose();
            }
            return hRegion;
        }

        /// <summary>
        /// 撤销上一步操作
        /// </summary>
        public void Undo()
        {
            int num = regiondata.Rows.Count;
            if (num == 0)
                return;

            regiondata.Rows.RemoveAt(num - 1);
            if (DispRegion)
            {
                HRegion hRegion = GenCompRegion(regiondata);
                HWControl.HalconWindow.SetColor(RegionColor);
                HWControl.HalconWindow.DispObj(hRegion);
                hRegion.Dispose();
            }
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            if (HWControl != null)
            {
                double row1, column1, row2, column2;
                HWControl.HalconWindow.SetColor(LineColor);
                HWControl.Focus();
                HWControl.HalconWindow.DrawLine(out row1, out column1, out row2, out column2);
                linedata["Row1"] = row1;
                linedata["Column1"] = column1;
                linedata["Row2"] = row2;
                linedata["Column2"] = column2;

                if (DispLine)
                {
                    HXLDCont hLine = GetLine(linedata);
                    HWControl.HalconWindow.SetColor(LineColor);
                    HWControl.HalconWindow.DispObj(hLine);
                    hLine.Dispose();
                }
            }
            this.OnClick(e);
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            if (HWControl != null)
            {
                double row, column, phi, ra, rb;
                Button btn = (Button)sender;
                HWControl.HalconWindow.SetColor(RegionColor);
                HWControl.Focus();
                if (btn == btnDrawCircle)
                {
                    HWControl.HalconWindow.DrawCircle(out row, out column, out ra);
                    regiondata.Rows.Add(row, column, 0, ra, ra, "Ellipse", true);
                }
                else if (btn == btnDrawEllipse)
                {
                    HWControl.HalconWindow.DrawEllipse(out row, out column, out phi, out ra, out rb);
                    regiondata.Rows.Add(row, column, phi, ra, rb, "Ellipse", true);
                }
                else if (btn == btnDrawRec1)
                {
                    double row1, column1, row2, column2;
                    HWControl.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
                    regiondata.Rows.Add((row1 + row2) / 2, (column1 + column2) / 2, 0, (column2 - column1) / 2, (row2 - row1) / 2, "Rectangle", true);
                }
                else if (btn == btnDrawRec2)
                {
                    HWControl.HalconWindow.DrawRectangle2(out row, out column, out phi, out ra, out rb);
                    regiondata.Rows.Add(row, column, phi, ra, rb, "Rectangle", true);
                }
                else if (btn == btnDiffCircle)
                {
                    HWControl.HalconWindow.DrawCircle(out row, out column, out ra);
                    regiondata.Rows.Add(row, column, 0, ra, ra, "Ellipse", false);
                }
                else if (btn == btnDiffEllipse)
                {
                    HWControl.HalconWindow.DrawEllipse(out row, out column, out phi, out ra, out rb);
                    regiondata.Rows.Add(row, column, phi, ra, rb, "Ellipse", false);
                }
                else if (btn == btnDiffRec1)
                {
                    double row1, column1, row2, column2;
                    HWControl.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
                    regiondata.Rows.Add((row1 + row2) / 2, (column1 + column2) / 2, 0, (column2 - column1) / 2, (row2 - row1) / 2, "Rectangle", false);
                }
                else if (btn == btnDiffRec2)
                {
                    HWControl.HalconWindow.DrawRectangle2(out row, out column, out phi, out ra, out rb);
                    regiondata.Rows.Add(row, column, phi, ra, rb, "Rectangle", false);
                }

                if (DispAll)
                {
                    DataRow dr = regiondata.Rows[regiondata.Rows.Count - 1];
                    if ((bool)dr["ToAdd"])
                        HWControl.HalconWindow.SetColor(AddColor);
                    else
                        HWControl.HalconWindow.SetColor(SubColor);
                    HRegion hRegion = GetRegion(dr);
                    HWControl.HalconWindow.DispObj(hRegion);
                    hRegion.Dispose();
                }

                if (DispRegion)
                {
                    HRegion hRegion = GenCompRegion(regiondata);
                    HWControl.HalconWindow.SetColor(RegionColor);
                    HWControl.HalconWindow.DispObj(hRegion);
                    hRegion.Dispose();
                }
            }
            this.OnClick(e);
        }

        private void OnPaint()
        {
            int num = (line ? 1 : 0)
                + System.Text.RegularExpressions.Regex.Matches(Convert.ToString((int)region, 2), @"1").Count;

            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.ColumnCount = num;
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < num; i++)
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / num));
            int current = 0;
            if (line)
                tableLayoutPanel1.Controls.Add(btnLine, current++, 0);
            if ((region & ShapeTypes.Circle) > 0)
                tableLayoutPanel1.Controls.Add(btnDrawCircle, current++, 0);
            if ((region & ShapeTypes.Ellipse) > 0)
                tableLayoutPanel1.Controls.Add(btnDrawEllipse, current++, 0);
            if ((region & ShapeTypes.Rectangle1) > 0)
                tableLayoutPanel1.Controls.Add(btnDrawRec1, current++, 0);
            if ((region & ShapeTypes.Rectangle2) > 0)
                tableLayoutPanel1.Controls.Add(btnDrawRec2, current++, 0);
            if ((region & ShapeTypes.DCircle) > 0)
                tableLayoutPanel1.Controls.Add(btnDiffCircle, current++, 0);
            if ((region & ShapeTypes.DEllipse) > 0)
                tableLayoutPanel1.Controls.Add(btnDiffEllipse, current++, 0);
            if ((region & ShapeTypes.DRectangle1) > 0)
                tableLayoutPanel1.Controls.Add(btnDiffRec1, current++, 0);
            if ((region & ShapeTypes.DRectangle2) > 0)
                tableLayoutPanel1.Controls.Add(btnDiffRec2, current++, 0);
            tableLayoutPanel1.ResumeLayout(true);
        }

        internal class SelectShapes : UserControl
        {
            public ShapeTypes ShapeTypes
            {
                get { return helper.RegionButton; }
                set
                {
                    total.Checked = value == ShapeTypes.All;
                    for (int i = 0; i < 8; i++)
                        sub[i].Checked = (value & (ShapeTypes)(1 << i)) > 0;
                }
            }

            private FlowLayoutPanel flowLayoutPanel1;
            private CheckBox total;
            private CheckBox[] sub = new CheckBox[8];
            private HalconROIHelper helper;
            public SelectShapes(HalconROIHelper helper)
            {
                InitializeComponent();
                this.helper = helper;
                this.Load += (sender, e) => ShapeTypes = helper.RegionButton;
                this.Leave += (sender, e) => {
                    int r = 0;
                    for (int i = 0; i < 8; i++)
                        r += (sub[i].Checked ? 1 << i : 0);
                    helper.RegionButton = (ShapeTypes)r;
                };
            }

            private void InitializeComponent()
            {
                flowLayoutPanel1 = new FlowLayoutPanel();
                flowLayoutPanel1.BackColor = SystemColors.Control;
                flowLayoutPanel1.Dock = DockStyle.Fill;
                this.Controls.Add(flowLayoutPanel1);
                this.Height = 225;

                total = new CheckBox();
                total.Text = "全部";
                total.CheckedChanged += (sender, e) => {
                    foreach (CheckBox cb in sub)
                        cb.Checked = total.Checked;
                };
                flowLayoutPanel1.Controls.Add(total);

                string[] text = new string[] {
                    "绘制圆形", "绘制椭圆", "绘制轴平行矩形", "绘制旋转矩形", "减去圆形", "减去椭圆", "减去轴平行矩形", "减去旋转矩形" };
                for (int i = 0; i < 8; i++)
                {
                    sub[i] = new CheckBox();
                    sub[i].Text = text[i];
                    sub[i].Margin = new Padding(24, 0, 0, 0);
                    flowLayoutPanel1.Controls.Add(sub[i]);
                }
            }
        }

        internal class ShapeTypesEditor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                if (context != null && context.Instance != null)
                    return UITypeEditorEditStyle.DropDown;
                return base.GetEditStyle(context);
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService editorService = null;
                if (context != null && context.Instance != null && provider != null)
                {
                    editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    if (editorService != null)
                    {
                        HalconROIHelper control = (HalconROIHelper)context.Instance;
                        editorService.DropDownControl(new SelectShapes(control));
                        value = control.RegionButton;
                        context.PropertyDescriptor.SetValue(context.Instance, value);
                    }
                }
                return base.EditValue(context, provider, value);
            }
        }
    }
}
