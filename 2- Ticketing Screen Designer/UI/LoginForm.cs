using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

using System.Drawing;
using System.Windows.Forms;
using Ticketing_Screen_Designer.Interfaces.Services;

namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class LoginForm : Form
    {
        private readonly IBankService _bankService;
        private readonly IServiceProvider _serviceProvider;
        public LoginForm(IBankService bankService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _bankService = bankService;
            _serviceProvider = serviceProvider;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            BankIdLabel.ForeColor = ColorTranslator.FromHtml("#3C3C3C");
            BankIdLabel.BackColor = ColorTranslator.FromHtml("#F5F7FA");


            BankIdTextBox.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            BankIdTextBox.ForeColor = ColorTranslator.FromHtml("#333333");

            LoginButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");

            RegisterButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            // move the keyboard pointer to ID
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void LoginForm_Click(object sender, EventArgs e)
        {

        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            int myNumber;
            if (int.TryParse(BankIdTextBox.Text, out myNumber))
            {
                var bankDetails = _bankService.GetBankDetails(myNumber);
                if (bankDetails != null)
                {
                    var mainForm = _serviceProvider.GetRequiredService<MainForm>();
                    mainForm.InitializeBankData(bankDetails);
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Bank doesn't exist");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid whole number.");
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var registerForm = _serviceProvider.GetRequiredService<RegisterForm>();
            registerForm.FormClosed += (s, args) => this.Show();
            registerForm.Show();
            this.Hide();
        }
    }
}
