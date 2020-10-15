using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.Models
{
    public class Game
    {
        public Game()
        {
            WishOrders = new List<WishOrder>(0);
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Publish { get; set; }

        public List<WishOrder> WishOrders { get; set; }
    }
}
