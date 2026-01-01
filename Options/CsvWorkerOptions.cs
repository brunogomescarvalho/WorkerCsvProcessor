public class CsvWorkerOptions
{
    public string InputFolder { get; set; } = string.Empty;
    public string ProcessedFolder { get; set; } = string.Empty;
    public string ErrorFolder { get; set; } = string.Empty;
    public int DelaySeconds { get; set; } = 5;
}