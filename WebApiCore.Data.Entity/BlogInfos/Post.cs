using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity
{
    [Table("Posts")]
    public class Post : BaseEntity
    {
        public int PostId { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "博客介绍在 3 到 10 个字符", MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "博客介绍在 3 到 50 个字符", MinimumLength = 3)]
        public string Introduction { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "内容至少需要 10 个字符")]
        public string Content { get; set; }
        public int Stars { get; set; }
        public int Shits { get; set; }
        public List<Comment> Comments { get; set; }
        public Blog Blog { get; set; }
    }
}
