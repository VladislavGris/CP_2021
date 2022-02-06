using System;

namespace CP_2021.Models.ViewEntities
{
    internal class PaymentReport : BaseViewEntity
    {
        public string Contract { get; set; }
        public string SpecNum { get; set; }
        public string SpecSum { get; set; }
        public string PaymentProject { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public DateTime? SecondPaymentDate { get; set; }
        public DateTime? FullPaymentDate { get; set; }
        public string IncDoc { get; set; }
    }
}
