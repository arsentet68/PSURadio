using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PSURadio.Models;
using PSURadio.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class EditPlaylistViewModel : INotifyPropertyChanged
    {
        private Playlist _playlist;
        private PlaylistsService _playlistService;

        public interface IPhotoPickerService
        {
            Task<byte[]> PickPhotoAsync();
        }

        public EditPlaylistViewModel(Playlist playlist)
        {
            _playlist = playlist;
            Title = playlist.Title;
            PublishedDate = playlist.PublishedDate;
            SongsText = string.Join(Environment.NewLine, playlist.Songs);
            Link = playlist.Link;
            ImageSource = playlist.ImageSource;
            ImageBytes = playlist.Image;
            _playlistService = new PlaylistsService();
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private DateTime _publishedDate;
        public DateTime PublishedDate
        {
            get => _publishedDate;
            set
            {
                _publishedDate = value;
                OnPropertyChanged();
            }
        }

        private string _songsText;
        public string SongsText
        {
            get => _songsText;
            set
            {
                _songsText = value;
                OnPropertyChanged();
            }
        }

        private string _link;
        public string Link
        {
            get => _link;
            set
            {
                _link = value;
                OnPropertyChanged();
            }
        }

        private byte[] _imageBytes;
        public byte[] ImageBytes
        {
            get => _imageBytes;
            set
            {
                _imageBytes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ImageSource));
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged();
            }
        }

        public Playlist Playlist
        {
            get => _playlist;
            set
            {
                _playlist = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand => new Command(async () => await SaveAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());

        private async Task SaveAsync()
        {
            _playlist.Title = Title;
            _playlist.PublishedDate = PublishedDate;
            _playlist.Songs = new List<string>(SongsText.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            _playlist.Link = Link;

            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                _playlist.Image = null;
            }

            await _playlistService.UpdatePlaylistAsync(_playlist);
            MessagingCenter.Send(this, "UpdatePlaylist", _playlist);
            await Application.Current.MainPage.Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public async Task ChangeImage()
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Выберите изображение"
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                Playlist.Image = memoryStream.ToArray();
                ImageSource = ImageSource.FromStream(() => new MemoryStream(Playlist.Image));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
