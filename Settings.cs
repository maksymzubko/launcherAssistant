namespace LauncherSchool
{
    public class Settings
    {
        public string CurrentCulture { get; set; }
        public Settings(string currcult = "ru-RU") => CurrentCulture = currcult;
    }
}
