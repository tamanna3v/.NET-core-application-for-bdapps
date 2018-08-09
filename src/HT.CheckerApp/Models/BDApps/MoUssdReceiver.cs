using System;

namespace HT.CheckerApp.API.Models
{
    public class MoUssdReceiver
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

        public MoUssdReceiver()
        {
        }
        public MoUssdReceiver(MoUssdReceiver moUssdReceiver)
        {
            this.SourceAddress = moUssdReceiver.SourceAddress;
            this.Message = moUssdReceiver.Message;
            this.RequestId = moUssdReceiver.RequestId;
            this.ApplicationId = moUssdReceiver.ApplicationId;
            this.Encoding = moUssdReceiver.Encoding;
            this.Version = moUssdReceiver.Version;
            this.SessionId = moUssdReceiver.SessionId;
            this.UssdOperation = moUssdReceiver.UssdOperation;
            this.VlrAddress = moUssdReceiver.VlrAddress;

            if (!((string.IsNullOrEmpty(this.SourceAddress) && string.IsNullOrEmpty(this.Message))))
            {
                throw new Exception("Some of the required parameters are not provided");
            }
            else {
                // Success received response
                //var responses = array("statusCode" => "S1000", "statusDetail" => "Success");
                //    header("Content-type: application/json");
                //    echo json_encode($responses);
            }
        }


        /*
            Define getters to return receive data
        **/

        public string getAddress()
        {
            return this.SourceAddress;
        }

        public string getMessage()
        {
            return this.Message;
        }

        public string getRequestID()
        {
            return this.RequestId;
        }

        public string getApplicationId()
        {
            return this.ApplicationId;
        }

        public string getEncoding()
        {
            return this.Encoding;
        }

        public string getVersion()
        {
            return this.Version;
        }

        public string getSessionId()
        {
            return this.SessionId;
        }

        public string getUssdOperation()
        {
            return this.UssdOperation;
        }

    }
}
