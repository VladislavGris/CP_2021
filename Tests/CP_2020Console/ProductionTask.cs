using System;

namespace Tasks
{
    class ProductionTask
    {
        private bool State { get; set; }                // Выполнено\Не выполнено
        private string Payment { get; set; }            // Оплата
        private string Solution { get;set; }            // Решение
        private string TaskName { get; set; }           // Название задачи
        private int Count { get; set; }                 // Количество
        private DateTime ProductionTime { get; set; }   // Срок изготовления
        private string PickingList { get; set; }        // Ведомость комплектации
        private string Receiving { get; set; }          // Получение комплектации
        private string ForProduction { get; set; }      // Выдача в производство
        private string Spending { get; set; }           // Акт расходования

        public ProductionTask(bool state, string payment, string solution, string name, int count, DateTime time, string list, string receiving, string production, string spending)
        {
            State = state;
            Payment = payment;
            Solution = solution;
            TaskName = name;
            Count = count;
            ProductionTime = time;
            PickingList = list;
            Receiving = receiving;
            ForProduction = production;
            Spending = spending;
        }
    }
}
