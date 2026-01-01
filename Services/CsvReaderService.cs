namespace WorkerCsvProcessor.Services
{
    public class CsvReaderService
    {
        public IEnumerable<string> GetCsvFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                return [];

            return Directory.GetFiles(folderPath, "*.csv");
        }

        public string[] ReadFile(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}
