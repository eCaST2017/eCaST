using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Web.Routing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Web.Mvc.Html;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using CTL.Models;
using CTL.ViewModels;

namespace CTL.Controllers

{
    
    public class ContentsController : Controller
    {


        
        private ApplicationDbContext context = new ApplicationDbContext();



        // 
        // GET: /Contents/

        public ActionResult Index()
        {
            return View(context.Contents.ToList());
        }


        [Authorize]
        public ActionResult _ContentList()

        {

            var ContentsCount = (from ast in context.Contents
                               
                                where ast.Active == true
                                select ast.ContentID).Count();

            ViewBag.ContentsCount = ContentsCount;


            return PartialView(context.Contents.ToList());



        }

       
        public ActionResult _SubContentList(int? id)
        {


            var SubContentItems = new List<Content>();
            List<SelectListItem> items1 = new List<SelectListItem>();


            var subcontent  = (from ast in context.ContentContents
                               //join ask in context.Contents
                               //on ast.ContentID equals ask.ContentID
                               where ast.ContentID == id
                               select ast.ContentListID).ToList();

            foreach (var c in subcontent)
            {

                var Data = (from P in context.Contents
                            where P.ContentID == c
                            select new
                            {
                                ContentID = P.ContentID,
                                ContentTitle = P.ContentTitle

                            }).Distinct();


                int ContentID = Data.FirstOrDefault().ContentID;
                string CID = ContentID.ToString();
                string ContentTitle = Data.FirstOrDefault().ContentTitle;

                bool alreadyExists = items1.Any(x => x.Value == CID);

                if (alreadyExists == false)
                {


                    items1.Add(new SelectListItem { Text = ContentTitle, Value = CID, Selected = false });

                }


            }

            foreach (SelectListItem item in items1)
            {

                int it = Convert.ToInt32(item.Value);


                var sub = (from ast in context.Contents
                                  //join ask in context.Contents
                                  //on ast.ContentID equals ask.ContentID
                                  where ast.ContentID == it
                                  select ast).ToList();




                foreach (Content co in sub)
                {

                    SubContentItems.Add(co);

                }

            }


            return PartialView(SubContentItems);



        }


        [Authorize]
        public ActionResult _AssetList()
        
        {
            
            
        return PartialView(context.Assets.ToList());


        }


       
        public ActionResult _ContentNavList()
        {

            string UserName = @User.Identity.GetUserName();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

            if (Uu == 1)
            {

                ViewBag.RoleID = 1;
                ViewBag.Id = U;

            }
            else
            {
                ViewBag.RoleID = 2;
                ViewBag.Id = U;


            }


            var contents = (from x in context.Contents
                              join l in context.ContentContents on x.ContentID equals l.ContentID into joinedProc
                              from joinedP in joinedProc.DefaultIfEmpty()
                              where x.Active == true && x.IsMainPage == true && x.IsSecure == false
                              orderby x.pageOrder ascending

                              select new ContentViewModel()
                              {
                                  ContentID = x.ContentID,
                                  ContentTypeID = x.ContentTypeID,
                                  ContentTitle = x.ContentTitle,
                                  ContentBody = x.ContentBody,
                                  SubContentCount = joinedProc.Count(),
                                  Active = x.Active,
                                  IsMainPage = x.IsMainPage,
                                  IsSecure = x.IsSecure,
                                  pageOrder = x.pageOrder

                              }).Distinct().OrderBy(x => x.pageOrder).ToList();



            return PartialView(contents);
        }


        public ActionResult _SecureContentNavList()
        {

            string UserName = @User.Identity.GetUserName();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            var Uu = (from P in context.AspNetUsers
                      where P.Id == U
                      select P.RoleBinID).SingleOrDefault();

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

            var contents = (from x in context.Contents
                            join l in context.ContentContents on x.ContentID equals l.ContentID into joinedProc
                            from joinedP in joinedProc.DefaultIfEmpty()

                            where x.Active == true && x.IsMainPage == true
                            orderby x.pageOrder ascending

                            select new ContentViewModel()
                            {
                                ContentID = x.ContentID,
                                ContentTypeID = x.ContentTypeID,
                                ContentTitle = x.ContentTitle,
                                ContentBody = x.ContentBody,
                                SubContentCount = joinedProc.Count(),
                                Active = x.Active,
                                IsMainPage = x.IsMainPage,
                                IsSecure = x.IsSecure,
                                pageOrder = x.pageOrder

                            }).Distinct().OrderBy(x => x.pageOrder).ToList();



            return PartialView(contents);



        }





        // GET: /Contents/Details/5
        
        public ActionResult Details(int id)
        {

            Content content = context.Contents.Single(x => x.ContentID == id);

            string UserName = @User.Identity.GetUserName();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

         

            ViewBag.PageID = id;

            return PartialView(content);
        }

       

        //
        // GET: /Contents/Create
        [Authorize]
        public ActionResult Create()
        {


            PopulateInitialContentData();

            return View();
        } 

        //
        // POST: /Contents/Create
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Content content, FormCollection formCollection, string[] selectedContents)
        {


            PopulateInitialContentData();

            if (ModelState.IsValid)
            {
               
                
                context.Contents.Add(content);
                context.SaveChanges();

                int Id1 = content.ContentID;
                ViewBag.IdHolder = Id1;


                var contentToUpdate = context.Contents
                    //.Include(i => i.Races)
                                .Where(i => i.ContentID == Id1)
                                .Single();

                if (TryUpdateModel(contentToUpdate, "", null, new string[] { "Contents" }))
                {
                    try
                    {

                        //InsertContentContents(selectedContents, contentToUpdate);


                    }
                    catch (DataException)
                    {
                        //Log the error (add a variable name after DataException)
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    }

                }
                context.SaveChanges();



                return RedirectToAction("Manage", "Dashboards");  
            }

            return View(content);
        }
        
