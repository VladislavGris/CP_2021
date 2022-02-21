using CP_2021.Infrastructure.PDF;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CP_2021.ViewModels.Reports
{
    internal class TimedGivingReportVM : BaseSKBReport<TimedGiving>
    {
        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            FullContent = Content = new ObservableCollection<TimedGiving>(TasksOperations.GetTimedGiving());
        }

        protected override void OnGeneratePDFCommandExecuted(object p)
        {
            base.OnGeneratePDFCommandExecuted(p);
            PdfDoc doc = new PdfDoc();

            doc.AddSection();
            doc.AddParagraph("Отчет \"Временная выдача\"")
                .SetParagraphTextBold(true)
                .SetParagraphFontSize(14)
                .SetParagraphAlignment(MigraDoc.DocumentObjectModel.ParagraphAlignment.Center);

            string filterInfo = "";
            if (SelectedHead != null)
            {
                filterInfo += "Проект: " + SelectedHead;
            }
            if (SelectedNumber != null)
            {
                if (filterInfo != "")
                {
                    filterInfo += "\t";
                }
                filterInfo += "СКБ: " + SelectedNumber;
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
                .AddTableColumns(6)
                .AddTableRow()
                .AddColumnHeaders("Документ", "Изделие на уровень выше", "Изделие", "Дата выдачи", "ФИО", "Номер СКБ");

            foreach (var row in Content.ToList().OrderBy(x => x.Task))
            {
                doc.AddTableRow()
                    .AddRowCells(
                    row.ManagDoc, 
                    row.ParentTask, 
                    row.Task, 
                    row.GivingDate.HasValue?row.GivingDate.Value.ToShortDateString():"",
                    row.FIO,
                    row.SKBNumber);
            }

            doc.Save();
        }

        public TimedGivingReportVM() : base() { }
    }
}
