using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models.LeaveModel
{
    public class UxLeaveTypes
    {
        [Key]

        public int UxLeaveTypeId { get; set; }

        [StringLength(200)]

        public string UxLeaveTypeName { get; set; }


        public int? LeaveTypeId { get; set; }

        [ForeignKey("LeaveTypeId")]
        public virtual LeaveTypes EmpLeaveType { get; set; }
    }
}