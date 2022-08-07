using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    class SpecificationsInVipisk : BaseViewEntity
    {
        public int? Count { get; set; }
        public string SpecNum { get; set; }
        public bool VipiskSpec { get; set; }
    }
}
