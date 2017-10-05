using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Routing;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using CTL.ViewModels;
using CTL.Models;





namespace CTL.Controllers
{

    [Authorize]
   
    public class UserProfilesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private HIDSEntities contextH = new HIDSEntities();
        private MarriageEntities marriage = new MarriageEntities(); 
      
      

        //
        // GET: /UserProfiles/

        //public ViewResult Index()
        //{
        //    return View(context.UserProfiles.Include(userprofile => userprofile.Organizations).ToList());
        //}

        
        
        //[Authorize]
        //public ActionResult ContactList()
        //{
        //    var asset = (from x in context.UserProfiles
        //                 join y in context.Organizations
        //                 on x.OrganizationID equals y.OrganizationID
        //                 join z in context.RoleBins
        //                 on x.RoleBinID equals z.RoleBinID
        //                 join u in context.PartnerTypeUserProfiles
        //                 on x.UserId equals u.UserId
        //                 join g in context.PartnerTypeBins
        //                 on u.PartnerTypeBinID equals g.PartnerTypeBinID
        //                 where x.Active == true && x.RoleBinID == 3

        //                 select new UserProfileViewModel()
        //                 {
        //                     UserId = x.UserId,
        //                     UserName = x.UserName,
        //                     FirstName = x.FirstName + " " + x.LastName,
        //                     PhoneNumber = x.PhoneNumber,
        //                     profilePic = x.ProfilePic,
        //                     PartnerTypeName = g.PartnerTypeBinName,
        //                     OrganizationName = y.OrganizationName,
        //                     RoleBinName = z.RoleBinName


        //                 }).ToList();

        //    var UserCount = (from ast in context.UserProfiles
        //                     where ast.Active == true && ast.RoleBinID == 2
        //                     select ast.UserId).Count();

        //    ViewBag.UserCount = UserCount;


        //    return PartialView(asset);


        //}

        //[Authorize]
        //public ActionResult PartnerList()
        //{
        //    var asset = (from x in context.AspNetUsers
        //                 join y in context.Organizations
        //                 on x.OrganizationID equals y.OrganizationID
        //                 join z in context.RoleBins
        //                 on x.RoleBinID equals z.RoleBinID
        //                 //join u in context.PartnerTypeAspNetUsers
        //                 //on x.Id equals u.Id
        //                 //join g in context.PartnerTypeBins
        //                 //on u.PartnerTypeBinID equals g.PartnerTypeBinID
        //                 where x.Active == true && x.RoleBinID == 3

        //                 select new UserProfileViewModel()
        //                 {
        //                     Id = x.Id,
        //                     UserName = x.UserName,
        //                     FirstName = x.FirstName + " " + x.LastName,
        //                     PhoneNumber = x.PhoneNumber,
        //                     profilePic = x.ProfilePic,
        //                     //PartnerTypeName = g.PartnerTypeBinName,
        //                     OrganizationName = y.OrganizationName,
        //                     RoleBinName = z.RoleBinName


        //                 }).ToList();

        //    var UserCount = (from ast in context.AspNetUsers
        //                     where ast.Active == true && ast.RoleBinID == 2
        //                     select ast.Id).Count();

        //    ViewBag.UserCount = UserCount;


        //    return PartialView(asset);


        //}

        [Authorize]
        public ActionResult MemberList(int? id)
        {
            var asset = (from x in context.AspNetUsers
                         //join y in context.Organizations
                         //on x.OrganizationID equals y.OrganizationID
                         //join dd in context.Organizations
                         //on x.OrganizationID equals dd.OrganizationID
                         //into ff
                         //from rr in ff.DefaultIfEmpty()
                         join z in context.RoleBins
                         on x.RoleBinID equals z.RoleBinID
                         //join u in context.PartnerTypeUserProfiles
                         //on x.UserId equals u.UserId
                         //join g in context.PartnerTypeBins
                         //on u.PartnerTypeBinID equals g.PartnerTypeBinID
                         where x.Active == true && x.OrganizationID == id

                         select new UserProfileViewModel()
                         {
                             Id = x.Id,
                             UserName = x.UserName,
                             FirstName = x.FirstName + " " + x.LastName,
                             TelephoneNumber = x.PhoneNumber,
                             profilePic = x.ProfilePic,
                             //PartnerTypeName = g.PartnerTypeBinName,
                             OrganizationName = x.OrganizationName,
                             RoleBinName = z.RoleBinName


                         }).ToList();

            var UserCount = (from ast in context.AspNetUsers
                             where ast.Active == true && ast.RoleBinID == 2
                             select ast.Id).Count();

            ViewBag.UserCount = UserCount;


            return PartialView(asset);


        }

        [Authorize]
        public ActionResult Details(string id)
        {

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.Id == id
                     select P).SingleOrDefault();
            
            
            ViewBag.UserID = id;

            if (U.RoleBinID == 1)
            {

                ViewBag.RoleID = U.RoleBinID;



                var apps = (from x in context.Applications
                            select x).Distinct().ToList();


                ViewBag.CurrentApplicationID = new SelectList(apps, "ApplicationID", "ApplicationName", U.CurrentApplicationID);



            }
            else if (U.RoleBinID == 2)
            {

                var rolelist = (from x in context.AspNetUsers
                                join y in context.SiteRoleProgramUserProfiles
                                on x.Id equals y.Id
                                where x.UserName == UserName
                                select y.RoleID).ToList();

                foreach (int r in rolelist)
                {

                    // Site Admin
                    if (r == 6)
                    {

                        ViewBag.RoleID = 6;

                    }
                    else
                    {

                        ViewBag.RoleID = 2;

                    }
                   
                }


                var apps = (from x in context.Applications
                            join w in context.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in context.SiteRoleProgramUserProfiles
                            on w.RoleID equals z.RoleID
                            where z.Id == U.Id && w.ProgramID == z.ProgramID
                            select x).Distinct().ToList();


                ViewBag.CurrentApplicationID = new SelectList(apps, "ApplicationID", "ApplicationName", U.CurrentApplicationID);




            }
            else
            {

                // Find out if User has any supplimental Roles

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

                var apps = (from x in context.Applications
                            join w in context.ApplicationRolePrograms
                            on x.ApplicationID equals w.ApplicationID
                            join z in context.RoleProgramUserProfiles
                            on w.RoleID equals z.RoleID
                            where z.Id == U.Id && w.ProgramID == z.ProgramID
                            select x).Distinct().ToList();


                ViewBag.CurrentApplicationID = new SelectList(apps, "ApplicationID", "ApplicationName", U.CurrentApplicationID);



            }



            

            var asset = (from x in context.AspNetUsers
                      
                         join z in context.RoleBins
                         on x.RoleBinID equals z.RoleBinID
                         where x.Id == id

                         select new UserProfileViewModel()
                         {
                             Id = x.Id,
                             UserName = x.UserName,
                             FirstName = x.FirstName + " " + x.LastName,
                             TelephoneNumber = x.TelephoneNumber,
                             profilePic = x.ProfilePic,
                             Dashboard = x.Dashboard,
                             CurrentApplicationID = x.CurrentApplicationID,
                             OrganizationName = x.OrganizationName,
                             RoleBinName = z.RoleBinName,
                           
                         }).FirstOrDefault();


