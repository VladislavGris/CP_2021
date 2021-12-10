using CP_2021.Models.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace CP_2021.Models.ProcedureResuts.Plan
{
    internal class TaskReport : NotifiedEntity
    {
        public Guid ToId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Header { get; set; }
        public DateTime CompleteDate { get; set; }
        public string Description {  get; set; }
        public bool Completion { get; set; }
        public string ReportDescription { get; set; }
        public string ReportToName {  get; set; }
        [NotMapped]
        private bool _reportState;
        public bool ReportState
        {
            get => _reportState;
            set => Set(ref _reportState, value); 
        }
    }
}
