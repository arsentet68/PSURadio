using System;
using Xamarin.Forms;
using System.IO;
using System.Threading;

namespace PSURadio.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; } // Изображение в формате массива байт
        public DateTime PublishedDate { get; set; }

        public ImageSource ImageSource
        {
            get
            {
                if (Image == null || Image.Length == 0)
                {
                    return ImageSource.FromFile("default_image.jpg");
                }
                return ImageSource.FromStream(() => new MemoryStream(Image));
            }
            set
            {
                if (value is StreamImageSource streamImageSource)
                {
                    using (var stream = streamImageSource.Stream(CancellationToken.None).Result)
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        Image = memoryStream.ToArray();
                    }
                }
            }
        }
    }
}


