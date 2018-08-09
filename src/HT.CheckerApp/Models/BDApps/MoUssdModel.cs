namespace HT.CheckerApp.API.Models
{
    public class MoUssdModel
    {
        public string SourceAddress { get; set; }

        public string Password { get; set; }

        public string Message { get; set; }

        public string ApplicationId { get; set; }

        public string Encoding { get; set; }

        public string UssdOperation { get; set; }

        public string Version { get; set; }

        public string VlrAddress { get; set; }

        public string RequestId { get; set; }

        public string SessionId { get; set; }

    }
}