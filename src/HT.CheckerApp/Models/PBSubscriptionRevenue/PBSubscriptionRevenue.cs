using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBSubscriptionRevenue
    {
        [Key]
        public int Id { get; set; }
        public string MSISDN{ get; set; }
        public int Cycle { get; set; }
        public string SubscriptionType { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive{ get; set; }

    }
}
