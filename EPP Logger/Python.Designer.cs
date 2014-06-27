namespace EPP_Logger
{
    partial class Python_Window
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
            this.python_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // python_TextBox
            // 
            this.python_TextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.python_TextBox.Location = new System.Drawing.Point(12, 12);
            this.python_TextBox.Multiline = true;
            this.python_TextBox.Name = "python_TextBox";
            this.python_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.python_TextBox.Size = new System.Drawing.Size(1473, 390);
            this.python_TextBox.TabIndex = 0;
            // 
            // Python_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1508, 414);
            this.Controls.Add(this.python_TextBox);
            this.Name = "Python_Window";
            this.Text = "Python Simulator";
            this.Load += new System.EventHandler(this.Python_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox python_TextBox;
    }
}