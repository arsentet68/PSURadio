using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PSURadio.ViewModels;
using PSURadio.Models;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAuthorPage : ContentPage
    {
        private AddAuthorViewModel _viewModel = new AddAuthorViewModel();
        public AddAuthorPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var user = e.SelectedItem as User;
                var result = await DisplayAlert("Подтверждение",
                    $"Вы действительно хотите дать пользователю {user.Username} права автора?", "Да", "Нет");

                if (result)
                {
                    // Логика для изменения роли пользователя
                    (BindingContext as AddAuthorViewModel).PromoteToAuthorCommand.Execute(user);
                }

                // Снятие выделения
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}