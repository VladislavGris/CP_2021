using CP_2021.Infrastructure.PDF;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace CP_2021.ViewModels.Reports
{
    internal class VKOnStorageReportVM : BaseReport<VKOnStorage>
    {
        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            FullContent = Content = new ObservableCollection<VKOnStorage>(TasksOperations.GetVKOnStorage());
        }

        protected override void OnGeneratePDFCommandExecuted(object p)
        {
            base.OnGeneratePDFCommandExecuted(p);
            PdfDoc doc = new PdfDoc();

            doc.AddSection();
            doc.AddParagraph("Отчет \"ВК на склад\"")
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
                .AddTableColumns(5)
                .AddTableRow()
                .AddColumnHeaders("Документ", "Изделие на уровень выше", "Изделие", "Количество", "Ведомость комплектации");

            foreach (var row in Content.ToList().OrderBy(x => x.Task))
            {
                doc.AddTableRow()
                    .AddRowCells(row.ManagDoc, row.ParentTask, row.Task, row.Count.ToString(), row.Complectation);
            }

            doc.Save();
        }

        public VKOnStorageReportVM() : base() { }
    }
}
