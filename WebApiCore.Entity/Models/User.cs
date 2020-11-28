using System.ComponentModel.DataAnnotations;

namespace WebApiCore.Entity.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        [MaxLength(50, ErrorMessage = "最大名称为50长度")]
        public string Name { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "不在年龄范围")]
        public int Age { get; set; }
    }
}
