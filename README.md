# WorkerCsvProcessor

Worker Service responsável por monitorar uma pasta de entrada, identificar arquivos CSV e processá-los automaticamente, movendo-os para pastas específicas conforme o resultado da validação.

---

## Objetivo

Demonstrar o uso de Worker Service (.NET) para processamento contínuo de arquivos CSV, aplicando boas práticas de arquitetura, separação de responsabilidades, validação e logging.

---

## Estrutura do Projeto

WorkerCsvProcessor  
├── Models  
│   └── ClientCsvModel.cs  
├── Services  
│   ├── CsvReaderService.cs  
│   └── CsvProcessorService.cs  
├── Workers  
│   └── CsvWorker.cs  
├── Options  
│   └── CsvWorkerOptions.cs  
├── appsettings.json  
└── Program.cs  

---

## Fluxo de Execução

1. O Worker inicia junto com a aplicação  
2. A cada intervalo configurado:
   - Verifica a pasta de entrada
   - Localiza arquivos CSV
3. Para cada arquivo:
   - Lê as linhas
   - Converte para modelos
   - Executa validações
4. O arquivo é movido para:
   - Pasta de processados → se válido
   - Pasta de erro → se inválido
5. Logs são registrados durante todo o processo

---

## Configuração (appsettings.json)

```json
{
  "CsvWorker": {
    "InputFolder": "C:\\Csv\\Input",
    "ProcessedFolder": "C:\\Csv\\Processed",
    "ErrorFolder": "C:\\Csv\\Error",
    "DelaySeconds": 5
  }
}
```

---

## CsvWorkerOptions

Classe utilizada para binding das configurações via Dependency Injection.

```csharp
public class CsvWorkerOptions
{
    public string InputFolder { get; set; } = string.Empty;
    public string ProcessedFolder { get; set; } = string.Empty;
    public string ErrorFolder { get; set; } = string.Empty;
    public int DelaySeconds { get; set; }
}
```

---

## Componentes Principais

### ClientCsvModel

Representa uma linha do CSV e concentra a regra de validação.

```csharp
 public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
                return false;

            if (string.IsNullOrWhiteSpace(Age))
                return false;

            return int.TryParse(Age, out _);
        }
```

---

### CsvReaderService

Responsável apenas por localizar arquivos CSV na pasta configurada.

```csharp
public IEnumerable<string> GetCsvFiles(string folderPath)
{
    return Directory.GetFiles(folderPath, "*.csv");
}
```

---

### CsvProcessorService

Responsável por:
- Ler o conteúdo do arquivo
- Converter linhas em modelos
- Validar os dados
- Decidir o destino do arquivo
- Mover o arquivo para a pasta correta

---

### CsvWorker

Classe principal do Worker Service.

Responsabilidades:
- Executar loop contínuo
- Respeitar CancellationToken
- Orquestrar serviços
- Registrar logs

---

## Logging

Exemplos de logs gerados:

- Worker iniciado
- Arquivo encontrado
- Arquivo processado com sucesso
- Arquivo inválido movido para pasta de erro
- Exceções inesperadas

---
