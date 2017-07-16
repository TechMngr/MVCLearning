using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class LeaveStatus
    {
        [Key]
        public int StatusId { get; set; }

        [StringLength(200)]
        
        public string Status { get; set; }
    }
}