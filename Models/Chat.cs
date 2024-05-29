namespace AutoStopAPI.Models
{
    public class Chat
    {
        public int? idChat { get; set; }
        public string? phoneUser1 { get; set; }
        public string? phoneUser2 { get; set; }
        public DateTime? dateCreate { get; set; }
        public bool deleteUser1 { get; set; } //Показывает, удален ли чат у User1
        public bool deleteUser2 { get; set;} //Показывает, удален ли чат у User2
        public List<Message>? messages { get; set; }
    }
}
