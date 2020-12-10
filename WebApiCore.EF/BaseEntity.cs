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

        public DateTime CreateTime { get; set; }

        public DateTime? ModifyTime { get; set; }
    }
}
