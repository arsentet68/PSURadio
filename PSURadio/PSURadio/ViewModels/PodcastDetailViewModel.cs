using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Core;

namespace PSURadio.ViewModels
{
    public class PodcastDetailViewModel:BaseViewModel
    {
        private readonly PodcastsService _podcastsService;
        public Podcast Podcast { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public bool IsMenuVisible { get; set; }

        public PodcastDetailViewModel(Podcast podcast)
        {
            _podcastsService = new PodcastsService();
            Podcast = podcast;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);

            // Determine user role and set IsMenuVisible accordingly
            var userRole = SessionService.CurrentAuthResult?.Role ?? 0;
            IsMenuVisible = userRole == 2 || userRole == 3;
        }

        private async void OnEdit()
        {
            Podcast podcast = this.Podcast;
            await Application.Current.MainPage.Navigation.PushAsync(new EditPodcastPage(podcast));
        }

        private async void OnDelete()
        {
            var result = await Application.Current.MainPage.DisplayAlert("Удалить", "Вы уверены, что хотите удалить этот подкаст?", "Да", "Нет");
            if (result)
            {
                try
                {
                    await _podcastsService.DeletePodcastAsync(Podcast.Id);

                    // Optionally, send a message to refresh the news list or navigate back
                    MessagingCenter.Send(this, "DeletePodcast", Podcast);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", $"Не удалось удалить подкаст: {ex.Message}", "ОК");
                }
            }
        }
        public void UpdatePodcast(Podcast updatedPodcast)
        {
            Podcast = updatedPodcast;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(PublishedDate));
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(ImageSource));
        }

        public string Title => Podcast.Title;
        public string Text => Podcast.Text;
        public DateTime PublishedDate => Podcast.PublishedDate;
        public ImageSource ImageSource => Podcast.ImageSource;
        public string AudioPath => Podcast.AudioPath;
    }
}
