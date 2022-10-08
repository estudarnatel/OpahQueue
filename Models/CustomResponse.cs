namespace OpahQueue.Models
{
    public class CustomResponse
    {
        public long Position { get; set; }
        // public bool IsComplete { get; set; } // TRUE BUSY
        public bool IsFirst { get; set; } = false;

        public CustomResponse(long position, bool isFirst)
        {
            this.Position = position;
            this.IsFirst = isFirst;
        }
        public CustomResponse(long position)
        {
            this.Position = position;
        }
        public CustomResponse()
        {
        }
    }
}