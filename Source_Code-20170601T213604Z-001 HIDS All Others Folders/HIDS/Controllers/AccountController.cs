using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using System.Web.Helpers;
using System.Net;
using System.Web.Mvc;
using System.Collections;
using System.Net.Mail;
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
    public class AccountController : Controller 
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private HIADMINEntities db = new HIADMINEntities();
        private ApplicationUserManager _userManager;
        

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

          
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        [AllowAnonymous]
        public ActionResult LoginInline(string returnUrl)
        {


            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

               // var user = await UserManager.FindAsync(model.Email, model.Password);
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var validCredentials = await UserManager.FindAsync(model.Email, model.Password);


                    if (validCredentials != null)
                    {

                        //Track UserLogin

                        var userLON =
                                (from c in context.AspNetUsers
                                 where c.UserName == model.Email
                                 select c).First();

                        userLON.CurrentlyLoggedIn = true;
                        userLON.AccessFailedCount = 0;
                        context.SaveChanges();




                    }

                    // Check if User is Active
                    var userCH =
                       (from c in context.AspNetUsers
                        where c.UserName == model.Email
                        select c).First();

                    if (userCH.Active == false)
                    {

                        return RedirectToAction("AccountPending", "Account");

                    }
                    // When a user is lockedout, this check is done to ensure that even if the credentials are valid
                    // the user can not login until the lockout duration has passed
                    if (await UserManager.IsLockedOutAsync(userCH.Id))
                    {


                        // COMMENTED OUT FOR TRAIN 
                        ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));
                    
                    
                    }
                    // if user is subject to lockouts and the credentials are invalid
                    // record the failure and check if user is lockedout and display message, otherwise,
                    // display the number of attempts remaining before lockout
                    else if (await UserManager.GetLockoutEnabledAsync(userCH.Id) && validCredentials == null)
                    {
                        
                        
                        // Record the failure which also may cause the user to be locked out

                        
                        
                        // COMMENTED OUT FOR TRAIN 
                        await UserManager.AccessFailedAsync(userCH.Id);

                        string message;

                        if (await UserManager.IsLockedOutAsync(userCH.Id))
                        {
                            message = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());
                        }
                        else
                        {
                            int accessFailedCount = await UserManager.GetAccessFailedCountAsync(userCH.Id);

                            int attemptsLeft =
                                Convert.ToInt32(
                                    ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString()) -
                                accessFailedCount;

                            message = string.Format(
                                "Invalid credentials. You have {0} more attempt(s) before your account gets locked out.", attemptsLeft);

                        }
                        ModelState.AddModelError("", message);





                    }
                    else if (validCredentials == null)
                    {
                        ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    }
                    else
                    {

                        await SignInAsync(user, model.RememberMe);
                        //return RedirectToLocal(returnUrl);

                        DateTime expDate = Convert.ToDateTime(userCH.LastPasswordChangedDate).AddDays(90);
                    if (expDate < DateTime.Now)
                    {

                        // Send to Profile, keep there until password changed
                        return RedirectToAction("Edit", "UserProfiles", new { id = userCH.Id });

                    }



                        // Check if first time logged in

                        var U = (from P in context.AspNetUsers
                                 where P.UserName == model.Email
                                 select P).SingleOrDefault();

                        if (U.firstTimeLoggedIn == true)
                        {
                           
                        if (U.LegacyId != null)
                        {


                            var guid = new Guid(U.LegacyId);


                            // get sitelist and loop through

                            var sitelist = (from P in db.UserProfiles
                                            where P.UserID == guid
                                            select P.SiteAccess).SingleOrDefault();


                            // Write sitelist to table for historical purposes
                            DateTime CreatedInit = DateTime.Now;

                            var userL =
                            (from c in context.AspNetUsers
                             where c.UserName == model.Email
                             select c).First();


                            userL.SiteAccess = sitelist;
                            userL.firstTimeLoggedIn = false;
                            userL.LastPasswordChangedDate = CreatedInit;
                            context.SaveChanges();

                            // if a power user, do nothing
                            if (U.RoleBinID == 2)
                            {


                                if (sitelist != null)
                                {


                                    // SiteAccess
                                    var stringToSplit = sitelist;

                                    var query = from val in stringToSplit.Split(',')
                                                select Convert.ToInt32(val);
                                    foreach (int val in query)
                                    {



                                        // Transform old site id to new site id

                                        var newsiteid = (from P in context.Sites
                                                         join T in context.AgencySiteProgramSites
                                                         on P.SiteID equals T.SiteID
                                                         where T.LegacySiteID == val
                                                         select P).SingleOrDefault();


                                        if (newsiteid != null)
                                        {


                                            var programid = (from P in context.Sites
                                                             join T in context.AgencySiteProgramSites
                                                             on P.SiteID equals T.SiteID
                                                             where T.LegacySiteID == val
                                                             select T).SingleOrDefault();


                                            SiteRoleProgramUserProfiles upss = new SiteRoleProgramUserProfiles
                                            {


                                                SiteID = newsiteid.SiteID,
                                                Id = U.Id,
                                                RoleID = 2,
                                                ProgramID = Convert.ToInt32(programid.ProgramID),
                                                Training = false


                                            };

                                            context.SiteRoleProgramUserProfiles.Add(upss);
                                            context.SaveChanges();


                                        }


                                    }

                                }


                            }
                            else
                            {

                                // Set Power Users to Program Managers, then we will manually set Data Managers

                                // Add user to RoleProgramUserProfiles Table by program



                                // Since old portal did not set program for Power Users, set all to HIDS then manual change
                                RoleProgramUserProfiles rpu = new RoleProgramUserProfiles
                                {


                                    RoleID = Convert.ToInt32(U.RoleBinID),
                                    ProgramID = 43,
                                    Id = U.Id,
                                    Coordinator = false


                                };

                                context.RoleProgramUserProfiles.Add(rpu);
                                context.SaveChanges();





                            }



                            }


                        if (U.PrivacyAgreement != true)
                        {

                            return RedirectToImportUser(returnUrl, U.Id);

                        }
                        else
                        {

                            return RedirectToLocal(returnUrl);

                        }


                       
                        }
                        else
                        {



                            //Check to make sure impersonation is not active

                            var imCheck = (from P in context.AspNetUsers
                                       where P.UserName == U.UserName
                                       select P.ImpersonationId).SingleOrDefault();


                            if (imCheck != null)
                            {

                                // Remove impersonation from user
                                ApplicationUser userprofile0 = context.AspNetUsers.Single(p => p.Id == imCheck);
                                userprofile0.ImpersonatorId = null;
                                context.SaveChanges();


                                // Then remove impersonation
                                ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == U.UserName);
                                userprofile1.ImpersonationId = null;
                                context.SaveChanges();

                            }


                            if (U.PrivacyAgreement != true)
                            {

                                return RedirectToImportUser(returnUrl, U.Id);

                            }
                            else
                            {



                                if (userCH.Dashboard == false)
                                {



                                    return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });



                                }
                                else
                                {



                                    return RedirectToLocal(returnUrl);


                                }





                            }


                        }
                        if (U.RoleBinID == 1)
                        {


                            if (userCH.Dashboard == false)
                            {



                                return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });



                            }
                            else
                            {




                                return RedirectToLocal(returnUrl);



                            }



                        }
                        //else if (U.RoleBinID == 3)
                        //{


                        //    return RedirectToPartnerUser(returnUrl, U.Id);


                        //}
                        else
                        {

                            if (U.Active == false)
                            {


                                if (U.Status == false)
                                {


                                    return RedirectToImportUser(returnUrl, U.Id);

                                }

                                else
                                {

                                    return RedirectToPartnerUser(returnUrl, U.Id);

                                }


                            }
                            else
                            {

                                // return RedirectToLocalUser(returnUrl, U.Id);

                                // Check if User set to go directly to application

                                if (userCH.Dashboard == false)
                                {

                       

                               return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });

                                

                                }
                                else
                                {

                                    return RedirectToLocal(returnUrl);

                                }

                                

                            }




                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                   
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginInline(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                // var user = await UserManager.FindAsync(model.Email, model.Password);
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var validCredentials = await UserManager.FindAsync(model.Email, model.Password);


                    if (validCredentials != null)
                    {

                        //Track UserLogin

                        var userLON =
                                (from c in context.AspNetUsers
                                 where c.UserName == model.Email
                                 select c).First();

                        userLON.CurrentlyLoggedIn = true;
                        userLON.AccessFailedCount = 0;
                        context.SaveChanges();




                    }

                    // Check if User is Active
                    var userCH =
                       (from c in context.AspNetUsers
                        where c.UserName == model.Email
                        select c).First();

                    if (userCH.Active == false)
                    {

                        return RedirectToAction("AccountPending", "Account");

                    }
                    // When a user is lockedout, this check is done to ensure that even if the credentials are valid
                    // the user can not login until the lockout duration has passed
                    if (await UserManager.IsLockedOutAsync(userCH.Id))
                    {


                        // COMMENTED OUT FOR TRAIN 
                        ModelState.AddModelError("", string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString()));


                    }
                    // if user is subject to lockouts and the credentials are invalid
                    // record the failure and check if user is lockedout and display message, otherwise,
                    // display the number of attempts remaining before lockout
                    else if (await UserManager.GetLockoutEnabledAsync(userCH.Id) && validCredentials == null)
                    {


                        // Record the failure which also may cause the user to be locked out



                        // COMMENTED OUT FOR TRAIN 
                        await UserManager.AccessFailedAsync(userCH.Id);

                        string message;

                        if (await UserManager.IsLockedOutAsync(userCH.Id))
                        {
                            message = string.Format("Your account has been locked out for {0} minutes due to multiple failed login attempts.", ConfigurationManager.AppSettings["DefaultAccountLockoutTimeSpan"].ToString());
                        }
                        else
                        {
                            int accessFailedCount = await UserManager.GetAccessFailedCountAsync(userCH.Id);

                            int attemptsLeft =
                                Convert.ToInt32(
                                    ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"].ToString()) -
                                accessFailedCount;

                            message = string.Format(
                                "Invalid credentials. You have {0} more attempt(s) before your account gets locked out.", attemptsLeft);

                        }
                        ModelState.AddModelError("", message);





                    }
                    else if (validCredentials == null)
                    {
                        ModelState.AddModelError("", "Invalid credentials. Please try again.");
                    }
                    else
                    {

                        await SignInAsync(user, model.RememberMe);
                        //return RedirectToLocal(returnUrl);

                        DateTime expDate = Convert.ToDateTime(userCH.LastPasswordChangedDate).AddDays(90);
                        if (expDate < DateTime.Now)
                        {

                            // Send to Profile, keep there until password changed
                            return RedirectToAction("Edit", "UserProfiles", new { id = userCH.Id });

                        }



                        // Check if first time logged in

                        var U = (from P in context.AspNetUsers
                                 where P.UserName == model.Email
                                 select P).SingleOrDefault();

                        if (U.firstTimeLoggedIn == true)
                        {

                            if (U.LegacyId != null)
                            {


                                var guid = new Guid(U.LegacyId);


                                // get sitelist and loop through

                                var sitelist = (from P in db.UserProfiles
                                                where P.UserID == guid
                                                select P.SiteAccess).SingleOrDefault();


                                // Write sitelist to table for historical purposes
                                DateTime CreatedInit = DateTime.Now;

                                var userL =
                                (from c in context.AspNetUsers
                                 where c.UserName == model.Email
                                 select c).First();


                                userL.SiteAccess = sitelist;
                                userL.firstTimeLoggedIn = false;
                                userL.LastPasswordChangedDate = CreatedInit;
                                context.SaveChanges();

                                // if a power user, do nothing
                                if (U.RoleBinID == 2)
                                {


                                    if (sitelist != null)
                                    {


                                        // SiteAccess
                                        var stringToSplit = sitelist;

                                        var query = from val in stringToSplit.Split(',')
                                                    select Convert.ToInt32(val);
                                        foreach (int val in query)
                                        {



                                            // Transform old site id to new site id

                                            var newsiteid = (from P in context.Sites
                                                             join T in context.AgencySiteProgramSites
                                                             on P.SiteID equals T.SiteID
                                                             where T.LegacySiteID == val
                                                             select P).SingleOrDefault();


                                            if (newsiteid != null)
                                            {


                                                var programid = (from P in context.Sites
                                                                 join T in context.AgencySiteProgramSites
                                                                 on P.SiteID equals T.SiteID
                                                                 where T.LegacySiteID == val
                                                                 select T).SingleOrDefault();


                                                SiteRoleProgramUserProfiles upss = new SiteRoleProgramUserProfiles
                                                {


                                                    SiteID = newsiteid.SiteID,
                                                    Id = U.Id,
                                                    RoleID = 2,
                                                    ProgramID = Convert.ToInt32(programid.ProgramID),
                                                    Training = false


                                                };

                                                context.SiteRoleProgramUserProfiles.Add(upss);
                                                context.SaveChanges();


                                            }


                                        }

                                    }


                                }
                                else
                                {

                                    // Set Power Users to Program Managers, then we will manually set Data Managers

                                    // Add user to RoleProgramUserProfiles Table by program



                                    // Since old portal did not set program for Power Users, set all to HIDS then manual change
                                    RoleProgramUserProfiles rpu = new RoleProgramUserProfiles
                                    {


                                        RoleID = Convert.ToInt32(U.RoleBinID),
                                        ProgramID = 43,
                                        Id = U.Id,
                                        Coordinator = false


                                    };

                                    context.RoleProgramUserProfiles.Add(rpu);
                                    context.SaveChanges();





                                }



                            }


                            if (U.PrivacyAgreement != true)
                            {

                                return RedirectToImportUser(returnUrl, U.Id);

                            }
                            else
                            {

                                return RedirectToLocal(returnUrl);

                            }



                        }
                        else
                        {



                            //Check to make sure impersonation is not active

                            var imCheck = (from P in context.AspNetUsers
                                           where P.UserName == U.UserName
                                           select P.ImpersonationId).SingleOrDefault();


                            if (imCheck != null)
                            {

                                // Remove impersonation from user
                                ApplicationUser userprofile0 = context.AspNetUsers.Single(p => p.Id == imCheck);
                                userprofile0.ImpersonatorId = null;
                                context.SaveChanges();


                                // Then remove impersonation
                                ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.UserName == U.UserName);
                                userprofile1.ImpersonationId = null;
                                context.SaveChanges();

                            }


                            if (U.PrivacyAgreement != true)
                            {

                                return RedirectToImportUser(returnUrl, U.Id);

                            }
                            else
                            {



                                if (userCH.Dashboard == false)
                                {



                                    return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });



                                }
                                else
                                {



                                    return RedirectToLocal(returnUrl);


                                }





                            }


                        }
                        if (U.RoleBinID == 1)
                        {


                            if (userCH.Dashboard == false)
                            {



                                return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });



                            }
                            else
                            {




                                return RedirectToLocal(returnUrl);



                            }



                        }
                        //else if (U.RoleBinID == 3)
                        //{


                        //    return RedirectToPartnerUser(returnUrl, U.Id);


                        //}
                        else
                        {

                            if (U.Active == false)
                            {


                                if (U.Status == false)
                                {


                                    return RedirectToImportUser(returnUrl, U.Id);

                                }

                                else
                                {

                                    return RedirectToPartnerUser(returnUrl, U.Id);

                                }


                            }
                            else
                            {

                                // return RedirectToLocalUser(returnUrl, U.Id);

                                // Check if User set to go directly to application

                                if (userCH.Dashboard == false)
                                {



                                    return RedirectToAction("RedirectToApp", "Account", new { id = userCH.Id });



                                }
                                else
                                {

                                    return RedirectToLocal(returnUrl);

                                }



                            }




                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");

                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult RedirectToApp(string id)
        {

            var userCH =
                      (from c in context.AspNetUsers
                       where c.Id == id
                       select c).First();

            var appLocation = (from c in context.Applications
                               join y in context.AspNetUsers
                               on c.ApplicationID equals y.CurrentApplicationID
                               where c.ApplicationID == userCH.CurrentApplicationID
                               select c.HTTP).First();


            return Content("<script>window.location = '" + appLocation + "';</script>");


           
        }



        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register(string id)
        {

            ViewBag.UserId = id;
            DateTime Created = DateTime.Now;

            ViewBag.DateCreated = Created;
           
            ViewBag.DateUpdated = Created;

            //string temp = Created.ToString();
            //System.Web.HttpContext.Current.Session.Add("UID", temp);
            //var svuid = System.Web.HttpContext.Current.Session["UID"].ToString();
            //ViewBag.UID = svuid;

            IEnumerable OptionF = context.Programs.Where(c => c.Active == true).OrderBy(x => x.ProgramName).AsEnumerable().Select(c => new SelectListItem()

            {
                
                Text = c.ProgramName,
                Value = c.ProgramID.ToString(),
                Selected = true,
            });


            var NewtermsF = new SelectList(OptionF, "Value", "Text");
            ViewBag.OptionListF = NewtermsF;


       
            return View();


        }


        [AllowAnonymous]
        public ActionResult _ProgramPartial(string id, string id2)
        {

            ViewBag.AgencyCheck = id;


            if (id != "")
            {

                //IEnumerable OptionF = new List<Program>();
                //List<string> stringList = new List<string>();
                //IEnumerable<string> stringEnumerable = stringList.AsEnumerable();
                List<SelectListItem> OptionF = new List<SelectListItem>();
               // var sitelist = new List<Site>();
                var programlist = new List<Program>();
                var stringToSplit2 = id;

                // Loop to get list of sites
                var queryT2 = from val in stringToSplit2.Split(',')
                              select Convert.ToInt32(val);
                foreach (int val in queryT2)
                {

                    // Build Program List
                    var allprogramlist = (from x in context.Programs
                                          join y in context.AgencySiteProgramSites
                                          on x.ProgramID equals y.ProgramID
                                          where y.AgencySiteID == val && x.Active == true
                                          select x).Distinct().ToList();

                    //var topagencysite = (from x in context.Sites
                    //  join y in context.AgencySiteProgramSites
                    //  on x.SiteID equals y.SiteID
                    //  where y.AgencySiteID == val
                    //  select y.SiteID).First();

                    //int s = Convert.ToInt32(topagencysite);

                    foreach (Program pr in allprogramlist)
                    {

                        bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(pr.ProgramID));

                        if (alreadyExists == false)
                        {


                            OptionF.Add(new SelectListItem { Text = pr.ProgramName, Value = Convert.ToString(pr.ProgramID) });

                            //var programs = (from x in context.Programs
                            //                join y in context.AgencySiteProgramSites
                            //                on x.ProgramID equals y.ProgramID
                            //                where x.Active == true && y.ProgramID == pr.ProgramID
                            //                select x).ToList();


                            //foreach (Program p in programs)
                            //{


                            //    bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(p.ProgramID));

                            //    if (alreadyExists == false)
                            //    {

                            //        OptionF.Add(new SelectListItem { Text = p.ProgramName, Value = Convert.ToString(p.ProgramID) });

                            //    }

                            //}


                        }

                    }


                }


              // Loop through Sites to get programs added to multiselect

             //   foreach (Site si in sitelist)
              //  {

                    //var programs = (from x in context.Programs
                    //                    join y in context.ProgramSites
                    //                    on x.ProgramID equals y.ProgramID
                    //                    where x.Active == true && y.SiteID == si.SiteID
                    //                    select x).ToList();
                   // var programs = (from x in context.Programs
                   //                 join y in context.AgencySiteProgramSites
                   //                 on x.ProgramID equals y.ProgramID
                   //                 where x.Active == true && y.SiteID == si.SiteID
                   //                 select x).ToList();


                   // foreach (Program p in programs)
                   // {


                   //     bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(p.ProgramID));

                   //     if (alreadyExists == false)
                   //     {

                   //         OptionF.Add(new SelectListItem { Text = p.ProgramName, Value = Convert.ToString(p.ProgramID) });

                   //     }

                   //}


            //    }

                var NewtermsF = new SelectList(OptionF, "Value", "Text");
                ViewBag.OptionListF = NewtermsF;

            }
            else
            {

                ViewBag.OptionListF = null;

            }

            //var topsitecount = (from x in context.Sites
            //               join y in context.LegacySiteProgramSites
            //               on x.SiteID equals y.SiteID
            //               where y.AgencySiteID == id
            //               select y.SiteID).Count();


            //if (topsitecount > 0)
            //{

            //    var topsite = (from x in context.Sites
            //                   join y in context.LegacySiteProgramSites
            //                   on x.SiteID equals y.SiteID
            //                   where y.AgencySiteID == id
            //                   select y.SiteID).First();

            //    if (topsite > 0)
            //    {


            //        IEnumerable OptionF = context.Programs.Join(context.ProgramSites, c => c.ProgramID, p => p.ProgramID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.SiteID == topsite).OrderBy(z => z.c.ProgramName).AsEnumerable().Select(z => new SelectListItem()

            //        {

            //            Text = z.c.ProgramName,
            //            Value = z.c.ProgramID.ToString(),
            //            Selected = true,
            //        });


            //       var NewtermsF = new SelectList(OptionF, "Value", "Text");
            //        ViewBag.OptionListF = NewtermsF;


            //    }
            //}
            //else
            //{

               

            //    ViewBag.OptionListF = null;


            //}




            var medoutFMW = (from x in context.AspNetUsers
                             join y in context.SiteRoleProgramUserProfiles
                             on x.Id equals y.Id
                             where x.Id == id2
                             select y.ProgramID).Count();

            if (medoutFMW > 0)
            {


                var rowsFMW =
           (from x in context.AspNetUsers
            join y in context.SiteRoleProgramUserProfiles
            on x.Id equals y.Id
            where x.Id == id2
            select y.ProgramID).Distinct().AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.Options = rowsFMW;

            }


            return PartialView();

        }

        [AllowAnonymous]
        public ActionResult _Step1Registration(int? id, string id2)
        {



       
            ViewBag.UserId = id2;
            DateTime Created = DateTime.Now;
            ViewBag.DateCreated = Created;
            ViewBag.DateUpdated = Created;


            int[] ids = new int [] {3,13,39};
            var J = (from P in context.RoleBins
                     join H in context.ApplicationRolePrograms
                     on P.RoleBinID equals H.RoleID
                     where P.Active == true && ids.Contains(P.RoleBinID)
                     select P).Distinct();


            ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName");


            if (id2 == "")
            {

                List<SelectListItem> OptionAS = new List<SelectListItem>();

                var agencies = (from P in context.AgencySites
                         where P.Active == true
                         select P).OrderBy(x => x.AgencySiteName).ToList();

                foreach (AgencySite ags in agencies)
                {


                    var agency = (from x in context.AgencySites
                                   where x.Active == true && x.AgencySiteID == ags.AgencySiteID
                                   select x).SingleOrDefault();


                    //foreach (SelectListItem oas in OptionAS)
                    //{


                        bool alreadyExists = OptionAS.Any(x => x.Value == Convert.ToString(agency.AgencySiteID));

                        if (alreadyExists == false)
                        {

                            OptionAS.Add(new SelectListItem { Text = agency.AgencySiteName, Value = Convert.ToString(agency.AgencySiteID) });

                        }

                    //}




                }
               
                //IEnumerable OptionAS = context.AgencySites.Join(context.LegacySiteProgramSites, c => c.AgencySiteID, p => p.AgencySiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.AgencySiteID != null).OrderBy(z => z.c.AgencySiteName).AsEnumerable().Distinct().Select(z => new SelectListItem()

                //{

                //    Text = z.c.AgencySiteName,
                //    Value = z.c.AgencySiteID.ToString(),
                //    Selected = true,
                //});


                var NewtermsAS = new SelectList(OptionAS, "Value", "Text");
                ViewBag.OptionListAS = NewtermsAS;






            }
            else
            {

                var R = (from P in context.AspNetUsers
                         join X in context.RoleBins
                         on P.RoleBinID equals X.RoleBinID
                         where P.Id == id2
                         select X).SingleOrDefault();

                ViewBag.UserRoleBinID = R.RoleBinID;


                if (R.RoleBinID == 3 || R.RoleBinID == 13 || R.RoleBinID == 39)
                {


                    var coord = (from x in context.AspNetUsers
                                  join y in context.RoleProgramUserProfiles
                                  on x.Id equals y.Id
                                  where x.Id == id2
                                  select y.Coordinator).First();

                    ViewBag.Coordinator = coord;


                    var roleid = (from x in context.AspNetUsers
                                    join y in context.RoleProgramUserProfiles
                                    on x.Id equals y.Id
                                    where x.Id == id2
                                    select y.RoleID).First();


                    int[] idsJ = new int[] { 3, 13, 39 };
                    var JJ = (from P in context.RoleBins
                             join H in context.ApplicationRolePrograms
                             on P.RoleBinID equals H.RoleID
                             where P.Active == true && idsJ.Contains(P.RoleBinID)
                             select P).Distinct();


                    ViewBag.RoleBinID = new SelectList(JJ, "RoleBinID", "RoleBinName", roleid);



                   

                    var medoutFMW = (from x in context.AspNetUsers
                                     join y in context.RoleProgramUserProfiles
                                     on x.Id equals y.Id
                                     where x.Id == id2
                                     select y.ProgramID).Count();

                    if (medoutFMW > 0)
                    {


                        var rowsFMW =
                   (from x in context.AspNetUsers
                    join y in context.RoleProgramUserProfiles
                    on x.Id equals y.Id
                    where x.Id == id2
                    select y.ProgramID).Distinct().AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                        ViewBag.Options = rowsFMW;

                    }



                }
                else
                {

                    List<SelectListItem> OptionAS = new List<SelectListItem>();

                    var agencies0 = (from P in context.AgencySites
                                    where P.Active == true
                                    select P).OrderBy(x => x.AgencySiteName).ToList();

                    foreach (AgencySite ags in agencies0)
                    {


                        var agency = (from x in context.AgencySites
                                      where x.Active == true && x.AgencySiteID == ags.AgencySiteID
                                      select x).SingleOrDefault();


                        //foreach (SelectListItem oas in OptionAS)
                        //{


                        bool alreadyExists = OptionAS.Any(x => x.Value == Convert.ToString(agency.AgencySiteID));

                        if (alreadyExists == false)
                        {

                            OptionAS.Add(new SelectListItem { Text = agency.AgencySiteName, Value = Convert.ToString(agency.AgencySiteID) });

                        }

                        //}




                    }

                 


                    var NewtermsAS = new SelectList(OptionAS, "Value", "Text");
                    ViewBag.OptionListAS = NewtermsAS;





                    // Loop through Sites to get Agencies and populate multiselect

                    var agencies = new List<AgencySite>();
                    var sitelistAS = (from x in context.AspNetUsers
                                    join y in context.SiteRoleProgramUserProfiles
                                    on x.Id equals y.Id
                                    join w in context.Sites
                                    on y.SiteID equals w.SiteID
                                    where y.Id == id2
                                    select w).Distinct().ToList();

                    foreach (Site si in sitelistAS)
                    {

                        var agency = (from x in context.AgencySites
                                      join u in context.AgencySiteProgramSites
                                      on x.AgencySiteID equals u.AgencySiteID
                                      where x.Active == true && u.SiteID == si.SiteID
                                      select x).First();


                         bool alreadyExists = agencies.Any(x => x.AgencySiteID == agency.AgencySiteID);

                         if (alreadyExists == false)
                         {

                             agencies.Add(agency);


                         }

                    }


                    if (agencies.Count() > 0)
                    {


                    var rowsAS = (from x in agencies
                    select x.AgencySiteID).Distinct().AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                        ViewBag.OptionsAS = rowsAS;

                    }



                }


            }


            


            IEnumerable OptionF = context.Programs.Where(c => c.Active == true ).OrderBy(c => c.ProgramName).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.ProgramName,
                Value = c.ProgramID.ToString(),
                Selected = true,
            });


            var NewtermsF = new SelectList(OptionF, "Value", "Text");
            ViewBag.OptionListF = NewtermsF;
        


            return PartialView();


        }

        [AllowAnonymous]
        public ActionResult _Step2Registration(string id, int? id2, int? id3, int? id4, int? id5, int? id6, string id7)
        {

            ViewBag.Id = id;
            ViewBag.ResultsCCount = id3;
            ViewBag.AgencySiteID = id6;
            ViewBag.AgencySiteAccess = id7;
            //Get Program List
            var programlist =
            (from c in context.AspNetUsers
             where c.Id == id
             select c.ProgramAccess).FirstOrDefault();

            if (programlist != null)
            {

                var results = new List<Program>();
                var resultsT = new List<Program>();
                var stringToSplit = programlist;

                // Get Top program not yet created
               

                // First, get list of all requested programs
                var queryT = from val in stringToSplit.Split(',')
                             select Convert.ToInt32(val);
                foreach (int val in queryT)
                {


                    var totalprogramcount = (from x in context.Programs
                                             where x.ProgramID == val
                                             select x).ToList();

                    foreach (Program program in totalprogramcount)
                    {

                        resultsT.Add(program);

                    }

                }

                if (resultsT.Count() != 0)
                {

                    // Find out which ones have yet to be created
                    foreach (Program p in resultsT)
                    {

                        // Construct a new list of those not yet handled

                        var unassignedprogramcount = (from x in context.SiteRoleProgramUserProfiles
                                                      join y in context.Programs
                                                      on x.ProgramID equals y.ProgramID
                                                      where x.ProgramID == p.ProgramID && x.Id == id
                                                      select y).ToList();


                        if (unassignedprogramcount.Count() < 1)
                        {

                            //foreach (Program program in unassignedprogramcount)
                            //{

                                results.Add(p);

                            //}

                        }

                    }

                    if (results.Count() > 0)
                    {


                       

                        // Program Info
                        var selectedprogram = (from x in results
                                               select x).First();


                    


                        ViewBag.ProgramID = selectedprogram.ProgramID;
                        ViewBag.ProgramName = selectedprogram.ProgramName;

                        //IEnumerable OptionF = context.Sites.Join(context.ProgramSites, c => c.SiteID, p => p.SiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.ProgramID == selectedprogram.ProgramID).OrderBy(z => z.c.SiteName).AsEnumerable().Select(z => new SelectListItem()

                        //{

                        //    Text = z.c.SiteName,
                        //    Value = z.c.SiteID.ToString(),
                        //    Selected = true,
                        //});
                        List<SelectListItem> OptionF = new List<SelectListItem>();

                        if (id7 != null)
                        {


                            var stringToSplitF = id7;

                            var queryF = from valF in stringToSplitF.Split(',')
                                         select Convert.ToInt32(valF);
                            foreach (int valF in queryF)
                            {

                                //var sitelist = (from x in context.Sites
                                //                 join y in context.ProgramSites
                                //                 on x.SiteID equals y.SiteID
                                //                 join z in context.LegacySiteProgramSites
                                //                 on x.SiteID equals z.SiteID
                                //                 where x.Active == true && y.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == valF
                                //                 select x).ToList();

                                var sitelist = (from x in context.Sites
                                                //join y in context.ProgramSites
                                                //on x.SiteID equals y.SiteID
                                                join z in context.AgencySiteProgramSites
                                                on x.SiteID equals z.SiteID
                                                where x.Active == true && z.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == valF
                                                select x).ToList();


                                foreach (Site s in sitelist)
                                {


                                    var site = (from x in context.Sites
                                                    where x.SiteID == s.SiteID
                                                    select x).SingleOrDefault();


                                    bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(site.SiteID));

                                    if (alreadyExists == false)
                                    {

                                        OptionF.Add(new SelectListItem { Text = site.SiteName, Value = Convert.ToString(site.SiteID) });

                                    }


                                }



                            }



                        }
                        //IEnumerable OptionF = (from x in context.Sites
                        //                              join y in context.ProgramSites
                        //                              on x.SiteID equals y.SiteID
                        //                              join z in context.LegacySiteProgramSites
                        //                              on x.SiteID equals z.SiteID
                        //                              where x.Active == true && y.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == id6
                        //                              select new {

                        //                                  Text = x.SiteName,
                        //                                  Value = x.SiteID.ToString(),
                        //                                  Selected = true
                                                      
                                                      
                                                      
                        //                              }).AsEnumerable().ToList();


                        var NewtermsF = new SelectList(OptionF, "Value", "Text");



                       


                        ViewBag.OptionListF = NewtermsF;



                        var medoutFMW = (from x in context.AspNetUsers
                                         join y in context.SiteRoleProgramUserProfiles
                                         on x.Id equals y.Id
                                         where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                         select y.SiteID).Count();

                        if (medoutFMW > 0)
                        {


                            var rowsFMW =
                       (from x in context.AspNetUsers
                        join y in context.SiteRoleProgramUserProfiles
                        on x.Id equals y.Id
                        where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                        select y.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                            ViewBag.Options = rowsFMW;

                        }


                     //   if (id5 != null)
                     //   {



                             var roleidcount = (from x in context.AspNetUsers
                                      join y in context.SiteRoleProgramUserProfiles
                                      on x.Id equals y.Id
                                      where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                      select y.RoleID).Count();


                             if (roleidcount > 0)
                             {


                                 var roleid = (from x in context.AspNetUsers
                                               join y in context.SiteRoleProgramUserProfiles
                                               on x.Id equals y.Id
                                               where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                               select y.RoleID).First();
                                
                                 int[] ids = new int[] { 2,6 };
                                 var J = (from P in context.RoleBins
                                          join H in context.ApplicationRolePrograms
                                          on P.RoleBinID equals H.RoleID
                                          where P.Active == true && P.IsPortalUser == true && H.ProgramID == selectedprogram.ProgramID && ids.Contains(P.RoleBinID)
                                         select P).Distinct();


                                 ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", roleid);



                             }
                             else
                             {

                                 //var J = from P in context.RoleBins
                                 //        where P.Active == true
                                 //        select P;
                                 int[] ids = new int[] { 2,6 };
                                 var J = (from P in context.RoleBins
                                          join H in context.ApplicationRolePrograms
                                          on P.RoleBinID equals H.RoleID
                                          where P.Active == true && P.IsPortalUser == true && H.ProgramID == selectedprogram.ProgramID && ids.Contains(P.RoleBinID)
                                         select P).Distinct();


                                 ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName");



                             }


                      //  }

                    }


                    //if (id3 != null && id4 == null)
                    //{



                    //    // Program Info
                    //    var selectedprogram = (from x in resultsT
                    //                           select x).First();



                    //    ViewBag.ProgramID = selectedprogram.ProgramID;
                    //    ViewBag.ProgramName = selectedprogram.ProgramName;

                    //    IEnumerable OptionF = context.Sites.Join(context.ProgramSites, c => c.SiteID, p => p.SiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.ProgramID == selectedprogram.ProgramID).OrderBy(z => z.c.SiteName).AsEnumerable().Select(z => new SelectListItem()

                    //    {

                    //        Text = z.c.SiteName,
                    //        Value = z.c.SiteID.ToString(),
                    //        Selected = true,
                    //    });


                    //    var NewtermsF = new SelectList(OptionF, "Value", "Text");
                    //    ViewBag.OptionListF = NewtermsF;



                    //    var medoutFMW = (from x in context.AspNetUsers
                    //                     join y in context.SiteRoleProgramUserProfiles
                    //                     on x.Id equals y.Id
                    //                     where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                    //                     select y.SiteID).Count();

                    //    if (medoutFMW > 0)
                    //    {


                    //        var rowsFMW =
                    //   (from x in context.AspNetUsers
                    //    join y in context.SiteRoleProgramUserProfiles
                    //    on x.Id equals y.Id
                    //    where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                    //    select y.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                    //        ViewBag.Options = rowsFMW;

                    //    }


                    //    var roleid = (from x in context.AspNetUsers
                    //                     join y in context.SiteRoleProgramUserProfiles
                    //                     on x.Id equals y.Id
                    //                     where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                    //                     select y.RoleID).First();

                    //    var J = from P in context.RoleBins
                    //            where P.Active == true
                    //            select P;


                    //    ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", roleid);


                    //}

                    else if (id3 == null && id4 == null)
                    {


                        // Program Info
                        var selectedprogram = (from x in results
                                               select x).First();



                        ViewBag.ProgramID = selectedprogram.ProgramID;
                        ViewBag.ProgramName = selectedprogram.ProgramName;

                        //IEnumerable OptionF = context.Sites.Join(context.ProgramSites, c => c.SiteID, p => p.SiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.ProgramID == selectedprogram.ProgramID).OrderBy(z => z.c.SiteName).AsEnumerable().Select(z => new SelectListItem()

                        //{

                        //    Text = z.c.SiteName,
                        //    Value = z.c.SiteID.ToString(),
                        //    Selected = true,
                        //});


                        List<SelectListItem> OptionF = new List<SelectListItem>();

                        if (id7 != null)
                        {


                            var stringToSplitF = id7;

                            var queryF = from valF in stringToSplitF.Split(',')
                                         select Convert.ToInt32(valF);
                            foreach (int valF in queryF)
                            {

                                var sitelist = (from x in context.Sites
                                                //join y in context.ProgramSites
                                                //on x.SiteID equals y.SiteID
                                                join z in context.AgencySiteProgramSites
                                                on x.SiteID equals z.SiteID
                                                where x.Active == true && z.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == valF
                                                select x).ToList();


                                foreach (Site s in sitelist)
                                {


                                    var site = (from x in context.Sites
                                                where x.SiteID == s.SiteID
                                                select x).SingleOrDefault();


                                    bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(site.SiteID));

                                    if (alreadyExists == false)
                                    {

                                        OptionF.Add(new SelectListItem { Text = site.SiteName, Value = Convert.ToString(site.SiteID) });

                                    }


                                }



                            }



                        }


                      
                        var NewtermsF = new SelectList(OptionF, "Value", "Text");
                        ViewBag.OptionListF = NewtermsF;



                        var medoutFMW = (from x in context.AspNetUsers
                                         join y in context.SiteRoleProgramUserProfiles
                                         on x.Id equals y.Id
                                         where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                         select y.SiteID).Count();

                        if (medoutFMW > 0)
                        {


                            var rowsFMW =
                       (from x in context.AspNetUsers
                        join y in context.SiteRoleProgramUserProfiles
                        on x.Id equals y.Id
                        where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                        select y.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                            ViewBag.Options = rowsFMW;

                        }
                    
                    
                   
                    
                    }
                    else
                    {


                        // Program Info
                        var selectedprogram = (from x in resultsT
                                               where x.ProgramID == id5
                                               select x).First();



                        ViewBag.ProgramID = selectedprogram.ProgramID;
                        ViewBag.ProgramName = selectedprogram.ProgramName;

                        //IEnumerable OptionF = context.Sites.Join(context.ProgramSites, c => c.SiteID, p => p.SiteID, (c, p) => new { c, p }).Where(z => z.c.Active == true && z.p.ProgramID == selectedprogram.ProgramID).OrderBy(z => z.c.SiteName).AsEnumerable().Select(z => new SelectListItem()

                        //{

                        //    Text = z.c.SiteName,
                        //    Value = z.c.SiteID.ToString(),
                        //    Selected = true,
                        //});

                        //IEnumerable OptionF = (from x in context.Sites
                        //                       join y in context.ProgramSites
                        //                       on x.SiteID equals y.SiteID
                        //                       join z in context.LegacySiteProgramSites
                        //                       on x.SiteID equals z.SiteID
                        //                       where x.Active == true && y.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == id6
                        //                       select new
                        //                       {

                        //                           Text = x.SiteName,
                        //                           Value = x.SiteID.ToString(),
                        //                           Selected = true



                        //                       }).AsEnumerable().ToList();



                        List<SelectListItem> OptionF = new List<SelectListItem>();

                        if (id7 != null)
                        {


                            var stringToSplitF = id7;

                            var queryF = from valF in stringToSplitF.Split(',')
                                         select Convert.ToInt32(valF);
                            foreach (int valF in queryF)
                            {

                                var sitelist = (from x in context.Sites
                                                //join y in context.ProgramSites
                                                //on x.SiteID equals y.SiteID
                                                join z in context.AgencySiteProgramSites
                                                on x.SiteID equals z.SiteID
                                                where x.Active == true && z.ProgramID == selectedprogram.ProgramID && z.AgencySiteID == valF
                                                select x).ToList();


                                foreach (Site s in sitelist)
                                {


                                    var site = (from x in context.Sites
                                                where x.SiteID == s.SiteID
                                                select x).SingleOrDefault();


                                    bool alreadyExists = OptionF.Any(x => x.Value == Convert.ToString(site.SiteID));

                                    if (alreadyExists == false)
                                    {

                                        OptionF.Add(new SelectListItem { Text = site.SiteName, Value = Convert.ToString(site.SiteID) });

                                    }


                                }



                            }



                        }


                        var NewtermsF = new SelectList(OptionF, "Value", "Text");
                        ViewBag.OptionListF = NewtermsF;



                        var medoutFMW = (from x in context.AspNetUsers
                                         join y in context.SiteRoleProgramUserProfiles
                                         on x.Id equals y.Id
                                         where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                         select y.SiteID).Count();

                        if (medoutFMW > 0)
                        {


                            var rowsFMW =
                       (from x in context.AspNetUsers
                        join y in context.SiteRoleProgramUserProfiles
                        on x.Id equals y.Id
                        where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                        select y.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                            ViewBag.Options = rowsFMW;

                        }


                        var roleidcount = (from x in context.AspNetUsers
                                      join y in context.SiteRoleProgramUserProfiles
                                      on x.Id equals y.Id
                                      where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                      select y.RoleID).Count();


                        if (roleidcount > 0)
                        {

                            var roleid = (from x in context.AspNetUsers
                                          join y in context.SiteRoleProgramUserProfiles
                                          on x.Id equals y.Id
                                          where x.Id == id && y.ProgramID == selectedprogram.ProgramID
                                          select y.RoleID).First();

                            //var J = from P in context.RoleBins
                            //        where P.Active == true
                            //        select P;
                            int[] ids = new int[] { 2,6 };
                            var J = (from P in context.RoleBins
                                     join H in context.ApplicationRolePrograms
                                     on P.RoleBinID equals H.RoleID
                                     where P.Active == true && P.IsPortalUser == true && H.ProgramID == selectedprogram.ProgramID && ids.Contains(P.RoleBinID)
                                     select P).Distinct();


                            ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName", roleid);

                        }
                        else
                        {

                            //var J = from P in context.RoleBins
                            //        where P.Active == true
                            //        select P;
                            int[] ids = new int[] { 2,6 };
                            var J = (from P in context.RoleBins
                                     join H in context.ApplicationRolePrograms
                                     on P.RoleBinID equals H.RoleID
                                     where P.Active == true && P.IsPortalUser == true && H.ProgramID == selectedprogram.ProgramID && ids.Contains(P.RoleBinID)
                                     select P).Distinct();


                            ViewBag.RoleBinID = new SelectList(J, "RoleBinID", "RoleBinName");



                        }


                    }

                }


            }


            DateTime Created = DateTime.Now;
            ViewBag.DateCreated = Created;
            ViewBag.DateUpdated = Created;

            return PartialView();


        }

        [AllowAnonymous]
        public ActionResult _Step3Registration(string id, int? id2)
        {


            var U = (from P in context.AspNetUsers
                     where P.Id == id
                     select P).SingleOrDefault();

            ViewBag.FullName = U.FirstName + " " + U.LastName;
            ViewBag.Email = U.Email;
            ViewBag.PhoneNumber = U.TelephoneNumber;


            ViewBag.Id = id;
            DateTime Created = DateTime.Now;
            ViewBag.DateCreated = Created;
            ViewBag.DateUpdated = Created;


            // Get RoleName/ID
            var R = (from P in context.AspNetUsers
                     join T in context.RoleBins
                     on P.RoleBinID equals T.RoleBinID
                     where P.Id == id
                     select T).SingleOrDefault();

            ViewBag.RoleBinID = R.RoleBinID;
            ViewBag.RoleBinName = R.RoleBinName;


            return PartialView();


        }


        [AllowAnonymous]
        public ActionResult _Step4Registration(string id, int? id2)
        {


            var U = (from P in context.AspNetUsers
                     where P.Id == id
                     select P).SingleOrDefault();

            ViewBag.FullName = U.FirstName + " " + U.LastName;
            ViewBag.Email = U.Email;
            ViewBag.PhoneNumber = U.TelephoneNumber;


            ViewBag.Id = id;
            DateTime Created = DateTime.Now;
            ViewBag.DateCreated = Created;
            ViewBag.DateUpdated = Created;


            // Get RoleName/ID
            var R = (from P in context.AspNetUsers
                     join T in context.RoleBins
                     on P.RoleBinID equals T.RoleBinID
                     where P.Id == id
                     select T).SingleOrDefault();

            ViewBag.RoleBinID = R.RoleBinID;
            ViewBag.RoleBinName = R.RoleBinName;


            return PartialView();


        }

        [HttpGet]
        public ActionResult SetSession(string session)
        {

            System.Web.HttpContext.Current.Session["UID"] = session;


            return View();
        }

        //
        // POST: /Account/Register
        //[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> _RegisterF(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {


                    string ID = "";
                    int? pid = null;
                    int? rid = null;
                    if (model.StepID == 1)
                    {


                        if (model.UserIdentity == null)
                        {



                            var userCHECK = await UserManager.FindByNameAsync(model.Email);
                            if (userCHECK != null)
                            {


                                string message;
                                message = string.Format(
                                          "This Email Address already exists. Please contact our support staff for further assistance.");

                                ModelState.AddModelError("", message);

                                return Json(new { Status = "Failure" }, JsonRequestBehavior.AllowGet);


                            }

                            // Set Users to Role 2
                            if (model.StateStaffCheck == 0)
                            {

                                model.RoleBinID = 2;

                            }


                            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                            if (result.Succeeded)
                            {
                                // await SignInAsync(user, isPersistent: false);

                                //Assign User to request and update account information
                                var userA =
                                (from c in context.AspNetUsers
                                 where c.UserName == model.Email
                                 select c).First();

                                // Update user record.

                                userA.FirstName = model.FirstName;
                                userA.LastName = model.LastName;
                                userA.MiddleName = model.MiddleName;
                                userA.Status = model.Status;
                                userA.DateUpdated = model.DateUpdated;
                                userA.DateCreated = model.DateCreated;
                                userA.CreatedBy = model.CreatedBy;
                                userA.LastPasswordChangedDate = model.DateCreated;
                                userA.UpdatedBy = model.UpdatedBy;
                                userA.ProfilePic = "https://www.healthinformatics.dphe.state.co.us/Content/UploadedFolder/userpic.jpg";
                                userA.UpdatedBy = model.FirstName + " " + model.LastName;
                                userA.CreatedBy = model.FirstName + " " + model.LastName;
                                userA.TelephoneNumber = model.TelephoneNumber;
                                userA.RoleBinID = model.RoleBinID;
                                userA.EmailConfirmed = true;
                                userA.LockoutEnabled = true;
                                userA.ProgramAccess = model.ProgramAccess;
                                userA.Status = true;
                                userA.Active = false;
                                userA.firstTimeLoggedIn = true;
                                userA.LegacyUserName = model.Email;
                                userA.Dashboard = true;


                                // Ask the DataContext to save all the changes.
                                context.SaveChanges();

                                ID = userA.Id;


                            }
                            else
                            {

                                DateTime Created = DateTime.Now;


                                ViewBag.DateCreated = Created;
                                //ViewBag.Public = id;

                                ViewBag.DateUpdated = Created;


                                AddErrors(result);
                            }



                            // Check for State Staff, if so set pcount = 0 and progress to step 3


                            if (model.RoleBinID == 3 || model.RoleBinID == 13 || model.RoleBinID == 39)
                            {

                                var pcount = 0;

                                // Add user to RoleProgramUserProfiles Table by program

                                if (model.ProgramAccess != null)
                                {

                                    var stringToSplit2 = model.ProgramAccess;


                                    // Loop again to get next ProgramID
                                    var queryT2 = from val in stringToSplit2.Split(',')
                                                  select Convert.ToInt32(val);
                                    foreach (int val in queryT2)
                                    {


                                        RoleProgramUserProfiles rpu = new RoleProgramUserProfiles
                                        {


                                            RoleID = Convert.ToInt32(model.RoleBinID),
                                            ProgramID = val,
                                            Id = ID,
                                            Coordinator = model.Coordinator
                                            

                                        };

                                        context.RoleProgramUserProfiles.Add(rpu);
                                        context.SaveChanges();

                                        var resultUlist = new List<ApplicationUser>();
                                        var adminlist = (from x in context.AspNetUsers
                                                         join y in context.RoleProgramUserProfiles
                                                         on x.Id equals y.Id
                                                         where y.RoleID == 13 && y.ProgramID == val && y.Coordinator == true
                                                         select x).Distinct().ToList();


                                        // Get Register info on user
                                        var registerinfo = (from x in context.AspNetUsers
                                                            where x.Id == ID
                                                            select x).SingleOrDefault();



                                        foreach (ApplicationUser u in adminlist)
                                        {

                                            bool alreadyExists = resultUlist.Any(x => x.Email == u.Email);

                                            if (alreadyExists == false)
                                            {

                                                   SendRegisterEmail(u.Email, registerinfo.FirstName, registerinfo.LastName, registerinfo.Email, registerinfo.TelephoneNumber);

                                                }

                                        }



                                    }

                                    pcount = 0;


                                }



                             


                            }



                        }
                        else
                        {


                            ID = model.UserIdentity;
                            pid = 1;
                            rid = 0;


                            if (model.RoleBinID == 3 || model.RoleBinID == 13 || model.RoleBinID == 39)
                            {


                              
                                var userB =
                               (from c in context.AspNetUsers
                                join z in context.RoleProgramUserProfiles
                                on c.Id equals z.Id
                                where c.Id == model.UserIdentity
                                select c).First();

                             
                                // Update user record.
                                userB.Active = true;
                                context.SaveChanges();

                                var pcount = 0;

                                // Add user to RoleProgramUserProfiles Table by program

                                if (model.ProgramAccess != null)
                                {


                                    var plist = (from c in context.AspNetUsers
                                    join z in context.RoleProgramUserProfiles
                                    on c.Id equals z.Id
                                    where c.Id == model.UserIdentity
                                    select z).ToList();

                                    // Remove All Program Records from SiteRoleProgramUserProfiles
                                    foreach (RoleProgramUserProfiles hs in plist)
                                    {

                                        context.RoleProgramUserProfiles.Remove(hs);
                                        context.SaveChanges();


                                    }

                                    var stringToSplit2 = model.ProgramAccess;

                                    // Loop again to get next ProgramID
                                    var queryT2 = from val in stringToSplit2.Split(',')
                                                  select Convert.ToInt32(val);
                                    foreach (int val in queryT2)
                                    {


                                        RoleProgramUserProfiles rpu = new RoleProgramUserProfiles
                                        {


                                            RoleID = Convert.ToInt32(model.RoleBinID),
                                            ProgramID = val,
                                            Id = ID,
                                            Coordinator = model.Coordinator


                                        };

                                        context.RoleProgramUserProfiles.Add(rpu);
                                        context.SaveChanges();




                                    }

                                    // Update Role on AspNetUsers Table
                                    var userA =
                                    (from c in context.AspNetUsers
                                     where c.Id == model.UserIdentity
                                     select c).First();

                                    // Update user record.
                                    userA.RoleBinID = Convert.ToInt32(model.RoleBinID);
                                    context.SaveChanges();


                                    pcount = 0;


                                }



                                return Json(new { Status = "Success", Modified = ID, Modified2 = model.StepID, Modified3 = 0, Modified4 = pid, Modified5 = rid, Modified6 = model.ProgramID, Modified7 = model.AgencySiteID }, JsonRequestBehavior.AllowGet);

                            }

                            else
                            {


                                //Assign User to request and update account information
                                var userA =
                                (from c in context.AspNetUsers
                                 where c.Id == model.UserIdentity
                                 select c).First();

                                // Update user record.
                                userA.ProgramAccess = model.ProgramAccess;
                                userA.Active = true;
                                // Ask the DataContext to save all the changes.
                                context.SaveChanges();


                                var resultsN = new List<Program>();

                                // Loop through Requested Programs, build list
                                if (model.ProgramAccess != null)
                                {

                                    var stringToSplit = model.ProgramAccess;

                                    // Get total number of programs to be created
                                    var queryT = from val in stringToSplit.Split(',')
                                                 select Convert.ToInt32(val);
                                    foreach (int val in queryT)
                                    {


                                        var totalprogramcount = (from x in context.Programs
                                                                 where x.ProgramID == val
                                                                 select x).ToList();

                                        foreach (Program program in totalprogramcount)
                                        {

                                            resultsN.Add(program);

                                        }


                                    }

                                    var results = new List<SiteRoleProgramUserProfiles>();
                                    // Get total existing programs
                                    var existingprogramlist = (from x in context.AspNetUsers
                                                               join y in context.SiteRoleProgramUserProfiles
                                                               on x.Id equals y.Id
                                                               join z in context.Programs
                                                               on y.ProgramID equals z.ProgramID
                                                               where y.Id == ID
                                                               select z).ToList();


                                    // Compare the new list to existing to find if any have been removed

                                    var programCompare = existingprogramlist.All(c => resultsN.Select(ds => ds.ProgramID).Contains(c.ProgramID));

                                    if (programCompare == false)
                                    {

                                        // Find out which programs were deleted
                                        IEnumerable<Program> differenceQuery = existingprogramlist.Except(resultsN);


                                        foreach (Program p in differenceQuery)
                                        {


                                            // Check to see if any programs were removed, and delete
                                            var programlist = (from x in context.AspNetUsers
                                                               join y in context.SiteRoleProgramUserProfiles
                                                               on x.Id equals y.Id
                                                               join z in context.Programs
                                                               on y.ProgramID equals z.ProgramID
                                                               where y.ProgramID == p.ProgramID && y.Id == ID
                                                               select y).ToList();


                                            foreach (SiteRoleProgramUserProfiles srpup in programlist)
                                            {

                                                results.Add(srpup);

                                            }


                                        }


                                        // Remove All Program Records from SiteRoleProgramUserProfiles
                                        foreach (SiteRoleProgramUserProfiles hs in results)
                                        {

                                            context.SiteRoleProgramUserProfiles.Remove(hs);
                                            context.SaveChanges();


                                        }


                                    }


                                    var results2 = new List<Program>();
                                    if (model.ProgramAccess != null)
                                    {

                                        var stringToSplit2 = model.ProgramAccess;


                                        // Loop again to get next ProgramID
                                        var queryT2 = from val in stringToSplit2.Split(',')
                                                      select Convert.ToInt32(val);
                                        foreach (int val in queryT2)
                                        {

                                            var totalprogramcount = (from x in context.Programs
                                                                     where x.ProgramID == val
                                                                     select x).ToList();

                                            foreach (Program program in totalprogramcount)
                                            {

                                                results2.Add(program);






                                            }


                                        }

                                    }





                                    var pid2 = (from x in results2
                                                select x.ProgramID).First();

                                    model.ProgramID = pid2;


                                    // Update ProgramAccess
                                    var rows =
                                       (from x in results2
                                        // where x.ProgramID != pid2
                                        select x.ProgramID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                                    model.ProgramAccess = rows;


                                    //Assign new program access
                                    var userAA =
                                    (from c in context.AspNetUsers
                                     where c.Id == ID
                                     select c).First();

                                    userAA.ProgramAccess = model.ProgramAccess;

                                    context.SaveChanges();


                                    ViewBag.ResultsTCount = resultsN.Count();
                                    ViewBag.ResultsRCount = model.ResultsRCount;
                                    pid = resultsN.Count();
                                    rid = model.ResultsRCount;

                                    var pcount = 0;

                                    if (pid != rid)
                                    {

                                        pcount = 1;

                                    }
                                    else
                                    {

                                        pcount = 0;

                                    }

                                    return Json(new { Status = "Success", Modified = ID, Modified2 = model.StepID, Modified3 = pcount, Modified4 = pid, Modified5 = rid, Modified6 = model.ProgramID, Modified7 = model.AgencySiteID, Modified8 = model.AgencySiteAccess }, JsonRequestBehavior.AllowGet);

                                }





                            }




                        }


                    }
                    else if (model.StepID == 2)
                    {

                        pid = model.ResultsCCount;
                        ID = model.Id;


                        if (pid == null)
                        {



                            if (model.SiteAccess != null)
                            {

                                // Site List
                                var stringToSplitS = model.SiteAccess;

                                var queryS = from valS in stringToSplitS.Split(',')
                                             select Convert.ToInt32(valS);
                                foreach (int valS in queryS)
                                {



                                    // Check if it already exists, if not add
                                    var sitecreatedcount = (from x in context.AspNetUsers
                                                            join y in context.SiteRoleProgramUserProfiles
                                                            on x.Id equals y.Id
                                                            join z in context.Programs
                                                            on y.ProgramID equals z.ProgramID
                                                            where y.ProgramID == model.ProgramID && y.Id == ID && y.SiteID == valS
                                                            select z).Count();
                                    if (sitecreatedcount < 1)
                                    {

                                        SiteRoleProgramUserProfiles srpu = new SiteRoleProgramUserProfiles
                                        {


                                            RoleID = Convert.ToInt32(model.RoleBinID),
                                            ProgramID = Convert.ToInt32(model.ProgramID),
                                            SiteID = valS,
                                            Id = ID,
                                            //AgencySiteID = Convert.ToInt32(model.AgencySiteID),
                                            Training = false



                                        };

                                        context.SiteRoleProgramUserProfiles.Add(srpu);
                                        context.SaveChanges();




                                    }



                                }



                            }



                        }
                        else
                        {
                            var resultsS = new List<Site>();
                            if (model.SiteAccess != null)
                            {


                                // Site List
                                var stringToSplitS = model.SiteAccess;

                                var queryS = from valS in stringToSplitS.Split(',')
                                             select Convert.ToInt32(valS);
                                foreach (int valS in queryS)
                                {


                                    var totalsitecount = (from x in context.Sites
                                                          where x.SiteID == valS
                                                          select x).ToList();

                                    foreach (Site site in totalsitecount)
                                    {

                                        resultsS.Add(site);

                                    }


                                    // Check if it already exists, if not add
                                    var sitecreatedcount = (from x in context.AspNetUsers
                                                            join y in context.SiteRoleProgramUserProfiles
                                                            on x.Id equals y.Id
                                                            join z in context.Programs
                                                            on y.ProgramID equals z.ProgramID
                                                            where y.ProgramID == model.ProgramID && y.Id == ID && y.SiteID == valS
                                                            select z).Count();
                                    if (sitecreatedcount < 1)
                                    {

                                        SiteRoleProgramUserProfiles srpu = new SiteRoleProgramUserProfiles
                                        {


                                            RoleID = Convert.ToInt32(model.RoleBinID),
                                            ProgramID = Convert.ToInt32(model.ProgramID),
                                            SiteID = valS,
                                            Id = ID,
                                            //AgencySiteID = Convert.ToInt32(model.AgencySiteID),
                                            Training = false



                                        };

                                        context.SiteRoleProgramUserProfiles.Add(srpu);
                                        context.SaveChanges();



                                    }
                                    else
                                    {


                                        // Remove and Recreate site to adjust role
                                        SiteRoleProgramUserProfiles roleupdatelist = (from x in context.AspNetUsers
                                                                                      join y in context.SiteRoleProgramUserProfiles
                                                                                      on x.Id equals y.Id
                                                                                      join z in context.Programs
                                                                                      on y.ProgramID equals z.ProgramID
                                                                                      where y.ProgramID == model.ProgramID && y.Id == ID && y.SiteID == valS
                                                                                      select y).SingleOrDefault();

                                        context.SiteRoleProgramUserProfiles.Remove(roleupdatelist);
                                        context.SaveChanges();

                                        SiteRoleProgramUserProfiles srpu = new SiteRoleProgramUserProfiles
                                        {


                                            RoleID = Convert.ToInt32(model.RoleBinID),
                                            ProgramID = Convert.ToInt32(model.ProgramID),
                                            SiteID = valS,
                                            //AgencySiteID = Convert.ToInt32(model.AgencySiteID),
                                            Id = ID,
                                            Training = false



                                        };

                                        context.SiteRoleProgramUserProfiles.Add(srpu);
                                        context.SaveChanges();


                                    }



                                    // Check for removed sites
                                    var resultsSS = new List<SiteRoleProgramUserProfiles>();
                                    // Get total existing programs
                                    var existingsitelist = (from x in context.AspNetUsers
                                                            join y in context.SiteRoleProgramUserProfiles
                                                            on x.Id equals y.Id
                                                            join z in context.Programs
                                                            on y.ProgramID equals z.ProgramID
                                                            join k in context.Sites
                                                            on y.SiteID equals k.SiteID
                                                            where y.ProgramID == model.ProgramID && y.Id == ID
                                                            select k).ToList();


                                    // Compare the new list to existing to find if any have been removed

                                    var siteCompare = existingsitelist.All(c => resultsS.Select(ds => ds.SiteID).Contains(c.SiteID));

                                    if (siteCompare == false)
                                    {

                                        // Find out which programs were deleted
                                        IEnumerable<Site> differenceQuery = existingsitelist.Except(resultsS);


                                        foreach (Site s in differenceQuery)
                                        {


                                            // Check to see if any programs were removed, and delete
                                            var sitelist = (from x in context.AspNetUsers
                                                            join y in context.SiteRoleProgramUserProfiles
                                                            on x.Id equals y.Id
                                                            join z in context.Programs
                                                            on y.ProgramID equals z.ProgramID
                                                            where y.ProgramID == model.ProgramID && y.Id == ID && y.SiteID == s.SiteID
                                                            select y).ToList();


                                            foreach (SiteRoleProgramUserProfiles srpup in sitelist)
                                            {

                                                resultsSS.Add(srpup);

                                            }


                                        }


                                        // Remove All Program Records from SiteRoleProgramUserProfiles
                                        foreach (SiteRoleProgramUserProfiles hss in resultsSS)
                                        {

                                            context.SiteRoleProgramUserProfiles.Remove(hss);
                                            context.SaveChanges();


                                        }


                                    }


                                }




                            }


                            //  //var results = new List<RegisterViewModel>();
                            var resultsN = new List<Program>();


                            // Get ProgramAccess from db

                            //Assign new program access
                            var userPA =
                            (from c in context.AspNetUsers
                             where c.Id == ID
                             select c.ProgramAccess).Single();


                            // Loop through Requested Programs, build list
                            //if (model.ProgramAccess != null)
                            if (userPA != null)
                            {

                                //var stringToSplit = model.ProgramAccess;
                                var stringToSplit = userPA;

                                // Get total number of programs to be created
                                var queryT = from val in stringToSplit.Split(',')
                                             select Convert.ToInt32(val);
                                foreach (int val in queryT)
                                {


                                    var totalprogramcount = (from x in context.Programs
                                                             where x.ProgramID == val
                                                             select x).ToList();

                                    foreach (Program program in totalprogramcount)
                                    {

                                        resultsN.Add(program);

                                    }




                                }


                                //    pid = resultsN.Count();
                                //    //rid = model.ResultsRCount;

                            }


                            // Check if last program, then flip pid
                            var singleprogramcheck = (from x in resultsN
                                                      where x.ProgramID != model.ProgramID
                                                      select x.ProgramID).Count();
                            var pcount = 1;


                            if (singleprogramcheck != 0)
                            {

                                //  if (pid > 0)
                                //  {

                                var rows =
                          (from x in resultsN
                           where x.ProgramID != model.ProgramID
                           select x.ProgramID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                                // model.ProgramAccess = rows;
                                userPA = rows;


                                //Assign new program access
                                var userA =
                                (from c in context.AspNetUsers
                                 where c.Id == ID
                                 select c).First();

                                // userA.ProgramAccess = model.ProgramAccess;
                                userA.ProgramAccess = userPA;

                                context.SaveChanges();



                                var results2 = new List<Program>();
                                // if (model.ProgramAccess != null)
                                pid = results2.Count();

                                if (userPA != null)
                                {

                                    // var stringToSplit2 = model.ProgramAccess;
                                    var stringToSplit2 = userPA;

                                    // Loop again to get next ProgramID
                                    var queryT2 = from val in stringToSplit2.Split(',')
                                                  select Convert.ToInt32(val);
                                    foreach (int val in queryT2)
                                    {

                                        var totalprogramcount = (from x in context.Programs
                                                                 where x.ProgramID == val
                                                                 select x).ToList();

                                        foreach (Program program in totalprogramcount)
                                        {

                                            results2.Add(program);

                                        }


                                    }

                                }

                                // }

                                var pid2 = (from x in results2
                                            select x.ProgramID).First();

                                model.ProgramID = pid2;

                                if (model.ProgramID == 53)
                                {

                                  //  SetSession(ID);
                                  //  System.Web.HttpContext.Current.Session["UID"] = ID;

                                    if (ID != "")
                                    {

                                        ID = model.Id;

                                    }
                                

                                }

                            }
                            else
                            {


                                pcount = 0;



                            }



                            return Json(new { Status = "Success", Modified = ID, Modified2 = model.StepID, Modified3 = pcount, Modified4 = pid, Modified5 = rid, Modified6 = model.ProgramID, Modified7 = model.AgencySiteID, Modified8 = model.AgencySiteAccess }, JsonRequestBehavior.AllowGet);


                        }



                    }


                    if (pid == null)
                    {



                        if (model.RoleBinID == 3 || model.RoleBinID == 13 || model.RoleBinID == 39)
                        {


                            
                            if (model.ProgramAccess != null)
                            {

                                var stringToSplit = model.ProgramAccess;


                                // Get total number of programs to be created
                                var queryT = from val in stringToSplit.Split(',')
                                             select Convert.ToInt32(val);
                                foreach (int val in queryT)
                                {


                                    if (val == 53)
                                    {

                                        pid = 53;

                                      //  SetSession(ID);
                                      //  System.Web.HttpContext.Current.Session["UID"] = ID;
                                        if (ID == "")
                                        {

                                            ID = model.Id;

                                        }


                                    }


                                }

                            }

                            return Json(new { Status = "Success", Modified = ID, Modified2 = model.StepID, Modified3 = 0, Modified4 = pid, Modified5 = rid, Modified6 = model.ProgramID, Modified7 = model.AgencySiteID }, JsonRequestBehavior.AllowGet);


                        }
                        else
                        { 
                        //var results = new List<RegisterViewModel>();
                        var results = new List<Program>();
                        var resultsT = new List<Program>();
                        var pcount = 0;
                        // Loop through Requested Programs, build list
                        if (model.ProgramAccess != null)
                        {

                            var stringToSplit = model.ProgramAccess;


                            // Get total number of programs to be created
                            var queryT = from val in stringToSplit.Split(',')
                                         select Convert.ToInt32(val);
                            foreach (int val in queryT)
                            {


                                var totalprogramcount = (from x in context.Programs
                                                         where x.ProgramID == val
                                                         select x).ToList();

                                foreach (Program program in totalprogramcount)
                                {

                                    resultsT.Add(program);

                                }


                            }
                            // Get count on programs created
                            var query = from val in stringToSplit.Split(',')
                                        select Convert.ToInt32(val);
                            foreach (int val in query)
                            {


                                var programcreatedcount = (from x in context.AspNetUsers
                                                           join y in context.SiteRoleProgramUserProfiles
                                                           on x.Id equals y.Id
                                                           join z in context.Programs
                                                           on y.ProgramID equals z.ProgramID
                                                           where y.ProgramID == val && y.Id == ID
                                                           select z).ToList();

                                foreach (Program program in programcreatedcount)
                                {

                                    results.Add(program);

                                }


                            

                            }

                            // Now Send email to access coordinators from all programs
                            var resultsE = new List<Program>();
                            var resultUlist = new List<ApplicationUser>();
                            foreach (Program program in results)
                            {


                                bool alreadyExistsE = resultsE.Any(x => x.ProgramID == program.ProgramID);

                                if (alreadyExistsE == false)
                                {



                                    // Get Register info on user
                                    var registerinfo = (from x in context.AspNetUsers
                                                        where x.Id == ID
                                                        select x).SingleOrDefault();



                                   
                                    var adminlist = (from x in context.AspNetUsers
                                                     join y in context.RoleProgramUserProfiles
                                                     on x.Id equals y.Id
                                                     where y.RoleID == 13 && y.ProgramID == program.ProgramID && y.Coordinator == true
                                                     select x).Distinct().ToList();


                                    foreach (ApplicationUser u in adminlist)
                                    {

                                        bool alreadyExists = resultUlist.Any(x => x.Email == u.Email);

                                        if (alreadyExists == false)
                                        {


                                            SendRegisterEmail(u.Email, registerinfo.FirstName, registerinfo.LastName, registerinfo.Email, registerinfo.TelephoneNumber);
                                            resultUlist.Add(u);

                                        }


                                    }


                                    resultsE.Add(program);


                                }



                            }

                            var resultsFinished = resultsT.All(c => results.Select(ds => ds.ProgramID).Contains(c.ProgramID));

                            // Check count to see if program creation is done


                            // if (resultsT.Count() != results.Count())
                            if (resultsFinished == false)
                            {

                                pcount = 1;

                            }
                            else
                            {

                                pcount = 0;




                                // Get list of all programs requested, then send email to all Data Managers

                                //var programlist = (from x in context.AspNetUsers
                                //                   join y in context.SiteRoleProgramUserProfiles
                                //                   on x.Id equals y.Id
                                //                   select y.ProgramID).Distinct().ToList();

                                //foreach (int p in programlist)
                                //{

                                //    var adminlist = (from x in context.AspNetUsers
                                //                     join y in context.RoleProgramUserProfiles
                                //                     on x.Id equals y.Id
                                //                     where y.RoleID == 13 && y.ProgramID == p && y.Coordinator == true
                                //                     select x.Email).Distinct().ToList();


                                //    // Get Register info on user
                                //    var registerinfo = (from x in context.AspNetUsers
                                //                        where x.Id == ID
                                //                        select x).SingleOrDefault();



                                //    foreach (var aEmail in adminlist)
                                //    {


                                //      SendRegisterEmail(aEmail, registerinfo.FirstName, registerinfo.LastName, registerinfo.Email, registerinfo.TelephoneNumber);

                                //    }

                                //}


                            }


                        }





                        if (model.ProgramID == 53)
                        {

                            pid = model.ProgramID;
                          //  SetSession(ID);
                          //  System.Web.HttpContext.Current.Session["UID"] = ID;
                            if (ID != "")
                            {

                              //  ID = model.Id;

                            }

                        }

                        

                        return Json(new { Status = "Success", Modified = ID, Modified2 = model.StepID, Modified3 = pcount, Modified4 = pid, Modified5 = rid, Modified6 = model.ProgramID, Modified7 = model.AgencySiteID, Modified8 = model.AgencySiteAccess }, JsonRequestBehavior.AllowGet);


                    }



                    }



                }

            
                return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

            

        }
       


        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: false);

                    //Assign User to request and update account information
                    var userA =
                    (from c in context.AspNetUsers
                     where c.UserName == model.Email
                     select c).First();

                    // Update user record.

                    userA.FirstName = model.FirstName;
                    userA.LastName = model.LastName;
                    userA.MiddleName = model.MiddleName;
                    //userA.OrganizationID = model.OrganizationID;
                    //userA.Address = model.Address;
                    //userA.City = model.City;
                    //userA.ZipCode = model.ZipCode;
                    //userA.Fax = model.Fax;
                    userA.Status = model.Status;
                    userA.DateUpdated = model.DateUpdated;
                    userA.DateCreated = model.DateCreated;
                    userA.CreatedBy = model.CreatedBy;
                    userA.UpdatedBy = model.UpdatedBy;
                    //userA.OrganizationName = model.OrganizationName;
                    userA.TelephoneNumber = model.TelephoneNumber;
                    userA.RoleBinID = model.RoleBinID;
                    //userA.StateBinID = model.StateBinID;
                    userA.PrivacyAgreement = model.PrivacyAgreement;
                    //userA.Active = model.Status;
                    userA.Status = true;
                    userA.Active = false; 
                    userA.firstTimeLoggedIn = false;

                    //if (model.GenderBinID == 1)
                    //{


                    //    userA.ProfilePic = "<p><img src='http://www.connecttoleadership.dphe.state.co.us/Assets/maleblankprofile.png'/></p>";

                    //}
                    //else
                    //{

                    //    userA.ProfilePic = "<p><img src='http://www.connecttoleadership.dphe.state.co.us/Assets/blankprofile.jpg'/></p>";

                    //}
                    
                    //user.Documents = model.Documents;

                    // Ask the DataContext to save all the changes.
                    context.SaveChanges();

                    string ID = userA.Id;

                 

                 
                 
                  //  return RedirectToAction("Success", "Home");
                  return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);


                }
                else
                {

                    DateTime Created = DateTime.Now;


                    ViewBag.DateCreated = Created;
                    //ViewBag.Public = id;

                    ViewBag.DateUpdated = Created;


                  




                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
           // return View(model);
            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
        }


        public void SendRegisterEmail(string aEmail, string FirstName, string LastName, string Email, string TelephoneNumber)
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
            mail.Subject = "Colorado Health Informatics Data Systems:" + " " + "A new account has been requested";

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Hello, a new account has been requested by:" + "</p>";
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
            mail.Body = mail.Body + "<p> " + "Phone Number:" + TelephoneNumber + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> Please login to activate this user: " + "<a target='_blank' href='https://www.healthinformatics.dphe.state.co.us'>Colorado Health Informatics Data Systems</a>" + "</p>";
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


        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string Id, string code)
        {
            if (Id == null || code == null) 
            {
                return View("Error");
            }

            IdentityResult result = await UserManager.ConfirmEmailAsync(Id, code);
            if (result.Succeeded)
            {
                return View("ConfirmEmail");
            }
            else
            {
                AddErrors(result);
                return View();
            }
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPasswordInitial()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult AccountPending()
        {
            return View();
        }


        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { Id = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string cbu = "Please reset your password by clicking the following link: <a href=\"" + callbackUrl + "\">" + callbackUrl + "</a> *NOTE: This link MAY OPEN in an older browser (i.e Internet Explorer 10 or less), if so, copy and paste into a modern browser (Chrome,Firefox or IE11).";
                SendResetEmail(user.Id,cbu);


                //Update Last Password Changed Date
                DateTime CreatedInit = DateTime.Now;
                ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.Id == user.Id);
                userprofile1.LastPasswordChangedDate = CreatedInit;
                context.SaveChanges();


                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPasswordInitial(ForgotPasswordInitialViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    ModelState.AddModelError("", "The entered email address does not exist in our system. Use the REQUEST ACCESS link above to request secure access.");
                    return View();
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { Id = user.Id, code = code }, protocol: Request.Url.Scheme);
                //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                string cbu = "<a href=\"" + callbackUrl + "\">Please reset your password by clicking here</a>";
                SendInitialResetEmail(user.Id, cbu);


                //Update Last Password Changed Date
                DateTime CreatedInit = DateTime.Now;
                ApplicationUser userprofile1 = context.AspNetUsers.Single(p => p.Id == user.Id);
                userprofile1.LastPasswordChangedDate = CreatedInit;
                context.SaveChanges();


                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }





        public void SendResetEmail(string Id, string cbu )
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

            var Email = (from ast in context.AspNetUsers
                             where ast.Id == Id
                             select ast.Email).SingleOrDefault();


            // // CREATE AND INSTATNCE OF THE MAIL MESSAGE CLASS //
            MailMessage mail = new MailMessage();
            //// SET THE PROPERTIES // 
            mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.To.Add(Email);
            //mail.CC.Add(eEmail);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Health Informatics Data Systems:" + " " + "Your password reset was successful";

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Hello, your Colorado Health Informatics Data Systems account password has been reset. Please following the instructions below:" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + cbu + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p>Once you reset your password you can access the portal here:" + "<a target='_blank' href='https://www.healthinformatics.dphe.state.co.us'>Colorado Health Informatics Data Systems</a> <b>Note: Don't forget to BOOKMARK this page in your favorites to easily access the portal!</b> " + "</p>";
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



        public void SendInitialResetEmail(string Id, string cbu)
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

            var Email = (from ast in context.AspNetUsers
                         where ast.Id == Id
                         select ast.Email).SingleOrDefault();


            // // CREATE AND INSTATNCE OF THE MAIL MESSAGE CLASS //
            MailMessage mail = new MailMessage();
            //// SET THE PROPERTIES // 
            mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.To.Add(Email);
            //mail.CC.Add(eEmail);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Health Informatics Data Systems:" + " " + "Your new Health Informatics Data Systems account is NOW ACTIVE!";

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Congratulations, your <b>Colorado Health Informatics Data Systems account is NOW ACTIVE</b>.Please follow the instructions below to set your new password:" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + cbu + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p>Once you reset your password you can access the portal here:" + "<a target='_blank' href='https://www.healthinformatics.dphe.state.co.us'>Colorado Health Informatics Data Systems</a>" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p><b>Note: Once you land on the new portal homepage, don't forget to BOOKMARK this link in your favorites to easily access the portal!</b> " + "</p>";
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


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
	
        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null) 
            {
                return View("Error");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        //[ValidateAntiForgeryToken]
        public ActionResult ResetPasswordInline(string id)
        {

            string UserName1 = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName1
                     select P.Id).SingleOrDefault();


            if (id == null)
            {
                return View("Error");
            }


            var userprofile = (from x in context.AspNetUsers
                               where x.Id == U

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
                                   RoleBinID = x.RoleBinID,
                                   Code = id,
                                   City = x.City,
                                   Active = x.Active,
                                   Status = x.Status,
                                   StateID = x.StateBinID,
                                   DateCreated = x.DateCreated,
                                   CreatedBy = x.CreatedBy



                               }).SingleOrDefault();




            return PartialView(userprofile);
        }

        
        //// POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        ////[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPasswordInline(UserProfileViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await UserManager.FindByNameAsync(model.UserName);
        //        if (user == null)
        //        {
        //            ModelState.AddModelError("", "No user found.");
        //            return PartialView();
        //        }
        //        IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Manage", "Dashboards");
        //        }
        //        else
        //        {
        //            AddErrors(result);
        //            return PartialView();
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}



        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                await SignInAsync(user, isPersistent: false);
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return PartialView();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        await SignInAsync(user, isPersistent: false);
                        //return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                        var idd = User.Identity.GetUserId().ToString();
                        ApplicationUser userRecord = context.AspNetUsers.Single(p => p.Id == idd );
                        userRecord.Active = true;
                        userRecord.firstTimeLoggedIn = false;
                        context.SaveChanges();

                        return RedirectToAction("UserAccount", "DashBoards", new { @id = User.Identity.GetUserId() });

                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };
                IdentityResult result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        
                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { Id = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");
                        
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        public ActionResult LogOffPortal()
        {


            string UserName = @User.Identity.Name.ToString();

            //Track UserLogin
            var userLON =
                    (from c in context.AspNetUsers
                     where c.UserName == UserName
                     select c).First();

            userLON.CurrentlyLoggedIn = false;
            context.SaveChanges();


            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);


            if ((Request.Url.GetLeftPart(UriPartial.Authority).ToString() == "https://www.healthinformatics.dphe.state.co.us"))
            {

                return Redirect("https://www.healthinformatics.dphe.state.co.us/");


            }
            else if ((Request.Url.GetLeftPart(UriPartial.Authority).ToString() == "https://www.test.healthinformatics.dphe.state.co.us"))
            {

                return Redirect("https://www.test.healthinformatics.dphe.state.co.us/");

            }
            else
            {

                return Redirect("https://www.train.healthinformatics.dphe.state.co.us/");

            }



        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {


            string UserName = @User.Identity.Name.ToString();

            //Track UserLogin
            var userLON =
                    (from c in context.AspNetUsers
                     where c.UserName == UserName
                     select c).First();

            userLON.CurrentlyLoggedIn = false;
            context.SaveChanges();


            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }



      
        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private void SendEmail(string email, string callbackUrl, string subject, string message)
        {
            // For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Manage", "DashBoards");
            }
        }


        private ActionResult RedirectToImportUser(string returnUrl, string U)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Edit", "UserProfiles", new { @id = U });
            }
        }


        private ActionResult RedirectToLocalUser(string returnUrl, string U)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("UserAccount", "DashBoards", new { @id = U });
            }
        }

        private ActionResult RedirectToPartnerUser(string returnUrl, string U)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string Id)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                Id = Id;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string Id { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (Id != null)
                {
                    properties.Dictionary[XsrfKey] = Id;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}