using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Entity.BlogInfos
{
    [Table("t_sys_profile")]
    public class Profile :BaseEntity
    {

        public string Name { get; set; }

        public Blog Blog { get; set; }
    }
}
