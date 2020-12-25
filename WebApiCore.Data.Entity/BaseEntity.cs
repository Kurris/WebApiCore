using System;

namespace WebApiCore.Data.Entity
{
    public class BaseEntity
    {
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }

#pragma warning disable CS8632 
        public string? Modifier { get; set; }
#pragma warning restore CS8632 

        public DateTime? ModifyTime { get; set; }
    }
}
