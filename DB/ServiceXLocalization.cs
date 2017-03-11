using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class ServiceXLocalization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ServiceName { get; set; }
        public string Remark { get; set; }
        public string Keywords { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }
     
        public virtual Localization Localization { get; set; }
       
        public virtual Service Services { get; set; }
    }
}
