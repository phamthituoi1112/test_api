using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;

namespace APITEST
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        public string conectMySQL = "server=localhost;database=test;uid=root;pwd=;";

        [WebMethod]
        public List<User> Login(string username, string password)
        {
            var users = Insert(username, password);
            List<User> lst = new List<User>();

            try{
                MySqlConnection connect = new MySqlConnection(conectMySQL);
                string query = "SELECT * FROM USERS WHERE USER = '" + username + "'";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                connect.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        User obj = new User();
                        obj.user = reader["user"].ToString();
                        obj.pass = reader["pass"].ToString();
                        obj.token = reader["tokenkey"].ToString();
                        lst.Add(obj);
                    }
                }
                connect.Close();
                connect.Dispose();

                return lst;
            }catch(Exception ex)
            {
                return null;
            }
        }

        public string Insert(string user, string pass)
        {
            string token = MD5(Guid.NewGuid().ToString());
            try
            {
                MySqlConnection connect = new MySqlConnection(conectMySQL);
                string query = "INSERT INTO USERS(USER, PASS, TOKENKEY) VALUES ('" + user + "', '" + pass + "','" + token + "')";
                MySqlCommand cmd = new MySqlCommand(query, connect);
                connect.Open();
                int result = cmd.ExecuteNonQuery();
                if(result == 1)
                {
                    return "OK";
                }
                else
                {
                    return "FAIL";
                }
                connect.Close();
                connect.Dispose();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private string MD5(string originalPassword)
        {
            #region
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-", "");
            #endregion
        }



        public class User
        {
            public string user { get; set; }
            public string pass { get; set; }
            public string token { get; set; }
        }
    }
}
