using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class ServiceXService
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        //public Service ParentServiceId { get; set; }
        public Service ComponentServiceId { get; set; }

        public virtual Service ParentServiceId { get; set; }
    }
}
