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
    public partial class NewsPage : ContentPage
    {
        private NewsViewModel _viewModel = new NewsViewModel();
        public NewsPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadNews();
        }
    }

}