using System;

namespace CP_2021.Models.ViewEntities
{
    class BaseViewEntity
    {
        public string Task { get; set; }    // Название выбранного изделия
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Project { get; set; } // Название самого верхнего изделия
        public string ParentTask { get; set; }
        public string Manufacturer { get; set; }
        public string ManagDoc { get; set; }
        public string SubProject { get; set; } // Название изделия под самым верхним изделием
    }
}
