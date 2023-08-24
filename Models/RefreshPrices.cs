using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.Security.Policy;

namespace LowPrice.Models
{
    public class RefreshPrices
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private const string connStr = @"server=127.0.0.1;uid=root;pwd=1590;database=lowprice";

        /// <summary>
        /// Возврат Цены
        /// </summary>
        /// <param name="platform">Платформа</param>
        /// <param name="hash">Хэш код товара</param>
        /// <returns>Цена товара</returns>
        public static string ReturnPricePlatform(string platform, string hash)
        {
            string price = "";
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"SELECT price FROM lowprice.prices WHERE hash = '{hash}' AND platform = '{platform}';";
                    price = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
                return price;
            }
            catch
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return " - ";
            }
        }

        /// <summary>
        /// Возврат ссылку на товар
        /// </summary>
        /// <param name="platform">Платформа</param>
        /// <param name="hash">Хэш код товара</param>
        /// <returns>Ссылку на товар</returns>
        public static string ReturnUriPlatform(string platform, string hash)
        {
            string price = "";
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"SELECT uri FROM lowprice.prices WHERE hash = '{hash}' AND platform = '{platform}';";
                    price = cmd.ExecuteScalar().ToString();
                    conn.Close();
                }
                return price;
            }
            catch
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return " - ";
            }
        }
    }
}
