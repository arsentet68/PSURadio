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
    public partial class EditPlaylistPage : ContentPage
    {
        private readonly Playlist _playlist;
        public EditPlaylistPage(Playlist playlist)
        {
            InitializeComponent();
            _playlist = playlist;
            BindingContext = new EditPlaylistViewModel(playlist);
        }
    }
}