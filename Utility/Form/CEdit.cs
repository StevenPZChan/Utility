using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Utility.Form
{
    /// <summary>
    /// 通用编辑块
    /// </summary>
    public partial class CEdit : UserControl
    {
        /// <summary>
        /// 标签的内容
        /// </summary>
        [Category("外观Ex"), Description("标签的内容")]
        public string Label
        {
            get { return cLabel.Text; }
            set { cLabel.Text = value; }
        }
        /// <summary>
        /// 是否含有按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否含有按钮")]
        public bool HasButton
        {
            get { return cButton.Visible; }
            set { cButton.Visible = value; }
        }
        /// <summary>
        /// 按钮文字
        /// </summary>
        [Category("外观Ex"), Description("按钮文字")]
        public string ButtonText
        {
            get { return cButton.Text; }
            set { cButton.Text = value; }
        }

        /// <summary>
        /// 可选控件的容器
        /// </summary>
        protected new TableLayoutPanel Container
        {
            get { return cTableLayout2; }
        }
        /// <summary>
        /// 是否含有边框
        /// </summary>
        protected bool PanelBorder
        {
            get { return cPanelEx.BorderSide == ToolStripStatusLabelBorderSides.All; }
            set { cPanelEx.BorderSide = value ? ToolStripStatusLabelBorderSides.All : ToolStripStatusLabelBorderSides.None; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CEdit()
        {
            InitializeComponent();
            HasButton = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }
    }

    /// <summary>
    /// 勾选框型编辑块
    /// </summary>
    public partial class CCheckBox : CEdit
    {
        /// <summary>
        /// CheckBox属性更改事件
        /// </summary>
        [Description("CheckBox属性更改事件")]
        public event EventHandler CheckedChanged;
        /// <summary>
        /// 是否勾选
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否勾选")]
        public bool Checked
        {
            get { return cbx.Checked; }
            set { cbx.Checked = value; }
        }
        /// <summary>
        /// 勾选框文本内容
        /// </summary>
        [Browsable(true), Category("外观Ex"), Description("勾选框文本内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return cbx.Text; }
            set { cbx.Text = value; }
        }

        private CheckBox cbx;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CCheckBox()
        {
            cbx = new CheckBox();
            Container.Controls.Add(cbx);
            cbx.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            cbx.Name = "cCheckBox";
            cbx.Text = "";
            cbx.CheckedChanged += (sender, e) => CheckedChanged(this, e);
            PanelBorder = false;
        }
    }

    /// <summary>
    /// 下拉框型编辑块
    /// </summary>
    public partial class CComboBox : CEdit
    {
        /// <summary>
        /// 文本内容改变事件
        /// </summary>
        [Browsable(true), Description("文本内容改变事件")]
        public new event EventHandler TextChanged;
        /// <summary>
        /// 设置下拉框的选项
        /// </summary>
        [Category("外观Ex"), Description("设置下拉框的选项")]
        public IList Items
        {
            get { return cbx.Items; }
            set
            {
                object[] items = new object[value.Count];
                value.CopyTo(items, 0);
                cbx.Items.Clear();
                cbx.Items.AddRange(items);
            }
        }
        /// <summary>
        /// 文本框内容
        /// </summary>
        [Browsable(true), Category("外观Ex"), Description("文本框内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return cbx.Text; }
            set { cbx.Text = value; }
        }
        /// <summary>
        /// 下拉框样式
        /// </summary>
        [Category("外观Ex"), DefaultValue(ComboBoxStyle.DropDown), Description("下拉框样式")]
        public ComboBoxStyle DropDownStyle
        {
            get { return cbx.DropDownStyle; }
            set
            {
                cbx.DropDownStyle = value;
                cbx.Anchor = value == ComboBoxStyle.Simple ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Left | AnchorStyles.Right);
            }
        }

        /// <summary>
        /// 下拉框选择序号
        /// </summary>
        public int SelectedIndex
        {
            get { return cbx.SelectedIndex; }
            set { cbx.SelectedIndex = value; }
        }
        /// <summary>
        /// 下拉框选择内容
        /// </summary>
        public string SelectedText
        {
            get { return cbx.SelectedText; }
        }

        private ComboBox cbx;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CComboBox()
        {
            cbx = new ComboBox();
            Container.Controls.Add(cbx);
            cbx.FlatStyle = FlatStyle.Flat;
            cbx.Margin = new Padding(1);
            cbx.Name = "cComboBox";
            cbx.TextChanged += (sender, e) => TextChanged?.Invoke(this, e);
            DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }

    /// <summary>
    /// 数字型编辑块
    /// </summary>
    public partial class CNumericUpDown : CEdit
    {
        /// <summary>
        /// 数字框的值
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("数字框的值")]
        public decimal Value
        {
            get { return nud.Value; }
            set { nud.Value = value; }
        }
        /// <summary>
        /// 设置数字的小数点位数
        /// </summary>
        [Category("外观Ex"), DefaultValue(0), Description("设置数字的小数点位数")]
        public int DecimalPlaces
        {
            get { return nud.DecimalPlaces; }
            set { nud.DecimalPlaces = value; }
        }
        /// <summary>
        /// 设置最小增量值
        /// </summary>
        [Category("外观Ex"), DefaultValue(1), Description("设置最小增量值")]
        public decimal Increment
        {
            get { return nud.Increment; }
            set { nud.Increment = value; }
        }
        /// <summary>
        /// 设置允许的最小值
        /// </summary>
        [Category("外观Ex"), DefaultValue(0), Description("设置允许的最小值")]
        public decimal Minimum
        {
            get { return nud.Minimum; }
            set { nud.Minimum = value; }
        }
        /// <summary>
        /// 设置允许的最大值
        /// </summary>
        [Category("外观Ex"), DefaultValue(100), Description("设置允许的最大值")]
        public decimal Maximum
        {
            get { return nud.Maximum; }
            set { nud.Maximum = value; }
        }

        private NumericUpDown nud;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CNumericUpDown()
        {
            nud = new NumericUpDown();
            Container.Controls.Add(nud);
            nud.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            nud.BorderStyle = BorderStyle.None;
            nud.Name = "cNumeric";
        }
    }

    /// <summary>
    /// 图片型编辑块
    /// </summary>
    public partial class CPictureBox : CEdit
    {
        /// <summary>
        /// 设置前景图
        /// </summary>
        [Category("外观Ex"), Description("设置前景图")]
        public Image Image
        {
            get { return pbx.Image; }
            set { pbx.Image = value; }
        }
        /// <summary>
        /// 设置背景图
        /// </summary>
        [Category("外观Ex"), Description("设置背景图")]
        public Image BackGroundImage
        {
            get { return pbx.BackgroundImage; }
            set { pbx.BackgroundImage = value; }
        }

        private PictureBox pbx;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CPictureBox()
        {
            pbx = new PictureBox();
            Container.Controls.Add(pbx);
            pbx.BackgroundImageLayout = ImageLayout.Stretch;
            pbx.Dock = DockStyle.Fill;
            pbx.Name = "cPictureBox";
            PanelBorder = false;
        }
    }

    /// <summary>
    /// 文本框型编辑块
    /// </summary>
    public partial class CTextBox : CEdit
    {
        /// <summary>
        /// 文本框内容
        /// </summary>
        [Browsable(true), Category("外观Ex"), Description("文本框内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return tbx.Text; }
            set { tbx.Text = value; }
        }
        /// <summary>
        /// 是否多行
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否多行")]
        public bool Multiline
        {
            get { return tbx.Multiline; }
            set
            {
                tbx.Multiline = value;
                tbx.Anchor = value ? (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right) : (AnchorStyles.Left | AnchorStyles.Right);
            }
        }
        /// <summary>
        /// 是否只读
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否只读")]
        public bool ReadOnly
        {
            get { return tbx.ReadOnly; }
            set { tbx.ReadOnly = value; }
        }
        /// <summary>
        /// 文本对齐方式
        /// </summary>
        [Category("外观Ex"), DefaultValue(HorizontalAlignment.Left), Description("文本对齐方式")]
        public HorizontalAlignment TextAlign
        {
            get { return tbx.TextAlign; }
            set { tbx.TextAlign = value; }
        }
        /// <summary>
        /// 文本框背景色
        /// </summary>
        [Category("外观Ex"), Description("文本框背景色")]
        public Color TextBackColor
        {
            get { return tbx.BackColor; }
            set { tbx.BackColor = value; }
        }
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Category("外观Ex"), Description("文本颜色")]
        public Color TextForeColor
        {
            get { return tbx.ForeColor; }
            set { tbx.ForeColor = value; }
        }

        private TextBox tbx;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CTextBox()
        {
            tbx = new TextBox();
            Container.Controls.Add(tbx);
            tbx.BorderStyle = BorderStyle.None;
            tbx.Name = "cTextBox";
            Multiline = false;
        }
    }

    /// <summary>
    /// 自定义型编辑块
    /// </summary>
    [Designer(typeof(UcFoldPanelDesigner))]
    public partial class CEditEx : CEdit
    {
        /// <summary>
        /// 可选控件的容器
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableLayoutPanel Panel
        {
            get { return Container; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CEditEx()
        {
            PanelBorder = false;
        }

        internal class UcFoldPanelDesigner : ControlDesigner
        {
            private CEditEx ucFoldPanelControl;
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);
                ucFoldPanelControl = (CEditEx)component;
                this.EnableDesignMode(ucFoldPanelControl.Panel, "Panel");
            }
        }
    }
}
