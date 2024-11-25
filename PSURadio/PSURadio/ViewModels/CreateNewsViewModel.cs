using System;
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
    public class CreateNewsViewModel : INotifyPropertyChanged
    {
        private readonly NewsService _newsService;

        public CreateNewsViewModel()
        {
            Title = string.Empty;
            Text = string.Empty;
            PublishedDate = DateTime.Now;
            ImageSource = "default_image.jpg";
            _newsService = new NewsService();
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

        public Command CreateCommand => new Command(async () => await CreateAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());

        private async Task CreateAsync()
        {
            if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Text))
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "ОК");
                return;
            }

            var news = new News
            {
                Title = Title,
                Text = Text,
                PublishedDate = PublishedDate,
                Image = ImageBytes
            };
            // Здесь нет необходимости проверять ImageSource, так как массив байт уже обновляется в ChangeImage
            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                news.Image = null;
            }

            await _newsService.AddNewsAsync(news);
            MessagingCenter.Send(this, "AddNews", news);
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
