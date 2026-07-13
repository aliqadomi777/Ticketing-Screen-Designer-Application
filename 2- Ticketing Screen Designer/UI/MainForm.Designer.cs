namespace _2__Ticketing_Screen_Designer.UI
{
    partial class MainForm
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.screenList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.TitleLabel.Location = new System.Drawing.Point(199, 24);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(165, 31);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Main Form - ";
            this.TitleLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 43);
            this.button1.TabIndex = 1;
            this.button1.Text = "Add Screen";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(321, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(129, 43);
            this.button2.TabIndex = 2;
            this.button2.Text = "Delete Screen";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(164, 108);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(129, 43);
            this.button3.TabIndex = 3;
            this.button3.Text = "Edit Screen";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // screenList
            // 
            this.screenList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.screenList.FormattingEnabled = true;
            this.screenList.ItemHeight = 25;
            this.screenList.Location = new System.Drawing.Point(12, 223);
            this.screenList.Name = "screenList";
            this.screenList.Size = new System.Drawing.Size(758, 429);
            this.screenList.TabIndex = 4;
            this.screenList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 675);
            this.Controls.Add(this.screenList);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.TitleLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListBox screenList;
    }
}