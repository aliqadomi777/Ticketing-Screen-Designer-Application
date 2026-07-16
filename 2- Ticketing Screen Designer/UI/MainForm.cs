using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Banks;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.Interfaces.Services;

namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class MainForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;


        public MainForm(IScreenService screenService, IServiceProvider serviceProvider, IUiStateService stateService)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            screenList.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            screenList.ForeColor = ColorTranslator.FromHtml("#333333");

            AddScreenButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");
            EditScreenButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");
            DeleteScreenButton.BackColor = ColorTranslator.FromHtml("#D83B01");
        }


        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            refreshList();

        }

        public void centerTitle()
        {
            int titleWidth = TextRenderer.MeasureText(ScreenTitleLabel.Text, ScreenTitleLabel.Font).Width;
            int formWidth = this.ClientSize.Width;
            int startingLeft = (formWidth - titleWidth) / 2;
            ScreenTitleLabel.Left = startingLeft;
        }
        public void refreshList()
        {
            screenList.Items.Clear();
            var bankSession = _stateService.Get<BankResponseDto>();
            TitleLabel.Text = $"Main Form - {bankSession.BankName}";
            centerTitle();
            if (bankSession != null)
            {
                var screens = _screenService.GetAllScreensDetails(bankSession.BankId);
                screenList.DisplayMember = "DisplayText";
                foreach (var screen in screens)
                {
                    screenList.Items.Add(screen);
                }
            }
        }
        private void MainForm_Click(object sender, EventArgs e)
        {
            this.ActiveControl = null;
            screenList.ClearSelected();
        }

        private void DeleteScreenButton_Click(object sender, EventArgs e)
        {
            if (screenList.SelectedItem is ScreenResponseDto selectedScreen)
            {
                int screenIdToDelete = selectedScreen.ScreenId;
                string screenName = selectedScreen.ScreenName;
                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete the screen : {screenName}?",
                    "Delete Screen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (_screenService.DeleteScreen(screenIdToDelete))
                        {
                            MessageBox.Show($"Successfully initiated deletion for Screen : {screenName}");


                        }
                        else
                        {
                            MessageBox.Show($"Screen is already deleted");
                        }
                        screenList.Items.Remove(selectedScreen);
                        screenList.ClearSelected();
                    }

                    catch (Exception)
                    {
                        MessageBox.Show($"A problem Occured while deleting this screen");
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a screen to delete.");
            }
        }

        private void EditScreenButton_Click(object sender, EventArgs e)
        {
            if (screenList.SelectedItem is ScreenResponseDto selectedScreen)
            {
                int screenIdToEdit = selectedScreen.ScreenId;
                string screenName = selectedScreen.ScreenName;
                try
                {
                    var screen = _screenService.GetScreenDetails(screenIdToEdit);
                    if (screen != null)
                    {
                        _stateService.Set(screen);
                        var editScreen = _serviceProvider.GetRequiredService<EditScreenForm>();
                        editScreen.Show();
                        this.Hide();
                    }

                    else
                    {
                        MessageBox.Show("This screen has been deleted by someone");
                        refreshList();
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("A problem occured while Retrieving screen info");

                }

            }


            else
            {
                MessageBox.Show("Please select a screen to Edit.");
            }
        }

        private void AddScreenButton_Click(object sender, EventArgs e)
        {
            var addScreen = _serviceProvider.GetRequiredService<AddScreenForm>();
            addScreen.Show();
            this.Hide();

        }
    }
}
