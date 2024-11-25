using PSURadio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSURadio.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditNewsPage : ContentPage
    {
        private readonly News _news;

        public EditNewsPage(News news)
        {
            InitializeComponent();
            _news = news;
            BindingContext = new EditNewsViewModel(news);
        }
    }

}