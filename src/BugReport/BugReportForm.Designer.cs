using BugReport.Common;

namespace BugReport
{
    partial class BugReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BugReportForm));
            this.label13 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelTitle1 = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new BugReport.Common.WaterTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.excpAddress = new System.Windows.Forms.Label();
            this.errorModule = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.timeTick = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(12, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(425, 12);
            this.label13.TabIndex = 26;
            this.label13.Text = "----------------------------------------------------------------------";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(54, 50);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // labelTitle1
            // 
            this.labelTitle1.Font = new System.Drawing.Font("宋体", 10F);
            this.labelTitle1.Location = new System.Drawing.Point(72, 27);
            this.labelTitle1.Name = "labelTitle1";
            this.labelTitle1.Size = new System.Drawing.Size(337, 35);
            this.labelTitle1.TabIndex = 28;
            this.labelTitle1.Text = "BugReport遇到错误，给您带来不便，我们深表歉意。";
            // 
            // labelInfo
            // 
            this.labelInfo.Font = new System.Drawing.Font("宋体", 10F);
            this.labelInfo.Location = new System.Drawing.Point(12, 87);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(423, 35);
            this.labelInfo.TabIndex = 29;
            this.labelInfo.Text = "我们已经产生了相关错误报告（报告中不包含您的任何隐私信息），希望您发送此报告给我们以帮助改善BugReport的质量。";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(12, 125);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(114, 16);
            this.checkBox1.TabIndex = 30;
            this.checkBox1.Text = "发送错误报告(&S)";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 147);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(420, 92);
            this.textBox1.TabIndex = 31;
            this.textBox1.WaterText = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(10, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 32;
            this.label3.Text = "错误偏移量：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(10, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "错误模块路径：";
            // 
            // excpAddress
            // 
            this.excpAddress.AutoSize = true;
            this.excpAddress.ForeColor = System.Drawing.Color.Gray;
            this.excpAddress.Location = new System.Drawing.Point(93, 255);
            this.excpAddress.Name = "excpAddress";
            this.excpAddress.Size = new System.Drawing.Size(0, 12);
            this.excpAddress.TabIndex = 34;
            // 
            // errorModule
            // 
            this.errorModule.ForeColor = System.Drawing.Color.Gray;
            this.errorModule.Location = new System.Drawing.Point(93, 277);
            this.errorModule.Name = "errorModule";
            this.errorModule.Size = new System.Drawing.Size(339, 27);
            this.errorModule.TabIndex = 35;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(10, 313);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(425, 12);
            this.label5.TabIndex = 36;
            this.label5.Text = "----------------------------------------------------------------------";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(342, 328);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 30);
            this.button1.TabIndex = 37;
            this.button1.Text = "确定(&O)";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(12, 336);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(90, 16);
            this.checkBox2.TabIndex = 38;
            this.checkBox2.Text = "重启程序(&R)";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // timeTick
            // 
            this.timeTick.AutoSize = true;
            this.timeTick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.timeTick.ForeColor = System.Drawing.Color.Red;
            this.timeTick.Location = new System.Drawing.Point(409, 337);
            this.timeTick.Name = "timeTick";
            this.timeTick.Size = new System.Drawing.Size(0, 12);
            this.timeTick.TabIndex = 39;
            // 
            // BugReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 367);
            this.Controls.Add(this.timeTick);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.errorModule);
            this.Controls.Add(this.excpAddress);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.labelTitle1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label13);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BugReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "错误报告";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BugReportForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelTitle1;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.CheckBox checkBox1;
        //private System.Windows.Forms.TextBox textBox1;
        private WaterTextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label excpAddress;
        private System.Windows.Forms.Label errorModule;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label timeTick;
    }
}

