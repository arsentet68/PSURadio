using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using PSURadio.Services;
using PSURadio.Models;
using System;
using System.Threading.Tasks;
using PSURadio.Views;

namespace PSURadio.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler AuthenticationSuccess;
        private readonly AuthService authService = new AuthService();

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
            NavigateToRegisterCommand = new Command(OnNavigateToRegister);
        }

        private async void OnLogin()
        {
            string username = Username;
            string passwordHash = HashService.ComputeSha256Hash(Password);
            bool isAuthenticated = await PerformAuthentication(username, passwordHash);
            if (isAuthenticated)
            {
                AuthenticationSuccess?.Invoke(this, EventArgs.Empty);

                // Переход на главную страницу и обновление элементов Flyout
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Неверные учетные данные. Попробуйте снова.", "OK");
            }
        }
        private void OnNavigateToRegister()
        {
            // Перенаправление на страницу регистрации
            Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }
        public async Task<bool> PerformAuthentication(string username, string password)
        {
            Auth auth = new Auth { Login = username, Password = password };
            return await authService.AuthenticateAndSaveSession(auth);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}





