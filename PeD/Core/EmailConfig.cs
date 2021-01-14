namespace PeD.Core
{
    public class EmailConfig
    {
        public static readonly string PrimaryColor = "#042769";
        public static readonly string DangerColor = "#de7272";
        public string ApiKey { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
    }
}