using Data.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Report
{
    public class ToDoListReport
    {
        #region Declaration
        int _totalColumn =2;
        Document _document;
        Font _fontStyle;
        PdfPTable _pdfTable = new PdfPTable(2);
        PdfPCell _pdfPCell;
        MemoryStream _memoryStream = new MemoryStream();
        List<ToDoListVM> _todoListVMs = new List<ToDoListVM>();
        #endregion

        public byte[] PrepareReport(List<ToDoListVM> toDoLists)
        {
            _todoListVMs = toDoLists;

            #region
            //_document = new Document(PageSize.A4, 0f, 0f, 0f, 0f);
            _document = new Document(PageSize.A4, 0.5f, 0.7f, 0.2f,0.3f);
            _document.SetPageSize(PageSize.A4);
            _pdfTable.WidthPercentage = 100;
            _pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 1);
            PdfWriter.GetInstance(_document,_memoryStream);
            _document.Open();
            _pdfTable.SetWidths(new float[] { 20f, 150f});
            #endregion

            this.ReportHeader();
            this.ReportBody();
            _pdfTable.HeaderRows = 2;
            _document.Add(_pdfTable);
            _document.Close();
            return _memoryStream.ToArray();
        }
        private void ReportHeader()
        {
            _fontStyle = FontFactory.GetFont("Tahoma", 11f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TodoList",_fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();

            _fontStyle = FontFactory.GetFont("Tahoma", 9f, 1);
            _pdfPCell = new PdfPCell(new Phrase("TodoList", _fontStyle));
            _pdfPCell.Colspan = _totalColumn;
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.Border = 0;
            _pdfPCell.BackgroundColor = BaseColor.WHITE;
            _pdfPCell.ExtraParagraphSpace = 0;
            _pdfTable.AddCell(_pdfPCell);
            _pdfTable.CompleteRow();
        }
        private void ReportBody()
        {
            #region Table Header
            _fontStyle = FontFactory.GetFont("Tahoma",8f, 1);
            _pdfPCell = new PdfPCell(new Phrase("ID", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(_pdfPCell);

            _pdfPCell = new PdfPCell(new Phrase("Name", _fontStyle));
            _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
            _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
            _pdfTable.AddCell(_pdfPCell);
            #endregion

            #region Table Body
            _fontStyle = FontFactory.GetFont("Tahoma", 8f, 0);
            int serialNumber = 1;
            foreach(ToDoListVM toDoListVM in _todoListVMs)
            {
                _pdfPCell = new PdfPCell(new Phrase(serialNumber++.ToString(), _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfPCell);

                _pdfPCell = new PdfPCell(new Phrase(toDoListVM.Name, _fontStyle));
                _pdfPCell.HorizontalAlignment = Element.ALIGN_CENTER;
                _pdfPCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                _pdfPCell.BackgroundColor = BaseColor.LIGHT_GRAY;
                _pdfTable.AddCell(_pdfPCell);
            }
            #endregion
        }

        
    }
}
