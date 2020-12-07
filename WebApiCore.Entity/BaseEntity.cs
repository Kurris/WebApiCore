using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore.IdGenerator;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCore.Entity
{
    public class BaseEntity
    {

        public BaseEntity()
        {
            Id = IdGeneratorHelper.Instance.GetId();
        }

        public long? Id { get; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }
    }
}
