using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Entity.Models
{
    public class HomePage
    {
        public int Id { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Introduction { get; set; }

    }
}
