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
            this.AddScreenButton = new System.Windows.Forms.Button();
            this.DeleteScreenButton = new System.Windows.Forms.Button();
            this.EditScreenButton = new System.Windows.Forms.Button();
            this.screenList = new System.Windows.Forms.ListBox();
            this.ScreenTitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.TitleLabel.Location = new System.Drawing.Point(373, 48);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(165, 31);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Main Form - ";
            this.TitleLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // AddScreenButton
            // 
            this.AddScreenButton.Location = new System.Drawing.Point(13, 148);
            this.AddScreenButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AddScreenButton.Name = "AddScreenButton";
            this.AddScreenButton.Size = new System.Drawing.Size(143, 49);
            this.AddScreenButton.TabIndex = 1;
            this.AddScreenButton.Text = "Add Screen";
            this.AddScreenButton.UseVisualStyleBackColor = true;
            this.AddScreenButton.Click += new System.EventHandler(this.AddScreenButton_Click);
            // 
            // DeleteScreenButton
            // 
            this.DeleteScreenButton.Location = new System.Drawing.Point(175, 148);
            this.DeleteScreenButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DeleteScreenButton.Name = "DeleteScreenButton";
            this.DeleteScreenButton.Size = new System.Drawing.Size(152, 49);
            this.DeleteScreenButton.TabIndex = 2;
            this.DeleteScreenButton.Text = "Delete Screen";
            this.DeleteScreenButton.UseVisualStyleBackColor = true;
            this.DeleteScreenButton.Click += new System.EventHandler(this.DeleteScreenButton_Click);
            // 
            // EditScreenButton
            // 
            this.EditScreenButton.Location = new System.Drawing.Point(349, 148);
            this.EditScreenButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EditScreenButton.Name = "EditScreenButton";
            this.EditScreenButton.Size = new System.Drawing.Size(152, 49);
            this.EditScreenButton.TabIndex = 3;
            this.EditScreenButton.Text = "Edit Screen";
            this.EditScreenButton.UseVisualStyleBackColor = true;
            this.EditScreenButton.Click += new System.EventHandler(this.EditScreenButton_Click);
            // 
            // screenList
            // 
            this.screenList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screenList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.screenList.FormattingEnabled = true;
            this.screenList.ItemHeight = 25;
            this.screenList.Location = new System.Drawing.Point(13, 294);
            this.screenList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.screenList.Name = "screenList";
            this.screenList.Size = new System.Drawing.Size(894, 352);
            this.screenList.TabIndex = 4;
            this.screenList.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // ScreenTitleLabel
            // 
            this.ScreenTitleLabel.AutoSize = true;
            this.ScreenTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.ScreenTitleLabel.Location = new System.Drawing.Point(417, 243);
            this.ScreenTitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ScreenTitleLabel.Name = "ScreenTitleLabel";
            this.ScreenTitleLabel.Size = new System.Drawing.Size(102, 29);
            this.ScreenTitleLabel.TabIndex = 5;
            this.ScreenTitleLabel.Text = "Screens";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(920, 675);
            this.Controls.Add(this.ScreenTitleLabel);
            this.Controls.Add(this.screenList);
            this.Controls.Add(this.EditScreenButton);
            this.Controls.Add(this.DeleteScreenButton);
            this.Controls.Add(this.AddScreenButton);
            this.Controls.Add(this.TitleLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Click += new System.EventHandler(this.MainForm_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Button AddScreenButton;
        private System.Windows.Forms.Button DeleteScreenButton;
        private System.Windows.Forms.Button EditScreenButton;
        private System.Windows.Forms.ListBox screenList;
        private System.Windows.Forms.Label ScreenTitleLabel;
    }
}