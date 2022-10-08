namespace OpahQueue.Models
{
    public class CustomRequest
    {        
        public long Id { get; set; }
        // public bool IsComplete { get; set; } // TRUE BUSY
        public bool FurarFila { get; set; } = false;

        public bool Logout { get; set; } = true;

        // public CustomRequest(long id, bool furarFila)
        public CustomRequest(long id, bool furarFila, bool logout)
        {
            this.Id = id;
            this.FurarFila = furarFila;
            this.Logout = logout;
        }
        public CustomRequest()
        {            
        }
    }
}