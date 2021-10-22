using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace LauncherSchool
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            string settingsPath = "settings.json";

            bool isExists = File.Exists(settingsPath);

            if (!isExists)
            {
                var content = new Settings();

                var JsonContent = JsonConvert.SerializeObject(content);

                File.WriteAllText(settingsPath, JsonContent);
            }

            settingsPath = "saveduser.json";

            isExists = File.Exists(settingsPath);

            if (isExists)
            {
                using (StreamReader sr = new StreamReader(settingsPath))
                {
                    string content = sr.ReadToEnd();
                    var itemUser = JsonConvert.DeserializeObject<User>(content);

                    var encrypter = new Encrypter();
                    bool isExistUser = encrypter.CheckUser(itemUser.ID, encrypter.Decrypt(itemUser.Password));

                    if (isExistUser)
                    {
                        SavedUser.ID = itemUser.ID;
                        SavedUser.Password = itemUser.Password;
                    }
                    else
                    {
                        SavedUser.ID = -1;
                        File.Delete(settingsPath);
                    }
                }
            }
            else SavedUser.ID = -1;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Launcher());
            }
            catch (Exception e)
            {
                MessageBox.Show("Возникла ошибка" + e.Message);
                return;
            }
            
        }
    }
}
