using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{
    public class SitesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private eCaSTContextEntities edb = new eCaSTContextEntities();
        private eCaSTBillingEntities bdb = new eCaSTBillingEntities();
        private HIDSEntities context = new HIDSEntities(); 
        // GET: Sites
        public ActionResult Index(int? id)
        {


            ViewBag.AgencySiteID = id;

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in db.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();

            ViewBag.RoleID = U.RoleBinID;



            var sites0 = new List<SiteViewModel>();
            var sites = (from x in db.Sites
                            join z in db.AgencySiteProgramSites
                            on x.SiteID equals z.SiteID
                            where z.AgencySiteID == id
                            select new SiteViewModel()
                            {
                                SiteID = x.SiteID,
                                SiteName = x.SiteName,
                                Active = x.Active,
                                RoleAList = (from anu in db.AgencySiteProgramSites
                                             join pr in db.Programs
                                             on anu.ProgramID equals pr.ProgramID
                                             where anu.SiteID == x.SiteID
                                             select pr.ProgramName).Distinct().ToList()

                            }).ToList();

            foreach (SiteViewModel up in sites)
            {


                bool alreadyExists = sites0.Any(x => x.SiteID == up.SiteID);

                if (alreadyExists == false)
                {

                    sites0.Add(up);

                }

            }



            return PartialView(sites0);
        }


        // GET: Sites
        public ActionResult _SiteListing(int? id, string id2)
        {


            var sitelist = (from x in db.Sites
                               join z in db.SiteRoleProgramUserProfiles
                               on x.SiteID equals z.SiteID
                               join u in db.AspNetUsers
                               on z.Id equals u.Id
                               where z.Id == id2 && z.ProgramID == id
                               select new SiteViewModel()
                               {
                                   SiteID = x.SiteID,
                                   SiteName = x.SiteName,
                                   Active = x.Active

                               }).Distinct().ToList();



            return PartialView(sitelist);
        }
        
        public ActionResult _Sites( string id)
        {

            //List<SiteListViewModel> slvm = new List<SiteListViewModel>();
            List<SelectListItem> slvm = new List<SelectListItem>();
            var results = new List<SiteListViewModel>();

            if (id != null)
            {

                // Programs
                var stringToSplitS = id;

                var queryS = from valS in stringToSplitS.Split(',')
                             select Convert.ToInt32(valS);



             
                foreach (int valS in queryS)
                {


                    // ProgramName
                    var programname = (from x in db.Programs
                                  where x.ProgramID == valS
                                  select x.ProgramName).FirstOrDefault();


                  
                    var sitelists = (from x in db.Programs
                                     where x.ProgramID == valS
                                     select new SiteListViewModel()
                                     {

                                         ProgramName = programname,
                                         ProgramID = valS



                                     }).Distinct().ToList();

                    foreach (SiteListViewModel slv in sitelists)
                    {

                        results.Add(slv);

                    }


                }



            }


            return PartialView(results);
        }

        public ActionResult _AddSite(int? id)
        {


            ViewBag.AgencySiteID = id;


            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();


            ViewBag.PossibleAgencySites = db.AgencySites.Where(x => x.Active == true);

            IEnumerable Option =

               db.Programs.AsEnumerable().Select(c => new SelectListItem()


               {

                   Text = c.ProgramName,
                   Value = c.ProgramID.ToString(),
                   Selected = true


               });


            var Newterms = new SelectList(Option, "Value", "Text");
            ViewBag.OptionList = Newterms;


          //  IEnumerable OptionSER =

          //context.Services.AsEnumerable().Select(c => new SelectListItem()


          //{

          //    Text = c.ServiceName,
          //    Value = c.ServiceID.ToString(),
          //    Selected = true


          //});


          //  var NewtermsSER = new SelectList(OptionSER, "Value", "Text");
          //  ViewBag.OptionListSER = NewtermsSER;


            return PartialView();
        } 

        public ActionResult _EmailBuilder()
        {

            // Agencies
            //IEnumerable OptionA = db.AgencySites.Where(c => c.Active == true).AsEnumerable().Select(c => new SelectListItem()

            //{

            //    Text = c.AgencySiteName,
            //    Value = c.AgencySiteID.ToString(),
            //    Selected = true,
            //});


            //var NewtermsA = new SelectList(OptionA, "Value", "Text");
            //ViewBag.OptionListA = NewtermsA;
             string UserName = @User.Identity.Name.ToString();
            var userid = (from x in db.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();


            if (userid.RoleBinID == 1 || userid.RoleBinID == 13 || userid.RoleBinID == 3)
            {

                // Contracts
                IEnumerable OptionCON = bdb.DD_ContractTypes.Where(c => c.Active == true && c.ProgramBinID == 20).AsEnumerable().Select(c => new SelectListItem()
               {
                   Text = c.Description,
                   Value = c.ID.ToString(),
                   Selected = true,
               });


                var NewtermsCON = new SelectList(OptionCON, "Value", "Text");
                ViewBag.OptionListCON = NewtermsCON;

                var possibleprograms = (from x in db.Programs
                                        join y in db.RoleProgramUserProfiles
                                        on x.ProgramID equals y.ProgramID
                                       where y.Id == userid.Id
                                       select new ProgramViewModel{
                                       
                                       ProgramID = x.ProgramID,
                                       ProgramName = x.ProgramName
                                       
                                       
                                       }).ToList();

                ViewBag.PossibleProgramIDs = possibleprograms;




            }
            else
            {

                // Contracts
                IEnumerable OptionCON = bdb.DD_ContractTypes.Where(c => c.Active == true && c.ProgramBinID == 20).AsEnumerable().Select(c => new SelectListItem()
                {
                    Text = c.Description,
                    Value = c.ID.ToString(),
                    Selected = true,
                });


                var NewtermsCON = new SelectList(OptionCON, "Value", "Text");
                ViewBag.OptionListCON = NewtermsCON;

                var possibleprograms = (from x in db.Programs
                                        join y in db.SiteRoleProgramUserProfiles
                                        on x.ProgramID equals y.ProgramID
                                        where y.Id == userid.Id
                                        select new ProgramViewModel
                                        {

                                            ProgramID = x.ProgramID,
                                            ProgramName = x.ProgramName


                                        }).ToList();

                ViewBag.PossibleProgramIDs = possibleprograms;


            }

          

            return PartialView();

        }

        [HttpGet]
        public virtual ActionResult GetAgencySites(string Contracts)
        {

            if (Contracts != "")
            {

                // Contracts
                List<AgencySite> items = new List<AgencySite>();
                var stringToSplit = Contracts;
                var query = from val in stringToSplit.Split(',')
                            select Convert.ToInt32(val);

                var agencies = db.AgencySites.ToList();
                var contracts = bdb.Contracts.ToList();


                foreach (int val in query)
                {

                    var curdate = DateTime.Now;
                    var agencysitelist =
                        (
                            from ag in agencies
                            join con in contracts
                            on ag.AgencySiteID equals con.AgencySiteID
                            where con.TypeID == val && ag.Active == true && curdate <= con.DateFYEnd
                            select con.AgencySiteID).Distinct().ToList();


                    foreach (int c in agencysitelist)
                    {

                        var agencysites =
                      (
                          from agency in db.AgencySites
                          where agency.AgencySiteID == c
                          select new
                          {
                              Id = agency.AgencySiteID,
                              Name = agency.AgencySiteName
                              
                          }
                      ).Distinct();


                        if (agencysites.Count() > 0)
                        {
                            int AgencySiteID = agencysites.FirstOrDefault().Id;

                            string AgencySiteName = agencysites.FirstOrDefault().Name;

                            bool alreadyExists = items.Any(x => x.AgencySiteID == AgencySiteID);

                            if (alreadyExists == false)
                            {

                                items.Add(new AgencySite() { AgencySiteID = AgencySiteID, AgencySiteName = AgencySiteName });

                            }
                        }


                    }


                }


                return Json(items, JsonRequestBehavior.AllowGet);



            }

            return Json(null, JsonRequestBehavior.AllowGet);


        }

        [HttpGet]
        public virtual ActionResult GetClinics(string AgencySites)
        {

            if (AgencySites != "")
            {

                // Providers
                List<Site> items = new List<Site>();
                var stringToSplit = AgencySites;
                var query = from val in stringToSplit.Split(',')
                            select Convert.ToInt32(val);
                foreach (int val in query)
                {


                    var cliniclist =
                        (
                            from clinic in db.Sites
                            join cas in db.AgencySiteProgramSites
                            on clinic.SiteID equals cas.SiteID
                            where cas.AgencySiteID == val && clinic.Active == true
                            select cas.SiteID).Distinct().ToList();


                    foreach (int c in cliniclist)
                    {


                        var clinics =
                      (
                          from clinic in db.Sites
                          join cas in db.AgencySiteProgramSites
                          on clinic.SiteID equals cas.SiteID
                          where cas.SiteID == c
                          select new
                          {
                              Id = clinic.SiteID,
                              Name = clinic.SiteName,
                              AgencySiteID = cas.AgencySiteID
                          }
                      ).Distinct();


                        if (clinics.Count() > 0)
                        {
                            int SiteID = clinics.FirstOrDefault().Id;

                            string SiteName = clinics.FirstOrDefault().Name;


                            bool alreadyExists = items.Any(x => x.SiteID == SiteID);

                            if (alreadyExists == false)
                            {

                                items.Add(new Site() { SiteID = SiteID, SiteName = SiteName });

                            }
                        }


                    }


                }


                return Json(items, JsonRequestBehavior.AllowGet);



            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public virtual ActionResult GetPrograms(string Clinics)
        {

            if (Clinics != "")
            {


                List<Program> items = new List<Program>();
                var stringToSplit = Clinics;
                var query = from val in stringToSplit.Split(',')
                            select Convert.ToInt32(val);
                foreach (int val in query)
                {


                    var programlist =
                        (
                            from clinic in db.Sites
                            join cas in db.AgencySiteProgramSites
                            on clinic.SiteID equals cas.SiteID
                            where cas.SiteID == val && clinic.Active == true
                            select cas.ProgramID).Distinct().ToList();


                    foreach (int p in programlist)
                    {


                        var programs =
                      (
                          from program in db.Programs
                          where program.ProgramID == p
                          select new
                          {
                              Id = program.ProgramID,
                              Name = program.ProgramName

                          }
                      ).Distinct();


                        if (programs.Count() > 0)
                        {
                            int ProgramID = programs.FirstOrDefault().Id;

                            string ProgramName = programs.FirstOrDefault().Name;


                            bool alreadyExists = items.Any(x => x.ProgramID == ProgramID);

                            if (alreadyExists == false)
                            {

                                items.Add(new Program() { ProgramID = ProgramID, ProgramName = ProgramName });

                            }
                        }


                    }


                }


                return Json(items, JsonRequestBehavior.AllowGet);



            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public virtual ActionResult GetRoles(string Programs, string Clinics)
        {

            if (Programs != "" && Clinics != "")
            {

                List<Site> items = new List<Site>();
                List<RoleBin> roles = new List<RoleBin>();

                var stringToSplit = Clinics;
                var query = from val in stringToSplit.Split(',')
                            select Convert.ToInt32(val);


                foreach (int val in query)
                {

                    var clinics =
                     (
                         from clinic in db.Sites
                         where clinic.SiteID == val && clinic.Active == true
                         select new
                         {
                             Id = clinic.SiteID,
                             Name = clinic.SiteName

                         }
                     ).Distinct();


                    if (clinics.Count() > 0)
                    {
                        int SiteID = clinics.FirstOrDefault().Id;

                        string SiteName = clinics.FirstOrDefault().Name;


                        bool alreadyExists = items.Any(x => x.SiteID == SiteID);

                        if (alreadyExists == false)
                        {

                            items.Add(new Site() { SiteID = SiteID, SiteName = SiteName });

                        }
                    }



                }


                // Loop through Clinic List

                foreach (Site clin in items)
                {

                    // Program loop
                    var stringToSplitP = Programs;
                    var queryP = from valP in stringToSplitP.Split(',')
                                 //select Convert.ToInt32(valP);
                                 select valP;

                    foreach (string valP in queryP)
                    {


                        // Make a List of Roles for this Program Per Site

                        var rolerlist =
                        (
                            from role in db.RoleBins
                            join dd in db.AgencyContactClinics
                            on role.RoleBinID equals dd.AgencyRoleBinID
                            join pp in db.Programs
                            on dd.ProgramBinID equals pp.ProgramID
                            where dd.ClinicID == clin.SiteID && pp.ProgramName == valP
                            select role.RoleBinID).Distinct().ToList();


                        foreach (int ror in rolerlist)
                        {



                            var roler =
                        (
                            from role in db.RoleBins
                            where role.RoleBinID == ror
                            select new
                            {
                                Id = role.RoleBinID,
                                Name = role.RoleBinName

                            }

                        ).Distinct();


                            if (roler.Count() > 0)
                            {
                                int RoleBinID = roler.FirstOrDefault().Id;

                                string RoleBinName = roler.FirstOrDefault().Name;


                                bool alreadyExists = roles.Any(x => x.RoleBinID == RoleBinID);

                                if (alreadyExists == false)
                                {

                                    roles.Add(new RoleBin() { RoleBinID = RoleBinID, RoleBinName = RoleBinName });

                                }
                            }



                        }




                    }


                }

                return Json(roles, JsonRequestBehavior.AllowGet);

            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        //[HttpGet]
        public virtual ActionResult GetContacts(string Programs, string Clinics, string Roles)
        {

            if (Programs != "" && Clinics != "" && Roles != "")
            {

                List<Site> items = new List<Site>();
                List<RoleBin> roles = new List<RoleBin>();
                List<AgencyContact> contacts = new List<AgencyContact>();

                var stringToSplit = Clinics;
                var query = from val in stringToSplit.Split(',')
                            select Convert.ToInt32(val);


                foreach (int val in query)
                {

                    var clinics =
                     (
                         from clinic in db.Sites
                         where clinic.SiteID == val && clinic.Active == true
                         select new
                         {
                             Id = clinic.SiteID,
                             Name = clinic.SiteName

                         }
                     ).Distinct();


                    if (clinics.Count() > 0)
                    {
                        int ClinicID = clinics.FirstOrDefault().Id;

                        string ClinicName = clinics.FirstOrDefault().Name;


                        bool alreadyExists = items.Any(x => x.SiteID == ClinicID);

                        if (alreadyExists == false)
                        {

                            items.Add(new Site() { SiteID = ClinicID, SiteName = ClinicName });

                        }
                    }



                }


                // Loop through Clinic List

                foreach (Site clin in items)
                {

                    // Program loop
                    var stringToSplitP = Programs;
                    var queryP = from valP in stringToSplitP.Split(',')
                                // select Convert.ToInt32(valP);
                                 select valP;

                    foreach (string valP in queryP)
                    {


                        var programid = (from x in db.Programs
                                              where x.ProgramName == valP
                                              select x.ProgramID).SingleOrDefault();

                        // Role loop
                        var stringToSplitR = Roles;
                        var queryR = from valR in stringToSplitR.Split(',')
                                     select Convert.ToInt32(valR);


                        foreach (int valR in queryR)
                        {


                            if (valR == 2)
                            {

                                // Begin Internal Roles

                                //var contactlist =
                                //(
                                // from contact in db.AspNetUsers
                                // join dd in db.SiteRoleProgramUserProfiles
                                // on contact.Id equals dd.Id
                                // into ff
                                // from rr in ff.DefaultIfEmpty()
                                // where rr.SiteID == clin.SiteID && rr.RoleID == 2 && rr.ProgramID == programid
                                // select contact).Distinct().ToList();

                                var contactlist =
                               (
                                from dd in db.SiteRoleProgramUserProfiles
                                where dd.SiteID == clin.SiteID && dd.RoleID == 2 && dd.ProgramID == programid
                                select dd.Id).Distinct().ToList();


                                foreach (string con in contactlist)
                                {

                                    var contactr =
                                      (
                                      from contact in db.AspNetUsers
                                     where contact.Id == con
                                     select new
                                      {
                                        //Id = contact.AgencyContactID,
                                          Id = contact.Email,
                                          Name = contact.FirstName + " " + contact.LastName

                                                 }).Distinct();


                                    if (contactr.Count() > 0)
                                    {
                                        string AgencyContactID = contactr.FirstOrDefault().Id;

                                        string AgencyContactName = contactr.FirstOrDefault().Name;


                                        bool alreadyExists = contacts.Any(x => x.Email == AgencyContactID);

                                        if (alreadyExists == false)
                                        {

                                            contacts.Add(new AgencyContact() { Email = AgencyContactID, FirstName = AgencyContactName });

                                        }
                                    }



                                }
                            

                            }
                            else
                            {


                                // Begin Internal Roles

                                var contactlist =
                             (
                                 from contact in db.AgencyContacts
                                 join dd in db.AgencyContactClinics
                                 on contact.AgencyContactID equals dd.AgencyContactID
                                 join Z in db.Programs
                                 on dd.ProgramBinID equals Z.ProgramID
                                 where dd.ClinicID == clin.SiteID && Z.ProgramName == valP && dd.AgencyRoleBinID == valR
                                 select contact.AgencyContactID).Distinct().ToList();


                                foreach (int con in contactlist)
                                {



                                    var contactr =
                                (
                                    from contact in db.AgencyContacts
                                    where contact.AgencyContactID == con && contact.Active == true
                                    select new
                                    {
                                        //Id = contact.AgencyContactID,
                                        Id = contact.Email,
                                        Name = contact.FirstName + " " + contact.LastName

                                    }

                                ).Distinct();


                                    if (contactr.Count() > 0)
                                    {
                                        string AgencyContactID = contactr.FirstOrDefault().Id;

                                        string AgencyContactName = contactr.FirstOrDefault().Name;


                                        bool alreadyExists = contacts.Any(x => x.Email == AgencyContactID);

                                        if (alreadyExists == false)
                                        {

                                            contacts.Add(new AgencyContact() { Email = AgencyContactID, FirstName = AgencyContactName });

                                        }
                                    }



                                }



                                // End Internal Roles
                            }

                        }



                    }


                }

                return Json(contacts, JsonRequestBehavior.AllowGet);

            }

            return Json(null, JsonRequestBehavior.AllowGet);

        }

        
        
        public JsonResult _AddSiteDetailF(SiteViewModel siteviewmodel)
        {

            if (ModelState.IsValid)
            {


              
                var sites = (from x in db.Sites
                               select x).ToList();



                foreach (Site s in sites)
                {


                    if (s.SiteName != null)
                    {

                        if (s.SiteName.Trim() == siteviewmodel.SiteName.Trim())
                        {


                            return Json(new { Status = "Duplicate Site", Modified = 0, Modified2 = siteviewmodel.SiteName, Modified3 = "failed" }, JsonRequestBehavior.AllowGet);


                        }


                    }

                }


                // Set Record Info
                string UserNameInit = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

             
                // Create Site in Portal

                Site si = new Site
                {

                    SiteName = siteviewmodel.SiteName,
                    Latitude = siteviewmodel.Latitude,
                    Longitude = siteviewmodel.Longitude,
                    Address = siteviewmodel.Address,
                    SecondaryAddress = siteviewmodel.SecondaryAddress,
                    CityBinID = siteviewmodel.CityBinID,
                    StateBinID = siteviewmodel.StateBinID,
                    CountyCodeBinID = siteviewmodel.CountyCodeBinID,
                    ZipCode = siteviewmodel.ZipCode,
                    Phone = siteviewmodel.Phone,
                    ReferralPhone = siteviewmodel.ReferralPhone,
                    Fax = siteviewmodel.Fax,
                    Website = siteviewmodel.Website,
                    Active = siteviewmodel.Active,
                    DateCreated = CreatedInit,
                    CreatedBy = UserNameInit,
                    DateUpdated = CreatedInit,
                    UpdatedBy = UserNameInit



                };


                try
                {
                    db.Sites.Add(si);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Debug.WriteLine("Property: {0} Error: {1}",
                       validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }


                // Add newly created SiteID to csl
                var SID = si.SiteID;
           

                if (siteviewmodel.ProgramName != null)
                {

                    // Program

                    var stringToSplit = siteviewmodel.ProgramName;

                    var query = from val in stringToSplit.Split(',')
                                select Convert.ToInt32(val);
                    foreach (int val in query)
                    {

                        
                        // Add to AgencySiteProgramSites

                        AgencySiteProgramSites agencysiteprogramsite = new AgencySiteProgramSites
                        {

                            SiteID = SID,
                            AgencySiteID = Convert.ToInt32(siteviewmodel.AgencySiteID),
                            ProgramID = val



                        };

                        db.AgencySiteProgramSites.Add(agencysiteprogramsite);
                        db.SaveChanges();



                        if (val == 20)
                        {


                            // Add Clinic as SubContractor 
                            SubContractor sub = new SubContractor
                            {


                                SubContractorName = siteviewmodel.SiteName,
                                Active = siteviewmodel.Active,
                                Address = siteviewmodel.Address,
                                CityBinID = siteviewmodel.CityBinID,
                                StateBinID = siteviewmodel.StateBinID,
                                CountyBinID = siteviewmodel.CountyCodeBinID,
                                ZipCode = Convert.ToString(siteviewmodel.ZipCode),
                                Phone = siteviewmodel.Phone,
                                DateUpdated = CreatedInit,
                                UpdatedBy = UserNameInit


                            };

                            db.SubContractors.Add(sub);
                            db.SaveChanges();

                            int SUBID = sub.SubContractorID;


                            var cancertypelist = (from x in edb.DDCancerTypeBins
                                                  where x.Active == true
                                                  select x).ToList();



                            //var stringToSplitC = cancertypelist;

                            //var queryC = from valC in stringToSplitC.Split(',')
                            //             select Convert.ToInt32(valC);
                            foreach (DDCancerTypeBin valC in cancertypelist)
                            {


                                // Check for WiWo Contract

                                var agency = (from Cc in db.AgencySiteProgramSites
                                              where Cc.SiteID == SID
                                              select Cc.AgencySiteID).First();

                                var contractcheck = (from Cc in bdb.Contracts
                                                     join Dd in bdb.DD_ContractTypes
                                                     on Cc.TypeID equals Dd.ID
                                                     where Cc.AgencySiteID == agency && Cc.TypeID == 3 && Dd.Active == true
                                                     select Cc.ID).Count();


                                if (contractcheck > 0 && val == 20 && valC.CancerTypeBinID == 4)
                                {


                                    SubContractorClinics subclin = new SubContractorClinics
                                    {


                                        SubContractorID = SUBID,
                                        ClinicID = SID,
                                        SiteID = SID,
                                        ProgramID = val,
                                        CancerTypeBinID = valC.CancerTypeBinID,
                                        IsClinic = true



                                    };

                                    db.SubContractorClinics.Add(subclin);
                                    db.SaveChanges();


                                }
                                else if (contractcheck < 1 && val == 20 && valC.CancerTypeBinID == 4)
                                {


                                    // Do Nothing


                                }
                                else
                                {

                                    SubContractorClinics subclin = new SubContractorClinics
                                    {


                                        SubContractorID = SUBID,
                                        ClinicID = SID,
                                        SiteID = SID,
                                        ProgramID = val,
                                        CancerTypeBinID = valC.CancerTypeBinID,
                                        IsClinic = true



                                    };

                                    db.SubContractorClinics.Add(subclin);
                                    db.SaveChanges();


                                }


                            }


                        }



                    }


                }

               
                // Get Agency
                var agencyid = (from y in db.AgencySiteProgramSites
                                where y.SiteID == SID
                                select y.AgencySiteID).First();


                // See if a clinic already exists
                var sitecount = (from y in db.AgencySiteProgramSites
                                   where y.AgencySiteID == agencyid
                                   select y).Count();


                if (sitecount > 0)
                {

                    // Get Top 1 Clinic ID
                    var clinicid = (from y in db.AgencySiteProgramSites
                                    where y.AgencySiteID == agencyid && y.SiteID != SID
                                    select y.SiteID).Take(1).SingleOrDefault();

                    // Get Existing Contacts from top clinic
                    var userlist = (from y in db.AgencyContactClinics
                                    join s in db.RoleBins
                                    on y.AgencyRoleBinID equals s.RoleBinID
                                    where y.ClinicID == clinicid && s.AgencyTypeRoleBinID == 5
                                    select y).ToList();



                    foreach (AgencyContactClinics acc in userlist)
                    {

                        // Add existing roles to new clinic
                        AgencyContactClinics accl = new AgencyContactClinics
                        {

                            AgencyContactID = acc.AgencyContactID,
                            ClinicID = SID,
                            ProgramBinID = Convert.ToInt32(acc.ProgramBinID),
                            AgencyRoleBinID = Convert.ToInt32(acc.AgencyRoleBinID)


                        };

                        db.AgencyContactClinics.Add(accl);
                        db.SaveChanges();


                    }

                }


              


              
                return Json(new { Status = "Success", Modified = SID, Modified2 = siteviewmodel.SiteName, Modified3 = "passed" }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = siteviewmodel.SiteName, Modified2 = "passed" }, JsonRequestBehavior.AllowGet);


        }


        public JsonResult _UpdateSiteDetailF(SiteViewModel siteviewmodel)
        {

            // User Info
            string UserName = @User.Identity.Name.ToString();
            DateTime Created = DateTime.Now;

            // Get Site Name
            var sitename = (from y in db.Sites
                            where y.SiteID == siteviewmodel.SiteID
                            select y.SiteName).SingleOrDefault();

            if (ModelState.IsValid)
            {

                Site site = db.Sites.Single(x => x.SiteID == siteviewmodel.SiteID);
                site.SiteName = siteviewmodel.SiteName;
                site.Active = siteviewmodel.Active;
                site.Address = siteviewmodel.Address;
                site.Latitude = siteviewmodel.Latitude;
                site.Longitude = siteviewmodel.Longitude;
                site.SecondaryAddress = siteviewmodel.SecondaryAddress;
                site.CityBinID = siteviewmodel.CityBinID;
                site.StateBinID = siteviewmodel.StateBinID;
                site.CountyCodeBinID = siteviewmodel.CountyCodeBinID;
                site.ZipCode = siteviewmodel.ZipCode;
                site.Phone = siteviewmodel.Phone;
                site.ReferralPhone = siteviewmodel.ReferralPhone;
                site.Fax = siteviewmodel.Fax;
                site.Website = siteviewmodel.Website;
                site.DateUpdated = Created;
                site.UpdatedBy = UserName;
                db.SaveChanges();

                // Clear Agency Programs

                var aspslist0 = (from y in db.AgencySiteProgramSites
                                where y.SiteID == siteviewmodel.SiteID
                                select y).ToList();

                foreach (AgencySiteProgramSites asP in aspslist0)
                {


                    db.AgencySiteProgramSites.Remove(asP);
                    db.SaveChanges();


                }

                // Add back

                if (siteviewmodel.ProgramName != null)
                {
                    // Program

                    var stringToSplit = siteviewmodel.ProgramName;

                    var query = from val in stringToSplit.Split(',')
                                select Convert.ToInt32(val);


                    var aspslist = (from y in db.AgencySiteProgramSites
                                    where y.SiteID == siteviewmodel.SiteID
                                    select y.ProgramID).ToList();


                    var results = from i in query
                                  where !aspslist.Contains(i)
                                  select i;

                    foreach (int val in results)
                    //foreach (int val in query)
                    {


                        
                                 // Add to AgencySiteProgramSites
                                 AgencySiteProgramSites agencysiteprogramsite = new AgencySiteProgramSites
                                 {

                                     SiteID = siteviewmodel.SiteID,
                                     AgencySiteID = Convert.ToInt32(siteviewmodel.AgencySiteID),
                                     ProgramID = val


                                 };

                                 db.AgencySiteProgramSites.Add(agencysiteprogramsite);
                                 db.SaveChanges();

                                 if (val == 20)
                                 {


                                     // Add Clinic as SubContractor 
                                     SubContractor sub = new SubContractor
                                     {


                                         SubContractorName = siteviewmodel.SiteName,
                                         Active = siteviewmodel.Active,
                                         Address = siteviewmodel.Address,
                                         CityBinID = siteviewmodel.CityBinID,
                                         StateBinID = siteviewmodel.StateBinID,
                                         CountyBinID = siteviewmodel.CountyCodeBinID,
                                         ZipCode = Convert.ToString(siteviewmodel.ZipCode),
                                         Phone = siteviewmodel.Phone,
                                         DateUpdated = Created,
                                         UpdatedBy = UserName


                                     };

                                     db.SubContractors.Add(sub);
                                     db.SaveChanges();

                                     int SUBID = sub.SubContractorID;


                                     var cancertypelist = (from x in edb.DDCancerTypeBins
                                                           where x.Active == true
                                                           select x).ToList();



                                     //var stringToSplitC = cancertypelist;

                                     //var queryC = from valC in stringToSplitC.Split(',')
                                     //             select Convert.ToInt32(valC);
                                     foreach (DDCancerTypeBin valC in cancertypelist)
                                     {


                                         // Check for WiWo Contract

                                         var agency = (from Cc in db.AgencySiteProgramSites
                                                       where Cc.SiteID == siteviewmodel.SiteID
                                                       select Cc.AgencySiteID).First();

                                         var contractcheck = (from Cc in bdb.Contracts
                                                              join Dd in bdb.DD_ContractTypes
                                                              on Cc.TypeID equals Dd.ID
                                                              where Cc.AgencySiteID == agency && Cc.TypeID == 3 && Dd.Active == true
                                                              select Cc.ID).Count();


                                         if (contractcheck > 0 && val == 20 && valC.CancerTypeBinID == 4)
                                         {


                                             SubContractorClinics subclin = new SubContractorClinics
                                             {


                                                 SubContractorID = SUBID,
                                                 ClinicID = siteviewmodel.SiteID,
                                                 SiteID = siteviewmodel.SiteID,
                                                 ProgramID = val,
                                                 CancerTypeBinID = valC.CancerTypeBinID,
                                                 IsClinic = true



                                             };

                                             db.SubContractorClinics.Add(subclin);
                                             db.SaveChanges();


                                         }
                                         else if (contractcheck < 1 && val == 20 && valC.CancerTypeBinID == 4)
                                         {


                                             // Do Nothing


                                         }
                                         else
                                         {

                                             SubContractorClinics subclin = new SubContractorClinics
                                             {


                                                 SubContractorID = SUBID,
                                                 ClinicID = siteviewmodel.SiteID,
                                                 SiteID = siteviewmodel.SiteID,
                                                 ProgramID = val,
                                                 CancerTypeBinID = valC.CancerTypeBinID,
                                                 IsClinic = true



                                             };

                                             db.SubContractorClinics.Add(subclin);
                                             db.SaveChanges();


                                         }


                                     }


                                 }


                    }


                }


                return Json(new { Status = "Success", Modified = siteviewmodel.SiteID, Modified2 = sitename, Modified3 = "passed" }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = siteviewmodel.SiteName, Modified2 = "passed" }, JsonRequestBehavior.AllowGet);


        }


        //public ActionResult _SiteList(int id)
        //{


        //    // Program Info
        //    var program = (from x in db.Programs
        //                       where x.ProgramID == id
        //                       select x).FirstOrDefault();


        //    ViewBag.ProgramID = program.ProgramID;
        //    ViewBag.ProgramName = program.ProgramName;




        //            IEnumerable OptionF = db.Sites.Join(db.ProgramSites, c => c.SiteID, p => p.SiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.ProgramID == id).OrderBy(z => z.c.SiteName).AsEnumerable().Select(z => new SelectListItem()

        //            {

        //                Text = z.c.SiteName,
        //                Value = z.c.SiteID.ToString(),
        //                Selected = true,
        //            });


        //            var NewtermsF = new SelectList(OptionF, "Value", "Text");
        //            ViewBag.OptionListF = NewtermsF;


                   
        //         return PartialView();


        //}


        // GET: Sites/Details/5
        public ActionResult Details(int? id)
        {
          

            ViewBag.SiteID = id;

            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();

            ViewBag.PossibleAgencySites = db.AgencySites.Where(x => x.Active == true);

            IEnumerable Option =

              db.Programs.AsEnumerable().Select(c => new SelectListItem()


              {

                  Text = c.ProgramName,
                  Value = c.ProgramID.ToString(),
                  Selected = true


              });

            var Newterms = new SelectList(Option, "Value", "Text");
            ViewBag.OptionList = Newterms;

            var sccount = (from x in db.Sites
                           where x.SiteID == id && x.Active == true
                           select x).Count();

            if (sccount > 0)
            {

                var rows =
               (from x in db.Sites
                join y in db.AgencySiteProgramSites
                on x.SiteID equals y.SiteID
                where x.SiteID == id && x.Active == true
                select y.ProgramID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.Options = rows;


            }

            //IEnumerable OptionSER =

            // context.Services.AsEnumerable().Select(c => new SelectListItem()


            // {

            //     Text = c.ServiceName,
            //     Value = c.ServiceID.ToString(),
            //     Selected = true


            // });


            //var NewtermsSER = new SelectList(OptionSER, "Value", "Text");
            //ViewBag.OptionListSER = NewtermsSER;


            var sites = db.Sites.Distinct().ToList();
            //var servicesites = context.ServiceSites.Distinct().ToList();

            //var serccount = (from x in context.ServiceSites
            //               where x.SiteID == id 
            //               select x).Count();

            //if (serccount > 0)
            //{

            //    var rowsSER =
            //   (from x in sites
            //    join y in servicesites
            //    on x.SiteID equals y.SiteID
            //    where x.SiteID == id && x.Active == true
            //    select y.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


            //    ViewBag.OptionsSER = rowsSER;


            //}

           
            var agencysiteid = db.AgencySiteProgramSites.Where(x => x.SiteID == id).Select(x => x.AgencySiteID).Distinct().First();
            ViewBag.AgencySiteID = agencysiteid;
            var citybins = context.DDCityBins.ToList();
            var statebins = context.DDStateBins.ToList();
            var countybins = context.DDCountyBins.ToList();


            var site = (from c in sites
                        //join dd in agencysiteprograms
                        //on c.SiteID equals dd.SiteID
                        join cc in citybins
                        on c.CityBinID equals cc.CityBinID
                        into pp
                        from bb in pp.DefaultIfEmpty()
                        join kk in statebins
                        on c.StateBinID equals kk.StateBinID
                        into aa
                        from jj in aa.DefaultIfEmpty()
                        join qq in countybins
                        on c.CountyCodeBinID equals qq.CountyBinID
                        into ii
                        from oo in ii.DefaultIfEmpty()
                        where c.SiteID == id
                        select new SiteViewModel()
                        {

                            SiteID = c.SiteID,
                            AgencySiteID = agencysiteid,
                            SiteName = c.SiteName,
                            Address = c.Address,
                            Latitude = c.Latitude,
                            Longitude = c.Longitude,
                            Active = c.Active,
                            CityBinID = bb.CityBinID,
                            StateBinID = jj.StateBinID,
                            CountyCodeBinID = oo.CountyBinID,
                            SecondaryAddress = c.SecondaryAddress,
                            ZipCode = c.ZipCode,
                            Phone = c.Phone,
                            Website = c.Website,
                            ReferralPhone = c.ReferralPhone,
                            Fax = c.Fax


                        }).SingleOrDefault();



            //var site = (from c in sites
            //             join dd in agencysiteprograms
            //             on c.SiteID equals dd.SiteID
            //             into ff
            //             from rr in ff.DefaultIfEmpty()
            //              join cc in citybins
            //              on c.CityBinID equals cc.CityBinID
            //              into pp
            //              from bb in pp.DefaultIfEmpty()
            //              join kk in statebins
            //              on c.StateBinID equals kk.StateBinID
            //              into aa
            //              from jj in aa.DefaultIfEmpty()
            //              join qq in countybins
            //              on c.CountyCodeBinID equals qq.CountyBinID
            //              into ii
            //              from oo in ii.DefaultIfEmpty()
            //              where c.SiteID == id
            //              select new SiteViewModel()
            //              {

            //                  SiteID = c.SiteID,
            //                  AgencySiteID = rr.AgencySiteID,
            //                  SiteName = c.SiteName,
            //                  Address = c.Address,
            //                  Active = c.Active,
            //                  CityBinID = bb.CityBinID,
            //                  StateBinID = jj.StateBinID,
            //                  CountyCodeBinID = oo.CountyBinID,
            //                  SecondaryAddress = c.SecondaryAddress,
            //                  ZipCode = c.ZipCode,
            //                  Phone = c.Phone,
            //                  Fax = c.Fax


            //              }).SingleOrDefault();


            return PartialView(site);
        }

        // GET: Sites/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SiteID,SiteName,SiteCode,Address,CityBinID,StateBinID,ZipCodeBinID,Phone,Fax,Email,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Sites.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        // GET: Sites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }



            string UserName = @User.Identity.Name.ToString();

            var U = (from P in db.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();

            ViewBag.RoleID = U.RoleBinID;


            // SiteName
            var sitename = (from x in db.Sites
                           where x.SiteID == id
                           select x).FirstOrDefault();

            var agencysiteid = (from x in db.AgencySiteProgramSites
                                //join y in db.AgencySites
                                //on x.AgencySiteID equals y.AgencySiteID
                                where x.SiteID == sitename.SiteID
                                select x.AgencySiteID).FirstOrDefault();

            ViewBag.SiteName = sitename.SiteName;
            ViewBag.SiteID = sitename.SiteID;
            ViewBag.AgencySiteID = agencysiteid;

            return PartialView(site);
        }

        // POST: Sites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiteID,SiteName,SiteCode,Address,CityBinID,StateBinID,ZipCodeBinID,Phone,Fax,Email,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Site site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }


        private void PopulateInitialCityData()
        {

            var allCities = context.DDCityBins;
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

            var allStates = context.DDStateBins;
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

            var allCounties = context.DDCountyBins;
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


        // GET: Sites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Site site = db.Sites.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        // POST: Sites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Site site = db.Sites.Find(id);
            db.Sites.Remove(site);
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
