using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using PSURadio.Services;
using PSURadio.Models;
using System;
using PSURadio.Views;
using System.Threading.Tasks;

namespace PSURadio.ViewModels
{
    public class AppShellViewModel : INotifyPropertyChanged
    {
        private ImageSource _profileImageSource;
        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }

        public ICommand LogoutCommand { get; }

        public AppShellViewModel()
        {
            LogoutCommand = new Command(OnLogout);
            NavigateToProfileCommand = new Command(async () => await NavigateToProfile(), () => !IsGuest);
            UpdateUserDetails();
        }

        private async void OnLogout()
        {
            SessionService.CurrentAuthResult = null;

            UpdateUserDetails();

            App.Current.MainPage = new NavigationPage(new LoginPage());

            await Application.Current.MainPage.DisplayAlert("Выход", "Вы вышли из системы", "OK");
        }

        private void UpdateUserDetails()
        {
            if (SessionService.CurrentAuthResult != null && SessionService.CurrentAuthResult.Role != 0)
            {
                if (SessionService.CurrentAuthResult.ProfilePic != null && SessionService.CurrentAuthResult.ProfilePic.Length > 0)
                {
                    ProfileImageSource = ImageSource.FromStream(() => new System.IO.MemoryStream(SessionService.CurrentAuthResult.ProfilePic));
                }
                else
                {
                    ProfileImageSource = "default_user.png";
                }

                UserName = SessionService.CurrentAuthResult.Username;
                UserRole = GetRoleName(SessionService.CurrentAuthResult.Role);
            }
            else
            {
                ProfileImageSource = "default_user.png";
                UserName = "Неизвестный пользователь";
                UserRole = "Гость";
            }
        }
        private string GetRoleName(int role)
        {
            switch (role)
            {
                case 1:
                    return "Слушатель";
                case 2:
                    return "Автор";
                case 3:
                    return "Администратор";
                default:
                    return "Гость";
            }
        }
        public bool IsGuest => UserRole == "Гость";
        public Command NavigateToProfileCommand { get; }
        private async Task NavigateToProfile()
        {
            if (!IsGuest)
            {
                // Закрываем Flyout
                Shell.Current.FlyoutIsPresented = false;

                // Переходим на страницу профиля
                await Shell.Current.GoToAsync("ProfilePage");
            }
        }
        public void Refresh()
        {
            OnPropertyChanged(nameof(UserName)); // Обновляем свойство, связанное с именем пользователя
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
