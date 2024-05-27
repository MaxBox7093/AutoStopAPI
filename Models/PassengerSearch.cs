namespace AutoStopAPI.Models
{
    public class PassengerSearch
    {
        public string? startCity { get; set; }
        public string? endCity { get; set; }
        public int numberPassenger { get; set; }
        public DateOnly? date { get; set; }
    }
}
