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
    public class RoleBinsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RoleBins
        public ActionResult Index()
        {
            return View(db.RoleBins.ToList());
        }


        public ActionResult _AgencyRoleList(int? id, int? id2)
        {

            // Get UserID
            string UserName = @User.Identity.Name.ToString();

            var userid = (from x in db.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();


            ViewBag.RoleBinID = userid.RoleBinID;

      

            ViewBag.RandomValue = id + id2;

            List<SelectListItem> items = new List<SelectListItem>();
            List<AgencyRoleViewModel> AgencyRoleItems = new List<AgencyRoleViewModel>();
           
            //var AgencyRoleType = (from P in db.AgencyContactClinics
            //                      //join X in db.RoleBins
            //                      //on P.AgencyRoleBinID equals X.LegacyAgencyRoleID
            //                      join X in db.RoleBins
            //                      on P.AgencyRoleBinID equals X.LegacyAgencyRoleID
            //                      into XX
            //                      from XXX in XX.DefaultIfEmpty()
            //                      where P.ClinicID == id && P.ProgramBinID == id2 
            //                      select P.AgencyRoleBinID).Distinct().ToList();

            var AgencyRoleType = (from x in db.RoleBins
                            where x.IsPortalUser == false
                            select x.RoleBinID).ToList();

            foreach (int ar in AgencyRoleType)
            {

                var AR = (from P in db.RoleBins
                          where P.RoleBinID == ar 
                          select new
                          {
                              AgencyRoleBinID = P.RoleBinID,
                              AgencyRoleBinName = P.RoleBinName

                          }).Distinct().SingleOrDefault();

                int ARoleBinID = Convert.ToInt32(AR.AgencyRoleBinID);
                string ARoleName = AR.AgencyRoleBinName;
                int AgencyRoleBinID = ARoleBinID;
              //  int AgencyRoleBinID = AR.FirstOrDefault().AgencyRoleBinID;
                string ARID = AgencyRoleBinID.ToString();
             //   string AgencyRoleBinName = AR.FirstOrDefault().AgencyRoleBinName;
                string AgencyRoleBinName = ARoleName;

                bool alreadyExists = items.Any(x => x.Value == ARID);

                if (alreadyExists == false)
                {

                    items.Add(new SelectListItem { Text = AgencyRoleBinName, Value = ARID, Selected = false });


                }



               
            }

            foreach (SelectListItem num in items)
            {

                int ARID = Convert.ToInt32(num.Value);

                var agencyroletypes = (from c in db.RoleBins
                                       where c.RoleBinID == ARID 
                                       select new AgencyRoleViewModel()
                                       {

                                           AgencyRoleBinID = c.RoleBinID,
                                           AgencyRoleBinName = c.RoleBinName


                                       }).Distinct().SingleOrDefault();



                if (agencyroletypes != null)
                {

                    int ARoleBinID = Convert.ToInt32(agencyroletypes.AgencyRoleBinID);
                    string ARoleName = agencyroletypes.AgencyRoleBinName;

                  //  int? AgencyRoleBinID = agencyroletypes.FirstOrDefault().AgencyRoleBinID;
                   int AgencyRoleBinID = ARoleBinID;
                 //  string AgencyRoleBinName = agencyroletypes.FirstOrDefault().AgencyRoleBinName;
                   string AgencyRoleBinName = ARoleName;

                    bool alreadyExists = AgencyRoleItems.Any(x => x.AgencyRoleBinID == AgencyRoleBinID);

                    if (alreadyExists == false)
                    {

                        AgencyRoleItems.Add(new AgencyRoleViewModel() { AgencyRoleBinID = AgencyRoleBinID, AgencyRoleBinName = AgencyRoleBinName, SiteID = id, ProgramID = id2 });

                    }
                }



            }



            return PartialView(AgencyRoleItems);

        }



        // GET: RoleBins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleBin roleBin = db.RoleBins.Find(id);
            if (roleBin == null)
            {
                return HttpNotFound();
            }
            return View(roleBin);
        }

        // GET: RoleBins/Create
        public ActionResult Create()
        {



            return PartialView();
        }

        // POST: RoleBins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoleBinID,RoleBinName,Active,LegacyRoleID,LegacyRoleName,IsPortalUser,AgencyTypeRoleBinID")] RoleBin roleBin)
        {
            if (ModelState.IsValid)
            {
                db.RoleBins.Add(roleBin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roleBin);
        }

        // GET: RoleBins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleBin roleBin = db.RoleBins.Find(id);
            if (roleBin == null)
            {
                return HttpNotFound();
            }
            return View(roleBin);
        }

        // POST: RoleBins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoleBinID,RoleBinName,Active,LegacyRoleID,LegacyRoleName,IsPortalUser,AgencyTypeRoleBinID")] RoleBin roleBin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roleBin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roleBin);
        }

        // GET: RoleBins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleBin roleBin = db.RoleBins.Find(id);
            if (roleBin == null)
            {
                return HttpNotFound();
            }
            return View(roleBin);
        }

        // POST: RoleBins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleBin roleBin = db.RoleBins.Find(id);
            db.RoleBins.Remove(roleBin);
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
