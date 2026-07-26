using App.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.WinForms
{
    public partial class LoginForm : Form
    {
        private readonly IBankService _bankService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;
        public LoginForm(IBankService bankService, IServiceProvider serviceProvider, IUiStateService stateService)
        {
            InitializeComponent();
            _bankService = bankService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;

            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            BankIdLabel.ForeColor = ColorTranslator.FromHtml("#3C3C3C");
            BankIdLabel.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            BankIdTextBox.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            BankIdTextBox.ForeColor = ColorTranslator.FromHtml("#333333");

            LoginButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");

            RegisterButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");

        }

        private void label1_Click(object sender, EventArgs e)
        {
            BankIdTextBox.Focus();
        }



        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                int bankId;
                int.TryParse(BankIdTextBox.Text, out bankId);
                if (bankId > 0)
                {
                    var bankDetails = _bankService.GetBankDetails(bankId);
                    if (bankDetails != null)
                    {
                        _stateService.Set(bankDetails);
                        var mainForm = _serviceProvider.GetRequiredService<MainForm>();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Bank doesn't exist", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            catch
            {
                MessageBox.Show("A problem occured while logging into the bank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            BankIdTextBox.Text = string.Empty;
            var registerForm = _serviceProvider.GetRequiredService<RegisterForm>();
            registerForm.Show();
            this.Hide();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
