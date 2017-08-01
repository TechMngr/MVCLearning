
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
using Microsoft.AspNet.Identity;

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
                    ctx.Configuration.ProxyCreationEnabled = false;
                    int TotalRequest = ctx.LeaveMapping.AsNoTracking().Count() > 0 ? ctx.LeaveMapping.AsNoTracking().Max(x => x.RequestId) : 0;
                    Leave.RequestId = TotalRequest + 1;
                    SqlParameter EndDateObj = new SqlParameter("@pinEndDate", Leave.EndDate.ToShortDateString());
                    Leave.EndDate = ctx.Database.SqlQuery<DateTime>("SELECT [dbo].[udf_GetCalculatedEndDate](@pinEndDate)", EndDateObj).SingleOrDefault();
                    SqlParameter StartDate = new SqlParameter("@pinStartDate", Leave.StartDate.ToShortDateString());
                    SqlParameter EndDate = new SqlParameter("@pinEndDate", Leave.EndDate.ToShortDateString());

                    Leave.LeaveCount = ctx.Database.SqlQuery<int>("SELECT [dbo].[udf_TotalWorkingDays](@pinStartDate , @pinEndDate)", StartDate, EndDate).SingleOrDefault();
                    ctx.LeaveMapping.Add(Leave);
                    ctx.SaveChanges();
                    LeaveMapping LatestLeave = ctx.LeaveMapping.AsNoTracking().OrderByDescending(x => x.LeaveMappingId).FirstOrDefault();
                    return Json(LatestLeave);
                }
                catch (Exception ex)
                {
                    return Json("Error");
                }
                finally {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }
        }

        [HttpPost]
        public ActionResult UpdaateLeave(LeaveMapping Leave)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    int TotalRequest = ctx.LeaveMapping.AsNoTracking().Count() > 0 ? ctx.LeaveMapping.AsNoTracking().Max(x => x.RequestId) : 0;
                    Leave.RequestId = TotalRequest + 1;
                    SqlParameter EndDateObj = new SqlParameter("@pinEndDate", Leave.EndDate.ToShortDateString());
                    Leave.EndDate = ctx.Database.SqlQuery<DateTime>("SELECT [dbo].[udf_GetCalculatedEndDate](@pinEndDate)", EndDateObj).SingleOrDefault();
                    SqlParameter StartDate = new SqlParameter("@pinStartDate", Leave.StartDate.ToShortDateString());
                    SqlParameter EndDate = new SqlParameter("@pinEndDate", Leave.EndDate.ToShortDateString());

                    Leave.LeaveCount = ctx.Database.SqlQuery<int>("SELECT [dbo].[udf_TotalWorkingDays](@pinStartDate , @pinEndDate)", StartDate, EndDate).SingleOrDefault();
                    var original = ctx.LeaveMapping.Find(Leave.LeaveMappingId);

                    ctx.Entry(original).CurrentValues.SetValues(Leave);
                   
                    ctx.SaveChanges();
                    return Json("Success");
                }
                catch (Exception ex)
                {
                    return Json("Error");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteLeave(LeaveMapping Leave)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    var original = ctx.LeaveMapping.Find(Leave.LeaveMappingId);
                    var AllList = ctx.LeaveMapping.ToList();
                    ctx.LeaveMapping.Remove(original);
                    ctx.SaveChanges();
                    var original1 = ctx.LeaveMapping.ToList();
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
            string UserID = User.Identity.GetUserId();
            List<LeaveMapping> Leaves = new List<LeaveMapping>();
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    Leaves = ctx.LeaveMapping.Include("MyLeaveType").Include("MyUxLeaveType").Where(x => x.EmployeeId == UserID).ToList(); 
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }

            return View(Leaves);

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

        [HttpGet]
        public ActionResult GetCalenderLeavesEvents(string start, string end)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    DateTime EventStart = Convert.ToDateTime(start);
                    DateTime EventEnd = Convert.ToDateTime(end);
                    ctx.Configuration.ProxyCreationEnabled = false;
                    List<LeaveEventViewModel> EventList = ctx.LeaveMapping.Where(y=>(y.StartDate >= EventStart && y.EndDate <= EventEnd)).ToList().Select(x => new LeaveEventViewModel { Id = x.LeaveMappingId, start = x.StartDate, end =x.EndDate.AddDays(1), title = x.LeaveDescription ,LeaveTypeId=x.LeaveTypeId,UxLeaveTypeId=x.UxLeaveTypeId}).ToList();
                    EventList.ForEach(x =>
                    {
                        DateTime s = x.start;
                        TimeSpan ts = new TimeSpan(7, 00, 0);
                        s = s.Date + ts;
                        x.start = s;
                        s = x.end;
                        ts = new TimeSpan(17, 00, 0);
                        s = s.Date + ts;
                        x.end = s;
                    });

                    return Json(EventList,JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }
        }

        public PartialViewResult LeaveTimeLine(string LeaveType)
        {
            int LeaveTypeId = 0;
            if ((!string.IsNullOrEmpty(LeaveType)) && (LeaveType.ToUpper() == "UX"))
            {
                LeaveTypeId = 2;
            }
            else if((!string.IsNullOrEmpty(LeaveType))) {
                LeaveTypeId = 1; 
            }

            string UserID = User.Identity.GetUserId();
            List<LeaveMapping> Leaves = new List<LeaveMapping>();
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    Leaves = ctx.LeaveMapping.Include("MyLeaveType").Include("MyUxLeaveType").Where(x => x.EmployeeId == UserID && x.LeaveTypeId == LeaveTypeId).OrderByDescending(t=>t.StartDate).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ctx.Configuration.ProxyCreationEnabled = true;
                }
            }

            return PartialView(Leaves);
        }



        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Add Holiday")]
        public ActionResult AddHoliday(Model.LeaveModel.HolidayList HolidayDetails)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                ctx.HolidayList.Add(HolidayDetails);
                ctx.SaveChanges();
                TempData["Message"] = "Holiday Added Successfully.";
                return RedirectToAction("Holidays", "Leave");
            }
        }


        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Update Holiday")]
        public ActionResult UpdateHoliday(Model.LeaveModel.HolidayList HolidayDetail)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {

                ctx.HolidayList.Attach(HolidayDetail);
                ctx.Entry(HolidayDetail).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                TempData["Message"] = "Holiday updated successfully";
                return RedirectToAction("Holidays", "Leave");
            }
        }

        [HttpGet]
        public PartialViewResult HolidayDetails(int HolidayId = 0)
        {
            Model.LeaveModel.HolidayList HolidayObj = new Model.LeaveModel.HolidayList();
            if (HolidayId > 0)
            {
                using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
                {
                    try
                    {
                        ctx.Configuration.ProxyCreationEnabled = false;
                        HolidayObj = ctx.HolidayList.Where(x => x.HolidayId == HolidayId).FirstOrDefault();
                    }

                    catch (Exception Ex)
                    {

                    }
                    finally
                    {
                        ctx.Configuration.ProxyCreationEnabled = true;
                    }
                }



            }
            return PartialView("HolidayDetails", HolidayObj);
        }

        [HttpPost]
        public ActionResult DeleteHoliday(string HolidayDelId)
        {
            using (Model.ApplicationDbContext ctx = new Model.ApplicationDbContext())
            {
                Model.LeaveModel.HolidayList DelHoliday = new Models.LeaveModel.HolidayList();
                int HolidayId = Convert.ToInt32(HolidayDelId);
                DelHoliday = ctx.HolidayList.Where(x => x.HolidayId == HolidayId).FirstOrDefault();
                ctx.HolidayList.Remove(DelHoliday);
                ctx.SaveChanges();
                TempData["Message"] = "Holiday Deleted Successfully.";
                return RedirectToAction("Holidays", "Leave");
            }
        }



    }
}