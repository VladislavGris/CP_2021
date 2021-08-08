using Common.Wpf.Data;
using CP_2021.Models.Classes;
using CP_2021.Models.DBModels;
using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.PDF
{
    class PdfDocument
    {
        private static string OpenSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".pdf";
            saveFileDialog.FileName = "document";
            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog.FileName;
            }
            else
            {
                return null;
            }
        }

        private static void GenerateHeader(Section section, string header)
        {
            Paragraph paragraph = section.AddParagraph(header);
            paragraph.Format.Font.Size = 14;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph();
        }

        private static void GenerateHeaderWithDates(Section section, string header, string dateString, DateTime dateFrom, DateTime dateTo)
        {
            Paragraph paragraph = section.AddParagraph(header);
            paragraph.Format.Font.Size = 14;
            paragraph.Format.Font.Bold = true;
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            section.AddParagraph();
            paragraph = section.AddParagraph();
            paragraph.AddFormattedText(dateString + " с ");
            paragraph.AddFormattedText(dateFrom.ToShortDateString(), TextFormat.Underline);
            paragraph.AddFormattedText(" по ");
            paragraph.AddFormattedText(dateTo.ToShortDateString(), TextFormat.Underline);
            section.AddParagraph();
        }

        private static void GenerateExecutorName(Section section, string name)
        {
            Paragraph paragraph = section.AddParagraph();
            paragraph.AddFormattedText("Исполнитель: ");
            paragraph.AddFormattedText(name, TextFormat.Underline);
            section.AddParagraph();
        }

        public static void GenerateNoSpecificationReport(ObservableCollection<ProductionTaskDB> results)
        {
            string filename = OpenSaveFileDialog();
            if(filename == null)
            {
                return;
            }

            Document document = new Document();
            Section section = document.AddSection();

            GenerateHeader(section, "Отчет отсутствия спецификации");

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Количество");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Предприятие изготовитель");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Номер письма");
            cell.Format.Font.Bold = true;

            foreach(ProductionTaskDB task in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(task.ManagDoc == null ? "" : task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(task.Count == null ? "" : task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(task.Manufacture.Name == null ? "" : task.Manufacture.Name);

                cell = row.Cells[4];
                cell.AddParagraph(task.Manufacture.LetterNum == null ? "" : task.Manufacture.LetterNum);
            }
            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }

        public static void GenerateGivingReportsPDF(ObservableCollection<ProductionTaskDB> results)
        {
            string filename = OpenSaveFileDialog();
            if (filename == null)
            {
                return;
            }

            Document document = new Document();
            Section section = document.AddSection();

            GenerateHeader(section, "Отчет о предоставлении давальческих отчетов");

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Количество");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Предприятие изготовитель");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Номер спецификации");
            cell.Format.Font.Bold = true;

            cell = row.Cells[5];
            cell.AddParagraph("Срок закупки");
            cell.Format.Font.Bold = true;

            foreach (ProductionTaskDB task in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(task.ManagDoc == null ? "" : task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(task.Count == null ? "" : task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(task.Manufacture.Name == null ? "" : task.Manufacture.Name);

                cell = row.Cells[4];
                cell.AddParagraph(task.Manufacture.SpecNum == null ? "" : task.Manufacture.SpecNum);

                cell = row.Cells[5];
                cell.AddParagraph(task.Giving.ReceivingDate == null ? "" : task.Giving.ReceivingDate.Value.ToShortDateString());
            }
            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }

        public static void GenerateGivingAvailabilityPDF(TreeGridModel results, DateTime dateFrom, DateTime dateTo)
        {
            string filename = OpenSaveFileDialog();
            if (filename == null)
            {
                return;
            }

            Document document = new Document();
            Section section = document.AddSection();

            GenerateHeaderWithDates(section, "Отчет о наличии на складе предприятия давальческого для передачи", "Срок закупки", dateFrom, dateTo);

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Количество");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Ведомость комплектации");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Срок закупки");
            cell.Format.Font.Bold = true;

            foreach(ProductionTask root in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(root.Task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(root.Task.Complectation.Complectation == null ? "" : root.Task.Complectation.Complectation);

                cell = row.Cells[4];
                cell.AddParagraph(root.Task.Giving.ReceivingDate == null ? "" : root.Task.Giving.ReceivingDate.Value.ToShortDateString());
                if (root.HasChildren)
                {
                    ProductionTask child = (ProductionTask)root.Children[0];
                    row = table.AddRow();

                    cell = row.Cells[0];
                    cell.AddParagraph(child.Task.ManagDoc == null ? "" : child.Task.ManagDoc);

                    cell = row.Cells[1];
                    Paragraph paragraph = cell.AddParagraph(child.Task.Name);
                    paragraph.Format.LeftIndent = Unit.FromCentimeter(0.5);

                    cell = row.Cells[2];
                    cell.AddParagraph(child.Task.Count == null ? "" : child.Task.Count.ToString());

                    cell = row.Cells[3];
                    cell.AddParagraph(child.Task.Complectation.Complectation == null ? "" : child.Task.Complectation.Complectation);

                    cell = row.Cells[4];
                    cell.AddParagraph(root.Task.Giving.ReceivingDate == null ? "" : root.Task.Giving.ReceivingDate.Value.ToShortDateString());
                }
            }

            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }

        public static void GenerateInProductionPDF(TreeGridModel results)
        {
            string filename = OpenSaveFileDialog();
            if (filename == null)
            {
                return;
            }

            Document document = new Document();
            //document.DefaultPageSetup.Orientation = Orientation.Landscape;
            Section section = document.AddSection();

            GenerateHeader(section, "Наличие изделий в работе");

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Кол-во");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Номер МСЛ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Дата выдачи в работу");
            cell.Format.Font.Bold = true;

            cell = row.Cells[5];
            cell.AddParagraph("Сборка");
            cell.Format.Font.Bold = true;

            cell = row.Cells[6];
            cell.AddParagraph("Электромонтаж");
            cell.Format.Font.Bold = true;

            cell = row.Cells[7];
            cell.AddParagraph("Дата готовности");
            cell.Format.Font.Bold = true;

            //ProductionTask root1 = (ProductionTask)results.First();

            //for(int i = 0; i< 50; i++)
            //{
            //    row = table.AddRow();

            //    cell = row.Cells[0];
            //    cell.AddParagraph(root1.Task.ManagDoc == null ? "" : root1.Task.ManagDoc);

            //    cell = row.Cells[1];
            //    cell.AddParagraph(root1.Task.Name);

            //    cell = row.Cells[2];
            //    cell.AddParagraph(root1.Task.Count == null ? "" : root1.Task.Count.ToString());

            //    cell = row.Cells[3];
            //    cell.AddParagraph(root1.Task.InProduction.Number == null ? "" : root1.Task.InProduction.Number);

            //    cell = row.Cells[4];
            //    cell.AddParagraph(root1.Task.InProduction.GivingDate == null ? "" : root1.Task.InProduction.GivingDate.Value.ToShortDateString());

            //    cell = row.Cells[5];
            //    cell.AddParagraph(root1.Task.InProduction.ExecutorName == null ? "" : root1.Task.InProduction.ExecutorName);

            //    cell = row.Cells[6];
            //    cell.AddParagraph(root1.Task.InProduction.ExecutorName2 == null ? "" : root1.Task.InProduction.ExecutorName2);

            //    cell = row.Cells[7];
            //    cell.AddParagraph(root1.Task.InProduction.CompletionDate == null ? "" : root1.Task.InProduction.CompletionDate.Value.ToShortDateString());
            //}

            foreach (ProductionTask root in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(root.Task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(root.Task.InProduction.Number == null ? "" : root.Task.InProduction.Number);

                cell = row.Cells[4];
                cell.AddParagraph(root.Task.InProduction.GivingDate == null ? "" : root.Task.InProduction.GivingDate.Value.ToShortDateString());

                cell = row.Cells[5];
                cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                cell = row.Cells[6];
                cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                cell = row.Cells[7];
                cell.AddParagraph(root.Task.InProduction.CompletionDate == null ? "" : root.Task.InProduction.CompletionDate.Value.ToShortDateString());

                if (root.HasChildren)
                {
                    ProductionTask child = (ProductionTask)root.Children[0];
                    row = table.AddRow();

                    cell = row.Cells[0];
                    cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                    cell = row.Cells[1];
                    Paragraph paragraph = cell.AddParagraph(child.Task.Name);
                    paragraph.Format.LeftIndent = Unit.FromCentimeter(0.5);

                    cell = row.Cells[2];
                    cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                    cell = row.Cells[3];
                    cell.AddParagraph(root.Task.InProduction.Number == null ? "" : root.Task.InProduction.Number);

                    cell = row.Cells[4];
                    cell.AddParagraph(root.Task.InProduction.GivingDate == null ? "" : root.Task.InProduction.GivingDate.Value.ToShortDateString());

                    cell = row.Cells[5];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                    cell = row.Cells[6];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                    cell = row.Cells[7];
                    cell.AddParagraph(root.Task.InProduction.CompletionDate == null ? "" : root.Task.InProduction.CompletionDate.Value.ToShortDateString());
                }
            }

            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }

        public static void GenerateExecutorInProductionPDF(TreeGridModel results, DateTime dateFrom, DateTime dateTo, string executorName)
        {
            string filename = OpenSaveFileDialog();
            if (filename == null)
            {
                return;
            }

            Document document = new Document();
            Section section = document.AddSection();

            GenerateHeaderWithDates(section, "Изделия в работе по исполнителю", "Дата выдачи в работу", dateFrom, dateTo);
            GenerateExecutorName(section, executorName.ToUpper());

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4.5));
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Кол-во");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Сборка");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Электромонтаж");
            cell.Format.Font.Bold = true;

            cell = row.Cells[5];
            cell.AddParagraph("Ведомость комплектации");
            cell.Format.Font.Bold = true;

            foreach (ProductionTask root in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(root.Task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                cell = row.Cells[4];
                cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                cell = row.Cells[5];
                cell.AddParagraph(root.Task.Complectation.Complectation == null ? "" : root.Task.Complectation.Complectation);

                if (root.HasChildren)
                {
                    ProductionTask child = (ProductionTask)root.Children[0];
                    row = table.AddRow();

                    cell = row.Cells[0];
                    cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                    cell = row.Cells[1];
                    Paragraph paragraph = cell.AddParagraph(child.Task.Name);
                    paragraph.Format.LeftIndent = Unit.FromCentimeter(0.5);

                    cell = row.Cells[2];
                    cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                    cell = row.Cells[3];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                    cell = row.Cells[4];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                    cell = row.Cells[5];
                    cell.AddParagraph(root.Task.Complectation.Complectation == null ? "" : root.Task.Complectation.Complectation);
                }
            }

            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }

        public static void GenerateExecutorCompletedPDF(TreeGridModel results, DateTime dateFrom, DateTime dateTo, string executorName)
        {
            string filename = OpenSaveFileDialog();
            if (filename == null)
            {
                return;
            }

            Document document = new Document();
            Section section = document.AddSection();

            GenerateHeaderWithDates(section, "Готовые изделия по исполнителю", "Дата выдачи в работу", dateFrom, dateTo);
            GenerateExecutorName(section, executorName.ToUpper());

            Table table = new Table();
            table.Borders.Width = 0.5;

            Column column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(4.5));
            column.Format.Alignment = ParagraphAlignment.Left;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(1.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(MigraDoc.DocumentObjectModel.Unit.FromCentimeter(3));
            column.Format.Alignment = ParagraphAlignment.Center;

            Row row = table.AddRow();

            Cell cell = row.Cells[0];
            cell.AddParagraph("Распорядительный документ");
            cell.Format.Font.Bold = true;

            cell = row.Cells[1];
            cell.AddParagraph("Изделие");
            cell.Format.Font.Bold = true;

            cell = row.Cells[2];
            cell.AddParagraph("Кол-во");
            cell.Format.Font.Bold = true;

            cell = row.Cells[3];
            cell.AddParagraph("Сборка");
            cell.Format.Font.Bold = true;

            cell = row.Cells[4];
            cell.AddParagraph("Электромонтаж");
            cell.Format.Font.Bold = true;

            cell = row.Cells[5];
            cell.AddParagraph("Ведомость комплектации");
            cell.Format.Font.Bold = true;

            foreach (ProductionTask root in results)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                cell = row.Cells[1];
                cell.AddParagraph(root.Task.Name);

                cell = row.Cells[2];
                cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                cell = row.Cells[3];
                cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                cell = row.Cells[4];
                cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                cell = row.Cells[5];
                cell.AddParagraph(root.Task.Complectation.Complectation == null ? "" : root.Task.Complectation.Complectation);

                if (root.HasChildren)
                {
                    ProductionTask child = (ProductionTask)root.Children[0];
                    row = table.AddRow();

                    cell = row.Cells[0];
                    cell.AddParagraph(root.Task.ManagDoc == null ? "" : root.Task.ManagDoc);

                    cell = row.Cells[1];
                    Paragraph paragraph = cell.AddParagraph(child.Task.Name);
                    paragraph.Format.LeftIndent = Unit.FromCentimeter(0.5);

                    cell = row.Cells[2];
                    cell.AddParagraph(root.Task.Count == null ? "" : root.Task.Count.ToString());

                    cell = row.Cells[3];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName == null ? "" : root.Task.InProduction.ExecutorName);

                    cell = row.Cells[4];
                    cell.AddParagraph(root.Task.InProduction.ExecutorName2 == null ? "" : root.Task.InProduction.ExecutorName2);

                    cell = row.Cells[5];
                    cell.AddParagraph(root.Task.Complectation.Complectation == null ? "" : root.Task.Complectation.Complectation);
                }
            }

            document.LastSection.Add(table);

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
        }
    }
}
