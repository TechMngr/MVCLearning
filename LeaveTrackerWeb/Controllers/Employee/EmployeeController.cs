using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LeaveTrackerWeb.Models;

namespace LeaveTrackerWeb.Controllers.Employee
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Login()
        {
            return View();
        }

        
        public  ActionResult AddEmployee(EmplloyeeAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ProjectId = model.ProjectId, EmployeeNo = model.EmployeeNo };
            } 

            return View();
        }

        public ActionResult EmployeeAdd()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


    }
}
