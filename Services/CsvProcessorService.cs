using WorkerCsvProcessor.Models;

namespace WorkerCsvProcessor.Services
{
    public class CsvProcessorService
    {
        public bool ProcessCsv(string filePath, string processedFolder, string errorFolder)
        {
            var lines = File.ReadAllLines(filePath);

            var models = lines
                .Skip(1)
                .Select(line =>
                {
                    var item = line.Split(';');
                    return new ClientCsvModel(item[0], item[1], item[2]);
                });

            bool isValid = true;

            foreach (var item in models)
                isValid &= item.Validate();

            var destinationFolder = isValid ? processedFolder : errorFolder;

            Directory.CreateDirectory(destinationFolder);

            var fileName = Path.GetFileName(filePath);
            var destinationPath = Path.Combine(destinationFolder, fileName);

            File.Move(filePath, destinationPath, overwrite: true);

            return isValid;
        }
    }
}
