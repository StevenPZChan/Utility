namespace Utility.Form
{
    partial class CalibTool
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
            this.hWinPic = new HalconDotNet.HWindowControl();
            this.buttonCalib = new System.Windows.Forms.Button();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.hWinContinuous = new HalconDotNet.HWindowControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cTextBoxDesc = new Utility.Form.CTextBox();
            this.cTextBoxSx = new Utility.Form.CTextBox();
            this.cTextBoxSy = new Utility.Form.CTextBox();
            this.cTextBoxFocus = new Utility.Form.CTextBox();
            this.cTextBoxResult = new Utility.Form.CTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWinPic
            // 
            this.hWinPic.BackColor = System.Drawing.Color.Black;
            this.hWinPic.BorderColor = System.Drawing.Color.Black;
            this.hWinPic.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWinPic.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWinPic.Location = new System.Drawing.Point(3, 3);
            this.hWinPic.Name = "hWinPic";
            this.hWinPic.Size = new System.Drawing.Size(397, 288);
            this.hWinPic.TabIndex = 0;
            this.hWinPic.WindowSize = new System.Drawing.Size(397, 288);
            // 
            // buttonCalib
            // 
            this.buttonCalib.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCalib.Enabled = false;
            this.buttonCalib.Location = new System.Drawing.Point(3, 159);
            this.buttonCalib.Name = "buttonCalib";
            this.buttonCalib.Size = new System.Drawing.Size(130, 23);
            this.buttonCalib.TabIndex = 3;
            this.buttonCalib.Text = "标定";
            this.buttonCalib.UseVisualStyleBackColor = true;
            this.buttonCalib.Click += new System.EventHandler(this.buttonCalib_Click);
            // 
            // buttonCapture
            // 
            this.buttonCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCapture.Location = new System.Drawing.Point(3, 101);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(130, 23);
            this.buttonCapture.TabIndex = 3;
            this.buttonCapture.Text = "采集";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.Enabled = false;
            this.buttonDelete.Location = new System.Drawing.Point(3, 130);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(130, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "移除";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // hWinContinuous
            // 
            this.hWinContinuous.BackColor = System.Drawing.Color.Black;
            this.hWinContinuous.BorderColor = System.Drawing.Color.Black;
            this.hWinContinuous.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWinContinuous.Location = new System.Drawing.Point(3, 3);
            this.hWinContinuous.Name = "hWinContinuous";
            this.hWinContinuous.Size = new System.Drawing.Size(130, 92);
            this.hWinContinuous.TabIndex = 0;
            this.hWinContinuous.WindowSize = new System.Drawing.Size(130, 92);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(406, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(166, 288);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(578, 392);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(137, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "保存";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "标定板描述 |*.cpd;*.descr";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.hWinPic, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.listBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cTextBoxDesc, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cTextBoxSx, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cTextBoxSy, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cTextBoxFocus, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cTextBoxResult, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(718, 420);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.hWinContinuous);
            this.flowLayoutPanel1.Controls.Add(this.buttonCapture);
            this.flowLayoutPanel1.Controls.Add(this.buttonDelete);
            this.flowLayoutPanel1.Controls.Add(this.buttonCalib);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(578, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(133, 288);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // cTextBoxDesc
            // 
            this.cTextBoxDesc.ButtonText = "...";
            this.tableLayoutPanel1.SetColumnSpan(this.cTextBoxDesc, 3);
            this.cTextBoxDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBoxDesc.HasButton = true;
            this.cTextBoxDesc.Label = "描述文件";
            this.cTextBoxDesc.Location = new System.Drawing.Point(3, 297);
            this.cTextBoxDesc.Name = "cTextBoxDesc";
            this.cTextBoxDesc.ReadOnly = true;
            this.cTextBoxDesc.Size = new System.Drawing.Size(712, 25);
            this.cTextBoxDesc.TabIndex = 6;
            this.cTextBoxDesc.TextBackColor = System.Drawing.SystemColors.Control;
            this.cTextBoxDesc.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.cTextBoxDesc.Click += new System.EventHandler(this.buttonDesc_Click);
            // 
            // cTextBoxSx
            // 
            this.cTextBoxSx.ButtonText = "...";
            this.cTextBoxSx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBoxSx.Label = "像元宽/um";
            this.cTextBoxSx.Location = new System.Drawing.Point(3, 328);
            this.cTextBoxSx.Name = "cTextBoxSx";
            this.cTextBoxSx.Size = new System.Drawing.Size(397, 25);
            this.cTextBoxSx.TabIndex = 6;
            this.cTextBoxSx.Text = "5.000";
            this.cTextBoxSx.TextBackColor = System.Drawing.SystemColors.Window;
            this.cTextBoxSx.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // cTextBoxSy
            // 
            this.cTextBoxSy.ButtonText = "...";
            this.cTextBoxSy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBoxSy.Label = "像元高/um";
            this.cTextBoxSy.Location = new System.Drawing.Point(3, 359);
            this.cTextBoxSy.Name = "cTextBoxSy";
            this.cTextBoxSy.Size = new System.Drawing.Size(397, 25);
            this.cTextBoxSy.TabIndex = 6;
            this.cTextBoxSy.Text = "5.000";
            this.cTextBoxSy.TextBackColor = System.Drawing.SystemColors.Window;
            this.cTextBoxSy.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // cTextBoxFocus
            // 
            this.cTextBoxFocus.ButtonText = "...";
            this.cTextBoxFocus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBoxFocus.Label = "焦距/mm";
            this.cTextBoxFocus.Location = new System.Drawing.Point(3, 390);
            this.cTextBoxFocus.Name = "cTextBoxFocus";
            this.cTextBoxFocus.Size = new System.Drawing.Size(397, 27);
            this.cTextBoxFocus.TabIndex = 6;
            this.cTextBoxFocus.Text = "35.000";
            this.cTextBoxFocus.TextBackColor = System.Drawing.SystemColors.Window;
            this.cTextBoxFocus.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // cTextBoxResult
            // 
            this.cTextBoxResult.ButtonText = "...";
            this.tableLayoutPanel1.SetColumnSpan(this.cTextBoxResult, 2);
            this.cTextBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTextBoxResult.HasButton = true;
            this.cTextBoxResult.Label = "保存路径";
            this.cTextBoxResult.Location = new System.Drawing.Point(406, 328);
            this.cTextBoxResult.Multiline = true;
            this.cTextBoxResult.Name = "cTextBoxResult";
            this.cTextBoxResult.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.cTextBoxResult, 2);
            this.cTextBoxResult.Size = new System.Drawing.Size(309, 56);
            this.cTextBoxResult.TabIndex = 6;
            this.cTextBoxResult.TextBackColor = System.Drawing.SystemColors.Control;
            this.cTextBoxResult.TextForeColor = System.Drawing.SystemColors.WindowText;
            this.cTextBoxResult.Click += new System.EventHandler(this.buttonResult_Click);
            // 
            // CalibTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "CalibTool";
            this.Size = new System.Drawing.Size(718, 420);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWinPic;
        private System.Windows.Forms.Button buttonCalib;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Button buttonDelete;
        private HalconDotNet.HWindowControl hWinContinuous;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private CTextBox cTextBoxDesc;
        private CTextBox cTextBoxSx;
        private CTextBox cTextBoxSy;
        private CTextBox cTextBoxFocus;
        private CTextBox cTextBoxResult;
    }
}
