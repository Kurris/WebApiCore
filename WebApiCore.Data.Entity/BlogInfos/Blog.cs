using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity.BlogInfos
{
    [Table("Blogs")]
    public class Blog : BaseEntity
    {
        [Key]
        public int BlogId { get; set; }
        public string Url { get; set; }
        public Profile Profile { get; set; }
       
        public List<Post> Posts { get; set; }
    }
}
