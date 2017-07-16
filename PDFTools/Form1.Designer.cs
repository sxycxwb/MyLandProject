namespace PDFTools
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
            this.btnSetWordPath = new System.Windows.Forms.Button();
            this.txtWorkPath = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSelectPlotCode = new System.Windows.Forms.Button();
            this.txtPlotPath = new System.Windows.Forms.TextBox();
            this.txtErrorMsg = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOpenGenerateTools = new System.Windows.Forms.Button();
            this.btnQueryHistory = new System.Windows.Forms.Button();
            this.btnAddGroup = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.GroupName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBFMaxCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delete = new System.Windows.Forms.DataGridViewLinkColumn();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGroupNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCBFBigCode = new System.Windows.Forms.TextBox();
            this.txtFBFCode = new System.Windows.Forms.TextBox();
            this.btnMakeDir = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCountryName = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ckIsDelete = new System.Windows.Forms.CheckBox();
            this.btnCombine = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSetPath = new System.Windows.Forms.Button();
            this.txtCombinePath = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetWordPath
            // 
            this.btnSetWordPath.Location = new System.Drawing.Point(291, 24);
            this.btnSetWordPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetWordPath.Name = "btnSetWordPath";
            this.btnSetWordPath.Size = new System.Drawing.Size(108, 27);
            this.btnSetWordPath.TabIndex = 10;
            this.btnSetWordPath.Text = "① 设置生成路径";
            this.btnSetWordPath.UseVisualStyleBackColor = true;
            this.btnSetWordPath.Click += new System.EventHandler(this.btnSetWordPath_Click);
            // 
            // txtWorkPath
            // 
            this.txtWorkPath.Location = new System.Drawing.Point(38, 21);
            this.txtWorkPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtWorkPath.Multiline = true;
            this.txtWorkPath.Name = "txtWorkPath";
            this.txtWorkPath.Size = new System.Drawing.Size(248, 33);
            this.txtWorkPath.TabIndex = 9;
            this.txtWorkPath.Text = "D:\\扫描文件";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnSelectPlotCode);
            this.groupBox1.Controls.Add(this.txtPlotPath);
            this.groupBox1.Controls.Add(this.txtErrorMsg);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.txtFBFCode);
            this.groupBox1.Controls.Add(this.btnMakeDir);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtCountryName);
            this.groupBox1.Controls.Add(this.txtWorkPath);
            this.groupBox1.Controls.Add(this.btnSetWordPath);
            this.groupBox1.Location = new System.Drawing.Point(14, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(770, 465);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1.生成文件夹";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("方正兰亭超细黑简体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.DarkRed;
            this.label8.Location = new System.Drawing.Point(628, 401);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(132, 18);
            this.label8.TabIndex = 27;
            this.label8.Text = "← 错误消息提示";
            // 
            // btnSelectPlotCode
            // 
            this.btnSelectPlotCode.Location = new System.Drawing.Point(470, 314);
            this.btnSelectPlotCode.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectPlotCode.Name = "btnSelectPlotCode";
            this.btnSelectPlotCode.Size = new System.Drawing.Size(153, 29);
            this.btnSelectPlotCode.TabIndex = 26;
            this.btnSelectPlotCode.Text = "③ 选择界址点成果表目录";
            this.btnSelectPlotCode.UseVisualStyleBackColor = true;
            this.btnSelectPlotCode.Click += new System.EventHandler(this.btnSelectPlotCode_Click);
            // 
            // txtPlotPath
            // 
            this.txtPlotPath.Location = new System.Drawing.Point(38, 319);
            this.txtPlotPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtPlotPath.Name = "txtPlotPath";
            this.txtPlotPath.Size = new System.Drawing.Size(391, 21);
            this.txtPlotPath.TabIndex = 20;
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.Location = new System.Drawing.Point(27, 354);
            this.txtErrorMsg.Margin = new System.Windows.Forms.Padding(2);
            this.txtErrorMsg.Multiline = true;
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.Size = new System.Drawing.Size(596, 103);
            this.txtErrorMsg.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(646, 49);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(101, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "(12位数字到村级)";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.groupBox3.Controls.Add(this.btnOpenGenerateTools);
            this.groupBox3.Controls.Add(this.btnQueryHistory);
            this.groupBox3.Controls.Add(this.btnAddGroup);
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.txtGroupName);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtGroupNum);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCBFBigCode);
            this.groupBox3.Location = new System.Drawing.Point(8, 75);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(754, 237);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "组操作";
            // 
            // btnOpenGenerateTools
            // 
            this.btnOpenGenerateTools.Location = new System.Drawing.Point(633, 188);
            this.btnOpenGenerateTools.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenGenerateTools.Name = "btnOpenGenerateTools";
            this.btnOpenGenerateTools.Size = new System.Drawing.Size(107, 41);
            this.btnOpenGenerateTools.TabIndex = 27;
            this.btnOpenGenerateTools.Text = "界址点面积精度评价表生成工具";
            this.btnOpenGenerateTools.UseVisualStyleBackColor = true;
            this.btnOpenGenerateTools.Click += new System.EventHandler(this.btnOpenGenerateTools_Click);
            // 
            // btnQueryHistory
            // 
            this.btnQueryHistory.Location = new System.Drawing.Point(631, 48);
            this.btnQueryHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnQueryHistory.Name = "btnQueryHistory";
            this.btnQueryHistory.Size = new System.Drawing.Size(108, 24);
            this.btnQueryHistory.TabIndex = 26;
            this.btnQueryHistory.Text = "软件使用情况";
            this.btnQueryHistory.UseVisualStyleBackColor = true;
            this.btnQueryHistory.Visible = false;
            this.btnQueryHistory.Click += new System.EventHandler(this.btnQueryHistory_Click);
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.Location = new System.Drawing.Point(631, 12);
            this.btnAddGroup.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(108, 24);
            this.btnAddGroup.TabIndex = 25;
            this.btnAddGroup.Text = "② 添加组信息";
            this.btnAddGroup.UseVisualStyleBackColor = true;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupName,
            this.GroupNum,
            this.CBFMaxCode,
            this.delete});
            this.dataGridView1.Location = new System.Drawing.Point(17, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(598, 181);
            this.dataGridView1.TabIndex = 24;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // GroupName
            // 
            this.GroupName.HeaderText = "组名称";
            this.GroupName.Name = "GroupName";
            this.GroupName.Width = 280;
            // 
            // GroupNum
            // 
            this.GroupNum.HeaderText = "组号";
            this.GroupNum.Name = "GroupNum";
            this.GroupNum.Width = 90;
            // 
            // CBFMaxCode
            // 
            this.CBFMaxCode.HeaderText = "承包方最大编码";
            this.CBFMaxCode.Name = "CBFMaxCode";
            this.CBFMaxCode.Width = 120;
            // 
            // delete
            // 
            this.delete.HeaderText = "删除";
            this.delete.Name = "delete";
            this.delete.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.delete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.delete.Width = 80;
            // 
            // txtGroupName
            // 
            this.txtGroupName.Location = new System.Drawing.Point(72, 17);
            this.txtGroupName.Margin = new System.Windows.Forms.Padding(2);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(206, 21);
            this.txtGroupName.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "组名称：";
            // 
            // txtGroupNum
            // 
            this.txtGroupNum.Location = new System.Drawing.Point(352, 16);
            this.txtGroupNum.Margin = new System.Windows.Forms.Padding(2);
            this.txtGroupNum.Name = "txtGroupNum";
            this.txtGroupNum.Size = new System.Drawing.Size(53, 21);
            this.txtGroupNum.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(431, 19);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "承包方最大编码：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "组号：";
            // 
            // txtCBFBigCode
            // 
            this.txtCBFBigCode.Location = new System.Drawing.Point(533, 14);
            this.txtCBFBigCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtCBFBigCode.Name = "txtCBFBigCode";
            this.txtCBFBigCode.Size = new System.Drawing.Size(84, 21);
            this.txtCBFBigCode.TabIndex = 21;
            // 
            // txtFBFCode
            // 
            this.txtFBFCode.Location = new System.Drawing.Point(497, 46);
            this.txtFBFCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtFBFCode.Name = "txtFBFCode";
            this.txtFBFCode.Size = new System.Drawing.Size(145, 21);
            this.txtFBFCode.TabIndex = 14;
            // 
            // btnMakeDir
            // 
            this.btnMakeDir.Location = new System.Drawing.Point(648, 429);
            this.btnMakeDir.Margin = new System.Windows.Forms.Padding(2);
            this.btnMakeDir.Name = "btnMakeDir";
            this.btnMakeDir.Size = new System.Drawing.Size(108, 28);
            this.btnMakeDir.TabIndex = 23;
            this.btnMakeDir.Text = "④ 生成文件夹";
            this.btnMakeDir.UseVisualStyleBackColor = true;
            this.btnMakeDir.Click += new System.EventHandler(this.btnMakeDir_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(418, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "发包方代码：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(444, 21);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "村名称：";
            // 
            // txtCountryName
            // 
            this.txtCountryName.Location = new System.Drawing.Point(497, 18);
            this.txtCountryName.Margin = new System.Windows.Forms.Padding(2);
            this.txtCountryName.Name = "txtCountryName";
            this.txtCountryName.Size = new System.Drawing.Size(235, 21);
            this.txtCountryName.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.groupBox2.Controls.Add(this.ckIsDelete);
            this.groupBox2.Controls.Add(this.btnCombine);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnSetPath);
            this.groupBox2.Controls.Add(this.txtCombinePath);
            this.groupBox2.Location = new System.Drawing.Point(14, 477);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(770, 145);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2.合并处理pdf文件";
            // 
            // ckIsDelete
            // 
            this.ckIsDelete.AutoSize = true;
            this.ckIsDelete.Checked = true;
            this.ckIsDelete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckIsDelete.Location = new System.Drawing.Point(473, 108);
            this.ckIsDelete.Name = "ckIsDelete";
            this.ckIsDelete.Size = new System.Drawing.Size(144, 16);
            this.ckIsDelete.TabIndex = 14;
            this.ckIsDelete.Text = "合并后是否删除原文件";
            this.ckIsDelete.UseVisualStyleBackColor = true;
            // 
            // btnCombine
            // 
            this.btnCombine.Location = new System.Drawing.Point(639, 97);
            this.btnCombine.Margin = new System.Windows.Forms.Padding(2);
            this.btnCombine.Name = "btnCombine";
            this.btnCombine.Size = new System.Drawing.Size(109, 37);
            this.btnCombine.TabIndex = 13;
            this.btnCombine.Text = "② 合并处理文件";
            this.btnCombine.UseVisualStyleBackColor = true;
            this.btnCombine.Click += new System.EventHandler(this.btnCombine_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(95, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(329, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "提示：选择发包方名称的根目录，可选择村级别或组级别目录";
            // 
            // btnSetPath
            // 
            this.btnSetPath.Location = new System.Drawing.Point(639, 46);
            this.btnSetPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetPath.Name = "btnSetPath";
            this.btnSetPath.Size = new System.Drawing.Size(108, 27);
            this.btnSetPath.TabIndex = 11;
            this.btnSetPath.Text = "① 选择操作路径";
            this.btnSetPath.UseVisualStyleBackColor = true;
            this.btnSetPath.Click += new System.EventHandler(this.btnSetPath_Click);
            // 
            // txtCombinePath
            // 
            this.txtCombinePath.Location = new System.Drawing.Point(38, 43);
            this.txtCombinePath.Multiline = true;
            this.txtCombinePath.Name = "txtCombinePath";
            this.txtCombinePath.Size = new System.Drawing.Size(579, 36);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 631);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "扫描文件处理工具";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSetWordPath;
        private System.Windows.Forms.TextBox txtWorkPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnSetPath;
        private System.Windows.Forms.TextBox txtCombinePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCombine;
        private System.Windows.Forms.CheckBox ckIsDelete;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCountryName;
        private System.Windows.Forms.TextBox txtFBFCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnAddGroup;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMakeDir;
        private System.Windows.Forms.TextBox txtGroupNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCBFBigCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupName;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn CBFMaxCode;
        private System.Windows.Forms.DataGridViewLinkColumn delete;
        private System.Windows.Forms.TextBox txtErrorMsg;
        private System.Windows.Forms.TextBox txtPlotPath;
        private System.Windows.Forms.Button btnSelectPlotCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnQueryHistory;
        private System.Windows.Forms.Button btnOpenGenerateTools;
    }
}

