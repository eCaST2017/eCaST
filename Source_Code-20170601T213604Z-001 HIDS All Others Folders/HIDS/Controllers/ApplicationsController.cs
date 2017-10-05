using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.Models;
using CTL.ViewModels;

namespace CTL.Controllers
{
    public class ApplicationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Applications
        public ActionResult Index()
        {


            string UserName = @User.Identity.Name.ToString();
            var userid = (from x in db.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();
           

            if (userid.RoleBinID == 1)
            {
                var applications = new List<ApplicationViewModel>();
                var apps = (from x in db.Applications
                            join w in db.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            //join z in db.SiteRoleProgramUserProfiles
                            //on w.RoleID equals z.RoleID
                            //join y in db.AspNetUsers
                            //on z.Id equals y.Id
                            //where z.Id == userid.Id && w.ProgramID == z.ProgramID
                            where x.Active == true
                            select new ApplicationViewModel()
                            {
                                ApplicationID = x.ApplicationID,
                                ApplicationName = x.ApplicationName,
                                ApplicationDescription = x.ApplicationDescription,
                                HTTP = x.HTTP,
                                Active = x.Active,
                                DateUpdated = x.DateUpdated,
                                ProgramAList = (from anu in db.Programs
                                             join sr in db.ApplicationRolePrograms
                                             on anu.ProgramID equals sr.ProgramID
                                             where sr.ApplicationID == x.ApplicationID && anu.Active == true
                                             select anu.ProgramName).Distinct().ToList(),
                                RoleBinID = 1,
                                UpdatedBy = x.UpdatedBy,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).ToList();

                foreach (ApplicationViewModel up in apps)
                {


                    bool alreadyExists = applications.Any(x => x.ApplicationID == up.ApplicationID);

                    if (alreadyExists == false)
                    {

                        applications.Add(up);

                    }

                }

                ViewBag.AppCount = applications.Count();


              

                ViewBag.RoleID = userid.RoleBinID;



                return PartialView(applications);


            }
            else if (userid.RoleBinID == 3 || userid.RoleBinID == 13)
            {
                var applications = new List<ApplicationViewModel>();
                var apps = (from x in db.Applications
                            join w in db.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in db.RoleProgramUserProfiles
                            on w.RoleID equals z.RoleID
                            where z.Id == userid.Id && w.ProgramID == z.ProgramID && x.Active == true

                            select new ApplicationViewModel()
                            {
                                ApplicationID = x.ApplicationID,
                                ApplicationName = x.ApplicationName,
                                ApplicationDescription = x.ApplicationDescription,
                                HTTP = x.HTTP,
                                Active = x.Active,
                                DateUpdated = x.DateUpdated,
                                UpdatedBy = x.UpdatedBy,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy,
                                ProgramAList = (from anu in db.Programs
                                                join sr in db.ApplicationRolePrograms
                                                on anu.ProgramID equals sr.ProgramID
                                                where sr.ApplicationID == x.ApplicationID && anu.Active == true
                                                select anu.ProgramName).Distinct().ToList(),
                                RoleListCount = (from c in db.AspNetUsers
                                                 join y in db.SiteRoleProgramUserProfiles
                                                 on c.Id equals y.Id
                                                 where c.Id == userid.Id && y.RoleID != 2
                                                 select y.RoleID).Distinct().Count(),
                                RoleBinID = (from s in db.AspNetUsers
                                             join y in db.SiteRoleProgramUserProfiles
                                             on s.Id equals y.Id
                                             join u in db.Programs
                                             on y.ProgramID equals u.ProgramID
                                             join h in db.ApplicationRolePrograms
                                             on u.ProgramID equals h.ProgramID
                                             where s.UserName == UserName && h.ProgramID == z.ProgramID 
                                             select y.RoleID).FirstOrDefault()

                            }).ToList();



                foreach (ApplicationViewModel up in apps)
                {


                    bool alreadyExists = applications.Any(x => x.ApplicationID == up.ApplicationID);

                    if (alreadyExists == false)
                    {

                        applications.Add(up);

                    }

                }

                ViewBag.AppCount = applications.Count();



                return PartialView(applications);


            }
            else
            {


                var applications = new List<ApplicationViewModel>();
                var apps = (from x in db.Applications
                            join w in db.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in db.SiteRoleProgramUserProfiles
                            on w.RoleID equals z.RoleID
                            where z.Id == userid.Id && w.ProgramID == z.ProgramID && x.Active == true

                            select new ApplicationViewModel()
                            {
                                ApplicationID = x.ApplicationID,
                                ApplicationName = x.ApplicationName,
                                ApplicationDescription = x.ApplicationDescription,
                                HTTP = x.HTTP,
                                Active = x.Active,
                                DateUpdated = x.DateUpdated,
                                UpdatedBy = x.UpdatedBy,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy,
                ProgramAList = (from anu in db.Programs
                                join sr in db.ApplicationRolePrograms
                                on anu.ProgramID equals sr.ProgramID
                                where sr.ApplicationID == x.ApplicationID && anu.Active == true
                                select anu.ProgramName).Distinct().ToList(),
                RoleListCount = (from c in db.AspNetUsers
                                join y in db.SiteRoleProgramUserProfiles
                                on c.Id equals y.Id
                                where c.Id == userid.Id && y.RoleID != 2
                                select y.RoleID).Distinct().Count(),
                RoleBinID =     (from s in db.AspNetUsers
                                 join y in db.SiteRoleProgramUserProfiles
                                 on s.Id equals y.Id
                                 join u in db.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join h in db.ApplicationRolePrograms
                                 on u.ProgramID equals h.ProgramID
                                 where s.UserName == UserName && h.ProgramID == z.ProgramID
                                 select y.RoleID).FirstOrDefault()

                            }).ToList();


                foreach (ApplicationViewModel up in apps)
                {


                    bool alreadyExists = applications.Any(x => x.ApplicationID == up.ApplicationID);

                    if (alreadyExists == false)
                    {

                        applications.Add(up);

                    }

                }



                ViewBag.AppCount = applications.Count();

                return PartialView(applications);
            }
            

           
           

            return PartialView();


        }

        // GET: Applications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // GET: Applications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicationID,ApplicationName,ApplicationDescription,HTTP,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(application);
        }

        // GET: Applications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }


            var appname = (from x in db.Applications
                            where x.ApplicationID == id
                            select x.ApplicationName).SingleOrDefault();

            ViewBag.ApplicationName = appname;

            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicationID,ApplicationName,ApplicationDescription,HTTP,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(application);
        }

        // GET: Applications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
