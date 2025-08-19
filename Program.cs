using System;
using System.IO;
using ReportPrinter_XML_to_PDF;
using SerializationJSON.Services;
using DirectorySearch.Services;
using ConfigWork.Services;
using Logger.Services;

string jsonFilePath = @"test.json";
//запуск программы
try
{
    DateTime date = Serialization.deserialize_date(jsonFilePath);
    LoggerNew.LogDo("Метка времени получена");

    List<string> files = ConfigInfo.GetListStringElements("FILENAMES");
    string mainpath = ConfigInfo.GetSingleElement("MAINPATH");
    LoggerNew.LogDo("Данные конфигурационного файла получены");

    List<string>[,] newFiles = NewDirectorySearch.getNewFiles(mainpath, files[0], files[1]);
    if (newFiles.Length != 0)
    {
        LoggerNew.LogDo("Новые файлы найдены");
        for (int k = 0; k < newFiles.Length / 2; k++)
        {
            new PDFFactory(newFiles[0, k], newFiles[1, k]);
            string message = $"Печать документа успешно завершена: {PDFFactory.NameFile}";
            LoggerNew.LogDo(message);
        }
        //сериализация новой даты
        Serialization.serialize_date(jsonFilePath);
    }
    else
        LoggerNew.LogDo("Новые файлы не найдены");
}
catch (Exception ex)
{
    LoggerNew.LogDo(ex, true);
    return;
}