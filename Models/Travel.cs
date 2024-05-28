namespace AutoStopAPI.Models
{
    public class Travel
    {
        public int? idTravel { get; set; }
        public string? carGRZ { get; set; }
        public string? startCity { get; set; }
        public string? endCity { get; set; }
        public DateTime? dateTime { get; set; }
        public int? numberPassenger { get; set; }
        public string? comment { get; set; }
        public List<Passenger>? Passengers { get; set; }
        public string? phoneDriver { get; set;}
        public bool? isActive { get; set; }
    }
}
