namespace IoTSuper_API.DTO
{
    public class ErrorDTO
    {
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int Status { get; set; } = 200;
        public Dictionary<string, List<string>> Errors { get; set; } = new();
    }
}
