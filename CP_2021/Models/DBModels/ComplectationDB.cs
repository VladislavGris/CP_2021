using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Complectation")]
    class ComplectationDB : Entity
    {
        [Column("Complectation")]
        public string Complectation { get; set; }
        [Column("C_Date")]
        public DateTime ComplectationDate { get; set; }
        [Column("Comp_Percentage")]
        public float? Percentage { get; set; }
    }
}
