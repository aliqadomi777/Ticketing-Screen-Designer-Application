using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.ButtonTypes;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.DTO.Services;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Utils;

namespace _2__Ticketing_Screen_Designer.UI
{
    public partial class AddEditButton : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUiStateService _stateService;
        private readonly IButtonService _buttonService;
        private readonly IServiceTypeService _serviceTypeService;
        private readonly IButtonTypeService _buttonTypeService;
        public AddEditButton(
            IServiceProvider serviceProvider,
            IUiStateService stateService,
            IButtonService buttonService,
            IServiceTypeService serviceTypeService,
            IButtonTypeService buttonTypeService
            )
        {
            _serviceProvider = serviceProvider;
            _stateService = stateService;
            _buttonService = buttonService;
            _serviceTypeService = serviceTypeService;
            _buttonTypeService = buttonTypeService;

            InitializeComponent();
        }

        private void ButtonNameEnLabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

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

        private void AddEditButton_Load(object sender, EventArgs e)
        {

            var button = _stateService.Get<BaseButtonResponseDto>();
            refreshForm();

            // Editing Existing Button
            if (button != null)
            {

                ButtonNameEnTextBox.Text = button.ButtonNameEN;
                ButtonNameArTextBox.Text = button.ButtonNameAR;
                refreshForm();
                if (button is TicketButtonResponseDto ticketButton)
                {
                    ServiceList.SelectedIndex = ServiceList.FindStringExact(ticketButton.ServiceName);

                }

                else if (button is MessageButtonResponseDto messageButton)
                {
                    EnMessageTextBox.Text = messageButton.MessageEN;
                    ArMessageTextBox.Text = messageButton.MessageAR;
                }

                ButtonActionList.SelectedIndex = ButtonActionList.FindStringExact(button.TypeName);

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

            _stateService.Clear<BaseButtonResponseDto>();

            this.Close();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            var button = _stateService.Get<BaseButtonResponseDto>();
            var screen = _stateService.Get<ScreenResponseDto>();
            string selectedAction = ButtonActionList.Text;
            var selectedButtonType = ButtonActionList.SelectedItem as ButtonTypeResponseDto;


            var selectedServiceType = ServiceList.SelectedItem as ServiceTypeResponseDto;

            var ticketButton = button as TicketButtonResponseDto;
            var messageButton = button as MessageButtonResponseDto;

            bool coreChanged = false;
            bool ticketChanged = false;
            bool messageChanged = false;
            if (ticketButton != null || messageButton != null)
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
            }
            try
            {
                //Editing A button
                if (ticketChanged || messageChanged || coreChanged)
                {
                    //Detecting button action type change is handled by Backend (Button Repository)


                    bool isUpdated = false;
                    if (selectedAction == "Issue Ticket")
                    {
                        var updatedTicketButton = new UpdateTicketButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonId = button.ButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            ServiceId = selectedServiceType.ServiceId,
                            TicketId = ticketButton?.TicketId ?? 0,
                        };

                        isUpdated = _buttonService.UpdateButton(updatedTicketButton);
                    }



                    else if (selectedAction == "Show Message")
                    {

                        var updatedMessageButton = new UpdateMessageButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonId = button.ButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            messageId = messageButton?.MessageId ?? 0,
                            MessageAR = ArMessageTextBox.Text,
                            MessageEN = EnMessageTextBox.Text,
                        };
                        isUpdated = _buttonService.UpdateButton(updatedMessageButton);



                    }
                    if (!isUpdated)
                    {
                        MessageBox.Show("This Button has been deleted by someone else");
                    }
                    else if (isUpdated)
                    {
                        MessageBox.Show("Button Edited correctly");
                    }



                }
                //Adding A button
                else
                {

                    int newButtonId = 0;
                    if (selectedAction == "Issue Ticket")
                    {

                        var newButton = new CreateTicketButtonRequestDto
                        {
                            ScreenId = screen.ScreenId,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ServiceId = selectedServiceType.ServiceId,
                            ButtonType = selectedButtonType.TypeId


                        };
                        newButtonId = _buttonService.AddButton(newButton);
                    }
                    else if (selectedAction == "Show Message")
                    {
                        var newButton = new CreateMessageButtonRequestDto
                        {
                            ScreenId = screen.ScreenId,
                            ButtonNameEN = ButtonNameEnTextBox.Text,
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonType = selectedButtonType.TypeId,
                            MessageAR = ArMessageTextBox.Text,
                            MessageEN = EnMessageTextBox.Text

                        };
                        newButtonId = _buttonService.AddButton(newButton);
                    }

                    if (newButtonId > 0)
                    {
                        MessageBox.Show("New Button has been created correctly");

                    }


                }

            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (DuplicateRecordException)
            {
                MessageBox.Show("A button with the same Name/s already exists");
            }

            catch (ParentDeletedWithChildConflictException)
            {
                MessageBox.Show("The screen holding this button has been deleted");

            }

            catch (Exception)
            {
                MessageBox.Show("A problem occured while editing the button");

            }


        }





        private void AddEditButton_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms["EditScreenForm"] is EditScreenForm editScreenForm)
            {
                editScreenForm.refreshList();
                editScreenForm.Show();
            }
        }


        private void ButtonActionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHideDetail();
        }
    }
}
