namespace Domain.Models
{
    public class Messages
    {
        public int id { get; set; }
        public string message { get; set; }
        public DateTime creationDate { get; set; }
        public Messages()
        {
            
        }

        public Messages(string message)
        {
            this.message = message;
            creationDate = DateTime.Now;
        }
    }
}
