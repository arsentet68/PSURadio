using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSURadio.ViewModels
{
    internal class PodcastsViewModel
    {
        private readonly PodcastsService _podcastsService;
        public ObservableCollection<Podcast> PodcastsCollection { get; }
        public ICommand PodcastTappedCommand { get; }

        public  PodcastsViewModel()
        {
            _podcastsService = new PodcastsService();
            PodcastsCollection = new ObservableCollection<Podcast>();
            PodcastTappedCommand = new Command<Podcast>(OnPodcastTapped);
            LoadPodcasts();
        }

        public async void LoadPodcasts()
        {
            var podcastsList = await _podcastsService.GetPodcastsAsync();
            PodcastsCollection.Clear();
            foreach (var podcast in podcastsList)
            {
                PodcastsCollection.Add(podcast);
            }
        }
        private async void OnPodcastTapped(Podcast podcast)
        {
            if (podcast == null)
                return;

            // Открытие страницы деталей новости
            await Application.Current.MainPage.Navigation.PushAsync(new PodcastDetailPage(podcast));
        }
    }
}
