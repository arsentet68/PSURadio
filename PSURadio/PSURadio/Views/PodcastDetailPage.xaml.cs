using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PSURadio.Models;
using PSURadio.ViewModels;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PodcastDetailPage : ContentPage
    {
        private PodcastDetailViewModel _viewModel;
        public PodcastDetailPage(Podcast podcast)
        {
            InitializeComponent();
            _viewModel = new PodcastDetailViewModel(podcast);
            BindingContext = _viewModel;
            // Add toolbar items based on user role
            if (_viewModel.IsMenuVisible)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    IconImageSource = "three_dots_icon.jpg",
                    Order = ToolbarItemOrder.Primary,
                    Command = new Command(async () =>
                    {
                        var action = await DisplayActionSheet("Выберите опцию", "Отмена", null, "Редактировать", "Удалить");
                        if (action == "Редактировать")
                        {
                            _viewModel.EditCommand.Execute(null);
                        }
                        else if (action == "Удалить")
                        {
                            _viewModel.DeleteCommand.Execute(null);
                        }
                    })
                });
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Подписка на сообщение об обновлении новости
            MessagingCenter.Subscribe<EditPodcastViewModel, Podcast>(this, "UpdatePodcast", (sender, updatedPodcast) =>
            {
                _viewModel.UpdatePodcast(updatedPodcast);
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Отписка от сообщения при закрытии страницы (рекомендуется)
            MessagingCenter.Unsubscribe<EditPodcastViewModel, Podcast>(this, "UpdatePodcast");
        }
    }
}