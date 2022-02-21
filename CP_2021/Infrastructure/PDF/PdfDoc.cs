using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Windows;

namespace CP_2021.Infrastructure.PDF
{
    internal class PdfDoc
    {

        private Document _document;

        public PdfDoc()
        {
            _document = new Document();
        }

        public void Save()
        {
            string filename = GetFileName();
            if(filename == null)
            {
                MessageBox.Show("Не удалось сохранить отчет. Не указано имя файла");
                return;
            }
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = _document;
            renderer.RenderDocument();
            renderer.PdfDocument.Save(filename);
            Process.Start("explorer.exe", filename);
        }

        private string GetFileName()
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

        public PdfDoc AddSection()
        {
            _document.AddSection();
            _document.LastSection.PageSetup = _document.DefaultPageSetup.Clone();
            _document.LastSection.PageSetup.PageFormat = PageFormat.A4;
            _document.LastSection.PageSetup.TopMargin = Unit.FromCentimeter(1);
            _document.LastSection.PageSetup.LeftMargin = Unit.FromCentimeter(2);
            _document.LastSection.PageSetup.RightMargin = Unit.FromCentimeter(1);
            return this;
        }

        public PdfDoc SetPageOrientationHorizontal()
        {
            _document.LastSection.PageSetup.Orientation = Orientation.Landscape;
            return this;
        }

        #region Paragraph

        public PdfDoc AddParagraph(string text)
        {
            _document.LastSection.AddParagraph(text);
            return this;
        }

        public PdfDoc AddParagraphText(string text)
        {
            _document.LastSection.LastParagraph.AddText(text);
            return this;
        }

        public PdfDoc AddParagraphFormattedText(string text, TextFormat format)
        {
            _document.LastSection.LastParagraph.AddFormattedText(text, format);
            return this;
        }

        public PdfDoc SetParagraphFontSize(int fontSize)
        {
            _document.LastSection.LastParagraph.Format.Font.Size = fontSize;
            return this;
        }

        public PdfDoc SetParagraphTextBold(bool isBold)
        {
            _document.LastSection.LastParagraph.Format.Font.Bold = isBold;
            return this;
        }

        public PdfDoc SetParagraphAlignment(ParagraphAlignment alignment)
        {
            _document.LastSection.LastParagraph.Format.Alignment = alignment;
            return this;
        }

        #endregion

        #region Table

        public PdfDoc AddTable()
        {
            _document.LastSection.AddTable().Borders.Width = 0.5;

            return this;
        }

        public PdfDoc AddTableColumns(int count)
        {
            float pageSize = 0;
            if (_document.LastSection.PageSetup.Orientation == Orientation.Landscape)
            {
                pageSize = _document.LastSection.PageSetup.PageHeight;
            }
            else
            {
                pageSize = _document.LastSection.PageSetup.PageWidth;
            }
            float sectionSize = pageSize - _document.LastSection.PageSetup.LeftMargin - _document.LastSection.PageSetup.RightMargin;
            float columnSize = sectionSize / count;
            int i = 0;
            while(i++ < count)
            {
                _document.LastSection.LastTable.AddColumn().Width = columnSize;
            }
            return this;
        }

        public PdfDoc AddTableColumn()
        {
            _document.LastSection.LastTable.AddColumn();
            return this;
        }

        public PdfDoc AddTableFormattedColumn(Unit size)
        {
            _document.LastSection.LastTable.AddColumn(size);
            return this;
        }

        public PdfDoc SetColumnAlignment(ParagraphAlignment alignment)
        {
            ((Column)_document.LastSection.LastTable.Columns.LastObject).Format.Alignment = alignment;
            return this;
        }

        public PdfDoc AddTableRow()
        {
            _document.LastSection.LastTable.AddRow();
            return this;
        }

        public PdfDoc AddColumnHeaders(params string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                Cell cell = ((Row)_document.LastSection.LastTable.Rows.LastObject).Cells[i];
                cell.AddParagraph(headers[i] is null?"":headers[i]);
                cell.Format.Font.Bold = true;
            }
            return this;
        }

        public PdfDoc AddRowCells(params string[] cells)
        {
            for(int i = 0; i < cells.Length; i++)
            {
                ((Row)_document.LastSection.LastTable.Rows.LastObject).Cells[i].AddParagraph(cells[i] is null?"":cells[i]);
            }
            return this;
        }

        #endregion

    }
}
