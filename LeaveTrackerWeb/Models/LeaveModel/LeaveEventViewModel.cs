using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class LeaveEventViewModel
    {
        public int Id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }

        public int LeaveTypeId { get; set; }
        public int UxLeaveTypeId { get; set; }
    }
}