using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Entity.Models
{
    public class WishOrder
    {
        public int UserId { get; set; }
        public int GameId { get; set; }

        public User User { get; set; }
        public Game Game { get; set; }
    }
}
