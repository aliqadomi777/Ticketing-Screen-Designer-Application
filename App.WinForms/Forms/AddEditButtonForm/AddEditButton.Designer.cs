namespace App.WinForms
{
    partial class AddEditButton
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
            this.ButtonNameEnLabel = new System.Windows.Forms.Label();
            this.ButtonNameArLabel = new System.Windows.Forms.Label();
            this.ButtonActionLabel = new System.Windows.Forms.Label();
            this.ButtonNameEnTextBox = new System.Windows.Forms.TextBox();
            this.ButtonNameArTextBox = new System.Windows.Forms.TextBox();
            this.ButtonActionList = new System.Windows.Forms.ComboBox();
            this.buttonDetailsLayout = new System.Windows.Forms.TableLayoutPanel();
            this.ArMessageTextBox = new System.Windows.Forms.TextBox();
            this.ArMessageLabel = new System.Windows.Forms.Label();
            this.EnMessageTextBox = new System.Windows.Forms.TextBox();
            this.EnMessageLabel = new System.Windows.Forms.Label();
            this.ServiceList = new System.Windows.Forms.ComboBox();
            this.ServiceLabel = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.buttonDetailsLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonNameEnLabel
            // 
            this.ButtonNameEnLabel.AutoSize = true;
            this.ButtonNameEnLabel.Location = new System.Drawing.Point(12, 30);
            this.ButtonNameEnLabel.Name = "ButtonNameEnLabel";
            this.ButtonNameEnLabel.Size = new System.Drawing.Size(157, 25);
            this.ButtonNameEnLabel.TabIndex = 0;
            this.ButtonNameEnLabel.Text = "Button Name EN";
            // 
            // ButtonNameArLabel
            // 
            this.ButtonNameArLabel.AutoSize = true;
            this.ButtonNameArLabel.Location = new System.Drawing.Point(12, 83);
            this.ButtonNameArLabel.Name = "ButtonNameArLabel";
            this.ButtonNameArLabel.Size = new System.Drawing.Size(157, 25);
            this.ButtonNameArLabel.TabIndex = 1;
            this.ButtonNameArLabel.Text = "Button Name AR";
            // 
            // ButtonActionLabel
            // 
            this.ButtonActionLabel.AutoSize = true;
            this.ButtonActionLabel.Location = new System.Drawing.Point(12, 142);
            this.ButtonActionLabel.Name = "ButtonActionLabel";
            this.ButtonActionLabel.Size = new System.Drawing.Size(128, 25);
            this.ButtonActionLabel.TabIndex = 2;
            this.ButtonActionLabel.Text = "Button Action";
            // 
            // ButtonNameEnTextBox
            // 
            this.ButtonNameEnTextBox.Location = new System.Drawing.Point(187, 30);
            this.ButtonNameEnTextBox.Name = "ButtonNameEnTextBox";
            this.ButtonNameEnTextBox.Size = new System.Drawing.Size(489, 30);
            this.ButtonNameEnTextBox.TabIndex = 3;
            // 
            // ButtonNameArTextBox
            // 
            this.ButtonNameArTextBox.Location = new System.Drawing.Point(187, 84);
            this.ButtonNameArTextBox.Name = "ButtonNameArTextBox";
            this.ButtonNameArTextBox.Size = new System.Drawing.Size(489, 30);
            this.ButtonNameArTextBox.TabIndex = 4;
            // 
            // ButtonActionList
            // 
            this.ButtonActionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ButtonActionList.FormattingEnabled = true;
            this.ButtonActionList.Location = new System.Drawing.Point(187, 139);
            this.ButtonActionList.Name = "ButtonActionList";
            this.ButtonActionList.Size = new System.Drawing.Size(212, 33);
            this.ButtonActionList.TabIndex = 5;
            this.ButtonActionList.SelectedIndexChanged += new System.EventHandler(this.ButtonActionList_SelectedIndexChanged);
            // 
            // buttonDetailsLayout
            // 
            this.buttonDetailsLayout.ColumnCount = 2;
            this.buttonDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.92199F));
            this.buttonDetailsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.07801F));
            this.buttonDetailsLayout.Controls.Add(this.ArMessageTextBox, 1, 2);
            this.buttonDetailsLayout.Controls.Add(this.ArMessageLabel, 0, 2);
            this.buttonDetailsLayout.Controls.Add(this.EnMessageTextBox, 1, 1);
            this.buttonDetailsLayout.Controls.Add(this.EnMessageLabel, 0, 1);
            this.buttonDetailsLayout.Controls.Add(this.ServiceList, 1, 0);
            this.buttonDetailsLayout.Controls.Add(this.ServiceLabel, 0, 0);
            this.buttonDetailsLayout.Location = new System.Drawing.Point(7, 205);
            this.buttonDetailsLayout.Name = "buttonDetailsLayout";
            this.buttonDetailsLayout.RowCount = 3;
            this.buttonDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.buttonDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53F));
            this.buttonDetailsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.buttonDetailsLayout.Size = new System.Drawing.Size(846, 149);
            this.buttonDetailsLayout.TabIndex = 6;
            // 
            // ArMessageTextBox
            // 
            this.ArMessageTextBox.Location = new System.Drawing.Point(180, 94);
            this.ArMessageTextBox.Name = "ArMessageTextBox";
            this.ArMessageTextBox.Size = new System.Drawing.Size(662, 30);
            this.ArMessageTextBox.TabIndex = 11;
            // 
            // ArMessageLabel
            // 
            this.ArMessageLabel.AutoSize = true;
            this.ArMessageLabel.Location = new System.Drawing.Point(3, 91);
            this.ArMessageLabel.Name = "ArMessageLabel";
            this.ArMessageLabel.Size = new System.Drawing.Size(124, 25);
            this.ArMessageLabel.TabIndex = 10;
            this.ArMessageLabel.Text = "AR message";
            // 
            // EnMessageTextBox
            // 
            this.EnMessageTextBox.Location = new System.Drawing.Point(180, 41);
            this.EnMessageTextBox.Name = "EnMessageTextBox";
            this.EnMessageTextBox.Size = new System.Drawing.Size(662, 30);
            this.EnMessageTextBox.TabIndex = 7;
            // 
            // EnMessageLabel
            // 
            this.EnMessageLabel.AutoSize = true;
            this.EnMessageLabel.Location = new System.Drawing.Point(3, 38);
            this.EnMessageLabel.Name = "EnMessageLabel";
            this.EnMessageLabel.Size = new System.Drawing.Size(124, 25);
            this.EnMessageLabel.TabIndex = 9;
            this.EnMessageLabel.Text = "EN message";
            // 
            // ServiceList
            // 
            this.ServiceList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ServiceList.FormattingEnabled = true;
            this.ServiceList.Location = new System.Drawing.Point(180, 3);
            this.ServiceList.Name = "ServiceList";
            this.ServiceList.Size = new System.Drawing.Size(281, 33);
            this.ServiceList.TabIndex = 8;
            // 
            // ServiceLabel
            // 
            this.ServiceLabel.AutoSize = true;
            this.ServiceLabel.Location = new System.Drawing.Point(3, 0);
            this.ServiceLabel.Name = "ServiceLabel";
            this.ServiceLabel.Size = new System.Drawing.Size(135, 25);
            this.ServiceLabel.TabIndex = 7;
            this.ServiceLabel.Text = "Service Name";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(569, 380);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(128, 36);
            this.SaveButton.TabIndex = 15;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(721, 380);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(128, 36);
            this.CancelButton.TabIndex = 14;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // AddEditButton
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(863, 432);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.buttonDetailsLayout);
            this.Controls.Add(this.ButtonActionList);
            this.Controls.Add(this.ButtonNameArTextBox);
            this.Controls.Add(this.ButtonNameEnTextBox);
            this.Controls.Add(this.ButtonActionLabel);
            this.Controls.Add(this.ButtonNameArLabel);
            this.Controls.Add(this.ButtonNameEnLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddEditButton";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddEditButton";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddEditButton_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddEditButton_FormClosed);
            this.Load += new System.EventHandler(this.AddEditButton_Load);
            this.buttonDetailsLayout.ResumeLayout(false);
            this.buttonDetailsLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ButtonNameEnLabel;
        private System.Windows.Forms.Label ButtonNameArLabel;
        private System.Windows.Forms.Label ButtonActionLabel;
        private System.Windows.Forms.TextBox ButtonNameEnTextBox;
        private System.Windows.Forms.TextBox ButtonNameArTextBox;
        private System.Windows.Forms.ComboBox ButtonActionList;
        private System.Windows.Forms.TableLayoutPanel buttonDetailsLayout;
        private System.Windows.Forms.ComboBox ServiceList;
        private System.Windows.Forms.Label ServiceLabel;
        private System.Windows.Forms.TextBox ArMessageTextBox;
        private System.Windows.Forms.Label ArMessageLabel;
        private System.Windows.Forms.TextBox EnMessageTextBox;
        private System.Windows.Forms.Label EnMessageLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton;
    }
}