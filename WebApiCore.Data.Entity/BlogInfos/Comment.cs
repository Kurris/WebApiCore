using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Data.Entity.BlogInfos
{
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public string Name { get; set; }

        public Post  Post { get; set; }
    }
}
