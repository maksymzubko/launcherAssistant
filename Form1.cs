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

namespace LauncherSchool
{
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        public string version = "";
        public string ServerVersion = "";
        public string path = @"School\";
        public string exeName = "School.exe";
        public string urlContent = "https://api.github.com/repos/s1maxx/lAssistant/"; //will change
        public string downloadLink = "";
        public List<RootCommit> commits;

        bool isDownloading = false;

        void changeLabel(string name, Color color)
        {
            label1.Text = name;
            label1.ForeColor = color;
        }

        Dictionary<string, Label> valuePairs;

        void PushCommitsIntoLbls()
        {
            valuePairs = new Dictionary<string, Label>();

            Label lastLabel = null;

            foreach (var root in commits.Select((value, i) => new { i, value}))
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
                Process.Start(commits.FirstOrDefault(c => c.sha.Equals(valuePairs.FirstOrDefault(l => l.Value == lbl).Key)).html_url);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (SavedUser.ID == -1) 
                panel2.Visible = true;
            else
            {
                panel3.Visible = true;
                label5.Text += " " + SavedUser.ID;

                customComboBox1.SelectedIndex = 0;
                if(new Encrypter().isAdmin(SavedUser.ID))
                customComboBox1.Items.Add("AdminAssistant (Close)");
            }

            var fullPath = Path.GetFullPath(path + exeName);
            if (!File.Exists(fullPath))
            {
                RegistryKey key;
                key = Registry.CurrentUser.CreateSubKey("School");
                key.SetValue("version", "none");
                key.Close();
                version = "none";
            }
            else
                version = (string)Registry.CurrentUser.OpenSubKey("School").GetValue("version");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlContent + "contents");
            request.UserAgent = System.Environment.MachineName+"1";
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

                var service = JsonConvert.DeserializeObject<List<RootContent>>(dataTemp)[0];

                downloadLink = service.download_url;

                var fileName = service.name;

                var index1 = fileName.IndexOf("ver") + 4;
                var index2 = fileName.LastIndexOf(".zip");
                ServerVersion = fileName.Replace(fileName, fileName.Substring(index1, index2 - index1));
                response.Close();
                reader.Close();
            }
            else
            {
                MessageBox.Show("К сожалению возникла ошибка, статус код: "+response.StatusCode);
                return;
            }

            request = (HttpWebRequest)WebRequest.Create(urlContent + "commits");
            request.UserAgent = "[SchoolUserAgent]";
            response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream recivestream = response.GetResponseStream();
                StreamReader reader = null;
                if (response.CharacterSet == null)
                    reader = new StreamReader(recivestream);
                else
                    reader = new StreamReader(recivestream, Encoding.GetEncoding(response.CharacterSet));

                string dataTemp = reader.ReadToEnd();

                commits = JsonConvert.DeserializeObject<List<RootCommit>>(dataTemp);

                if (commits.Count > 0)
                    PushCommitsIntoLbls();

                response.Close();
                reader.Close();
            }
            else
            {
                new Label()
                {
                    Location = new Point(label2.Location.X, label2.Location.Y - 20),
                    Parent = panel1,
                    Text = "Ошибка подключения: " + response.StatusCode
                };
            }

            if (version != ServerVersion)
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
                if (SavedUser.ID != -1)
                {
                    customButton1.Text = "Запустить";
                    customButton1.Click += Play;
                    changeLabel("Готово к запуску", Color.LightGreen);
                    customProgressBar1.Value = 100;
                }
                else
                {
                    customButton1.Text = "Авторизация";
                    customButton1.Click += Auth;
                    changeLabel("Готово к запуску", Color.LightGreen);
                    customProgressBar1.Value = 100;
                }
            }
        }

        private void UpdateEvent(object sender, EventArgs e)
        {
            isDownloading = false;
            customButton1.Enabled = true;

            if (File.Exists("School.zip"))
                File.Delete("School.zip");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DirectoryInfo dir = new DirectoryInfo(Path.GetFullPath(path));
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
                        if (!File.Exists(Path.GetFullPath(path + exeName)))
                        {
                            ZipFile.ExtractToDirectory("School.zip", path);
                            RegistryKey key;
                            key = Registry.CurrentUser.CreateSubKey("School");
                            key.SetValue("version", ServerVersion);
                            key.Close();
                            version = ServerVersion;
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

                wc.DownloadFileTaskAsync(new Uri(downloadLink), "School.zip");
            }
        }

        private void Auth(object sender, EventArgs e)
        {
            var encrypter = new Encrypter();
            string userPath = "saveduser.json";

            if (panel2.Visible)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Заполните все поля!");
                    return;
                }

                if (!encrypter.CheckUser(Convert.ToInt16(textBox1.Text), textBox2.Text))
                {
                    MessageBox.Show("Введены неверные данные!");
                    return;
                }
                else
                {
                    SavedUser.ID = Convert.ToInt16(textBox1.Text);
                    SavedUser.Password = encrypter.Encrypt(textBox2.Text);

                    if (checkBox1.Checked)
                    {
                        File.WriteAllText(userPath, JsonConvert.SerializeObject(new User()
                        {
                            ID = SavedUser.ID,
                            Password = SavedUser.Password

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

                    customButton1.Click -= Auth;
                    customButton1.Text = "Запустить";
                    customButton1.Click += Play;
                }
            }
        }

        private void Play(object sender, EventArgs e)
        {
            if(File.Exists("School.zip"))
            File.Delete("School.zip");

            string currentCulture;
            string settingsPath = "settings.json";

            using (StreamReader reader = new StreamReader(settingsPath))
            {
                currentCulture = JsonConvert.DeserializeObject<Settings>(reader.ReadToEnd()).CurrentCulture;
            }

            Process.Start(path + exeName, $"{currentCulture};{SavedUser.ID};{new Encrypter().Decrypt(SavedUser.Password)}");
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
            customButton1.Text = "Авторизация";
            customButton1.Click += Auth;
        }
    }
}
