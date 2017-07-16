
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using Model = LeaveTrackerWeb.Models;
using LeaveTrackerWeb.Models.LeaveModel;

namespace LeaveTrackerWeb.Controllers.Leave
{
    [Authorize]
    public class LeaveController : Controller
    {
        // GET: Leave
        public ActionResult ApplyLeave()
        {
            List<LeaveTypes> LeaveTypeList = GetAllLeaveTypes();
            ViewData["LeaveTypes"] = LeaveTypeList;
            return View();
        }

        private List<LeaveTypes> GetAllLeaveTypes()
        {
            using (Model.ApplicationDbContext ctx = new Models.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    return ctx.LeaveTypes.ToList();
                }
                catch (Exception ex)
                {
                    return new List<LeaveTypes>();
                }
                finally {
                    ctx.Configuration.ProxyCreationEnabled =true;
                }
            }
            
        }

        [HttpGet]
        public ActionResult GetSubLeaveType(string LeaveTypeId)
        {
            int intLeaveTypeId = 0;
            bool Success = int.TryParse(LeaveTypeId, out intLeaveTypeId);
            using (Model.ApplicationDbContext ctx = new Models.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    return Json(ctx.UxLeaveTypes.ToList().Where(x => x.LeaveTypeId == intLeaveTypeId).ToList(),JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new List<UxLeaveTypes>(),JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }

        }

        [HttpGet]
        public ActionResult LeaveDetails()
        {
            return View();
           
        }
        [HttpGet]
        public ActionResult Holidays()
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    List<Model.LeaveModel.HolidayList> holiDaysList = ctx.HolidayList.ToList();
                    if (TempData["Message"] != null)
                    {
                        ViewBag.message = TempData["Message"];
                    }
                    return View(holiDaysList);
                }
                catch (Exception ex)
                {
                    return View();
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }
           
        }
           
    }
}