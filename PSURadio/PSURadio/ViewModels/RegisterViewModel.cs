using PSURadio.Models;
using PSURadio.Services;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string username;
        private string email;
        private string password;
        private string confirmPassword;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string ConfirmPassword
        {
            get => confirmPassword;
            set => SetProperty(ref confirmPassword, value);
        }

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(OnRegister);
        }

        private async void OnRegister()
        {
            if (IsDataValid())
            {
                var authService = new AuthService();
                var registerService = new RegisterService();
                string hashPassword = HashService.ComputeSha256Hash(Password);
                bool isSuccess = await registerService.RegisterUser(Username, Email, hashPassword);

                if (isSuccess)
                {
                    // Perform authentication after successful registration
                    await Application.Current.MainPage.DisplayAlert("Успех", "Регистрация прошла успешно.", "OK");
                    LoginViewModel loginViewModel = new LoginViewModel();
                    await loginViewModel.PerformAuthentication(Username,hashPassword);
                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось зарегистрироваться.", "OK");
                }
            }
        }

        private bool IsDataValid()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Все поля должны быть заполнены", "OK");
                return false;
            }

            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Некорректный формат e-mail", "OK");
                return false;
            }

            if (Password != ConfirmPassword)
            {
                Application.Current.MainPage.DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return false;
            }

            return true;
        }
    }
}

