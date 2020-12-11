using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore.Entity.BlogInfos
{
    [Table("Profiles")]
    public class Profile : BaseEntity
    {
        [Key]
        public int ProfileId { get; set; }
        public byte[] Avatar { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public Blog Blog { get; set; }
    }

    public enum Gender
    {
        男 = 0,
        女 = 1
    }
}
