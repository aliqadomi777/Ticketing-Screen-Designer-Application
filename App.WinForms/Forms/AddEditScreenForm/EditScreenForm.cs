using App.Application.DTO.Banks;
using App.Application.DTO.Buttons;
using App.Application.DTO.Screens;
using App.Application.Interfaces;
using App.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;


namespace App.WinForms
{

    public partial class EditScreenForm : Form
    {
        private readonly IScreenService _screenService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;
        private readonly IButtonService _buttonService;
        private bool _isNavigatingBack = false;
        private bool _isCoreScreenChanged = false;
        private DateTimeOffset _fetchTime;

        public EditScreenForm(IScreenService screenService,
            IServiceProvider serviceProvider, IUiStateService stateService,
            IButtonService buttonService)
        {
            InitializeComponent();
            _screenService = screenService;
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            _buttonService = buttonService;


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
            var cachedDbButtons = _stateService.Get<List<BaseButtonResponseDto>>();
            int finalButtonCount = 0;

            if (screenDetails == null)
            {
                finalButtonCount = pendingCreates.Count;
            }
            else
            {
                try
                {
                    var databaseButtons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId);
                    var latestScreen = _screenService.GetScreenDetails(screenDetails.ScreenId);
                    if (latestScreen == null)
                    {
                        MessageBox.Show("This Screen has been deleted by someone else", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                        return;
                    }
                    if (latestScreen.ModifiedAt > _fetchTime)
                    {
                        _fetchTime = latestScreen.ModifiedAt;
                        _isCoreScreenChanged = ScreenNameTextBox.Text != latestScreen.ScreenName
                           || ActivateButton.Checked != latestScreen.IsActive;

                        var dbIds = new HashSet<int>(databaseButtons.Select(db => db.ButtonId));
                        var cachedIds = new HashSet<int>(cachedDbButtons.Select(c => c.ButtonId));
                        var deleteIds = new HashSet<int>(pendingDeletes);
                        var pendingUpdateIds = new HashSet<int>(pendingUpdates.Select(p => p.ButtonId));
                        var cachedLookup = cachedDbButtons.ToDictionary(c => c.ButtonId);

                        // Check if any button we want to update is missing from the database -> Deleted
                        bool hasMissingUpdates = pendingUpdates.Any(p => !dbIds.Contains(p.ButtonId));

                        // Check if any button we want to delete still exists in the database
                        bool hasDeletesToProcess = deleteIds.Any(id => dbIds.Contains(id));

                        // Check if any buttons newly added to db that is not yet loaded into Current instance
                        bool hasNewDbButtons = databaseButtons.Any(db => !cachedIds.Contains(db.ButtonId));

                        // Check if cached button is  deleted in db exluding already deleted from cached -> Other instance deleted it
                        bool isCacheOutdatedByDeletes = cachedDbButtons.Any(c => !dbIds.Contains(c.ButtonId)
                                                                            && !deleteIds.Contains(c.ButtonId));

                        // Check if a db button was modified and not reflected on current cached buttons
                        // excluding already modified buttons on current instance
                        bool hasModifiedButtons = databaseButtons.Any(db =>
                            !pendingUpdateIds.Contains(db.ButtonId)
                            && cachedLookup.TryGetValue(db.ButtonId, out var cachedBtn)
                            && db.ModifiedAt > cachedBtn.ModifiedAt
                        );
                        screenDetails.ScreenName = latestScreen.ScreenName;
                        screenDetails.IsActive = latestScreen.IsActive;

                        if (hasModifiedButtons || hasMissingUpdates || hasDeletesToProcess || _isCoreScreenChanged || hasNewDbButtons || isCacheOutdatedByDeletes)
                        {
                            DialogResult syncOrCancel = MessageBox.Show(
                                            "Current Info are outdated press Ok to Sync Screen's Info or Cancel to Exit", "Warning",
                                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (syncOrCancel == DialogResult.OK)
                            {
                                _fetchTime = latestScreen.ModifiedAt;
                                //preserveScreen();
                                refreshList();
                                //IF we want to sync the name and status of screen
                                ScreenNameTextBox.Text = latestScreen.ScreenName;
                                ActivateButton.Checked = latestScreen.IsActive;
                                DeactivateButton.Checked = !latestScreen.IsActive;

                                return;
                            }
                            else if (syncOrCancel == DialogResult.Cancel)
                            {
                                ClearSessionCache();
                                _isNavigatingBack = true;
                                refreshParentForm();
                                this.Close();
                            }
                        }

                    }
                    //var databaseButtons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId);
                    //if (databaseButtons.Count() == 0)
                    //{
                    //    MessageBox.Show("This Screen has been deleted by someone else", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
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
                    var newScreen = new CreateScreenRequestDto
                    {
                        ScreenName = currentName,
                        IsActive = isActivatedCurrent,
                        BankId = bankSession.BankId
                    };
                    //Change into a one scope of transaction -> if buttons adding fails -> screen not commited
                    var newScreenId = _screenService.CreateScreenWithButtons(newScreen, pendingCreates);
                    if (newScreenId > 0)
                    {
                        ClearSessionCache();
                        //MessageBox.Show($"Screen and buttons created successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _isNavigatingBack = true;
                        refreshParentForm();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Screen and buttons could not be created.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                catch (DuplicateRecordException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    if (hasScreenChanges || hasButtonChanges || _isCoreScreenChanged)
                    {
                        _isCoreScreenChanged = false;
                        validateScreen();
                        var updatedScreen = new BaseScreenRequestDto
                        {
                            screenId = screenDetails.ScreenId,
                            ScreenName = currentName,
                            IsActive = isActivatedCurrent,
                        };
                        bool isUpdated = _screenService.UpdateScreenAndButtons(updatedScreen,
                            pendingCreates, pendingUpdates, pendingDeletes);
                        if (isUpdated)
                        {
                            ClearSessionCache();
                            //MessageBox.Show($"All changes updated correctly", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            _isNavigatingBack = true;
                            refreshParentForm();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Screen and buttons could not be updated.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
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
                catch (DuplicateRecordException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                catch (ParentDeletedWithChildConflictException ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
            _stateService.Clear<BaseScreenRequestDto>();
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
                var editButtonForm = new AddEditButton(
                    _serviceProvider.GetRequiredService<IUiStateService>(),
                    _serviceProvider.GetRequiredService<IServiceTypeService>(),
                    _serviceProvider.GetRequiredService<IButtonTypeService>()
                    );
                editButtonForm.StartPosition = FormStartPosition.CenterParent;
                editButtonForm.ShowDialog(this);

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
                    var editButtonForm = new AddEditButton(
                        _serviceProvider.GetRequiredService<IUiStateService>(),
                        _serviceProvider.GetRequiredService<IServiceTypeService>(),
                        _serviceProvider.GetRequiredService<IButtonTypeService>()
                        );
                    editButtonForm.StartPosition = FormStartPosition.CenterParent;
                    editButtonForm.ShowDialog(this);
                    //refreshList();
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

                    //MessageBox.Show($"The button has been deleted : {selectedDbButton.ButtonNameEN}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


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
            _fetchTime = DateTimeOffset.UtcNow;
            refreshList();
        }


        //unifiedButtons -> Holds all buttons -> for listing and selecting 

        //This can be further optimized by checking cached buttons vs db buttons -> Not clearing every refresh
        public void refreshList()
        {
            _stateService.Clear<List<BaseButtonResponseDto>>();

            ButtonsList.BeginUpdate();
            ButtonsList.Items.Clear();

            var screenDetails = _stateService.Get<ScreenResponseDto>();
            var pendingCreateScreen = _stateService.Get<CreateScreenRequestDto>();
            var pendingUpdateScreen = _stateService.Get<BaseScreenRequestDto>();
            var pendingButtonsUpdate = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
            var pendingButtonsCreate = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
            var pendingDeletesSet = new HashSet<int>(_stateService.Get<List<int>>() ?? new List<int>());
            var unifiedButtons = new Dictionary<string, object>();
            List<BaseButtonResponseDto> databaseButtons = new List<BaseButtonResponseDto>();
            ScreenResponseDto latestScreen = null;
            if (screenDetails != null)
            {
                latestScreen = _screenService.GetScreenDetails(screenDetails.ScreenId);

            }

            this.Text = "Edit Screen";

            if (screenDetails == null)
            {
                this.Text = "Add Screen";
                RefreshButton.Hide();
            }
            else
            {
                ScreenNameTextBox.Text = screenDetails.ScreenName;
                ActivateButton.Checked = screenDetails.IsActive;
                DeactivateButton.Checked = !screenDetails.IsActive;

                try
                {
                    databaseButtons = _buttonService.GetAllButtonsDetails(screenDetails.ScreenId) ?? new List<BaseButtonResponseDto>();
                    _stateService.Set(databaseButtons);

                    if (latestScreen == null)
                    {
                        MessageBox.Show("This Screen has been deleted by someone else", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        refreshParentForm();
                        this.Close();
                        return;
                    }
                    foreach (var btn in databaseButtons)
                    {
                        if (!pendingDeletesSet.Contains(btn.ButtonId))
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

            if (pendingCreateScreen != null)
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

            var dbIds = new HashSet<int>(databaseButtons.Select(o => o.ButtonId));
            pendingButtonsUpdate.RemoveAll(b => !dbIds.Contains(b.ButtonId));

            foreach (var updateBtn in pendingButtonsUpdate)
            {
                if (!pendingDeletesSet.Contains(updateBtn.ButtonId))
                {
                    unifiedButtons[updateBtn.ButtonId.ToString()] = updateBtn;
                }
            }

            foreach (var createBtn in pendingButtonsCreate)
            {
                unifiedButtons[createBtn.ButtonNameEN] = createBtn;
            }

            ButtonsList.DisplayMember = "DisplayText";
            ButtonsList.Items.AddRange(unifiedButtons.Values.ToArray());
            ButtonsList.EndUpdate();

        }


        private void CancelButton_Click(object sender, EventArgs e)
        {
            ClearSessionCache();
            refreshParentForm();
            this.Close();
        }



        //Refreshing prdouces no problems -> we preserve all needed states of data
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            preserveScreen();
            refreshList();
        }
        private void refreshParentForm()
        {
            if (this.Owner is MainForm mainForm)
            {
                mainForm.refreshList();
            }
        }
    }
}
