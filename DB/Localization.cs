using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Localization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [StringLength(2)]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
       
        public virtual ICollection<MedicalServiceGroupsXLocalization> MedicalServiceGroupsXLocalizations { get; set; }

        public virtual ICollection<LocationXLocalization> LocationXLocalizations { get; set; }
        public virtual ICollection<ServiceXLocalization> ServiceXLocalizations { get; set; }

        public Localization()
        {
            MedicalServiceGroupsXLocalizations = new List<MedicalServiceGroupsXLocalization>();
            LocationXLocalizations = new List<LocationXLocalization>();
            ServiceXLocalizations = new List<ServiceXLocalization>();
        }
    }
}
