using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class ServiceXMedicalServiceGroups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        public virtual Service Service { get; set; }
   
        public virtual MedicalServiceGroups MedicalServiceGroups { get; set; }
    }
}
