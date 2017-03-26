using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spravochnik_Api.DTO
{
    public class MedicalServiceGroupDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ParentGroupId { get; set; }
    }
}