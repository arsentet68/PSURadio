using PSURadio.Models;
using PSURadio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteAuthorPage : ContentPage
    {
        public DeleteAuthorPage()
        {
            InitializeComponent();
            BindingContext = new DeleteAuthorViewModel();
        }
        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var user = e.SelectedItem as User;
                var result = await DisplayAlert("Подтверждение",
                    $"Вы действительно хотите забрать у пользователя {user.Username} права автора?", "Да", "Нет");

                if (result)
                {
                    // Логика для изменения роли пользователя
                    (BindingContext as DeleteAuthorViewModel).DemoteFromAuthorCommand.Execute(user);
                }

                // Снятие выделения
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}