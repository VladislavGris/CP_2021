using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Task_Users")]
    class TaskUsersDB :Entity
    {
        public int TaskId { get; set; }
        public TaskDB Task { get; set; }

        public int SenderId { get; set; }
        public UserDB Sender { get; set; }

        public int ReceiverId { get; set; }
        public UserDB Receiver { get; set; }
    }
}
