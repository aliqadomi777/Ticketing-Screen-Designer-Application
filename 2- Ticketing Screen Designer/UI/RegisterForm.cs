using System;
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
        }


        private void button1_Click(object sender, EventArgs e)
        {

            string newBankName = bankName.Text;

            int newBankId = _bankService.CreateBank(new CreateBankRequestDto
            {
                BankName = newBankName
            });

            MessageBox.Show($"the new bank ID is {newBankId}");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
