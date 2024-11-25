using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class CreateMaterialViewModel : BaseViewModel
    {
        public ICommand CreateNewsCommand { get; }
        public ICommand CreatePodcastCommand { get; }
        public ICommand CreatePlaylistCommand { get; }

        public CreateMaterialViewModel()
        {
            CreateNewsCommand = new Command(CreateNews);
            CreatePodcastCommand = new Command(CreatePodcast);
            CreatePlaylistCommand = new Command(CreatePlaylist);
        }

        private async void CreateNews()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.CreateNewsPage());
        }

        private async void CreatePodcast()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.CreatePodcastPage());
        }

        private async void CreatePlaylist()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.CreatePlaylistPage());
        }
    }
}
