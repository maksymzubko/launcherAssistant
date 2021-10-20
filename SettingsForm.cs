using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace LauncherSchool
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        string currentCulture;
        string settingsPath = "settings.json";

        private void SettingsForm_Load(object sender, EventArgs e)
        {         
            using(StreamReader reader = new StreamReader(settingsPath))
            {
                currentCulture = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd()).CurrentCulture;

                customComboBox1.SelectedIndex = currentCulture == "uk-UA" ? 1 : 0;
            }
        }

        private void customButton1_Click(object sender, EventArgs e)
        {
            if(customComboBox1.SelectedIndex == (currentCulture == "uk-UA" ? 1 : 0))
            {
                MessageBox.Show("Вы ничего не изменили", "Ошибка");
                return;
            }
            else
            {
                var currCult = customComboBox1.SelectedItem.ToString() == "Русский" ? "ru-RU" : "uk-UA";
                
                var newSettings = new Settings(currCult);

                var newString = JsonConvert.SerializeObject(newSettings);

                File.WriteAllText(settingsPath, newString);

                currentCulture = currCult;

                MessageBox.Show("Успешно сохранено!");
            }
        }
    }
}
