using HT.CheckerApp.API.Models;
using HT.CheckerApp.API.Models.Common;
using HT.CheckerApp.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HT.CheckerApp.API.Controllers
{
    [Produces("application/json")]
    [Route("api/PBCheck")]
    public class PBCheckController : Controller
    {
        private readonly IConfiguration Configuration;
        private readonly IPBDrawResultRepository PBDrawResultRepository;
        private readonly IPBOnDemandRepository PBOnDemandRepository;
        private readonly IPBSubscriptionsRepository PBSubscriptionsRepository;
        private readonly ConfigurationHelper _configurationHelper;
        private IMemoryCache _cache;

        public PBCheckController(IConfiguration configuration
            , IPBDrawResultRepository pbDrawResultRepository
            , IPBOnDemandRepository pbOnDemandRepository
            , IPBSubscriptionsRepository pbSubscriptionsRepository
            , IMemoryCache memoryCache)
        {
            Configuration = configuration;
            _cache = memoryCache;
            PBDrawResultRepository = pbDrawResultRepository;
            PBOnDemandRepository = pbOnDemandRepository;
            PBSubscriptionsRepository = pbSubscriptionsRepository;
            _configurationHelper = new ConfigurationHelper(configuration,memoryCache);
        }

        
        // GET: api/PBCheck
        [HttpGet("", Name = "GetSubscriberAlert")]
        public string Get()
        {
            return SendSubscriberNotification();
        }
        // GET: api/PBCheck/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _configurationHelper.FindNextDrawDate();
        }

        // POST: api/PBCheck
        [HttpPost]
        public string Post([FromBody]Object id)
        {
            MoUssdReceiver receiver = JsonConvert.DeserializeObject<MoUssdReceiver>(id.ToString());
            string content = receiver.getMessage();
            string address = receiver.getAddress(); // get the sender's address

            BDAppsRepository bdapps = new BDAppsRepository(Configuration, PBDrawResultRepository, PBOnDemandRepository, _cache);
            string json = bdapps.SendMessage(receiver);

            if (content.ToUpper().Contains(_configurationHelper.SmsKeyword))
            {
                SaveOnDemandData(content, address);

                if (!string.IsNullOrEmpty(receiver.SessionId))
                {
                    var result = this.SendOndemandSMSNotificationByMSISDN(address.Split(':').LastOrDefault()).Result;
                }
            }
            return json;
        }

        private void SaveSubscriptionData(string content, string address)
        {
            string numbers = content.Substring(7).Trim(); // get the numbers
            string MSISDN = address.Split(':').LastOrDefault();

            PBSubscriptions pbSubscriptions = new PBSubscriptions()
            {
                MSISDN = MSISDN,
                PBNo = numbers,
                CreatedBy = MSISDN,
                LastUpdatedBy = MSISDN,
                PrizeDate = DateTime.Now,
                SubStartedDate=DateTime.Now,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                IsActive = true
            };
            PBSubscriptionsRepository.Add(pbSubscriptions);
            PBSubscriptionsRepository.Save();
        }

        private void SaveOnDemandData(string numbers, string address)
        {
            //// Should save for ussd and sms both
            if (numbers.ToUpper().Contains(_configurationHelper.SubscriptionKeyword) || numbers.ToUpper().Contains(_configurationHelper.UnSubscriptionKeyword))
                return;
            string msisdnNumber = address.Split(':').LastOrDefault();
            string pbNumbers = numbers.Substring(4).ToUpper().Trim();
            BDAppsRepository bdapps = new BDAppsRepository(Configuration, PBDrawResultRepository, PBOnDemandRepository, _cache);
            SaveAllPBNumbers(numbers, address, msisdnNumber, pbNumbers, bdapps);

        }

        private void SaveAllPBNumbers(string numbers, string address, string msisdnNumber, string pbNumbers, BDAppsRepository bdapps)
        {
            PBOnDemand pbOnDemandToUpdate = PBOnDemandRepository.GetAll().FirstOrDefault(oItem => oItem.MSISDN == msisdnNumber && oItem.Keyword == _configurationHelper.SmsKeyword);
            if (pbOnDemandToUpdate != null)
            {
                string concatePBNumbers = pbOnDemandToUpdate.PBNo + "," + pbNumbers;
                pbOnDemandToUpdate.PBNo = concatePBNumbers.Split(',').Distinct().Aggregate((a, b) => a.Trim() + "," + b.Trim()).ToString();

                pbOnDemandToUpdate.LastUpdatedDate = DateTime.Now;
                PBOnDemandRepository.Update(pbOnDemandToUpdate);
                PBOnDemandRepository.Save();
            }
            else
            {
                PBOnDemand pbOnDemand = new PBOnDemand();
                pbOnDemand.MSISDN = msisdnNumber;
                pbOnDemand.Keyword = numbers.Substring(0, 3).ToUpper().Trim();
                pbOnDemand.PBNo = pbNumbers;
                pbOnDemand.CreatedBy = pbOnDemand.MSISDN;
                pbOnDemand.LastUpdatedBy = pbOnDemand.MSISDN;
                pbOnDemand.PrizeDate = DateTime.Now;
                pbOnDemand.CreatedDate = DateTime.Now;
                pbOnDemand.LastUpdatedDate = DateTime.Now;
                pbOnDemand.IsActive = true;
                pbOnDemand.IsActiveSubscription = bdapps.IsActiveSubscriber(address);
                PBOnDemandRepository.Add(pbOnDemand);
                PBOnDemandRepository.Save();
            }
        }

        private string SendSubscriberNotification()
        {
            BDAppsRepository bdapps = new BDAppsRepository(Configuration, PBDrawResultRepository, PBOnDemandRepository, _cache);
            return bdapps.SendSubscriberNotification();
        }
        private async Task<string> SendOndemandSMSNotificationByMSISDN(string MSISDN)
        {
            if (_cache.TryGetValue(MSISDN, out string value))
                return null;

            _configurationHelper.AddCache(MSISDN, MSISDN);
           
            await Task.Delay(5*60*1000);
            BDAppsRepository bdapps = new BDAppsRepository(Configuration, PBDrawResultRepository, PBOnDemandRepository, _cache);
            return bdapps.SendIndivualSMSNotificationByMSISDN(MSISDN); 
        }

        // PUT: api/PBCheck/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
