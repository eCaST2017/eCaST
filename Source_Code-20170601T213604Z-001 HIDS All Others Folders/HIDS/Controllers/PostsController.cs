using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CTL.Models;
using CTL.ViewModels;
using System.Collections;
using System.Web.Script.Serialization;


namespace CTL.Controllers
{   
    public class PostsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Posts/

        public ViewResult Index()
        {

            // Check for posts
            var postcount = (from P in context.Posts
                             where P.ExpirationDate > DateTime.Now
                             select P.PostID).Count();

            ViewBag.PostCount = postcount;


            return View(context.Posts.ToList());
        }


        [Authorize]
        public ActionResult _PostList(int? id)
        {

            var postlist = (from x in context.Posts
                         join y in context.PostOrganizations
                         on x.PostID equals y.PostID
                         join l in context.ProfileUserPosts on x.PostID equals l.PostID into joinedProc
                         from joinedP in joinedProc.DefaultIfEmpty()
                         where y.OrganizationID == id

                         select new PostViewModel()
                         {
                             PostID = x.PostID,
                             PostTitle = x.PostTitle,
                             ExpirationDate = x.ExpirationDate,
                             PostFollowersCount = joinedProc.Count(),
                             AwardedId = x.AwardedId,
                             Active = x.Active,
                             DateCreated = x.DateCreated,
                             CreatedBy = x.CreatedBy

                         }).Distinct().ToList();


            return PartialView(postlist);


        }


        public ActionResult _ExternalPostList()
        {


            DateTime exDate = DateTime.Now;

            var postlist = (from x in context.Posts
                            join y in context.PostOrganizations
                            on x.PostID equals y.PostID
                            join z in context.Organizations
                            on y.OrganizationID equals z.OrganizationID
                            where x.ExpirationDate > exDate
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                OrganizationName = z.OrganizationName,
                                ExpirationDate = x.ExpirationDate,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).ToList();


            return PartialView(postlist);


        }


        [Authorize]
        public ActionResult OpportunityList()
        {

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            DateTime exDate = DateTime.Now;

            var oplist = (from x in context.Posts
                          join y in context.PostOrganizations
                          on x.PostID equals y.PostID
                          //join l in context.ProfileUserPosts on x.PostID equals l.PostID into joinedProc
                          //from joinedP in joinedProc.DefaultIfEmpty() 
                          join z in context.Organizations
                          on y.OrganizationID equals z.OrganizationID
                          where x.ExpirationDate > exDate 
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                PostContent = x.PostContent,
                                //IsFollowed = joinedProc.Count(),
                                ExpirationDate = x.ExpirationDate,
                                OrgPic = z.OrgPic,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).ToList();


            return PartialView(oplist);


        }


        [Authorize]
        public ActionResult FollowList()
        {

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();


            DateTime exDate = DateTime.Now;

            var oplist = (from x in context.Posts
                          join y in context.PostOrganizations
                          on x.PostID equals y.PostID
                          join u in context.ProfileUserPosts
                          on x.PostID equals u.PostID
                          join z in context.Organizations
                          on y.OrganizationID equals z.OrganizationID
                          where x.ExpirationDate > exDate && u.Id == U
                          select new PostViewModel()
                          {
                              PostID = x.PostID,
                              PostTitle = x.PostTitle,
                              PostContent = x.PostContent,
                              ExpirationDate = x.ExpirationDate,
                              OrgPic = z.OrgPic,
                              Active = x.Active,
                              DateCreated = x.DateCreated,
                              CreatedBy = x.CreatedBy

                          }).ToList();


            return PartialView(oplist);


        }


        public ActionResult _LocationDetail(int? id)
        {

            var countylist = (from P in context.CountyBinPosts
                           join Y in context.CountyBins
                           on P.CountyBinID equals Y.CountyBinID
                           where P.PostID == id
                           select Y.CountyBinName).ToList();


            string[] ResultList = countylist.ToArray();
            string ResultString = string.Join(",", ResultList.ToArray());

            var colist = (from c in context.Posts
                           where c.PostID == id
                           select new PostViewModel()
                           {

                               Counties = ResultString


                           }).Distinct().ToList();



            return PartialView(colist);

        }

