using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBDrawResult
    {
        [Key]
        public int Id { get; set; }
        public string PBNo { get; set; }
        public int Prize { get; set; }
        public double PrizeValue { get; set; }
        public int Draw{ get; set; }
        public DateTime Validity { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }

    }
}
