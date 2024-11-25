using Xamarin.Forms;

namespace PSURadio.Controls
{
    public class ThreeDotsMenuButton : ImageButton
    {
        public ThreeDotsMenuButton()
        {
            // Установите иконку для кнопки (изображение с тремя точками)
            Source = "three_dots_icon.png"; // Укажите путь к изображению ваших трех точек
            // Установите размеры кнопки
            WidthRequest = 40;
            HeightRequest = 40;
            // Установите обработчик события нажатия на кнопку
            Clicked += ThreeDotsMenuButton_Clicked;
        }

        private void ThreeDotsMenuButton_Clicked(object sender, System.EventArgs e)
        {
            // Добавьте здесь логику для отображения меню с тремя точками
            // Например, откройте диалоговое окно с вариантами действий
        }
    }
}

