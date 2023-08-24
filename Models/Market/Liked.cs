namespace LowPrice.Models.Market
{
    public class Liked
    {
        /// <summary>
        /// Приватные свойства
        /// </summary>
        private string PATH = Environment.CurrentDirectory + @"\AllItems";
        private string PATHwww = Environment.CurrentDirectory + @"\wwwroot";


        /// <summary>
        /// Публичные свойства
        /// </summary>
        public string Number { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public List<string> Hashs { get; set; }


        /// <summary>
        /// Получение всех Избранных по номеру
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Список товаров</returns>
        public static List<Item> GetAll(string number)
        {
            List<Item> likeds = new List<Item>();
            var hashs = Auth.ReturnLiked(number);

            foreach (var hash in hashs)
            {
                likeds.Add(Item.GetByHash(hash));
            }

            return likeds;
        }
    }
}
