namespace AutoStopAPI.Models
{
    public class Car
    {
        public string? GRZ { get; set; }
        public string? OldGRZ { get; set; } //Это нужно чисто для patch для того что бы менять данные
        public string? PhoneUser { get; set; }
        public string? CarModel { get; set; }
        public string? Color { get; set; }
    }
}
