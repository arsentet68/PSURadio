using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;
        public ObservableCollection<News> NewsCollection { get; }
        public ICommand NewsTappedCommand { get; }

        public NewsViewModel()
        {
            _newsService = new NewsService();
            NewsCollection = new ObservableCollection<News>();
            NewsTappedCommand = new Command<News>(OnNewsTapped);
            LoadNews();
        }

        public async void LoadNews()
        {
            var newsList = await _newsService.GetNewsAsync();
            NewsCollection.Clear();
            foreach (var news in newsList)
            {
                NewsCollection.Add(news);
            }
        }
        private async void OnNewsTapped(News news)
        {
            if (news == null)
                return;

            // Открытие страницы деталей новости
            await Application.Current.MainPage.Navigation.PushAsync(new NewsDetailPage(news));
        }
    }
}


