using PSURadio.Models;
using PSURadio.Services;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private readonly MessageService _messageService;
        private ObservableCollection<Message> _messages;

        public ChatPage()
        {
            InitializeComponent();
            _messageService = new MessageService();
            _messages = new ObservableCollection<Message>();
            MessagesListView.ItemsSource = _messages;
            LoadMessages();
        }

        private async void LoadMessages()
        {
            var messages = await _messageService.GetMessagesAsync();
            foreach (var message in messages)
            {
                _messages.Add(message);
            }
        }

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(MessageEntry.Text))
            {
                var message = new Message
                {
                    Text = MessageEntry.Text,
                    Sender = SessionService.CurrentAuthResult.Username,
                    TimeStamp = DateTime.Now,
                    SenderProfilePic = SessionService.CurrentAuthResult.ProfilePic
                };

                var success = await _messageService.SendMessageAsync(message);
                if (success)
                {
                    _messages.Add(message);
                    MessageEntry.Text = string.Empty;
                    // Scroll to the last message
                    MessagesListView.ScrollTo(_messages[_messages.Count - 1], ScrollToPosition.End, true);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Не удалось отправить сообщение.", "OK");
                }
            }
        }
    }
}