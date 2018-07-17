namespace Utility.Form
{
    partial class CEdit
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
            this.cTableLayout1 = new System.Windows.Forms.TableLayoutPanel();
            this.cLabel = new System.Windows.Forms.Label();
            this.cButton = new System.Windows.Forms.Button();
            this.cPanelEx = new Utility.Form.PanelEx();
            this.Container = new System.Windows.Forms.TableLayoutPanel();
            this.cTableLayout1.SuspendLayout();
            this.cPanelEx.SuspendLayout();
            this.SuspendLayout();
            // 
            // cTableLayout1
            // 
            this.cTableLayout1.ColumnCount = 3;
            this.cTableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.cTableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cTableLayout1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.cTableLayout1.Controls.Add(this.cLabel, 0, 0);
            this.cTableLayout1.Controls.Add(this.cButton, 2, 0);
            this.cTableLayout1.Controls.Add(this.cPanelEx, 1, 0);
            this.cTableLayout1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cTableLayout1.Location = new System.Drawing.Point(0, 0);
            this.cTableLayout1.Name = "cTableLayout1";
            this.cTableLayout1.RowCount = 1;
            this.cTableLayout1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cTableLayout1.Size = new System.Drawing.Size(169, 29);
            this.cTableLayout1.TabIndex = 0;
            // 
            // cLabel
            // 
            this.cLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cLabel.AutoSize = true;
            this.cLabel.Location = new System.Drawing.Point(3, 8);
            this.cLabel.Name = "cLabel";
            this.cLabel.Size = new System.Drawing.Size(41, 12);
            this.cLabel.TabIndex = 0;
            this.cLabel.Text = "cLabel";
            // 
            // cButton
            // 
            this.cButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cButton.AutoSize = true;
            this.cButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cButton.Location = new System.Drawing.Point(131, 3);
            this.cButton.Name = "cButton";
            this.cButton.Size = new System.Drawing.Size(35, 23);
            this.cButton.TabIndex = 1;
            this.cButton.Text = "...";
            this.cButton.UseVisualStyleBackColor = true;
            this.cButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // cPanelEx
            // 
            this.cPanelEx.BackColor = System.Drawing.Color.Transparent;
            this.cPanelEx.BorderColor = System.Drawing.Color.LightGray;
            this.cPanelEx.BorderSide = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.cPanelEx.Controls.Add(this.Container);
            this.cPanelEx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cPanelEx.Location = new System.Drawing.Point(50, 3);
            this.cPanelEx.Name = "cPanelEx";
            this.cPanelEx.Size = new System.Drawing.Size(75, 23);
            this.cPanelEx.TabIndex = 2;
            // 
            // cTableLayout2
            // 
            this.Container.BackColor = System.Drawing.Color.Transparent;
            this.Container.ColumnCount = 1;
            this.Container.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Container.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container.Location = new System.Drawing.Point(0, 0);
            this.Container.Margin = new System.Windows.Forms.Padding(0);
            this.Container.Name = "Container";
            this.Container.RowCount = 1;
            this.Container.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Container.Size = new System.Drawing.Size(75, 23);
            this.Container.TabIndex = 0;
            // 
            // CEdit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.cTableLayout1);
            this.Name = "CEdit";
            this.Size = new System.Drawing.Size(169, 29);
            this.cTableLayout1.ResumeLayout(false);
            this.cTableLayout1.PerformLayout();
            this.cPanelEx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel cTableLayout1;
        private System.Windows.Forms.Label cLabel;
        private System.Windows.Forms.Button cButton;
        private PanelEx cPanelEx;
    }
}
