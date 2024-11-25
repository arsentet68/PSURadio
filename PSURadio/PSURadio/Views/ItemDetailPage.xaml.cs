using PSURadio.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace PSURadio.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}