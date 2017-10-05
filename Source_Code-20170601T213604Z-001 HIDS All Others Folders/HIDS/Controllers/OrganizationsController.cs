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
    public class OrganizationsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Organizations/
         [Authorize]
        public ViewResult Index()
        {
            return View(context.Organizations.ToList());
        }


         [Authorize]
         public ActionResult _AddOrganization()
         {

             ViewBag.PossibleStateBins = context.StateBins;


             return PartialView();
         }



         [Authorize]
         public ActionResult _ActiveOrganizations()
         {

             var orgCount = (from ast in context.Organizations
                             where ast.Active == true
                             select ast.OrganizationID).Count();

             ViewBag.OrgCount = orgCount;

             ViewBag.OrgMode = 0;

             var asset = (from x in context.Organizations
                          join k in context.AspNetUsers on x.OrganizationID equals k.OrganizationID into joinedProc2
                          from joinedP2 in joinedProc2.DefaultIfEmpty()

                          select new OrganizationsViewModel()
                          {
                             OrganizationID = x.OrganizationID,
                             OrganizationName = x.OrganizationName,
                             OrganizationSubName = x.OrganizationSubName,
                             MailingAddress = x.MailingAddress,
                             CityName = x.CityName,
                             ZipCode = x.ZipCode,
                             Active = x.Active,
                             MemberCount = joinedProc2.Count()



                          }).Distinct().ToList();


             return PartialView(asset);
         }


         [Authorize]
         public ActionResult _OrganizationList()
         {

             var orglist = (from x in context.Organizations
                            join w in context.StateBins
                            on x.StateBinID equals w.StateBinID
                            into ff
                            from gg in ff.DefaultIfEmpty()
                           where x.Active == true
                           select new OrganizationViewModel()
                           {
                               OrganizationID = x.OrganizationID,
                               OrganizationName = x.OrganizationName,
                               MailingAddress = x.MailingAddress,
                               CityName = x.CityName,
                               ZipCode = x.ZipCode,
                               StateBinName = gg.StateBinName,
                               OrgPic = x.OrgPic,
                               Active = x.Active,
                               //DateCreated = x.DateCreated,
                               //CreatedBy = x.CreatedBy

                           }).ToList();


             return PartialView(orglist);


         }


        //
        // GET: /Organizations/Details/5
         [Authorize]
        public ActionResult Details(int id)
        {
            //Organization organization = context.Organizations.Single(x => x.OrganizationID == id);

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

            ViewBag.Role = Uu;

            var active = (from ast in context.AspNetUsers
                          where ast.Id == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;


            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

            var Oname = (from P in context.Organizations
                         where P.OrganizationID == O
                         select P.OrganizationName).SingleOrDefault();

             // Opportunity Count
            //var opcount = (from P in context.PostOrganizations
            //             where P.OrganizationID == O
            //             select P.PostID).Count();

            //ViewBag.OpCount = opcount;


            // Opportunity Count
            var membercount = (from P in context.AspNetUsers
                           where P.OrganizationID == O
                           select P.Id).Count();

            ViewBag.MemberCount = membercount;


            if (Uu == 1)
            {

                ViewBag.RoleID = 1;
                ViewBag.UID = U;

            }
            else if (Uu == 3)
            {
                ViewBag.RoleID = 3;
                ViewBag.UID = U;


            }
            else
            {
                ViewBag.RoleID = 2;
                ViewBag.UID = U;


            }



            var organization = (from x in context.Organizations
                               join u in context.StateBins
                               on x.StateBinID equals u.StateBinID
                               //join c in context.Organizations
                               //on x.OrganizationID equals c.OrganizationID
                               where x.OrganizationID == id
                               select new OrganizationViewModel()
                               {
                                   OrganizationID = x.OrganizationID,
                                   OrgPic = x.OrgPic,
                                   OrganizationName = x.OrganizationName,
                                   MailingAddress = x.MailingAddress,
                                   CityName = x.CityName,
                                   StateBinName = u.StateBinName,
                                   ZipCode = x.ZipCode,
                                   Active = x.Active,
                                   DateUpdated = x.DateUpdated


                               }).Distinct().SingleOrDefault();

            
             ViewBag.UID = U;
            ViewBag.OID = O;
            ViewBag.OName = Oname;
             
             
             return View(organization);
        }


         public ActionResult _Activate(int? id)
         {

             string UserName1 = @User.Identity.Name.ToString();

             var U = (from P in context.Organizations
                      where P.OrganizationID == id
                      select P.OrganizationID).SingleOrDefault();

             var Un = (from P in context.Organizations
                       where P.OrganizationID == id
                       select P.OrganizationName).SingleOrDefault();

             ViewBag.AOrgID = id;
             ViewBag.AOrgName = Un;

             var Ua = (from P in context.Organizations
                       where P.OrganizationID == id
                       select P.Active).SingleOrDefault();

             ViewBag.Activate = Ua;

             //var Uu = (from P in context.AspNetUsers
             //          where P.Id == U
             //          select P.RoleBinID).SingleOrDefault();

             //ViewBag.Role = Uu;

             //var active = (from ast in context.AspNetUsers
             //              where ast.Id == U
             //              select ast.Active).SingleOrDefault();

             //ViewBag.Active = active;

             //var O = (from P in context.AspNetUsers
             //         where P.UserName == UserName1
             //         select P.OrganizationID).SingleOrDefault();

             //var Oname = (from P in context.Organizations
             //             where P.OrganizationID == O
             //             select P.OrganizationName).SingleOrDefault();


             //ViewBag.UID = id;
             //ViewBag.OID = O;
             //ViewBag.OName = Oname;


             Organization organization = context.Organizations.Single(x => x.OrganizationID == id);


             ViewBag.DateUpdated = DateTime.Now;

             string UserName = @User.Identity.Name.ToString();

             ViewBag.UpdatedBy = UserName;

             ViewBag.DateCreated = organization.DateCreated;
             ViewBag.CreatedBy = organization.CreatedBy;
             ViewBag.Status = organization.Active;



             return PartialView(organization);


         }



         public JsonResult _ActivateOrgF(Organization aorg)
         {

             if (ModelState.IsValid)
             {

                 // Set Record Info
                 string UserName = @User.Identity.Name.ToString();
                 DateTime CreatedInit = DateTime.Now;

                 var name = (from x in context.Organizations
                             where x.OrganizationID == aorg.OrganizationID
                             select x.OrganizationName).FirstOrDefault();

                 if (aorg.Active == false)
                 {

                     //Flip IsApproved Flag
                     Organization org1 = context.Organizations.Single(p => p.OrganizationID == aorg.OrganizationID);
                     org1.Active = true;
                     org1.UpdatedBy = name;
                     org1.DateUpdated = CreatedInit;
                     context.SaveChanges();


                 }


                 return Json(new { Status = "Success", Modified = aorg.OrganizationName }, JsonRequestBehavior.AllowGet);


             }


             return Json(new { Status = "Success", Modified = aorg.OrganizationName }, JsonRequestBehavior.AllowGet);


         }


         public JsonResult _CreateOrgF(Organization org)
         {

             if (ModelState.IsValid)
             {

                 // Set Record Info
                 string UserName = @User.Identity.Name.ToString();
                 DateTime CreatedInit = DateTime.Now;

                 var name = (from x in context.AspNetUsers
                             where x.UserName == UserName
                             select x.FirstName + " " + x.LastName).FirstOrDefault();

                 Organization orgs = new Organization
                 {

                    OrganizationName = org.OrganizationName,
                    OrganizationSubName = org.OrganizationSubName,
                    Active = org.Active,
                    MailingAddress = org.MailingAddress,
                    CityName = org.CityName,
                    ZipCode = org.ZipCode,
                    StateBinID = org.StateBinID

                 };

                 context.Organizations.Add(orgs);
                 context.SaveChanges();


                 return Json(new { Status = "Success", Modified = org.OrganizationName }, JsonRequestBehavior.AllowGet);


             }


             return Json(new { Status = "Success", Modified = org.OrganizationName }, JsonRequestBehavior.AllowGet);


         }

        //
        // GET: /Organizations/Create
        // [Authorize]
        //public ActionResult Create()
        //{

        //    ViewBag.PossibleStateBins = context.StateBins;
        //    string UserName = @User.Identity.Name.ToString();

         
        //    DateTime Created = DateTime.Now;
        //    DateTime Created1 = DateTime.Now;

        //    ViewBag.DateCreated = Created;
        //    ViewBag.DateUpdated = Created1;

        //    ViewBag.UpdatedBy = UserName;
        //    ViewBag.CreatedBy = UserName;

        //    //ViewBag.PossibleStateBins = context.StateBins;
        //    return PartialView();
        //}

         // GET: /Organizations/Create
         [Authorize]
         public ActionResult _CreateOrg()
         {

             ViewBag.PossibleStateBins = context.StateBins;
             string UserName = @User.Identity.Name.ToString();


             DateTime Created = DateTime.Now;
             DateTime Created1 = DateTime.Now;

             ViewBag.DateCreated = Created;
             ViewBag.DateUpdated = Created1;

             ViewBag.UpdatedBy = UserName;
             ViewBag.CreatedBy = UserName;

             //ViewBag.PossibleStateBins = context.StateBins;
             return PartialView();
         } 

        //
        // POST: /Organizations/Create
        // [Authorize]
        //[HttpPost]
        //public ActionResult Create(Organization organization)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Organizations.Add(organization);
        //        context.SaveChanges();

        //        return Redirect(Url.RouteUrl(new { controller = "DashBoards", action = "Manage" }) + "#" + "tabs-5");
        //        //return RedirectToAction("Index");  
        //    }

        //    //ViewBag.PossibleStateBins = context.StateBins;
        //    return View(organization);
        //}
        
        //
        // GET: /Organizations/Edit/5
        //[Authorize]
        //public ActionResult Edit(int id)
        //{

            
        //    string UserName = @User.Identity.Name.ToString();

        //    var U = (from P in context.AspNetUsers
        //             where P.UserName == UserName
        //             select P.Id).SingleOrDefault();

        //    var Uu = (from P in context.AspNetUsers
        //              where P.Id == U
        //              select P.RoleBinID).SingleOrDefault();

        //    ViewBag.Role = Uu;

        //    var active = (from ast in context.AspNetUsers
        //                  where ast.Id == U
        //                  select ast.Active).SingleOrDefault();

        //    ViewBag.Active = active;

        //    ViewBag.PossibleStateBins = context.StateBins;
        //    //Organization organization = context.Organizations.Single(x => x.OrganizationID == id);

        //    var organization = (from x in context.Organizations
        //                        join y in context.StateBins
        //                        on x.StateBinID equals y.StateBinID
        //                  where x.OrganizationID == id 

        //                 select new OrganizationViewModel()
        //                 {
        //                     OrganizationID = x.OrganizationID,
        //                     OrganizationName = x.OrganizationName,
        //                     MailingAddress = x.MailingAddress,
        //                     CityName = x.CityName,
        //                     StateBinID = x.StateBinID,
        //                     StateBinName = y.StateBinName,
        //                     ZipCode = x.ZipCode,
        //                     OrgPic = x.OrgPic,
        //                     Active = x.Active

        //                 }).SingleOrDefault();


        //    //ViewBag.PossibleStateBins = context.StateBins;
        //    return View(organization);
        //}


         [Authorize]
         public ActionResult Edit(int id)
         {

             string UserName = @User.Identity.Name.ToString();

             var U = (from P in context.AspNetUsers
                      where P.UserName == UserName
                      select P.Id).SingleOrDefault();

             var Uu = (from P in context.AspNetUsers
                       where P.Id == U
                       select P.RoleBinID).SingleOrDefault();

             ViewBag.Role = Uu;

             var active = (from ast in context.AspNetUsers
                           where ast.Id == U
                           select ast.Active).SingleOrDefault();

             ViewBag.Active = active;

             ViewBag.PossibleStateBins = context.StateBins;
             //Organization organization = context.Organizations.Single(x => x.OrganizationID == id);

             var organization = (from x in context.Organizations
                                 join y in context.StateBins
                                 on x.StateBinID equals y.StateBinID
                                 where x.OrganizationID == id

                                 select new OrganizationViewModel()
                                 {
                                     OrganizationID = x.OrganizationID,
                                     OrganizationName = x.OrganizationName,
                                     OrganizationSubName = x.OrganizationSubName,
                                     MailingAddress = x.MailingAddress,
                                     CityName = x.CityName,
                                     StateBinID = x.StateBinID,
                                     StateBinName = y.StateBinName,
                                     ZipCode = x.ZipCode,
                                     OrgPic = x.OrgPic,
                                     Active = x.Active

                                 }).SingleOrDefault();


             //ViewBag.PossibleStateBins = context.StateBins;
             return View(organization);
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
         public ActionResult _UpdateOrgPic(OrganizationViewModel organization, HttpPostedFileBase file00, int OrganizationID)
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



                 string filePath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Assets"), ScreeningGUID + filename);
                 System.IO.File.WriteAllBytes(filePath, ReadData(file00.InputStream));


                 //Get Org Id
                 Organization org = context.Organizations.Single(
                     X => X.OrganizationID == organization.OrganizationID);


              
                 //asset1.AssetContent = asset1.AssetContent + "<a href=" + "'" + filePath + "'" + " target='_blank'>" + Title + "</a>";
                 org.OrgPic = "<p><img src='" + "https://www.connecttoleadership.dphe.state.co.us/Assets/" + ScreeningGUID + fname + "'/></p>";
                 context.SaveChanges();

                 return RedirectToAction("Edit", "Organizations", new { id = organization.OrganizationID });

             }



             return View();


         }




         public JsonResult _EditOrgF(Organization org)
         {

             if (ModelState.IsValid)
             {

                 // Set Record Info

                 string UserName = @User.Identity.Name.ToString();
                 DateTime CreatedInit = DateTime.Now;

                 //Update Org Info
                 Organization orgU = context.Organizations.Single(p => p.OrganizationID == org.OrganizationID);
                 orgU.OrganizationName = org.OrganizationName;
                 orgU.OrganizationSubName = org.OrganizationSubName;
                 orgU.MailingAddress = org.MailingAddress;
                 orgU.CityName = org.CityName;
                 orgU.StateBinID = org.StateBinID;
                 orgU.ZipCode = org.ZipCode;
                 orgU.Active = org.Active;
                 orgU.DateUpdated = CreatedInit;
                 orgU.UpdatedBy = UserName;
                 context.SaveChanges();


                 return Json(new { Status = "Success", Modified = org.OrganizationName }, JsonRequestBehavior.AllowGet);

             }


             return Json(new { Status = "Success", Modified = org.OrganizationName }, JsonRequestBehavior.AllowGet);


         }


        //
        // POST: /Organizations/Edit/5
        [Authorize]
        [HttpPost]
        public ActionResult Edit(Organization organization)
        {
            if (ModelState.IsValid)
            {
                context.Entry(organization).State = EntityState.Modified;
                context.SaveChanges();
                
                return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }) + "#" + "tabs-5");
                //return RedirectToAction("Index");
            }
            //ViewBag.PossibleStateBins = context.StateBins;
            return View(organization);
        }

        //
        // GET: /Organizations/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            Organization organization = context.Organizations.Single(x => x.OrganizationID == id);
            return PartialView(organization);
        }


        public JsonResult _DeleteOrgF(Organization org)
        {

            if (ModelState.IsValid)
            {

                Organization organization = context.Organizations.Single(x => x.OrganizationID == org.OrganizationID);
                context.Organizations.Remove(organization);
                context.SaveChanges();

                string o = organization.OrganizationName; 

                return Json(new { Status = "Success", Modified = o, Modified2 = org.OrganizationID }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = org.OrganizationName }, JsonRequestBehavior.AllowGet);


        }



        //
        // POST: /Organizations/Delete/5
         [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Organization organization = context.Organizations.Single(x => x.OrganizationID == id);
            context.Organizations.Remove(organization);
            context.SaveChanges();

            //var postorgs = (from cc in context.PostOrganizations
            //                       where cc.OrganizationID == id 
            //                       select cc).ToList();

            //// Remove Posts
            //foreach (PostOrganizations po in postorgs)
            //{

            //    context.PostOrganizations.Remove(po);
            //    context.SaveChanges();
            //}

            
             return Redirect(Url.RouteUrl(new { controller = "Dashboards", action = "Manage" }) + "#" + "tabs-5");
             //return RedirectToAction("Index");
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