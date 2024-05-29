namespace AutoStopAPI.Models
{
    public class Message
    {
        public int? idMessage { get; set; }
        public int? refChat { get; set; }
        public string? senderPhone { get; set; }
        public string? content { get; set;}
        public DateTime? sendDate { get; set; }
    }
}
