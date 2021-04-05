using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Giving")]
    class GivingDB : Entity
    {
        [Column("G_State")]
        public bool? State { get; set; }
        [Column("Bill")]
        public string Bill { get; set; }
        [Column("Report")]
        public string Report { get; set; }
        [Column("Return_Report")]
        public string ReturnReport { get; set; }
        [Column("Receiving_Date")]
        public DateTime ReceivingDate { get; set; }
    }
}
