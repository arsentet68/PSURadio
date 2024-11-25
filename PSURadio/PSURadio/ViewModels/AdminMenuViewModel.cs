using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class AdminMenuViewModel : BindableObject
    {
        public ICommand NavigateCommand => new Command<string>(async (page) =>
        {
            Page nextPage = null;

            switch (page)
            {
                case "AddAuthor":
                    nextPage = new PSURadio.Views.AddAuthorPage();
                    break;
                case "DeleteAuthor":
                    nextPage = new PSURadio.Views.DeleteAuthorPage();
                    break;
                case "TransferAdminRights":
                    nextPage = new PSURadio.Views.TransferAdminRightsPage();
                    break;
            }

            if (nextPage != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(nextPage);
            }
        });
    }
}
