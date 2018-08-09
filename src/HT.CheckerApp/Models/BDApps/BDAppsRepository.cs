using HT.CheckerApp.API.Models.Common;
using HT.CheckerApp.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HT.CheckerApp.API.Models
{
    public class BDAppsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IPBDrawResultRepository _pbDrawResultRepository;
        private readonly IPBOnDemandRepository _pbOnDemandRepository;
        private IMemoryCache _cache;
        private readonly ConfigurationHelper _configurationHelper;
        public BDAppsRepository(IConfiguration configuration
            , IPBDrawResultRepository pbDrawResultRepository
            , IPBOnDemandRepository pbOnDemandRepository
            , IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _cache = memoryCache;
            _pbDrawResultRepository = pbDrawResultRepository;
            _pbOnDemandRepository = pbOnDemandRepository;
            _configurationHelper = new ConfigurationHelper(configuration, memoryCache);
        }

        public string SendMessage(MoUssdReceiver receiver)
        {
            //// TODO:: Check http session in netcore
            string cacheKey = "PrevMsg_" + receiver.SessionId;
            bool clearCache = false;
            string prevMsg = _configurationHelper.CacheTryGetValueSet(cacheKey, receiver.Message);
            string response = string.Empty;
            string message = string.Empty;
            string sessionId = receiver.SessionId;
            string dialNo = _configurationHelper.USSDDialNo;

            if (receiver.getMessage() == dialNo)
            {
                message = _configurationHelper.MenuLevel1;
            }
            else if (prevMsg.Contains(_configurationHelper.MenuLevel2) && receiver.getMessage() != "0")
            {
                message = GetSubscriptionConfirmationMenu(receiver, message);
            }
            else if (prevMsg.Contains(_configurationHelper.MenuLevel3) && receiver.getMessage() == "1")//// Subscriptions
            {
                message = "Thanks for subscription. We will notify you every 3 months after draw.\r\n";
                
                UpdateSubscription(receiver.getAddress(), true);
                clearCache = ClearCache(cacheKey,ref receiver);

            }
            else if (prevMsg.Contains(_configurationHelper.MenuLevel4) && receiver.getMessage() == "1")//// Subscriptions
            {
                message = "You have unsubcribed successfully";
                UpdateSubscription(receiver.getAddress(), false);
                clearCache = ClearCache(cacheKey, ref receiver);

            }
            else if (receiver.getMessage() == "0")
            {
                string responseExitMsg = "Thanks for using our service.";
                message = responseExitMsg;
                clearCache = ClearCache(cacheKey, ref receiver);
            }
            else
            {
                try
                {
                    message = GetSubscriptionMenu(receiver, prevMsg, message);

                    RunCheckerProcess(receiver, cacheKey, ref clearCache, ref response, ref message, sessionId);

                }
                catch (Exception ex)
                {
                    message = @"Unable to validate.Please try again. Error:" + ex.Message;
                    ClearCache(cacheKey, ref receiver);
                }
            }

            if (!clearCache)
                _configurationHelper.AddCache(cacheKey, message);

            response = loadObjectSender(sessionId, message, receiver);
            return response;
        }

        private string GetSubscriptionMenu(MoUssdReceiver receiver, string prevMsg, string message)
        {
            if (prevMsg == _configurationHelper.MenuLevel1 && receiver.getMessage() != "0")
            {
                if (receiver.getMessage().ToUpper().StartsWith(_configurationHelper.SmsKeyword))
                {
                    message = _configurationHelper.MenuLevel2;
                }
                else
                {
                    throw new Exception("Invalid Format.");
                }
            }

            return message;
        }

        private void RunCheckerProcess(MoUssdReceiver receiver, string cacheKey, ref bool clearCache, ref string response, ref string message, string sessionId)
        {
            var numbers = receiver.getMessage();
            if (string.IsNullOrEmpty(sessionId))// for SMS check
            {
                string smsKeyword = _configurationHelper.SmsKeyword;
                string subscriptionKeyword = _configurationHelper.SubscriptionKeyword;
                string unSubscriptionKeyword = _configurationHelper.UnSubscriptionKeyword;

                if (numbers.ToUpper().Trim().Contains(subscriptionKeyword))
                {
                    string chargeAmount = _configurationHelper.SubscriptionChargeAmount;
                    UpdateSubscription(receiver.SourceAddress, true);
                    message = _configurationHelper.SubscriptionConfirmMsg + _configurationHelper.UnSubscriptionMsg;
                    response = loadObjectSender(sessionId, message, receiver, chargeAmount);
                    clearCache = ClearCache(cacheKey, ref receiver);
                }
                else if (numbers.ToUpper().Trim().Contains(unSubscriptionKeyword))
                {
                    UpdateSubscription(receiver.SourceAddress, false);
                    message = _configurationHelper.UnSubscriptionConfirmMsg;
                }
                else if (numbers.ToUpper().Trim().Contains(smsKeyword))
                {
                    message = _configurationHelper.SMSLevel1;
                    message = CheckPBNumbers(numbers) + "\r\n" + message;
                }
                else {
                    message = receiver.getMessage();
                }
            }
            else
            {
                message = CheckPBNumbers(numbers) + "\r\n" + message;
            }

        }

        private bool ClearCache(string cacheKey, ref MoUssdReceiver receiver)
        {
            bool clearCache;
            this._configurationHelper.ClearCache(cacheKey);
            clearCache = true;
            receiver.Message = "0";
            return clearCache;
        }

        private string GetSubscriptionConfirmationMenu(MoUssdReceiver receiver, string message)
        {
            if (receiver.getMessage() == "1") //// Subscribe
            {
                message = _configurationHelper.MenuLevel3;
            }
            else if (receiver.getMessage() == "2") //// unsubscribe
            {
                message = _configurationHelper.MenuLevel4;
            }
            else if (receiver.getMessage() == "3") //// add more
            {
                message = _configurationHelper.MenuLevel1;
            }

            return message;
        }

        private string CheckPBNumbers(string pbNumbers)
        {
            string message = string.Empty;

            return message;
        }

        /*
   Get the session id and Response message as parameter
   Create sender object and send ussd with appropriate parameters
**/

        private string loadObjectSender(string sessionId, string responseMessage, MoUssdReceiver receiver, string chargingAmount = "0")
        {
            string password = _configurationHelper.Password;         // Configuration.GetSection("BDApps:password").Value;
            string destinationAddress = receiver.SourceAddress;
            string ussdOperation = string.Empty;

            if (!string.IsNullOrEmpty(sessionId))
            {
                ussdOperation = receiver.UssdOperation;
                if (receiver.Message == "0")
                {
                    ussdOperation = "mt-fin";
                }
                else
                {
                    ussdOperation = "mt-cont";
                }
            }
            string applicationId = receiver.ApplicationId;
            string encoding = receiver.Encoding;
            string version = receiver.Version;

            try
            {
                // Create the sender object server url
                string ussdServer = _configurationHelper.MtUssdSenderServer;     // Configuration.GetSection("BDApps:MtUssdSenderServer").Value;
                string smsServer = _configurationHelper.MtSmsSenderServer;       // Configuration.GetSection("BDApps:MtSmsSenderServer").Value;
                string response = string.Empty;
                MtUssdSender sender = new MtUssdSender(ussdServer, smsServer);  // Application ussd-mt sending http url

                if (string.IsNullOrEmpty(sessionId))
                {
                    chargingAmount = this._configurationHelper.SmsChargeAmount;
                    response = sender.sms(applicationId, password, version, responseMessage,
                   destinationAddress, encoding, chargingAmount);
                }
                else
                {
                    response = sender.ussd(applicationId, password, version, responseMessage,
               sessionId, ussdOperation, destinationAddress, encoding, chargingAmount);
                }
                return response;
            }
            catch (Exception ex)
            {
                //throws when failed sending or receiving the ussd
                //error_log("USSD ERROR: {ex->getStatusCode()} | {ex->getStatusMessage()}");
                //return null;
                throw ex;
            }
        }

        private void UpdateSubscription(string address, bool activate)
        {
            string MSISDN = address.Split(':').LastOrDefault();

            List<PBOnDemand> userOnDemandList = _pbOnDemandRepository.GetAll().Where(oItem => oItem.MSISDN == MSISDN).ToList();
            userOnDemandList.ForEach(oItem => { oItem.IsActiveSubscription= activate; oItem.LastUpdatedDate = DateTime.Now; });
            _pbOnDemandRepository.Save();
        }

        public bool IsActiveSubscriber(string address)
        {
            string MSISDN = address.Split(':').LastOrDefault();

            return _pbOnDemandRepository.GetAll().Any(oItem => oItem.MSISDN == MSISDN && oItem.IsActiveSubscription==true);
        }

        public List<string> GetAllSubscribers()
        {
            return _pbOnDemandRepository.GetAll().Where(oItem => oItem.IsActiveSubscription == true).Select(oItem=>oItem.MSISDN).Distinct().ToList();
        }

        public string GetAllDrawNumbersBySubscriberId(string subscriberID)
        {
            return _pbOnDemandRepository.GetAll().Where(oItem => oItem.MSISDN == subscriberID).Select(oItem => oItem.PBNo).Aggregate((a, b) => a + ',' + b);
        }


        public string SendSubscriberNotification() {
            string jsonResponse = string.Empty;
            try
            {
                List<string> subscribers = this.GetAllSubscribers();
                foreach (string subscriber in subscribers)
                {
                    SendIndivualSMSNotificationByMSISDN(subscriber);
                }

                jsonResponse = string.Format("Notification sent to {0} subscribers", subscribers.Count);
                return jsonResponse;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public string SendIndivualSMSNotificationByMSISDN(string subscriber)
        {
            string pbNumbers = this.GetAllDrawNumbersBySubscriberId(subscriber);
            string response = this._configurationHelper.SmsKeyword + " " + pbNumbers;
            MoUssdReceiver smsReceiver = new MoUssdReceiver();
            smsReceiver.ApplicationId = this._configurationHelper.ApplicationId;
            smsReceiver.Password = this._configurationHelper.Password;
            smsReceiver.SourceAddress = "tel:" + subscriber;
            smsReceiver.Message = response;
            smsReceiver.Version = "1.0";
            smsReceiver.Encoding = "0";

           return this.SendMessage(smsReceiver);
        }
    }
}
