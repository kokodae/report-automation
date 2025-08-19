using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ReportPrinter_XML_to_PDF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using PdfPrintData;
using System.Data;
using System.Reflection.Metadata;

namespace ReportPrinter_XML_to_PDF
{
    internal class PDFFactory
    {
        private PrintData printData;
        private static string _nameFile;
        //необходимо для логирования
        public static string NameFile { get { return _nameFile; } }

        //сама печать производится через конструктор
        internal PDFFactory(List<string> firstFileList, List<string> secondFileList)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            printData = new PrintData();
            var firstXmlDocList = new List<XDocument>();

            foreach (var file in firstFileList)
            {
                firstXmlDocList.Add(XDocument.Load(file));
            }

            var secondXmlDocList = new List<XDocument>();
            foreach (var file in secondFileList)
            {
                secondXmlDocList.Add(XDocument.Load(file));
            }
            //в данную папку сохраняются все PDF документы
            var directoryPathName = $@"out";

            //формирует PDF документ
            CreatePDF(directoryPathName, printData, firstXmlDocList, secondXmlDocList);
        }

        //формирует PDF документ
        private void CreatePDF(string dirName, PrintData printData, params List<XDocument>[] xmlDoc)
        {
            try
            {
                var head = new List<(string, string)>();
                var caretka = printData.Ident[3];

                //с помощью метода добавляется шапка
                caretka = PrintHead(head, caretka, printData.Graph, xmlDoc);

                foreach( var listDoc in xmlDoc ) 
                {
                    foreach (var doc in listDoc)
                    {
                        //для каждого xml документа производится добавление информации из него
                        caretka = DoCaretka(caretka, printData.CaretkaSize[1]);
                        caretka = PrintBody(caretka, doc);
                    }
                }

                //создается название файла и сам PDF документ
                var fullName = CreateFullName(dirName, xmlDoc);
                printData.PdfDoc.Save(fullName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //создает название PDF документа на основе информации из xml документа
        private string? CreateFullName(string dirName, params List<XDocument>[] xmlDoc)
        {
            try
            {
                //в file_name.json находится необходимая форма названия файла
                using (FileStream fileStream = new FileStream("file_name.json", FileMode.OpenOrCreate))
                {
                    var fileNameString = JsonSerializer.Deserialize<DataString>(fileStream);
                    Directory.CreateDirectory(dirName);
                    var fullName = $@"{dirName}\{fileNameString.StaticData}";

                    foreach (Tuple<string, string, DataType> dynamicDataString in fileNameString.DynamicData)
                    {
                        //с помощью метода получаем данные о нужных атрибутах
                        var dynamicData = GetDataForHead(dynamicDataString.Item1, dynamicDataString.Item2, xmlDoc);
                        if (dynamicDataString.Item3 == DataType.DateTime)
                        {
                            dynamicData = dynamicData?.Replace("-", ".");
                        }
                        fullName = $"{fullName}_{dynamicData}";
                    }

                    fullName = $"{fullName}.pdf";
                    _nameFile = Path.GetFileName(fullName);
                    return fullName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //регулирует перенос на новую страницу
        private List<string> DoShift(string text)
        {
            try
            {
                var list = new List<string>();
                var nullstring = "";
                foreach (string line in text.Split((char)10))
                {
                    foreach (string part in line.Split(' '))
                    {
                        if ($"{nullstring} {part}".Length > 125)
                        {
                            list.Add(nullstring);
                            nullstring = part;
                        }
                        else if (nullstring == "")
                            nullstring = part;
                        else
                            nullstring += $" {part}";
                    }
                    list.Add(nullstring);
                    nullstring = "";
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //добавляет в PDF документ основную информацию из xml документа
        private int PrintBody(int caretka, XDocument file)
        {
            try
            {
                printData.Graph.DrawLine(printData.Pen, printData.Ident[4], caretka + printData.CaretkaSize[1], printData.Page.Width.Point - printData.Ident[4], caretka + printData.CaretkaSize[1]);
                printData.Graph.DrawLine(printData.Pen, printData.Ident[4], caretka + printData.CaretkaSize[1] * 2, printData.Page.Width.Point - printData.Ident[4], caretka + printData.CaretkaSize[1] * 2);
                foreach (XNode node in file.DescendantNodes())
                {
                    if (node.NodeType == System.Xml.XmlNodeType.Element)
                    {
                        caretka = DoCaretka(caretka, printData.CaretkaSize[1]);
                        var element = (XElement)node;
                        printData.Graph.DrawString($"{element.Name.LocalName}:", printData.Font[2], printData.Color, new XRect(printData.Ident[4], caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopLeft);
                        foreach (XAttribute attr in element.Attributes())
                        {
                            caretka = DoCaretka(caretka, printData.CaretkaSize[1]);
                            printData.Graph.DrawString($"{attr.Name}:  {attr.Value}", printData.Font[1], printData.Color, new XRect(printData.Ident[4] + 20, caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopLeft);
                        }
                    }
                    else
                    {
                        var text = (XText)node;
                        foreach (string text_part in DoShift(text.Value))
                        {
                            caretka = DoCaretka(caretka, printData.CaretkaSize[1]);
                            printData.Graph.DrawString(text_part, printData.Font[3], printData.Color, new XRect(printData.Ident[4] + 20, caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopLeft);
                        }
                    }
                }
                return caretka;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //изменяет положение каретки
        private int DoCaretka(int caretka, int size)
        {
            try
            {
                if (caretka < printData.Page.Height.Point - 40)
                {
                    caretka += size;
                    return caretka;
                }
                else
                {
                    printData.Page = printData.PdfDoc.AddPage();
                    printData.Graph = XGraphics.FromPdfPage(printData.Page);
                    caretka = printData.Ident[3];
                    return caretka;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //добавляет в PDF документ заголовок и шапку
        private int PrintHead(List<(string, string)> head, int caretka, XGraphics? graph, params List<XDocument>[] docList)
        {
            try
            {
                //в head.json находится необходимая форма вывода данных
                using (FileStream fileStream = new FileStream("head.json", FileMode.OpenOrCreate))
                {
                    var headFile = JsonSerializer.Deserialize<DataString[]>(fileStream);
                    if (headFile != null)
                    {
                        foreach (var headString in headFile)
                        {
                            var newHeadCaption = $"{headString.StaticData}";
                            var newHeadData = "";
                            if (headString.DynamicData != null)
                            {
                                foreach (Tuple<string, string, DataType> dynamicDataString in headString.DynamicData)
                                {
                                    //получает необходимые данные из xml документов
                                    var dynamicData = GetDataForHead(dynamicDataString.Item1, dynamicDataString.Item2, docList);
                                    if (dynamicDataString.Item3 == DataType.DateTime)
                                    {
                                        dynamicData = dynamicData?.Replace("T", " ");
                                    }
                                    newHeadData = $"{newHeadData}  {dynamicData}";
                                }
                            }
                            head.Add((newHeadCaption, newHeadData));
                        }
                    }
                }
                printData.Graph.DrawString($"{head[0].Item1}{head[0].Item2}", printData.Font[0], printData.Color, new XRect(0, caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopCenter);
                caretka += printData.CaretkaSize[0];
                for (int i = 1; i < head.Count; i++)
                {
                    caretka = DoCaretka(caretka, printData.CaretkaSize[1]);
                    printData.Graph.DrawString(head[i].Item1, printData.Font[1], printData.Color, new XRect(printData.Ident[4] - printData.Page.Width.Point / 3 * 2, caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopRight);
                    printData.Graph.DrawString(head[i].Item2, printData.Font[2], printData.Color, new XRect(printData.Ident[4] + printData.Page.Width.Point / 3, caretka, printData.Page.Width.Point, printData.Page.Height.Point), XStringFormats.TopLeft);
                }
                return caretka;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //возвращает информацию об указанном атрибуте в элементе из списка xml документов, если информации нет, то возвращает null
        private string? GetDataForHead(string element, string attribute, List<XDocument>[] docList)
        {
            try
            {
                var tempDoc = new XDocument(); 
                if (docList[0].Any())
                {
                    tempDoc = docList[0].Last();
                    if (tempDoc != null)
                    {
                        var value = tempDoc.Descendants().Where(x => x.Name.LocalName == element).FirstOrDefault()?.Attributes()
                                .Where(a => a.Name.LocalName == attribute).FirstOrDefault()?.Value;
                        if (value != null) return value;
                    }
                }

                if (docList[1].Any())
                {
                    tempDoc = docList[1].Last();
                    if (tempDoc != null)
                    {
                        var value = tempDoc.Descendants().Where(x => x.Name.LocalName == element).FirstOrDefault()?.Attributes()
                                .Where(a => a.Name.LocalName == attribute).FirstOrDefault()?.Value;
                        if (value != null) return value;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
