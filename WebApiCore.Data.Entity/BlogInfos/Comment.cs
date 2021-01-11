using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity
{
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }

        public Post Post { get; set; }
    }
}
