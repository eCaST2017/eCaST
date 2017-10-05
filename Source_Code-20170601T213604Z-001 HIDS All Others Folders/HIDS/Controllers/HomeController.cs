using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Owin;
using CTL.Models;

namespace ACNM.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();
        //private HIDSEntities db = new HIDSEntities();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to the Colorado Health Informatics Data System";


            var noticecount = (from P in context.Notices
                             where P.ExpirationDate > DateTime.Now
                             select P.NoticeID).Count();

            ViewBag.NoticeCount = noticecount;



            if (User.Identity.IsAuthenticated == true)
            {


                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Index", "Home");



            }




            
            //string UserName = @User.Identity.Name.ToString();

           
            //var U = (from P in context.AspNetUsers
            //         where P.UserName == UserName
            //         select P.Id).SingleOrDefault();


            //var role = (from ast in context.AspNetUsers
            //                 where ast.RoleBinID == 1 && ast.Id == U
            //                 select ast.RoleBinID).Count();

            //ViewBag.Role = role;


            //var user = (from ast in context.AspNetUsers
            //            where ast.Id == U
            //            select ast).SingleOrDefault();

            //ViewBag.Active = user.Active;
            //ViewBag.FTLoggedIn = user.firstTimeLoggedIn;

            //// Check for posts
            //var postcount = (from P in context.Posts
            //         where P.ExpirationDate > DateTime.Now
            //         select P.PostID).Count();

            //ViewBag.PostCount = postcount;


            return View();


        }


        public ActionResult _WWCLocations()
        {
           


            return View();


        }


        public ActionResult Contact()
        {

            var users = new List<UserProfileViewModel>();
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
                          where k.Coordinator == true && x.Active == true

                          select new UserProfileViewModel()
                          {
                              Id = x.Id,
                              UserName = x.UserName,
                              FirstName = x.FirstName + " " + x.LastName,
                              TelephoneNumber = x.TelephoneNumber,
                              OrganizationID = x.OrganizationID,
                              profilePic = x.ProfilePic,
                              AgencySiteName = "State Staff",
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
           

            return View(users);


        }
        public ActionResult _Tiles()
        {
            ViewBag.Message = "Welcome to the Colorado Health Informatics Data System";


            string UserName = @User.Identity.Name.ToString();


            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();


            var role = (from ast in context.AspNetUsers
                        where ast.RoleBinID == 1 && ast.Id == U
                        select ast.RoleBinID).Count();

            ViewBag.Role = role;


            //var user = (from ast in context.AspNetUsers
            //            where ast.Id == U
            //            select ast).SingleOrDefault();

            //ViewBag.Active = user.Active;
            //ViewBag.FTLoggedIn = user.firstTimeLoggedIn;

            //// Check for posts
            //var postcount = (from P in context.Posts
            //         where P.ExpirationDate > DateTime.Now
            //         select P.PostID).Count();

            //ViewBag.PostCount = postcount;


            return PartialView();


        }


        public ActionResult Success()
        {
            ViewBag.Message = "Success! You have been added to the Connect To Leadership Registry System!";


            string UserName = @User.Identity.Name.ToString();


            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();


            var role = (from ast in context.AspNetUsers
                        where ast.RoleBinID == 1 && ast.Id == U
                        select ast.RoleBinID).Count();

            ViewBag.Role = role;


            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;



            return View();


        }

    }
}
