namespace OpahQueue.Models
{
    public class Exceller
    {
        public int Id { get; set; }
        public string? Protocol { get; set; }
        public DateTime Datetime { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = "ativo";

        public Exceller(string protocol, int id)
        {
            this.Protocol = protocol;
            this.UserId = Id;
            this.Datetime = DateTime.Now;
        }
        public Exceller()
        {
            this.Datetime = DateTime.Now;
        }
    }
}