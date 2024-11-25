using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using PSURadio.Models;
using PSURadio.Services;
using System.Linq;

namespace PSURadio.ViewModels
{
    public class TransferAdminRightsViewModel : BindableObject
    {
        private readonly ProfileEditService _userService;
        private string _searchText;

        public TransferAdminRightsViewModel()
        {
            _userService = new ProfileEditService();
            Users = new ObservableCollection<User>();
            SearchCommand = new Command(async () => await SearchUsers());
            PromoteToAdminCommand = new Command<User>(async (user) => await PromoteToAdmin(user));

            // Загрузить всех пользователей при инициализации
            Task.Run(async () => await LoadUsers());
        }

        public ObservableCollection<User> Users { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand SearchCommand { get; }
        public ICommand PromoteToAdminCommand { get; }

        public async Task LoadUsers()
        {
            var users = await _userService.GetUsersByRoleAsync(2);
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task SearchUsers()
        {
            var users = await _userService.GetUsersByRoleAsync(2);
            var filteredUsers = users.Where(u => u.Username.ToLower().Contains(SearchText.ToLower())).ToList();

            Users.Clear();
            foreach (var user in filteredUsers)
            {
                Users.Add(user);
            }
        }

        private async Task PromoteToAdmin(User user)
        {
            // Логика для изменения роли пользователя
            user.Role = 3; // 2 - роль автора
            // Обновление данных на сервере
            bool isUpdated = await _userService.UpdateUserRole(user.Id.ToString(), user.Role);
            if (isUpdated)
            {
                // Дальнейшие действия при успешном обновлении роли
                await Application.Current.MainPage.DisplayAlert("Успех", "Пользователю присвоена роль администротора.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось обновить роль пользователя.", "OK");
            }
            SessionService.CurrentAuthResult.Role = 2;
            await _userService.UpdateUserRole(SessionService.CurrentAuthResult.Id.ToString(), 2);
        }
    }
}
