using PSURadio.Models;
using PSURadio.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class DeleteAuthorViewModel: BindableObject
    {
        private readonly ProfileEditService _userService;
        private string _searchText;
        public DeleteAuthorViewModel()
        {
            _userService = new ProfileEditService();
            Users = new ObservableCollection<User>();
            SearchCommand = new Command(async () => await SearchUsers());
            DemoteFromAuthorCommand = new Command<User>(async (user) => await DemoteFromAuthor(user));

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
        public ICommand DemoteFromAuthorCommand { get; }

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

        private async Task DemoteFromAuthor(User user)
        {
            // Логика для изменения роли пользователя
            user.Role = 1; // 2 - роль автора
            // Обновление данных на сервере
            bool isUpdated = await _userService.UpdateUserRole(user.Id.ToString(), user.Role);
            if (isUpdated)
            {
                // Дальнейшие действия при успешном обновлении роли
                await Application.Current.MainPage.DisplayAlert("Успех", "Пользователю присвоена роль слушателя.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Не удалось обновить роль пользователя.", "OK");
            }
        }
    }
}
