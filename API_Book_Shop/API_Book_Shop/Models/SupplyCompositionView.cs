namespace API_Book_Shop.Models
{
    public class SupplyCompositionView
    {
        public string НазваниеТовара { get; set; }
        public string НомерПоставки { get; set; }
        public DateTime? ДатаПоставки { get; set; }
        public int? КоличествоПоставки { get; set; }
        public decimal СтоимостьПоставки { get; set; }
    }
}
