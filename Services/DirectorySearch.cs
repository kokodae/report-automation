using SerializationJSON.Services;
using System;
using System.IO;

namespace DirectorySearch.Services
{
    internal static class NewDirectorySearch
    {
        //возвращается двумерный массив списков адресов файлов в определенной директории, созданных позднее времени, сериализованного в json-файле
        //первый индекс массива указывает на то, список каких файлов был найден
        //второй индекс указывает на папку, в которой файлы были найдены
        //само название папки не требуется в дальнейшей работе, потому файлы из одной папки соответствуют именно второму индексу
        public static List<string>[,] getNewFiles(string directoryPath, string name1, string name2)
        {
            int count = 0;

            //десериализуем время из json-файла
            string jsonFilePath = @"test.json";
            DateTime date = Serialization.deserialize_date(jsonFilePath);
            //с помощью метода получаем массив адресов новых папок
            string[] newDirectories = getNewDirectrories(directoryPath, date);

            count = newDirectories.Length;
            List <string>[,] newFiles = new List<string>[2, count];

            for (int i = 0; i < newDirectories.Length; i++)
            {
                //добавляем в массив адреса новых файлов
                newFiles[0, i] = SearchFiles(newDirectories[i], name1);
                newFiles[1, i] = SearchFiles(newDirectories[i], name2);
            }

            return newFiles;
        }

        //возвращается масссив с адресами всех папок, созданных позднее указанной даты в указанной директории
        private static string[] getNewDirectrories(string directoryPath, DateTime date)
        {
            try
            {
                //получаем все директории и считаем количество новых
                string[] subdirectories = Directory.GetDirectories(directoryPath);
                int count = 0;
                foreach (string subdirectoryPath in subdirectories)
                {
                    DateTime creationDate = Directory.GetCreationTime(subdirectoryPath);
                    if (creationDate > date)
                    {
                        count = count + 1;
                    }
                }

                string[] newSubdirectories = new string[count];
                //создаем массив и помещаем туда адреса всех новых
                int i = 0;
                foreach (string subdirectoryPath in subdirectories)
                {
                    DateTime creationDate = Directory.GetCreationTime(subdirectoryPath);
                    if (creationDate > date)
                    {
                        newSubdirectories[i] = subdirectoryPath;
                        ++i;
                    }
                }
                return newSubdirectories;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //возвращает путь к первому найденному файлу с нужным именем в указанной директории, используется рекурсия
        private static string SearchFile(string directoryPath, string fileName)
        {
            try
            {
                string tempFile;
                string[] subdirectories = Directory.GetDirectories(directoryPath);
                foreach (string subdirectory in subdirectories)
                {
                    tempFile = SearchFile(subdirectory, fileName);
                    if (tempFile != null)
                    {
                        return tempFile;
                    }
                }

                string[] files = Directory.GetFiles(directoryPath, fileName);
                foreach (string file in files)
                {
                    return file;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //возвращает список путей к файлам с нужным именем в указанной директории
        //подазумевается, что каждый файл из списка в отдельной папке
        private static List<string> SearchFiles(string directoryPath, string fileName)
        {
            try
            {
                List<string> files = new List<string>();
                string tempFile;
                string[] subdirectories = Directory.GetDirectories(directoryPath);
                foreach (string subdirectory in subdirectories)
                {
                    //с помощью рекурсивной функции ищется нужный файл
                    tempFile = SearchFile(subdirectory, fileName);
                    if (tempFile != null)
                    {
                        files.Add(tempFile);
                    }
                }
                return files;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}