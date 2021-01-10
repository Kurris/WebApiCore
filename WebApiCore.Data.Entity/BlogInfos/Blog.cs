using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity.BlogInfos
{
    [Table("Blogs")]
    public class Blog : BaseEntity
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public List<Post> Posts { get; set; }
    }
}
