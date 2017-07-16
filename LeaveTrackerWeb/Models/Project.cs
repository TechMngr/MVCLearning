using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveTrackerWeb.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [MaxLength (200)]
        [DisplayName("Project Name")]
        public string ProjectName { get; set; }

    }
}