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
    public partial class EditPodcastPage : ContentPage
    {
        private readonly Podcast _podcast;
        public EditPodcastPage(Podcast podcast)
        {
            InitializeComponent();
            _podcast = podcast;
            BindingContext = new EditPodcastViewModel(podcast);
        }
    }
}