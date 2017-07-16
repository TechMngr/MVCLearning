using LeaveTrackerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model = LeaveTrackerWeb.Models;

namespace LeaveTrackerWeb.Controllers.Project
{
    [Authorize]
    public class ProjectController : Controller
    {
        [HttpGet]
        public ActionResult ProjectList()
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                try
                {
                    ctx.Configuration.ProxyCreationEnabled = false;
                    List<Model.Project> ProjectList = ctx.Project.ToList();
                    if (TempData["Message"] != null)
                    {
                        ViewBag.message = TempData["Message"];
                    }
                    return View(ProjectList);
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

       

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Add Project")]
        public ActionResult AddProject(Model.Project ProjectDetails)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                ctx.Project.Add(ProjectDetails);
                ctx.SaveChanges();
                TempData["Message"] = "Project Added Successfully.";
                return RedirectToAction("ProjectList","Project");
            }
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Update Project")]
        public ActionResult UpdateProject(Model.Project ProjectDetails)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                ctx.Project.Attach(ProjectDetails);
                ctx.Entry(ProjectDetails).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
                TempData["Message"] = "Project Updated Successfully.";
                return RedirectToAction("ProjectList","Project");
            }
        }

        [HttpPost]
        public ActionResult DeleteProject(string ProjectDelId)
        {
            using (ApplicationDbContext ctx = new ApplicationDbContext())
            {
                Model.Project DelProj = new Models.Project();
                int ProjectId = Convert.ToInt32(ProjectDelId);
                DelProj = ctx.Project.Where(x => x.ProjectId == ProjectId).FirstOrDefault();
                ctx.Project.Remove(DelProj);
                ctx.SaveChanges();
                TempData["Message"] = "Project Deleted Successfully.";
                return RedirectToAction("ProjectList", "Project");
            }
        }


        [HttpGet]
        public PartialViewResult ProjectDetails(int ProjectId = 0)
        {
            Model.Project ProjectObj = new Model.Project();
            if (ProjectId > 0)
            {
                using (ApplicationDbContext ctx = new ApplicationDbContext())
                {
                    try
                    {
                        ctx.Configuration.ProxyCreationEnabled = false;
                        ProjectObj = ctx.Project.Where(x => x.ProjectId == ProjectId).FirstOrDefault();
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
            return PartialView("ProjectDetails", ProjectObj);
        }

    }
}