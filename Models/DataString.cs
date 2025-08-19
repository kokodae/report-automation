using ReportPrinter_XML_to_PDF.Interfaces;

namespace ReportPrinter_XML_to_PDF.Models
{
    internal class DataString : Interfaces.IDataString
    {
        private string? _staticData;
        private Tuple<string, string, DataType>[]? _dynamicData;

        public string? StaticData { get => _staticData; set => _staticData = value; }
        public Tuple<string, string, DataType>[]? DynamicData { get => _dynamicData; set => _dynamicData = value; }
    }
}
