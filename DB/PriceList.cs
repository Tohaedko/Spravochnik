using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class PriceList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public decimal? Price { get; set; }

        public int? Workingdays { get; set; }

        public virtual Location Location { get; set; }

        public virtual Service Services { get; set; }
    }
}
