using System;

namespace CP_2021.Models.ViewEntities
{
    internal class InWork : BaseViewEntity
    {
        public int? Count { get; set; }
        public string Complectation { get; set; }
        public string Number { get; set; }
        public string Executor1 { get; set; }
        public string Executor2 {  get; set;}
        public DateTime? GivingDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}
