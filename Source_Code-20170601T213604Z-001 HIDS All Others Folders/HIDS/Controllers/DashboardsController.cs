using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTL.Models;
using CTL.ViewModels;

namespace CTL.Controllers
{   
    public class DashboardsController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();
        private HIDSEntities contextH = new HIDSEntities(); 
        //
        // GET: /Dashboards/
        [Authorize]
        public ViewResult Index()
        {
            return View(context.Dashboards.ToList());
        }


        // GET: Tile Bar
        public ActionResult _TileBar()
        {

            string UserName = @User.Identity.Name.ToString();

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.UserID = userid.Id;
            ViewBag.RoleID = userid.RoleBinID;


            // Find out if User has any supplimental Roles

            var rolelist = (from x in context.AspNetUsers
                            join y in context.SiteRoleProgramUserProfiles
                            on x.Id equals y.Id
                          where x.UserName == UserName 
                          select y.RoleID).ToList();

            foreach (int r in rolelist)
            {

                // Site Administrator
                if (r == 6)
                {


                    ViewBag.RoleID = 6;

                }

                // Data Manager
                if (r == 13)
                {

                    ViewBag.RoleID = 13;

                }

                // Program Manager
                if (r == 3)
                {

                    ViewBag.RoleID = 3;


                }



            }


            if (userid.RoleBinID == 1)
            {

                var applications = new List<ApplicationViewModel>();
                var apps = (from x in context.Applications
                            join w in context.ApplicationRolePrograms
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
                                ProgramAList = (from anu in context.Programs
                                                join sr in context.ApplicationRolePrograms
                                                on anu.ProgramID equals sr.ProgramID
                                                where sr.ApplicationID == x.ApplicationID
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



                var results = (from c in context.AgencySites
                               //where c.Active == true
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active


                               }).Distinct().ToList().OrderBy(c => c.AgencySiteName);

                ViewBag.AgencyCount = results.Count();


             



            }

            else if (userid.RoleBinID == 3 || userid.RoleBinID == 13)
            {

                var applications = new List<ApplicationViewModel>();
                var apps = (from x in context.Applications
                            join w in context.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in context.RoleProgramUserProfiles
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
                                ProgramAList = (from anu in context.Programs
                                                join sr in context.ApplicationRolePrograms
                                                on anu.ProgramID equals sr.ProgramID
                                                where sr.ApplicationID == x.ApplicationID
                                                select anu.ProgramName).Distinct().ToList(),
                                RoleListCount = (from c in context.AspNetUsers
                                                 join y in context.SiteRoleProgramUserProfiles
                                                 on c.Id equals y.Id
                                                 where c.Id == userid.Id && y.RoleID != 2
                                                 select y.RoleID).Distinct().Count(),
                                RoleBinID = (from s in context.AspNetUsers
                                             join y in context.SiteRoleProgramUserProfiles
                                             on s.Id equals y.Id
                                             join u in context.Programs
                                             on y.ProgramID equals u.ProgramID
                                             join h in context.ApplicationRolePrograms
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


                var results = (from c in context.AgencySites
                               join x in context.AgencySiteProgramSites
                               on c.AgencySiteID equals x.AgencySiteID
                               join y in context.Programs
                               on x.ProgramID equals y.ProgramID
                               join k in context.RoleProgramUserProfiles
                               on y.ProgramID equals k.ProgramID
                               where k.Id == userid.Id
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active


                               }).Distinct().AsQueryable().OrderBy(c => c.AgencySiteName);


                ViewBag.AgencyCount = results.Count();





                
            }
            else
            {

                var applications = new List<ApplicationViewModel>();
                var apps = (from x in context.Applications
                            join w in context.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in context.SiteRoleProgramUserProfiles
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
                                ProgramAList = (from anu in context.Programs
                                                join sr in context.ApplicationRolePrograms
                                                on anu.ProgramID equals sr.ProgramID
                                                where sr.ApplicationID == x.ApplicationID
                                                select anu.ProgramName).Distinct().ToList(),
                                RoleListCount = (from c in context.AspNetUsers
                                                 join y in context.SiteRoleProgramUserProfiles
                                                 on c.Id equals y.Id
                                                 where c.Id == userid.Id && y.RoleID != 2
                                                 select y.RoleID).Distinct().Count(),
                                RoleBinID = (from s in context.AspNetUsers
                                             join y in context.SiteRoleProgramUserProfiles
                                             on s.Id equals y.Id
                                             join u in context.Programs
                                             on y.ProgramID equals u.ProgramID
                                             join h in context.ApplicationRolePrograms
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

                var results = (from c in context.AgencySites
                               join x in context.AgencySiteProgramSites
                               on c.AgencySiteID equals x.AgencySiteID
                               join y in context.Programs
                               on x.ProgramID equals y.ProgramID
                               join k in context.SiteRoleProgramUserProfiles
                               on y.ProgramID equals k.ProgramID
                               where k.Id == userid.Id && k.SiteID == x.SiteID
                               select new AgencySearchCriteria()
                               {

                                   AgencySiteID = c.AgencySiteID,
                                   AgencySiteName = c.AgencySiteName,
                                   Active = c.Active


                               }).Distinct().AsQueryable().OrderBy(c => c.AgencySiteName);


                ViewBag.AgencyCount = results.Count();


                if (results.Count() == 1)
                {


                    var results0 = (from c in context.AgencySites
                                   join x in context.AgencySiteProgramSites
                                   on c.AgencySiteID equals x.AgencySiteID
                                   join y in context.Programs
                                   on x.ProgramID equals y.ProgramID
                                   join k in context.SiteRoleProgramUserProfiles
                                   on y.ProgramID equals k.ProgramID
                                   where k.Id == userid.Id && k.SiteID == x.SiteID
                                   select new AgencySearchCriteria()
                                   {

                                       AgencySiteID = c.AgencySiteID,
                                       AgencySiteName = c.AgencySiteName,
                                       Active = c.Active


                                   }).First();


                    ViewBag.AgencySiteID = results0.AgencySiteID;





                }

                
            }


            // Get Pending User Count
            var users = new List<UserProfileViewModel>();
            // Get List of Programs that Data Manager has Access to
           

            if (userid.RoleBinID == 1)
            {

                var asset = (from x in context.AspNetUsers
                             join z in context.RoleBins
                             on x.RoleBinID equals z.RoleBinID
                             where x.Active == false
                             select new UserProfileViewModel()
                             {
                                 Id = x.Id,
                                 UserName = x.UserName,
                                 FirstName = x.FirstName + " " + x.LastName,
                                 TelephoneNumber = x.TelephoneNumber,
                                 OrganizationID = x.OrganizationID,
                                 profilePic = x.ProfilePic,
                                 //OrganizationName = gg.OrganizationName,
                                 Status = x.Status,
                                 RoleBinName = z.RoleBinName,
                                 Active = x.Active

                             }).ToList();


                ViewBag.UserCount = asset.Count();



            }
            else
            {


                var PL = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == userid.Id
                          select Y.ProgramID).Distinct().ToList();

                foreach (int p in PL)
                {


                    var asset = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 join y in context.SiteRoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 join u in context.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 where y.ProgramID == p && x.Active == false

                                 select new UserProfileViewModel()
                                 {
                                     Id = x.Id,
                                     UserName = x.UserName,
                                     FirstName = x.FirstName + " " + x.LastName,
                                     TelephoneNumber = x.TelephoneNumber,
                                     OrganizationID = x.OrganizationID,
                                     profilePic = x.ProfilePic,
                                     //OrganizationName = gg.OrganizationName,
                                     Status = x.Status,
                                     RoleBinName = z.RoleBinName,
                                     Active = x.Active

                                 }).Distinct().ToList();


                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }


                }


                var PLP = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == userid.Id
                          select Y.ProgramID).Distinct().ToList();

                
                foreach (int p in PLP)
                {


                    var assetP = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 join y in context.RoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 join u in context.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 where y.ProgramID == p && x.Active == false

                                 select new UserProfileViewModel()
                                 {
                                     Id = x.Id,
                                     UserName = x.UserName,
                                     FirstName = x.FirstName + " " + x.LastName,
                                     TelephoneNumber = x.TelephoneNumber,
                                     OrganizationID = x.OrganizationID,
                                     profilePic = x.ProfilePic,
                                     //OrganizationName = gg.OrganizationName,
                                     Status = x.Status,
                                     RoleBinName = z.RoleBinName,
                                     Active = x.Active

                                 }).Distinct().ToList();


                    foreach (UserProfileViewModel up in assetP)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }


                }

