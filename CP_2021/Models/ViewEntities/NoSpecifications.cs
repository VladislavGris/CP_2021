using Microsoft.EntityFrameworkCore;
using System;

namespace CP_2021.Models.ViewEntities
{
    class NoSpecifications : BaseViewEntity
    {
        public string Manufacturer { get; set; }
        public string LetterNum { get; set; }
        public string SpecNum { get; set; }
    }
}
