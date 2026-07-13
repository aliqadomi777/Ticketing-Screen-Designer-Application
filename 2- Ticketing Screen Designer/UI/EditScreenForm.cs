using System;
using System.Drawing;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.Interfaces.Services;


namespace _2__Ticketing_Screen_Designer.UI
{

    //private readonly IButtonService _buttonService;
    public partial class EditScreenForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        public EditScreenForm(IScreenService screenService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            ButtonsList.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            ButtonsList.ForeColor = ColorTranslator.FromHtml("#333333");

            AddButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");
            EditButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");
            DeleteButton.BackColor = ColorTranslator.FromHtml("#D83B01");

        }
        //public void InitializeScreenData(ScreenResponseDto screenDetails)
        //{
        //    ScreenName.Text = screenDetails.ScreenName;
        //    ActivateButton.Checked = screenDetails.IsActive;
        //    DeactivateButton.Checked = !screenDetails.IsActive;
        //}

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
