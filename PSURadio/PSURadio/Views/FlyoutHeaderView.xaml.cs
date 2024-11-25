using PSURadio.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutHeaderView : ContentView
    {
        public FlyoutHeaderView()
        {
            InitializeComponent();
            BindingContext = (App.Current.MainPage as AppShell)?.BindingContext as AppShellViewModel;
        }
    }
}