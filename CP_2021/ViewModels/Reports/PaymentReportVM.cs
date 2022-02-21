using CP_2021.Infrastructure.PDF;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CP_2021.ViewModels.Reports
{
    internal class PaymentReportVM : BaseReport<PaymentReport>
    {
        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            FullContent = Content = new ObservableCollection<PaymentReport>(TasksOperations.GetPaymentReport());
        }

        protected override void OnGeneratePDFCommandExecuted(object p)
        {
            base.OnGeneratePDFCommandExecuted(p);
            PdfDoc doc = new PdfDoc();

            doc.AddSection().SetPageOrientationHorizontal();
            doc.AddParagraph("Отчет \"Оплата\"")
                .SetParagraphTextBold(true)
                .SetParagraphFontSize(14)
                .SetParagraphAlignment(MigraDoc.DocumentObjectModel.ParagraphAlignment.Center);

            string filterInfo = "";
            if (SelectedHead != null)
            {
                filterInfo += "Проект: " + SelectedHead;
            }
            if (SelectedManufacture != null)
            {
                if (filterInfo != "")
                {
                    filterInfo += "\t";
                }
                filterInfo += "Изготовитель: " + SelectedManufacture;
            }
            if (filterInfo != "")
            {
                filterInfo += "\n";
            }
            filterInfo += "Дата формирования: " + DateTime.Now.ToString();
            doc.AddParagraph(filterInfo)
                .SetParagraphFontSize(12)
                .SetParagraphAlignment(MigraDoc.DocumentObjectModel.ParagraphAlignment.Left);

            doc.AddTable()
                .AddTableColumns(9)
                .AddTableRow()
                .AddColumnHeaders("Договор", "Контрагент", "Протокол спецификации", "Сумма", "Проект", "Оплата 50%(1 ч.)", "Оплата 50%(2 ч.)", "Оплата 100%", "№ Акта/ТТН");

            foreach (var row in Content.ToList().OrderBy(x => x.Manufacturer))
            {
                doc.AddTableRow()
                    .AddRowCells(
                    row.Contract, 
                    row.Manufacturer, 
                    row.SpecNum, 
                    row.SpecSum, 
                    row.Project,
                    row.FirstPaymentDate.HasValue?row.FirstPaymentDate.Value.ToShortDateString():"",
                    row.SecondPaymentDate.HasValue?row.SecondPaymentDate.Value.ToShortDateString() : "",
                    row.FullPaymentDate.HasValue?row.FullPaymentDate.Value.ToShortDateString() : "",
                    row.IncDoc);
            }

            doc.Save();
        }

        public PaymentReportVM() : base() { }
    }
}
