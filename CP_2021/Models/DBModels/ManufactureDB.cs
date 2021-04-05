using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Manufacture")]
    class ManufactureDB : Entity
    {
        [Column("M_Name")]
        public string Name { get; set; }
        [Column("Letter_Num")]
        public string LetterNum { get; set; }
        [Column("Specification_Num")]
        public string SpecNum { get; set; }
        [Column("Specification_Cost")]
        public decimal? SpecCost { get; set; }
    }
}
