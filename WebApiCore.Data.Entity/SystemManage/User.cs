using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Data.Entity.SystemManage
{
    [Table("Users")]
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public DateTime? LastLogin { get; set; }

        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public DateTime? RefreshTime { get; set; }
    }
}
