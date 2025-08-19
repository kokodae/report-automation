
namespace Logger.Services
{
    internal static class LoggerNew
    {
        //��� ��������� ����� � ��������������� ����� �� ���������, � ���� ������ ������ ���������� ��� �����
        private static string GetLogFile()
        {
            string path = "log_"+ DateTime.Now.ToString("ddMMyyyy") +".txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            return path;
        }

        //������ [INFO]
        public static void LogDo(string message)
        {
            LogWrite("[INFO]", message);
        }

        //������ �� ������
        public static void LogDo(Exception ex, Boolean debug) 
        {
            LogWrite("[ERROR]", ex.Message);
            if (debug == true)
            {
                LogWrite("[DEBUG]", ex.StackTrace);
            }
        }

        //������ ��������� � log - ����������� � �����, ����������� ������
        private static void LogWrite(string type, string message)
        {
            using (StreamWriter writer = new StreamWriter(GetLogFile(), true))
                writer.WriteLine($"{DateTime.Now} - {type} {message}");
        }
    }
}