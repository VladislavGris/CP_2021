﻿using CP_2021.Models.Base;
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
    class TaskDB : Entity
    {
        [Column("From_Id")]
        public int FromId { get; set; }
        public UserDB From { get; set; }
        [Column("To_Id")]
        public int ToId { get; set; }
        public UserDB To { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Header { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CompleteDate { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string Description { get; set; }
    }
}