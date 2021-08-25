using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Payment")]
    class PaymentDB : Entity
    {
        // Договор
        public string Contract { get; set; }
        // Сумма спецификации
        public decimal? SpecificationSum { get; set; }
        // Проект
        public string Project { get; set; }
        // Цена за единицу без НДС
        public decimal? PriceWithoutVAT { get; set; }
        // Примечание
        public string Note { get; set; }

        // Оплата 50% 1 часть (да/нет)
        public bool IsFirstPayment { get; set; }
        // Оплата 50% 1 часть сумма
        public decimal? FirstPaymentSum { get; set; }
        // Оплата 50% 1 часть дата
        public DateTime? FirstPaymentDate { get; set; }

        // Оплата 50% 2 часть (да/нет)
        public bool IsSecondPayment { get; set; }
        // Оплата 50% 2 часть сумма
        public decimal? SecondPaymentSum { get; set; }
        // Оплата 50% 2 часть дата
        public DateTime? SecondPaymentDate { get; set; }

        // Оплата 100% (да/нет)
        public bool IsFullPayment { get; set; }
        // Оплата 100% сумма
        public decimal? FullPaymentSum { get; set; }
        // Оплата 100% дата
        public DateTime? FullPaymentDate { get; set; }

        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public PaymentDB() { }

        public PaymentDB Clone()
        {
            PaymentDB payment = new PaymentDB();
            payment.Contract = this.Contract;
            payment.Project = this.Project;
            payment.SpecificationSum = this.SpecificationSum;
            payment.PriceWithoutVAT = this.PriceWithoutVAT;
            payment.Note = this.Note;
            payment.IsFirstPayment = this.IsFirstPayment;
            payment.IsSecondPayment = this.IsSecondPayment;
            payment.IsFullPayment = this.IsFullPayment;
            payment.FirstPaymentSum = this.FirstPaymentSum;
            payment.SecondPaymentSum = this.SecondPaymentSum;
            payment.FullPaymentSum = this.FullPaymentSum;
            payment.FirstPaymentDate = this.FirstPaymentDate;
            payment.SecondPaymentDate = this.SecondPaymentDate;
            payment.FullPaymentDate = this.FullPaymentDate;
            return payment;
        }
    }
}
