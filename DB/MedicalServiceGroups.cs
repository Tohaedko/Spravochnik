using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class MedicalServiceGroups
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string SiteCode { get; set; }

        public virtual MedicalServiceGroups ParentGroupId { get; set; }

        public virtual ICollection<ServiceXMedicalServiceGroups> ServiceXMedicalServiceGroups { get; set; }

        public virtual ICollection<MedicalServiceGroupsXLocalization> MedicalServiceGroupsXLocalization { get; set; }

        public MedicalServiceGroups()
        {
            ServiceXMedicalServiceGroups = new List<ServiceXMedicalServiceGroups>();
            MedicalServiceGroupsXLocalization = new List<MedicalServiceGroupsXLocalization>();
        }

    }
}
