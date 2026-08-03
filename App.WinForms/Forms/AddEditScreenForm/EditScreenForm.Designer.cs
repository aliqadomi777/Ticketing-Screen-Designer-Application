namespace App.WinForms
{
    partial class EditScreenForm
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
            this.ScreenNameLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.ScreenNameTextBox = new System.Windows.Forms.TextBox();
            this.ActivateButton = new System.Windows.Forms.RadioButton();
            this.DeactivateButton = new System.Windows.Forms.RadioButton();
            this.ButtonsList = new System.Windows.Forms.ListBox();
            this.ButtonLabel = new System.Windows.Forms.Label();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ScreenNameLabel
            // 
            this.ScreenNameLabel.AutoSize = true;
            this.ScreenNameLabel.Location = new System.Drawing.Point(13, 42);
            this.ScreenNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ScreenNameLabel.Name = "ScreenNameLabel";
            this.ScreenNameLabel.Size = new System.Drawing.Size(64, 25);
            this.ScreenNameLabel.TabIndex = 0;
            this.ScreenNameLabel.Text = "Name";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(13, 106);
            this.StatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(68, 25);
            this.StatusLabel.TabIndex = 1;
            this.StatusLabel.Text = "Status";
            // 
            // ScreenNameTextBox
            // 
            this.ScreenNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ScreenNameTextBox.Location = new System.Drawing.Point(102, 40);
            this.ScreenNameTextBox.Name = "ScreenNameTextBox";
            this.ScreenNameTextBox.Size = new System.Drawing.Size(718, 30);
            this.ScreenNameTextBox.TabIndex = 2;
            // 
            // ActivateButton
            // 
            this.ActivateButton.AutoSize = true;
            this.ActivateButton.Location = new System.Drawing.Point(102, 104);
            this.ActivateButton.Name = "ActivateButton";
            this.ActivateButton.Size = new System.Drawing.Size(103, 29);
            this.ActivateButton.TabIndex = 3;
            this.ActivateButton.TabStop = true;
            this.ActivateButton.Text = "Activate";
            this.ActivateButton.UseVisualStyleBackColor = true;
            // 
            // DeactivateButton
            // 
            this.DeactivateButton.AutoSize = true;
            this.DeactivateButton.Location = new System.Drawing.Point(251, 104);
            this.DeactivateButton.Name = "DeactivateButton";
            this.DeactivateButton.Size = new System.Drawing.Size(125, 29);
            this.DeactivateButton.TabIndex = 4;
            this.DeactivateButton.TabStop = true;
            this.DeactivateButton.Text = "Deactivate";
            this.DeactivateButton.UseVisualStyleBackColor = true;
            // 
            // ButtonsList
            // 
            this.ButtonsList.FormattingEnabled = true;
            this.ButtonsList.ItemHeight = 25;
            this.ButtonsList.Location = new System.Drawing.Point(12, 275);
            this.ButtonsList.Name = "ButtonsList";
            this.ButtonsList.Size = new System.Drawing.Size(900, 379);
            this.ButtonsList.TabIndex = 5;
            // 
            // ButtonLabel
            // 
            this.ButtonLabel.AutoSize = true;
            this.ButtonLabel.Location = new System.Drawing.Point(419, 241);
            this.ButtonLabel.Name = "ButtonLabel";
            this.ButtonLabel.Size = new System.Drawing.Size(78, 25);
            this.ButtonLabel.TabIndex = 6;
            this.ButtonLabel.Text = "Buttons";
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(200, 177);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(154, 53);
            this.EditButton.TabIndex = 9;
            this.EditButton.Text = "Edit Button";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(378, 177);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(154, 53);
            this.DeleteButton.TabIndex = 8;
            this.DeleteButton.Text = "Delete Button";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(18, 177);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(154, 53);
            this.AddButton.TabIndex = 7;
            this.AddButton.Text = "Add Button";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(632, 669);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(128, 36);
            this.SaveButton.TabIndex = 13;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(784, 669);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(128, 36);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(757, 177);
            this.RefreshButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(154, 53);
            this.RefreshButton.TabIndex = 14;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // EditScreenForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(924, 717);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.ButtonLabel);
            this.Controls.Add(this.ButtonsList);
            this.Controls.Add(this.DeactivateButton);
            this.Controls.Add(this.ActivateButton);
            this.Controls.Add(this.ScreenNameTextBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.ScreenNameLabel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "EditScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AddEditScreen";
            this.Load += new System.EventHandler(this.EditScreenForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ScreenNameLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox ScreenNameTextBox;
        private System.Windows.Forms.RadioButton ActivateButton;
        private System.Windows.Forms.RadioButton DeactivateButton;
        private System.Windows.Forms.ListBox ButtonsList;
        private System.Windows.Forms.Label ButtonLabel;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button RefreshButton;
    }
}