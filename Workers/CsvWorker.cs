using Microsoft.Extensions.Options;
using WorkerCsvProcessor.Services;

namespace WorkerCsvProcessor.Workers;

public class CsvWorker : BackgroundService
{
    private readonly CsvWorkerOptions _options;
    private readonly CsvReaderService _csvReaderService;
    private readonly CsvProcessorService _csvProcessorService;
    private readonly ILogger<CsvWorker> _logger;

    public CsvWorker(
        IOptions<CsvWorkerOptions> options,
        CsvReaderService csvReaderService,
        CsvProcessorService csvProcessorService,
        ILogger<CsvWorker> logger)
    {
        _options = options.Value;
        _csvReaderService = csvReaderService;
        _csvProcessorService = csvProcessorService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "CsvWorker iniciado. Monitorando pasta: {Folder}",
            _options.InputFolder
        );

        while (!stoppingToken.IsCancellationRequested)
        {
            var files = _csvReaderService.GetCsvFiles(_options.InputFolder);

            foreach (var file in files)
            {
                try
                {
                    _logger.LogInformation(
                        "Processando arquivo CSV: {File}",
                        Path.GetFileName(file)
                    );

                    var result = _csvProcessorService.ProcessCsv(
                        file,
                        _options.ProcessedFolder,
                        _options.ErrorFolder
                    );

                    _logger.LogInformation(
                        "Arquivo {File} processado com sucesso. Destino: {Destination}",
                        Path.GetFileName(file),
                        result ? "Processados" : "Erro"
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Erro ao processar arquivo CSV: {File}",
                        Path.GetFileName(file)
                    );
                }
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_options.DelaySeconds),
                stoppingToken
            );
        }

        _logger.LogInformation("CsvWorker finalizado.");
    }

}
