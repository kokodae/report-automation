using System;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

//работа с конфигурационным файлом. он находится в директории программы в bin
namespace ConfigWork.Services
{
    internal static class ConfigInfo
    {
        //путь к конфигурационному файлу
        private static XDocument xdoc = XDocument.Load("config.xml");

        //возвращает информацию об элементе
        internal static string GetSingleElement(string elementName)
        {
            XElement? config = xdoc.Root;
            string singleElement = config.Element(elementName)?.Value;
            return singleElement;
        }

        //возвращает информацию о списке подэлементов строкового типа
        internal static List<string> GetListStringElements(string elementName) 
        {
            XElement? config = xdoc.Root;
            XElement? mainElement = config.Element(elementName);

            List<string> elementList = new List<string>();
            foreach (XElement? element in mainElement.Elements())
            {
                elementList.Add(element.Value);
            }

            return elementList;
        }

        //возвращает информацию о списке подэлементов целочисленного типа
        internal static List<int> GetListIntElements(string elementName)
        {
            XElement? config = xdoc.Root;
            XElement? mainElement = config.Element(elementName);

            List<int> elementList = new List<int>();
            foreach (XElement? element in mainElement.Elements())
            {
                elementList.Add(int.Parse(element.Value));
            }

            return elementList;
        }

        //возвращает информацию о шрифтах
        internal static List<XFont> GetListFonts(string elementName) 
        {
            XElement? config = xdoc.Root;
            XElement? mainElement = config.Element(elementName);

            List<XFont> fontList = new List<XFont>();
            foreach (XElement? element in mainElement.Elements())
            {
                string type = element.Attribute("type").Value;
                int size = int.Parse(element.Attribute("size").Value);
                int style = int.Parse(element.Attribute("style").Value);
                fontList.Add(new(type, size, (XFontStyle)style));
            }
            return fontList;
        }
    }
}