using App.Application.DTO.Banks;
using App.Application.Interfaces;
using App.Shared;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
namespace App.WinForms
{
    public partial class RegisterForm : Form
    {
        private readonly IBankService _bankService;
        public RegisterForm(IBankService bankService)
        {
            InitializeComponent();
            _bankService = bankService;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");
            bankNameTextBox.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            bankNameTextBox.ForeColor = ColorTranslator.FromHtml("#333333");

            RegisterButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");

            CancelButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string newBankName = bankNameTextBox.Text;
            try
            {



                int newBankId = _bankService.CreateBank(new CreateBankRequestDto
                {
                    BankName = newBankName.Trim()
                });

                MessageBox.Show($"the new bank ID is {newBankId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (DuplicateRecordException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("A problem occured while registering new bank", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (System.Windows.Forms.Application.OpenForms["LoginForm"] is LoginForm loginForm)
            {
                loginForm.Show();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            bankNameTextBox.Focus();
        }
    }
}
