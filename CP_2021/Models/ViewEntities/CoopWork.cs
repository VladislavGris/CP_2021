using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    class CoopWork : BaseViewEntity
    {
        public string ManagDoc { get; set; }
        public string Manufacturer { get; set; }
        public string SpecNum { get; set; }
        public string LetterNum { get; set; }
        public int? Count { get; set; }
    }
}
