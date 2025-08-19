using ReportPrinter_XML_to_PDF.Models;

namespace ReportPrinter_XML_to_PDF.Interfaces
{
    internal interface IDataString
    {
        string? StaticData { get; set; }
        Tuple<string, string, DataType>[]? DynamicData { get; set; }
    }
}
