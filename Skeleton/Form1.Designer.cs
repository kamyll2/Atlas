namespace Skeleton
{
    partial class GameLoopForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.glControl = new OpenTK.GLControl();
            this.buttonNextModel = new System.Windows.Forms.Button();
            this.buttonChangeType = new System.Windows.Forms.Button();
            this.boneNameENG = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.boneNamePL = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(784, 564);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            this.glControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseClick);
            // 
            // buttonNextModel
            // 
            this.buttonNextModel.Location = new System.Drawing.Point(31, 12);
            this.buttonNextModel.Name = "buttonNextModel";
            this.buttonNextModel.Size = new System.Drawing.Size(75, 23);
            this.buttonNextModel.TabIndex = 1;
            this.buttonNextModel.Text = "NextModel";
            this.buttonNextModel.UseVisualStyleBackColor = true;
            this.buttonNextModel.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonChangeType
            // 
            this.buttonChangeType.Location = new System.Drawing.Point(31, 41);
            this.buttonChangeType.Name = "buttonChangeType";
            this.buttonChangeType.Size = new System.Drawing.Size(75, 23);
            this.buttonChangeType.TabIndex = 2;
            this.buttonChangeType.Text = "ChangeType";
            this.buttonChangeType.UseVisualStyleBackColor = true;
            this.buttonChangeType.Click += new System.EventHandler(this.button2_Click);
            // 
            // boneNameENG
            // 
            this.boneNameENG.Location = new System.Drawing.Point(15, 70);
            this.boneNameENG.Name = "boneNameENG";
            this.boneNameENG.Size = new System.Drawing.Size(100, 20);
            this.boneNameENG.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.boneNamePL);
            this.panel1.Controls.Add(this.buttonNextModel);
            this.panel1.Controls.Add(this.boneNameENG);
            this.panel1.Controls.Add(this.buttonChangeType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(657, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 564);
            this.panel1.TabIndex = 4;
            // 
            // boneNamePL
            // 
            this.boneNamePL.Location = new System.Drawing.Point(15, 162);
            this.boneNamePL.Name = "boneNamePL";
            this.boneNamePL.Size = new System.Drawing.Size(100, 20);
            this.boneNamePL.TabIndex = 4;
            // 
            // GameLoopForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 564);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.glControl);
            this.Name = "GameLoopForm";
            this.Text = "Cube";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl glControl;
        private System.Windows.Forms.Button buttonNextModel;
        private System.Windows.Forms.Button buttonChangeType;
        private System.Windows.Forms.TextBox boneNameENG;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox boneNamePL;
    }
}