using PSURadio.Models;
using PSURadio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistDetailPage : ContentPage
    {
        private PlaylistDetailViewModel _viewModel;
        public PlaylistDetailPage()
        {
            InitializeComponent();
        }
        public PlaylistDetailPage(Playlist playlist)
        {
            InitializeComponent();
            _viewModel = new PlaylistDetailViewModel(playlist);
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
    }
}