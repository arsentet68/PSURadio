using System;
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
    public class EditNewsViewModel : INotifyPropertyChanged
    {
        private News _news;
        private NewsService _newsService;
        public interface IPhotoPickerService
        {
            Task<byte[]> PickPhotoAsync();
        }

        public EditNewsViewModel(News news)
        {
            _news = news;
            Title = news.Title;
            PublishedDate = news.PublishedDate;
            Text = news.Text;
            ImageSource = news.ImageSource;
            ImageBytes = news.Image; // Initialize ImageBytes
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

        public News News
        {
            get => _news;
            set
            {
                _news = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand => new Command(async () => await SaveAsync());
        public Command ChangeImageCommand => new Command(async () => await ChangeImage());

        private async Task SaveAsync()
        {
            _news.Title = Title;
            _news.Text = Text;
            _news.PublishedDate = PublishedDate;

            // Здесь нет необходимости проверять ImageSource, так как массив байт уже обновляется в ChangeImage
            if (ImageSource is FileImageSource fileImageSource && fileImageSource.File == "default_image.jpg")
            {
                _news.Image = null;
            }

            await _newsService.UpdateNewsAsync(_news);
            // Отправка сообщения с обновленными данными
            MessagingCenter.Send(this, "UpdateNews", _news);
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
                News.Image = memoryStream.ToArray();

                // Установка ImageSource для отображения в интерфейсе
                ImageSource = ImageSource.FromStream(() => new MemoryStream(News.Image));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
