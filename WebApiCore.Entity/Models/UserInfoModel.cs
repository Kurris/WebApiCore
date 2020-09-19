using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApiCore.Entity.Models
{
    /// <summary>
    /// 用户信息实体
    /// </summary>
    [Table("UserInfo")]
    public class UserInfoModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Range(1, 100, ErrorMessage = "超出年龄范围")]
        public int Age { get; set; }
    }
}
