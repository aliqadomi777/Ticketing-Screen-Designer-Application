using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Utils;


namespace _2__Ticketing_Screen_Designer.UI
{

    public partial class EditScreenForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;
        private readonly IButtonService _buttonService;
        public EditScreenForm(IScreenService screenService,
            IServiceProvider serviceProvider, IUiStateService stateService,
            IButtonService buttonService)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            _buttonService = buttonService;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            ButtonsList.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            ButtonsList.ForeColor = ColorTranslator.FromHtml("#333333");

            AddButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");
            EditButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");
            DeleteButton.BackColor = ColorTranslator.FromHtml("#D83B01");

        }




        private void SaveButton_Click(object sender, EventArgs e)
        {

            var screenDetails = _stateService.Get<ScreenResponseDto>();

            bool isActivatedCurrent = ActivateButton.Checked;
            string currentName = ScreenNameTextBox.Text.Trim();
            try
            {
                if (currentName != screenDetails.ScreenName || isActivatedCurrent != screenDetails.IsActive)
                {
                    bool isUpdated = _screenService.UpdateScreen(new BaseScreenRequestDto
                    {
                        screenId = screenDetails.ScreenId,
                        ScreenName = currentName,
                        IsActive = isActivatedCurrent,
                    });

                    if (isUpdated)
                    {
                        var updatedScreen = _screenService.GetScreenDetails(screenDetails.ScreenId);
                        _stateService.Set(updatedScreen);
                        MessageBox.Show($"Updated correctly");
                    }
                }
                else
                {
                    MessageBox.Show($"The current info are up to date");

                }
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ExcessiveScreenActivationException)
            {
                MessageBox.Show("A screen is already active");
            }

            catch (DuplicateRecordException)
            {
                MessageBox.Show("A screen with the same name already exists");

            }
            catch (Exception)
            {
                MessageBox.Show("A problem occured while updating screen");
            }


        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var editButton = _serviceProvider.GetRequiredService<AddEditButton>();
            editButton.Show();
            this.Hide();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ButtonsList.SelectedItem is BaseButtonResponseDto selectedButton)
            {
                try
                {
                    int buttonIdToEdit = selectedButton.ButtonId;
                    var button = _buttonService.GetButtonDetails(buttonIdToEdit, selectedButton.ButtonType);
                    if (button != null)
                    {
                        _stateService.Set(button);
                        var editButton = _serviceProvider.GetRequiredService<AddEditButton>();
                        editButton.Show();
                        this.Hide();
                    }

                    else
                    {
                        MessageBox.Show("This button has been deleted by someone");
                        refreshList();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show($"A problem Occured while retrieving info for this button");

                }

            }


            else
            {
                MessageBox.Show("Please select a button to Edit.");
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ButtonsList.SelectedItem is BaseButtonResponseDto selectedButton)
            {
                int buttonIdToDelete = selectedButton.ButtonId;
                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete the button : {selectedButton.ButtonNameEN}?",
                    "Delete Screen",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (confirm == DialogResult.Yes)
                {
                    try
                    {
                        if (_buttonService.DeleteButton(buttonIdToDelete))
                        {
                            MessageBox.Show($"Successfully initiated deletion for Screen : {selectedButton.ButtonNameEN}");


                        }
                        else
                        {
                            MessageBox.Show($"Button is already deleted");
                        }
                        ButtonsList.Items.Remove(selectedButton);
                        ButtonsList.ClearSelected();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"A problem Occured while deleting this button");
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select a button to delete.");
            }
        }

        private void EditScreenForm_Load(object sender, EventArgs e)
        {
            refreshList();

        }


        public void refreshList()
        {
            ButtonsList.Items.Clear();
            var screenDetails = _stateService.Get<ScreenResponseDto>();


            if (screenDetails != null)
            {
                ScreenNameTextBox.Text = screenDetails.ScreenName;
                ActivateButton.Checked = screenDetails.IsActive;
                DeactivateButton.Checked = !screenDetails.IsActive;
                try
                {
                    var buttons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId);
                    ButtonsList.DisplayMember = "DisplayText";
                    foreach (var button in buttons)
                    {
                        ButtonsList.Items.Add(button);
                    }
                }

                catch (Exception)
                {
                    MessageBox.Show($"A problem Occured while retrieving buttons for this screen");

                }
            }
        }

        private void EditScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["MainForm"] is MainForm mainForm)
            {
                mainForm.refreshList();
                mainForm.Show();
            }

        }
    }
}