        public ActionResult _AttributeDetail(int? id, int? id2)
        {

            var attlist = (from P in context.AttributePosts
                                join Y in context.Attributes
                                on P.AttributeID equals Y.AttributeID
                                where Y.AttributeParentID == id2 && P.PostID == id 
                                select Y.AttributeName).ToList();


            string[] ResultList = attlist.ToArray();
            string ResultString = string.Join(",", ResultList.ToArray());

            var faclist = (from c in context.Posts
                           where c.PostID == id
                           select new PostViewModel()
                           {

                              MeetingTimes = ResultString


                           }).Distinct().ToList();



            return PartialView(faclist);

        }


        public ActionResult _AwardPost(int id)
        {

            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            ViewBag.Id = U;

            var Uo = (from P in context.AspNetUsers
                      where P.UserName == UserName
                      select P.OrganizationID).SingleOrDefault();

            ViewBag.OrgID = Uo;



            ViewBag.PossibleUsers = context.AspNetUsers.Where(x => x.Active == true);

            var postlist = (from x in context.Posts
                            //join y in context.ProfileUserPosts
                            //on x.PostID equals y.PostID
                            //join z in context.AspNetUsers
                            //on y.Id equals z.Id
                            //join w in context.StateBins
                            //on z.StateBinID equals w.StateBinID
                            where x.PostID == id
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                PostContent = x.PostContent,
                                ExpirationDate = x.ExpirationDate,
                                AwardedId = x.AwardedId,
                                //Id = z.Id,
                                //UserName = z.UserName,
                                //FirstName = z.FirstName,
                                //LastName = z.LastName,
                                //PhoneNumber = z.PhoneNumber,
                                //Address = z.Address,
                                //City = z.City,
                                //StateID = z.StateBinID,
                                //StateBinName = w.StateBinName,
                                //ZipCode = z.ZipCode,
                                //Fax = z.Fax,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).SingleOrDefault();




            return PartialView(postlist);
        }


        public ActionResult _Following(int id)
        {
            
            var postlist = (from x in context.Posts
                            //join y in context.ProfileUserPosts
                            //on x.PostID equals y.PostID
                            //join z in context.AspNetUsers
                            //on y.Id equals z.Id
                            //join w in context.StateBins
                            //on z.StateBinID equals w.StateBinID
                            where x.PostID == id
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                PostContent = x.PostContent,
                                ExpirationDate = x.ExpirationDate,
                                //Id = z.Id,
                                //UserName = z.UserName,
                                //FirstName = z.FirstName,
                                //LastName = z.LastName,
                                //PhoneNumber = z.PhoneNumber,
                                //Address = z.Address,
                                //City = z.City,
                                //StateID = z.StateBinID,
                                //StateBinName = w.StateBinName,
                                //ZipCode = z.ZipCode,
                                //Fax = z.Fax,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).SingleOrDefault();




            return PartialView(postlist);
        }

        [Authorize]
        public ActionResult _UserFollowList(int? id)
        {
            var asset = (from x in context.AspNetUsers
                         join y in context.ProfileUserPosts
                         on x.Id equals y.Id
                         where x.Active == true && y.PostID == id

                         select new UserProfileViewModel()
                         {
                             Id = x.Id,
                             UserName = x.UserName,
                             FirstName = x.FirstName + " " + x.LastName,
                             TelephoneNumber = x.PhoneNumber,
                             profilePic = x.ProfilePic,
                             //PartnerTypeName = g.PartnerTypeBinName,
                             OrganizationName = x.OrganizationName,
                             //RoleBinName = z.RoleBinName


                         }).ToList();


            ViewBag.PostID = id;

            return PartialView(asset);


        }


        //
        // GET: /Posts/Details/5

        public ActionResult Details(int id)
        {
           // Post post = context.Posts.Single(x => x.PostID == id);


            string UserName = @User.Identity.Name.ToString();

            var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();


            var fc = (from P in context.ProfileUserPosts
                     where P.PostID == id && P.Id == U
                     select P.PostID).Count();

            ViewBag.FollowCheck = fc;


            var postlist = (from x in context.Posts
                            join y in context.PostOrganizations
                            on x.PostID equals y.PostID
                            join z in context.Organizations
                            on y.OrganizationID equals z.OrganizationID
                            where x.PostID == id
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                PostContent = x.PostContent,
                                OrganizationName = z.OrganizationName,
                                OrgPic = z.OrgPic,
                                ExpirationDate = x.ExpirationDate,
                                OpportunityOpenDate = x.OpportunityOpenDate,
                                OpportunityExpirationDate = x.OpportunityExpirationDate,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).SingleOrDefault();




            return PartialView(postlist);
        }

        //
        // GET: /Posts/Create

        public ActionResult Create()
        {

            string UserName = @User.Identity.Name.ToString();

             var U = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.Id).SingleOrDefault();

            ViewBag.Id = U;

            var Uo = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

            ViewBag.OrgID = Uo;

            ViewBag.PossiblePostTypes = context.PostTypes;


            // Focus Areas
            IEnumerable OptionFA = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 9).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFA = new SelectList(OptionFA, "Value", "Text");
            ViewBag.OptionListFA = NewtermsFA;


            IEnumerable OptionFMW = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 12).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMW = new SelectList(OptionFMW, "Value", "Text");
            ViewBag.OptionListFMW = NewtermsFMW;


