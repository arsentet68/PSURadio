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
    public class EditPodcastViewModel
    {
        private Podcast _podcast;
        private PodcastsService _podcastsService;
        public interface IPhotoPickerService
        {
            Task<byte[]> PickPhotoAsync();
        }
        public EditPodcastViewModel(Podcast podcast)
        {
            _podcast = podcast;
            Title = podcast.Title;
            PublishedDate = podcast.PublishedDate;
            Text = podcast.Text;
            ImageSource = podcast.ImageSource;
            ImageBytes = podcast.Image; // Initialize ImageBytes
            AudioPath = podcast.AudioPath;
            AudioBytes = podcast.Audio;

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

        public Podcast Podcast
        {
            get => _podcast;
            set
            {
                _podcast = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand => new Command(async () => await SaveAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());
        public Command ChangeAudioCommand => new Command(async () => await ChangeAudioAsync());

        private async Task SaveAsync()
        {
            _podcast.Title = Title;
            _podcast.Text = Text;
            _podcast.PublishedDate = PublishedDate;

            // Здесь нет необходимости проверять ImageSource, так как массив байт уже обновляется в ChangeImage
            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                _podcast.Image = null;
            }

            await _podcastsService.UpdatePodcastAsync(_podcast);
            // Отправка сообщения с обновленными данными
            MessagingCenter.Send(this, "UpdateNews", _podcast);
            // Возврат на два уровня назад
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

                // Сохранение данных изображения в News
                Podcast.Image = memoryStream.ToArray();

                // Установка ImageSource для отображения в интерфейсе
                ImageSource = ImageSource.FromStream(() => new MemoryStream(Podcast.Image));
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
                    _podcast.Audio = memoryStream.ToArray();
                    AudioPath = _podcast.AudioPath;
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
