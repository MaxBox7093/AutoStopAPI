namespace AutoStopAPI.Models.SQL
{
    public class Rating
    {
        public int? idComment { get; set; } //id коммента
        public string? phoneGet{ get; set; } //Номер телефона получателя
        public string? phoneSend { get; set; } //Номер телефона отправителя
        public string? NameGet { get; set; } //Номер телефона получателя
        public string? NameSend { get; set; } //Номер телефона отправителя
        public DateOnly? dateRating { get; set; } //Дата отзыва
        public string? comment { get; set; } //Содержание коммента
        public double? ratingTravel { get; set; } //Сама оценка за поездку
    }
}