            IEnumerable OptionFMY = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 13).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMY = new SelectList(OptionFMY, "Value", "Text");
            ViewBag.OptionListFMY = NewtermsFMY;


            IEnumerable OptionFMYC = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 14).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMYC = new SelectList(OptionFMYC, "Value", "Text");
            ViewBag.OptionListFMYC = NewtermsFMYC;



            // Meeting Times
            IEnumerable OptionMT = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 4).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsMT = new SelectList(OptionMT, "Value", "Text");
            ViewBag.OptionListMT = NewtermsMT;





            // Meeting Format
            IEnumerable OptionMF = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 6).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsMF = new SelectList(OptionMF, "Value", "Text");
            ViewBag.OptionListMF = NewtermsMF;


            // Expected Deliverables
            IEnumerable OptionED = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 7).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsED = new SelectList(OptionED, "Value", "Text");
            ViewBag.OptionListED = NewtermsED;

            // Compensation
            IEnumerable OptionCER = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 8).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsCER = new SelectList(OptionCER, "Value", "Text");
            ViewBag.OptionListCER = NewtermsCER;

            // Skills
            IEnumerable Option = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 1).AsEnumerable().Select(c => new SelectListItem()

            {
               
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var Newterms = new SelectList(Option, "Value", "Text");
            ViewBag.OptionList = Newterms;

            // Knowledge
            IEnumerable OptionK = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 2).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsK = new SelectList(OptionK, "Value", "Text");
            ViewBag.OptionListK = NewtermsK;

            // Abilities
            IEnumerable OptionA = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 3).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsA = new SelectList(OptionA, "Value", "Text");
            ViewBag.OptionListA = NewtermsA;

            // Counties
            IEnumerable OptionCounty = context.CountyBins.Where(c => c.Active == 1).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.CountyBinName,
                Value = c.CountyBinID.ToString(),
                Selected = true,
            });


            var NewtermsCounty = new SelectList(OptionCounty, "Value", "Text");
            ViewBag.OptionListCounty = NewtermsCounty;

            return PartialView();
        } 

        //
        // POST: /Posts/Create


        public JsonResult _FollowPostF(PostViewModel postviewmodel, string UserId, bool? Unfollow)
        {

            if (ModelState.IsValid)
            {


                if (Unfollow == false)
                {

                    // Set Record Info
                    string UserName = @User.Identity.Name.ToString();
                    DateTime CreatedInit = DateTime.Now;

                    var name = (from x in context.AspNetUsers
                                where x.UserName == UserName
                                select x.FirstName + " " + x.LastName).FirstOrDefault();

                    ProfileUserPosts proposts = new ProfileUserPosts
                    {

                        Id = UserId,
                        PostID = postviewmodel.PostID

                    };

                    context.ProfileUserPosts.Add(proposts);
                    context.SaveChanges();

                    string UserName0 = @User.Identity.Name.ToString();
                    var emailinfo = (from x in context.AspNetUsers
                                     where x.UserName == UserName0
                                     select x).FirstOrDefault();

                    var postinfo = (from x in context.Posts
                                    where x.PostID == postviewmodel.PostID
                                    select x).FirstOrDefault();

                    var posteremail = (from x in context.AspNetUsers
                                       where x.FirstName + " " + x.LastName == postinfo.CreatedBy
                                       select x.Email).FirstOrDefault();


                    SendFollowEmail(emailinfo.Email, emailinfo.FirstName, emailinfo.LastName, posteremail, postinfo.PostTitle, postviewmodel.FollowNotes);



                }
                else
                {


                    ProfileUserPosts profilepost = context.ProfileUserPosts.Single(x => x.PostID == postviewmodel.PostID && x.Id == UserId);
                    context.ProfileUserPosts.Remove(profilepost);
                    context.SaveChanges();



                }

              

                



                return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = UserId, Modified3 = Unfollow }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = UserId, Modified3 = Unfollow }, JsonRequestBehavior.AllowGet);


        }

        public void SendFollowEmail(string Email, string FirstName, string LastName, string PostEmail, string PostTitle, string FollowNotes)
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
            //mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.From = new MailAddress (Email);
            mail.To.Add(PostEmail);
            mail.CC.Add(Email);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Family Leadership Registry:" + " " + FirstName + " " + LastName + " is now following your opportunity: " + PostTitle;

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + "Hello, " + FirstName + " " + LastName + " is now following your opportunity: " + PostTitle + ".</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + FirstName + " " + LastName + " wrote: " + FollowNotes + ".</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> Login to follow up: " + "<a target='_blank' href='https://www.connecttoleadership.dphe.state.co.us'>Family Leadership Registry</a>" + "</p>";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<br>Thank you!.<br>Colorado Family Leadership Registry Support";
            mail.Body = mail.Body + "</TD>";
            mail.Body = mail.Body + "</TR>";
            mail.Body = mail.Body + "</TABLE>";
            mail.IsBodyHtml = true;

            //UNCOMMENT THIS LINE TO EMAIL
            SmtpClient smtp = new SmtpClient();
            smtp.Send(mail);
        }

        public JsonResult _AwardPostF(PostViewModel postviewmodel, int? Id, int? OrgID)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();

                var name = (from x in context.AspNetUsers
                            where x.UserName == UserName
                            select x.FirstName + " " + x.LastName).FirstOrDefault();



                DateTime CreatedInit = DateTime.Now;


                // Update Clinic Fields
                Post post = context.Posts.Single(p => p.PostID == postviewmodel.PostID);


                post.AwardedId = postviewmodel.AwardedId;
                post.UpdatedBy = name;
                post.DateUpdated = CreatedInit;

                context.SaveChanges();

                


                return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = Id, Modified3 = OrgID }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = Id, Modified3 = OrgID }, JsonRequestBehavior.AllowGet);


        }





        public JsonResult _AddPostF(PostViewModel postviewmodel, int? Id, int? OrgID)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();

                var name = (from x in context.AspNetUsers
                           where x.UserName == UserName
                           select x.FirstName + " " + x.LastName).FirstOrDefault();



                DateTime CreatedInit = DateTime.Now;

                Post post = new Post
                {


              PostTypeID = postviewmodel.PostTypeID,
              PostTitle = postviewmodel.PostTitle,
              PostContent = postviewmodel.PostContent,
              Active = postviewmodel.Active,
              OpenDate = postviewmodel.OpenDate,
              OpportunityOpenDate = postviewmodel.OpportunityOpenDate,
              OpportunityExpirationDate = postviewmodel.OpportunityExpirationDate,
              ExpirationDate = postviewmodel.ExpirationDate,
              DateCreated = CreatedInit,
              CreatedBy = name,
              DateUpdated = CreatedInit,
              UpdatedBy = name

            };

                context.Posts.Add(post);
                context.SaveChanges();

                int? PID = post.PostID;


                PostOrganizations postorgs = new PostOrganizations
                {


                    PostID = Convert.ToInt32(PID),
                    OrganizationID = Convert.ToInt32(OrgID)
                  

                };

                context.PostOrganizations.Add(postorgs);
                context.SaveChanges();



                if (postviewmodel.FocusAreas != null)
                {



                    // Focus Areas
                    var stringToSplitFA = postviewmodel.FocusAreas;

                    var queryFA = from valFA in stringToSplitFA.Split(',')
                                select Convert.ToInt32(valFA);
                    foreach (int valFA in queryFA)
                    {

                        var mts = (from x in context.Attributes
                                   where x.AttributeID == valFA
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atp = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valFA),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atp);
                        context.SaveChanges();


                    }

                }



                if (postviewmodel.Women != null)
                {



                    // Women
                    var stringToSplitFMW = postviewmodel.Women;

                    var queryFMW = from valFMW in stringToSplitFMW.Split(',')
                                   select Convert.ToInt32(valFMW);
                    foreach (int valFMW in queryFMW)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valFMW
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpuw = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valFMW),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpuw);
                        context.SaveChanges();


                    }

                }

                if (postviewmodel.Youth != null)
                {



                    // Youth
                    var stringToSplitFMY = postviewmodel.Youth;

                    var queryFMY = from valFMY in stringToSplitFMY.Split(',')
                                   select Convert.ToInt32(valFMY);
                    foreach (int valFMY in queryFMY)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valFMY
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpuy = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valFMY),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpuy);
                        context.SaveChanges();


                    }

                }


                if (postviewmodel.YoungChildren != null)
                {



                    // Young Children
                    var stringToSplitFMYC = postviewmodel.YoungChildren;

                    var queryFMYC = from valFMYC in stringToSplitFMYC.Split(',')
                                    select Convert.ToInt32(valFMYC);
                    foreach (int valFMYC in queryFMYC)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valFMYC
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpuyc = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valFMYC),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpuyc);
                        context.SaveChanges();


                    }

                }




                if (postviewmodel.MeetingTimes != null)
                {



                    // Meeting Times
                    var stringToSplit = postviewmodel.MeetingTimes;

                    var query = from val in stringToSplit.Split(',')
                                select Convert.ToInt32(val);
                    foreach (int val in query)
                    {

                        var mts = (from x in context.Attributes
                                   where x.AttributeID == val
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atp = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(val),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atp);
                        context.SaveChanges();


                    }

                }

                if (postviewmodel.MeetingFormat != null)
                {



                    // Meeting Format
                    var stringToSplitMF = postviewmodel.MeetingFormat;

                    var queryMF = from valMF in stringToSplitMF.Split(',')
                                select Convert.ToInt32(valMF);
                    foreach (int valMF in queryMF)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valMF
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpf = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valMF),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpf);
                        context.SaveChanges();


                    }

                }

                if (postviewmodel.ExpectedDeliveries != null)
                {



                    // Expected Deliveries
                    var stringToSplitED = postviewmodel.ExpectedDeliveries;

                    var queryED = from valED in stringToSplitED.Split(',')
                                  select Convert.ToInt32(valED);
                    foreach (int valED in queryED)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valED
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atped = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valED),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atped);
                        context.SaveChanges();


                    }

                }

                if (postviewmodel.Compensation != null)
                {



                    // Compensation
                    var stringToSplitC = postviewmodel.Compensation;

                    var queryC = from valC in stringToSplitC.Split(',')
                                  select Convert.ToInt32(valC);
                    foreach (int valC in queryC)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valC
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpc = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valC),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpc);
                        context.SaveChanges();


                    }

                }

                if (postviewmodel.Skills != null)
                {



                    // Skills
                    var stringToSplitS = postviewmodel.Skills;

                    var queryS = from valS in stringToSplitS.Split(',')
                                 select Convert.ToInt32(valS);
                    foreach (int valS in queryS)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valS
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atps = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valS),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atps);
                        context.SaveChanges();


                    }

                }


                if (postviewmodel.Knowledge != null)
                {



                    // Knowledge
                    var stringToSplitK = postviewmodel.Knowledge;

                    var queryK = from valK in stringToSplitK.Split(',')
                                 select Convert.ToInt32(valK);
                    foreach (int valK in queryK)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valK
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpk = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valK),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpk);
                        context.SaveChanges();


                    }

                }


                if (postviewmodel.Abilities != null)
                {



                    // Abilities
                    var stringToSplitA = postviewmodel.Abilities;

                    var queryA = from valA in stringToSplitA.Split(',')
                                 select Convert.ToInt32(valA);
                    foreach (int valA in queryA)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valA
                                   select x.AttributeParentID).FirstOrDefault();


                        AttributePosts atpa = new AttributePosts
                        {


                            AttributeID = Convert.ToInt32(valA),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.AttributePosts.Add(atpa);
                        context.SaveChanges();


                    }

                }


                if (postviewmodel.Counties != null)
                {



                    // Counties
                    var stringToSplitCO = postviewmodel.Counties;

                    var queryCO = from valCO in stringToSplitCO.Split(',')
                                 select Convert.ToInt32(valCO);
                    foreach (int valCO in queryCO)
                    {

                        var mfs = (from x in context.Attributes
                                   where x.AttributeID == valCO
                                   select x.AttributeParentID).FirstOrDefault();


                        CountyBinPosts atpco = new CountyBinPosts
                        {


                            CountyBinID = Convert.ToInt32(valCO),
                            PostID = Convert.ToInt32(PID),



                        };

                        context.CountyBinPosts.Add(atpco);
                        context.SaveChanges();


                    }

                }


                return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = Id, Modified3 = OrgID }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success", Modified = postviewmodel.PostTitle, Modified2 = Id, Modified3 = OrgID }, JsonRequestBehavior.AllowGet);


            }


               
        [HttpPost]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                context.Posts.Add(post);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.PossiblePostTypes = context.PostTypes;
            return View(post);
        }
        
        //
        // GET: /Posts/Edit/5
 
        public ActionResult Edit(int id)
        {
            //Post post = context.Posts.Single(x => x.PostID == id);
            ViewBag.PossiblePostTypes = context.PostTypes;


            // Focus Areas
            IEnumerable OptionFA = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 9).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFA = new SelectList(OptionFA, "Value", "Text");
            ViewBag.OptionListFA = NewtermsFA;

             
            var medoutFA = (from x in context.AttributePosts
                             join y in context.Attributes
                             on x.AttributeID equals y.AttributeID
                             join z in context.AttributeParents
                             on y.AttributeParentID equals z.AttributeParentID
                             where x.PostID == id && z.AttributeParentID == 9
                             select x.AttributeID).Count();

            if (medoutFA > 0)
            {


                var rowsFA =
                (from x in context.AttributePosts
                 join y in context.Attributes
                 on x.AttributeID equals y.AttributeID
                 join z in context.AttributeParents
                 on y.AttributeParentID equals z.AttributeParentID
                 where x.PostID == id && z.AttributeParentID == 9
                 select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

                ViewBag.OptionsFA = rowsFA;

            }

            IEnumerable OptionFMW = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 12).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMW = new SelectList(OptionFMW, "Value", "Text");
            ViewBag.OptionListFMW = NewtermsFMW;


             var medoutFMW = (from x in context.AttributePosts
                             join y in context.Attributes
                             on x.AttributeID equals y.AttributeID
                             join z in context.AttributeParents
                             on y.AttributeParentID equals z.AttributeParentID
                             where x.PostID == id && z.AttributeParentID == 12
                             select x.AttributeID).Count();

             if (medoutFMW > 0)
             {

                 var rowsFMW =
                (from x in context.AttributePosts
                 join y in context.Attributes
                 on x.AttributeID equals y.AttributeID
                 join z in context.AttributeParents
                 on y.AttributeParentID equals z.AttributeParentID
                 where x.PostID == id && z.AttributeParentID == 12
                 select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

                 ViewBag.OptionsFMW = rowsFMW;


             }

            IEnumerable OptionFMY = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 13).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMY = new SelectList(OptionFMY, "Value", "Text");
            ViewBag.OptionListFMY = NewtermsFMY;


             var medoutFMY = (from x in context.AttributePosts
                             join y in context.Attributes
                             on x.AttributeID equals y.AttributeID
                             join z in context.AttributeParents
                             on y.AttributeParentID equals z.AttributeParentID
                             where x.PostID == id && z.AttributeParentID == 13
                             select x.AttributeID).Count();

             if (medoutFMY > 0)
             {

                 var rowsFMY =
                (from x in context.AttributePosts
                 join y in context.Attributes
                 on x.AttributeID equals y.AttributeID
                 join z in context.AttributeParents
                 on y.AttributeParentID equals z.AttributeParentID
                 where x.PostID == id && z.AttributeParentID == 13
                 select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

                 ViewBag.OptionsFMY = rowsFMY;

             }

            IEnumerable OptionFMYC = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 14).OrderBy(x => x.AttributeName).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsFMYC = new SelectList(OptionFMYC, "Value", "Text");
            ViewBag.OptionListFMYC = NewtermsFMYC;


             var medoutFMYC = (from x in context.AttributePosts
                             join y in context.Attributes
                             on x.AttributeID equals y.AttributeID
                             join z in context.AttributeParents
                             on y.AttributeParentID equals z.AttributeParentID
                             where x.PostID == id && z.AttributeParentID == 14
                             select x.AttributeID).Count();

             if (medoutFMYC > 0)
             {


                 var rowsFMYC =
              (from x in context.AttributePosts
               join y in context.Attributes
               on x.AttributeID equals y.AttributeID
               join z in context.AttributeParents
               on y.AttributeParentID equals z.AttributeParentID
               where x.PostID == id && z.AttributeParentID == 14
               select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

                 ViewBag.OptionsFMYC = rowsFMYC;

             }

            // Meeting Times
            IEnumerable OptionMT = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 4).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsMT = new SelectList(OptionMT, "Value", "Text");
            ViewBag.OptionListMT = NewtermsMT;

            var rowsMT =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 4
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsMT = rowsMT;



            // Meeting Format
            IEnumerable OptionMF = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 6).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsMF = new SelectList(OptionMF, "Value", "Text");
            ViewBag.OptionListMF = NewtermsMF;

            var rowsMF =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 9
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsMF = rowsMF;


            // Expected Deliverables
            IEnumerable OptionED = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 7).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsED = new SelectList(OptionED, "Value", "Text");
            ViewBag.OptionListED = NewtermsED;

            var rowsED =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 7
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsMF = rowsED;

            // Compensation
            IEnumerable OptionCER = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 8).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsCER = new SelectList(OptionCER, "Value", "Text");
            ViewBag.OptionListCER = NewtermsCER;


            var rowsCER =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 8
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsCER = rowsCER;



            // Skills
            IEnumerable Option = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 1).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var Newterms = new SelectList(Option, "Value", "Text");
            ViewBag.OptionList = Newterms;

            var rows =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 1
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.Options = rows;

            // Knowledge
            IEnumerable OptionK = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 2).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsK = new SelectList(OptionK, "Value", "Text");
            ViewBag.OptionListK = NewtermsK;


            var rowsK =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 2
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsK = rowsK;


            // Abilities
            IEnumerable OptionA = context.Attributes.Where(c => c.Active == true && c.AttributeParentID == 3).AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.AttributeName,
                Value = c.AttributeID.ToString(),
                Selected = true,
            });


            var NewtermsA = new SelectList(OptionA, "Value", "Text");
            ViewBag.OptionListA = NewtermsA;

            var rowsA =
            (from x in context.AttributePosts
             join y in context.Attributes
             on x.AttributeID equals y.AttributeID
             join z in context.AttributeParents
             on y.AttributeParentID equals z.AttributeParentID
             where x.PostID == id && z.AttributeParentID == 3
             select x.AttributeID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);

            ViewBag.OptionsA = rowsA;



            // Counties
            IEnumerable OptionCounty = context.CountyBins.Where(c => c.Active == 1).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.CountyBinName,
                Value = c.CountyBinID.ToString(),
                Selected = true,
            });


            var NewtermsCounty = new SelectList(OptionCounty, "Value", "Text");
            ViewBag.OptionListCounty = NewtermsCounty;



            var post = (from x in context.Posts
                            join y in context.PostOrganizations
                            on x.PostID equals y.PostID
                            join z in context.Organizations
                            on y.OrganizationID equals z.OrganizationID
                            where x.PostID == id
                            select new PostViewModel()
                            {
                                PostID = x.PostID,
                                PostTitle = x.PostTitle,
                                PostContent = x.PostContent,
                                OrganizationName = z.OrganizationName,
                                OrgPic = z.OrgPic,
                                ExpirationDate = x.ExpirationDate,
                                Active = x.Active,
                                DateCreated = x.DateCreated,
                                CreatedBy = x.CreatedBy

                            }).SingleOrDefault();
            
     

            return PartialView(post);
        }

        //
        // POST: /Posts/Edit/5

        [HttpPost]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PossiblePostTypes = context.PostTypes;
            return View(post);
        }

        //
        // GET: /Posts/Delete/5
 
        public ActionResult Delete(int id)
        {
            Post post = context.Posts.Single(x => x.PostID == id);


            // Org ID
            var orgid = (from P in context.PostOrganizations
                         where P.PostID == id
                         select P.OrganizationID).SingleOrDefault();

            ViewBag.OrganizationID = orgid;


            return View(post);
        }

        //
        // POST: /Posts/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = context.Posts.Single(x => x.PostID == id);
            context.Posts.Remove(post);
            context.SaveChanges();

            // Org ID
            var orgid = (from P in context.PostOrganizations
                         where P.PostID == id
                         select P.OrganizationID).SingleOrDefault();


            var postorgs = (from cc in context.PostOrganizations
                            where cc.PostID == id
                            select cc).ToList();

            // Remove Posts
            foreach (PostOrganizations po in postorgs)
            {

                context.PostOrganizations.Remove(po);
                context.SaveChanges();
            }




            return RedirectToAction("Details", "Organizations", new { id = orgid });


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