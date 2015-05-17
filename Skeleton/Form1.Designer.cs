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
            this.buttonChangeType = new System.Windows.Forms.Button();
            this.boneNameENG = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.resetPositionButton = new System.Windows.Forms.Button();
            this.hideOthersButton = new System.Windows.Forms.Button();
            this.wikipediaButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.display = new System.Windows.Forms.TextBox();
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
            // buttonChangeType
            // 
            this.buttonChangeType.Location = new System.Drawing.Point(15, 87);
            this.buttonChangeType.Name = "buttonChangeType";
            this.buttonChangeType.Size = new System.Drawing.Size(100, 24);
            this.buttonChangeType.TabIndex = 2;
            this.buttonChangeType.Text = "ChangeType";
            this.buttonChangeType.UseVisualStyleBackColor = true;
            this.buttonChangeType.Click += new System.EventHandler(this.button2_Click);
            // 
            // boneNameENG
            // 
            this.boneNameENG.Location = new System.Drawing.Point(15, 229);
            this.boneNameENG.Name = "boneNameENG";
            this.boneNameENG.Size = new System.Drawing.Size(100, 20);
            this.boneNameENG.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.resetPositionButton);
            this.panel1.Controls.Add(this.hideOthersButton);
            this.panel1.Controls.Add(this.wikipediaButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.display);
            this.panel1.Controls.Add(this.boneNamePL);
            this.panel1.Controls.Add(this.boneNameENG);
            this.panel1.Controls.Add(this.buttonChangeType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(657, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(127, 564);
            this.panel1.TabIndex = 4;
            // 
            // resetPositionButton
            // 
            this.resetPositionButton.Location = new System.Drawing.Point(15, 428);
            this.resetPositionButton.Name = "resetPositionButton";
            this.resetPositionButton.Size = new System.Drawing.Size(100, 23);
            this.resetPositionButton.TabIndex = 10;
            this.resetPositionButton.Text = "Reset Position";
            this.resetPositionButton.UseVisualStyleBackColor = true;
            this.resetPositionButton.Click += new System.EventHandler(this.resetPositionButton_Click);
            // 
            // hideOthersButton
            // 
            this.hideOthersButton.Location = new System.Drawing.Point(15, 399);
            this.hideOthersButton.Name = "hideOthersButton";
            this.hideOthersButton.Size = new System.Drawing.Size(100, 23);
            this.hideOthersButton.TabIndex = 9;
            this.hideOthersButton.Text = "Hide Others";
            this.hideOthersButton.UseVisualStyleBackColor = true;
            this.hideOthersButton.Click += new System.EventHandler(this.hideOthersButton_Click);
            // 
            // wikipediaButton
            // 
            this.wikipediaButton.Location = new System.Drawing.Point(15, 281);
            this.wikipediaButton.Name = "wikipediaButton";
            this.wikipediaButton.Size = new System.Drawing.Size(100, 24);
            this.wikipediaButton.TabIndex = 8;
            this.wikipediaButton.Text = "Wikipedia";
            this.wikipediaButton.UseVisualStyleBackColor = true;
            this.wikipediaButton.Click += new System.EventHandler(this.wikipediaButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "bone name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "display type:";
            // 
            // display
            // 
            this.display.Enabled = false;
            this.display.Location = new System.Drawing.Point(15, 61);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(100, 20);
            this.display.TabIndex = 5;
            // 
            // boneNamePL
            // 
            this.boneNamePL.Location = new System.Drawing.Point(15, 255);
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
        private System.Windows.Forms.Button buttonChangeType;
        private System.Windows.Forms.TextBox boneNameENG;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox boneNamePL;
        private System.Windows.Forms.Button resetPositionButton;
        private System.Windows.Forms.Button hideOthersButton;
        private System.Windows.Forms.Button wikipediaButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox display;
    }
}