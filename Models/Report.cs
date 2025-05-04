using System;
using System.ComponentModel.DataAnnotations;

namespace security_scan.Models
{
    public class Report
    {
        public int Id { get; set; }

  
        public string? UserId { get; set; }

     
        public DateTime ScanDate { get; set; }

    
        public string? SeverityLevel { get; set; }

        public string? Summary { get; set; }

        public string? Note { get; set; }


        
    }
}
