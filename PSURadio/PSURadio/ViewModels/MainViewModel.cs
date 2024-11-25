using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using Xamarin.Forms;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace PSURadio.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;

        public ObservableCollection<News> RecentNews { get; }
        public Command<News> NavigateToNewsDetailCommand { get; }
        private readonly PodcastsService _podcastsService;
        public ObservableCollection<Podcast> RecentPodcasts { get; }
        public Command <Podcast> NavigateToPodcastDetailCommand {  get; }
        public MainViewModel()
        {
            _newsService = new NewsService();
            RecentNews = new ObservableCollection<News>();
            NavigateToNewsDetailCommand = new Command<News>(async (news) => await NavigateToNewsDetail(news));
            LoadRecentNews();
            _podcastsService = new PodcastsService();
            RecentPodcasts = new ObservableCollection<Podcast>();
            NavigateToPodcastDetailCommand = new Command<Podcast>(async (podcast) => await NavigateToPodcastDetail(podcast));
            LoadRecentPodcasts();
            // Инициализация
            IsUserAuthenticated = SessionService.CurrentAuthResult != null;
        }
        public async void LoadRecentNews()
        {
            var newsList = await _newsService.GetNewsAsync();
            foreach (var news in newsList)
            {
                RecentNews.Add(news);
            }
        }
        public async void LoadRecentPodcasts()
        {
            IsBusy = true;
            try
            {
                var podcastsList = await _podcastsService.GetPodcastsAsync();
                var recentPodcasts = podcastsList.OrderByDescending(n => n.PublishedDate)
                                         .Take(3)
                                         .ToList();

                foreach (var podcast in recentPodcasts)
                {
                    RecentPodcasts.Add(podcast);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task NavigateToNewsDetail(News news)
        {
            if (news != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new NewsDetailPage(news));
            }
        }
        private async Task NavigateToPodcastDetail(Podcast podcast)
        {
            if (podcast != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new PodcastDetailPage(podcast));
            }
        }

        private bool _isUserAuthenticated;
        public bool IsUserAuthenticated
        {
            get => _isUserAuthenticated;
            set
            {
                _isUserAuthenticated = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
