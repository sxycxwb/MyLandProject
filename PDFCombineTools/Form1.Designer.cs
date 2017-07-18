namespace PDFCombineTools
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckIsDelete = new System.Windows.Forms.CheckBox();
            this.btnCombine = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.txtCombinePath = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox2.Controls.Add(this.ckIsDelete);
            this.groupBox2.Controls.Add(this.btnCombine);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnSetPath);
            this.groupBox2.Controls.Add(this.txtCombinePath);
            this.groupBox2.Location = new System.Drawing.Point(12, 26);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(956, 255);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "合并处理pdf文件";
            // 
            // ckIsDelete
            // 
            this.ckIsDelete.AutoSize = true;
            this.ckIsDelete.Checked = true;
            this.ckIsDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckIsDelete.Location = new System.Drawing.Point(375, 157);
            this.ckIsDelete.Margin = new System.Windows.Forms.Padding(4);
            this.ckIsDelete.Name = "ckIsDelete";
            this.ckIsDelete.Size = new System.Drawing.Size(214, 22);
            this.ckIsDelete.TabIndex = 14;
            this.ckIsDelete.Text = "合并后是否删除原文件";
            this.ckIsDelete.UseVisualStyleBackColor = true;
            // 
            // btnCombine
            // 
            this.btnCombine.Location = new System.Drawing.Point(623, 141);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(164, 56);
            this.btnCombine.TabIndex = 13;
            this.btnCombine.Text = "② 合并处理文件";
            this.btnCombine.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(51, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(494, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "提示：选择发包方名称的根目录，可选择村级别或组级别目录";
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(623, 64);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(162, 40);
            this.btnSetPath.TabIndex = 11;
            this.btnSetPath.Text = "① 选择操作路径";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // txtCombinePath
            // 
            this.txtCombinePath.Location = new System.Drawing.Point(26, 64);
            this.txtCombinePath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCombinePath.Multiline = true;
            this.txtCombinePath.Name = "txtCombinePath";
            this.txtCombinePath.Size = new System.Drawing.Size(558, 52);
            this.txtCombinePath.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 317);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "合并工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ckIsDelete;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.TextBox txtCombinePath;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

