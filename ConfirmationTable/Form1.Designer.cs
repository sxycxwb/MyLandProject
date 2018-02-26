namespace ConfirmationTable
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbDbSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDWMC = new System.Windows.Forms.TextBox();
            this.btnSure = new System.Windows.Forms.Button();
            this.txtSelectDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "单位选择：";
            // 
            // cbDbSelect
            // 
            this.cbDbSelect.FormattingEnabled = true;
            this.cbDbSelect.Location = new System.Drawing.Point(73, 22);
            this.cbDbSelect.Name = "cbDbSelect";
            this.cbDbSelect.Size = new System.Drawing.Size(180, 20);
            this.cbDbSelect.TabIndex = 1;
            this.cbDbSelect.SelectedIndexChanged += new System.EventHandler(this.cbDbSelect_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "单位名称：(可修改)";
            // 
            // txtDWMC
            // 
            this.txtDWMC.Location = new System.Drawing.Point(406, 21);
            this.txtDWMC.Name = "txtDWMC";
            this.txtDWMC.Size = new System.Drawing.Size(184, 21);
            this.txtDWMC.TabIndex = 3;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(292, 86);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(111, 29);
            this.btnSure.TabIndex = 5;
            this.btnSure.Text = "开始生成确认表";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // txtSelectDate
            // 
            this.txtSelectDate.Location = new System.Drawing.Point(73, 88);
            this.txtSelectDate.Name = "txtSelectDate";
            this.txtSelectDate.Size = new System.Drawing.Size(180, 21);
            this.txtSelectDate.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "日期选择：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 322);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSelectDate);
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.txtDWMC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDbSelect);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "农村土地承包经营权确权登记单户确认表生成";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDbSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDWMC;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.DateTimePicker txtSelectDate;
        private System.Windows.Forms.Label label3;
    }
}

