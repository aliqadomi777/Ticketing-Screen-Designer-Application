using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Banks;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Services;

namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class MainForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        public MainForm(IScreenService screenService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void InitializeBankData(BankResponseDto bankDetails)
        {
            TitleLabel.Text += bankDetails.BankName;
            var screens = _screenService.GetAllScreensDetails(bankDetails.BankId);

        }
    }
}
