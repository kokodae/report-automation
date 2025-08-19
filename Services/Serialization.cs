using DirectoryInfo;
using System;
using System.IO;
using System.Text.Json;

namespace SerializationJSON.Services
{
    internal static class Serialization
    {
        //����������� ���� �������� ��������� ����� � ����
        internal static void serialize_date(string directoryPath, string jsonFilePath)
        {
            FolderData folderData = new FolderData();
            folderData.CreationDate = Directory.GetCreationTime(directoryPath);
            string jsonString = JsonSerializer.Serialize(folderData.CreationDate);
            File.WriteAllText(jsonFilePath, jsonString);
        }

        //����������� ��������� ���� ����
        internal static void serialize_date(string jsonFilePath)
        {
            FolderData folderData = new FolderData();
            folderData.CreationDate = DateTime.Now;
            string jsonString = JsonSerializer.Serialize(folderData.CreationDate);
            File.WriteAllText(jsonFilePath, jsonString);
        }

        //������������� ���� �� �����
        internal static DateTime deserialize_date(string jsonFilePath)
        {
            string jsonFromFile = File.ReadAllText(jsonFilePath);
            FolderData folderData = new FolderData();
            folderData.CreationDate = JsonSerializer.Deserialize<DateTime>(jsonFromFile);
            DateTime deserializedCreationDate = folderData.CreationDate;
            return deserializedCreationDate;
        }
    }
}