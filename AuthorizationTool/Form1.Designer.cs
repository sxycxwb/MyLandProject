namespace AuthorizationTool
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
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.txtCombinePath = new System.Windows.Forms.TextBox();
            this.txtGroupInfo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartAuth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(29, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(185, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "提示：选择 待授权工具 的文件夹";
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(410, 64);
            this.btnSetPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(108, 27);
            this.btnSetPath.TabIndex = 14;
            this.btnSetPath.Text = "① 选择操作路径";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // txtCombinePath
            // 
            this.txtCombinePath.Location = new System.Drawing.Point(12, 64);
            this.txtCombinePath.Multiline = true;
            this.txtCombinePath.Name = "txtCombinePath";
            this.txtCombinePath.Size = new System.Drawing.Size(373, 36);
            this.txtCombinePath.TabIndex = 13;
            // 
            // txtGroupInfo
            // 
            this.txtGroupInfo.Location = new System.Drawing.Point(11, 145);
            this.txtGroupInfo.Margin = new System.Windows.Forms.Padding(2);
            this.txtGroupInfo.Multiline = true;
            this.txtGroupInfo.Name = "txtGroupInfo";
            this.txtGroupInfo.Size = new System.Drawing.Size(474, 150);
            this.txtGroupInfo.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(29, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(437, 12);
            this.label1.TabIndex = 30;
            this.label1.Text = "提示：复制或填写要授权的发包方编码，每个发包方编码占一行(中间允许有空行)";
            // 
            // btnStartAuth
            // 
            this.btnStartAuth.Location = new System.Drawing.Point(11, 315);
            this.btnStartAuth.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartAuth.Name = "btnStartAuth";
            this.btnStartAuth.Size = new System.Drawing.Size(122, 39);
            this.btnStartAuth.TabIndex = 31;
            this.btnStartAuth.Text = "② 开始授权";
            this.btnStartAuth.UseVisualStyleBackColor = true;
            this.btnStartAuth.Click += new System.EventHandler(this.btnStartAuth_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 365);
            this.Controls.Add(this.btnStartAuth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGroupInfo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnSetPath);
            this.Controls.Add(this.txtCombinePath);
            this.Name = "Form1";
            this.Text = "授权工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.TextBox txtCombinePath;
        private System.Windows.Forms.TextBox txtGroupInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartAuth;
    }
}