            return PartialView(asset);
        }



        public JsonResult _UpdateAppSettings(UserProfileViewModel userprofile)
        {

            if (ModelState.IsValid)
            {


                if (userprofile.Dashboard == true)
                {


                    userprofile.CurrentApplicationID = null;

                }


                    //Set Application
                    ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.Id == userprofile.Id);
                    userprofile1.Dashboard = userprofile.Dashboard;
                    userprofile1.CurrentApplicationID = userprofile.CurrentApplicationID;
                    context.SaveChanges();

                    return Json(new { Status = "Success", Modified = userprofile.Id }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

        }


        [Authorize]
        public ActionResult AllMemberList()
        {
            var asset = (from x in context.AspNetUsers
                         //join y in context.Organizations
                         //on x.OrganizationID equals y.OrganizationID
                         //join dd in context.Organizations
                         //on x.OrganizationID equals dd.OrganizationID
                         //into ff
                         //from rr in ff.DefaultIfEmpty()
                         join z in context.RoleBins
                         on x.RoleBinID equals z.RoleBinID
                         //join ddd in context.GraduatedBins
                         //on x.GraduatedBinID equals ddd.GraduatedBinID
                         //into fff
                         //from rrr in fff.DefaultIfEmpty()
                         //join u in context.PartnerTypeUserProfiles
                         //on x.UserId equals u.UserId
                         //join g in context.PartnerTypeBins
                         //on u.PartnerTypeBinID equals g.PartnerTypeBinID
                         where x.Active == true 

                         select new UserProfileViewModel()
                         {
                             Id = x.Id,
                             UserName = x.UserName,
                             FirstName = x.FirstName + " " + x.LastName,
                             TelephoneNumber = x.TelephoneNumber,
                             profilePic = x.ProfilePic,
                             //PartnerTypeName = g.PartnerTypeBinName,
                             OrganizationName = x.OrganizationName,
                             RoleBinName = z.RoleBinName,
                             //GraduatedBinName = rrr.GraduatedBinName

                         }).ToList();

            var UserCount = (from ast in context.AspNetUsers
                             where ast.Active == true && ast.RoleBinID == 2
                             select ast.Id).Count();

            ViewBag.UserCount = UserCount;


            return PartialView(asset);


        }



        public ActionResult LoginUser(string id)
        {

            var username = (from x in context.AspNetUsers
                            where x.Id == id
                            select x).FirstOrDefault();

            ViewBag.UserName = username.UserName;
            ViewBag.Name = username.FirstName + " " + username.LastName;

            return PartialView();

        }


        [HttpPost]
        public async Task<ActionResult> LoginUser(UserProfileViewModel usermodel)
        {


            string originalUsername = @User.Identity.Name.ToString();


            var userA =
                   (from c in context.AspNetUsers
                    where c.UserName == originalUsername
                    select c).First();

            var userToImpersonate = await UserManager.FindByNameAsync(usermodel.UserName);
            var identityToImpersonate = await UserManager.CreateIdentityAsync(userToImpersonate, DefaultAuthenticationTypes.ApplicationCookie);
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = false
            }, identityToImpersonate);

            var temp = userA.Id;
            // Set Session Variable
            System.Web.HttpContext.Current.Session.Add("_ImpersonationId", temp);
            //Set Impersonation Id
            ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == originalUsername);
            userprofile1.ImpersonationId = usermodel.Id;
           
            context.SaveChanges();

            //Set Impersonator Id
            ApplicationUser userprofile11 = context.AspNetUsers.Single(p => p.UserName == usermodel.UserName);
            userprofile11.ImpersonatorId = userA.Id;
            context.SaveChanges();

            //identityToImpersonate.AddClaim(new Claim("UserImpersonation", usermodel.Id));
            //identityToImpersonate.AddClaim(new Claim("OriginalUsername", originalUsername));

            return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }));



        }


        public ActionResult StopImpersonation(string id)
        {

            // person who is impersonated
            var username = (from x in context.AspNetUsers
                            where x.Id == id
                            select x).FirstOrDefault();

            // person who is impersonating
            var usernameI = (from x in context.AspNetUsers
                            where x.Id == id
                            select x).FirstOrDefault();

            ViewBag.UserName = username.ImpersonatorId;
            ViewBag.Name = usernameI.FirstName + " " + usernameI.LastName;

            return PartialView();

        }


        [HttpPost]
        public async Task<ActionResult> StopImpersonation(UserProfileViewModel usermodel)
        {

            // Person who is doing impersonation
            var username = (from x in context.AspNetUsers
                            where x.Id == usermodel.UserName
                            select x.UserName).FirstOrDefault();

           // Person who is being impersonated
            string originalUsername = @User.Identity.Name.ToString();

            var userToImpersonate = await UserManager.FindByNameAsync(username);
            var identityToImpersonate = await UserManager.CreateIdentityAsync(userToImpersonate, DefaultAuthenticationTypes.ApplicationCookie);
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties()
            {
                IsPersistent = false
            }, identityToImpersonate);

            //Set Impersonation Id
            ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == username);
            userprofile1.ImpersonationId = null;
            context.SaveChanges();

            //Set Impersonator Id
            ApplicationUser userprofile11 = context.AspNetUsers.Single(p => p.UserName == originalUsername);
            userprofile11.ImpersonatorId = null;
            context.SaveChanges();


            //identityToImpersonate.AddClaim(new Claim("UserImpersonation", usermodel.Id));
            //identityToImpersonate.AddClaim(new Claim("OriginalUsername", originalUsername));

            return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }));



        }


        [Authorize]
        public ActionResult _ExternalApprovalCheck(string id)
        {
            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();


            var MCount = (from P in marriage.tblUserAccounts
                     where P.UserId == id
                     select P).Count();

            if (MCount > 0)
            {

                var M = (from P in marriage.tblUserAccounts
                         where P.UserId == id
                         select P).SingleOrDefault();

                ViewBag.Approved = M.SupApproval;


            }

            return PartialView();

        }

        
        [Authorize]
        public ActionResult _ActiveUserProfiles()
        {
            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();

            int roleid = 0;


            if (U.RoleBinID == 1)
            {

                ViewBag.RoleID = U.RoleBinID;
                roleid = 1;

            }
            else if (U.RoleBinID == 2)
            {


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
                        roleid = 6;

                    }

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
            else
            {

                // Find out if User has any supplimental Roles

                var rolelist = (from x in context.AspNetUsers
                                join y in context.RoleProgramUserProfiles
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

            var users = new List<UserProfileViewModel>();
          



            if (roleid == 1)
            {

                // Get List of Programs that Data Manager has Access to
                var PL = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
                          select Y.ProgramID).Distinct().ToList();

                foreach (int p in PL)
                {


                    var asset = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 //join k in context.RoleProgramUserProfiles
                                 //on x.Id equals k.Id
                                 join y in context.SiteRoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 join u in context.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 //join t in context.AgencySites
                                 //on y.AgencySiteID equals t.AgencySiteID
                                 //  where y.ProgramID == p && x.RoleBinID != 1

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
                                     AgencySiteName = z.RoleBinName,
                                     Coordinator = false,
                                     //AgencySiteName = (from anu in context.AspNetUsers
                                     //                  join sr in context.RoleProgramUserProfiles
                                     //                  on anu.Id equals sr.Id
                                     //                  join pr in context.Programs
                                     //                  on sr.ProgramID equals pr.ProgramID
                                     //                  join ro in context.RoleBins
                                     //                  on sr.RoleID equals ro.RoleBinID
                                     //                  where anu.Id == x.Id
                                     //                  select ro.RoleBinName).FirstOrDefault(),
                                     Active = x.Active,
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName).Distinct().ToList(),
                                     AgencyRoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select ro.RoleBinName).Distinct().ToList(),
                                     SiteAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join ss in context.Sites
                                                  on sr.SiteID equals ss.SiteID
                                                  where anu.Id == x.Id
                                                  select ss.SiteName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();


                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }



                    var assetP = (from x in context.AspNetUsers
                                  //join w in context.Organizations
                                  //on x.OrganizationID equals w.OrganizationID
                                  //into ff
                                  //from gg in ff.DefaultIfEmpty()
                                  join k in context.RoleProgramUserProfiles
                                  on x.Id equals k.Id
                                  //join y in context.SiteRoleProgramUserProfiles
                                  //on x.Id equals y.Id
                                  join u in context.Programs
                                  on k.ProgramID equals u.ProgramID
                                  join z in context.RoleBins
                                  on x.RoleBinID equals z.RoleBinID
                                  //  where k.ProgramID == p && x.RoleBinID != 1

                                  select new UserProfileViewModel()
                                  {
                                      Id = x.Id,
                                      UserName = x.UserName,
                                      FirstName = x.FirstName + " " + x.LastName,
                                      TelephoneNumber = x.TelephoneNumber,
                                      OrganizationID = x.OrganizationID,
                                      profilePic = x.ProfilePic,
                                      AgencySiteName = (from anu in context.AspNetUsers
                                                        join sr in context.RoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).FirstOrDefault(),
                                      Coordinator = k.Coordinator,
                                      //OrganizationName = gg.OrganizationName,
                                      Status = x.Status,
                                      RoleBinName = z.RoleBinName,
                                      Active = x.Active,
                                      RoleAList = (from anu in context.AspNetUsers
                                                   join sr in context.RoleProgramUserProfiles
                                                   on anu.Id equals sr.Id
                                                   join pr in context.Programs
                                                   on sr.ProgramID equals pr.ProgramID
                                                   join ro in context.RoleBins
                                                   on sr.RoleID equals ro.RoleBinID
                                                   where anu.Id == x.Id
                                                   select pr.ProgramName).Distinct().ToList(),
                                      AgencyRoleAList = (from anu in context.AspNetUsers
                                                         join sr in context.SiteRoleProgramUserProfiles
                                                         on anu.Id equals sr.Id
                                                         join pr in context.Programs
                                                         on sr.ProgramID equals pr.ProgramID
                                                         join ro in context.RoleBins
                                                         on sr.RoleID equals ro.RoleBinID
                                                         where anu.Id == x.Id
                                                         select ro.RoleBinName).Distinct().ToList(),
                                      SiteAList = (from anu in context.AspNetUsers
                                                   join sr in context.RoleProgramUserProfiles
                                                   on anu.Id equals sr.Id
                                                   join ss in context.Programs
                                                   on sr.ProgramID equals ss.ProgramID
                                                   where anu.Id == x.Id
                                                   select ss.ProgramName).Distinct().ToList(),
                                      CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                  }).ToList();


                    foreach (UserProfileViewModel upP in assetP)
                    {


                        bool alreadyExists = users.Any(x => x.Id == upP.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(upP);

                        }

                    }



                 



                    ViewBag.UserCount = users.Count();


                }

                  return PartialView(users);


            }
            else if (roleid == 3 || roleid == 13)
            {

                // Get List of Programs that Data Manager has Access to
                var PL = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
                          select Y.ProgramID).Distinct().ToList();

                foreach (int p in PL)
                {


                    var asset = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 //join k in context.RoleProgramUserProfiles
                                 //on x.Id equals k.Id
                                 join y in context.SiteRoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 join u in context.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 //join t in context.AgencySites
                                 //on y.AgencySiteID equals t.AgencySiteID
                                 where y.ProgramID == p && x.RoleBinID != 1

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
                                     Active = x.Active,
                                     AgencySiteName = z.RoleBinName,
                                     Coordinator = false,
                                     //AgencySiteName = (from anu in context.AspNetUsers
                                     //                  join sr in context.RoleProgramUserProfiles
                                     //                  on anu.Id equals sr.Id
                                     //                  join pr in context.Programs
                                     //                  on sr.ProgramID equals pr.ProgramID
                                     //                  join ro in context.RoleBins
                                     //                  on sr.RoleID equals ro.RoleBinID
                                     //                  where anu.Id == x.Id
                                     //                  select ro.RoleBinName).FirstOrDefault(),
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName).Distinct().ToList(),
                                     AgencyRoleAList = (from anu in context.AspNetUsers
                                                        join sr in context.SiteRoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).Distinct().ToList(),
                                     SiteAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join ss in context.Sites
                                                  on sr.SiteID equals ss.SiteID
                                                  where anu.Id == x.Id
                                                  select ss.SiteName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();


                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }



                    var assetP = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 join k in context.RoleProgramUserProfiles
                                 on x.Id equals k.Id
                                 //join y in context.SiteRoleProgramUserProfiles
                                 //on x.Id equals y.Id
                                 join u in context.Programs
                                 on k.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 where k.ProgramID == p && x.RoleBinID != 1

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
                                    // AgencySiteName = "State Staff",
                                     AgencySiteName = (from anu in context.AspNetUsers
                                                  join sr in context.RoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                       select ro.RoleBinName).FirstOrDefault(),
                                     Coordinator = k.Coordinator,
                                     Active = x.Active,
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.RoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName).Distinct().ToList(),
                                     AgencyRoleAList = (from anu in context.AspNetUsers
                                                        join sr in context.SiteRoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).Distinct().ToList(),
                                     SiteAList = (from anu in context.AspNetUsers
                                                  join sr in context.RoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join ss in context.Programs
                                                  on sr.ProgramID equals ss.ProgramID
                                                  where anu.Id == x.Id
                                                  select ss.ProgramName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();


                    foreach (UserProfileViewModel upP in assetP)
                    {


                        bool alreadyExists = users.Any(x => x.Id == upP.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(upP);

                        }

                    }



                   

                    //var assetcount = (from x in context.AspNetUsers
                    //                  join y in context.SiteRoleProgramUserProfiles
                    //                  on x.Id equals y.Id
                    //                  join u in context.Programs
                    //                  on y.ProgramID equals u.ProgramID
                    //                  join z in context.RoleBins
                    //                  on x.RoleBinID equals z.RoleBinID
                    //                  where y.ProgramID == p

                    //                  select new UserProfileViewModel()
                    //                  {
                    //                      Id = x.Id,
                    //                  }).Distinct().ToList();


                    ViewBag.UserCount = users.Count();




                }


                return PartialView(users);

            }
            else if (roleid == 6)
            {

                // Get Agency of User
                var PL = (from P in context.AspNetUsers
                          join Y in context.SiteRoleProgramUserProfiles
                          on P.Id equals Y.Id
                          join K in context.AgencySiteProgramSites
                          on Y.SiteID equals K.SiteID
                          join Z in context.AgencySites
                          on K.AgencySiteID equals Z.AgencySiteID
                          where P.Id == U.Id
                          select Z.AgencySiteID).Distinct().ToList();

                foreach (int p in PL)
                {


                    var asset = (from x in context.AspNetUsers
                                 //join w in context.Organizations
                                 //on x.OrganizationID equals w.OrganizationID
                                 //into ff
                                 //from gg in ff.DefaultIfEmpty()
                                 //join k in context.RoleProgramUserProfiles
                                 //on x.Id equals k.Id
                                 join y in context.SiteRoleProgramUserProfiles
                                 on x.Id equals y.Id
                                 join u in context.Programs
                                 on y.ProgramID equals u.ProgramID
                                 join z in context.RoleBins
                                 on x.RoleBinID equals z.RoleBinID
                                 join K in context.AgencySiteProgramSites
                                 on y.SiteID equals K.SiteID
                                 join t in context.AgencySites
                                 on K.AgencySiteID equals t.AgencySiteID
                                 where t.AgencySiteID == p && x.RoleBinID != 1 && x.RoleBinID != 13 && x.RoleBinID != 3 

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
                                     Active = x.Active,
                                     AgencySiteName = z.RoleBinName,
                                     Coordinator = false,
                                     //AgencySiteName = (from anu in context.AspNetUsers
                                     //                  join sr in context.RoleProgramUserProfiles
                                     //                  on anu.Id equals sr.Id
                                     //                  join pr in context.Programs
                                     //                  on sr.ProgramID equals pr.ProgramID
                                     //                  join ro in context.RoleBins
                                     //                  on sr.RoleID equals ro.RoleBinID
                                     //                  where anu.Id == x.Id
                                     //                  select ro.RoleBinName).FirstOrDefault(),
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName).Distinct().ToList(),
                                     AgencyRoleAList = (from anu in context.AspNetUsers
                                                        join sr in context.SiteRoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).Distinct().ToList(),
                                     SiteAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join ss in context.Sites
                                                  on sr.SiteID equals ss.SiteID
                                                  where anu.Id == x.Id
                                                  select ss.SiteName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();


                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }



                    var assetP = (from x in context.AspNetUsers
                                  //join w in context.Organizations
                                  //on x.OrganizationID equals w.OrganizationID
                                  //into ff
                                  //from gg in ff.DefaultIfEmpty()
                                  join k in context.RoleProgramUserProfiles
                                  on x.Id equals k.Id
                                  //join y in context.SiteRoleProgramUserProfiles
                                  //on x.Id equals y.Id
                                  join u in context.Programs
                                  on k.ProgramID equals u.ProgramID
                                  join z in context.RoleBins
                                  on x.RoleBinID equals z.RoleBinID
                                  where k.ProgramID == p && x.RoleBinID != 1

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
                                      // AgencySiteName = "State Staff",
                                      AgencySiteName = (from anu in context.AspNetUsers
                                                        join sr in context.RoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).FirstOrDefault(),
                                      Coordinator = k.Coordinator,
                                      Active = x.Active,
                                      RoleAList = (from anu in context.AspNetUsers
                                                   join sr in context.RoleProgramUserProfiles
                                                   on anu.Id equals sr.Id
                                                   join pr in context.Programs
                                                   on sr.ProgramID equals pr.ProgramID
                                                   join ro in context.RoleBins
                                                   on sr.RoleID equals ro.RoleBinID
                                                   where anu.Id == x.Id
                                                   select pr.ProgramName).Distinct().ToList(),
                                      AgencyRoleAList = (from anu in context.AspNetUsers
                                                         join sr in context.SiteRoleProgramUserProfiles
                                                         on anu.Id equals sr.Id
                                                         join pr in context.Programs
                                                         on sr.ProgramID equals pr.ProgramID
                                                         join ro in context.RoleBins
                                                         on sr.RoleID equals ro.RoleBinID
                                                         where anu.Id == x.Id
                                                         select ro.RoleBinName).Distinct().ToList(),
                                      SiteAList = (from anu in context.AspNetUsers
                                                   join sr in context.RoleProgramUserProfiles
                                                   on anu.Id equals sr.Id
                                                   join ss in context.Programs
                                                   on sr.ProgramID equals ss.ProgramID
                                                   where anu.Id == x.Id
                                                   select ss.ProgramName).Distinct().ToList(),
                                      CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                  }).ToList();


                    foreach (UserProfileViewModel upP in assetP)
                    {


                        bool alreadyExists = users.Any(x => x.Id == upP.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(upP);

                        }

                    }





                    //var assetcount = (from x in context.AspNetUsers
                    //                  join y in context.SiteRoleProgramUserProfiles
                    //                  on x.Id equals y.Id
                    //                  join u in context.Programs
                    //                  on y.ProgramID equals u.ProgramID
                    //                  join z in context.RoleBins
                    //                  on x.RoleBinID equals z.RoleBinID
                    //                  where y.ProgramID == p

                    //                  select new UserProfileViewModel()
                    //                  {
                    //                      Id = x.Id,
                    //                  }).Distinct().ToList();


                    ViewBag.UserCount = users.Count();




                }


                return PartialView(users);

            }
            else
            {

                // Get List of Programs that Data Manager has Access to
                var PL = (from P in context.AspNetUsers
                          join Y in context.SiteRoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
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
                                 //join t in context.AgencySites
                                 //on y.AgencySiteID equals t.AgencySiteID
                                 where y.ProgramID == p

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
                                     AgencySiteName = z.RoleBinName,
                                     Coordinator = false,
                                     //AgencySiteName = (from anu in context.AspNetUsers
                                     //                  join sr in context.RoleProgramUserProfiles
                                     //                  on anu.Id equals sr.Id
                                     //                  join pr in context.Programs
                                     //                  on sr.ProgramID equals pr.ProgramID
                                     //                  join ro in context.RoleBins
                                     //                  on sr.RoleID equals ro.RoleBinID
                                     //                  where anu.Id == x.Id
                                     //                  select ro.RoleBinName).FirstOrDefault(),
                                     Active = x.Active,
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName).Distinct().ToList(),
                                     AgencyRoleAList = (from anu in context.AspNetUsers
                                                        join sr in context.SiteRoleProgramUserProfiles
                                                        on anu.Id equals sr.Id
                                                        join pr in context.Programs
                                                        on sr.ProgramID equals pr.ProgramID
                                                        join ro in context.RoleBins
                                                        on sr.RoleID equals ro.RoleBinID
                                                        where anu.Id == x.Id
                                                        select ro.RoleBinName).Distinct().ToList(),
                                     SiteAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join ss in context.Sites
                                                  on sr.SiteID equals ss.SiteID
                                                  where anu.Id == x.Id
                                                  select ss.SiteName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();



                    var assetcount = (from x in context.AspNetUsers
                                      join y in context.SiteRoleProgramUserProfiles
                                      on x.Id equals y.Id
                                      join u in context.Programs
                                      on y.ProgramID equals u.ProgramID
                                      join z in context.RoleBins
                                      on x.RoleBinID equals z.RoleBinID
                                      where y.ProgramID == p

                                      select new UserProfileViewModel()
                                      {
                                          Id = x.Id,
                                      }).Distinct().ToList();


                    ViewBag.UserCount = assetcount.Count();




                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }


                }


                return PartialView(users);

            }
            
            return PartialView();

        }


        [OutputCache(NoStore = true, Location = OutputCacheLocation.Client, Duration = 10)] // every 10 sec
        [Authorize]
        public ActionResult _UserProfilesFeed()
        {


            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P).SingleOrDefault();


            ViewBag.UserID = U.Id;

            if (U.RoleBinID == 1)
            {

                ViewBag.RoleID = U.RoleBinID;


            }
            else if (U.RoleBinID == 3 || U.RoleBinID == 13)
            {

                // Find out if User has any supplimental Roles

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
            else{

                ViewBag.RoleID = U.RoleBinID;

            }
            var users = new List<UserProfileViewModel>();
            // Get List of Programs that Data Manager has Access to
          



            if (U.RoleBinID == 1)
            {

                var asset = (from x in context.AspNetUsers
                             join z in context.RoleBins
                             on x.RoleBinID equals z.RoleBinID
                             where x.CurrentlyLoggedIn == true
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
                                 Active = x.Active,
                                 RoleAList = (from anu in context.AspNetUsers
                                              join sr in context.SiteRoleProgramUserProfiles
                                              on anu.Id equals sr.Id
                                              join pr in context.Programs
                                              on sr.ProgramID equals pr.ProgramID
                                              join ro in context.RoleBins
                                              on sr.RoleID equals ro.RoleBinID
                                              where anu.Id == x.Id
                                              select pr.ProgramName + " " + ro.RoleBinName).Distinct().ToList(),
                                 CurrentlyLoggedIn = x.CurrentlyLoggedIn

                             }).ToList();


                var assetcount = (from x in context.AspNetUsers
                                  join z in context.RoleBins
                                  on x.RoleBinID equals z.RoleBinID


                                  select new UserProfileViewModel()
                                  {
                                      Id = x.Id,
                                  }).Distinct().ToList();


                ViewBag.UserCount = assetcount.Count();
               


                return PartialView(asset);



            }
            else if (U.RoleBinID == 3 || U.RoleBinID == 13)
            {


                var PL = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
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
                                 where y.ProgramID == p && x.CurrentlyLoggedIn == true 

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
                                     Active = x.Active,
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName + " " + ro.RoleBinName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();



                    //var assetcount = (from x in context.AspNetUsers
                    //                  join y in context.SiteRoleProgramUserProfiles
                    //                  on x.Id equals y.Id
                    //                  join u in context.Programs
                    //                  on y.ProgramID equals u.ProgramID
                    //                  join z in context.RoleBins
                    //                  on x.RoleBinID equals z.RoleBinID
                    //                  where y.ProgramID == p

                    //                  select new UserProfileViewModel()
                    //                  {
                    //                      Id = x.Id,
                    //                  }).Distinct().ToList();


                  




                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }


                    ViewBag.UserCount = users.Count();

                }


                 var PLSS = (from P in context.AspNetUsers
                          join Y in context.RoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
                          select Y.ProgramID).Distinct().ToList();


                 foreach (int p in PLSS)
                 {




                     var assetSS = (from x in context.AspNetUsers
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
                                    where y.ProgramID == p && x.CurrentlyLoggedIn == true

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
                                        Active = x.Active,
                                        RoleAList = (from anu in context.AspNetUsers
                                                     join sr in context.SiteRoleProgramUserProfiles
                                                     on anu.Id equals sr.Id
                                                     join pr in context.Programs
                                                     on sr.ProgramID equals pr.ProgramID
                                                     join ro in context.RoleBins
                                                     on sr.RoleID equals ro.RoleBinID
                                                     where anu.Id == x.Id
                                                     select pr.ProgramName + " " + ro.RoleBinName).Distinct().ToList(),
                                        CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                    }).ToList();








                     foreach (UserProfileViewModel up in assetSS)
                     {


                         bool alreadyExists = users.Any(x => x.Id == up.Id);

                         if (alreadyExists == false)
                         {

                             users.Add(up);

                         }

                     }


                 }



                return PartialView(users);

            }
            else
            {

                var PLU = (from P in context.AspNetUsers
                          join Y in context.SiteRoleProgramUserProfiles
                          on P.Id equals Y.Id
                          where P.Id == U.Id
                          select Y.ProgramID).Distinct().ToList();

                foreach (int p in PLU)
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
                                 where y.ProgramID == p && x.CurrentlyLoggedIn == true 

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
                                     Active = x.Active,
                                     RoleAList = (from anu in context.AspNetUsers
                                                  join sr in context.SiteRoleProgramUserProfiles
                                                  on anu.Id equals sr.Id
                                                  join pr in context.Programs
                                                  on sr.ProgramID equals pr.ProgramID
                                                  join ro in context.RoleBins
                                                  on sr.RoleID equals ro.RoleBinID
                                                  where anu.Id == x.Id
                                                  select pr.ProgramName + " " + ro.RoleBinName).Distinct().ToList(),
                                     CurrentlyLoggedIn = x.CurrentlyLoggedIn

                                 }).ToList();



                    var assetcount = (from x in context.AspNetUsers
                                      join y in context.SiteRoleProgramUserProfiles
                                      on x.Id equals y.Id
                                      join u in context.Programs
                                      on y.ProgramID equals u.ProgramID
                                      join z in context.RoleBins
                                      on x.RoleBinID equals z.RoleBinID
                                      where y.ProgramID == p

                                      select new UserProfileViewModel()
                                      {
                                          Id = x.Id,
                                      }).Distinct().ToList();


                    ViewBag.UserCount = assetcount.Count();




                    foreach (UserProfileViewModel up in asset)
                    {


                        bool alreadyExists = users.Any(x => x.Id == up.Id);

                        if (alreadyExists == false)
                        {

                            users.Add(up);

                        }

                    }


                }


                return PartialView(users);

            }






            return PartialView();

        }
        
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateIdentityAsync(UserManager));
        }


        //[HttpPost]
        //public async Task<ActionResult> _Admin(ApplicationUser appuser)
        //{


        //    if (Request.Files["fileUpload1"].ContentLength > 0)
        //    {
        //        string extension = System.IO.Path.GetExtension(Request.Files["fileUpload1"].FileName);
        //        string p = Request.Files["fileUpload1"].FileName;
        //        string pp = System.IO.Path.GetFileName(p);
        //        string path1 = string.Format("{0}\\{1}", Server.MapPath("~/Content/UploadedFolder"), pp);
        //        ViewBag.FilePath = path1;
        //        if (System.IO.File.Exists(path1))
        //            System.IO.File.Delete(path1);

        //        Request.Files["fileUpload1"].SaveAs(path1);
        //        string sqlConnectionString = @"Data Source=DPHE332\PROD;Initial Catalog=CTL;UID=CTL;Password=Connect@1!;MultipleActiveResultSets=True;";

        //        //Create connection string to Excel work book
        //        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
        //        //Create Connection to Excel work book
        //        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
        //        //Create OleDbCommand to fetch data from Excel
        //        OleDbCommand cmd = new OleDbCommand("Select [UserDumpID],[FirstName],[LastName],[Email],[Graduated],[Gender] from [Sheet1$]", excelConnection);

        //        excelConnection.Open();
        //        OleDbDataReader dReader;
        //        dReader = cmd.ExecuteReader();

        //        SqlBulkCopy sqlBulk = new SqlBulkCopy(sqlConnectionString);
        //        //Give your Destination table name
        //        sqlBulk.DestinationTableName = "dbo.USER_DUMP";
        //        sqlBulk.WriteToServer(dReader);
        //        excelConnection.Close();


        //        string UserName = @User.Identity.Name.ToString();
        //        DateTime exDate = DateTime.Now;


        //        //Loop through Indicators Dump
        //        var dumpuser = (from x in context.USER_DUMPs
        //                       select x).Distinct().ToList();




        //        foreach (USER_DUMP d in dumpuser)
        //        {


        //       var user = new ApplicationUser() { UserName = d.Email, Email = d.Email };
        //        IdentityResult result = await UserManager.CreateAsync(user, "FamilyLeader2014!");
        //        if (result.Succeeded)
        //        {
        //            await SignInAsync(user, isPersistent: false);

        //            //Assign User to request and update account information
        //            var userA =
        //            (from c in context.AspNetUsers
        //             where c.UserName == d.Email
        //             select c).First();

        //            // Update user record.

        //            userA.FirstName = d.FirstName;
        //            userA.LastName = d.LastName;
                    
        //            //userA.OrganizationID = model.OrganizationID;
        //            //userA.Address = model.Address;
        //            //userA.City = model.City;
        //            //userA.ZipCode = model.ZipCode;
        //            //userA.Fax = model.Fax;
        //            //userA.Status = model.Status;
        //            userA.DateUpdated = exDate;
        //            userA.DateCreated = exDate;
        //            userA.CreatedBy = UserName;
        //            userA.UpdatedBy = UserName;
        //            //userA.OrganizationName = model.OrganizationName;
        //            //userA.PhoneNumber = model.PhoneNumber;
        //            userA.RoleBinID = 2;
        //            userA.Status = false;
        //            userA.EmailConfirmed = true;
        //            //userA.StateBinID = model.StateBinID;

        //            var year = d.Graduated.Trim();

        //            //var grad = (from P in context.GraduatedBins
        //            //         where P.GraduatedBinName == year
        //            //         select P.GraduatedBinID).SingleOrDefault();


        //            //userA.GraduatedBinID = grad;
        //            //userA.Active = model.Status;
        //            userA.Active = false;
        //            userA.firstTimeLoggedIn = true;

        //            if (d.Gender == "1")
        //            {


        //                userA.ProfilePic = "<p><img src='http://www.connecttoleadership.dphe.state.co.us/Assets/maleblankprofile.png'/></p>";

        //            }
        //            else
        //            {

        //                userA.ProfilePic = "<p><img src='http://www.connecttoleadership.dphe.state.co.us/Assets/blankprofile.jpg'/></p>";

        //            }


                    
                 
        //            //user.Documents = model.Documents;

        //            // Ask the DataContext to save all the changes.
        //            context.SaveChanges();


        //        }


        //        }


        //        System.IO.File.Delete(path1);


        //    }


        //    return Redirect(Url.Action("Manage", "Dashboards", new RouteValueDictionary(new { id = 1 })));

           
        //}





        //[Authorize]
        //public ActionResult _ActiveAdministrators()
        //{


        //    var asset = (from x in context.UserProfiles
        //                 //join y in context.Organizations
        //                 //on x.OrganizationID equals y.OrganizationID
        //                 join z in context.RoleBins
        //                 on x.RoleBinID equals z.RoleBinID
        //                 where x.Active == true && x.RoleBinID == 1 

        //                 select new UserProfileViewModel()
        //                 {
        //                     UserId = x.UserId,
        //                     UserName = x.UserName,
        //                     FirstName = x.FirstName + " " + x.LastName,
        //                     PhoneNumber = x.PhoneNumber,
        //                     //OrganizationName = y.OrganizationName,
        //                     RoleBinName = z.RoleBinName


        //                 }).ToList();

        //    var UserCount = (from ast in context.UserProfiles
        //                     where ast.Active == true && ast.RoleBinID == 1
        //                     select ast.UserId).Count();

        //    ViewBag.UserCount = UserCount;


        //    return PartialView(asset);


        //}

        //[Authorize]
        //public ActionResult _ActiveStudents()
        //{


        //    var asset = (from x in context.UserProfiles
        //                 join y in context.Organizations
        //                 on x.OrganizationID equals y.OrganizationID
        //                 //join m in context.Evaluations
        //                 //on x.UserId equals m.UserId
        //                 join z in context.RoleBins
        //                 on x.RoleBinID equals z.RoleBinID
        //                 where x.Active == true && x.RoleBinID == 3

        //                 select new UserProfileViewModel()
        //                 {
        //                     UserId = x.UserId,
        //                     UserName = x.UserName,
        //                     FirstName = x.FirstName + " " + x.LastName,
        //                     PhoneNumber = x.PhoneNumber,
        //                     OrganizationName = y.OrganizationName,
        //                     RoleBinName = z.RoleBinName
        //                     //EvaluationID = m.EvaluationID

        //                 }).ToList();



        //    var StudentCount = (from ast in context.UserProfiles
        //                        join y in context.RoleBins on ast.RoleBinID equals y.RoleBinID
        //                        where ast.Active == true && ast.RoleBinID == 3
        //                        select ast.UserId).Count();

        //    ViewBag.StudentCount = StudentCount;



        //    return PartialView(asset);


        //}


        ////
        //// GET: /UserProfiles/Details/5

        //public ViewResult Details(int id)
        //{
        //    UserProfile userprofile = context.UserProfiles.Single(x => x.UserId == id);
        //    return View(userprofile);
        //}

        ////
        //// GET: /UserProfiles/Create

        //public ActionResult Create()
        //{

        //    var Q = from P in context.Organizations
        //            where P.Active == true
        //            select P;


        //    ViewBag.OrganizationID = new SelectList(Q, "OrganizationID", "OrganizationName");

        //    var J = from P in context.RoleBins
        //            where P.Active == true
        //            select P;


        //    ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName");



        //    return PartialView();
        //} 

        ////
        //// POST: /UserProfiles/Create

        //[HttpPost]
        //public ActionResult Create(UserProfile userprofile)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.UserProfiles.Add(userprofile);
        //        context.SaveChanges();
        //        return RedirectToAction("Index");  
        //    }

        //    var Q = from P in context.Organizations
        //            where P.Active == true
        //            select P;


        //    ViewBag.OrganizationID = new SelectList(Q, "OrganizationID", "OrganizationName", userprofile.OrganizationID);

        //    var J = from P in context.RoleBins
        //            where P.Active == true
        //            select P;


        //    ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", userprofile.RoleBinID);

        //    return View(userprofile);
        //}
        
        ////
        //// GET: /UserProfiles/Edit/5


        public ActionResult _Activate(string id)
        {

            string UserName1 = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.Id).SingleOrDefault();
           
            var Un = (from P in context.AspNetUsers
                     where P.Id == id
                     select P.UserName).SingleOrDefault();

            ViewBag.AUserID = id;
            ViewBag.AUserName = Un;


            var UuU = (from P in context.AspNetUsers
                      join R in context.RoleBins
                      on P.RoleBinID equals R.RoleBinID
                      where P.Id == id
                      select R).SingleOrDefault();

            ViewBag.RoleBinID = UuU.RoleBinID;
            ViewBag.RoleBinName = UuU.RoleBinName;


            var Ua = (from P in context.AspNetUsers
                      where P.Id == id
                      select P.Active).SingleOrDefault();

            ViewBag.Activate = Ua;

            var Uu = (from P in context.AspNetUsers
                      join R in context.RoleBins
                      on P.RoleBinID equals R.RoleBinID
                      where P.Id == U
                      select R).SingleOrDefault();

            ViewBag.Role = Uu.RoleBinID;
            ViewBag.RoleName = Uu.RoleBinName;

            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;

            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.OrganizationID).SingleOrDefault();

            //var Oname = (from P in context.Organizations
            //             where P.OrganizationID == O
            //             select P.OrganizationName).SingleOrDefault();


            ViewBag.UID = id;
            ViewBag.OID = O;
            //ViewBag.OName = Oname;


            ApplicationUser userprofile = context.AspNetUsers.Single(x => x.Id == id);


            ViewBag.DateUpdated = DateTime.Now;

            string UserName = @User.Identity.Name.ToString();

            ViewBag.UpdatedBy = UserName;

            ViewBag.DateCreated = userprofile.DateCreated;
            ViewBag.CreatedBy = userprofile.CreatedBy;
            ViewBag.firstTimeLoggedIn = userprofile.firstTimeLoggedIn;
            ViewBag.Status = userprofile.Status;



            return PartialView(userprofile);


        }



        public JsonResult _ActivateUserF(ApplicationUser auser)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

                var name = (from x in context.AspNetUsers
                            where x.UserName == UserName
                            select x.FirstName + " " + x.LastName).FirstOrDefault();

                if (auser.Active == false)
                {

                    //Flip IsApproved Flag
                    ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == auser.UserName);
                    userprofile1.Active = true;
                    userprofile1.EmailConfirmed = true;
                    userprofile1.UpdatedBy = name;
                    userprofile1.DateUpdated = CreatedInit;
                    userprofile1.LastPasswordChangedDate = CreatedInit;
                    context.SaveChanges();

                    // Send User Activation Email
                    SendActivateEmail(userprofile1.Email);

                     var programlist = (from x in context.AspNetUsers
                                             join y in context.SiteRoleProgramUserProfiles
                                             on x.Id equals y.Id
                                             where x.UserName == UserName
                                             select y.ProgramID).Distinct().ToList();

                     foreach (int p in programlist)
                     {

                         var adminlist = (from x in context.AspNetUsers
                                          join y in context.RoleProgramUserProfiles
                                          on x.Id equals y.Id
                                          where y.RoleID == 13 && y.ProgramID == p && y.Coordinator == true
                                          select x.Email).Distinct().ToList();

                         foreach (var aEmail in adminlist)
                         {

                             SendAdminNotifyEmail(aEmail, userprofile1.FirstName, userprofile1.LastName, userprofile1.Email, userprofile1.TelephoneNumber);


                         }


                     }


                }

                
                return Json(new { Status = "Success", Modified = auser.FirstName + " " + auser.LastName }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = auser.FirstName + " " + auser.LastName }, JsonRequestBehavior.AllowGet);


        }


        public void SendActivateEmail(string Email)
        {
            ////***************************************************************************
            // <summary>
            // SEND EMAIL
            // </summary>
            // <param name="eEmail">Email Address to Send</param>
            // <param name="eSubject">Subject of Email</param>
            // <param name="eMessage">Message to Email</param>
            ////***************************************************************************
            //Get User to Email
            // Dim cn As New Connection
            //Set Temporary Variables

            // // CREATE AND INSTATNCE OF THE MAIL MESSAGE CLASS //
            MailMessage mail = new MailMessage();
            //// SET THE PROPERTIES // 
            mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.To.Add(Email);
            //mail.CC.Add(eEmail);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Health Informatics Data Systems:" + " " + "Your account has been activated";

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Hello, your Colorado Health Informatics Data Systems account has been activated. Please click the link below to login!" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "<a target='_blank' href='https://www.healthinformatics.dphe.state.co.us'>Colorado Health Informatics Data Systems</a>" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Have you forgotten your password? click the following link: <a target='_blank' href='https://www.healthinformatics.dphe.state.co.us/Account/ForgotPassword'>Retrieve your password</a>" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<br>Thank you!.<br>Colorado Health Informatics Data Systems Support";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "</TABLE>";
            mail.IsBodyHtml = true;

            //UNCOMMENT THIS LINE TO EMAIL
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }


        public void SendAdminNotifyEmail(string aEmail, string FirstName, string LastName, string Email, string TelephoneNumber)
        {
            ////***************************************************************************
            // <summary>
            // SEND EMAIL
            // </summary>
            // <param name="eEmail">Email Address to Send</param>
            // <param name="eSubject">Subject of Email</param>
            // <param name="eMessage">Message to Email</param>
            ////***************************************************************************
            //Get User to Email
            // Dim cn As New Connection
            //Set Temporary Variables

            // // CREATE AND INSTATNCE OF THE MAIL MESSAGE CLASS //
            MailMessage mail = new MailMessage();
            //// SET THE PROPERTIES // 
            mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.To.Add(aEmail);
            //mail.CC.Add(eEmail);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Health Informatics Data Systems:" + " " + "The account for " + FirstName + " " + LastName + " has been activated";

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Hello, the account requested by:" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Name:" + FirstName + " " + LastName + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Email Address:" + Email + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Telephone Number:" + TelephoneNumber + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> Has been activated. " + "<a target='_blank' href='https://www.healthinformatics.dphe.state.co.us'>Colorado Health Informatics Data Systems</a>" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<br>Thank you!<br>Colorado Health Informatics Data Systems Support";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "</TABLE>";
            mail.IsBodyHtml = true;

            //UNCOMMENT THIS LINE TO EMAIL
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }



          public ActionResult _Assign(string id)
        {

            string UserName1 = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.Id).SingleOrDefault();
           
            var Un = (from P in context.AspNetUsers
                     where P.Id == id
                     select P.UserName).SingleOrDefault();

            ViewBag.AUserID = id;
            ViewBag.AUserName = Un;

           // ViewBag.PossibleOrganizationBins = context.Organizations.Where(x => x.Active == true);

            var Ua = (from P in context.AspNetUsers
                      where P.Id == id
                      select P.Active).SingleOrDefault();

            ViewBag.Activate = Ua;

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

            ViewBag.Role = Uu;

            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;

            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.OrganizationID).SingleOrDefault();

            //var Oname = (from P in context.Organizations
            //             where P.OrganizationID == O
            //             select P.OrganizationName).SingleOrDefault();


            ViewBag.UID = id;
            ViewBag.OID = O;
            //ViewBag.OName = Oname;


            ApplicationUser userprofile = context.AspNetUsers.Single(x => x.Id == id);


            ViewBag.DateUpdated = DateTime.Now;

            string UserName = @User.Identity.Name.ToString();

            ViewBag.UpdatedBy = UserName;

            ViewBag.DateCreated = userprofile.DateCreated;
            ViewBag.CreatedBy = userprofile.CreatedBy;
            ViewBag.firstTimeLoggedIn = userprofile.firstTimeLoggedIn;
            ViewBag.Status = userprofile.Status;



            return PartialView(userprofile);


        }



        public JsonResult _AssignUserF(ApplicationUser auser)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

                var name = (from x in context.AspNetUsers
                            where x.UserName == UserName
                            select x.FirstName + " " + x.LastName).FirstOrDefault();

                //if (auser.Active == false)
                //{

                    //Flip IsApproved Flag
                    ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == auser.UserName);
                    //userprofile1.Active = true;
                    userprofile1.OrganizationID = auser.OrganizationID;
                    userprofile1.UpdatedBy = name;
                    userprofile1.DateUpdated = CreatedInit;
                    context.SaveChanges();


                //}

                
                return Json(new { Status = "Success", Modified = auser.FirstName + " " + auser.LastName }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = auser.FirstName + " " + auser.LastName }, JsonRequestBehavior.AllowGet);


        }



        public async Task<ActionResult> _Profile(string id, string id2)
        {

           
            string UserName1 = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.Id).SingleOrDefault();

            var userId = User.Identity.GetUserId();
            ViewBag.UUID = userId;
            // Get User RoleID

            var Uz = (from P in context.AspNetUsers
                      join y in context.RoleBins
                      on P.RoleBinID equals y.RoleBinID
                     where P.Id == id
                     select y.RoleBinID).SingleOrDefault();
           
            ViewBag.UserRoleBinID = Uz;


            var pa = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.PrivacyAgreement).SingleOrDefault();

            ViewBag.PrivacyAgreement = pa;

            var ftl = (from P in context.AspNetUsers
                      where P.UserName == UserName1
                      select P.firstTimeLoggedIn).SingleOrDefault();

            ViewBag.FirstTime = ftl;

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

            ViewBag.Role = Uu;


            // Find out if User has any supplimental Roles

            var rolelist = (from x in context.AspNetUsers
                            join y in context.SiteRoleProgramUserProfiles
                            on x.Id equals y.Id
                            where x.UserName == UserName1
                            select y.RoleID).ToList();

            foreach (int r in rolelist)
            {

                // Site Administrator
                if (r == 6)
                {


                    ViewBag.Role = 6;

                }

                // Data Manager
                if (r == 13)
                {

                    ViewBag.Role = 13;

                }

                // Program Manager
                if (r == 3)
                {

                    ViewBag.Role = 3;


                }



            }



            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;

            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.OrganizationID).SingleOrDefault();

       
            ViewBag.OCount = O;

            ViewBag.UID = id;
            ViewBag.OID = O;
       


            var userprofile = (from x in context.AspNetUsers
                               //join y in context.Organizations
                               //on x.OrganizationID equals y.OrganizationID
                               //join dd in context.Organizations
                               //on x.OrganizationID equals dd.OrganizationID
                               //into ff
                               //from rr in ff.DefaultIfEmpty()
                               join z in context.RoleBins
                               on x.RoleBinID equals z.RoleBinID
                               //join u in context.PartnerTypeUserProfiles
                               //on x.UserId equals u.UserId
                               //join g in context.PartnerTypeBins
                               //on u.PartnerTypeBinID equals g.PartnerTypeBinID
                               where x.Id == id

                               select new UserProfileViewModel()
                               {
                                   Id = x.Id,
                                   UserName = x.UserName,
                                   FirstName = x.FirstName,
                                   LastName = x.LastName,
                                   MiddleName = x.MiddleName,
                                   TelephoneNumber = x.TelephoneNumber,
                                   profilePic = x.ProfilePic,
                                   Address = x.Address,
                                   ZipCode = x.ZipCode,
                                   Fax = x.Fax,
                                   OrganizationName = x.OrganizationName,
                                   RoleBinName = z.RoleBinName,
                                   RoleBinID = x.RoleBinID,
                                   City = x.City,
                                   
                                   //PrivacyAgreement = x.PrivacyAgreement,
                                   Active = x.Active,
                                   ExternalID = x.ExternalID,
                                   Status = x.Status,
                                   StateID = x.StateBinID,
                                   DateCreated = x.DateCreated,
                                   CreatedBy = x.CreatedBy



                               }).SingleOrDefault();

            ViewBag.UserId = id;


            var ULPC = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.LastPasswordChangedDate).SingleOrDefault();

            DateTime expDate = Convert.ToDateTime(ULPC).AddDays(90);
            // Check to see if user needs to reset password
            if ( expDate < DateTime.Now)
            {


                ViewBag.ChangePassword = true;
                string code = await UserManager.GeneratePasswordResetTokenAsync(id);
                ViewBag.Code = code;

            }
            else
            {

                ViewBag.ChangePassword = false;

            }
            //var Q = from P in context.Organizations
            //        where P.Active == true
            //        select P;


            //ViewBag.OrganizationID = new SelectList(Q, "OrganizationID", "OrganizationName", userprofile.OrganizationID);


            if (id2 != "")
            {

                var J = from P in context.RoleBins
                        where P.Active == true && P.RoleBinID == 2
                        select P;


                ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", userprofile.RoleBinID);


            }
            else
            {

                var J = from P in context.RoleBins
                        where P.Active == true && P.RoleBinID == 1 || P.RoleBinID == 2
                        select P;


                ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", userprofile.RoleBinID);


            }



            if (Uu == 1)
            {

                var J = from P in context.RoleBins
                        where P.Active == true && P.RoleBinID == 1 || P.RoleBinID == 2
                        select P;


                ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", userprofile.RoleBinID);


            }




            var status = (from P in context.AspNetUsers
                          where P.Id == id
                          select P.Status).SingleOrDefault();


            ViewBag.Status = status;


      
            var S = (from P in context.AspNetUsers
                     where P.Id == id
                     select P.FirstName).SingleOrDefault();


            ViewBag.FirstName = S;
            ViewBag.UserCheck = id2;


            var T = from P in contextH.DDStateBins
                    where P.Active == true
                    select P;


            ViewBag.StateID = new SelectList(T, "StateBinID", "StateBinName", userprofile.StateID);


            ViewBag.DateUpdated = DateTime.Now;

            string UserName = @User.Identity.Name.ToString();

            ViewBag.UpdatedBy = UserName;

            ViewBag.DateCreated = userprofile.DateCreated;
            ViewBag.CreatedBy = userprofile.CreatedBy;
            ViewBag.firstTimeLoggedIn = userprofile.firstTimeLoggedIn;
            ViewBag.Status = userprofile.Status;



            return PartialView(userprofile);


        }

        private byte[] ReadData(Stream stream)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult _UpdateProfilePic(UserProfileViewModel userprofile, HttpPostedFileBase file00, string Id)
        {
            if (ModelState.IsValid)
            {
                
                
                    string filename = Path.GetFileName(file00.FileName);
                    var fname = System.Web.HttpUtility.UrlPathEncode(filename);

                    string ScreeningGUID1 = Convert.ToString(DateTime.Now.Year);
                    string ScreeningGUID2 = Convert.ToString(DateTime.Now.Day);
                    string ScreeningGUID3 = Convert.ToString(DateTime.Now.Hour);
                    string ScreeningGUID4 = Convert.ToString(DateTime.Now.Minute);
                    string ScreeningGUID5 = Convert.ToString(DateTime.Now.Second);
                    string ScreeningGUID6 = Convert.ToString(DateTime.Now.Millisecond);
                    string ScreeningGUID7 = Convert.ToString(ScreeningGUID1) + Convert.ToString(ScreeningGUID2) + Convert.ToString(ScreeningGUID3) + Convert.ToString(ScreeningGUID4) + Convert.ToString(ScreeningGUID5) + Convert.ToString(ScreeningGUID6);
                    long ScreeningGUID = Convert.ToInt64(ScreeningGUID7);
                    ViewBag.ScreeningGuidID = ScreeningGUID;


                    string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Content/UploadedFolder"), ScreeningGUID + filename);
                    System.IO.File.WriteAllBytes(filePath, ReadData(file00.InputStream));


                    //Get User Id
                    ApplicationUser appuser = context.AspNetUsers.Single(
                        X => X.Id == Id);

                    //asset1.AssetContent = asset1.AssetContent + "<a href=" + "'" + filePath + "'" + " target='_blank'>" + Title + "</a>";


                    if ((Request.Url.GetLeftPart(UriPartial.Authority).ToString() == "https://www.healthinformatics.dphe.state.co.us"))
                    {


                        appuser.ProfilePic = "https://www.healthinformatics.dphe.state.co.us/Content/UploadedFolder/" + ScreeningGUID + fname;


                    }
                    else
                    {

                        appuser.ProfilePic = "https://www.train.healthinformatics.dphe.state.co.us/Content/UploadedFolder/" + ScreeningGUID + fname;

                    }
                
                context.SaveChanges();

                    return RedirectToAction("Edit", "UserProfiles", new { id = Id }); 
              
                }

              

            return View();


        }



        public async Task<JsonResult> _UpdateUserProfileF(UserProfileViewModel userprofileviewmodel, string id , string id2)
        {

            if (ModelState.IsValid)
            {

                // Change password
                if (userprofileviewmodel.Code != null)
                {

                    var userPC = await UserManager.FindByNameAsync(userprofileviewmodel.UserName);
                    if (userPC == null)
                    {
                        ModelState.AddModelError("", "No user found.");
                        //return PartialView();
                    }
                    IdentityResult result = await UserManager.ResetPasswordAsync(userprofileviewmodel.Id, userprofileviewmodel.Code, userprofileviewmodel.Password);
                    if (result.Succeeded)
                    {

                        DateTime CreatedInit = DateTime.Now;
                        //Update Profile Info
                        ApplicationUser userUP = context.AspNetUsers.Single(p => p.Id == userprofileviewmodel.Id);
                        userUP.LastPasswordChangedDate = CreatedInit;
                       
                        context.SaveChanges();


                        return Json(new { Status = "Success", Modified = id, Modified2 = id2 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //AddErrors(result);
                        //return PartialView();
                    }



                }

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();
                //var status = false;

                //if (userprofileviewmodel.PrivacyAgreement == true)
                //{


                //    status = true;


                //}
                //else
                //{


                //    status = false;

                //}


                //Update Profile Info
                ApplicationUser user = context.AspNetUsers.Single(p => p.Id == id);
                user.UserName = userprofileviewmodel.UserName;
                user.OrganizationName = userprofileviewmodel.OrganizationName;
                user.FirstName = userprofileviewmodel.FirstName;
                user.LastName = userprofileviewmodel.LastName;
                user.Address = userprofileviewmodel.Address;
                user.City = userprofileviewmodel.City;
                user.ZipCode = userprofileviewmodel.ZipCode;
                user.Fax = userprofileviewmodel.Fax;
                user.StateBinID = userprofileviewmodel.StateID;
                user.RoleBinID = userprofileviewmodel.RoleBinID;
                user.ExternalID = userprofileviewmodel.ExternalID;
                //user.Status = status;
                user.TelephoneNumber = userprofileviewmodel.TelephoneNumber;
                user.Active = userprofileviewmodel.Active;
                user.PrivacyAgreement = userprofileviewmodel.PrivacyAgreement;
                user.firstTimeLoggedIn = false;
                context.SaveChanges();


                string PN = user.PhoneNumber;

           
                return Json(new { Status = "Success", Modified = id , Modified2 = id2 }, JsonRequestBehavior.AllowGet);



            }


            return Json(new { Status = "Success", Modified = id, Modified2 = id2 }, JsonRequestBehavior.AllowGet);

        }


        [Authorize]
        public ActionResult Edit(string id, string id2)
        {


            if (!HttpContext.Request.IsAuthenticated)
            {


                return Redirect("https://www.healthinformatics.dphe.state.co.us/");

            }


            string UserName1 = @User.Identity.Name.ToString();


            // Check if Admin is Impersonating
            var userId = User.Identity.GetUserId();
            ViewBag.UUID = userId;
            ViewBag.UserName = userId;
            // First, see if ImpersonatorId is not null
            var impersonatorid = (from x in context.AspNetUsers
                                  where x.Id == userId
                                  select x.ImpersonatorId).FirstOrDefault();

            if (impersonatorid != null)
            {


                ViewBag.LoginUserCheck = true;



            }
            else
            {

                ViewBag.LoginUserCheck = false;

            }


      

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.Id).SingleOrDefault();
            var UFT = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.firstTimeLoggedIn).SingleOrDefault();
            ViewBag.FTLoggedIn = UFT;
            var UPIC = (from P in context.AspNetUsers
                        where P.UserName == UserName1
                        select P.ProfilePic).SingleOrDefault();

            ViewBag.ProfilePic = UPIC;

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();


            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;



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


            var pa = (from P in context.AspNetUsers
                      where P.UserName == UserName1
                      select P.PrivacyAgreement).SingleOrDefault();

            ViewBag.PrivacyAgreement = pa;

            var ftl = (from P in context.AspNetUsers
                       where P.UserName == UserName1
                       select P.firstTimeLoggedIn).SingleOrDefault();

            ViewBag.FirstTime = ftl;

            //var Uu = (from P in context.AspNetUsers
            //          where P.Id == U
            //          select P.RoleBinID).SingleOrDefault();

            //ViewBag.Role = Uu;


            //var active = (from ast in context.AspNetUsers
            //              where ast.Id == U
            //              select ast.Active).SingleOrDefault();

           // ViewBag.Active = active;

            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.OrganizationID).SingleOrDefault();

            //var Oname = (from P in context.Organizations
            //             where P.OrganizationID == O
            //             select P.OrganizationName).SingleOrDefault();


            ViewBag.OCount = O;

            ViewBag.UID = id;
            ViewBag.OID = O;
            //ViewBag.OName = Oname;

            var userprofile = (from x in context.AspNetUsers
                         //join y in context.Organizations
                         //on x.OrganizationID equals y.OrganizationID
                         //join dd in context.Organizations
                         //on x.OrganizationID equals dd.OrganizationID
                         //into ff
                         //from rr in ff.DefaultIfEmpty()
                         join z in context.RoleBins
                         on x.RoleBinID equals z.RoleBinID
                         //join u in context.PartnerTypeUserProfiles
                         //on x.UserId equals u.UserId
                         //join g in context.PartnerTypeBins
                         //on u.PartnerTypeBinID equals g.PartnerTypeBinID
                         where x.Id == id

                         select new UserProfileViewModel()
                         {
                             Id = x.Id,
                             UserName = x.UserName,
                             FirstName = x.FirstName,
                             LastName = x.LastName,
                             TelephoneNumber = x.PhoneNumber,
                             profilePic = x.ProfilePic,
                             OrganizationName = x.OrganizationName,
                             RoleBinName = z.RoleBinName,
                             //PrivacyAgreement = Convert.ToBoolean(x.PrivacyAgreement),
                             Active = x.Active,
                             Status = x.Status,
                             StateID = x.StateBinID,
                             DateCreated = x.DateCreated,
                             CreatedBy = x.CreatedBy



                         }).SingleOrDefault();


            ViewBag.UserId = id;

        
            return View(userprofile);


        }

        //
        // POST: /UserProfiles/Edit/5

        //[HttpPost]
        //public ActionResult Edit(UserProfile userprofile, string id2)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Entry(userprofile).State = EntityState.Modified;
        //        context.SaveChanges();

        //        bool? activecheck = userprofile.Active;
        //        string UserName = userprofile.UserName;
        //        bool? Status = userprofile.Status;

        //        if (activecheck == true && Status == false)
        //        {


        //            ForgotPassword(UserName);

        //            //Flip IsApproved Flag
        //            UserProfile userprofile1 = context.UserProfiles.Single(p => p.UserName == UserName);
        //            userprofile1.Status = true;
        //            context.SaveChanges();

        //        }


        //        if (id2 != "")
        //        {

        //            return Redirect(Url.Action("UserAccount", "Dashboards", new RouteValueDictionary(new { id = userprofile.UserId })));



        //        }
        //        else
        //        {

        //            return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }) + "#" + "tabs-4Home");

        //        }






        //        //return RedirectToAction("Index","Requests");
        //    }


        //    var Q = from P in context.Organizations
        //            where P.Active == true
        //            select P;


        //    ViewBag.OrganizationID = new SelectList(Q, "OrganizationID", "OrganizationName", userprofile.OrganizationID);

        //    var J = from P in context.RoleBins
        //            where P.Active == true
        //            select P;


        //    ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", userprofile.RoleBinID);



        //    return View(userprofile);
        //}


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ForgotPassword(string UserName)
        //{
        //    //check user existance
        //    var user = Membership.GetUser(UserName);
        //    if (user == null)
        //    {
        //        TempData["Message"] = "User Not exist.";
        //    }
        //    else
        //    {
        //        //generate password token
        //        var token = WebSecurity.GeneratePasswordResetToken(UserName);
        //        //create url with above token
        //        var resetLink = "<a href='" + Url.Action("ResetPassword", "Account", new { un = UserName, rt = token }, "http") + "'>Reset Password</a>";
        //        //get user emailid
        //        UsersContext db = new UsersContext();
        //        var emailid = (from i in db.UserProfiles
        //                       where i.UserName == UserName
        //                       select i.UserName).FirstOrDefault();
        //        //send mail
        //        string subject = "Password Reset Token";
        //        string body = "<b>Please find the Password Reset Token</b><br/>" + resetLink; //edit it
        //        try
        //        {
        //            SendEMail(emailid, subject, body);
        //            TempData["Message"] = "Mail Sent.";
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["Message"] = "Error occured while sending email." + ex.Message;
        //        }
        //        //only for testing
        //        //TempData["Message"] = resetLink;
        //    }

        //    return View();
        //}



        //private void SendEMail(string emailid, string subject, string body)
        //{


        //    MailMessage msg = new MailMessage();
        //    msg.From = new MailAddress("criticalelement@msn.com");
        //    msg.To.Add(new MailAddress(emailid));


        //    msg.Subject = subject;
        //    msg.IsBodyHtml = true;
        //    msg.Body = body;


        //    //UNCOMMENT THIS LINE TO EMAIL
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.Send(msg);
        //}







        //
        // GET: /UserProfiles/Delete/5

        public ActionResult Delete(string id)
        {
            ApplicationUser userprofile = context.AspNetUsers.Single(x => x.Id == id);
            return PartialView(userprofile);

        }


        public JsonResult _DeleteUserF(ApplicationUser appUser)
        {

            if (ModelState.IsValid)
            {

                ApplicationUser appUser1 = context.AspNetUsers.Single(x => x.Id == appUser.Id);
                context.AspNetUsers.Remove(appUser1);
                context.SaveChanges();

                string a = appUser1.FirstName + " " + appUser1.LastName;

                return Json(new { Status = "Success", Modified = a, Modified2 = appUser.Id }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = appUser.FirstName + " " + appUser.LastName, Modified2 = appUser.Id }, JsonRequestBehavior.AllowGet);


        }



        ////
        //// POST: /UserProfiles/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    UserProfile userprofile = context.UserProfiles.Single(x => x.UserId == id);
        //    context.UserProfiles.Remove(userprofile);
        //    context.SaveChanges();

        //    return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }) + "#" + "tabs-4Home");

        //    //return RedirectToAction("Index","Requests");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}