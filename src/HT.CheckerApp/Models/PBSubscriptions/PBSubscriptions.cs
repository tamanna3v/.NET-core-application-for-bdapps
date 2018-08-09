using System;
using System.ComponentModel.DataAnnotations;

namespace HT.CheckerApp.API.Models
{
    public class PBSubscriptions
    {
        [Key]
        public int Id { get; set; }
        public string PBNo { get; set; }
        public string MSISDN{ get; set; }
        public string Keyword { get; set; }
        public DateTime PrizeDate { get; set; }
        public DateTime SubStartedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive{ get; set; }

    }
}
