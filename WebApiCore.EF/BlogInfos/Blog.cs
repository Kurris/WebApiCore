using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.BlogInfos
{
    [Table("t_sys_blog")]
    public class Blog :BaseEntity
    {
        public string Url { get; set; }

        public BlogType BlogType { get; set; }

        public List<Post> Posts { get; set; }
    }

    public enum BlogType
    {
        CSDN = 0,
        MSDN = 1
    }
}
