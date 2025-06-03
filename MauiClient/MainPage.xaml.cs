using MauiClient.Service;

namespace MauiClient
{
    public partial class MainPage : ContentPage
    {
        private ChatService _chatService = new ChatService();
        private string _username = "";
        private List<string> _chatVerlauf = new();
        private bool isVerbunden = false;

        public MainPage()
        {
            InitializeComponent();
            _chatService.NachrichtEmpfangen += OnNachrichtEmpfangen;
            _chatService.BenutzerlisteAktualisiert += OnBenutzerlisteAktualisiert;

        }

        private async void OnVerbindenClicked(object sender, EventArgs e)
        {
            _username = UsernameEntry.Text?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(_username))
            {
                isVerbunden = true;
                await _chatService.StartAsync(_username);
            }
        }

        private async void OnSendenClicked(object sender, EventArgs e)
        {
            var empfaenger = UserPicker.SelectedItem?.ToString();
            var nachricht = NachrichtEntry.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(empfaenger) && !string.IsNullOrWhiteSpace(nachricht))
            {
                await _chatService.SendMessageToUser(empfaenger, nachricht);
                AddNachricht($"Du → {empfaenger}: {nachricht}");
                NachrichtEntry.Text = "";
            }
        }

        private void OnNachrichtEmpfangen(string sender, string message)
        {
            AddNachricht($"{sender} → Du: {message}");
        }

        private void OnBenutzerlisteAktualisiert(List<string> users)
        {
            // Nur andere Benutzer anzeigen
            users.Remove(_username);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UserPicker.ItemsSource = users;
                isVerbunden = true;
                VerbundenButton.IsEnabled = !isVerbunden;
            });
        }

        private void AddNachricht(string text)
        {
            _chatVerlauf.Add(text);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ChatVerlauf.ItemsSource = null;
                ChatVerlauf.ItemsSource = _chatVerlauf;
            });
        }

    }
}
