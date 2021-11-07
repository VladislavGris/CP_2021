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
    internal class HierarchyDB : Entity
    {
        public Guid? ParentId { get; set; }
        [NotMapped]
        public virtual ProductionTaskDB Parent { get; set; }

        public Guid ChildId { get; set; }
        [NotMapped]
        public virtual ProductionTaskDB Child { get; set; }
        
        public int LineOrder { get; set; }

        public HierarchyDB() { }

        public HierarchyDB(ProductionTaskDB parent, ProductionTaskDB child)
        {
            Parent = parent;
            Child = child;
        }

        public HierarchyDB(ProductionTaskDB child)
        {
            Parent = null;
            Child = child;
        }
    }
}
