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
    internal class GivingDB : Entity
    {
        [Column("G_State", TypeName ="bit")]
        public bool? State { get; set; }

        [Column("Bill", TypeName = "nvarchar(MAX)")]
        public string Bill { get; set; }

        [Column("Report", TypeName = "nvarchar(MAX)")]
        public string Report { get; set; }

        [Column("Return_Report", TypeName = "nvarchar(MAX)")]
        public string ReturnReport { get; set; }

        [Column("Receiving_Date", TypeName ="date")]
        public DateTime? ReceivingDate { get; set; }

        public bool ReturnGiving { get; set; }
        //Закупное имущество
        public string PurchaseGoods { get; set; }
        //Примечание
        public string Note { get; set; }

        [Column("Production_Task_Id")]
        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public GivingDB Clone()
        {
            GivingDB giving = new GivingDB();
            giving.State = this.State;
            giving.Bill = this.Bill;
            giving.Report = this.Report;
            giving.ReturnReport = this.ReturnReport;
            giving.ReceivingDate = this.ReceivingDate;
            giving.ReturnGiving = this.ReturnGiving;
            giving.PurchaseGoods = this.PurchaseGoods;
            giving.Note = this.Note;
            return giving;
        }
    }
}
