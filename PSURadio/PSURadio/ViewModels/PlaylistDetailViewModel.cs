using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    internal class PlaylistDetailViewModel
    {
        private readonly PlaylistsService _playlistsService;
        public Playlist Playlist { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public bool IsMenuVisible { get; set; }
        public PlaylistDetailViewModel(Playlist playlist)
        {
            _playlistsService = new PlaylistsService();
            Playlist = playlist;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);

            // Determine user role and set IsMenuVisible accordingly
            var userRole = SessionService.CurrentAuthResult?.Role ?? 0;
            IsMenuVisible = userRole == 2 || userRole == 3;
        }
        private async void OnEdit()
        {
            Playlist playlist = this.Playlist;
            await Application.Current.MainPage.Navigation.PushAsync(new EditPlaylistPage(playlist));
        }

        private async void OnDelete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Удалить", "Вы уверены, что хотите удалить этот плейлист?", "Да", "Нет");
            if (result)
            {
                try
                {
                    await _playlistsService.DeletePlaylistAsync(Playlist.Id);

                    // Optionally, send a message to refresh the news list or navigate back
                    MessagingCenter.Send(this, "DeletePlaylist", Playlist);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить плейлист: {ex.Message}", "ОК");
                }
            }
        }
        public ICommand OpenLinkCommand => new Command<string>((link) => {
            Device.OpenUri(new Uri(link));
        });
        public string Title => Playlist.Title;
        public List<string> Songs => Playlist.Songs;
        public DateTime PublishedDate => Playlist.PublishedDate;
        public ImageSource ImageSource => Playlist.ImageSource;
        public string Link => Playlist.Link;
    }
}
