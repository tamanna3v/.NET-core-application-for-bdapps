using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBOnDemandViewModel
    {
        [Required]
        public string PBNo { get; set; }
        [Required] public string MSISDN { get; set; }
        public DateTime PrizeDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