        //
        // GET: /Contents/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Content content = context.Contents.Single(x => x.ContentID == id);

            var U = (from P in context.Contents
                     where P.ContentID == id
                     select P.ContentTitle).SingleOrDefault();


            PopulateAssignedContentData(content);

            ViewBag.ContentTitle = U;

            var IMP = (from P in context.Contents
                     where P.ContentID == id
                     select P.IsMainPage).SingleOrDefault();

            ViewBag.IMP = IMP;

            return View(content);


        }

        //
        // POST: /Contents/Edit/5
        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(int id, FormCollection formCollection, string[] selectedContents)
        {


            //var clientToUpdate = context.Clients
            //  .Include(i => i.Races)
            //  .Where(i => i.ClientID == id)
            //  .Single();

            var contentToUpdate = (from c in context.Contents
                                  //join cc in context.RaceClients
                                  //on c.ClientID equals cc.ClientID
                                  where c.ContentID == id
                                  select c).SingleOrDefault();




            if (TryUpdateModel(contentToUpdate, "", null, new string[] { "Contents" }))
            {
                try
                {


                    UpdateContentContents(selectedContents, contentToUpdate);

                    context.Entry(contentToUpdate).State = EntityState.Modified;



                    context.SaveChanges();
                    //return RedirectToAction("Index");
                    return Redirect(Url.Action("Manage", "Dashboards", new RouteValueDictionary(new { id = id })));

                }
                catch (DataException)
                {
                    //Log the error (add a variable name after DataException)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedContentData(contentToUpdate);


            return View(contentToUpdate);





        }

        //
        // GET: /Contents/Delete/5
 [Authorize]
        public ActionResult Delete(int id)
        {
            Content content = context.Contents.Single(x => x.ContentID == id);
            return View(content);
        }

        //
        // POST: /Contents/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = context.Contents.Single(x => x.ContentID == id);
            context.Contents.Remove(content);
            context.SaveChanges();
            return RedirectToAction("Manage", "Dashboards");
        }


        private void PopulateAssignedContentData(Content content)
        {
            var allContents = context.Contents.Where(c => c.Active == true && c.IsMainPage != true);

            var contents = (from c in context.Contents
                            join cc in context.ContentContents
                            on c.ContentID equals cc.ContentID
                            where cc.ContentID == content.ContentID
                            select cc.ContentListID).ToList();


            var contentcontents = new HashSet<int>(contents);


            var viewModel = new List<AssignedContents>();
            foreach (var contenta in allContents)
            {
                viewModel.Add(new AssignedContents
                {
                    ContentID = contenta.ContentID,
                    ContentName = contenta.ContentTitle,
                    Assigned = contentcontents.Contains(contenta.ContentID)
                });
            }
            ViewBag.Contents = viewModel;
        }


        private void PopulateInitialContentData()
        {

            var allContents = context.Contents.Where(c => c.Active == true && c.IsMainPage != true);
            //var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedContents>();
            foreach (var content in allContents)
            {

                viewModel.Add(new AssignedContents
                {
                    ContentID = content.ContentID,
                    ContentName = content.ContentTitle,
                    Assigned = false
                    //Active = race.Active.Equals(true)
                });

            }

            ViewBag.Contents = viewModel;
        }

        private void UpdateContentContents(string[] selectedContents, Content contentToUpdate)
        {
            if (selectedContents == null)
            {
                //clientToUpdate.Races = new List<Race>();

                var contents = (from c in context.ContentContents
                                where c.ContentID == contentToUpdate.ContentID
                                select c).ToList();

                // Remove Contents
                foreach (ContentContents r in contents)
                {

                    context.ContentContents.Remove(r);
                    context.SaveChanges();
                }


                return;
            }

            var selectedContentContentsHS = new HashSet<string>(selectedContents);


            // Get Races
            var contents1 = (from cc in context.ContentContents
                             where cc.ContentID == contentToUpdate.ContentID
                             select cc).ToList();

            // Remove Contents
            foreach (ContentContents r in contents1)
            {

                context.ContentContents.Remove(r);
                context.SaveChanges();
            }



            // Add Selected Contents
            foreach (string sel in selectedContentContentsHS)
            {


                string UserName1 = Membership.GetUser().UserName.ToString();
                DateTime Created1 = DateTime.Now;
                int sel1 = Convert.ToInt32(sel);

                ContentContents cc = new ContentContents
                {
                    ContentListID = sel1,
                    ContentID = contentToUpdate.ContentID,
                    //DateUpdated = Created1,
                    //UpdatedBy = UserName1

                };

                context.ContentContents.Add(cc);
                context.SaveChanges();


            }


        }

        private void InsertContentContents(string[] selectedContents, Content contentToUpdate)
        {
            if (selectedContents == null)
            {

                var contents = (from c in context.Contents
                             join cc in context.ContentContents
                             on c.ContentID equals cc.ContentListID
                             where cc.ContentID == contentToUpdate.ContentID
                             select c.ContentID).ToList();



                return;
            }

            var selectedContentContentsHS = new HashSet<string>(selectedContents);



            // Add Selected Contents
            foreach (string sel in selectedContentContentsHS)
            {
                string UserName = Membership.GetUser().UserName.ToString();
                DateTime Created = DateTime.Now;
                int sel1 = Convert.ToInt32(sel);

                ContentContents cc = new ContentContents
                {

                    ContentListID = sel1,
                    ContentID = contentToUpdate.ContentID,
                    //CreatedBy = UserName,
                    //DateCreated = Created,
                    //UpdatedBy = UserName,
                    //DateUpdated = Created


                };

                context.ContentContents.Add(cc);
                context.SaveChanges();


            }


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