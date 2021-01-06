using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiCore.Data.Entity.BlogInfos;

namespace WebApiCore.Data.Entity
{
    [Table("Posts")]
    public class Post : BaseEntity
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Stars { get; set; }
        public int Shits { get; set; }
        public List<Comment> Comments { get; set; }
        public Blog Blog { get; set; }
    }
}
