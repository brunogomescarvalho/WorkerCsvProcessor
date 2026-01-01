namespace WorkerCsvProcessor.Models
{
    public class ClientCsvModel
    {
        public string Name { get; }
        public string Email { get; }
        public string Age { get; }

        public ClientCsvModel(string? name, string? email, string? age)
        {
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
            Age = age ?? string.Empty;
        }

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
    }
}
