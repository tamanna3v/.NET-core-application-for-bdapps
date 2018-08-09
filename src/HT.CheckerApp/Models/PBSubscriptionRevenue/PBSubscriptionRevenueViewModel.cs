using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBSubscriptionRevenueViewModel
    {
       
        [Required]
        public string MSISDN { get; set; }
        public int Cycle { get; set; }
        [Required]
        public string SubscriptionType { get; set; }
        [Required]
        public DateTime SubscriptionDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
