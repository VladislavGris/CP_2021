using System;

namespace CP_2021.Models.ViewEntities
{
    class BaseViewEntity
    {
        public string Task { get; set; }
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Project { get; set; }
        public string ParentTask { get; set; }
        public string Manufacturer { get; set; }
        public string ManagDoc { get; set; }
    }
}
