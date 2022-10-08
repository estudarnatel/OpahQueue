namespace OpahQueue.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ImageTitle { get; set; }
        public byte[] ImageData { get; set; } = default!;
        public string ImageStatus { get; set; } = "ativa";
    }
}