using App.Application.Interfaces;
using App.WinForms.Forms;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data.SqlClient;
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
                        var mainForm = new MainForm(
                            _serviceProvider.GetRequiredService<IScreenService>(),
                            _serviceProvider.GetRequiredService<IServiceProvider>(),
                            _serviceProvider.GetRequiredService<IUiStateService>()
                            );
                        mainForm.StartPosition = FormStartPosition.CenterParent;
                        mainForm.ShowDialog(this);
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

            catch (SqlException ex) when (ex.Number == (int)SqlErrorTypes.LoginFailed ||
                ex.Number == (int)SqlErrorTypes.DatabaseAccessDenied || ex.Number == (int)SqlErrorTypes.ServerNotFound)
            {
                MessageBox.Show("Unable to connect to the database. Please verify your network connection, server details, and credentials."
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception)
            {
                MessageBox.Show("A problem occured while logging into the bank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            BankIdTextBox.Text = string.Empty;


            var registerForm = new RegisterForm(
                        _serviceProvider.GetRequiredService<IBankService>()
                        );
            registerForm.StartPosition = FormStartPosition.CenterParent;
            registerForm.ShowDialog(this);
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