                ViewBag.UserCount = users.Count();


            }



            return PartialView();
        }

        // GET: Nav Bar
        public ActionResult _NavBar()
        {

            string UserName = @User.Identity.Name.ToString();

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.UserID = userid.Id;
           
            ViewBag.FTLoggedIn = userid.PrivacyAgreement;

            // Find out if User has any supplimental Roles

             if (userid.RoleBinID == 1)
            {

                ViewBag.RoleID = userid.RoleBinID;


            }
             else if (userid.RoleBinID == 3 || userid.RoleBinID == 13)
             {



                 var rolelist = (from x in context.AspNetUsers
                                 join y in context.RoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 where x.UserName == UserName
                                 select y.RoleID).ToList();

                 foreach (int r in rolelist)
                 {

                     // Data Manager
                     if (r == 13)
                     {

                         ViewBag.RoleID = 13;

                     }

                     // Program Manager
                     if (r == 3)
                     {

                         ViewBag.RoleID = 3;


                     }



                 }


             }
             else
             {


                 ViewBag.RoleID = userid.RoleBinID;

             }

            return PartialView();
        }

        // GET: Nav Bar
        public ActionResult _ProfileNavBar()
        {

            string UserName = @User.Identity.Name.ToString();

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.UserID = userid.Id;
            ViewBag.RoleID = userid.RoleBinID;
            ViewBag.FTLoggedIn = userid.PrivacyAgreement;

            // Find out if User has any supplimental Roles

            var rolelist = (from x in context.AspNetUsers
                            join y in context.SiteRoleProgramUserProfiles
                            on x.Id equals y.Id
                            where x.UserName == UserName
                            select y.RoleID).ToList();

            foreach (int r in rolelist)
            {

                // Site Administrator
                if (r == 6)
                {


                    ViewBag.RoleID = 6;

                }

                // Data Manager
                if (r == 13)
                {

                    ViewBag.RoleID = 13;

                }

                // Program Manager
                if (r == 3)
                {

                    ViewBag.RoleID = 3;


                }



            }



            return PartialView();
        }


        [Authorize]
        public ActionResult Manage(Dashboard dashboard, int? id)
        {


            if (!HttpContext.Request.IsAuthenticated)
            {


                return Redirect("https://www.healthinformatics.dphe.state.co.us/");

            }

            ViewBag.Upload = id;

            var UC = (from P in context.AspNetUsers
                     select P.Id).Count();

            ViewBag.UserCount = UC;

            //var UG = (from P in context.Organizations
            //          select P.OrganizationID).Count();

            //ViewBag.OrgCount = UG;

            //var UCON = (from P in context.Contents
            //          select P.ContentID).Count();

            //ViewBag.ContentCount = UCON;


            string UserName = @User.Identity.Name.ToString();


            var noticecount = (from P in context.Notices
                               where P.ExpirationDate > DateTime.Now
                               select P.NoticeID).Count();

            ViewBag.NoticeCount = noticecount;


            var UFT = (from P in context.AspNetUsers
                       where P.UserName == UserName
                       select P.firstTimeLoggedIn).SingleOrDefault();
            ViewBag.FTLoggedIn = UFT;
            // Check if Admin is Impersonating
            var userId = User.Identity.GetUserId();
            ViewBag.UserName = userId;


            // First, see if ImpersonatorId is not null
            var impersonatorid = (from x in context.AspNetUsers
                                   where x.Id == userId
                                   select x.ImpersonatorId).FirstOrDefault();

          

            if (impersonatorid != null )
            {


                ViewBag.LoginUserCheck = true;


            }
            else
            {

                ViewBag.LoginUserCheck = false;

            }


          


            //var impersonationid = (from x in context.AspNetUsers
            //                       where x.Id == userId
            //                       select x.ImpersonationId).FirstOrDefault();

            //var impersonatorid = (from x in context.AspNetUsers
            //                       where x.Id == userId
            //                       select x.ImpersonatorId).FirstOrDefault();

            //if (impersonationid != null)
            //{


            //    ViewBag.LoginUserCheck = true;

            //}
            //else
            //{

            //    ViewBag.LoginUserCheck = false;

            //}

         

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            var UPIC = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.ProfilePic).SingleOrDefault();

            ViewBag.ProfilePic = UPIC;

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();


            var user = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast).SingleOrDefault();

            ViewBag.Active = user.Active;
            ViewBag.FTLoggedIn = user.firstTimeLoggedIn;
            

            ViewBag.Role = Uu;

            if (Uu == 1)
            {

                ViewBag.RoleID = 1;
                ViewBag.Id = U;

            }
            else if (Uu == 3)
            {
                ViewBag.RoleID = 3;
                ViewBag.Id = U;


            }
            else
            {
                ViewBag.RoleID = 2;
                ViewBag.Id = U;


            }
           

         


    
            return View(dashboard);

        }


        [Authorize]
        public ActionResult UserAccount(string id)
        {

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;

            
            //var AC = (from P in context.Posts
            //         where P.AwardedId == U
            //         select P.PostID).Count();

            //ViewBag.AwardCount = AC;

            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

           

            ViewBag.OCount = O;

            //var Oname = (from P in context.Organizations
            //             where P.OrganizationID == O
            //             select P.OrganizationName).SingleOrDefault();

            if (Uu == 1)
            {

                ViewBag.RoleID = 1;
                ViewBag.Id = U;

            }
            else if (Uu == 3)
            {
                ViewBag.RoleID = 3;
                ViewBag.Id = U;


            }
            else
            {
                ViewBag.RoleID = 2;
                ViewBag.Id = U;


            }

            ViewBag.Role = Uu;

            var userprofile = (from x in context.AspNetUsers
                               join u in contextH.DDStateBins
                               on x.StateBinID equals u.StateBinID
                               //join w in context.Organizations
                               //on x.OrganizationID equals w.OrganizationID
                               //into ff
                               //from gg in ff.DefaultIfEmpty()
                                 where x.Id == id
                                 select new UserProfileViewModel()
                                 {
                                     Id = x.Id,
                                     UserName = x.UserName,
                                     FirstName = x.FirstName,
                                     LastName = x.LastName,
                                     TelephoneNumber = x.PhoneNumber,
                                     firstTimeLoggedIn = x.firstTimeLoggedIn,
                                     //OrganizationName = gg.OrganizationName,
                                     profilePic = x.ProfilePic,
                                     Address = x.Address,
                                     City = x.City,
                                     StateBinName = u.StateBinName,
                                     ZipCode = x.ZipCode,
                                     Fax = x.Fax,
                                     Status = x.Status,
                                     Active = x.Active,
                                     LastUpdated = x.DateUpdated
                                     
                                 
                                 }).Distinct().SingleOrDefault();


            ViewBag.UID = id;
            ViewBag.OID = O;
            //ViewBag.OName = Oname;
            //ViewBag.LayoutModel = userprofile.firstTimeLoggedIn;


            return View(userprofile);

        }


        //[Authorize]
        //public ActionResult PartnerAccount(int id)
        //{

        //    string UserName = @User.Identity.Name.ToString();

        //    var U = (from P in context.AspNetUsers
        //             where P.UserName == UserName
        //             select P.Id).SingleOrDefault();

        //    var Uu = (from P in context.AspNetUsers
        //              where P.Id == U
        //              select P.RoleBinID).SingleOrDefault();

        //    var O = (from P in context.AspNetUsers
        //             where P.UserName == UserName
        //             select P.OrganizationID).SingleOrDefault();

        //    var Oname = (from P in context.Organizations
        //             where P.OrganizationID == O
        //             select P.OrganizationName).SingleOrDefault();


        //    if (Uu == 1)
        //    {

        //        ViewBag.RoleID = 1;
        //        ViewBag.Id = U;

        //    }
        //    else if (Uu == 3)
        //    {
        //        ViewBag.RoleID = 3;
        //        ViewBag.Id = U;


        //    }
        //    else
        //    {
        //        ViewBag.RoleID = 2;
        //        ViewBag.Id = U;


        //    }

        //    var userprofile = (from x in context.AspNetUsers
        //                       join u in context.StateBins
        //                       on x.StateBinID equals u.StateBinID
        //                       join c in context.Organizations
        //                       on x.OrganizationID equals c.OrganizationID
        //                       where Convert.ToInt32(x.Id) == id
        //                       select new UserProfileViewModel()
        //                       {
        //                           Id = x.Id,
        //                           UserName = x.UserName,
        //                           FirstName = x.FirstName,
        //                           LastName = x.LastName,
        //                           PhoneNumber = x.PhoneNumber,
        //                           firstTimeLoggedIn = x.firstTimeLoggedIn,
        //                           OrganizationID = x.OrganizationID,
        //                           OrganizationName = c.OrganizationName,
        //                           OrgPic = c.OrgPic,
        //                           profilePic = x.ProfilePic,
        //                           Address = x.Address,
        //                           City = x.City,
        //                           StateBinName = u.StateBinName,
        //                           ZipCode = x.ZipCode,
        //                           Fax = x.Fax,
        //                           Status = x.Status,
        //                           Active = x.Active,
        //                           LastUpdated = x.DateUpdated


        //                       }).Distinct().SingleOrDefault();


        //    ViewBag.UID = id;
        //    ViewBag.OID = O;
        //    ViewBag.OName = Oname;
        //    ViewBag.LayoutModel = userprofile.firstTimeLoggedIn;


        //    return View(userprofile);

        //}


        //[Authorize]
        //public ActionResult AccountControl(int id)
        //{


        //    var flcount = (from P in context.AspNetUsers
        //                 where P.RoleBinID == 2
        //                 select P.Id).Count();
            
        //    ViewBag.FLCount = flcount;

        //    var postcount = (from P in context.Posts
        //                   where P.Active == true
        //                   select P.PostID).Count();

        //    ViewBag.PostCount = postcount;

        //    var orgcount = (from P in context.Organizations
        //                     where P.Active == true
        //                     select P.OrganizationID).Count();

        //    ViewBag.OrgCount = orgcount;



        //    var userprofile = (from x in context.AspNetUsers
        //                       join u in context.StateBins
        //                       on x.StateBinID equals u.StateBinID
        //                       join c in context.Organizations
        //                       on x.OrganizationID equals c.OrganizationID
        //                       where Convert.ToInt32(x.Id) == id
        //                       select new UserProfileViewModel()
        //                       {
        //                           Id = x.Id,
        //                           UserName = x.UserName,
        //                           FirstName = x.FirstName,
        //                           LastName = x.LastName,
        //                           PhoneNumber = x.PhoneNumber,
        //                           firstTimeLoggedIn = x.firstTimeLoggedIn,
        //                           OrganizationName = c.OrganizationName,
        //                           profilePic = x.ProfilePic,
        //                           Address = x.Address,
        //                           City = x.City,
        //                           StateBinName = u.StateBinName,
        //                           ZipCode = x.ZipCode,
        //                           Fax = x.Fax,
        //                           Status = x.Status,
        //                           Active = x.Active,
        //                           LastUpdated = x.DateUpdated


        //                       }).Distinct().SingleOrDefault();



        //    ViewBag.LayoutModel = userprofile.firstTimeLoggedIn;


        //    return PartialView(userprofile);

        //}

        [Authorize]
        public ActionResult AccountControlUser(string id)
        {


            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            //var fc = (from P in context.AspNetUsers
            //          join Up in context.ProfileUserPosts
            //          on P.Id equals Up.Id
            //          where P.Id == U
            //          select Up.PostID).Count();

            //ViewBag.FollowCount = fc;

            var flcount = (from P in context.AspNetUsers
                           where P.Active == true
                           select P.Id).Count();

            ViewBag.FLCount = flcount;

            //var postcount = (from P in context.Posts
            //                 where P.Active == true
            //                 select P.PostID).Count();

            //ViewBag.PostCount = postcount;

            //var orgcount = (from P in context.Organizations
            //                where P.Active == true
            //                select P.OrganizationID).Count();

            //ViewBag.OrgCount = orgcount;




            var userprofile = (from x in context.AspNetUsers
                               join u in contextH.DDStateBins
                               on x.StateBinID equals u.StateBinID
                               //join c in context.Organizations
                               //on x.OrganizationID equals c.OrganizationID
                               where x.Id == id
                               select new UserProfileViewModel()
                               {
                                   Id = x.Id,
                                   UserName = x.UserName,
                                   FirstName = x.FirstName,
                                   LastName = x.LastName,
                                   TelephoneNumber = x.PhoneNumber,
                                   firstTimeLoggedIn = x.firstTimeLoggedIn,
                                   OrganizationName = x.OrganizationName,
                                   profilePic = x.ProfilePic,
                                   Address = x.Address,
                                   City = x.City,
                                   StateBinName = u.StateBinName,
                                   ZipCode = x.ZipCode,
                                   Fax = x.Fax,
                                   Status = x.Status,
                                   Active = x.Active,
                                   LastUpdated = x.DateUpdated


                               }).Distinct().SingleOrDefault();



            //ViewBag.LayoutModel = userprofile.firstTimeLoggedIn;


            return PartialView(userprofile);

        }


        //
        // GET: /Dashboards/Details/5
        [Authorize]
        public ViewResult Details(int id)
        {
            Dashboard dashboard = context.Dashboards.Single(x => x.DashboardID == id);
            return View(dashboard);
        }

        //
        // GET: /Dashboards/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Dashboards/Create
        [Authorize]
        [HttpPost]
        public ActionResult Create(Dashboard dashboard)
        {
            if (ModelState.IsValid)
            {
                context.Dashboards.Add(dashboard);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(dashboard);
        }
        
        //
        // GET: /Dashboards/Edit/5
 [Authorize]
        public ActionResult Edit(int id)
        {
            Dashboard dashboard = context.Dashboards.Single(x => x.DashboardID == id);
            return View(dashboard);
        }

        //
        // POST: /Dashboards/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Dashboard dashboard)
        {
            if (ModelState.IsValid)
            {
                context.Entry(dashboard).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dashboard);
        }

        //
        // GET: /Dashboards/Delete/5
 [Authorize]
        public ActionResult Delete(int id)
        {
            Dashboard dashboard = context.Dashboards.Single(x => x.DashboardID == id);
            return View(dashboard);
        }

        //
        // POST: /Dashboards/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Dashboard dashboard = context.Dashboards.Single(x => x.DashboardID == id);
            context.Dashboards.Remove(dashboard);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}