namespace OpahQueue.Models
{
    public class TIatende
    {
        public int Id { get; set; }
        public string? Called { get; set; }
        public DateTime Datetime { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = "ativo";

        public TIatende(string call, int id)
        {
            this.Called = call;
            this.UserId = Id;
            this.Datetime = DateTime.Now;
        }
        public TIatende()
        {
            this.Datetime = DateTime.Now;
        }
    }
}