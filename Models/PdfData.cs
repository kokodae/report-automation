using ConfigWork.Services;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace PdfPrintData
{
    internal class PrintData
    {
        private readonly List<int> _ident;
        private readonly List<int> _caretkaSize;
        private readonly List<XFont> _font;

        private readonly XSolidBrush _color;
        private readonly XPen _pen = XPens.Black;

        private PdfDocument? _pdfDoc;
        private PdfPage? _page;
        private XGraphics? _graph;

        public List<int> Ident => _ident;
        public List<int> CaretkaSize => _caretkaSize;
        public List<XFont> Font => _font;

        public XSolidBrush Color => _color;
        public XPen Pen => _pen;

        public PdfDocument? PdfDoc { get { return _pdfDoc; } set { _pdfDoc = value; } }
        public PdfPage? Page { get { return _page; } set { _page = value; } }
        public XGraphics? Graph { get { return _graph; } set { _graph = value; } }


        internal PrintData()
        {
            _ident = ConfigInfo.GetListIntElements("IDENTS");
            _caretkaSize = ConfigInfo.GetListIntElements("CARETKASIZES");
            _font = ConfigInfo.GetListFonts("FONTS");

            _color = XBrushes.Black;
            _pen = XPens.Black;

            _pdfDoc = new PdfDocument();
            _page = _pdfDoc.AddPage();
            _graph = XGraphics.FromPdfPage(_page);
        }
    }
}