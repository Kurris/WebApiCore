using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiCore.Data.Entity.BlogInfos;

namespace WebApiCore.Data.Entity
{
    [Table("Posts")]
    public class Post : BaseEntity
    {
        public int PostId { get; set; }
        //[Required]
        //[Range(3,20,ErrorMessage = "博客标题在 3 到 20 个字符")]
        public string Title { get; set; }
        //[Required]
        //[Range(3, 50, ErrorMessage = "博客介绍在 3 到 50 个字符")]
        public string Instruction { get; set; }
        //[Required]
        //[MinLength(10,ErrorMessage = "博客内容最小需要 10 个字符")]
        public string Content { get; set; }
        public int Stars { get; set; }
        public int Shits { get; set; }
        public List<Comment> Comments { get; set; }
        public Blog Blog { get; set; }
    }
}
