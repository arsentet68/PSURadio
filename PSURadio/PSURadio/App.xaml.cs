using PSURadio.Services;
using PSURadio.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSURadio
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            // Инициализация роли пользователя
            if (!Current.Properties.ContainsKey("UserRole"))
            {
                Current.Properties["UserRole"] = 0; // Роль по умолчанию
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
