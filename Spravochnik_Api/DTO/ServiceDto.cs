using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spravochnik_Api.DTO
{
    public class ServiceDto
    {
        public long Id;
        public string Code;
        public string Name;
        public string Description;
        public string Keywords;
        public string Remark;
        public long[] MedicalServiceGroups;
        public long[] Components;
        public bool IsDiscountAllowed;
    }
}