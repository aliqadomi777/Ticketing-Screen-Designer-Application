using App.Application.DTO.Banks;
using App.Application.DTO.Screens;
using App.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace App.WinForms
{
    public partial class MainForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;

        private Timer _periodicTimer;
        public MainForm(IScreenService screenService, IServiceProvider serviceProvider, IUiStateService stateService)
        {
            InitializeComponent();
            InitializePeriodicTimer();
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

        private void InitializePeriodicTimer()
        {
            _periodicTimer = new Timer();
            //5 mins 
            _periodicTimer.Interval = 1000 * 60 * 5;

            _periodicTimer.Tick += PeriodicTimer_Exec;

            _periodicTimer.Start();
        }
        private void PeriodicTimer_Exec(object sender, EventArgs e)
        {
            refreshList();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
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
        //Optimize solution -> usage of modified date
        public void refreshList()
        {

            var bankSession = _stateService.Get<BankResponseDto>();
            TitleLabel.Text = $"{bankSession.BankName}";
            centerTitle();
            screenList.BeginUpdate();
            screenList.Items.Clear();
            try
            {
                var screens = _screenService.GetAllScreensDetails(bankSession.BankId);
                screenList.DisplayMember = "DisplayText";
                screenList.Items.AddRange(screens.ToArray());
                screenList.EndUpdate();
            }
            catch (Exception)
            {
                MessageBox.Show($"A problem Occured while loading screens", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                        if (!_screenService.DeleteScreen(screenIdToDelete))
                        {
                            MessageBox.Show($"Screen is already deleted", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }

                        screenList.Items.Remove(selectedScreen);
                        screenList.ClearSelected();
                    }

                    catch (Exception)
                    {
                        MessageBox.Show($"A problem Occured while deleting this screen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a screen to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        FormUtils.CenterToForm(this, editScreen);
                        editScreen.Show();
                    }

                    else
                    {
                        MessageBox.Show("This screen has been deleted by someone", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        refreshList();
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show("A problem occured while Retrieving screen info", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }


            else
            {
                MessageBox.Show("Please select a screen to Edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AddScreenButton_Click(object sender, EventArgs e)
        {
            _stateService.Clear<BaseScreenRequestDto>();
            _stateService.Clear<CreateScreenRequestDto>();

            var addScreen = _serviceProvider.GetRequiredService<EditScreenForm>();
            FormUtils.CenterToForm(this, addScreen);
            addScreen.Show();
        }


        private void RefreshButton_Click(object sender, EventArgs e)
        {
            //Resets the timer if manually refreshed
            _periodicTimer.Stop();
            _periodicTimer.Start();
            refreshList();
        }
    }
}
