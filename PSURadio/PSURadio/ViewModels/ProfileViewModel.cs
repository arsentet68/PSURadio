using PSURadio.Services;
using PSURadio.ViewModels;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Threading.Tasks;
using PSURadio.Views;

namespace PSURadio.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private string _userRole;
        private string _email;
        private ImageSource _profileImageSource;
        private readonly ProfileEditService _profileEditService;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public string UserRole
        {
            get => _userRole;
            set
            {
                _userRole = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }


        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged();
            }
        }
        public ICommand ChangeProfileImageCommand => new Command(async () => await OnChangeProfileImage());
        public ICommand ChangeUserNameCommand => new Command(async () => await OnChangeUserName());
        public ICommand ChangeEmailCommand { get; }
        public ICommand LogoutCommand { get; }

        public ProfileViewModel()
        {
            // Initialize commands
            ChangeEmailCommand = new Command(OnChangeEmail);
            LogoutCommand = new Command(OnLogout);
            _profileEditService = new ProfileEditService();
            var session = SessionService.CurrentAuthResult;
            if (session != null)
            {
                UserName = session.Username;
                Email = session.Email;
                ProfileImageSource = session.ProfilePic != null
                    ? ImageSource.FromStream(() => new System.IO.MemoryStream(session.ProfilePic))
                    : "default_user";

                switch (session.Role)
                {
                    case 0:
                        UserRole = "Гость";
                        break;
                    case 1:
                        UserRole = "Слушатель";
                        break;
                    case 2:
                        UserRole = "Автор";
                        break;
                    case 3:
                        UserRole = "Администратор";
                        break;
                }
            }
        }
        private async Task OnChangeProfileImage()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Выберите изображение профиля"
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Отправляем изображение на сервер
                //var isSuccess = await UploadProfileImage(imageBytes);

/*                if (isSuccess)
                {
                    // Обновляем сессию и страницу профиля
                    var session = await SessionService.ReloadSession();
                    UserName = session.Username;
                    Email = session.Email;
                    ProfileImageSource = session.ProfilePic != null
                        ? ImageSource.FromStream(() => new MemoryStream(session.ProfilePic))
                        : "default_user";
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось обновить изображение профиля", "OK");
                }*/
            }
        }

        private async Task OnChangeUserName()
        {
            string newUserName = await Application.Current.MainPage.DisplayPromptAsync("Изменить имя пользователя", "Введите новое имя пользователя:");

            if (!string.IsNullOrWhiteSpace(newUserName))
            {
                int userId = SessionService.CurrentAuthResult.Id;
                bool isSuccess = await _profileEditService.UpdateUserName(userId.ToString(), newUserName);

                if (isSuccess)
                {
                    SessionService.CurrentAuthResult.Username = newUserName;
                    UserName = newUserName;
                    if (App.Current.MainPage is AppShell appShell && appShell.BindingContext is AppShellViewModel appShellViewModel)
                    {
                        appShellViewModel.UserName = newUserName;
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось изменить имя пользователя", "OK");
                }
            }
        }

        private void OnChangeEmail()
        {
            // Logic to change email
        }

        private void OnLogout()
        {
            // Logic to logout
            SessionService.CurrentAuthResult = null;
            Application.Current.MainPage = new AppShell();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
