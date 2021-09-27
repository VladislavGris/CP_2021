using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    class BaseViewEntity
    {
        public string Task { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Project { get; set; }
        public string ParentTask { get; set; }
    }
}
