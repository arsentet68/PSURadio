using PSURadio.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
    public partial class MainPage : ContentPage
    {
        private MainViewModel _viewModel = new MainViewModel();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
            ChatButton.Clicked += async (s, e) => await Navigation.PushAsync(new ChatPage());
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadRecentNews();
            _viewModel.LoadRecentPodcasts();
            BindingContext = _viewModel;
        }
    }
}