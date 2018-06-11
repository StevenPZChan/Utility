namespace Utility.Form
{
    partial class HalconROIHelper
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
            this.btnLine = new System.Windows.Forms.Button();
            this.btnDiffRec2 = new System.Windows.Forms.Button();
            this.btnDrawCircle = new System.Windows.Forms.Button();
            this.btnDiffRec1 = new System.Windows.Forms.Button();
            this.btnDrawEllipse = new System.Windows.Forms.Button();
            this.btnDiffEllipse = new System.Windows.Forms.Button();
            this.btnDrawRec1 = new System.Windows.Forms.Button();
            this.btnDiffCircle = new System.Windows.Forms.Button();
            this.btnDrawRec2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
            this.tableLayoutPanel1.Controls.Add(this.btnLine, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDiffRec2, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawCircle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDiffRec1, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawEllipse, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDiffEllipse, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawRec1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDiffCircle, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDrawRec2, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.AddColumns;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(470, 54);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnLine
            // 
            this.btnLine.BackColor = System.Drawing.SystemColors.Control;
            this.btnLine.BackgroundImage = global::Utility.Properties.Resources.line;
            this.btnLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLine.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLine.Location = new System.Drawing.Point(0, 0);
            this.btnLine.Margin = new System.Windows.Forms.Padding(0);
            this.btnLine.Name = "btnLine";
            this.btnLine.Size = new System.Drawing.Size(52, 54);
            this.btnLine.TabIndex = 0;
            this.btnLine.UseVisualStyleBackColor = false;
            this.btnLine.Click += new System.EventHandler(this.btnLine_Click);
            // 
            // btnDiffRec2
            // 
            this.btnDiffRec2.BackColor = System.Drawing.SystemColors.Control;
            this.btnDiffRec2.BackgroundImage = global::Utility.Properties.Resources.rect2_sub;
            this.btnDiffRec2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiffRec2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDiffRec2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDiffRec2.Location = new System.Drawing.Point(416, 0);
            this.btnDiffRec2.Margin = new System.Windows.Forms.Padding(0);
            this.btnDiffRec2.Name = "btnDiffRec2";
            this.btnDiffRec2.Size = new System.Drawing.Size(54, 54);
            this.btnDiffRec2.TabIndex = 0;
            this.btnDiffRec2.UseVisualStyleBackColor = false;
            this.btnDiffRec2.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDrawCircle
            // 
            this.btnDrawCircle.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrawCircle.BackgroundImage = global::Utility.Properties.Resources.circle_add;
            this.btnDrawCircle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDrawCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawCircle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDrawCircle.Location = new System.Drawing.Point(52, 0);
            this.btnDrawCircle.Margin = new System.Windows.Forms.Padding(0);
            this.btnDrawCircle.Name = "btnDrawCircle";
            this.btnDrawCircle.Size = new System.Drawing.Size(52, 54);
            this.btnDrawCircle.TabIndex = 0;
            this.btnDrawCircle.UseVisualStyleBackColor = false;
            this.btnDrawCircle.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDiffRec1
            // 
            this.btnDiffRec1.BackColor = System.Drawing.SystemColors.Control;
            this.btnDiffRec1.BackgroundImage = global::Utility.Properties.Resources.rect1_sub;
            this.btnDiffRec1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiffRec1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDiffRec1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDiffRec1.Location = new System.Drawing.Point(364, 0);
            this.btnDiffRec1.Margin = new System.Windows.Forms.Padding(0);
            this.btnDiffRec1.Name = "btnDiffRec1";
            this.btnDiffRec1.Size = new System.Drawing.Size(52, 54);
            this.btnDiffRec1.TabIndex = 0;
            this.btnDiffRec1.UseVisualStyleBackColor = false;
            this.btnDiffRec1.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDrawEllipse
            // 
            this.btnDrawEllipse.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrawEllipse.BackgroundImage = global::Utility.Properties.Resources.ellipse_add;
            this.btnDrawEllipse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDrawEllipse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawEllipse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDrawEllipse.Location = new System.Drawing.Point(104, 0);
            this.btnDrawEllipse.Margin = new System.Windows.Forms.Padding(0);
            this.btnDrawEllipse.Name = "btnDrawEllipse";
            this.btnDrawEllipse.Size = new System.Drawing.Size(52, 54);
            this.btnDrawEllipse.TabIndex = 0;
            this.btnDrawEllipse.UseVisualStyleBackColor = false;
            this.btnDrawEllipse.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDiffEllipse
            // 
            this.btnDiffEllipse.BackColor = System.Drawing.SystemColors.Control;
            this.btnDiffEllipse.BackgroundImage = global::Utility.Properties.Resources.ellipse_sub;
            this.btnDiffEllipse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiffEllipse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDiffEllipse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDiffEllipse.Location = new System.Drawing.Point(312, 0);
            this.btnDiffEllipse.Margin = new System.Windows.Forms.Padding(0);
            this.btnDiffEllipse.Name = "btnDiffEllipse";
            this.btnDiffEllipse.Size = new System.Drawing.Size(52, 54);
            this.btnDiffEllipse.TabIndex = 0;
            this.btnDiffEllipse.UseVisualStyleBackColor = false;
            this.btnDiffEllipse.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDrawRec1
            // 
            this.btnDrawRec1.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrawRec1.BackgroundImage = global::Utility.Properties.Resources.rect1_add;
            this.btnDrawRec1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDrawRec1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawRec1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDrawRec1.Location = new System.Drawing.Point(156, 0);
            this.btnDrawRec1.Margin = new System.Windows.Forms.Padding(0);
            this.btnDrawRec1.Name = "btnDrawRec1";
            this.btnDrawRec1.Size = new System.Drawing.Size(52, 54);
            this.btnDrawRec1.TabIndex = 0;
            this.btnDrawRec1.UseVisualStyleBackColor = false;
            this.btnDrawRec1.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDiffCircle
            // 
            this.btnDiffCircle.BackColor = System.Drawing.SystemColors.Control;
            this.btnDiffCircle.BackgroundImage = global::Utility.Properties.Resources.circle_sub;
            this.btnDiffCircle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDiffCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDiffCircle.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDiffCircle.Location = new System.Drawing.Point(260, 0);
            this.btnDiffCircle.Margin = new System.Windows.Forms.Padding(0);
            this.btnDiffCircle.Name = "btnDiffCircle";
            this.btnDiffCircle.Size = new System.Drawing.Size(52, 54);
            this.btnDiffCircle.TabIndex = 0;
            this.btnDiffCircle.UseVisualStyleBackColor = false;
            this.btnDiffCircle.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // btnDrawRec2
            // 
            this.btnDrawRec2.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrawRec2.BackgroundImage = global::Utility.Properties.Resources.rect2_add;
            this.btnDrawRec2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDrawRec2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDrawRec2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDrawRec2.Location = new System.Drawing.Point(208, 0);
            this.btnDrawRec2.Margin = new System.Windows.Forms.Padding(0);
            this.btnDrawRec2.Name = "btnDrawRec2";
            this.btnDrawRec2.Size = new System.Drawing.Size(52, 54);
            this.btnDrawRec2.TabIndex = 0;
            this.btnDrawRec2.UseVisualStyleBackColor = false;
            this.btnDrawRec2.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // HalconROIHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "HalconROIHelper";
            this.Size = new System.Drawing.Size(470, 54);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnLine;
        private System.Windows.Forms.Button btnDrawCircle;
        private System.Windows.Forms.Button btnDiffRec1;
        private System.Windows.Forms.Button btnDrawEllipse;
        private System.Windows.Forms.Button btnDiffEllipse;
        private System.Windows.Forms.Button btnDrawRec1;
        private System.Windows.Forms.Button btnDiffCircle;
        private System.Windows.Forms.Button btnDrawRec2;
        private System.Windows.Forms.Button btnDiffRec2;
    }
}
