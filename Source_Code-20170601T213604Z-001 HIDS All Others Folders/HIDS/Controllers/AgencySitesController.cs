using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Collections;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{
    public class AgencySitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HIDSEntities context = new HIDSEntities();
        private eCaSTBillingEntities edb = new eCaSTBillingEntities();
        // GET: AgencySites
        public ActionResult Index()
        {


            string UserName = @User.Identity.Name.ToString();

            var U = (from P in db.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();


            ViewBag.UserID = U.Id;
            int? roleid = 0;

            if (U.RoleBinID == 1 || U.RoleBinID == 13 || U.RoleBinID == 3)
            {

                ViewBag.RoleID = U.RoleBinID;
                roleid = 1;

            }
            else if (U.RoleBinID == 2)
            {


                // Find out if User has any supplimental Roles

                var rolelist = (from x in db.AspNetUsers
                                join y in db.SiteRoleProgramUserProfiles
                                on x.Id equals y.Id
                                where x.UserName == UserName
                                select y.RoleID).ToList();

                foreach (int r in rolelist)
                {

                    // Site Administrator
                    if (r == 6)
                    {


                        ViewBag.RoleID = 6;
                        roleid = 6;

                    }


                }


                }
            else
            {

                // Find out if User has any supplimental Roles

                var rolelist = (from x in db.AspNetUsers
                                join y in db.RoleProgramUserProfiles
                                on x.Id equals y.Id
                                where x.UserName == UserName
                                select y.RoleID).ToList();

                foreach (int r in rolelist)
                {

                   
                    // Data Manager
                    if (r == 13)
                    {

                        ViewBag.RoleID = 13;
                        roleid = 13;

                    }

                    // Program Manager
                    if (r == 3)
                    {

                        ViewBag.RoleID = 3;
                        roleid = 3;


                    }



                }


            }


            if (roleid == 1)
            {



                var results = (from c in db.AgencySites
                               //where c.Active == true
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active,
                                   RoleAList = (from anu in db.AgencySiteProgramSites
                                                join pr in db.Programs
                                                on anu.ProgramID equals pr.ProgramID
                                                where anu.AgencySiteID == c.AgencySiteID
                                                select pr.ProgramName).Distinct().ToList()


                               }).AsQueryable().OrderBy(c => c.AgencySiteName);



                return PartialView(results);


            }
            else if (roleid == 13)
            {

                var results = (from c in db.AgencySites
                               join x in db.AgencySiteProgramSites
                               on c.AgencySiteID equals x.AgencySiteID
                               join y in db.Programs
                               on x.ProgramID equals y.ProgramID
                               join k in db.RoleProgramUserProfiles
                               on y.ProgramID equals k.ProgramID
                               where k.Id == U.Id
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active,
                                   RoleAList = (from anu in db.AgencySiteProgramSites
                                                join pr in db.Programs
                                                on anu.ProgramID equals pr.ProgramID
                                                where anu.AgencySiteID == c.AgencySiteID
                                                select pr.ProgramName).Distinct().ToList()


                               }).AsQueryable().OrderBy(c => c.AgencySiteName);



                return PartialView(results);



            }
            else if (roleid == 3)
            {

                var results = (from c in db.AgencySites
                               join x in db.AgencySiteProgramSites
                               on c.AgencySiteID equals x.AgencySiteID
                               join y in db.Programs
                               on x.ProgramID equals y.ProgramID
                               join k in db.RoleProgramUserProfiles
                               on y.ProgramID equals k.ProgramID
                               where k.Id == U.Id
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active,
                                   RoleAList = (from anu in db.AgencySiteProgramSites
                                                join pr in db.Programs
                                                on anu.ProgramID equals pr.ProgramID
                                                where anu.AgencySiteID == c.AgencySiteID
                                                select pr.ProgramName).Distinct().ToList()


                               }).AsQueryable().OrderBy(c => c.AgencySiteName);



                return PartialView(results);



            }
            else if (roleid == 6)
            {

                var results = (from c in db.AgencySites
                               join x in db.AgencySiteProgramSites
                               on c.AgencySiteID equals x.AgencySiteID
                               join y in db.Programs
                               on x.ProgramID equals y.ProgramID
                               join k in db.SiteRoleProgramUserProfiles
                               on y.ProgramID equals k.ProgramID
                               where k.Id == U.Id && k.SiteID == x.SiteID
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active


                               }).Distinct().AsQueryable().OrderBy(c => c.AgencySiteName);



                return PartialView(results);



            }

            return PartialView();


        }


        // GET: AgencySites/Details/5
        public ActionResult _AgencyInfo(int? id)
        {

            ViewBag.AgencySiteID = id;
            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();

            if (id == null)
            {

                IEnumerable Option =

                 db.Programs.AsEnumerable().Select(c => new SelectListItem()


                 {

                     Text = c.ProgramName,
                     Value = c.ProgramID.ToString(),
                     Selected = true


                 });


                var Newterms = new SelectList(Option, "Value", "Text");
                ViewBag.OptionList = Newterms;

                return PartialView();

            }
            else
            {

                var results = (from c in db.AgencySites
                               where c.AgencySiteID == id
                               select new AgencySiteViewModel()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active,
                                   Address = c.Address,
                                   SecondaryAddress = c.SecondaryAddress,
                                   CityBinID = c.CityBinID,
                                   StateBinID = c.StateBinID,
                                   CountyCodeBinID = c.CountyCodeBinID,
                                   ZipCode = c.ZipCode,
                                   Phone = c.Phone,
                                   Fax = c.Fax,
                                   FEIN = c.FEIN,
                                   MailAddress = c.MailAddress,
                                   SecondaryMailAddress = c.SecondaryMailAddress,
                                   MailCityBinID = c.MailCityBinID,
                                   MailStateBinID = c.MailStateBinID,
                                   MailZipCode = c.MailZipCode,
                                   Website = c.Website

                               }).SingleOrDefault();

                return PartialView(results);

            }


            return PartialView();
        }



        // GET: AgencySites/Details/5
        public ActionResult Details(int? id)
        {


            string UserName = @User.Identity.Name.ToString();

            var U = (from P in db.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();

            ViewBag.RoleID = U.RoleBinID;
            ViewBag.AgencySiteID = id;
            var results = (from c in db.AgencySites
                           where c.AgencySiteID == id
                           select new AgencySiteViewModel()
                           {

                               AgencySiteID = c.AgencySiteID,
                               AgencySiteName = c.AgencySiteName,
                               Active = c.Active,
                               Address = c.Address,
                               SecondaryAddress = c.SecondaryAddress,
                               CityBinID = c.CityBinID,
                               StateBinID = c.StateBinID,
                               ZipCode = c.ZipCode,
                               Phone = c.Phone,
                               Fax = c.Fax,
                               FEIN = c.FEIN,
                               MailAddress = c.MailAddress,
                               SecondaryMailAddress = c.SecondaryMailAddress,
                               MailCityBinID = c.MailCityBinID,
                               MailStateBinID = c.MailStateBinID,
                               MailZipCode = c.MailZipCode,
                               Website = c.Website

                           }).SingleOrDefault();


            return PartialView(results);
        }

        // GET: AgencySites/Create
        public ActionResult Create()
        {

            return PartialView();
        }

       
        public JsonResult _CreateAgencyF(AgencySiteViewModel agencysite)
        {

            // User Info
            string UserName = @User.Identity.Name.ToString();
            DateTime Created = DateTime.Now;


            if (ModelState.IsValid)
            {


                // Get Current Year
                DateTime dtC = DateTime.Now;
                // Get Current Year Name
                string dtCY = dtC.Year.ToString();

                // Get Current Month Name
                string dtCM = dtC.Month.ToString();

                // Get Last Year
                //DateTime dtP = DateTime.Now;
                DateTime dtP = dtC;
                dtP = dtP.AddYears(-1);
                // Get Last Year Name
                string dtPY = dtP.Year.ToString();


                // Get Last Month Name
                DateTime dtPMM = dtC.AddMonths(-1);
                string dtPM = dtPMM.Month.ToString();


                // Get Next Year
                DateTime dtF = dtC;
                dtF = dtF.AddYears(1);
                // Get Next Year Name
                string dF = dtF.Year.ToString();

                // Get Next Month Name
                DateTime dtPNM = dtC.AddMonths(1);
                string dtNM = dtPNM.Month.ToString();
                int? cy = 0;
                // Set Fiscal Year Range

                DateTime ytdsCHECK = new DateTime(Convert.ToInt32(dtPY), 7, 01);
                DateTime ytdeCHECK = new DateTime(Convert.ToInt32(dtCY), 6, 30, 23, 59, 59);

                DateTime ytds = new DateTime();
                DateTime ytde = new DateTime();

                if (dtC > ytdeCHECK)
                {

                    ytds = new DateTime(Convert.ToInt32(dtCY), 7, 01);
                    ytde = new DateTime(Convert.ToInt32(dF), 6, 30);
                    cy = Convert.ToInt32(dF);

                }
                else
                {

                    ytds = new DateTime(Convert.ToInt32(dtPY), 7, 01);
                    ytde = new DateTime(Convert.ToInt32(dtCY), 6, 30);
                    cy = Convert.ToInt32(dtCY);

                }

                AgencySite AS = new AgencySite
                {

                    AgencySiteName = agencysite.AgencySiteName,
                    Address = agencysite.Address,
                    SecondaryAddress = agencysite.SecondaryAddress,
                    CityBinID = agencysite.CityBinID,
                    StateBinID = agencysite.StateBinID,
                    CountyCodeBinID = agencysite.CountyCodeBinID,
                    ZipCode = agencysite.ZipCode,
                    Phone = agencysite.Phone,
                    Fax = agencysite.Fax,
                    FEIN = agencysite.FEIN,
                    MailAddress = agencysite.MailAddress,
                    SecondaryMailAddress = agencysite.SecondaryMailAddress,
                    MailCityBinID = agencysite.MailCityBinID,
                    MailStateBinID = agencysite.MailStateBinID,
                    MailZipCode = agencysite.MailZipCode,
                    Website = agencysite.Website,
                    Active = true,
                    DateCreated = Created,
                    DateUpdated = Created,
                    UpdatedBy = UserName,
                    CreatedBy = UserName


                };

                db.AgencySites.Add(AS);
                db.SaveChanges();

                int agencysiteid = AS.AgencySiteID;


                Site ST = new Site
                {

                    SiteName = agencysite.AgencySiteName,
                    Address = agencysite.Address,
                    SecondaryAddress = agencysite.SecondaryAddress,
                    CityBinID = agencysite.CityBinID,
                    StateBinID = agencysite.StateBinID,
                    CountyCodeBinID = agencysite.CountyCodeBinID,
                    ZipCode = agencysite.ZipCode,
                    Phone = agencysite.Phone,
                    Fax = agencysite.Fax,
                    Website = agencysite.Website,
                    Active = true,
                    DateCreated = Created,
                    DateUpdated = Created,
                    UpdatedBy = UserName,
                    CreatedBy = UserName


                };

                db.Sites.Add(ST);
                db.SaveChanges();

                int siteid = ST.SiteID;


                if (agencysite.ProgramName != null)
                {

                   
                    var stringToSplitS = agencysite.ProgramName;

                    var queryS = from valS in stringToSplitS.Split(',')
                                 select Convert.ToInt32(valS);
                    foreach (int valS in queryS)
                    {

                        AgencySiteProgramSites ASPS = new AgencySiteProgramSites
                        {

                            ProgramID = valS,
                            SiteID = siteid,
                            AgencySiteID = agencysiteid


                        };


                        db.AgencySiteProgramSites.Add(ASPS);
                        db.SaveChanges();

                        if (valS == 20)
                        {
                            Contract Con = new Contract
                            {

                                AgencySiteID = agencysiteid,
                                FiscalYear = Convert.ToInt32(cy),
                                TypeID = 1,
                                DateCreated = Created,
                                DateUpdated = Created,
                                UpdateBy = UserName,
                                CreatedBy = UserName


                            };

                            edb.Contracts.Add(Con);
                            edb.SaveChanges();


                        }

                    }
                }

              
             
                return Json(new { Status = "Success", Modified = agencysiteid }, JsonRequestBehavior.AllowGet);



            }

            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult _UpdateAgencyF(AgencySiteViewModel agencysite)
        {

            // User Info
            string UserName = @User.Identity.Name.ToString();
            DateTime Created = DateTime.Now;


            if (ModelState.IsValid)
            {


                AgencySite agsite = db.AgencySites.Single(x => x.AgencySiteID == agencysite.AgencySiteID);
                agsite.AgencySiteName = agencysite.AgencySiteName;
                agsite.Active = agencysite.Active;
                agsite.Address = agencysite.Address;
                agsite.SecondaryAddress = agencysite.SecondaryAddress;
                agsite.CityBinID = agencysite.CityBinID;
                agsite.StateBinID = agencysite.StateBinID;
                agsite.CountyCodeBinID = agencysite.CountyCodeBinID;
                agsite.ZipCode = agencysite.ZipCode;
                agsite.Phone = agencysite.Phone;
                agsite.Fax = agencysite.Fax;
                agsite.FEIN = agencysite.FEIN;
                agsite.MailAddress = agencysite.MailAddress;
                agsite.SecondaryMailAddress = agencysite.SecondaryMailAddress;
                agsite.MailCityBinID = agencysite.MailCityBinID;
                agsite.MailStateBinID = agencysite.MailStateBinID;
                agsite.MailZipCode = agencysite.MailZipCode;
                agsite.Website = agencysite.Website;
                agsite.DateUpdated = Created;
                agsite.UpdatedBy = UserName;
                db.SaveChanges();




                return Json(new { Status = "Success", Modified = agencysite.AgencySiteID }, JsonRequestBehavior.AllowGet);



            }

            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

        }


        private void PopulateInitialCityData()
        {

            var allCities = context.DDCityBins.OrderBy(x => x.CityBinName);
            var viewModel = new List<AssignedCityData>();
            foreach (var city in allCities)
            {

                viewModel.Add(new AssignedCityData
                {
                    CityBinID = city.CityBinID,
                    CityBinName = city.CityBinName,

                });

            }

            ViewBag.PossibleCityBins = viewModel;
        }




        private void PopulateInitialStateData()
        {

            var allStates = context.DDStateBins.OrderBy(x => x.StateBinName);
            var viewModel = new List<AssignedStateData>();
            foreach (var state in allStates)
            {

                viewModel.Add(new AssignedStateData
                {
                    StateBinID = state.StateBinID,
                    StateBinName = state.StateBinName,

                });

            }

            ViewBag.PossibleStateBins = viewModel;
        }



        private void PopulateInitialCountyData()
        {

            var allCounties = context.DDCountyBins.OrderBy(x => x.CountyBinName);
            var viewModel = new List<AssignedCountyData>();
            foreach (var county in allCounties)
            {

                viewModel.Add(new AssignedCountyData
                {
                    CountyBinID = county.CountyBinID,
                    CountyBinName = county.CountyBinName,

                });

            }

            ViewBag.PossibleCountyBins = viewModel;
        }

        // POST: AgencySites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgencySiteID,AgencySiteName,Address,SecondaryAddress,CityBinID,StateBinID,ZipCodeBinID,Phone,Fax,Email,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] AgencySite agencySite)
        {
            if (ModelState.IsValid)
            {
                db.AgencySites.Add(agencySite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agencySite);
        }

        // GET: AgencySites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencySite agencySite = db.AgencySites.Find(id);

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in db.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();

            ViewBag.RoleID = U.RoleBinID;

            // Check if Contract exists

            var concount = edb.Contracts.Where(x => x.AgencySiteID == id).Count();

            ViewBag.ConCount = concount;


            if (agencySite == null)
            {
                return HttpNotFound();
            }
            return PartialView(agencySite);
        }

        // POST: AgencySites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgencySiteID,AgencySiteName,Address,SecondaryAddress,CityBinID,StateBinID,ZipCodeBinID,Phone,Fax,Email,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] AgencySite agencySite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agencySite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agencySite);
        }

        // GET: AgencySites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencySite agencySite = db.AgencySites.Find(id);
            if (agencySite == null)
            {
                return HttpNotFound();
            }
            return View(agencySite);
        }

        // POST: AgencySites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgencySite agencySite = db.AgencySites.Find(id);
            db.AgencySites.Remove(agencySite);
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
