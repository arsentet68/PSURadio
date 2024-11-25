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
    public partial class PodcastsPage : ContentPage
    {
        private PodcastsViewModel _viewModel = new PodcastsViewModel();
        public PodcastsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadPodcasts();
        }
    }
}