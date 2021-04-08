﻿using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("In_Production")]
    class InProductionDB : Entity
    {
        [Column("Number", TypeName ="nvarchar(30)")]
        public string Number { get; set; }
        [Column("Giving_Date", TypeName ="date")]
        public DateTime? GivingDate { get; set; }
        [Column("Executor_Name", TypeName ="nvarchar(80)")]
        public string ExecutorName { get; set; }
        [Column("Completion_Date", TypeName="date")]
        public DateTime? CompletionDate { get; set; }

    }
}
