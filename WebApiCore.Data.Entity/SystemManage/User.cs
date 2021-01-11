using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity
{
    [Table("Users")]
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime? LastLogin { get; set; }



        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public DateTime? RefreshTime { get; set; }
    }
}
