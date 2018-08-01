namespace Utility.Form
{
    partial class ScaraCalibTool
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbxFocus = new Utility.Form.CTextBox();
            this.tbxKappa = new Utility.Form.CTextBox();
            this.tbxSx = new Utility.Form.CTextBox();
            this.tbxSy = new Utility.Form.CTextBox();
            this.tbxCx = new Utility.Form.CTextBox();
            this.tbxCy = new Utility.Form.CTextBox();
            this.tbxWidth = new Utility.Form.CTextBox();
            this.tbxHeight = new Utility.Form.CTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbxTx = new Utility.Form.CTextBox();
            this.tbxTy = new Utility.Form.CTextBox();
            this.tbxTz = new Utility.Form.CTextBox();
            this.tbxAlpha = new Utility.Form.CTextBox();
            this.tbxBeta = new Utility.Form.CTextBox();
            this.tbxGamma = new Utility.Form.CTextBox();
            this.tbxType = new Utility.Form.CTextBox();
            this.tbxError = new Utility.Form.CTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnReadPos = new System.Windows.Forms.Button();
            this.btnSavePos = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.增加点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.普通输入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.交叉点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.圆点中心ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSub = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnCalib = new System.Windows.Forms.Button();
            this.btnReadParam = new System.Windows.Forms.Button();
            this.btnSavePose = new System.Windows.Forms.Button();
            this.HwControl = new HalconDotNet.HWindowControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.机器人坐标ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.HwControl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(732, 672);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 406);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(506, 263);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(247, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "内参";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.tbxFocus);
            this.flowLayoutPanel2.Controls.Add(this.tbxKappa);
            this.flowLayoutPanel2.Controls.Add(this.tbxSx);
            this.flowLayoutPanel2.Controls.Add(this.tbxSy);
            this.flowLayoutPanel2.Controls.Add(this.tbxCx);
            this.flowLayoutPanel2.Controls.Add(this.tbxCy);
            this.flowLayoutPanel2.Controls.Add(this.tbxWidth);
            this.flowLayoutPanel2.Controls.Add(this.tbxHeight);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(241, 237);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // tbxFocus
            // 
            this.tbxFocus.ButtonText = "...";
            this.tbxFocus.Label = "f/m";
            this.tbxFocus.Location = new System.Drawing.Point(0, 0);
            this.tbxFocus.Margin = new System.Windows.Forms.Padding(0);
            this.tbxFocus.Name = "tbxFocus";
            this.tbxFocus.Size = new System.Drawing.Size(169, 29);
            this.tbxFocus.TabIndex = 0;
            this.tbxFocus.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxFocus.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxKappa
            // 
            this.tbxKappa.ButtonText = "...";
            this.tbxKappa.Label = "Kappa/m^-2";
            this.tbxKappa.Location = new System.Drawing.Point(0, 29);
            this.tbxKappa.Margin = new System.Windows.Forms.Padding(0);
            this.tbxKappa.Name = "tbxKappa";
            this.tbxKappa.Size = new System.Drawing.Size(169, 29);
            this.tbxKappa.TabIndex = 0;
            this.tbxKappa.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxKappa.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxSx
            // 
            this.tbxSx.ButtonText = "...";
            this.tbxSx.Label = "Sx/m";
            this.tbxSx.Location = new System.Drawing.Point(0, 58);
            this.tbxSx.Margin = new System.Windows.Forms.Padding(0);
            this.tbxSx.Name = "tbxSx";
            this.tbxSx.Size = new System.Drawing.Size(169, 29);
            this.tbxSx.TabIndex = 0;
            this.tbxSx.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxSx.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxSy
            // 
            this.tbxSy.ButtonText = "...";
            this.tbxSy.Label = "Sy/m";
            this.tbxSy.Location = new System.Drawing.Point(0, 87);
            this.tbxSy.Margin = new System.Windows.Forms.Padding(0);
            this.tbxSy.Name = "tbxSy";
            this.tbxSy.Size = new System.Drawing.Size(169, 29);
            this.tbxSy.TabIndex = 0;
            this.tbxSy.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxSy.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxCx
            // 
            this.tbxCx.ButtonText = "...";
            this.tbxCx.Label = "Cx/pixel";
            this.tbxCx.Location = new System.Drawing.Point(0, 116);
            this.tbxCx.Margin = new System.Windows.Forms.Padding(0);
            this.tbxCx.Name = "tbxCx";
            this.tbxCx.Size = new System.Drawing.Size(169, 29);
            this.tbxCx.TabIndex = 0;
            this.tbxCx.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxCx.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxCy
            // 
            this.tbxCy.ButtonText = "...";
            this.tbxCy.Label = "Cy/pixel";
            this.tbxCy.Location = new System.Drawing.Point(0, 145);
            this.tbxCy.Margin = new System.Windows.Forms.Padding(0);
            this.tbxCy.Name = "tbxCy";
            this.tbxCy.Size = new System.Drawing.Size(169, 29);
            this.tbxCy.TabIndex = 0;
            this.tbxCy.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxCy.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxWidth
            // 
            this.tbxWidth.ButtonText = "...";
            this.tbxWidth.Label = "Width/pixel";
            this.tbxWidth.Location = new System.Drawing.Point(0, 174);
            this.tbxWidth.Margin = new System.Windows.Forms.Padding(0);
            this.tbxWidth.Name = "tbxWidth";
            this.tbxWidth.Size = new System.Drawing.Size(169, 29);
            this.tbxWidth.TabIndex = 0;
            this.tbxWidth.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxWidth.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxHeight
            // 
            this.tbxHeight.ButtonText = "...";
            this.tbxHeight.Label = "Height/pixel";
            this.tbxHeight.Location = new System.Drawing.Point(0, 203);
            this.tbxHeight.Margin = new System.Windows.Forms.Padding(0);
            this.tbxHeight.Name = "tbxHeight";
            this.tbxHeight.Size = new System.Drawing.Size(169, 29);
            this.tbxHeight.TabIndex = 0;
            this.tbxHeight.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxHeight.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.flowLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(256, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 257);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "外参";
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.tbxTx);
            this.flowLayoutPanel3.Controls.Add(this.tbxTy);
            this.flowLayoutPanel3.Controls.Add(this.tbxTz);
            this.flowLayoutPanel3.Controls.Add(this.tbxAlpha);
            this.flowLayoutPanel3.Controls.Add(this.tbxBeta);
            this.flowLayoutPanel3.Controls.Add(this.tbxGamma);
            this.flowLayoutPanel3.Controls.Add(this.tbxType);
            this.flowLayoutPanel3.Controls.Add(this.tbxError);
            this.flowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(241, 237);
            this.flowLayoutPanel3.TabIndex = 1;
            // 
            // tbxTx
            // 
            this.tbxTx.ButtonText = "...";
            this.tbxTx.Label = "Tx/m";
            this.tbxTx.Location = new System.Drawing.Point(0, 0);
            this.tbxTx.Margin = new System.Windows.Forms.Padding(0);
            this.tbxTx.Name = "tbxTx";
            this.tbxTx.ReadOnly = true;
            this.tbxTx.Size = new System.Drawing.Size(169, 29);
            this.tbxTx.TabIndex = 0;
            this.tbxTx.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxTx.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxTy
            // 
            this.tbxTy.ButtonText = "...";
            this.tbxTy.Label = "Ty/m";
            this.tbxTy.Location = new System.Drawing.Point(0, 29);
            this.tbxTy.Margin = new System.Windows.Forms.Padding(0);
            this.tbxTy.Name = "tbxTy";
            this.tbxTy.ReadOnly = true;
            this.tbxTy.Size = new System.Drawing.Size(169, 29);
            this.tbxTy.TabIndex = 0;
            this.tbxTy.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxTy.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxTz
            // 
            this.tbxTz.ButtonText = "...";
            this.tbxTz.Label = "Tz/m";
            this.tbxTz.Location = new System.Drawing.Point(0, 58);
            this.tbxTz.Margin = new System.Windows.Forms.Padding(0);
            this.tbxTz.Name = "tbxTz";
            this.tbxTz.ReadOnly = true;
            this.tbxTz.Size = new System.Drawing.Size(169, 29);
            this.tbxTz.TabIndex = 0;
            this.tbxTz.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxTz.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxAlpha
            // 
            this.tbxAlpha.ButtonText = "...";
            this.tbxAlpha.Label = "Alpha/degree";
            this.tbxAlpha.Location = new System.Drawing.Point(0, 87);
            this.tbxAlpha.Margin = new System.Windows.Forms.Padding(0);
            this.tbxAlpha.Name = "tbxAlpha";
            this.tbxAlpha.ReadOnly = true;
            this.tbxAlpha.Size = new System.Drawing.Size(169, 29);
            this.tbxAlpha.TabIndex = 0;
            this.tbxAlpha.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxAlpha.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxBeta
            // 
            this.tbxBeta.ButtonText = "...";
            this.tbxBeta.Label = "Beta/degree";
            this.tbxBeta.Location = new System.Drawing.Point(0, 116);
            this.tbxBeta.Margin = new System.Windows.Forms.Padding(0);
            this.tbxBeta.Name = "tbxBeta";
            this.tbxBeta.ReadOnly = true;
            this.tbxBeta.Size = new System.Drawing.Size(169, 29);
            this.tbxBeta.TabIndex = 0;
            this.tbxBeta.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxBeta.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxGamma
            // 
            this.tbxGamma.ButtonText = "...";
            this.tbxGamma.Label = "Gamma/degree";
            this.tbxGamma.Location = new System.Drawing.Point(0, 145);
            this.tbxGamma.Margin = new System.Windows.Forms.Padding(0);
            this.tbxGamma.Name = "tbxGamma";
            this.tbxGamma.ReadOnly = true;
            this.tbxGamma.Size = new System.Drawing.Size(169, 29);
            this.tbxGamma.TabIndex = 0;
            this.tbxGamma.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxGamma.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxType
            // 
            this.tbxType.ButtonText = "...";
            this.tbxType.Label = "type";
            this.tbxType.Location = new System.Drawing.Point(0, 174);
            this.tbxType.Margin = new System.Windows.Forms.Padding(0);
            this.tbxType.Name = "tbxType";
            this.tbxType.ReadOnly = true;
            this.tbxType.Size = new System.Drawing.Size(169, 29);
            this.tbxType.TabIndex = 0;
            this.tbxType.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxType.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // tbxError
            // 
            this.tbxError.ButtonText = "...";
            this.tbxError.Label = "error/pixel";
            this.tbxError.Location = new System.Drawing.Point(0, 203);
            this.tbxError.Margin = new System.Windows.Forms.Padding(0);
            this.tbxError.Name = "tbxError";
            this.tbxError.ReadOnly = true;
            this.tbxError.Size = new System.Drawing.Size(169, 29);
            this.tbxError.TabIndex = 0;
            this.tbxError.TextBackColor = System.Drawing.SystemColors.Window;
            this.tbxError.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(515, 3);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(214, 397);
            this.dataGridView1.TabIndex = 1;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column1.DataPropertyName = "ID";
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.DataPropertyName = "r";
            this.Column2.HeaderText = "r";
            this.Column2.Name = "Column2";
            this.Column2.Width = 36;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "c";
            this.Column3.HeaderText = "c";
            this.Column3.Name = "Column3";
            this.Column3.Width = 36;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.DataPropertyName = "x";
            this.Column4.HeaderText = "x";
            this.Column4.Name = "Column4";
            this.Column4.Width = 36;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column5.DataPropertyName = "y";
            this.Column5.HeaderText = "y";
            this.Column5.Name = "Column5";
            this.Column5.Width = 36;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column6.DataPropertyName = "z";
            this.Column6.HeaderText = "z";
            this.Column6.Name = "Column6";
            this.Column6.Width = 36;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnReadPos);
            this.flowLayoutPanel1.Controls.Add(this.btnSavePos);
            this.flowLayoutPanel1.Controls.Add(this.menuStrip1);
            this.flowLayoutPanel1.Controls.Add(this.btnSub);
            this.flowLayoutPanel1.Controls.Add(this.btnReset);
            this.flowLayoutPanel1.Controls.Add(this.btnCalib);
            this.flowLayoutPanel1.Controls.Add(this.btnReadParam);
            this.flowLayoutPanel1.Controls.Add(this.btnSavePose);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(515, 406);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(214, 263);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnReadPos
            // 
            this.btnReadPos.Location = new System.Drawing.Point(3, 3);
            this.btnReadPos.Name = "btnReadPos";
            this.btnReadPos.Size = new System.Drawing.Size(75, 23);
            this.btnReadPos.TabIndex = 0;
            this.btnReadPos.Text = "读取坐标";
            this.btnReadPos.UseVisualStyleBackColor = true;
            this.btnReadPos.Click += new System.EventHandler(this.btnReadPos_Click);
            // 
            // btnSavePos
            // 
            this.btnSavePos.Location = new System.Drawing.Point(84, 3);
            this.btnSavePos.Name = "btnSavePos";
            this.btnSavePos.Size = new System.Drawing.Size(75, 23);
            this.btnSavePos.TabIndex = 0;
            this.btnSavePos.Text = "保存坐标";
            this.btnSavePos.UseVisualStyleBackColor = true;
            this.btnSavePos.Click += new System.EventHandler(this.btnSavePos_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.增加点ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 29);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(175, 27);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 增加点ToolStripMenuItem
            // 
            this.增加点ToolStripMenuItem.AutoSize = false;
            this.增加点ToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.增加点ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.普通输入ToolStripMenuItem,
            this.交叉点ToolStripMenuItem,
            this.圆点中心ToolStripMenuItem,
            this.机器人坐标ToolStripMenuItem});
            this.增加点ToolStripMenuItem.Name = "增加点ToolStripMenuItem";
            this.增加点ToolStripMenuItem.Size = new System.Drawing.Size(75, 23);
            this.增加点ToolStripMenuItem.Text = "增加点";
            // 
            // 普通输入ToolStripMenuItem
            // 
            this.普通输入ToolStripMenuItem.Name = "普通输入ToolStripMenuItem";
            this.普通输入ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.普通输入ToolStripMenuItem.Text = "普通输入";
            this.普通输入ToolStripMenuItem.Click += new System.EventHandler(this.普通输入ToolStripMenuItem_Click);
            // 
            // 交叉点ToolStripMenuItem
            // 
            this.交叉点ToolStripMenuItem.Name = "交叉点ToolStripMenuItem";
            this.交叉点ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.交叉点ToolStripMenuItem.Text = "交叉点";
            this.交叉点ToolStripMenuItem.Click += new System.EventHandler(this.交叉点ToolStripMenuItem_Click);
            // 
            // 圆点中心ToolStripMenuItem
            // 
            this.圆点中心ToolStripMenuItem.Name = "圆点中心ToolStripMenuItem";
            this.圆点中心ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.圆点中心ToolStripMenuItem.Text = "圆点中心";
            this.圆点中心ToolStripMenuItem.Click += new System.EventHandler(this.圆点中心ToolStripMenuItem_Click);
            // 
            // btnSub
            // 
            this.btnSub.Location = new System.Drawing.Point(3, 59);
            this.btnSub.Name = "btnSub";
            this.btnSub.Size = new System.Drawing.Size(75, 23);
            this.btnSub.TabIndex = 0;
            this.btnSub.Text = "删除点";
            this.btnSub.UseVisualStyleBackColor = true;
            this.btnSub.Click += new System.EventHandler(this.btnSub_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(84, 59);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnCalib
            // 
            this.btnCalib.Location = new System.Drawing.Point(3, 88);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(75, 23);
            this.btnCalib.TabIndex = 0;
            this.btnCalib.Text = "标定";
            this.btnCalib.UseVisualStyleBackColor = true;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // btnReadParam
            // 
            this.btnReadParam.Location = new System.Drawing.Point(84, 88);
            this.btnReadParam.Name = "btnReadParam";
            this.btnReadParam.Size = new System.Drawing.Size(75, 23);
            this.btnReadParam.TabIndex = 0;
            this.btnReadParam.Text = "读取内参";
            this.btnReadParam.UseVisualStyleBackColor = true;
            this.btnReadParam.Click += new System.EventHandler(this.btnReadParam_Click);
            // 
            // btnSavePose
            // 
            this.btnSavePose.Location = new System.Drawing.Point(3, 117);
            this.btnSavePose.Name = "btnSavePose";
            this.btnSavePose.Size = new System.Drawing.Size(75, 23);
            this.btnSavePose.TabIndex = 0;
            this.btnSavePose.Text = "保存外参";
            this.btnSavePose.UseVisualStyleBackColor = true;
            this.btnSavePose.Click += new System.EventHandler(this.btnSavePose_Click);
            // 
            // hWindowControl1
            // 
            this.HwControl.BackColor = System.Drawing.Color.Black;
            this.HwControl.BorderColor = System.Drawing.Color.Black;
            this.HwControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HwControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.HwControl.Location = new System.Drawing.Point(3, 3);
            this.HwControl.Name = "HWControl";
            this.HwControl.Size = new System.Drawing.Size(506, 397);
            this.HwControl.TabIndex = 3;
            this.HwControl.WindowSize = new System.Drawing.Size(506, 397);
            // 
            // 机器人坐标ToolStripMenuItem
            // 
            this.机器人坐标ToolStripMenuItem.Name = "机器人坐标ToolStripMenuItem";
            this.机器人坐标ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.机器人坐标ToolStripMenuItem.Text = "机器人坐标";
            this.机器人坐标ToolStripMenuItem.Click += new System.EventHandler(this.机器人坐标ToolStripMenuItem_Click);
            // 
            // ScaraCalibTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScaraCalibTool";
            this.Size = new System.Drawing.Size(732, 672);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.flowLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private Utility.Form.CTextBox tbxFocus;
        private Utility.Form.CTextBox tbxKappa;
        private Utility.Form.CTextBox tbxSx;
        private Utility.Form.CTextBox tbxSy;
        private Utility.Form.CTextBox tbxCx;
        private Utility.Form.CTextBox tbxCy;
        private Utility.Form.CTextBox tbxWidth;
        private Utility.Form.CTextBox tbxHeight;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private Utility.Form.CTextBox tbxTx;
        private Utility.Form.CTextBox tbxTy;
        private Utility.Form.CTextBox tbxTz;
        private Utility.Form.CTextBox tbxAlpha;
        private Utility.Form.CTextBox tbxBeta;
        private Utility.Form.CTextBox tbxGamma;
        private Utility.Form.CTextBox tbxType;
        private Utility.Form.CTextBox tbxError;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSub;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnCalib;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.Button btnSavePose;
        private System.Windows.Forms.Button btnReadPos;
        private System.Windows.Forms.Button btnSavePos;
        private System.Windows.Forms.Button btnReadParam;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 增加点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 普通输入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 交叉点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 圆点中心ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 机器人坐标ToolStripMenuItem;
    }
}
