using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity.BlogInfos
{
    [Table("Profiles")]
    public class Profile : BaseEntity
    {
        public int ProfileId { get; set; }
        public string AvatarUrl { get; set; }
        [Required]
        public string Name { get; set; }
        public Gender? Gender { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }
        public string GithubUrl { get; set; }
        public Blog Blog { get; set; }
    }

    public enum Gender
    {
        Female = 0,
        Male = 1,
    }
}
