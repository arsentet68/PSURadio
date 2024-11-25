using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PSURadio.Models;
using PSURadio.ViewModels;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailPage : ContentPage
    {
        private NewsDetailViewModel _viewModel;
        public NewsDetailPage()
        {
            InitializeComponent();
        }

        public NewsDetailPage(News news)
        {
            InitializeComponent();
            _viewModel = new NewsDetailViewModel(news);
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
            MessagingCenter.Subscribe<EditNewsViewModel, News>(this, "UpdateNews", (sender, updatedNews) =>
            {
                _viewModel.UpdateNews(updatedNews);
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            // Отписка от сообщения при закрытии страницы (рекомендуется)
            MessagingCenter.Unsubscribe<EditNewsViewModel, News>(this, "UpdateNews");
        }
    }
}
