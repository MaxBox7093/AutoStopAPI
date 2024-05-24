namespace AutoStopAPI.Models
{
    public class User
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public byte[]? Img { get; set; }
        public float? Rating { get; set; }
    }
}
