using PSURadio.Models;
using PSURadio.Services;
using PSURadio.ViewModels;
using PSURadio.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics; // Добавляем для отладки

namespace PSURadio
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            SetFlyoutItems();
            BindingContext = new AppShellViewModel();
            // Подписка на изменения роли пользователя
            /*            MessagingCenter.Subscribe<AuthResult>(this, "UserRoleChanged", (authResult) =>
                        {
                            SetFlyoutItems();
                        });*/
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(NewsPage), typeof(NewsPage));
            Routing.RegisterRoute("register", typeof(RegisterPage));
            Routing.RegisterRoute("ProfilePage", typeof(Views.ProfilePage));
            Routing.RegisterRoute(nameof(NewsDetailPage), typeof(NewsDetailPage));
            Routing.RegisterRoute(nameof(EditNewsPage), typeof(EditNewsPage));
            Routing.RegisterRoute(nameof(PodcastsPage), typeof(PodcastsPage));
            Routing.RegisterRoute(nameof(PodcastDetailPage), typeof(PodcastDetailPage));
            Routing.RegisterRoute(nameof(EditPodcastPage), typeof(EditPodcastPage));
            Routing.RegisterRoute(nameof(PlaylistsPage), typeof(PlaylistsPage));
            Routing.RegisterRoute(nameof(PlaylistDetailPage), typeof(PlaylistDetailPage));
            Routing.RegisterRoute(nameof(EditPlaylistPage), typeof(EditPlaylistPage));
            Routing.RegisterRoute(nameof(CreateMaterialPage), typeof(CreateMaterialPage));
            Routing.RegisterRoute(nameof(CreateNewsPage), typeof(CreateNewsPage));
            Routing.RegisterRoute(nameof(CreatePodcastPage), typeof(CreatePodcastPage));
            Routing.RegisterRoute(nameof(CreatePlaylistPage), typeof(CreatePlaylistPage));
            Routing.RegisterRoute(nameof(AdminMenuPage), typeof(AdminMenuPage));
            Routing.RegisterRoute(nameof(AddAuthorPage), typeof(AddAuthorPage));
            Routing.RegisterRoute(nameof(DeleteAuthorPage), typeof(DeleteAuthorPage));
            Routing.RegisterRoute(nameof(TransferAdminRightsPage), typeof(TransferAdminRightsPage));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
            Routing.RegisterRoute(nameof(TeamPage), typeof(TeamPage));
            Routing.RegisterRoute(nameof(TeamMemberPage), typeof(TeamMemberPage));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetFlyoutItems(); // Обновляем элементы Flyout каждый раз при появлении AppShell
        }
        private void OnLogout()
        {
            // Сброс роли пользователя на "гость"
            SessionService.CurrentAuthResult = new AuthResult { Role = 0 };
            // Обновление элементов Flyout
            SetFlyoutItems();
        }
        private async void OnFlyoutHeaderTapped(object sender, EventArgs e)
        {
            // Закрываем Flyout
            Shell.Current.FlyoutIsPresented = false;

            // Переходим на страницу профиля
            await Shell.Current.GoToAsync("ProfilePage");
        }
        public void SetFlyoutItems()
        {
            var userRole = SessionService.CurrentAuthResult?.Role ?? 0;
            // Настройка видимости элементов Flyout в зависимости от роли пользователя
            GuestItem.IsVisible = userRole == 0;
            ListenerItem.IsVisible = userRole >= 1;
            AuthorItem.IsVisible = userRole >= 2; // Author (автор) и выше
            AdminItem.IsVisible = userRole >= 3; // Admin (администратор) и выше
        }
    }
}
