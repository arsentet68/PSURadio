using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PSURadio.ViewModels;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatePodcastPage : ContentPage
    {
        public CreatePodcastPage()
        {
            InitializeComponent();
            BindingContext = new CreatePodcastViewModel();
        }
    }
}