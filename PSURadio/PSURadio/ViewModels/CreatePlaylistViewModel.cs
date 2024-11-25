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
    public class CreatePlaylistViewModel : INotifyPropertyChanged
    {
        private Playlist _playlist;
        private PlaylistsService _playlistService;

        public interface IPhotoPickerService
        {
            Task<byte[]> PickPhotoAsync();
        }

        public CreatePlaylistViewModel()
        {
            Title = string.Empty;
            PublishedDate = DateTime.Now;
            SongsText = string.Empty;
            Link = string.Empty;
            ImageSource = "default_image.jpg";
            ImageBytes = null;
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
        public Command CreateCommand => new Command(async () => await CreateAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());

        private async Task CreateAsync()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(SongsText))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "ОК");
                return;
            }

            var playlist = new Playlist
            {
                Title = Title,
                Songs = new List<string>(SongsText.Split(new[] { Environment.NewLine }, StringSplitOptions.None)),
                PublishedDate = PublishedDate,
                Image = ImageBytes,
                Link = Link
            };
            // Здесь нет необходимости проверять ImageSource, так как массив байт уже обновляется в ChangeImage
            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                playlist.Image = null;
            }

            await _playlistService.AddPlaylistAsync(playlist);
            MessagingCenter.Send(this, "AddPlaylist", playlist);
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

                ImageBytes = memoryStream.ToArray();
                ImageSource = ImageSource.FromStream(() => new MemoryStream(ImageBytes));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
