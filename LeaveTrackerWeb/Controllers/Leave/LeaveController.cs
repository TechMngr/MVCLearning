
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using Model = LeaveTrackerWeb.Models;
using LeaveTrackerWeb.Models.LeaveModel;
using System.Data.SqlClient;
using System.Data;

namespace LeaveTrackerWeb.Controllers.Leave
{
    [Authorize]
    public class LeaveController : Controller
    {
        // GET: Leave
        public ActionResult ApplyLeave()
        {
            List<LeaveTypes> LeaveTypeList = GetAllLeaveTypes();
            List<HolidayList> Holidays = GetAllHolidays();
            ViewData["LeaveTypes"] = LeaveTypeList;
            ViewData["Holidays"] = Holidays;
            return View();
        }

        private List<HolidayList> GetAllHolidays()
        {
            using (Model.ApplicationDbContext ctx = new Models.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    return ctx.HolidayList.ToList();
                }
                catch (Exception ex)
                {
                    return new List<HolidayList>();
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }
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
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
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
                    return Json(ctx.UxLeaveTypes.ToList().Where(x => x.LeaveTypeId == intLeaveTypeId).ToList(), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new List<UxLeaveTypes>(), JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }

        }

        [HttpPost]
        public ActionResult ApplyLeave(LeaveMapping Leave)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    int TotalRequest = ctx.LeaveMapping.AsNoTracking().Count() > 0 ? ctx.LeaveMapping.AsNoTracking().Max(x => x.RequestId) : 0;
                    Leave.RequestId = TotalRequest + 1;

                    SqlParameter StartDate = new SqlParameter("@pinStartDate", Leave.StartDate.ToShortDateString());
                    SqlParameter EndDate = new SqlParameter("@pinEndDate", Leave.EndDate.ToShortDateString());

                    Leave.LeaveCount = ctx.Database.SqlQuery<int>("SELECT [dbo].[udf_TotalWorkingDays](@pinStartDate , @pinEndDate)", StartDate , EndDate ).SingleOrDefault();
                    ctx.LeaveMapping.Add(Leave);
                    ctx.SaveChanges();
                    return Json("Success");
                }
                catch (Exception ex)
                {
                    return Json("Error");
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