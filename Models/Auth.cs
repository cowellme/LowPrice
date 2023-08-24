using System.Data;
using Google.Protobuf.Collections;
using LowPrice.Models.Market;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LowPrice.Models
{
    public  class Auth
    {
        //Приватная переменная хранит в себе строку подкл-я к БД
        private static string _connection;

        /// <summary>
        /// Подключение к БД
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns>Статус подключения правда или ложь</returns>
        public static bool Connect(string strSql)
        {
             
            try
            {
                _connection = strSql;

                return true;
            }
            catch
            {


                return false;
            }
        }

        /// <summary>
        /// Проверку учетной записи
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>Номер ттелефона</returns>
        public static string Login(string ip)
        {
            string number = @"none";

            try
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory + @"/wwwroot/clients/");
                foreach (var file in files)
                {
                    var js = System.IO.File.ReadAllText(file);
                    var client = Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(js);
                    foreach (var ipOne in client.Ip)
                    {
                        if (ipOne != null)
                        {
                            if (ipOne == ip)
                            {
                                number = client.Number;
                            }
                            else
                            {
                                client.Ip.Add(ip);
                                var jsnew = JsonConvert.SerializeObject(client);
                                var path = Environment.CurrentDirectory + $@"/wwwroot/clients/{number}.json";
                                
                                File.WriteAllText(path, jsnew);
                                number = client.Number;
                            }
                        }
                        else
                        {
                            return "none";
                        }
                    }
                }

                return number;

                //string js = System.IO.File.ReadAllText(Environment.CurrentDirectory + $@"/wwwroot/clients/{numberLogin}.json");
                //var client = Newtonsoft.Json.JsonConvert.DeserializeObject<Client>(js);
                //client.Ip.Add(ip);
                //return numberLogin;
            }
            catch
            {
                return number;
            }



            //if (false)
            //{
                
            //}
            //else
            //{
            //    try
            //    {
            //        using (var conn = new MySqlConnection(_connection))
            //        {
            //            conn.Open();
            //            MySqlCommand cmd = conn.CreateCommand();
            //            cmd.CommandText = $@"SELECT number FROM lowprice.users WHERE ip = '{ip}';";

            //            number = cmd.ExecuteScalar().ToString();

            //            conn.Close();
            //        }

            //        return number;
            //    }
            //    catch
            //    {
            //        using (var conn = new MySqlConnection(_connection))
            //        {
            //            if (conn.State == ConnectionState.Open)
            //                conn.Close();
            //        }
            //        return number;
            //    }
            //}
            
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="number"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CreateUser(string number, string ip)
        {
            try
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"INSERT lowprice.users(number, ip, tid, name, liked) VALUES ('{number}', '{ip}','none','none','none');";
                    
                    cmd.ExecuteScalar();
                    conn.Close();

                    var ips = new List<string> { ip };
                    var client = new Client()
                    {
                        Number = number,
                        Ip = ips
                    };
                    var path = Environment.CurrentDirectory + $@"/wwwroot/clients/{number}.json";
                    var js = Newtonsoft.Json.JsonConvert.SerializeObject(client);
                    File.WriteAllText(path, js);

                }

                return true;
            }
            catch
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return false;
            }
        }

        /// <summary>
        /// Избранные по номеру телефона
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Список избранных товаров</returns>
        public static List<string> ReturnLiked(string number)
        {
            List<string> likeds = new List<string>();

            try
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    if (number == "none")
                        return likeds;
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"SELECT liked FROM lowprice.users WHERE number = '{number}';";
                    string hashs = cmd.ExecuteScalar().ToString();
                    conn.Close();
                    var hash = hashs.Split(';');
                    foreach (var str in hash)
                    {
                        if(str != "")
                            likeds.Add(str);
                    }
                }   //Возвращаем заполненный список, если имеются избранные предметы
                return likeds;
            }
            catch
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                return likeds;
            }
        }

        /// <summary>
        /// Обновляет информацию о лайках
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="hash"></param>
        /// <param name="act"></param>
        /// <returns>Правда или ложь</returns>
        public static bool UpdateLiked(string ip, string hash, string? act)
        {
            try
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    conn.Open();
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $@"SELECT liked FROM lowprice.users WHERE ip = '{ip}';";
                    string likeds = cmd.ExecuteScalar().ToString();

                    if (act != null && likeds != null)
                    {
                        if (likeds.Contains($"{hash};"))
                        {
                            likeds = likeds.Replace(hash + ";", "");
                            cmd.CommandText = $@"UPDATE lowprice.users SET liked = '{likeds};' WHERE ip = '{ip}';";
                            cmd.ExecuteScalar();
                        }
                    }
                    else
                    {
                        if (likeds == "none")
                        {
                            likeds = ""; cmd.CommandText = $@"UPDATE lowprice.users SET liked = '{likeds}{hash};' WHERE ip = '{ip}';";
                            cmd.ExecuteScalar();
                        }
                        else
                        {
                            if (!likeds.Contains($"{hash};"))
                            {
                                cmd.CommandText = $@"UPDATE lowprice.users SET liked = '{likeds}{hash};' WHERE ip = '{ip}';";
                                cmd.ExecuteScalar();
                            }
                        }
                    }
                    
                    
                    conn.Close();
                }

                return true;
            }
            catch
            {
                using (var conn = new MySqlConnection(_connection))
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

                return false;
            }
        }
        
        /// <summary>
        /// Парсит цену с сайтов
        /// </summary>
        /// <param name="hash"></param>
        public static void ParserFace(string hash)
        {
            for (int i = 0; i < 2; i++)
            {
                try
                {
                    using (var conn = new MySqlConnection(_connection))
                    {
                        conn.Open();
                        MySqlCommand cmd = conn.CreateCommand();
                        if(i == 0) 
                            cmd.CommandText = $@"INSERT lowprice.prices(hash, platform, price, uri) VALUES ('{hash}', 'sitilink','none','none');";
                        if (i == 1)
                            cmd.CommandText = $@"INSERT lowprice.prices(hash, platform, price, uri) VALUES ('{hash}', 'ldor','none','none');";

                        cmd.ExecuteScalar();
                        conn.Close();
                    }
                }
                catch
                {
                    using (var conn = new MySqlConnection(_connection))
                    {
                        if (conn.State == ConnectionState.Open)
                            conn.Close();
                    }
                }
            }
        }
    }
}
