namespace DAL.Entities
{
    public class AppSettings
    {
        public ConnectionString ConnectionStrings { get; set; }

        public string AllowedHosts { get; set; }

        public bool EnableCORS { get; set; }

        public string[] CORSTrustedOrigins { get; set; }

    }
}
