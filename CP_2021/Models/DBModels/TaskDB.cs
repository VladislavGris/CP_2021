using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Tasks")]
    internal class TaskDB : Entity
    {
        [Column("To_Id")]
        public int ToId { get; set; }
        public virtual UserDB To { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(60)")]
        public string Header { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CompleteDate { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
        [Column(TypeName = "bit")]
        public bool Completion { get; set; }

        public virtual ReportDB Report { get; set; }
    }
}
