using Newtonsoft.Json;
using System;

namespace HT.CheckerApp.API.Models
{
    public class MtUssdSender
    {
        private readonly string ussdServer;
        private readonly string smsServer;

        public MtUssdSender(string ussdServer, string smsServer)
        {
            this.smsServer = smsServer; // Assign server url
            this.ussdServer = ussdServer; // Assign server url
        }

        /*
            Get parameters form the application
            check one or more addresses
            Send them to ussdMany
        **/

        public string ussd(string applicationId, string password, string version, string responseMsg,
                            string sessionId, string ussdOperation, string destinationAddress, string encoding, string chargingAmount)
        {
            if (!string.IsNullOrEmpty(destinationAddress))
            { //Check destination address is a array or not
                return this.ussdMany(applicationId, password, version, responseMsg,
                    sessionId, ussdOperation, destinationAddress, encoding, chargingAmount);
            }
            else if (string.IsNullOrEmpty(destinationAddress))
            {
                return this.ussdMany(applicationId, password, version, responseMsg,
                    sessionId, ussdOperation, destinationAddress, encoding, chargingAmount);
            }
            else
            {
                throw new Exception("address should a string or a array of strings");
            }
        }

        public string sms(string applicationId, string password, string version, string responseMsg,
                           string destinationAddress, string encoding, string chargingAmount)
        {
            if (!string.IsNullOrEmpty(destinationAddress))
            { //Check destination address is a array or not
                return this.smsMany(applicationId, password, version, responseMsg,
                     destinationAddress, encoding, chargingAmount);
            }
            else if (string.IsNullOrEmpty(destinationAddress))
            {
                return this.smsMany(applicationId, password, version, responseMsg,
                    destinationAddress, encoding, chargingAmount);
            }
            else
            {
                throw new Exception("address should a string or a array of strings");
            }
        }

        /*
            Get parameters form the ussd
            Assign them to an array according to json format
            encode that array to json format
            Send json to sendRequest
        **/

        private string ussdMany(string applicationId, string password, string version, string message,
                                 string sessionId, string ussdOperation, string destinationAddress, string encoding, string chargingAmount)
        {
            string data = @"{" + string.Format("'applicationId':'{0}', 'password':'{1}','message': '{2}','sessionId': '{3}','ussdOperation': '{4}','destinationAddress': '{5}'", applicationId, password, message, sessionId, ussdOperation, destinationAddress) + "}";

            return this.sendRequest(this.ussdServer, data);
        }
        private string smsMany(string applicationId, string password, string version, string message,
                                 string destinationAddress, string encoding, string chargingAmount)
        {
            //string data = @"{" + string.Format("'applicationId':'{0}', 'password':'{1}','message': '{2}','destinationAddress': ['{3}'],'sourceAddress':'21213_check','encoding':'{4}','version':'{5}','binary-header':'','chargingAmount':'0', 'deliveryStatusRequest':'0'", applicationId, password, message, destinationAddress,encoding,version) + "}";
            string data = JsonConvert.SerializeObject(new
            {
                password = password.ToString(),
                message = message.ToString(),
                destinationAddresses = new string[] { destinationAddress.ToString() },
               applicationId = applicationId.ToString(),
               deliveryStatusRequest = "0",
               version = version.ToString(),
               sourceAddress = "21213_check",
               encoding = encoding.ToString(),
               chargingAmount = chargingAmount.ToString()
           });
            return this.sendRequest(this.smsServer, data);
        }
        /*
            Get the json request from ussdMany
            use curl methods to send Ussd
            Send the response to handleResponse
        **/

        private string sendRequest(string endPoint, string jsonData)
        {
            //string endPoint = this.server;
            var client = new RestClient(endpoint: endPoint,
                            method: HttpVerb.POST,
                            postData: jsonData);
            var json = client.MakeRequest();
            return json;
        }
    }
}