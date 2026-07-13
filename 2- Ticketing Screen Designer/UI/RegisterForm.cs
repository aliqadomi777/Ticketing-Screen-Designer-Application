using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Banks;
using Ticketing_Screen_Designer.Interfaces.Services;
namespace _2__Ticketing_Screen_Designer.UI
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
            try
            {

                string newBankName = bankNameTextBox.Text;

                int newBankId = _bankService.CreateBank(new CreateBankRequestDto
                {
                    BankName = newBankName.Trim()
                });

                MessageBox.Show($"the new bank ID is {newBankId}");
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            bankNameTextBox.Focus();
        }
    }
}
