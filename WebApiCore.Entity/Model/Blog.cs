using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.Model
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Publish { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime EditTime { get; set; }

        public List<Post> Posts { get; set; }
    }
}
