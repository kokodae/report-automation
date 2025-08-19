using SerializationJSON.Services;
using System;
using System.IO;

namespace DirectorySearch.Services
{
    internal static class NewDirectorySearch
    {
        //������������ ��������� ������ ������� ������� ������ � ������������ ����������, ��������� ������� �������, ���������������� � json-�����
        //������ ������ ������� ��������� �� ��, ������ ����� ������ ��� ������
        //������ ������ ��������� �� �����, � ������� ����� ���� �������
        //���� �������� ����� �� ��������� � ���������� ������, ������ ����� �� ����� ����� ������������� ������ ������� �������
        public static List<string>[,] getNewFiles(string directoryPath, string name1, string name2)
        {
            int count = 0;

            //������������� ����� �� json-�����
            string jsonFilePath = @"test.json";
            DateTime date = Serialization.deserialize_date(jsonFilePath);
            //� ������� ������ �������� ������ ������� ����� �����
            string[] newDirectories = getNewDirectrories(directoryPath, date);

            count = newDirectories.Length;
            List <string>[,] newFiles = new List<string>[2, count];

            for (int i = 0; i < newDirectories.Length; i++)
            {
                //��������� � ������ ������ ����� ������
                newFiles[0, i] = SearchFiles(newDirectories[i], name1);
                newFiles[1, i] = SearchFiles(newDirectories[i], name2);
            }

            return newFiles;
        }

        //������������ ������� � �������� ���� �����, ��������� ������� ��������� ���� � ��������� ����������
        private static string[] getNewDirectrories(string directoryPath, DateTime date)
        {
            try
            {
                //�������� ��� ���������� � ������� ���������� �����
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
                //������� ������ � �������� ���� ������ ���� �����
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

        //���������� ���� � ������� ���������� ����� � ������ ������ � ��������� ����������, ������������ ��������
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

        //���������� ������ ����� � ������ � ������ ������ � ��������� ����������
        //��������������, ��� ������ ���� �� ������ � ��������� �����
        private static List<string> SearchFiles(string directoryPath, string fileName)
        {
            try
            {
                List<string> files = new List<string>();
                string tempFile;
                string[] subdirectories = Directory.GetDirectories(directoryPath);
                foreach (string subdirectory in subdirectories)
                {
                    //� ������� ����������� ������� ������ ������ ����
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