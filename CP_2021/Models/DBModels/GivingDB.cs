﻿using CP_2021.Models.Base;
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
        [Column("G_State", TypeName ="bit")]
        public bool? State { get; set; }

        [Column("Bill", TypeName ="nvarchar(50)")]
        public string Bill { get; set; }

        [Column("Report", TypeName ="nvarchar(50)")]
        public string Report { get; set; }

        [Column("Return_Report", TypeName ="nvarchar(50)")]
        public string ReturnReport { get; set; }

        [Column("Receiving_Date", TypeName ="date")]
        public DateTime? ReceivingDate { get; set; }

        [Column("Production_Task_Id")]
        public int ProductionTaskId { get; set; }
        public ProductionTaskDB ProductionTask { get; set; }

        public GivingDB Clone()
        {
            GivingDB giving = new GivingDB();
            giving.State = this.State;
            giving.Bill = this.Bill;
            giving.Report = this.Report;
            giving.ReturnReport = this.ReturnReport;
            giving.ReceivingDate = this.ReceivingDate;
            return giving;
        }
    }
}
