namespace LowPrice.Models
{
    /// <summary>
    /// Класс клиента
    /// Хранит в себе номер и IP
    /// </summary>
    public class Client
    {
        public string Number { get; set; }
        public List<string> Ip { get; set; }
    }
}
