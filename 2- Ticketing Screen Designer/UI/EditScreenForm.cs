using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Banks;
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
        private readonly IAddButtonService _addButtonService;
        private bool _isNavigatingBack = false;



        public EditScreenForm(IScreenService screenService,
            IServiceProvider serviceProvider, IUiStateService stateService,
            IButtonService buttonService, IAddButtonService addButtonService)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            _buttonService = buttonService;
            _addButtonService = addButtonService;
            this.BackColor = ColorTranslator.FromHtml("#F5F7FA");

            ButtonsList.BackColor = ColorTranslator.FromHtml("#FFFFFF");
            ButtonsList.ForeColor = ColorTranslator.FromHtml("#333333");

            AddButton.BackColor = ColorTranslator.FromHtml("#0F6CBD");
            EditButton.ForeColor = ColorTranslator.FromHtml("#0F6CBD");
            DeleteButton.BackColor = ColorTranslator.FromHtml("#D83B01");



        }


        private void validateScreen()
        {
            var bankSession = _stateService.Get<BankResponseDto>();
            bool isActivatedCurrent = ActivateButton.Checked;
            string currentName = ScreenNameTextBox.Text.Trim();

            ValidationExtensions.ValidateModel(new CreateScreenRequestDto
            {
                BankId = bankSession.BankId,
                ScreenName = currentName,
                IsActive = isActivatedCurrent,
            });

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var screenDetails = _stateService.Get<ScreenResponseDto>();
            var bankSession = _stateService.Get<BankResponseDto>();
            bool isActivatedCurrent = ActivateButton.Checked;
            string currentName = ScreenNameTextBox.Text.Trim();
            var pendingDeletes = _stateService.Get<List<int>>() ?? new List<int>();
            var pendingUpdates = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
            var pendingCreates = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
            int finalButtonCount = 0;

            if (screenDetails == null)
            {
                finalButtonCount = pendingCreates.Count;
            }
            else
            {
                try
                {
                    var databaseButtons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId) ?? new List<BaseButtonResponseDto>();
                    int remainingDbButtonsCount = databaseButtons.Count(btn => !pendingDeletes.Contains(btn.ButtonId));
                    finalButtonCount = remainingDbButtonsCount + pendingCreates.Count;
                }
                catch (Exception)
                {
                    MessageBox.Show("Could not verify button counts. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (finalButtonCount <= 0)
            {
                MessageBox.Show("A screen must contain at least one button.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // CASE 1: Creating a New Screen
            if (screenDetails == null)
            {
                try
                {
                    validateScreen();
                    var newScreenId = _screenService.AddScreen(new CreateScreenRequestDto
                    {
                        ScreenName = currentName,
                        IsActive = isActivatedCurrent,
                        BankId = bankSession.BankId
                    });

                    if (newScreenId > 0)
                    {
                        var newScreen = _screenService.GetScreenDetails(newScreenId);
                        _stateService.Set(newScreen);
                        foreach (var button in pendingCreates)
                        {
                            button.ScreenId = newScreenId;
                        }
                        _addButtonService.AddButtons(pendingCreates);
                        ClearSessionCache();
                        MessageBox.Show($"Screen and buttons created successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _isNavigatingBack = true;
                        this.Close();
                    }
                }
                catch (ValidationException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (ExcessiveScreenActivationException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                catch (DuplicateRecordException)
                {
                    MessageBox.Show("A screen with the same name already exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception)
                {
                    MessageBox.Show("A problem occurred while creating screen and buttons", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // CASE 2: Updating an Existing Screen
            else
            {
                try
                {
                    bool hasScreenChanges = (currentName != screenDetails.ScreenName || isActivatedCurrent != screenDetails.IsActive);
                    bool hasButtonChanges = (pendingDeletes.Count > 0 || pendingUpdates.Count > 0 || pendingCreates.Count > 0);

                    if (!hasScreenChanges && !hasButtonChanges)
                    {
                        MessageBox.Show($"The current info are up to date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (hasScreenChanges)
                    {
                        validateScreen();
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
                        }
                    }
                    if (pendingDeletes.Count > 0)
                    {
                        _buttonService.DeleteButtons(pendingDeletes);
                    }

                    if (pendingUpdates.Count > 0)
                    {
                        _buttonService.UpdateButtons(pendingUpdates);
                    }
                    if (pendingCreates.Count > 0)
                    {
                        _addButtonService.AddButtons(pendingCreates);
                    }



                    ClearSessionCache();
                    MessageBox.Show($"All changes updated correctly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _isNavigatingBack = true;
                    this.Close();
                }
                catch (ValidationException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (ExcessiveScreenActivationException)
                {
                    MessageBox.Show("A screen is already active", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (DuplicateRecordException)
                {
                    MessageBox.Show("A screen with the same name already exists", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception)
                {
                    MessageBox.Show("A problem occurred while updating screen and buttons", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearSessionCache()
        {
            _stateService.Clear<List<int>>();
            _stateService.Clear<List<UpdateButtonRequestDto>>();
            _stateService.Clear<List<BaseButtonDto>>();
            _stateService.Clear<ScreenResponseDto>();
            _stateService.Clear<CreateScreenRequestDto>();
        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            _isNavigatingBack = true;
            ClearSessionCache();
            this.Close();
        }

        private void preserveScreen()
        {
            var screenDetails = _stateService.Get<ScreenResponseDto>();
            var bankSession = _stateService.Get<BankResponseDto>();
            bool isActivatedCurrent = ActivateButton.Checked;
            string currentName = ScreenNameTextBox.Text.Trim();

            //validateScreen();
            if (screenDetails == null)
            {

                var pendingCreateScreen = new CreateScreenRequestDto
                {
                    ScreenName = currentName,
                    IsActive = isActivatedCurrent,
                    BankId = bankSession.BankId
                };
                _stateService.Set(pendingCreateScreen);
            }
            else if (screenDetails != null)
            {
                var pendingUpdateScreen = new BaseScreenRequestDto
                {
                    screenId = screenDetails.ScreenId,
                    ScreenName = currentName,
                    IsActive = isActivatedCurrent
                };
                _stateService.Set(pendingUpdateScreen);

            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {

            try
            {
                preserveScreen();
                _stateService.Clear<BaseButtonResponseDto>();
                _stateService.Clear<UpdateButtonRequestDto>();
                _stateService.Clear<BaseButtonDto>();
                var editButtonForm = _serviceProvider.GetRequiredService<AddEditButton>();
                editButtonForm.ShowDialog();

            }

            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ButtonsList.SelectedItem == null)
            {
                MessageBox.Show("Please select a button to Edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool navigate = false;

            // CASE 1: Editing a database button (No changes made yet)
            if (ButtonsList.SelectedItem is BaseButtonResponseDto selectedButton)
            {
                try
                {
                    int buttonIdToEdit = selectedButton.ButtonId;
                    var button = _buttonService.GetButtonDetails(buttonIdToEdit, selectedButton.ButtonType);

                    if (button != null)
                    {
                        _stateService.Set(button);
                        navigate = true;
                    }
                    else
                    {
                        MessageBox.Show("This button has been deleted by someone else.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        refreshList();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("A problem occurred while retrieving information for this button.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // CASE 2: Editing an item that ALREADY has an uncommitted pending update (Only Db buttons)
            else if (ButtonsList.SelectedItem is UpdateButtonRequestDto selectedPendingUpdatedButton)
            {
                var listOfPendingUpdates = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
                var buttonToEdit = listOfPendingUpdates.FirstOrDefault(b => b.ButtonNameEN == selectedPendingUpdatedButton.ButtonNameEN);

                if (buttonToEdit != null)
                {
                    _stateService.Set(buttonToEdit);

                    navigate = true;
                }
            }

            // CASE 3: Editing a new button created on the client side (Not in DB) -> always create new button on client side 
            else if (ButtonsList.SelectedItem is BaseButtonDto selectedPendingButton)
            {
                var listOfPendingCreates = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
                var buttonToEdit = listOfPendingCreates.FirstOrDefault(b => b.ButtonNameEN == selectedPendingButton.ButtonNameEN);

                if (buttonToEdit != null)
                {
                    _stateService.Set(buttonToEdit);
                    navigate = true;
                }
            }

            if (navigate)
            {
                try
                {
                    preserveScreen();
                    var editButtonForm = _serviceProvider.GetRequiredService<AddEditButton>();
                    editButtonForm.ShowDialog();
                    refreshList();
                }
                catch (ValidationException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
        }


        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ButtonsList.SelectedItem == null)
            {
                MessageBox.Show("Please select a button to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // CASE 1: The selected button already exists in the database
            if (ButtonsList.SelectedItem is BaseButtonResponseDto selectedDbButton)
            {
                DialogResult confirm = MessageBox.Show(
                    $"Are you sure you want to delete the button : {selectedDbButton.ButtonNameEN}?",
                    "Delete Button", MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    var pendingDeletes = _stateService.Get<List<int>>() ?? new List<int>();
                    if (!pendingDeletes.Contains(selectedDbButton.ButtonId))
                    {
                        pendingDeletes.Add(selectedDbButton.ButtonId);
                        _stateService.Set(pendingDeletes);
                    }

                    var pendingUpdates = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
                    var updateToRemove = pendingUpdates.FirstOrDefault(b => b.ButtonNameEN == selectedDbButton.ButtonNameEN);
                    if (updateToRemove != null)
                    {
                        pendingUpdates.Remove(updateToRemove);
                        _stateService.Set(pendingUpdates);
                    }

                    MessageBox.Show($"The button has been deleted : {selectedDbButton.ButtonNameEN}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    ButtonsList.Items.Remove(selectedDbButton);
                    ButtonsList.ClearSelected();
                }
            }

            // CASE 2: The selected button is an uncommitted pending UPDATE
            else if (ButtonsList.SelectedItem is UpdateButtonRequestDto selectedPendingUpdatedButton)
            {
                var pendingUpdates = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
                var updateToRemove = pendingUpdates.FirstOrDefault(b => b.ButtonNameEN == selectedPendingUpdatedButton.ButtonNameEN);

                if (updateToRemove != null)
                {
                    pendingUpdates.Remove(updateToRemove);
                    _stateService.Set(pendingUpdates);
                }

                var pendingDeletes = _stateService.Get<List<int>>() ?? new List<int>();
                if (!pendingDeletes.Contains(selectedPendingUpdatedButton.ButtonId))
                {
                    pendingDeletes.Add(selectedPendingUpdatedButton.ButtonId);
                    _stateService.Set(pendingDeletes);
                }

                ButtonsList.Items.Remove(selectedPendingUpdatedButton);
                ButtonsList.ClearSelected();
            }

            // CASE 3: The selected button is a new pending created button
            else if (ButtonsList.SelectedItem is BaseButtonDto selectedPendingButton)
            {
                var listOfPendingCreates = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
                var buttonToRemove = listOfPendingCreates.FirstOrDefault(b => b.ButtonNameEN == selectedPendingButton.ButtonNameEN);

                if (buttonToRemove != null)
                {
                    listOfPendingCreates.Remove(buttonToRemove);
                    _stateService.Set(listOfPendingCreates);
                }
                ButtonsList.Items.Remove(selectedPendingButton);
                ButtonsList.ClearSelected();
            }
        }


        private void EditScreenForm_Load(object sender, EventArgs e)
        {
            DeactivateButton.Checked = true;
            refreshList();
        }


        //unifiedButtons -> Holds all buttons -> for listing and selecting 
        public void refreshList()
        {
            _stateService.Clear<List<BaseButtonResponseDto>>();
            ButtonsList.Items.Clear();
            var screenDetails = _stateService.Get<ScreenResponseDto>();
            var pendingCreateScreen = _stateService.Get<CreateScreenRequestDto>();
            var pendingUpdateScreen = _stateService.Get<BaseScreenRequestDto>();

            var pendingButtonsUpdate = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
            var pendingButtonsCreate = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();

            var pendingDeletes = _stateService.Get<List<int>>() ?? new List<int>();

            var unifiedButtons = new Dictionary<string, object>();
            this.Text = "Edit Screen";
            if (screenDetails == null)
            {
                this.Text = "Add Screen";
            }
            if (screenDetails != null)
            {
                ScreenNameTextBox.Text = screenDetails.ScreenName;
                ActivateButton.Checked = screenDetails.IsActive;
                DeactivateButton.Checked = !screenDetails.IsActive;

                try
                {

                    var databaseButtons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId);
                    _stateService.Set(databaseButtons);
                    foreach (var btn in databaseButtons)
                    {
                        if (!pendingDeletes.Contains(btn.ButtonId))
                        {
                            unifiedButtons[btn.ButtonId.ToString()] = btn;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("A problem occurred while retrieving buttons for this screen", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (pendingCreateScreen != null)
            {
                ScreenNameTextBox.Text = pendingCreateScreen.ScreenName;
                ActivateButton.Checked = pendingCreateScreen.IsActive;
                DeactivateButton.Checked = !pendingCreateScreen.IsActive;
            }
            if (pendingUpdateScreen != null)
            {
                ScreenNameTextBox.Text = pendingUpdateScreen.ScreenName;
                ActivateButton.Checked = pendingUpdateScreen.IsActive;
                DeactivateButton.Checked = !pendingUpdateScreen.IsActive;
            }
            foreach (var updateBtn in pendingButtonsUpdate)
            {
                if (!pendingDeletes.Contains(updateBtn.ButtonId))
                {
                    unifiedButtons[updateBtn.ButtonId.ToString()] = updateBtn;
                }
            }


            foreach (var createBtn in pendingButtonsCreate)
            {
                unifiedButtons[createBtn.ButtonNameEN] = createBtn;
            }

            ButtonsList.DisplayMember = "DisplayText";

            foreach (var buttonObj in unifiedButtons.Values)
            {
                ButtonsList.Items.Add(buttonObj);
            }
        }


        private void EditScreenForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_isNavigatingBack && Application.OpenForms["MainForm"] is MainForm mainForm)
            {
                ClearSessionCache();
                mainForm.refreshList();
                mainForm.Show();
            }
            else if (!_isNavigatingBack && e.CloseReason == CloseReason.UserClosing)
            {

                Environment.Exit(0);

            }
        }


    }
}
