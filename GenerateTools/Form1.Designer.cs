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
            this.txtGeneratePath = new System.Windows.Forms.TextBox();
            this.btnSetGeneratePath = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtWorkPath
            // 
            this.txtWorkPath.Location = new System.Drawing.Point(11, 21);
            this.txtWorkPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtWorkPath.Multiline = true;
            this.txtWorkPath.Name = "txtWorkPath";
            this.txtWorkPath.Size = new System.Drawing.Size(248, 33);
            this.txtWorkPath.TabIndex = 11;
            this.txtWorkPath.Text = "C:\\Users\\spring\\Documents\\Tencent Files\\646323970\\FileRecv\\寺前 4组界址点成果表\\寺前 4组界址点成果" +
    "表";
            // 
            // btnSetWordPath
            // 
            this.btnSetWordPath.Location = new System.Drawing.Point(264, 24);
            this.btnSetWordPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetWordPath.Name = "btnSetWordPath";
            this.btnSetWordPath.Size = new System.Drawing.Size(136, 27);
            this.btnSetWordPath.TabIndex = 12;
            this.btnSetWordPath.Text = "选择界址点成果表路径";
            this.btnSetWordPath.UseVisualStyleBackColor = true;
            this.btnSetWordPath.Click += new System.EventHandler(this.btnSetWordPath_Click);
            // 
            // txtGeneratePath
            // 
            this.txtGeneratePath.Location = new System.Drawing.Point(11, 82);
            this.txtGeneratePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtGeneratePath.Multiline = true;
            this.txtGeneratePath.Name = "txtGeneratePath";
            this.txtGeneratePath.Size = new System.Drawing.Size(248, 19);
            this.txtGeneratePath.TabIndex = 13;
            this.txtGeneratePath.Text = "D:\\界址点和面积精度表";
            // 
            // btnSetGeneratePath
            // 
            this.btnSetGeneratePath.Location = new System.Drawing.Point(264, 78);
            this.btnSetGeneratePath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetGeneratePath.Name = "btnSetGeneratePath";
            this.btnSetGeneratePath.Size = new System.Drawing.Size(136, 27);
            this.btnSetGeneratePath.TabIndex = 14;
            this.btnSetGeneratePath.Text = "设置生成路径";
            this.btnSetGeneratePath.UseVisualStyleBackColor = true;
            this.btnSetGeneratePath.Click += new System.EventHandler(this.btnSetGeneratePath_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(11, 146);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(136, 27);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "开始生成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 259);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtGeneratePath);
            this.Controls.Add(this.btnSetGeneratePath);
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
        private System.Windows.Forms.TextBox txtGeneratePath;
        private System.Windows.Forms.Button btnSetGeneratePath;
        private System.Windows.Forms.Button btnStart;
    }
}

