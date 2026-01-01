using WorkerCsvProcessor.Services;
using WorkerCsvProcessor.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<CsvWorkerOptions>(
    builder.Configuration.GetSection("CsvWorker")
);

builder.Services.AddSingleton<CsvProcessorService>();
builder.Services.AddSingleton<CsvReaderService>();
builder.Services.AddHostedService<CsvWorker>();

var host = builder.Build();
host.Run();
