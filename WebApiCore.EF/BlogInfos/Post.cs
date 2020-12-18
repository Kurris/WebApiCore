using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Entity.BlogInfos
{
    [Table("Posts")]
    public class Post:BaseEntity
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Blog Blog { get; set; }
    }
}
