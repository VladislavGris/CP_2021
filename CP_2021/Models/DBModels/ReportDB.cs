using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Reports")]
    internal class ReportDB :Entity
    {
        [Column(TypeName ="nvarchar(MAX)")]
        public string Description { get; set; }
        [Column(TypeName ="bit")]
        public bool State { get; set; }

        public int TaskId { get; set; }
        public virtual TaskDB Task { get; set; }

        public virtual UserDB To { get; set; }

        public ReportDB() { }

        public ReportDB(UserDB userTo, TaskDB task)
        {
            State = false;
            To = userTo;
            Task = task;
        }
    }
}
