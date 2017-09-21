namespace AutomaticValidationTool
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
            this.btnStart = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.txtCombinePath = new System.Windows.Forms.TextBox();
            this.txtCheckName = new System.Windows.Forms.TextBox();
            this.txtCheckDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 156);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(20, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(371, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "提示：选择待正确性检测的文件夹(包含权属数据,汇总表格等文件夹)";
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(284, 44);
            this.btnSetPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(108, 27);
            this.btnSetPath.TabIndex = 17;
            this.btnSetPath.Text = "① 选择操作路径";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // txtCombinePath
            // 
            this.txtCombinePath.Location = new System.Drawing.Point(12, 39);
            this.txtCombinePath.Multiline = true;
            this.txtCombinePath.Name = "txtCombinePath";
            this.txtCombinePath.Size = new System.Drawing.Size(265, 36);
            this.txtCombinePath.TabIndex = 16;
            // 
            // txtCheckName
            // 
            this.txtCheckName.Location = new System.Drawing.Point(101, 103);
            this.txtCheckName.Name = "txtCheckName";
            this.txtCheckName.Size = new System.Drawing.Size(91, 21);
            this.txtCheckName.TabIndex = 26;
            // 
            // txtCheckDate
            // 
            this.txtCheckDate.Location = new System.Drawing.Point(263, 102);
            this.txtCheckDate.Name = "txtCheckDate";
            this.txtCheckDate.Size = new System.Drawing.Size(136, 21);
            this.txtCheckDate.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "检查日期：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 106);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "检查人员姓名：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(93, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 12);
            this.label1.TabIndex = 27;
            this.label1.Text = "提示：点击开始后需等待一会，操作完成会有提醒";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 264);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCheckName);
            this.Controls.Add(this.txtCheckDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSetPath);
            this.Controls.Add(this.txtCombinePath);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "土地确权成果数据自动检查工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.TextBox txtCombinePath;
        private System.Windows.Forms.TextBox txtCheckName;
        private System.Windows.Forms.DateTimePicker txtCheckDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

