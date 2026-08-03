using App.Application.DTO.Banks;
using App.Application.Interfaces;
using App.Shared;
using App.WinForms.Forms;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
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

                DialogResult confirm = MessageBox.Show($"the new bank ID is {newBankId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (confirm == DialogResult.OK)
                {
                    button2_Click(this, EventArgs.Empty);
                }
            }
            catch (DuplicateRecordException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SqlException ex) when (ex.Number == (int)SqlErrorTypes.LoginFailed ||
                ex.Number == (int)SqlErrorTypes.DatabaseAccessDenied || ex.Number == (int)SqlErrorTypes.ServerNotFound)
            {
                MessageBox.Show("Unable to connect to the database. Please verify your network connection, server details, and credentials."
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private void label1_Click(object sender, EventArgs e)
        {
            bankNameTextBox.Focus();
        }


    }
}
