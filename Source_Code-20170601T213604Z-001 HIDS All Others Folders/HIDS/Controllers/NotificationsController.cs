using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Web.Mvc;
using System.Collections;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{
    public class NotificationsController : Controller
    {

        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Notifications
        public ActionResult Index()
        {
            return View();
        }

        // GET: Notifications/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // GET: Notifications
        public ActionResult _NotificationBuilder(int? id)
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
                          where ast.UserName == U
                          select ast.Active).SingleOrDefault();

            ViewBag.Active = active;


            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

            var Oname = (from P in context.Organizations
                         where P.OrganizationID == O
                         select P.OrganizationName).SingleOrDefault();

            ViewBag.UID = U;
            ViewBag.OID = O;
            ViewBag.OName = Oname;

            ViewBag.PostID = id;

            //var posttitle = (from ast in context.Posts
            //                 where ast.PostID == id
            //                 select ast.PostTitle).SingleOrDefault();

            //ViewBag.PostTitle = posttitle;


            //var postbody = (from ast in context.Posts
            //                where ast.PostID == id
            //                select ast).SingleOrDefault();

            //ViewBag.PostBody = postbody.PostContent;


            //var rowsBCC = "";


            // Focus Areas
            //var attlist = (from P in context.AttributePosts
            //               join Y in context.Attributes
            //               on P.AttributeID equals Y.AttributeID
            //               where Y.AttributeParentID == 9 && P.PostID == id
            //               select Y.AttributeName).ToList();


            //string[] ResultList = attlist.ToArray();
            //string ResultString = string.Join(",", ResultList.ToArray());

            //var myList = new List<string>();
            //var attlistint = (from P in context.AttributePosts
            //               join Y in context.Attributes
            //               on P.AttributeID equals Y.AttributeID
            //               where Y.AttributeParentID == 9 && P.PostID == id
            //               select Y.AttributeID).ToList();


            //foreach (int rs in attlistint) {


            // // BCC
            //rowsBCC =
            //   (from x in context.AttributeUsers
            //    join y in context.AspNetUsers
            //    on x.Id equals y.Id
            //    join z in context.Attributes
            //    on x.AttributeID equals z.AttributeID
            //    where x.AttributeID == rs
            //    select y.Email).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


            //myList.Add(rowsBCC);

            //}

            
            //ViewBag.OptionsBCC = myList;


            //var attlistU = (from P in context.AttributePosts
            //              join Y in context.Attributes
            //              on P.AttributeID equals Y.AttributeID
            //              join z in context.AttributeUsers
            //              on Y.AttributeID equals z.AttributeID
            //              join u in context.AspNetUsers
            //              on z.Id equals u.Id
            //              where Y.AttributeParentID == 9 && P.PostID == id 
            //              select u.Email ).Distinct().ToList();


            //string[] ResultListU = attlistU.ToArray();
            //string ResultStringU = string.Join(",", ResultListU.ToArray());


            //ViewBag.OptionsBCC = ResultStringU;


            // Meeting Times
            //var mattlist = (from P in context.AttributePosts
            //               join Y in context.Attributes
            //               on P.AttributeID equals Y.AttributeID
            //               where Y.AttributeParentID == 4 && P.PostID == id
            //               select Y.AttributeName).ToList();


            //string[] MResultList = mattlist.ToArray();
            //string MResultString = string.Join(",", MResultList.ToArray());


            // Meeting Format
            //var mfattlist = (from P in context.AttributePosts
            //                join Y in context.Attributes
            //                on P.AttributeID equals Y.AttributeID
            //                where Y.AttributeParentID == 6 && P.PostID == id
            //                select Y.AttributeName).ToList();


            //string[] MFResultList = mfattlist.ToArray();
            //string MFResultString = string.Join(",", MFResultList.ToArray());

            
            // Expected Deliverables
            //var edattlist = (from P in context.AttributePosts
            //                 join Y in context.Attributes
            //                 on P.AttributeID equals Y.AttributeID
            //                 where Y.AttributeParentID == 7 && P.PostID == id
            //                 select Y.AttributeName).ToList();


            //string[] EDResultList = edattlist.ToArray();
            //string EDResultString = string.Join(",", EDResultList.ToArray());


            // Compensation
            //var cattlist = (from P in context.AttributePosts
            //                 join Y in context.Attributes
            //                 on P.AttributeID equals Y.AttributeID
            //                 where Y.AttributeParentID == 8 && P.PostID == id
            //                 select Y.AttributeName).ToList();


            //string[] CResultList = cattlist.ToArray();
            //string CResultString = string.Join(",", CResultList.ToArray());


            //// Skills
            //var sattlist = (from P in context.AttributePosts
            //                join Y in context.Attributes
            //                on P.AttributeID equals Y.AttributeID
            //                where Y.AttributeParentID == 1 && P.PostID == id
            //                select Y.AttributeName).ToList();


            //string[] SResultList = sattlist.ToArray();
            //string SResultString = string.Join(",", SResultList.ToArray());


            //// Knowledge
            //var kattlist = (from P in context.AttributePosts
            //                join Y in context.Attributes
            //                on P.AttributeID equals Y.AttributeID
            //                where Y.AttributeParentID == 2 && P.PostID == id
            //                select Y.AttributeName).ToList();


            //string[] KResultList = kattlist.ToArray();
            //string KResultString = string.Join(",", KResultList.ToArray());


            //// Abilities
            //var aattlist = (from P in context.AttributePosts
            //                join Y in context.Attributes
            //                on P.AttributeID equals Y.AttributeID
            //                where Y.AttributeParentID == 3 && P.PostID == id
            //                select Y.AttributeName).ToList();


            //string[] AResultList = aattlist.ToArray();
            //string AResultString = string.Join(",", AResultList.ToArray());


            //// Counties
            //var coattlist = (from P in context.CountyBinPosts
            //                join Y in context.CountyBins
            //                on P.CountyBinID equals Y.CountyBinID
            //                where Y.Active == 1 && P.PostID == id
            //                select Y.CountyBinName).ToList();


            //string[] COResultList = coattlist.ToArray();
            //string COResultString = string.Join(",", COResultList.ToArray());


            //var faclist = (from c in context.Posts
            //               where c.PostID == id
            //               select new NotificationViewModel()
            //               {

            //                   FocusAreas = ResultString,
            //                   MeetingTimes = MResultString,
            //                   MeetingFormat = MFResultString,
            //                   ExpectedDeliveries = EDResultString,
            //                   Compensation = CResultString,
            //                   PostID = c.PostID,
            //                   PostBody = c.PostContent,
            //                   Skills = SResultString,
            //                   Knowledge = KResultString,
            //                   Abilities = AResultString,
            //                   Counties = COResultString,
            //                   ToField = "",
            //                   CarbonCopy = "",
            //                   BlindCarbonCopy = ""

            //               }).Distinct().SingleOrDefault();


           

            // Options
            //IEnumerable OptionTO = context.AspNetUsers.Where(c => c.Active == true).AsEnumerable().Select(c => new SelectListItem()

            //{

            //    Text = c.FirstName + " " + c.LastName,
            //    Value = c.UserName.ToString(),
            //    Selected = true,
            //});


            //var NewtermsTO = new SelectList(OptionTO, "Value", "Text");
            //ViewBag.OptionListTO = NewtermsTO;

          
            return PartialView();


        }


        // GET: Notifications/Create
        public ActionResult _MessageConfig()
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


            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

            var Oname = (from P in context.Organizations
                         where P.OrganizationID == O
                         select P.OrganizationName).SingleOrDefault();

            ViewBag.UID = U;
            ViewBag.OID = O;
            ViewBag.OName = Oname;

            //ViewBag.PostID = id;

            //var posttitle = (from ast in context.Posts
            //                 where ast.PostID == id
            //                 select ast.PostTitle).SingleOrDefault();

            //ViewBag.PostTitle = posttitle;

            // Options
            IEnumerable OptionTO = context.AspNetUsers.AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.FirstName + " " + c.LastName,
                Value = c.UserName.ToString(),
                Selected = true,
            });


            var NewtermsTO = new SelectList(OptionTO, "Value", "Text");
            ViewBag.OptionListTO = NewtermsTO;



            return PartialView();
        }


        // GET: Notifications/Create
        public ActionResult NewMessage()
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


            var O = (from P in context.AspNetUsers
                     where P.UserName == UserName
                     select P.OrganizationID).SingleOrDefault();

            var Oname = (from P in context.Organizations
                         where P.OrganizationID == O
                         select P.OrganizationName).SingleOrDefault();

            ViewBag.UID = U;
            ViewBag.OID = O;
            ViewBag.OName = Oname;

            //ViewBag.PostID = id;

            //var posttitle = (from ast in context.Posts
            //                 where ast.PostID == id
            //                 select ast.PostTitle).SingleOrDefault();

            //ViewBag.PostTitle = posttitle;

            // Options
            IEnumerable OptionTO = context.AspNetUsers.AsEnumerable().Select(c => new SelectListItem()

            {

                Text = c.FirstName + " " + c.LastName,
                Value = c.UserName.ToString(),
                Selected = true,
            });


            var NewtermsTO = new SelectList(OptionTO, "Value", "Text");
            ViewBag.OptionListTO = NewtermsTO;



            return View();
        }



        // GET: Notifications/Create
        public ActionResult Create(int? id)
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


            ViewBag.PostID = id;

            return View();


        }


         public JsonResult _SendNotificationF(NotificationViewModel notificationviewmodel)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;
                
                 Notification notification = new Notification
                {

                    PostSubject = notificationviewmodel.PostSubject,
                    To = notificationviewmodel.ToField,
                    CarbonCopy = notificationviewmodel.CarbonCopy,
                    BlindCarbonCopy = notificationviewmodel.BlindCarbonCopy,
                    PostBody = notificationviewmodel.PostBody,
                    CreatedBy = UserName,
                    DateCreated = CreatedInit,
                    UpdatedBy = UserName,
                    DateUpdated = CreatedInit

                };

                context.Notifications.Add(notification);
                context.SaveChanges();

                int NID = notification.NotificationID;

                if (notificationviewmodel.ToField != null)
                {

                    // To
                    var stringToSplit = notificationviewmodel.ToField;

                    var query = from val in stringToSplit.Split(',')
                                select val;
                    foreach (string val in query)
                    {

                        
                        SendEmail(notification.PostSubject, notification.PostBody, val);


                    }


                }

                if (notificationviewmodel.CarbonCopy != null)
                {

                    // CC
                    var stringToSplit = notificationviewmodel.CarbonCopy;

                    var query = from val in stringToSplit.Split(',')
                                select val;
                    foreach (string val in query)
                    {


                        SendEmail(notification.PostSubject, notification.PostBody, val);


                    }


                }


                if (notificationviewmodel.BlindCarbonCopy != null)
                {

                    // BCC
                    var stringToSplit = notificationviewmodel.BlindCarbonCopy;

                    var query = from val in stringToSplit.Split(',')
                                select val;
                    foreach (string val in query)
                    {


                        SendEmail(notification.PostSubject, notification.PostBody, val);


                    }


                }



                return Json(new { Status = "Success", Modified = NID }, JsonRequestBehavior.AllowGet);



            }


            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

        }


         public JsonResult _EmailNotificationF(NotificationViewModel notificationviewmodel)
         {


             // Set Record Info
             string UserName = @User.Identity.Name.ToString();
             DateTime CreatedInit = DateTime.Now;


             var U = (from P in context.AspNetUsers
                      where P.UserName == UserName
                      select P.Id).SingleOrDefault();

             var Uu = (from P in context.AspNetUsers
                       where P.Id == U
                       select P.RoleBinID).SingleOrDefault();

             ViewBag.Role = Uu;

             var active = (from ast in context.AspNetUsers
                           where ast.UserName == U
                           select ast.Active).SingleOrDefault();

             ViewBag.Active = active;


             var O = (from P in context.AspNetUsers
                      where P.UserName == UserName
                      select P.OrganizationID).SingleOrDefault();

             var Oname = (from P in context.Organizations
                          where P.OrganizationID == O
                          select P.OrganizationName).SingleOrDefault();

             ViewBag.UID = U;
             ViewBag.OID = O;
             ViewBag.OName = Oname;

             ViewBag.PostID = notificationviewmodel.PostID;

             //var posttitle = (from ast in context.Posts
             //                 where ast.PostID == notificationviewmodel.PostID
             //                 select ast.PostTitle).SingleOrDefault();

             //ViewBag.PostTitle = posttitle;


             //var postbody = (from ast in context.Posts
             //                where ast.PostID == id
             //                select ast).SingleOrDefault();

             //ViewBag.PostBody = postbody.PostContent;


             //var rowsBCC = "";


             // Focus Areas
             //var attlist = (from P in context.AttributePosts
             //               join Y in context.Attributes
             //               on P.AttributeID equals Y.AttributeID
             //               where Y.AttributeParentID == 9 && P.PostID == notificationviewmodel.PostID
             //               select Y.AttributeName).ToList();


             //string[] ResultList = attlist.ToArray();
             //string ResultString = string.Join(",", ResultList.ToArray());

             //var myList = new List<string>();
             //var attlistint = (from P in context.AttributePosts
             //               join Y in context.Attributes
             //               on P.AttributeID equals Y.AttributeID
             //               where Y.AttributeParentID == 9 && P.PostID == id
             //               select Y.AttributeID).ToList();


             //foreach (int rs in attlistint) {


             // // BCC
             //rowsBCC =
             //   (from x in context.AttributeUsers
             //    join y in context.AspNetUsers
             //    on x.Id equals y.Id
             //    join z in context.Attributes
             //    on x.AttributeID equals z.AttributeID
             //    where x.AttributeID == rs
             //    select y.Email).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


             //myList.Add(rowsBCC);

             //}


             //ViewBag.OptionsBCC = myList;


             //var attlistU = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                join z in context.AttributeUsers
             //                on Y.AttributeID equals z.AttributeID
             //                join u in context.AspNetUsers
             //                on z.Id equals u.Id
             //                where Y.AttributeParentID == 9 && P.PostID == notificationviewmodel.PostID
             //                select u.Email).Distinct().ToList();


             //string[] ResultListU = attlistU.ToArray();
             //string ResultStringU = string.Join(",", ResultListU.ToArray());


             //ViewBag.OptionsBCC = ResultStringU;


             //// Meeting Times
             //var mattlist = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                where Y.AttributeParentID == 4 && P.PostID == notificationviewmodel.PostID
             //                select Y.AttributeName).ToList();


             //string[] MResultList = mattlist.ToArray();
             //string MResultString = string.Join(",", MResultList.ToArray());


             //// Meeting Format
             //var mfattlist = (from P in context.AttributePosts
             //                 join Y in context.Attributes
             //                 on P.AttributeID equals Y.AttributeID
             //                 where Y.AttributeParentID == 6 && P.PostID == notificationviewmodel.PostID
             //                 select Y.AttributeName).ToList();


             //string[] MFResultList = mfattlist.ToArray();
             //string MFResultString = string.Join(",", MFResultList.ToArray());


             //// Expected Deliverables
             //var edattlist = (from P in context.AttributePosts
             //                 join Y in context.Attributes
             //                 on P.AttributeID equals Y.AttributeID
             //                 where Y.AttributeParentID == 7 && P.PostID == notificationviewmodel.PostID
             //                 select Y.AttributeName).ToList();


             //string[] EDResultList = edattlist.ToArray();
             //string EDResultString = string.Join(",", EDResultList.ToArray());


             //// Compensation
             //var cattlist = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                where Y.AttributeParentID == 8 && P.PostID == notificationviewmodel.PostID
             //                select Y.AttributeName).ToList();


             //string[] CResultList = cattlist.ToArray();
             //string CResultString = string.Join(",", CResultList.ToArray());


             //// Skills
             //var sattlist = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                where Y.AttributeParentID == 1 && P.PostID == notificationviewmodel.PostID
             //                select Y.AttributeName).ToList();


             //string[] SResultList = sattlist.ToArray();
             //string SResultString = string.Join(",", SResultList.ToArray());


             //// Knowledge
             //var kattlist = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                where Y.AttributeParentID == 2 && P.PostID == notificationviewmodel.PostID
             //                select Y.AttributeName).ToList();


             //string[] KResultList = kattlist.ToArray();
             //string KResultString = string.Join(",", KResultList.ToArray());


             //// Abilities
             //var aattlist = (from P in context.AttributePosts
             //                join Y in context.Attributes
             //                on P.AttributeID equals Y.AttributeID
             //                where Y.AttributeParentID == 3 && P.PostID == notificationviewmodel.PostID
             //                select Y.AttributeName).ToList();


             //string[] AResultList = aattlist.ToArray();
             //string AResultString = string.Join(",", AResultList.ToArray());


             //// Counties
             //var coattlist = (from P in context.CountyBinPosts
             //                 join Y in context.CountyBins
             //                 on P.CountyBinID equals Y.CountyBinID
             //                 where Y.Active == 1 && P.PostID == notificationviewmodel.PostID
             //                 select Y.CountyBinName).ToList();


             //string[] COResultList = coattlist.ToArray();
             //string COResultString = string.Join(",", COResultList.ToArray());


             //var faclist = (from c in context.Posts
             //               where c.PostID == notificationviewmodel.PostID
             //               select new NotificationViewModel()
             //               {

             //                   FocusAreas = ResultString,
             //                   MeetingTimes = MResultString,
             //                   MeetingFormat = MFResultString,
             //                   ExpectedDeliveries = EDResultString,
             //                   Compensation = CResultString,
             //                   PostBody = c.PostContent,
             //                   Skills = SResultString,
             //                   Knowledge = KResultString,
             //                   Abilities = AResultString,
             //                   Counties = COResultString

             //               }).Distinct().SingleOrDefault();




             // Options
             IEnumerable OptionTO = context.AspNetUsers.Where(c => c.Active == true).AsEnumerable().Select(c => new SelectListItem()

             {

                 Text = c.FirstName + " " + c.LastName,
                 Value = c.UserName.ToString(),
                 Selected = true,
             });


             var NewtermsTO = new SelectList(OptionTO, "Value", "Text");
             ViewBag.OptionListTO = NewtermsTO;


             if (ModelState.IsValid)
             {

                

                 Notification notification = new Notification
                 {

                     PostSubject = notificationviewmodel.PostSubject,
                     To = notificationviewmodel.ToField,
                     CarbonCopy = notificationviewmodel.CarbonCopy,
                     BlindCarbonCopy = notificationviewmodel.BlindCarbonCopy,
                     PostBody = notificationviewmodel.PostBody,
                     CreatedBy = UserName,
                     DateCreated = CreatedInit,
                     UpdatedBy = UserName,
                     DateUpdated = CreatedInit

                 };

                 context.Notifications.Add(notification);
                 context.SaveChanges();

                 int NID = notification.NotificationID;

                 if (notificationviewmodel.ToField != null)
                 {

                     // To
                     var stringToSplit = notificationviewmodel.ToField;

                     var query = from val in stringToSplit.Split(',')
                                 select val;
                     foreach (string val in query)
                     {


                       //  SendPostEmail(posttitle, faclist.PostBody, val, faclist.FocusAreas, faclist.MeetingTimes,faclist.MeetingFormat, faclist.ExpectedDeliveries,faclist.Compensation, faclist.Skills,faclist.Knowledge,faclist.Abilities,faclist.Counties);


                     }


                 }

                 if (notificationviewmodel.CarbonCopy != null)
                 {

                     // CC
                     var stringToSplit = notificationviewmodel.CarbonCopy;

                     var query = from val in stringToSplit.Split(',')
                                 select val;
                     foreach (string val in query)
                     {


                        // SendPostEmail(posttitle, faclist.PostBody, val, faclist.FocusAreas, faclist.MeetingTimes, faclist.MeetingFormat, faclist.ExpectedDeliveries, faclist.Compensation, faclist.Skills, faclist.Knowledge, faclist.Abilities, faclist.Counties);


                     }


                 }


                 if (notificationviewmodel.BlindCarbonCopy != null)
                 {

                     // BCC
                     var stringToSplit = notificationviewmodel.BlindCarbonCopy;

                     var query = from val in stringToSplit.Split(',')
                                 select val;
                     foreach (string val in query)
                     {


                       //  SendPostEmail(posttitle, faclist.PostBody, val, faclist.FocusAreas, faclist.MeetingTimes, faclist.MeetingFormat, faclist.ExpectedDeliveries, faclist.Compensation, faclist.Skills, faclist.Knowledge, faclist.Abilities, faclist.Counties);


                     }


                 }



                 // OrganizationID
                 //var orgid = (from P in context.PostOrganizations
                 //                 join Y in context.Organizations
                 //                 on P.OrganizationID equals Y.OrganizationID
                 //                 where P.PostID == notificationviewmodel.PostID
                 //                 select Y.OrganizationID).ToList();

                 //, Modified2 = orgid

                 return Json(new { Status = "Success", Modified = NID }, JsonRequestBehavior.AllowGet);



             }


             return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);

         }




         public void SendEmail(string PostSubject, string PostBody, string Email)
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
             mail.From = new MailAddress("eileen.forlenza@state.co.us");
             mail.To.Add(Email);
             //mail.CC.Add(eEmail);
             mail.Priority = MailPriority.Normal;
             mail.Subject = PostSubject;

             //// SET THE BODY //
             mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + PostBody + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<br>Thank you!<br>Colorado Family Leadership Registry Support";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "</TABLE>";
             mail.IsBodyHtml = true;

             //UNCOMMENT THIS LINE TO EMAIL
             SmtpClient smtp = new SmtpClient();
             smtp.Send(mail);
         }


         public void SendPostEmail(string PostSubject, string PostBody, string Email, string FocusAreas, string MeetingTimes, string MeetingFormat, string ExpectedDeliveries,string Compensation, string Skills, string Knowledge, string Abilities, string Counties)
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
             mail.From = new MailAddress("eileen.forlenza@state.co.us");
             mail.To.Add(Email);
             //mail.CC.Add(eEmail);
             mail.Priority = MailPriority.Normal;
             mail.Subject = "Colorado Family Leadership Registry Opportunity: " + PostSubject;

             //// SET THE BODY //
             mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + PostSubject + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Goal and Purpose" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + PostBody + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Focus Areas" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + FocusAreas + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Meeting Times" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + MeetingTimes + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Meeting Format" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + MeetingFormat + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Expected Deliverables" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + ExpectedDeliveries + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Compensation" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + Compensation + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Skills" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + Skills + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Knowledge" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + Knowledge + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Abilities" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + Abilities + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + "Counties" + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<p> " + Counties + "</p>";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "<TR>";
             mail.Body = mail.Body + "<TD>";
             mail.Body = mail.Body + "<br>Thank you!<br>Colorado Family Leadership Registry Support";
             mail.Body = mail.Body + "</TD>";
             mail.Body = mail.Body + "</TR>";
             mail.Body = mail.Body + "</TABLE>";
             mail.IsBodyHtml = true;

             //UNCOMMENT THIS LINE TO EMAIL
             SmtpClient smtp = new SmtpClient();
             smtp.Send(mail);
         }




        // POST: Notifications/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Notifications/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Notifications/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Notifications/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Notifications/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
