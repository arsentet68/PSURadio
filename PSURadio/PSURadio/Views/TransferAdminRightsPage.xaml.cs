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
    public partial class TransferAdminRightsPage : ContentPage
    {
        private TransferAdminRightsViewModel _viewModel = new TransferAdminRightsViewModel();
        public TransferAdminRightsPage()
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
                    $"Вы действительно хотите передать пользователю {user.Username} свои права администратора?", "Да", "Нет");

                if (result)
                {
                    // Логика для изменения роли пользователя
                    (BindingContext as TransferAdminRightsViewModel).PromoteToAdminCommand.Execute(user);
                    Application.Current.MainPage.Navigation.PopAsync();
                    Application.Current.MainPage.Navigation.PopAsync();
                }

                // Снятие выделения
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}