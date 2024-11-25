using System;
using System.Collections.Generic;
using System.Text;

namespace PSURadio.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] SenderProfilePic { get; set; }
    }
}
