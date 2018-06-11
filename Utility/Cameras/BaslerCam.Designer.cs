namespace Utility.Cameras
{
    partial class BaslerCamProperty
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
            this.triggerPolarity = new System.Windows.Forms.CheckBox();
            this.triggerEnable = new System.Windows.Forms.CheckBox();
            this.sharpnessEnable = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.gammaEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.fpsEnable = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.packetSize = new Utility.Form.CTextBox();
            this.transferDelay = new Utility.Form.CTextBox();
            this.dataDelay = new Utility.Form.CTextBox();
            this.triggerSource = new Utility.Form.CComboBox();
            this.triggerSelector = new Utility.Form.CComboBox();
            this.triggerDelay = new Utility.Form.CTextBox();
            this.gainAuto = new Utility.Form.CCheckBox();
            this.exposureTime = new Utility.Form.CTextBox();
            this.gainValue = new Utility.Form.CTextBox();
            this.gammaValue = new Utility.Form.CTextBox();
            this.sharpnessValue = new Utility.Form.CTextBox();
            this.exposureAuto = new Utility.Form.CCheckBox();
            this.fpsValue = new Utility.Form.CTextBox();
            this.imageHeight = new Utility.Form.CTextBox();
            this.imageWidth = new Utility.Form.CTextBox();
            this.pixelFormat = new Utility.Form.CComboBox();
            this.cameraSelector = new Utility.Form.CComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // triggerPolarity
            // 
            this.triggerPolarity.AutoSize = true;
            this.triggerPolarity.Checked = true;
            this.triggerPolarity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.triggerPolarity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerPolarity.Enabled = false;
            this.triggerPolarity.Location = new System.Drawing.Point(169, 41);
            this.triggerPolarity.Name = "triggerPolarity";
            this.triggerPolarity.Size = new System.Drawing.Size(60, 32);
            this.triggerPolarity.TabIndex = 1;
            this.triggerPolarity.Text = "上升沿";
            this.triggerPolarity.UseVisualStyleBackColor = true;
            // 
            // triggerEnable
            // 
            this.triggerEnable.AutoSize = true;
            this.triggerEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerEnable.Enabled = false;
            this.triggerEnable.Location = new System.Drawing.Point(169, 3);
            this.triggerEnable.Name = "triggerEnable";
            this.triggerEnable.Size = new System.Drawing.Size(60, 32);
            this.triggerEnable.TabIndex = 1;
            this.triggerEnable.Text = "触发";
            this.triggerEnable.UseVisualStyleBackColor = true;
            // 
            // sharpnessEnable
            // 
            this.sharpnessEnable.AutoSize = true;
            this.sharpnessEnable.Checked = true;
            this.sharpnessEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sharpnessEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sharpnessEnable.Enabled = false;
            this.sharpnessEnable.Location = new System.Drawing.Point(3, 117);
            this.sharpnessEnable.Name = "sharpnessEnable";
            this.sharpnessEnable.Size = new System.Drawing.Size(169, 33);
            this.sharpnessEnable.TabIndex = 1;
            this.sharpnessEnable.Text = "锐度";
            this.sharpnessEnable.UseVisualStyleBackColor = true;
            this.sharpnessEnable.CheckedChanged += new System.EventHandler(this.sharpnessEnable_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cameraSelector, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 44F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(489, 356);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel5);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(247, 217);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(239, 136);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "传输设置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel4);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 217);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(238, 136);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "触发设置";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.triggerEnable, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.triggerPolarity, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.triggerSource, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.triggerSelector, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.triggerDelay, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(232, 116);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(247, 38);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 173);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "采集设置";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.gainAuto, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.sharpnessEnable, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.gammaEnable, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.exposureTime, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.gainValue, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.gammaValue, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.sharpnessValue, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.exposureAuto, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(233, 153);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // gammaEnable
            // 
            this.gammaEnable.AutoSize = true;
            this.gammaEnable.Checked = true;
            this.gammaEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gammaEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gammaEnable.Enabled = false;
            this.gammaEnable.Location = new System.Drawing.Point(3, 79);
            this.gammaEnable.Name = "gammaEnable";
            this.gammaEnable.Size = new System.Drawing.Size(169, 32);
            this.gammaEnable.TabIndex = 1;
            this.gammaEnable.Text = "灰度系数";
            this.gammaEnable.UseVisualStyleBackColor = true;
            this.gammaEnable.CheckedChanged += new System.EventHandler(this.gammaEnable_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 173);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图像设置";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.fpsValue, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.imageHeight, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.imageWidth, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.pixelFormat, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.fpsEnable, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(232, 153);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // fpsEnable
            // 
            this.fpsEnable.AutoSize = true;
            this.fpsEnable.Checked = true;
            this.fpsEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fpsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsEnable.Enabled = false;
            this.fpsEnable.Location = new System.Drawing.Point(3, 3);
            this.fpsEnable.Name = "fpsEnable";
            this.fpsEnable.Size = new System.Drawing.Size(102, 32);
            this.fpsEnable.TabIndex = 1;
            this.fpsEnable.Text = "帧率(frame/s)";
            this.fpsEnable.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.packetSize, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.transferDelay, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.dataDelay, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(233, 116);
            this.tableLayoutPanel5.TabIndex = 4;
            // 
            // packetSize
            // 
            this.packetSize.ButtonText = "...";
            this.packetSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.packetSize.Enabled = false;
            this.packetSize.Label = "缓冲区大小(Byte)";
            this.packetSize.Location = new System.Drawing.Point(3, 3);
            this.packetSize.Name = "packetSize";
            this.packetSize.Size = new System.Drawing.Size(227, 32);
            this.packetSize.TabIndex = 0;
            this.packetSize.Text = "8192";
            this.packetSize.TextBackColor = System.Drawing.SystemColors.Window;
            this.packetSize.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // transferDelay
            // 
            this.transferDelay.ButtonText = "...";
            this.transferDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transferDelay.Enabled = false;
            this.transferDelay.Label = "传输延时(us)";
            this.transferDelay.Location = new System.Drawing.Point(3, 79);
            this.transferDelay.Name = "transferDelay";
            this.transferDelay.Size = new System.Drawing.Size(227, 34);
            this.transferDelay.TabIndex = 0;
            this.transferDelay.Text = "0";
            this.transferDelay.TextBackColor = System.Drawing.SystemColors.Window;
            this.transferDelay.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // dataDelay
            // 
            this.dataDelay.ButtonText = "...";
            this.dataDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataDelay.Enabled = false;
            this.dataDelay.Label = "数据包延时(us)";
            this.dataDelay.Location = new System.Drawing.Point(3, 41);
            this.dataDelay.Name = "dataDelay";
            this.dataDelay.Size = new System.Drawing.Size(227, 32);
            this.dataDelay.TabIndex = 0;
            this.dataDelay.Text = "16456";
            this.dataDelay.TextBackColor = System.Drawing.SystemColors.Window;
            this.dataDelay.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // triggerSource
            // 
            this.triggerSource.ButtonText = "...";
            this.triggerSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSource.Enabled = false;
            this.triggerSource.Label = "触发源";
            this.triggerSource.Location = new System.Drawing.Point(3, 41);
            this.triggerSource.Name = "triggerSource";
            this.triggerSource.SelectedIndex = -1;
            this.triggerSource.Size = new System.Drawing.Size(160, 32);
            this.triggerSource.TabIndex = 2;
            // 
            // triggerSelector
            // 
            this.triggerSelector.ButtonText = "...";
            this.triggerSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.triggerSelector.Enabled = false;
            this.triggerSelector.Label = "";
            this.triggerSelector.Location = new System.Drawing.Point(3, 3);
            this.triggerSelector.Name = "triggerSelector";
            this.triggerSelector.SelectedIndex = -1;
            this.triggerSelector.Size = new System.Drawing.Size(160, 32);
            this.triggerSelector.TabIndex = 2;
            // 
            // triggerDelay
            // 
            this.triggerDelay.ButtonText = "...";
            this.tableLayoutPanel4.SetColumnSpan(this.triggerDelay, 2);
            this.triggerDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerDelay.Enabled = false;
            this.triggerDelay.Label = "触发延时(us)";
            this.triggerDelay.Location = new System.Drawing.Point(3, 79);
            this.triggerDelay.Name = "triggerDelay";
            this.triggerDelay.Size = new System.Drawing.Size(226, 34);
            this.triggerDelay.TabIndex = 3;
            this.triggerDelay.Text = "0";
            this.triggerDelay.TextBackColor = System.Drawing.SystemColors.Window;
            this.triggerDelay.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // gainAuto
            // 
            this.gainAuto.ButtonText = "...";
            this.gainAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gainAuto.Enabled = false;
            this.gainAuto.Label = "增益(dB)";
            this.gainAuto.Location = new System.Drawing.Point(3, 41);
            this.gainAuto.Name = "gainAuto";
            this.gainAuto.Size = new System.Drawing.Size(169, 32);
            this.gainAuto.TabIndex = 4;
            this.gainAuto.Text = "自动";
            this.gainAuto.CheckedChanged += new System.EventHandler(this.gainAuto_CheckedChanged);
            // 
            // exposureTime
            // 
            this.exposureTime.ButtonText = "...";
            this.exposureTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exposureTime.Enabled = false;
            this.exposureTime.Label = "";
            this.exposureTime.Location = new System.Drawing.Point(178, 3);
            this.exposureTime.Name = "exposureTime";
            this.exposureTime.Size = new System.Drawing.Size(52, 32);
            this.exposureTime.TabIndex = 2;
            this.exposureTime.Text = "1000";
            this.exposureTime.TextBackColor = System.Drawing.SystemColors.Window;
            this.exposureTime.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // gainValue
            // 
            this.gainValue.ButtonText = "...";
            this.gainValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gainValue.Enabled = false;
            this.gainValue.Label = "";
            this.gainValue.Location = new System.Drawing.Point(178, 41);
            this.gainValue.Name = "gainValue";
            this.gainValue.Size = new System.Drawing.Size(52, 32);
            this.gainValue.TabIndex = 2;
            this.gainValue.Text = "3.00";
            this.gainValue.TextBackColor = System.Drawing.SystemColors.Window;
            this.gainValue.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // gammaValue
            // 
            this.gammaValue.ButtonText = "...";
            this.gammaValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gammaValue.Enabled = false;
            this.gammaValue.Label = "";
            this.gammaValue.Location = new System.Drawing.Point(178, 79);
            this.gammaValue.Name = "gammaValue";
            this.gammaValue.Size = new System.Drawing.Size(52, 32);
            this.gammaValue.TabIndex = 2;
            this.gammaValue.Text = "1.00";
            this.gammaValue.TextBackColor = System.Drawing.SystemColors.Window;
            this.gammaValue.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // sharpnessValue
            // 
            this.sharpnessValue.ButtonText = "...";
            this.sharpnessValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sharpnessValue.Enabled = false;
            this.sharpnessValue.Label = "";
            this.sharpnessValue.Location = new System.Drawing.Point(178, 117);
            this.sharpnessValue.Name = "sharpnessValue";
            this.sharpnessValue.Size = new System.Drawing.Size(52, 33);
            this.sharpnessValue.TabIndex = 2;
            this.sharpnessValue.Text = "0.00";
            this.sharpnessValue.TextBackColor = System.Drawing.SystemColors.Window;
            this.sharpnessValue.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // exposureAuto
            // 
            this.exposureAuto.ButtonText = "...";
            this.exposureAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exposureAuto.Enabled = false;
            this.exposureAuto.Label = "曝光(us)";
            this.exposureAuto.Location = new System.Drawing.Point(3, 3);
            this.exposureAuto.Name = "exposureAuto";
            this.exposureAuto.Size = new System.Drawing.Size(169, 32);
            this.exposureAuto.TabIndex = 3;
            this.exposureAuto.Text = "自动";
            this.exposureAuto.CheckedChanged += new System.EventHandler(this.exposureAuto_CheckedChanged);
            // 
            // fpsValue
            // 
            this.fpsValue.ButtonText = "...";
            this.fpsValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpsValue.Enabled = false;
            this.fpsValue.Label = "";
            this.fpsValue.Location = new System.Drawing.Point(111, 3);
            this.fpsValue.Name = "fpsValue";
            this.fpsValue.Size = new System.Drawing.Size(118, 32);
            this.fpsValue.TabIndex = 2;
            this.fpsValue.Text = "30.00";
            this.fpsValue.TextBackColor = System.Drawing.SystemColors.Window;
            this.fpsValue.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // imageHeight
            // 
            this.imageHeight.ButtonText = "...";
            this.tableLayoutPanel2.SetColumnSpan(this.imageHeight, 2);
            this.imageHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageHeight.Enabled = false;
            this.imageHeight.Label = "高(pixel)";
            this.imageHeight.Location = new System.Drawing.Point(3, 117);
            this.imageHeight.Name = "imageHeight";
            this.imageHeight.Size = new System.Drawing.Size(226, 33);
            this.imageHeight.TabIndex = 5;
            this.imageHeight.Text = "960";
            this.imageHeight.TextBackColor = System.Drawing.SystemColors.Window;
            this.imageHeight.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // imageWidth
            // 
            this.imageWidth.ButtonText = "...";
            this.tableLayoutPanel2.SetColumnSpan(this.imageWidth, 2);
            this.imageWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageWidth.Enabled = false;
            this.imageWidth.Label = "宽(pixel)";
            this.imageWidth.Location = new System.Drawing.Point(3, 79);
            this.imageWidth.Name = "imageWidth";
            this.imageWidth.Size = new System.Drawing.Size(226, 32);
            this.imageWidth.TabIndex = 4;
            this.imageWidth.Text = "1280";
            this.imageWidth.TextBackColor = System.Drawing.SystemColors.Window;
            this.imageWidth.TextForeColor = System.Drawing.SystemColors.WindowText;
            // 
            // pixelFormat
            // 
            this.pixelFormat.ButtonText = "...";
            this.tableLayoutPanel2.SetColumnSpan(this.pixelFormat, 2);
            this.pixelFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pixelFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.pixelFormat.Enabled = false;
            this.pixelFormat.Label = "图像格式";
            this.pixelFormat.Location = new System.Drawing.Point(3, 41);
            this.pixelFormat.Name = "pixelFormat";
            this.pixelFormat.SelectedIndex = -1;
            this.pixelFormat.Size = new System.Drawing.Size(226, 32);
            this.pixelFormat.TabIndex = 3;
            // 
            // cameraSelector
            // 
            this.cameraSelector.ButtonText = "...";
            this.tableLayoutPanel1.SetColumnSpan(this.cameraSelector, 2);
            this.cameraSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.cameraSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraSelector.Label = "选择相机";
            this.cameraSelector.Location = new System.Drawing.Point(3, 3);
            this.cameraSelector.Name = "cameraSelector";
            this.cameraSelector.SelectedIndex = -1;
            this.cameraSelector.Size = new System.Drawing.Size(483, 29);
            this.cameraSelector.TabIndex = 0;
            this.cameraSelector.TextChanged += new System.EventHandler(this.cameraSelector_TextChanged);
            // 
            // BaslerCamProperty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "BaslerCamProperty";
            this.Size = new System.Drawing.Size(489, 356);
            this.Load += new System.EventHandler(this.BaslerCamProperty_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox triggerPolarity;
        private System.Windows.Forms.CheckBox triggerEnable;
        private System.Windows.Forms.CheckBox sharpnessEnable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private Form.CComboBox cameraSelector;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox gammaEnable;
        private Form.CTextBox exposureTime;
        private Form.CTextBox gainValue;
        private Form.CTextBox gammaValue;
        private Form.CTextBox sharpnessValue;
        private Form.CCheckBox gainAuto;
        private Form.CCheckBox exposureAuto;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Form.CComboBox triggerSource;
        private Form.CComboBox triggerSelector;
        private Form.CTextBox triggerDelay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Form.CTextBox packetSize;
        private Form.CTextBox transferDelay;
        private Form.CTextBox dataDelay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Form.CTextBox fpsValue;
        private Form.CTextBox imageHeight;
        private Form.CTextBox imageWidth;
        private Form.CComboBox pixelFormat;
        private System.Windows.Forms.CheckBox fpsEnable;
    }
}
