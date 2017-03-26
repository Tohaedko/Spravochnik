using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string Code { get; set; }

        //public virtual ICollection<Service> Services { get; set; }

        public virtual ICollection<LocationXLocalization> LocationXLocalizations { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; set; }

        public Location()
        {
            //Services = new List<Service>();
            LocationXLocalizations = new List<LocationXLocalization>();
            PriceLists = new List<PriceList>();
        }

    }
}
