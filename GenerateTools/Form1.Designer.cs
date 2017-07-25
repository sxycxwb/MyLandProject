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
            this.SuspendLayout();
            // 
            // txtWorkPath
            // 
            this.txtWorkPath.Location = new System.Drawing.Point(16, 32);
            this.txtWorkPath.Multiline = true;
            this.txtWorkPath.Name = "txtWorkPath";
            this.txtWorkPath.Size = new System.Drawing.Size(370, 78);
            this.txtWorkPath.TabIndex = 11;
            // 
            // btnSetWordPath
            // 
            this.btnSetWordPath.Location = new System.Drawing.Point(394, 70);
            this.btnSetWordPath.Name = "btnSetWordPath";
            this.btnSetWordPath.Size = new System.Drawing.Size(204, 40);
            this.btnSetWordPath.TabIndex = 12;
            this.btnSetWordPath.Text = "选择界址点成果表路径";
            this.btnSetWordPath.UseVisualStyleBackColor = true;
            this.btnSetWordPath.Click += new System.EventHandler(this.btnSetWordPath_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(16, 267);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(204, 40);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "开始生成";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 207);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 18);
            this.label1.TabIndex = 16;
            this.label1.Text = "采集数据进度：";
            // 
            // progressBarCollect
            // 
            this.progressBarCollect.Location = new System.Drawing.Point(152, 200);
            this.progressBarCollect.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.progressBarCollect.Name = "progressBarCollect";
            this.progressBarCollect.Size = new System.Drawing.Size(448, 34);
            this.progressBarCollect.TabIndex = 17;
            // 
            // lbPrecent
            // 
            this.lbPrecent.AutoSize = true;
            this.lbPrecent.Location = new System.Drawing.Point(531, 251);
            this.lbPrecent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPrecent.Name = "lbPrecent";
            this.lbPrecent.Size = new System.Drawing.Size(0, 18);
            this.lbPrecent.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 154);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 18);
            this.label2.TabIndex = 19;
            this.label2.Text = "检查人员姓名：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 154);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 20;
            this.label3.Text = "检查日期：";
            // 
            // txtCheckDate
            // 
            this.txtCheckDate.Location = new System.Drawing.Point(394, 148);
            this.txtCheckDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCheckDate.Name = "txtCheckDate";
            this.txtCheckDate.Size = new System.Drawing.Size(202, 28);
            this.txtCheckDate.TabIndex = 21;
            // 
            // txtCheckName
            // 
            this.txtCheckName.Location = new System.Drawing.Point(152, 150);
            this.txtCheckName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCheckName.Name = "txtCheckName";
            this.txtCheckName.Size = new System.Drawing.Size(134, 28);
            this.txtCheckName.TabIndex = 22;
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.Location = new System.Drawing.Point(16, 379);
            this.txtErrorMsg.Multiline = true;
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.Size = new System.Drawing.Size(580, 218);
            this.txtErrorMsg.TabIndex = 23;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(13, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 18);
            this.label4.TabIndex = 24;
            this.label4.Text = "错误文件名列表信息↓";
            // 
            // btnCopyErrorMsg
            // 
            this.btnCopyErrorMsg.Location = new System.Drawing.Point(392, 344);
            this.btnCopyErrorMsg.Name = "btnCopyErrorMsg";
            this.btnCopyErrorMsg.Size = new System.Drawing.Size(204, 29);
            this.btnCopyErrorMsg.TabIndex = 25;
            this.btnCopyErrorMsg.Text = "复制错误信息";
            this.btnCopyErrorMsg.UseVisualStyleBackColor = true;
            this.btnCopyErrorMsg.Click += new System.EventHandler(this.btnCopyErrorMsg_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 629);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
    }
}

