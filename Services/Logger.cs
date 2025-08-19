
namespace Logger.Services
{
    internal static class LoggerNew
    {
        //при отсутсвии файла с соответствующей датой он создается, в ином случае просто передается его адрес
        private static string GetLogFile()
        {
            string path = "log_"+ DateTime.Now.ToString("ddMMyyyy") +".txt";
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            return path;
        }

        //запись [INFO]
        public static void LogDo(string message)
        {
            LogWrite("[INFO]", message);
        }

        //запись об ошибке
        public static void LogDo(Exception ex, Boolean debug) 
        {
            LogWrite("[ERROR]", ex.Message);
            if (debug == true)
            {
                LogWrite("[DEBUG]", ex.StackTrace);
            }
        }

        //запись сообщения в log - подключение к файлу, конструктор записи
        private static void LogWrite(string type, string message)
        {
            using (StreamWriter writer = new StreamWriter(GetLogFile(), true))
                writer.WriteLine($"{DateTime.Now} - {type} {message}");
        }
    }
}