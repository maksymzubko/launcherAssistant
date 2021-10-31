using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using School.Data.CustomElements;

namespace LauncherSchool
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        public List<RootCommit> Commits;
        public List<RootContent> Contents;
        public string urlContent = "https://api.github.com/repos/s1maxx/RepoForLauncher/";
        public string versionAdmin = "";
        public string versionSchool = "";
        public Versions Versions;

        bool isDownloading = false;

        void changeLabel(string name, Color color)
        {
            if (!label1.Visible)
                label1.Visible = true;

            label1.Text = name;
            label1.ForeColor = color;
        }

        Dictionary<string, Label> valuePairs;

        void PushCommitsIntoLbls()
        {
            valuePairs = new Dictionary<string, Label>();

            Label lastLabel = null;

            foreach (var root in Commits.Select((value, i) => new { i, value}))
            {
                var label = new Label()
                {
                    AutoSize = true,
                    Location = new Point(lastLabel == null ? label2.Location.X : lastLabel.Location.X, lastLabel == null ? label2.Location.Y + 50 : lastLabel.Location.Y + 40),
                    ForeColor = Color.White,
                    Font = new Font(label2.Font.FontFamily,14, FontStyle.Regular),
                    Text = root.value.commit.message + $" [{root.value.commit.committer.date.ToString("dd-MM-yyyy HH:mm")}] "
                };

                lastLabel = label;

                valuePairs.Add(root.value.sha, label);
                label.Click += Label_Click;
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                panel1.Controls.Add(label);
            }
              
        }

        private void Label_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.White;
            lbl.Cursor = Cursors.Default;
        }

        private void Label_MouseEnter(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.Red;
            lbl.Cursor = Cursors.Hand;
        }

        private void Label_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            if (MessageBox.Show("Вы уверены что хотите перейти по текущей версии?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Process.Start(Commits.FirstOrDefault(c => c.sha.Equals(valuePairs.FirstOrDefault(l => l.Value == lbl).Key)).html_url);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForVersions();

            if (Application.ProductVersion != Versions.LauncherVersion)
            {
                if (MessageBox.Show("Обнаружена более новая версия лаунчера. Обновить?", "Оп-па",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!Directory.Exists(@"Updater\"))
                    {
                        MessageBox.Show("К сожалению не был найден Updater");
                        Application.Exit();
                    }
                    else
                    {
                        if (File.Exists(@"Updater\Launcher.zip"))
                            File.Delete(@"Updater\Launcher.zip");
                    }
                    using (WebClient wc = new WebClient())
                    {
                        wc.DownloadFileCompleted += (o, args) =>
                        {
                            try
                            {
                                Process.Start(@"Updater\Updater.exe", "LauncherSchool.exe");
                                Process.GetCurrentProcess().Kill();
                            }
                            catch (Exception) { }
                        };

                        wc.DownloadFileTaskAsync(Contents.FirstOrDefault(cnt => cnt.name.Contains("Launcher")).download_url, $"Updater\\Launcher.zip");
                    }
                }
            }

            customComboBox1.OnSelectedIndexChanged += CustomComboBox1_OnSelectedIndexChanged;

            if (SavedUser.ID == -1)
            {
                panel2.Visible = true;
                customButton1.Text = "Авторизация";
                customButton1.Click += Auth;
            }
            else
            {
                panel3.Visible = true;
                label5.Text += " " + SavedUser.ID;

                if(new Encrypter().isAdmin(SavedUser.ID))
                customComboBox1.Items.Add("AdminAssistant (Close)");
                customComboBox1.SelectedIndex = 0;
            }
        }

        private void CustomComboBox1_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bool isIdentity = false;

            string fullPath = "";

            if (customComboBox1.SelectedItem == "SchoolAssistant")
            {
                if (!customButton1.Enabled)
                    customButton1.Enabled = true;

                fullPath = Path.GetFullPath("School\\" + "School.exe");

                if (!File.Exists(fullPath))
                    versionSchool = "none";
                else
                    versionSchool = Application.ProductVersion;

                isIdentity = Versions.SchoolVersion == versionSchool;
            }
            else
            {
                customButton1.Enabled = false; //WILL BE DELETED
                MessageBox.Show("В данный момент это приложение в разработке!"); //WILL BE DELETED

                fullPath = Path.GetFullPath("Admin\\" + "Admin.exe");

                if (!File.Exists(fullPath))
                {
                    RegistryKey key;
                    key = Registry.CurrentUser.CreateSubKey("School");

                    versionAdmin = "none";

                    key.SetValue("versionAdmin", versionAdmin);
                    key.Close();
                }
                else
                    versionAdmin = (string)Registry.CurrentUser.OpenSubKey("School").GetValue("versionAdmin");

                isIdentity = Versions.AdminVersion == versionAdmin;
            }

            customButton1.Click -= UpdateEvent;
            customButton1.Click -= Play;

            if (!isIdentity)
            {
                if (File.Exists(fullPath))
                {
                    customButton1.Text = "Обновить";
                    customButton1.Click += UpdateEvent;
                    changeLabel("Требуется обновление", Color.Yellow);
                }
                else
                {
                    customButton1.Text = "Скачать";
                    customButton1.Click += UpdateEvent;
                    changeLabel("Требуется скачать", Color.Red);
                }
            }
            else
            {
                customButton1.Text = "Запустить";
                customButton1.Click += Play;
                changeLabel("Готово к запуску", Color.LightGreen);
                customProgressBar1.Value = 100;
            }
        }

        private bool GetConnection()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlContent + "commits");
            request.UserAgent = "[SchoolUserAgent]";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recivestream = response.GetResponseStream();
                StreamReader reader = null;
                if (response.CharacterSet == null)
                    reader = new StreamReader(recivestream);
                else
                    reader = new StreamReader(recivestream, Encoding.GetEncoding(response.CharacterSet));

                string dataTemp = reader.ReadToEnd();

                Commits = JsonConvert.DeserializeObject<List<RootCommit>>(dataTemp);

                if (Commits.Count > 0)
                    PushCommitsIntoLbls();

                response.Close();
                reader.Close();
                return true;
            }
            else
            {
                new Label()
                {
                    Location = new Point(label2.Location.X, label2.Location.Y - 20),
                    Parent = panel1,
                    Text = "Ошибка подключения: " + response.StatusCode
                };
                return false;
            }
        }

        private Versions GetVersions(List<RootContent> rootContents)
        {
            string versionsLink = rootContents.FirstOrDefault(ver => ver.name.Contains("versions")).git_url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(versionsLink);
            request.UserAgent = System.Environment.MachineName + "2";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recivestream = response.GetResponseStream();
                StreamReader reader = null;
                if (response.CharacterSet == null)
                    reader = new StreamReader(recivestream);
                else
                    reader = new StreamReader(recivestream, Encoding.GetEncoding(response.CharacterSet));

                string dataTemp = reader.ReadToEnd();

                Content content = JsonConvert.DeserializeObject<Content>(dataTemp);

                Versions versions = JsonConvert.DeserializeObject<Versions>(Encoding.UTF8.GetString(Convert.FromBase64String(content.content)));

                response.Close();
                reader.Close();

                return versions;
            }
            else
            {
                MessageBox.Show("К сожалению возникла ошибка, статус код: " + response.StatusCode);
                return null;
            }
        }

        private void CheckForVersions()
        {
            if (!GetConnection())
            {
                MessageBox.Show("Ошибка приложения, возможно из-за качества интернета, пожалуйста проверьте подлючены ли вы к интернету. Приложение будет завершено!");
                Application.Exit();
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlContent + "contents");
            request.UserAgent = System.Environment.MachineName + "1";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recivestream = response.GetResponseStream();
                StreamReader reader = null;
                if (response.CharacterSet == null)
                    reader = new StreamReader(recivestream);
                else
                    reader = new StreamReader(recivestream, Encoding.GetEncoding(response.CharacterSet));

                string dataTemp = reader.ReadToEnd();

                Contents = JsonConvert.DeserializeObject<List<RootContent>>(dataTemp);

                Versions = GetVersions(Contents);

                response.Close();
                reader.Close();
            }
            else
            {
                MessageBox.Show("Ошибка приложения, возможно из-за качества интернета, пожалуйста проверьте подлючены ли вы к интернету. Приложение будет завершено!");
                Application.Exit();
            }
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            isDownloading = false;
            customButton1.Enabled = true;

            string fileName = "";
            string directoryName = "";

            if (customComboBox1.SelectedItem == "SchoolAssistant")
            {
                fileName = "School";
                directoryName = @"School\";
            }
            else
            {
                fileName = "Admin";
                directoryName = @"Admin\";
            }
            if (File.Exists($"{fileName}.zip"))
                File.Delete($"{fileName}.zip");

            if (!Directory.Exists(directoryName)) 
                Directory.CreateDirectory(directoryName);

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(directoryName));
            foreach (var item in dir.GetFiles())
                item.Delete();

            foreach (var item in dir.GetDirectories())
                item.Delete(true);

            using (WebClient wc = new WebClient())
            {
                int counter = 0;
                wc.DownloadProgressChanged += (s, g) =>
                {
                    counter = counter == 3 ? 1 : ++counter;
                    if(g.ProgressPercentage!=100)
                    changeLabel("Обновляем " + new String('.',counter), Color.BlueViolet);

                    customProgressBar1.Value = g.ProgressPercentage;
                    if (g.ProgressPercentage == 100)
                    {
                        if (!File.Exists(Path.GetFullPath(directoryName + fileName + ".exe")))
                        {
                            ZipFile.ExtractToDirectory($"{fileName}.zip", directoryName);
                            RegistryKey key;
                            key = Registry.CurrentUser.CreateSubKey("School");
                            key.SetValue("version"+fileName, fileName == "Admin"  ? Versions.AdminVersion : Versions.SchoolVersion);
                            key.Close();

                            if (fileName == "Admin")
                                versionAdmin = Versions.AdminVersion;
                            else
                                versionSchool = Versions.SchoolVersion;

                            changeLabel("Готово к запуску", Color.Green);

                            if (SavedUser.ID != -1)
                            {
                                customButton1.Text = "Запустить";
                                customButton1.Click -= UpdateEvent;
                                customButton1.Click += Play;
                            }
                            else
                            {
                                customButton1.Text = "Авторизация";
                                customButton1.Click -= UpdateEvent;
                                customButton1.Click += Auth;
                            }
                        }
                    }
                };

                wc.DownloadFileTaskAsync(new Uri(Contents.FirstOrDefault(cnt=>cnt.name.Contains(fileName)).download_url), $"{fileName}.zip");
            }
        }

        private void Auth(object sender, EventArgs e)
        {
            var encrypter = new Encrypter();
            string userPath = "saveduser.json";

            if (panel2.Visible)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                if (!encrypter.CheckUser(Convert.ToInt16(textBox1.Text)))
                {
                    MessageBox.Show("Введены неверные данные!");
                    return;
                }
                else
                {
                    SavedUser.ID = Convert.ToInt16(textBox1.Text);

                    if (checkBox1.Checked)
                    {
                        File.WriteAllText(userPath, JsonConvert.SerializeObject(new User()
                        {
                            ID = SavedUser.ID

                        }));
                    }
                    else
                    {
                        if (File.Exists(userPath))
                            File.Delete(userPath);
                    }

                    panel2.Visible = false;
                    panel3.Visible = true;

                    label5.Text += " " + SavedUser.ID;

                    customComboBox1.SelectedIndex = 0;
                    if (new Encrypter().isAdmin(SavedUser.ID))
                        customComboBox1.Items.Add("AdminAssistant (Close)");
                    CustomComboBox1_OnSelectedIndexChanged(customComboBox1,null);
                }
            }
        }

        private void Play(object sender, EventArgs e)
        {
            string path = "";

            if (customComboBox1.SelectedItem == "SchoolAssistant")
                path = "School\\School.exe";
            else path = "Admin\\Admin.exe";

            if(File.Exists("School.zip"))
            File.Delete("School.zip");

            string currentCulture;
            string settingsPath = "settings.json";

            using (StreamReader reader = new StreamReader(settingsPath))
            {
                currentCulture = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd()).CurrentCulture;
            }

            Process.Start(path, $"{currentCulture};{SavedUser.ID};");
            Application.Exit();
        }

        private void Launcher_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
               this.ClientRectangle,
               Color.FromArgb(60, 111, 22, 132), Color.FromArgb(60, 83, 175, 186),
               45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void settings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog(this);
        }

        private void customButton1_Click(object sender, EventArgs e)
        {

        }

        private void customButton2_Click(object sender, EventArgs e)
        {
            string userPath = "saveduser.json";

            if (File.Exists(userPath))
            File.Delete(userPath);

            SavedUser.ID = -1;

            panel3.Visible = false;
            panel2.Visible = true;

            label5.Text = "Ваш ID:";

            customButton1.Click -= Play;
            customButton1.Click -= UpdateEvent;
            label1.Visible = false;
            customButton1.Text = "Авторизация";
            customButton1.Click += Auth;
        }
    }
}
