using System;
using System.Collections.Generic;
using System.Text;

namespace PSURadio.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int Role {get; set; }
        public byte[] ProfilePic { get; set; }
    }
}
