using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PSURadio.Models;
using PSURadio.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class CreatePodcastViewModel
    {
        private readonly PodcastsService _podcastsService;
        public CreatePodcastViewModel()
        {
            Title = string.Empty;
            PublishedDate = DateTime.Now;
            Text = string.Empty;
            ImageSource = "default_image.jpg";
            ImageBytes = null; // Initialize ImageBytes
            AudioPath = string.Empty;
            AudioBytes = null;

            _podcastsService = new PodcastsService();
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

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
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
                OnPropertyChanged(nameof(ImageSource)); // Notify that ImageSource has changed
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
        private byte[] _audioBytes;
        public byte[] AudioBytes
        {
            get => _audioBytes;
            set
            {
                _audioBytes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AudioPath)); // Notify that ImageSource has changed
            }
        }
        private string _audioPath;
        public string AudioPath
        {
            get => _audioPath;
            set
            {
                _audioPath = value;
                OnPropertyChanged();
            }
        }

        public Command CreateCommand => new Command(async () => await CreateAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());

        private async Task CreateAsync()
        {
            if (string.IsNullOrWhiteSpace(Title) || AudioBytes == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "ОК");
                return;
            }

            var podcast = new Podcast
            {
                Title = Title,
                Text = Text,
                PublishedDate = PublishedDate,
                Image = ImageBytes
            };
            // Здесь нет необходимости проверять ImageSource, так как массив байт уже обновляется в ChangeImage
            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                podcast.Image = null;
            }

            await _podcastsService.AddPodcastAsync(podcast);
            MessagingCenter.Send(this, "AddPodcast", podcast);
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
        private async Task ChangeAudioAsync()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.audio" } },
                    { DevicePlatform.Android, new[] { "audio/*" } },
                    { DevicePlatform.UWP, new[] { ".mp3", ".wav", ".wma" } }
                }),
                PickerTitle = "Выберите аудиофайл"
            });

            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    AudioBytes = memoryStream.ToArray();
                    OnPropertyChanged(nameof(AudioPath));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
