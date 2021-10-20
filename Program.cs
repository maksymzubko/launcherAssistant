using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherSchool
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
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
            Application.Run(new Launcher());
        }
    }
}
