using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class LeaveTypes
    {
        [Key]
        public int LeaveTypeId { get; set; }

        [StringLength(200)]
        public string LeaveTypeName { get; set; }
    }
}