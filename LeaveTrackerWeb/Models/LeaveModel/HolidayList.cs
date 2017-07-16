using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class HolidayList
    {
        [Key]
        public int HolidayId { get; set; }
        public DateTime HolidayDate { get; set; }
        [StringLength(500)]
        public string DateDescription { get; set; }
    }
}