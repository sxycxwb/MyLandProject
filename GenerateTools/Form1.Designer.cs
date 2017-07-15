namespace GenerateTools
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtWorkPath = new System.Windows.Forms.TextBox();
            this.btnSetWordPath = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBarCollect = new System.Windows.Forms.ProgressBar();
            this.lbPrecent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtWorkPath
            // 
            this.txtWorkPath.Location = new System.Drawing.Point(11, 21);
            this.txtWorkPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtWorkPath.Multiline = true;
            this.txtWorkPath.Name = "txtWorkPath";
            this.txtWorkPath.Size = new System.Drawing.Size(248, 53);
            this.txtWorkPath.TabIndex = 11;
            this.txtWorkPath.Text = "C:\\Users\\spring\\Documents\\Tencent Files\\646323970\\FileRecv\\寺前 4组界址点成果表\\寺前 4组界址点成果" +
    "表";
            // 
            // btnSetWordPath
            // 
            this.btnSetWordPath.Location = new System.Drawing.Point(263, 47);
            this.btnSetWordPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetWordPath.Name = "btnSetWordPath";
            this.btnSetWordPath.Size = new System.Drawing.Size(136, 27);
            this.btnSetWordPath.TabIndex = 12;
            this.btnSetWordPath.Text = "选择界址点成果表路径";
            this.btnSetWordPath.UseVisualStyleBackColor = true;
            this.btnSetWordPath.Click += new System.EventHandler(this.btnSetWordPath_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(11, 189);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(136, 27);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "开始生成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "采集数据进度：";
            // 
            // progressBarCollect
            // 
            this.progressBarCollect.Location = new System.Drawing.Point(101, 121);
            this.progressBarCollect.Name = "progressBarCollect";
            this.progressBarCollect.Size = new System.Drawing.Size(299, 23);
            this.progressBarCollect.TabIndex = 17;
            // 
            // lbPrecent
            // 
            this.lbPrecent.AutoSize = true;
            this.lbPrecent.Location = new System.Drawing.Point(354, 196);
            this.lbPrecent.Name = "lbPrecent";
            this.lbPrecent.Size = new System.Drawing.Size(0, 12);
            this.lbPrecent.TabIndex = 18;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 259);
            this.Controls.Add(this.lbPrecent);
            this.Controls.Add(this.progressBarCollect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtWorkPath);
            this.Controls.Add(this.btnSetWordPath);
            this.Name = "Form1";
            this.Text = "界址点精度与面积精度记录生成工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtWorkPath;
        private System.Windows.Forms.Button btnSetWordPath;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBarCollect;
        private System.Windows.Forms.Label lbPrecent;
    }
}

