using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    class HierarchyDB : Entity
    {
        public int ParentId { get; set; }
        public ProductionTaskDB Parent { get; set; }

        [Required]
        public int ChildId { get; set; }
        public ProductionTaskDB Child { get; set; }

        public HierarchyDB() { }

        public HierarchyDB(ProductionTaskDB parent, ProductionTaskDB child)
        {
            Parent = parent;
            Child = child;
        }
    }
}
