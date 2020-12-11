using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiCore.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.Creator = "System";
        }

        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }

#pragma warning disable CS8632 
        public string? Modifier { get; set; }
#pragma warning restore CS8632 

        public DateTime? ModifyTime { get; set; }
    }
}
