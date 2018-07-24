using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Utility.Form
{
    /// <summary>
    /// 勾选框型编辑块
    /// </summary>
    [ToolboxBitmap(typeof(CheckBox))]
    public class CCheckBox : CEdit
    {
        #region Fields
        private readonly CheckBox cbx;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CCheckBox()
        {
            this.cbx = new CheckBox {Anchor = AnchorStyles.Left | AnchorStyles.Right, Name = "cCheckBox", Text = ""};
            this.Container.Controls.Add(this.cbx);
            this.cbx.CheckedChanged += (sender, e) => CheckedChanged?.Invoke(this, e);
            this.PanelBorder = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 是否勾选
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否勾选")]
        public bool Checked
        {
            get { return this.cbx.Checked; }
            set { this.cbx.Checked = value; }
        }

        /// <inheritdoc />
        [Browsable(true), Category("外观Ex"), Description("勾选框文本内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return this.cbx.Text; }
            set { this.cbx.Text = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// CheckBox属性更改事件
        /// </summary>
        [Description("CheckBox属性更改事件")]
        public event EventHandler CheckedChanged;
        #endregion
    }

    /// <summary>
    /// 下拉框型编辑块
    /// </summary>
    [ToolboxBitmap(typeof(ComboBox))]
    public class CComboBox : CEdit
    {
        #region Fields
        private readonly ComboBox cbx;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CComboBox()
        {
            this.cbx = new ComboBox {FlatStyle = FlatStyle.Flat, Margin = new Padding(1), Name = "cComboBox"};
            this.Container.Controls.Add(this.cbx);
            this.cbx.TextChanged += (sender, e) => TextChanged?.Invoke(this, e);
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 下拉框样式
        /// </summary>
        [Category("外观Ex"), DefaultValue(ComboBoxStyle.DropDown), Description("下拉框样式")]
        public ComboBoxStyle DropDownStyle
        {
            get { return this.cbx.DropDownStyle; }
            set
            {
                this.cbx.DropDownStyle = value;
                this.cbx.Anchor = value == ComboBoxStyle.Simple ? AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                    : AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        /// <summary>
        /// 设置下拉框的选项
        /// </summary>
        [Category("外观Ex"), Description("设置下拉框的选项")]
        public IList Items
        {
            get { return this.cbx.Items; }
            set
            {
                var items = new object[value.Count];
                value.CopyTo(items, 0);
                this.cbx.Items.Clear();
                this.cbx.Items.AddRange(items);
            }
        }

        /// <summary>
        /// 下拉框选择序号
        /// </summary>
        public int SelectedIndex
        {
            get { return this.cbx.SelectedIndex; }
            set { this.cbx.SelectedIndex = value; }
        }

        /// <inheritdoc />
        [Browsable(true), Category("外观Ex"), Description("文本框内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return this.cbx.Text; }
            set { this.cbx.Text = value; }
        }

        /// <summary>
        /// 下拉框选择内容
        /// </summary>
        public string SelectedText => this.cbx.SelectedText;
        #endregion

        #region Events
        /// <summary>
        /// 文本内容改变事件
        /// </summary>
        [Browsable(true), Description("文本内容改变事件")]
        public new event EventHandler TextChanged;
        #endregion
    }

    /// <summary>
    /// 通用编辑块
    /// </summary>
    public partial class CEdit : UserControl
    {
        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        protected CEdit()
        {
            InitializeComponent();
            this.HasButton = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 按钮文字
        /// </summary>
        [Category("外观Ex"), Description("按钮文字")]
        public string ButtonText
        {
            get { return this.cButton.Text; }
            set { this.cButton.Text = value; }
        }

        /// <summary>
        /// 是否含有按钮
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否含有按钮")]
        public bool HasButton
        {
            get { return this.cButton.Visible; }
            set { this.cButton.Visible = value; }
        }

        /// <summary>
        /// 标签的内容
        /// </summary>
        [Category("外观Ex"), Description("标签的内容")]
        public string Label
        {
            get { return this.cLabel.Text; }
            set { this.cLabel.Text = value; }
        }

        /// <summary>
        /// 可选控件的容器
        /// </summary>
        protected new TableLayoutPanel Container { get; private set; }

        /// <summary>
        /// 是否含有边框
        /// </summary>
        protected bool PanelBorder
        {
            get { return this.cPanelEx.BorderSide == ToolStripStatusLabelBorderSides.All; }
            set { this.cPanelEx.BorderSide = value ? ToolStripStatusLabelBorderSides.All : ToolStripStatusLabelBorderSides.None; }
        }
        #endregion

        #region Methods
        private void button1_Click(object sender, EventArgs e) => OnClick(e);
        #endregion
    }

    /// <summary>
    /// 自定义型编辑块
    /// </summary>
    [Designer(typeof(UcFoldPanelDesigner))]
    public class CEditEx : CEdit
    {
        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CEditEx()
        {
            this.PanelBorder = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 可选控件的容器
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TableLayoutPanel Panel => this.Container;
        #endregion

        #region Nested type: UcFoldPanelDesigner
        private class UcFoldPanelDesigner : ControlDesigner
        {
            #region Fields
            private CEditEx ucFoldPanelControl;
            #endregion

            #region Methods
            public override void Initialize(IComponent component)
            {
                base.Initialize(component);
                this.ucFoldPanelControl = (CEditEx)component;
                EnableDesignMode(this.ucFoldPanelControl.Panel, "Panel");
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 数字型编辑块
    /// </summary>
    [ToolboxBitmap(typeof(NumericUpDown))]
    public class CNumericUpDown : CEdit
    {
        #region Fields
        private readonly NumericUpDown nud;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CNumericUpDown()
        {
            this.nud = new NumericUpDown {Anchor = AnchorStyles.Left | AnchorStyles.Right, BorderStyle = BorderStyle.None, Name = "cNumeric"};
            this.Container.Controls.Add(this.nud);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 设置数字的小数点位数
        /// </summary>
        [Category("外观Ex"), DefaultValue(0), Description("设置数字的小数点位数")]
        public int DecimalPlaces
        {
            get { return this.nud.DecimalPlaces; }
            set { this.nud.DecimalPlaces = value; }
        }

        /// <summary>
        /// 设置最小增量值
        /// </summary>
        [Category("外观Ex"), DefaultValue(1), Description("设置最小增量值")]
        public decimal Increment
        {
            get { return this.nud.Increment; }
            set { this.nud.Increment = value; }
        }

        /// <summary>
        /// 设置允许的最大值
        /// </summary>
        [Category("外观Ex"), DefaultValue(100), Description("设置允许的最大值")]
        public decimal Maximum
        {
            get { return this.nud.Maximum; }
            set { this.nud.Maximum = value; }
        }

        /// <summary>
        /// 设置允许的最小值
        /// </summary>
        [Category("外观Ex"), DefaultValue(0), Description("设置允许的最小值")]
        public decimal Minimum
        {
            get { return this.nud.Minimum; }
            set { this.nud.Minimum = value; }
        }

        /// <summary>
        /// 数字框的值
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("数字框的值")]
        public decimal Value
        {
            get { return this.nud.Value; }
            set { this.nud.Value = value; }
        }
        #endregion
    }

    /// <summary>
    /// 图片型编辑块
    /// </summary>
    [ToolboxBitmap(typeof(PictureBox))]
    public class CPictureBox : CEdit
    {
        #region Fields
        private readonly PictureBox pbx;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CPictureBox()
        {
            this.pbx = new PictureBox {BackgroundImageLayout = ImageLayout.Stretch, Dock = DockStyle.Fill, Name = "cPictureBox"};
            this.Container.Controls.Add(this.pbx);
            this.PanelBorder = false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 设置背景图
        /// </summary>
        [Category("外观Ex"), Description("设置背景图")]
        public Image BackGroundImage
        {
            get { return this.pbx.BackgroundImage; }
            set { this.pbx.BackgroundImage = value; }
        }

        /// <summary>
        /// 设置前景图
        /// </summary>
        [Category("外观Ex"), Description("设置前景图")]
        public Image Image
        {
            get { return this.pbx.Image; }
            set { this.pbx.Image = value; }
        }
        #endregion
    }

    /// <summary>
    /// 文本框型编辑块
    /// </summary>
    [ToolboxBitmap(typeof(TextBox))]
    public class CTextBox : CEdit
    {
        #region Fields
        private readonly TextBox tbx;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public CTextBox()
        {
            this.tbx = new TextBox {BorderStyle = BorderStyle.None, Name = "cTextBox", Multiline = false};
            this.Container.Controls.Add(this.tbx);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 是否多行
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否多行")]
        public bool Multiline
        {
            get { return this.tbx.Multiline; }
            set
            {
                this.tbx.Multiline = value;
                this.tbx.Anchor = value ? AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
                    : AnchorStyles.Left | AnchorStyles.Right;
            }
        }

        /// <summary>
        /// 是否只读
        /// </summary>
        [Category("外观Ex"), DefaultValue(false), Description("是否只读")]
        public bool ReadOnly
        {
            get { return this.tbx.ReadOnly; }
            set { this.tbx.ReadOnly = value; }
        }

        /// <inheritdoc />
        [Browsable(true), Category("外观Ex"), Description("文本框内容"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return this.tbx.Text; }
            set { this.tbx.Text = value; }
        }

        /// <summary>
        /// 文本对齐方式
        /// </summary>
        [Category("外观Ex"), DefaultValue(HorizontalAlignment.Left), Description("文本对齐方式")]
        public HorizontalAlignment TextAlign
        {
            get { return this.tbx.TextAlign; }
            set { this.tbx.TextAlign = value; }
        }

        /// <summary>
        /// 文本框背景色
        /// </summary>
        [Category("外观Ex"), Description("文本框背景色")]
        public Color TextBackColor
        {
            get { return this.tbx.BackColor; }
            set { this.tbx.BackColor = value; }
        }

        /// <summary>
        /// 文本颜色
        /// </summary>
        [Category("外观Ex"), Description("文本颜色")]
        public Color TextForeColor
        {
            get { return this.tbx.ForeColor; }
            set { this.tbx.ForeColor = value; }
        }
        #endregion
    }
}
