using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherSchool
{
    public class Encrypter
    {
        private readonly string Server = "db9.freehost.com.ua"; //may be changed
        private readonly string DatabaseName = "veresen_school"; //may be changed
        private readonly string UserName = "veresen_school"; //may be changed
        private readonly string Password = "vmEX1Aurh"; //may be changed
        private readonly string Port = "3306"; //may be changed

        public MySqlConnection Connection;

        public void CreateConnection()
        {
            string connstring = string.Format("Server={0};Database={1};port={2};User id={3};password={4};SSL Mode = None", Server, DatabaseName, Port, UserName, Password);
            Connection = new MySqlConnection(connstring);
        }

        public bool CheckUser(int id)
        {
            CreateConnection();

            bool tempResult = false;

            string request = $"Select * FROM Teacher where Teacher_ID = {id}";
            Connection.Open();
            var command = new MySqlCommand(request, Connection);
            var reader = command.ExecuteReader();
            if (reader.HasRows)
                tempResult = true;
            else tempResult = false;

            reader.Close();
            Connection.Close();
            return tempResult;
        }

        public bool isAdmin(int id)
        {
            CreateConnection();
            bool tempResult = false;

            string request = $"Select TRole FROM Teacher where Teacher_ID = {id} ";
            Connection.Open();
            var command = new MySqlCommand(request, Connection);
            var reader = command.ExecuteReader();
            string role = "";

            while (reader.Read())
                role = reader[0].ToString();

            if(role == "Admin")
            tempResult = true;
            else tempResult = false;

            reader.Close();
            Connection.Close();
            return tempResult;
        }
        public string Encrypt(string pass)
        {
            byte[] buf = Encoding.UTF8.GetBytes(pass);
            StringBuilder sb = new StringBuilder(buf.Length * 8);
            foreach (byte b in buf)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }

            string binaryStr = sb.ToString();

            return binaryStr;
        } //create encrypted binary text

        public string Decrypt(string encrypt_pass)
        {
            StringBuilder sb = new StringBuilder(encrypt_pass.Length);
            for (int i = 0; i < encrypt_pass.Length; i += 8)
            {
                sb.Append((char)Convert.ToInt32(encrypt_pass.Substring(i, 8), 2));
            }

            return sb.ToString();
        } //decrypt encrypted binary text
    }
}
