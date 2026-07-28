namespace App.WinForms
{
    partial class LoginForm
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
            this.BankIdTextBox = new System.Windows.Forms.TextBox();
            this.BankIdLabel = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.RegisterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BankIdTextBox
            // 
            this.BankIdTextBox.BackColor = System.Drawing.Color.DarkSlateGray;
            this.BankIdTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.BankIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BankIdTextBox.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.BankIdTextBox.Location = new System.Drawing.Point(187, 78);
            this.BankIdTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.BankIdTextBox.Name = "BankIdTextBox";
            this.BankIdTextBox.Size = new System.Drawing.Size(242, 30);
            this.BankIdTextBox.TabIndex = 0;
            // 
            // BankIdLabel
            // 
            this.BankIdLabel.AutoSize = true;
            this.BankIdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BankIdLabel.Location = new System.Drawing.Point(73, 78);
            this.BankIdLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BankIdLabel.Name = "BankIdLabel";
            this.BankIdLabel.Size = new System.Drawing.Size(81, 25);
            this.BankIdLabel.TabIndex = 1;
            this.BankIdLabel.Text = "Bank ID";
            this.BankIdLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.BankIdLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(187, 151);
            this.LoginButton.Margin = new System.Windows.Forms.Padding(4);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(100, 55);
            this.LoginButton.TabIndex = 2;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(329, 151);
            this.RegisterButton.Margin = new System.Windows.Forms.Padding(4);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(100, 55);
            this.RegisterButton.TabIndex = 3;
            this.RegisterButton.Text = "Register";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(512, 260);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.BankIdLabel);
            this.Controls.Add(this.BankIdTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox BankIdTextBox;
        private System.Windows.Forms.Label BankIdLabel;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Button RegisterButton;
    }
}