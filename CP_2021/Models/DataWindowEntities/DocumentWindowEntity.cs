using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class DocumentWindowEntity : Entity
    {
        public string ManagDoc { get; set; }
        public DateTime? VishDate { get; set; }
        public DateTime? RealDate { get; set; }
    }
}
