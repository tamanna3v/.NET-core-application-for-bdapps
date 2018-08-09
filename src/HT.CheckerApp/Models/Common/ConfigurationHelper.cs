﻿using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;

namespace HT.CheckerApp.API.Models.Common
{
    public class ConfigurationHelper
    {
        IConfiguration Configuration;
        private IMemoryCache _cache;
        public ConfigurationHelper(IConfiguration configuration
            , IMemoryCache memorycache)
        {
            Configuration = configuration;
            _cache = memorycache;
            this.USSDDialNo = Configuration.GetSection("BDApps:USSDDialNo").Value;
            this.MtUssdSenderServer = Configuration.GetSection("BDApps:MtUssdSenderServer").Value;
            this.MtSmsSenderServer = Configuration.GetSection("BDApps:MtSmsSenderServer").Value;
            this.ApplicationId = Configuration.GetSection("BDApps:applicationId").Value;
            this.Password = Configuration.GetSection("BDApps:password").Value;
            this.SmsKeyword = Configuration.GetSection("BDApps:SmsKeyword").Value;
            this.SubscriptionKeyword = Configuration.GetSection("BDApps:SubscriptionKeyword").Value;
            this.UnSubscriptionKeyword = Configuration.GetSection("BDApps:UnSubscriptionKeyword").Value;
            this.SubscriptionChargeAmount = Configuration.GetSection("BDApps:SubscriptionChargeAmount").Value;
            this.SmsChargeAmount = Configuration.GetSection("BDApps:SmsChargeAmount").Value;

            this.MenuLevel1 = "Input your prizebond numbers (Max 2 numbers or 1 series) (Ex:PBC 123456,524545 or PBC 125501-125560)";
            this.MenuLevel2 = "1. Subscribe (2.44 BDT/3month per draw)\r\n" +
                              "2. Unsubscribe\r\n" +
                              "3. Add more\r\n" +
                              "0. Exit";
            this.MenuLevel3 = "To confirm subscription, Press 1\r\n" +
                              "0. Exit";
            this.SMSLevel1 = "To get auto result (2.44 BDT/draw) type PBC SUB and send to 21213)";

            this.MenuLevel4 = "To confirm unsubscription, Press 1\r\n" +
                              "0. Exit";
            this.SubscriptionConfirmMsg = "Thanks for subscription. We will notify you every 3 months after draw.\r\n";
            this.UnSubscriptionConfirmMsg = "You have unsubscribed successfully.\r\n";
            this.UnSubscriptionMsg = "To unsubscribe, type PBC UNSUB and send to 21213";
        }

        public string USSDDialNo
        {
            get;
            set;
        }
        public string MtUssdSenderServer
        {
            get; set;
        }
        public string MtSmsSenderServer
        {
            get; set;
        }
        public string ApplicationId
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }
        public string SmsKeyword
        {
            get; set;
        }
        public string SubscriptionKeyword
        {
            get; set;
        }
        public string UnSubscriptionKeyword { get; set; }
        public string SubscriptionChargeAmount
        {
            get; set;
        }
        public string SmsChargeAmount
        {
            get; set;
        }
        public string MenuLevel1 { get; private set; }

        public string MenuLevel2 { get; private set; }

        public string MenuLevel3 { get; private set; }
        public string SMSLevel1 { get; private set; }
        public string MenuLevel4 { get; private set; }
        public string SubscriptionConfirmMsg { get; private set; }
        public string UnSubscriptionConfirmMsg { get; private set; }
        public string UnSubscriptionMsg { get; private set; }

        public string CacheTryGetValueSet(string key, string value)
        {
            // Look for cache key.
            if (!_cache.TryGetValue(key, out string cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = value;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(5 * 60));

                // Save data in cache.
                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
        public void AddCache(string key, string value)
        {

            // Set cache options.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Keep in cache for this time, reset time if accessed.
                .SetSlidingExpiration(TimeSpan.FromSeconds(5 * 60));

            // Save data in cache.
            _cache.Set(key, value, cacheEntryOptions);
        }
        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
        public string FindNextDrawDate()
        {
            DateTime dt = new DateTime();
            int[] monthArray = new int[] { 1, 4, 7, 10 };
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;
            foreach (int month in monthArray)
            {
                if (currentMonth == 11 || currentMonth == 12)
                {
                    dt = new DateTime(currentYear + 1, 1, DateTime.DaysInMonth(currentYear + 1, 1));
                    break;
                }
                if (currentMonth <= month)
                {
                    dt = new DateTime(currentYear, month, DateTime.DaysInMonth(currentYear, month));
                    break;
                }
                else if (currentMonth > month)
                {
                    continue;
                }

            }
            return dt.ToShortDateString();
        }
    }
}
