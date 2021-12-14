using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class PaymentWindowEntity : Entity
    {
        public string Contract { get; set; }
        public string SpecificationSum { get; set; }
        public string Project { get; set; }
        public string PriceWithoutVAT { get; set; }
        public string Note { get; set; }
        public string SpecNum { get; set; }

        public bool IsFirstPayment { get; set; }
        public string FirstPaymentSum { get; set; }
        public DateTime? FirstPaymentDate { get; set; }

        public bool IsSecondPayment { get; set; }
        public string SecondPaymentSum { get; set; }
        public DateTime? SecondPaymentDate { get; set; }

        public bool IsFullPayment { get; set; }
        public string FullPaymentSum { get; set; }
        public DateTime? FullPaymentDate { get; set; }
    }
}
