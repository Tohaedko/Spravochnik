using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class MedicalServiceGroupsXLocalization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string MedicalServiceGroupsName { get; set; }
        public virtual MedicalServiceGroups MedicalServiceGroups { get; set; }
        public virtual Localization Localization { get; set; }
    }
}
