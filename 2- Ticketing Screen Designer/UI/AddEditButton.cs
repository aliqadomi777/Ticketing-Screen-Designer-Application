using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.ButtonTypes;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.DTO.Services;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;

namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class AddEditButton : Form
    {
        private readonly IUiStateService _stateService;
        private readonly IServiceTypeService _serviceTypeService;
        private readonly IButtonTypeService _buttonTypeService;

        private bool _isNavigatingBack = false;
        public AddEditButton(
            IUiStateService stateService,
            IServiceTypeService serviceTypeService,
            IButtonTypeService buttonTypeService
            )
        {
            _stateService = stateService;
            _serviceTypeService = serviceTypeService;
            _buttonTypeService = buttonTypeService;
            InitializeComponent();
        }


        public void refreshForm()
        {

            var serviceTypes = _serviceTypeService.GetAllServices();
            var buttonTypes = _buttonTypeService.GetAllButtonTypes();
            ButtonActionList.DisplayMember = "DisplayText";
            ServiceList.DisplayMember = "DisplayText";
            ButtonActionList.Items.Clear();
            ServiceList.Items.Clear();
            foreach (var buttonType in buttonTypes)
            {
                ButtonActionList.Items.Add(buttonType);
            }

            foreach (var serviceType in serviceTypes)
            {
                ServiceList.Items.Add(serviceType);
            }
            ButtonActionList.SelectedIndex = 0;
            ServiceList.SelectedIndex = 0;
            showHideDetail();
        }
        private void loadInfo(object button)
        {
            refreshForm();

            if (button == null) return;

            if (button is BaseButtonDto baseBtn)
            {
                ButtonNameEnTextBox.Text = baseBtn.ButtonNameEN;
                ButtonNameArTextBox.Text = baseBtn.ButtonNameAR;
            }
            else if (button is UpdateButtonRequestDto updateBtn)
            {
                ButtonNameEnTextBox.Text = updateBtn.ButtonNameEN;
                ButtonNameArTextBox.Text = updateBtn.ButtonNameAR;
            }

            if (button is TicketButtonResponseDto ticketButton)
            {
                ServiceList.SelectedItem = ServiceList.Items.Cast<ServiceTypeResponseDto>()
                    .FirstOrDefault(s => s.ServicesName == ticketButton.ServiceName);
            }
            else if (button is UpdateTicketButtonRequest dbTicket)
            {
                ServiceList.SelectedItem = ServiceList.Items.Cast<ServiceTypeResponseDto>()
                    .FirstOrDefault(s => s.ServiceId == dbTicket.ServiceId);
            }

            else if (button is MessageButtonResponseDto messageButton)
            {
                EnMessageTextBox.Text = messageButton.MessageEN;
                ArMessageTextBox.Text = messageButton.MessageAR;
            }
            else if (button is UpdateMessageButtonRequest dbMessage)
            {
                EnMessageTextBox.Text = dbMessage.MessageEN;
                ArMessageTextBox.Text = dbMessage.MessageAR;
            }
        }

        private void AddEditButton_Load(object sender, EventArgs e)
        {
            this.Text = "Add Button";
            var button = _stateService.Get<BaseButtonResponseDto>();
            var dbButtonUpdate = _stateService.Get<UpdateButtonRequestDto>();
            var nonDbButton = _stateService.Get<BaseButtonDto>();

            var nonDbTicket = nonDbButton as CreateTicketButtonRequestDto;
            var nonDbMessage = nonDbButton as CreateMessageButtonRequestDto;
            var dbTicket = dbButtonUpdate as UpdateTicketButtonRequest;
            var dbMessage = dbButtonUpdate as UpdateMessageButtonRequest;

            refreshForm();

            if (button != null)
            {
                this.Text = "Edit Button";

                loadInfo(button);
                ButtonActionList.SelectedItem = ButtonActionList.Items.Cast<ButtonTypeResponseDto>()
                    .FirstOrDefault(b => b.TypeName == button.TypeName);
            }
            else if (nonDbButton != null)
            {
                this.Text = "Edit Button";

                ButtonNameEnTextBox.Text = nonDbButton.ButtonNameEN;
                ButtonNameArTextBox.Text = nonDbButton.ButtonNameAR;

                if (nonDbTicket != null)
                {

                    ButtonActionList.SelectedItem = ButtonActionList.Items.Cast<ButtonTypeResponseDto>()
                        .FirstOrDefault(b => b.TypeId == nonDbTicket.ButtonType);

                    ServiceList.SelectedItem = ServiceList.Items.Cast<ServiceTypeResponseDto>()
                        .FirstOrDefault(s => s.ServiceId == nonDbTicket.ServiceId);
                }
                else if (nonDbMessage != null)
                {

                    ButtonActionList.SelectedItem = ButtonActionList.Items.Cast<ButtonTypeResponseDto>()
                        .FirstOrDefault(b => b.TypeId == nonDbMessage.ButtonType);

                    EnMessageTextBox.Text = nonDbMessage.MessageEN;
                    ArMessageTextBox.Text = nonDbMessage.MessageAR;
                }
            }
            else if (dbButtonUpdate != null)
            {
                this.Text = "Edit Button";

                loadInfo(dbButtonUpdate);

                if (dbTicket != null)
                {
                    ButtonActionList.SelectedItem = ButtonActionList.Items.Cast<ButtonTypeResponseDto>()
                        .FirstOrDefault(b => b.TypeId == dbTicket.ButtonType);
                }
                else if (dbMessage != null)
                {
                    ButtonActionList.SelectedItem = ButtonActionList.Items.Cast<ButtonTypeResponseDto>()
                        .FirstOrDefault(b => b.TypeId == dbMessage.ButtonType);
                }
            }
        }


        //Method to toggle Combo boxes and text boxes based on button action
        private void showHideDetail()
        {
            ServiceLabel.Hide();
            ServiceList.Hide();
            ArMessageLabel.Hide();
            ArMessageTextBox.Hide();
            EnMessageLabel.Hide();
            EnMessageTextBox.Hide();
            string selectedAction = ButtonActionList.Text;
            if (selectedAction == "Issue Ticket")
            {
                ServiceLabel.Show();
                ServiceList.Show();
            }
            else if (selectedAction == "Show Message")
            {
                ArMessageLabel.Show();
                ArMessageTextBox.Show();
                EnMessageLabel.Show();
                EnMessageTextBox.Show();
            }
        }



        private void CancelButton_Click(object sender, EventArgs e)
        {


            _isNavigatingBack = true;
            this.Close();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {

            var button = _stateService.Get<BaseButtonResponseDto>();
            var screen = _stateService.Get<ScreenResponseDto>();

            var dbButtonUpdate = _stateService.Get<UpdateButtonRequestDto>();
            var nonDbButtonUpdate = _stateService.Get<BaseButtonDto>();

            string selectedAction = ButtonActionList.Text;
            var selectedButtonType = ButtonActionList.SelectedItem as ButtonTypeResponseDto;
            var listOfPendingUpdatedButtons = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
            var listOfPendingCreatedButtons = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
            var listDbButtons = _stateService.Get<List<BaseButtonResponseDto>>();
            var selectedServiceType = ServiceList.SelectedItem as ServiceTypeResponseDto;

            var ticketButton = button as TicketButtonResponseDto;
            var messageButton = button as MessageButtonResponseDto;

            // check if pending messages or tickets if we wanted to update them 


            bool coreChanged = false;
            bool ticketChanged = false;
            bool messageChanged = false;
            bool buttonChanged = false;
            bool buttonExists = ticketButton != null || messageButton != null;

            if (buttonExists)
            {
                //Checks if any changes occured on the existing button info 
                string originalAction = button.TypeName;

                coreChanged = ButtonNameArTextBox.Text != button.ButtonNameAR ||
                                  ButtonNameEnTextBox.Text != button.ButtonNameEN ||
                                  originalAction != selectedAction;


                ticketChanged = selectedAction == "Issue Ticket" &&
                                    (selectedServiceType?.ServiceId != (ticketButton?.ServiceId));

                messageChanged = selectedAction == "Show Message" &&
                                     (ArMessageTextBox.Text != (messageButton?.MessageAR ?? string.Empty) ||
                                      EnMessageTextBox.Text != (messageButton?.MessageEN ?? string.Empty));
                buttonChanged = ticketChanged || messageChanged || coreChanged;
            }

            try
            {

                //Editing A button from DB 
                if ((buttonChanged && buttonExists) || dbButtonUpdate != null)
                {
                    int activeButtonId = dbButtonUpdate?.ButtonId ?? button?.ButtonId ?? 0;
                    //Detecting button action type change is handled by Backend (Button Repository)
                    if (dbButtonUpdate != null)
                    {
                        listOfPendingUpdatedButtons.RemoveAll(b => b.ButtonId == activeButtonId);
                    }
                    if (selectedAction == "Issue Ticket")
                    {
                        var updatedTicketButton = new UpdateTicketButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonId = activeButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            ServiceId = selectedServiceType.ServiceId,
                            TicketId = ticketButton?.TicketId ?? 0,
                        };

                        if (!checkIfExists(updatedTicketButton, activeButtonId))
                        {
                            ValidationExtensions.ValidateModel(updatedTicketButton);
                            listOfPendingUpdatedButtons.Add(updatedTicketButton);
                            _stateService.Set(listOfPendingUpdatedButtons);
                            MessageBox.Show("Button has been Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _isNavigatingBack = true;
                            this.Close();
                        }


                    }



                    else if (selectedAction == "Show Message")
                    {
                        if (dbButtonUpdate != null)
                        {
                            listOfPendingUpdatedButtons.RemoveAll(b => b.ButtonId == activeButtonId);
                        }
                        var updatedMessageButton = new UpdateMessageButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonId = activeButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            messageId = messageButton?.MessageId ?? 0,
                            MessageAR = ArMessageTextBox.Text,
                            MessageEN = EnMessageTextBox.Text,
                        };
                        if (!checkIfExists(updatedMessageButton, activeButtonId))
                        {
                            ValidationExtensions.ValidateModel(updatedMessageButton);
                            listOfPendingUpdatedButtons.Add(updatedMessageButton);
                            _stateService.Set(listOfPendingUpdatedButtons);
                            MessageBox.Show("Button has been Updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _isNavigatingBack = true;
                            this.Close();
                        }


                    }




                }


                else if (!buttonChanged && buttonExists)
                {
                    MessageBox.Show("Button's Information are up to date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Adding A button
                else
                {

                    if (selectedAction == "Issue Ticket")
                    {
                        var newButton = new CreateTicketButtonRequestDto
                        {
                            ScreenId = screen?.ScreenId ?? 0,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ServiceId = selectedServiceType.ServiceId,
                            ButtonType = selectedButtonType.TypeId
                        };
                        ValidationExtensions.ValidateModel(newButton);
                        if (SaveOrUpdatePendingButton(newButton, nonDbButtonUpdate))
                        {
                            _isNavigatingBack = true;
                            this.Close();
                        }


                    }
                    else if (selectedAction == "Show Message")
                    {
                        var newButton = new CreateMessageButtonRequestDto
                        {
                            ScreenId = screen?.ScreenId ?? 0,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonType = selectedButtonType.TypeId,
                            MessageAR = ArMessageTextBox.Text,
                            MessageEN = EnMessageTextBox.Text
                        };
                        ValidationExtensions.ValidateModel(newButton);

                        if (SaveOrUpdatePendingButton(newButton, nonDbButtonUpdate))
                        {
                            _isNavigatingBack = true;
                            this.Close();
                        }
                    }

                }

            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
            }



            catch (Exception)
            {
                MessageBox.Show("A problem occured while editing the button", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        //Method helper for either updating an existing pending update button (originally a db button) or creating a new button 
        private bool SaveOrUpdatePendingButton(BaseButtonDto newButton, BaseButtonDto nonDbButtonUpdate)
        {
            var listOfPendingCreatedButtons = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();

            int exactIndex = nonDbButtonUpdate != null ? listOfPendingCreatedButtons.IndexOf(nonDbButtonUpdate) : -1;

            if (checkIfExists(newButton, updatingNonDbIndex: exactIndex))
            {
                return false;
            }

            if (exactIndex != -1)
            {
                listOfPendingCreatedButtons[exactIndex] = newButton;

            }
            else
            {
                listOfPendingCreatedButtons.Add(newButton);
            }

            _stateService.Set(listOfPendingCreatedButtons);

            string msg = exactIndex != -1 ? "The button has been updated" : "A new button has been created";
            MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        //A helper method to prevent duplicate button names and if found prevent user from commiting (no backend checking for duplicates)
        private bool checkIfExists(BaseButtonDto newButton, int currentButtonId = 0, int? updatingNonDbIndex = null)
        {
            var listOfPendingUpdatedButtons = _stateService.Get<List<UpdateButtonRequestDto>>() ?? new List<UpdateButtonRequestDto>();
            var listOfPendingCreatedButtons = _stateService.Get<List<BaseButtonDto>>() ?? new List<BaseButtonDto>();
            var listDbButtons = _stateService.Get<List<BaseButtonResponseDto>>() ?? new List<BaseButtonResponseDto>();
            var pendingDeletes = _stateService.Get<List<int>>() ?? new List<int>();
            var pendingUpdateIds = listOfPendingUpdatedButtons.Select(b => b.ButtonId).ToList();

            bool isDuplicateInCreated = listOfPendingCreatedButtons
                .Select((b, idx) => new { Button = b, Index = idx })
                .Any(x => (x.Button.ButtonNameEN == newButton.ButtonNameEN || x.Button.ButtonNameAR == newButton.ButtonNameAR)
                          && x.Index != updatingNonDbIndex);

            bool isDuplicateInUpdated = listOfPendingUpdatedButtons
                .Any(b => (b.ButtonNameEN == newButton.ButtonNameEN || b.ButtonNameAR == newButton.ButtonNameAR)
                && b.ButtonId != currentButtonId);

            bool isDuplicateInDb = listDbButtons
                .Any(b => (b.ButtonNameEN == newButton.ButtonNameEN || b.ButtonNameAR == newButton.ButtonNameAR)
                && b.ButtonId != currentButtonId && !pendingDeletes.Contains(b.ButtonId) && !pendingUpdateIds.Contains(b.ButtonId));

            if (isDuplicateInCreated || isDuplicateInUpdated || isDuplicateInDb)
            {
                MessageBox.Show("A button already exists with the same English or Arabic name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }


        private void AddEditButton_FormClosed(object sender, FormClosedEventArgs e)
        {
            _stateService.Clear<BaseButtonResponseDto>();
            if (_isNavigatingBack && Application.OpenForms["EditScreenForm"] is EditScreenForm editScreenForm)
            {
                editScreenForm.refreshList();
                editScreenForm.Show();
            }
            else if (!_isNavigatingBack && e.CloseReason == CloseReason.UserClosing)
            {
                Environment.Exit(0);
            }
        }


        //changes shown data dynamically based on button action  
        private void ButtonActionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHideDetail();
        }


    }
}
