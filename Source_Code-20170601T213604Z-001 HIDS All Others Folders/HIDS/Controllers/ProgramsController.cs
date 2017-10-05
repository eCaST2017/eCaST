using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{
    public class ProgramsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Programs
        public ActionResult Index()
        {
            string UserName = @User.Identity.Name.ToString();
            var userid = (from x in db.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();
           

            if (userid.RoleBinID == 1)
            {
                var programs = new List<ProgramViewModel>();
                var apps = (from x in db.Programs
                            join w in db.ApplicationRolePrograms
                            on x.ProgramID equals w.ProgramID
                            //join z in db.SiteRoleProgramUserProfiles
                            //on w.RoleID equals z.RoleID
                            //join y in db.AspNetUsers
                            //on z.Id equals y.Id
                            //where z.Id == userid.Id && w.ProgramID == z.ProgramID

                            select new ProgramViewModel()
                            {
                                ProgramID = x.ProgramID,
                                ProgramName = x.ProgramName,
                                ProgramCode = x.ProgramCode,
                                ProgramContact = x.ProgramContact,
                                ApplicationAList = (from anu in db.Applications
                                             join pr in db.ApplicationRolePrograms
                                             on anu.ApplicationID equals pr.ApplicationID
                                             where pr.ProgramID == x.ProgramID
                                             select anu.ApplicationName).Distinct().ToList(),
                                RoleBinID = 1,
                                Active = x.Active,
                                DateUpdated = x.DateUpdated,
                                UpdatedBy = x.UpdatedBy,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).ToList();


                foreach (ProgramViewModel up in apps)
                {


                    bool alreadyExists = programs.Any(x => x.ProgramID == up.ProgramID);

                    if (alreadyExists == false)
                    {

                        programs.Add(up);

                    }

                }


                ViewBag.ProgramCount = programs.Count();


            

                return PartialView(programs);


            }
            else if (userid.RoleBinID == 3 || userid.RoleBinID == 13 || userid.RoleBinID == 39)
            {
                var programs = new List<ProgramViewModel>();
                var apps = (from x in db.Programs
                            join w in db.ApplicationRolePrograms
                            on x.ProgramID equals w.ProgramID
                            join z in db.RoleProgramUserProfiles
                            on w.RoleID equals z.RoleID
                            join y in db.AspNetUsers
                            on z.Id equals y.Id
                            where z.Id == userid.Id && w.ProgramID == z.ProgramID

                            select new ProgramViewModel()
                            {
                                ProgramID = x.ProgramID,
                                ProgramName = x.ProgramName,
                                ProgramCode = x.ProgramCode,
                                ProgramContact = x.ProgramContact,
                                ApplicationAList = (from anu in db.Applications
                                                    join pr in db.ApplicationRolePrograms
                                                    on anu.ApplicationID equals pr.ApplicationID
                                                    where pr.ProgramID == x.ProgramID
                                                    select anu.ApplicationName).Distinct().ToList(),
                                 RoleBinID = (from s in db.AspNetUsers
                                             join a in db.RoleProgramUserProfiles
                                             on s.Id equals a.Id
                                             join u in db.Programs
                                             on a.ProgramID equals u.ProgramID
                                             join h in db.ApplicationRolePrograms
                                             on u.ProgramID equals h.ProgramID
                                             where s.UserName == UserName && h.ProgramID == z.ProgramID
                                             select a.RoleID).FirstOrDefault(),
                                Active = x.Active,
                                DateUpdated = x.DateUpdated,
                                UpdatedBy = x.UpdatedBy,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).ToList();

                foreach (ProgramViewModel up in apps)
                {


                    bool alreadyExists = programs.Any(x => x.ProgramID == up.ProgramID);

                    if (alreadyExists == false)
                    {

                        programs.Add(up);

                    }

                }


                ViewBag.ProgramCount = programs.Count();




                return PartialView(programs);


            }

            return PartialView();
        }



        public ActionResult _ProgramManagement(int? id)
        {

            ViewBag.SiteID = id;
            List<SelectListItem> items = new List<SelectListItem>();
            List<ProgramViewModel> ProgramItems = new List<ProgramViewModel>();
            string UserName = @User.Identity.Name.ToString();
            var userid = (from x in db.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.RoleBinID = userid.RoleBinID;

            if (userid.RoleBinID == 1 || userid.RoleBinID == 13 || userid.RoleBinID == 3)
            {


                var programs = (from Y in db.AgencySiteProgramSites
                                join P in db.Sites
                               on Y.SiteID equals P.SiteID
                                join X in db.Programs
                                on Y.ProgramID equals X.ProgramID
                                join Z in db.RoleProgramUserProfiles
                                on X.ProgramID equals Z.ProgramID
                                where P.SiteID == id && Z.Id == userid.Id
                                select X.ProgramID).ToList();

                foreach (int p in programs)
                {

                    var SL = (from Y in db.AgencySiteProgramSites
                              join P in db.Sites
                             on Y.SiteID equals P.SiteID
                              join X in db.Programs
                              on Y.ProgramID equals X.ProgramID
                              where X.ProgramID == p
                              select new
                              {
                                  ProgramID = X.ProgramID,
                                  ProgramName = X.ProgramName

                              }).Distinct();


                    if (SL.Count() > 0)
                    {

                        int ProgramID = SL.FirstOrDefault().ProgramID;
                        string PID = ProgramID.ToString();
                        string ProgramName = SL.FirstOrDefault().ProgramName;

                        bool alreadyExists = items.Any(x => x.Value == PID);

                        if (alreadyExists == false)
                        {

                            items.Add(new SelectListItem { Text = ProgramName, Value = PID, Selected = false });


                        }

                    }


                }

                foreach (SelectListItem num in items)
                {

                    int PID = Convert.ToInt32(num.Value);

                    var programs0 = (from c in db.Programs
                                     where c.ProgramID == PID
                                     select new ProgramViewModel()
                                     {

                                         ProgramID = c.ProgramID,
                                         ProgramName = c.ProgramName


                                     }).Distinct();



                    if (programs0.Count() > 0)
                    {
                        int ProgramID = programs0.FirstOrDefault().ProgramID;
                        string ProgramName = programs0.FirstOrDefault().ProgramName;
                        //string ClinicName = clinics.FirstOrDefault().ClinicName;

                        bool alreadyExists = ProgramItems.Any(x => x.ProgramID == ProgramID);

                        if (alreadyExists == false)
                        {

                            ProgramItems.Add(new ProgramViewModel() { ProgramID = ProgramID, ProgramName = ProgramName, SiteID = id });


                        }
                    }


                }


            }
            else
            {

                var programs = (from Y in db.AgencySiteProgramSites
                                join P in db.Sites
                               on Y.SiteID equals P.SiteID
                                join X in db.Programs
                                on Y.ProgramID equals X.ProgramID
                                join Z in db.SiteRoleProgramUserProfiles
                                on X.ProgramID equals Z.ProgramID
                                where P.SiteID == id && Z.Id == userid.Id
                                select X.ProgramID).ToList();

                foreach (int p in programs)
                {

                    var SL = (from Y in db.AgencySiteProgramSites
                              join P in db.Sites
                             on Y.SiteID equals P.SiteID
                              join X in db.Programs
                              on Y.ProgramID equals X.ProgramID
                              where X.ProgramID == p
                              select new
                              {
                                  ProgramID = X.ProgramID,
                                  ProgramName = X.ProgramName

                              }).Distinct();


                    if (SL.Count() > 0)
                    {

                        int ProgramID = SL.FirstOrDefault().ProgramID;
                        string PID = ProgramID.ToString();
                        string ProgramName = SL.FirstOrDefault().ProgramName;

                        bool alreadyExists = items.Any(x => x.Value == PID);

                        if (alreadyExists == false)
                        {

                            items.Add(new SelectListItem { Text = ProgramName, Value = PID, Selected = false });


                        }

                    }


                }

                foreach (SelectListItem num in items)
                {

                    int PID = Convert.ToInt32(num.Value);

                    var programs0 = (from c in db.Programs
                                     where c.ProgramID == PID
                                     select new ProgramViewModel()
                                     {

                                         ProgramID = c.ProgramID,
                                         ProgramName = c.ProgramName


                                     }).Distinct();



                    if (programs0.Count() > 0)
                    {
                        int ProgramID = programs0.FirstOrDefault().ProgramID;
                        string ProgramName = programs0.FirstOrDefault().ProgramName;
                        //string ClinicName = clinics.FirstOrDefault().ClinicName;

                        bool alreadyExists = ProgramItems.Any(x => x.ProgramID == ProgramID);

                        if (alreadyExists == false)
                        {

                            ProgramItems.Add(new ProgramViewModel() { ProgramID = ProgramID, ProgramName = ProgramName, SiteID = id });


                        }
                    }


                }




            }

            return PartialView(ProgramItems);

        }


        // GET: Sites
        public ActionResult _ProgramListing(int? id, string id2)
        {


            var programlist = (from x in db.Programs
                            join z in db.RoleProgramUserProfiles
                            on x.ProgramID equals z.ProgramID
                            join u in db.AspNetUsers
                            on z.Id equals u.Id
                            where z.Id == id2 && z.RoleID == id
                            select new ProgramViewModel()
                            {
                                ProgramID = x.ProgramID,
                                ProgramName = x.ProgramName,
                                Active = x.Active

                            }).Distinct().ToList();



            return PartialView(programlist);
        }


        public ActionResult _ProgramRoleList(string id)
        {

            ViewBag.Id = id;
            var roles = new List<ProgramViewModel>();
            
            var programlist = (from x in db.Programs
                               join z in db.RoleProgramUserProfiles
                               on x.ProgramID equals z.ProgramID
                               join u in db.AspNetUsers
                               on z.Id equals u.Id
                               join k in db.RoleBins
                               on z.RoleID equals k.RoleBinID
                               where z.Id == id
                               select new ProgramViewModel()
                               {
                                   Id = id,
                                   ProgramID = x.ProgramID,
                                   RoleBinID = z.RoleID,
                                   RoleBinName = k.RoleBinName,
                                   ProgramName = x.ProgramName,
                                   ProgramCode = x.ProgramCode,
                                   ProgramEmail = x.ProgramEmail,
                                   Active = x.Active

                               }).Distinct().ToList();

            foreach (ProgramViewModel up in programlist)
            {


                bool alreadyExists = roles.Any(x => x.RoleBinID == up.RoleBinID);

                if (alreadyExists == false)
                {

                    roles.Add(up);

                }

            }


            return PartialView(roles);
        }





        public ActionResult _ProgramList(string id)
        {

            ViewBag.Id = id;

            var programlist = (from x in db.Programs
                         join z in db.SiteRoleProgramUserProfiles
                         on x.ProgramID equals z.ProgramID
                         join u in db.AspNetUsers
                         on z.Id equals u.Id
                         join k in db.RoleBins
                         on z.RoleID equals k.RoleBinID
                         where z.Id == id 
                         select new ProgramViewModel()
                         {
                             Id = id,
                             ProgramID = x.ProgramID,
                             RoleBinID = z.RoleID,
                             RoleBinName = k.RoleBinName,
                             ProgramName = x.ProgramName,
                             ProgramCode = x.ProgramCode,
                             ProgramEmail = x.ProgramEmail,
                             Active = x.Active

                         }).Distinct().ToList();
            
            
            return PartialView(programlist);
        }



        // GET: Programs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = db.Programs.Find(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // GET: Programs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Programs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProgramID,ProgramName,ProgramCode,ProgramEmail,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Programs.Add(program);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(program);
        }

        // GET: Programs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = db.Programs.Find(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: Programs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProgramID,ProgramName,ProgramCode,ProgramEmail,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Program program)
        {
            if (ModelState.IsValid)
            {
                db.Entry(program).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(program);
        }

        // GET: Programs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Program program = db.Programs.Find(id);
            if (program == null)
            {
                return HttpNotFound();
            }
            return View(program);
        }

        // POST: Programs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Program program = db.Programs.Find(id);
            db.Programs.Remove(program);
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
