using System;
using System.Windows.Input;
using Xamarin.Forms;
using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;

namespace PSURadio.ViewModels
{
    public class NewsDetailViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;
        public News News { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public bool IsMenuVisible { get; set; }

        public NewsDetailViewModel(News news)
        {
            _newsService = new NewsService();
            News = news;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);

            // Determine user role and set IsMenuVisible accordingly
            var userRole = SessionService.CurrentAuthResult?.Role ?? 0;
            IsMenuVisible = userRole == 2 || userRole == 3;
        }

        private async void OnEdit()
        {
            News news = this.News;
            await Application.Current.MainPage.Navigation.PushAsync(new EditNewsPage(news));
        }

        private async void OnDelete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Удалить", "Вы уверены, что хотите удалить эту новость?", "Да", "Нет");
            if (result)
            {
                try
                {
                    await _newsService.DeleteNewsAsync(News.Id);

                    // Optionally, send a message to refresh the news list or navigate back
                    MessagingCenter.Send(this, "DeleteNews", News);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить новость: {ex.Message}", "ОК");
                }
            }
        }
        public void UpdateNews(News updatedNews)
        {
            News = updatedNews;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(PublishedDate));
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(ImageSource));
        }

        public string Title => News.Title;
        public string Text => News.Text;
        public DateTime PublishedDate => News.PublishedDate;
        public ImageSource ImageSource => News.ImageSource;
    }
}
