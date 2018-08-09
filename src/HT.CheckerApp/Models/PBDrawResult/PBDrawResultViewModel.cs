using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBDrawResultViewModel
    {
        [Required]
        public string PBNo { get; set; }
        [Required]
        public int Prize { get; set; }
        [Required]
        public double PrizeValue { get; set; }
        [Required]
        public int Draw { get; set; }
        [Required]
        public DateTime Validity { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
