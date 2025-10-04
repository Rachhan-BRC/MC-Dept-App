using System;
using System.Text;
using System.Data.SqlClient;
using System.IO;

namespace MachineDeptApp
{
    internal class SQLConnectUpdate
    {
        public SqlConnection con;
        public string server;
        public string username;
        public string password;
        public string db;

        public void Connection()
        {
            var CDirectory = Environment.CurrentDirectory;
            try
            {
                //now to read file:
                string FilePath = CDirectory + @"\UpdateSetting.ini";
                using (StreamReader SysSettingRead = new StreamReader(FilePath))
                {
                    string line;
                    while ((line = SysSettingRead.ReadLine()) != null)
                    {
                        string separate = Encoding.UTF8.GetString(Convert.FromBase64String(line)).ToString();
                        string[] array = separate.Split('\n');
                        string Server = array[1];
                        string DB = array[2];
                        string User = array[3];
                        string Pass = array[4];

                        server = Encoding.UTF8.GetString(Convert.FromBase64String(Server.Replace("Server=", ""))).ToString();
                        db = Encoding.UTF8.GetString(Convert.FromBase64String(DB.Replace("DB=", ""))).ToString();
                        username = Encoding.UTF8.GetString(Convert.FromBase64String(User.Replace("User=", ""))).ToString();
                        password = Encoding.UTF8.GetString(Convert.FromBase64String(Pass.Replace("Password=", ""))).ToString();

                    }
                    SysSettingRead.Close();
                }

            }
            catch
            {

            }
            con = new SqlConnection("Data Source=tcp:" + server + ";Initial Catalog=" + db + ";User ID= " + username + ";Password=" + password + "; Connection Timeout=3");
        }
    }
}
