using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.BlogInfos
{
    [Table("t_sys_post")]
    public class Post:BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime EditTime { get; set; }

        public Blog Blog { get; set; }
    }
}
