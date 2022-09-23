namespace NSE.WebApp.MVC.Extensions
{
    public class AppSettings
    {
        public string APIIdentidadeURL { get; set; }
        public string CatalogoURLs { get; set; }
        public Logging Logging { get; set; }

    }
    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }
    public class LogLevel
    {
        public string Default { get; set; }
    }
}
