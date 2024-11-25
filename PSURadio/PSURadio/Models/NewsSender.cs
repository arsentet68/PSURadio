using System;
using System.Collections.Generic;
using System.Text;

namespace PSURadio.Models
{
    public class NewsSender
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public byte[] Image { get; set; } // Изображение в формате массива байт
        public DateTime PublishedDate { get; set; }
    }
}
