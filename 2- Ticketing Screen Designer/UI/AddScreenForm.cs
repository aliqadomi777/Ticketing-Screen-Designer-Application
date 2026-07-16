
using System;

using System.ComponentModel.DataAnnotations;

using System.Drawing;

using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Banks;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Utils;


namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class AddScreenForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;
        public AddScreenForm(IScreenService screenService, IServiceProvider serviceProvider, IUiStateService stateService)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");
        }

        private void AddScreenForm_Load(object sender, EventArgs e)
        {
            NewScreenInactiveButton.Checked = true;
        }

        private void NewScreenSaveButton_Click(object sender, EventArgs e)
        {
            string enteredScreenName = NewScreenNameTextBox.Text.Trim();

            try
            {
                var bankSession = _stateService.Get<BankResponseDto>();

                int newScreenId = _screenService.AddScreen(new CreateScreenRequestDto
                {
                    ScreenName = enteredScreenName,
                    IsActive = NewScreenActiveButton.Checked,
                    BankId = bankSession.BankId
                });
                if (newScreenId > 0)
                {
                    MessageBox.Show($"Screen with name {enteredScreenName} successfully!");
                    NewScreenNameTextBox.Text = string.Empty;
                    NewScreenActiveButton.Checked = false;
                    NewScreenInactiveButton.Checked = true;

                }
                else
                {
                    MessageBox.Show("Failed to add screen. Please try again.");
                }


            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DuplicateRecordException)
            {
                MessageBox.Show($"A screen with the name \"{enteredScreenName}\" already exists.");
            }
            //This version "this error never occurs since the adding deactivate active screen"
            catch (ExcessiveScreenActivationException)
            {

                MessageBox.Show("Another screen is already active for the bank");
            }

            catch (Exception)
            {
                MessageBox.Show("A problem occured while adding a new screen to the bank");

            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["MainForm"] is MainForm mainForm)
            {
                mainForm.refreshList();
                mainForm.Show();
            }

        }
    }
}
