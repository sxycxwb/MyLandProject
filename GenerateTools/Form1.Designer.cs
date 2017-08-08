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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCheckDate = new System.Windows.Forms.DateTimePicker();
            this.txtCheckName = new System.Windows.Forms.TextBox();
            this.txtErrorMsg = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCopyErrorMsg = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtWorkPath
            // 
            this.txtWorkPath.Location = new System.Drawing.Point(11, 21);
            this.txtWorkPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtWorkPath.Multiline = true;
            this.txtWorkPath.Name = "txtWorkPath";
            this.txtWorkPath.Size = new System.Drawing.Size(248, 53);
            this.txtWorkPath.TabIndex = 11;
            // 
            // btnSetWordPath
            // 
            this.btnSetWordPath.Location = new System.Drawing.Point(263, 47);
            this.btnSetWordPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSetWordPath.Name = "btnSetWordPath";
            this.btnSetWordPath.Size = new System.Drawing.Size(136, 27);
            this.btnSetWordPath.TabIndex = 12;
            this.btnSetWordPath.Text = "选择界址点成果表路径";
            this.btnSetWordPath.UseVisualStyleBackColor = true;
            this.btnSetWordPath.Click += new System.EventHandler(this.btnSetWordPath_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(261, 181);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.label1.Location = new System.Drawing.Point(14, 138);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 16;
            this.label1.Text = "采集数据进度：";
            // 
            // progressBarCollect
            // 
            this.progressBarCollect.Location = new System.Drawing.Point(101, 133);
            this.progressBarCollect.Name = "progressBarCollect";
            this.progressBarCollect.Size = new System.Drawing.Size(299, 23);
            this.progressBarCollect.TabIndex = 17;
            // 
            // lbPrecent
            // 
            this.lbPrecent.AutoSize = true;
            this.lbPrecent.Location = new System.Drawing.Point(354, 167);
            this.lbPrecent.Name = "lbPrecent";
            this.lbPrecent.Size = new System.Drawing.Size(0, 12);
            this.lbPrecent.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "检查人员姓名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "检查日期：";
            // 
            // txtCheckDate
            // 
            this.txtCheckDate.Location = new System.Drawing.Point(263, 99);
            this.txtCheckDate.Name = "txtCheckDate";
            this.txtCheckDate.Size = new System.Drawing.Size(136, 21);
            this.txtCheckDate.TabIndex = 21;
            // 
            // txtCheckName
            // 
            this.txtCheckName.Location = new System.Drawing.Point(101, 100);
            this.txtCheckName.Name = "txtCheckName";
            this.txtCheckName.Size = new System.Drawing.Size(91, 21);
            this.txtCheckName.TabIndex = 22;
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.Location = new System.Drawing.Point(11, 253);
            this.txtErrorMsg.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtErrorMsg.Multiline = true;
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.Size = new System.Drawing.Size(388, 147);
            this.txtErrorMsg.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(9, 229);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "错误文件名列表信息↓";
            // 
            // btnCopyErrorMsg
            // 
            this.btnCopyErrorMsg.Location = new System.Drawing.Point(261, 229);
            this.btnCopyErrorMsg.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCopyErrorMsg.Name = "btnCopyErrorMsg";
            this.btnCopyErrorMsg.Size = new System.Drawing.Size(136, 19);
            this.btnCopyErrorMsg.TabIndex = 25;
            this.btnCopyErrorMsg.Text = "复制错误信息";
            this.btnCopyErrorMsg.UseVisualStyleBackColor = true;
            this.btnCopyErrorMsg.Click += new System.EventHandler(this.btnCopyErrorMsg_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(9, 188);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "选择采集模式：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "默认Doc模式",
            "Excel新模式"});
            this.comboBox1.Location = new System.Drawing.Point(112, 185);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 31;
            this.comboBox1.Text = "默认Doc模式";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 419);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCopyErrorMsg);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtErrorMsg);
            this.Controls.Add(this.txtCheckName);
            this.Controls.Add(this.txtCheckDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbPrecent);
            this.Controls.Add(this.progressBarCollect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtWorkPath);
            this.Controls.Add(this.btnSetWordPath);
            this.Name = "Form1";
            this.Text = "界址点精度与面积精度记录生成工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker txtCheckDate;
        private System.Windows.Forms.TextBox txtCheckName;
        private System.Windows.Forms.TextBox txtErrorMsg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCopyErrorMsg;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

