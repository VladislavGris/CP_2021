using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.PDF
{
    internal class PdfDoc
    {
        private Document _pdfDocument;
        private PdfDocumentRenderer _documentRenderer;
        private string _filePath;

        public PdfDoc(string filepath)
        {
            _pdfDocument = new Document();
            _documentRenderer = new PdfDocumentRenderer();
            _documentRenderer.Document = _pdfDocument;
            _filePath = filepath;
        }



    }
}
