using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class LeaveMapping
    {
        [Key]
     
        public int LeaveMappingId { get; set; }

        public int RequestId { get; set; }

        public string EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public ApplicationUser Emp { get; set; }

        public int LeaveTypeId { get; set; }

        [ForeignKey("LeaveTypeId")]

        public LeaveTypes MyLeaveType { get; set; }

        public int UxLeaveTypeId { get; set; }

        [ForeignKey("UxLeaveTypeId")]
        public UxLeaveTypes MyUxLeaveType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int LeaveStatusId { get; set; }

        [ForeignKey("LeaveStatusId")]
        public LeaveStatus MyLeaveStatus { get; set; }

        public string LeaveDescription { get; set; }

        public float LeaveCount { get; set; }
    }
}