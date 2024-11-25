using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class PlaylistsViewModel : BaseViewModel
    {
        private readonly PlaylistsService _playlistsService;
        public ObservableCollection<Playlist> PlaylistsCollection { get; }
        public ICommand PlaylistTappedCommand { get; }

        public PlaylistsViewModel()
        {
            _playlistsService = new PlaylistsService();
            PlaylistsCollection = new ObservableCollection<Playlist>();
            PlaylistTappedCommand = new Command<Playlist>(OnPlaylistTapped);
            LoadPlaylists();
        }

        public async void LoadPlaylists()
        {
            var playlistsList = await _playlistsService.GetPlaylistsAsync();
            PlaylistsCollection.Clear();
            foreach (var playlist in playlistsList)
            {
                PlaylistsCollection.Add(playlist);
            }
        }
        private async void OnPlaylistTapped(Playlist playlist)
        {
            if (playlist == null)
                return;

            // Открытие страницы деталей плейлиста
            await Application.Current.MainPage.Navigation.PushAsync(new PlaylistDetailPage(playlist));
        }
    }
}


