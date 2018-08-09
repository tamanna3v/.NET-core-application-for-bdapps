using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBSubscriptionsViewModel
    {
        [Required]
        public string PBNo { get; set; }

        [Required]
        public string MSISDN { get; set; }

        [Required]
        public DateTime PrizeDate { get; set; }

        [Required]
        public DateTime SubStartedDate { get; set; }

        [Required]
        public string Keyword { get; set; }

        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
