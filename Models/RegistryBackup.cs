namespace C_TweaksPs1.Models
{
    public class RegistryBackup
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public object? Value { get; set; }
        public bool Existed { get; set; }
        public DateTime BackupDate { get; set; } = DateTime.Now;
    }
}
