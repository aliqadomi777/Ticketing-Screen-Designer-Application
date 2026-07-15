using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using Ticketing_Screen_Designer.DTO.Buttons;
using Ticketing_Screen_Designer.DTO.ButtonTypes;
using Ticketing_Screen_Designer.DTO.Screens;
using Ticketing_Screen_Designer.DTO.Services;
using Ticketing_Screen_Designer.Interfaces.Services;

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

            showHideDetail();
        }

        private void AddEditButton_Load(object sender, EventArgs e)
        {

            var button = _stateService.Get<BaseButtonResponseDto>();
            refreshForm();
            //for editing existing button
            if (button != null)
            {

                ButtonNameEnTextBox.Text = button.ButtonNameEN;
                ButtonNameArTextBox.Text = button.ButtonNameAR;
                if (button is TicketButtonResponseDto ticketButton)
                {
                    ServiceList.SelectedIndex = ServiceList.FindStringExact(ticketButton.ServiceName);

                }

                else if (button is MessageButtonResponseDto messageButton)
                {
                    EnMessageTextBox.Text = messageButton.MessageEN;
                    ArMessageTextBox.Text = messageButton.MessageAR;
                }
                refreshForm();

                ButtonActionList.SelectedIndex = ButtonActionList.FindStringExact(button.TypeName);

            }

        }

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

            if (ticketButton != null || messageButton != null)
            {

                string originalAction = button.TypeName;

                bool coreChanged = ButtonNameArTextBox.Text != button.ButtonNameAR ||
                                   ButtonNameEnTextBox.Text != button.ButtonNameEN ||
                                   originalAction != selectedAction;



                bool ticketChanged = selectedAction == "Issue Ticket" &&
                                     (selectedServiceType?.ServiceId != (ticketButton?.ServiceId));

                bool messageChanged = selectedAction == "Show Message" &&
                                      (ArMessageTextBox.Text != (messageButton?.MessageAR ?? string.Empty) ||
                                       EnMessageTextBox.Text != (messageButton?.MessageEN ?? string.Empty));



                if (ticketChanged || messageChanged || coreChanged)
                {


                    if (selectedAction == "Issue Ticket")
                    {
                        var updatedTicketButton = new UpdateTicketButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameArTextBox.Text,
                            ButtonId = button.ButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            ServiceId = selectedServiceType.ServiceId,
                            TicketId = ticketButton?.TicketId ?? 0,
                        };

                        bool isUpdated = _buttonService.UpdateButton(updatedTicketButton);
                    }

                    else if (selectedAction == "Show Message")
                    {
                        var updatedMessageButton = new UpdateMessageButtonRequest
                        {
                            ButtonNameAR = ButtonNameArTextBox.Text,
                            ButtonNameEN = ButtonNameArTextBox.Text,
                            ButtonId = button.ButtonId,
                            ButtonType = selectedButtonType.TypeId,
                            messageId = messageButton?.MessageId ?? 0,
                            MessageAR = ArMessageTextBox.Text,
                            MessageEN = EnMessageTextBox.Text,
                        };
                        bool isUpdated = _buttonService.UpdateButton(updatedMessageButton);

                    }
                }
            }



            else
            {
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
                    int newButtonId = _buttonService.AddButton(newButton);
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
                    int newButtonId = _buttonService.AddButton(newButton);
                }
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

        private void ServiceList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ButtonActionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHideDetail();
        }
    }
}
