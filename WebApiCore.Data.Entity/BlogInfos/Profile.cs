using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity.BlogInfos
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
        Female = 0,
        Male = 1,
    }
}
