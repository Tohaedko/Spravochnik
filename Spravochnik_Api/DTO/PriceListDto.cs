using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spravochnik_Api.DTO
{
    public class PriceListDto
    {
        public long LocationId { get; set; }
        public long ServiceId { get; set; }
        public Decimal Price { get; set; }
        public int? Workingdays { get; set; }
    }
}