
using System.Drawing;

namespace SolidWorksSecDev
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.btnLinkToAPI = new System.Windows.Forms.Button();
            this.indicatorLightLabel = new System.Windows.Forms.Label();
            this.indicatorTextLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnManageProg = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "生成BOM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLinkToAPI
            // 
            this.btnLinkToAPI.Location = new System.Drawing.Point(423, 33);
            this.btnLinkToAPI.Name = "btnLinkToAPI";
            this.btnLinkToAPI.Size = new System.Drawing.Size(150, 35);
            this.btnLinkToAPI.TabIndex = 2;
            this.btnLinkToAPI.Text = "与SolidWorks链接";
            this.btnLinkToAPI.UseVisualStyleBackColor = true;
            this.btnLinkToAPI.Click += new System.EventHandler(this.btnLinkToAPI_Click);
            // 
            // indicatorLightLabel
            // 
            this.indicatorLightLabel.AutoSize = true;
            this.indicatorLightLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.indicatorLightLabel.ForeColor = System.Drawing.Color.Red;
            this.indicatorLightLabel.Location = new System.Drawing.Point(39, 27);
            this.indicatorLightLabel.Name = "indicatorLightLabel";
            this.indicatorLightLabel.Size = new System.Drawing.Size(37, 39);
            this.indicatorLightLabel.TabIndex = 3;
            this.indicatorLightLabel.Text = "●";
            // 
            // indicatorTextLabel
            // 
            this.indicatorTextLabel.AutoSize = true;
            this.indicatorTextLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.indicatorTextLabel.Location = new System.Drawing.Point(76, 36);
            this.indicatorTextLabel.Name = "indicatorTextLabel";
            this.indicatorTextLabel.Size = new System.Drawing.Size(72, 27);
            this.indicatorTextLabel.TabIndex = 4;
            this.indicatorTextLabel.Text = "未连接";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(766, 320);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "功 能";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(40, 227);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(676, 29);
            this.panel1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Right;
            this.label1.Location = new System.Drawing.Point(623, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(40, 256);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(676, 36);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.Visible = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(140, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(113, 45);
            this.button2.TabIndex = 1;
            this.button2.Text = "反导物料代码";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnManageProg);
            this.groupBox2.Controls.Add(this.indicatorLightLabel);
            this.groupBox2.Controls.Add(this.indicatorTextLabel);
            this.groupBox2.Controls.Add(this.btnLinkToAPI);
            this.groupBox2.Location = new System.Drawing.Point(12, 338);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(766, 91);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "状 态";
            // 
            // btnManageProg
            // 
            this.btnManageProg.Location = new System.Drawing.Point(598, 33);
            this.btnManageProg.Name = "btnManageProg";
            this.btnManageProg.Size = new System.Drawing.Size(150, 35);
            this.btnManageProg.TabIndex = 5;
            this.btnManageProg.Text = "查看运行中的sw";
            this.btnManageProg.UseVisualStyleBackColor = true;
            this.btnManageProg.Click += new System.EventHandler(this.btnManageProg_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "SolidWorks工具箱";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLinkToAPI;
        public System.Windows.Forms.Label indicatorLightLabel;
        public System.Windows.Forms.Label indicatorTextLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnManageProg;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}

