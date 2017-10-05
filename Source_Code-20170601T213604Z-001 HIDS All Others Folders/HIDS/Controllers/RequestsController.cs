using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;
using System.Collections;

namespace CTL.Controllers
{
    public class RequestsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private PortalRequestEntitiesEntities db = new PortalRequestEntitiesEntities();
        private eCaSTContextEntities edb = new eCaSTContextEntities();
        private WHEntities wdb = new WHEntities();
        // GET: Requests
        public ActionResult Index(string id)
        {
            string UserName = @User.Identity.Name.ToString();
            var userinfo = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.RoleBinID = userinfo.RoleBinID;

            //ViewBag.UserName = UserName;
            //var guid = new Guid(userid.Id);
            //// Pending Count
            //var requestcount = (from x in db.Requests
            //                    where x.StateID == 1 && x.UserID == guid
            //                    select x).Count();
            //ViewBag.RequestCount = requestcount;

            //// Open Count
            //var openrequestcount = (from x in db.Requests
            //                        where x.StateID == 7 || x.StateID == 13 || x.StateID == 14 || x.StateID == 15 && x.UserID == userid
            //                        select x).Count();
            //ViewBag.OpenRequestCount = openrequestcount;

            //// My Requests Count
            //var myrequestcount = (from x in db.Requests
            //                      where x.RequestorID == UserName && x.StateID != 9 && x.StateID != 11 && x.StateID != 17
            //                      select x).Count();
            //ViewBag.MyRequestCount = myrequestcount;

            if (userinfo.RoleBinID == 1 || userinfo.RoleBinID == 13)
            {
            // Program List
            var programcount = (from x in context.AspNetUsers 
                            join y in context.RoleProgramUserProfiles
                            on x.Id equals y.Id
                            where x.Id == userinfo.Id
                            select x).Count();

            if (programcount > 0)
            {


                List<SelectListItem> items = new List<SelectListItem>();
              
                // App List
                var programlist = (from x in context.AspNetUsers
                                    join y in context.RoleProgramUserProfiles
                                    on x.Id equals y.Id
                                    join t in context.Programs
                                    on y.ProgramID equals t.ProgramID
                                    where x.Id == userinfo.Id
                                    select t).ToList();

                foreach (Program p in programlist)
                {

                    // Program Count
                    var program = (from x in context.Programs
                                        where x.ProgramID == p.ProgramID
                                        select x).SingleOrDefault();

                    if (program != null)
                    {

                     
                        
                        items.Add(new SelectListItem { Text = program.ProgramName, Value = program.ProgramID.ToString(), Selected = false });
                       



                    }

                }


                ViewBag.FirstProgramID = items.Last().Value;
                ViewBag.ProgramCount = items.Count();
              //  ViewBag.ProgramID = items.ToList();
                ViewBag.ProgramID = new SelectList(items, "Value", "Text");
               





            }

            


            }

           // .Join(edb.Screenings, rs => rs.ScreeningID, s => s.ScreeningID, (rs, s) => new { rs, s })
            var reqlist = (from B in db.Requests select B).ToList();
            var reqclientlist = (from B in db.RequestClients select B).Distinct().ToList();
            var clientlist = new List<ClientViewModel>();
            var clients = new List<int>();

            foreach (RequestClient rc in reqclientlist)
            {

                var clientcheck = (from x in edb.Clients
                                   where x.ClientID == rc.ClientID
                                   select x).Count();

                if (clientcheck > 0)
                {


                    clients.Add(rc.ClientID);

                }


            }


            foreach (int c in clients)
            {


                ClientViewModel modelC = new ClientViewModel();
                modelC.ClientID = c;

                clientlist.Add(modelC);


            }
            
            
            
            
            var reqscreeninglist = (from B in db.RequestScreenings select B).Distinct().ToList();
            // Build List of Valid Screenings
            var screeninglist = new List<ScreeningViewModel>();
            var screens = new List<int>();

            foreach (RequestScreening rs in reqscreeninglist)
            {

                var screencheck = (from x in edb.Screenings
                                    where x.ScreeningID == rs.ScreeningID
                                    select x).Count();

                if (screencheck > 0)
                {


                    screens.Add(rs.ScreeningID);

                }


            }


            foreach (int s in screens)
            {


                ScreeningViewModel model = new ScreeningViewModel();
                model.ScreeningID = s;

                screeninglist.Add(model);


            }



            var useridreqlist = (from B in db.UserIdRequests select B).Distinct().ToList();
            var reqtypelist = (from B in db.RequestTypes select B).ToList();
            var reqactionlist = (from B in db.RequestActions select B).ToList();
           // var reqtrackerlist = (from B in db.RequestTrackers select B).ToList();


            var prioritylist = (from B in db.Priorities select B).ToList();
            var statelist = (from B in db.States select B).ToList();
            var ganttlist = (from B in db.GanttTasks select B).ToList();
            var userlist = (from B in context.AspNetUsers select B).Distinct().ToList();
            var siteroleprogramlist = (from B in context.SiteRoleProgramUserProfiles select B).Distinct().ToList();
            var sitelist = (from B in context.Sites select B).Distinct().ToList();
            var agencysiteprogramlist = (from B in context.AgencySiteProgramSites select B).Distinct().ToList();
            var agencysitelist = (from B in context.AgencySites select B).Distinct().ToList();

            if (userinfo.RoleBinID == 1 || userinfo.RoleBinID == 13)
            {


                var reqfinallist = new List<RequestViewModel>();
                
                foreach (ScreeningViewModel ssc in screeninglist)
                {


                      var screencheck = (from x in edb.Screenings
                                    where x.ScreeningID == ssc.ScreeningID
                                    select x).Count();

                      if (screencheck > 0)
                      {

                          var request = (from x in reqlist
                                         //join ddf in db.RequestClients
                                         //on x.RequestID equals ddf.RequestID
                                         //into dddf
                                         //from ddddf in dddf.DefaultIfEmpty()
                                         join dde in db.RequestScreenings
                                         on x.RequestID equals dde.RequestID
                                         //into ddde
                                         //from dddde in ddde.DefaultIfEmpty()
                                         //join rr in reqactionlist
                                         //on x.RequestActionID equals rr.RequestActionID
                                         join mm in statelist
                                         on x.StateID equals mm.StateID
                                         //join ddd in ganttlist
                                         //on x.ProjectID equals ddd.ID
                                         join dddd in userlist
                                         on x.RequestorID equals dddd.UserName
                                         //join srp in context.SiteRoleProgramUserProfiles
                                         //on dddd.Id equals srp.Id
                                         //join s in context.Sites
                                         //on srp.SiteID equals s.SiteID
                                         //join asp in context.AgencySiteProgramSites
                                         //on srp.SiteID equals asp.SiteID
                                         //join ass in context.AgencySites
                                         //on asp.AgencySiteID equals ass.AgencySiteID
                                         where x.ProjectID == 69 && x.ApplicationID == 6 && dde.ScreeningID == ssc.ScreeningID

                                         select new RequestViewModel()
                                         {
                                             RequestID = x.RequestID,
                                             RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                             DueDate = x.DueDate,
                                             UserID = x.UserID,
                                             RequestTrackerTypeName = "Screening",
                                             RequestTrackerComment = (from ba in db.RequestTrackers
                                                          where x.RequestID == ba.RequestID
                                                          select ba.RequestDesc).First(),
                                            // DaysOpen = DbFunctions.DiffDays(Convert.ToDateTime(x.RequestDate), DateTime.Now).Value,
                                             // ScreeningID = dddde.ScreeningID,
                                             //ClientID = ddddf.ClientID,
                                             // RequestTypeName = aa.RequestTypeName,
                                             //  RequestActionName = rr.RequestActionName,
                                             //AssetCount = (from ba in db.Assets
                                             //              where ba.RequestID == x.RequestID
                                             //              select ba.AssetID).Count(),
                                             RequestTitle = x.RequestTitle,
                                             // ProjectName = ddd.Title,
                                             StateName = mm.StateName,
                                             RequestorID = x.RequestorID,
                                             RequestDesc = x.RequestDesc,
                                             UserName = dddd.FirstName + " " + dddd.LastName,
                                             // AgencyName = ass.AgencySiteName



                                         }).Distinct().ToList();
                          foreach (RequestViewModel r in request)
                          {

                               bool alreadyExists = reqfinallist.Any(x => x.RequestID == r.RequestID );

                               if (alreadyExists == false)
                               {

                                   reqfinallist.Add(r);



                               }

                          }

                                           

                      }


                }


                foreach (ClientViewModel ccc in clientlist)
                {


                    var clientcheck = (from x in edb.Clients
                                       where x.ClientID == ccc.ClientID
                                       select x).Count();

                    if (clientcheck > 0)
                    {

                        var requestC = (from x in reqlist
                                       join ddf in db.RequestClients
                                       on x.RequestID equals ddf.RequestID
                                       //into dddf
                                       //from ddddf in dddf.DefaultIfEmpty()
                                      // join dde in db.RequestScreenings
                                     //  on x.RequestID equals dde.RequestID
                                       //into ddde
                                       //from dddde in ddde.DefaultIfEmpty()
                                       //join rr in reqactionlist
                                       //on x.RequestActionID equals rr.RequestActionID
                                       join mm in statelist
                                       on x.StateID equals mm.StateID
                                       //join ddd in ganttlist
                                       //on x.ProjectID equals ddd.ID
                                       join dddd in userlist
                                       on x.RequestorID equals dddd.UserName
                                       //join srp in context.SiteRoleProgramUserProfiles
                                       //on dddd.Id equals srp.Id
                                       //join s in context.Sites
                                       //on srp.SiteID equals s.SiteID
                                       //join asp in context.AgencySiteProgramSites
                                       //on srp.SiteID equals asp.SiteID
                                       //join ass in context.AgencySites
                                       //on asp.AgencySiteID equals ass.AgencySiteID
                                       where x.ProjectID == 69 && x.ApplicationID == 6 && ddf.ClientID == ccc.ClientID

                                       select new RequestViewModel()
                                       {
                                           RequestID = x.RequestID,
                                           RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                           DueDate = x.DueDate,
                                           UserID = x.UserID,
                                           RequestTrackerTypeName = "Client",
                                           RequestTrackerComment = (from ba in db.RequestTrackers
                                                                    where x.RequestID == ba.RequestID
                                                                    select ba.RequestDesc).First(),
                                           RequestTitle = x.RequestTitle,
                                           // ProjectName = ddd.Title,
                                           StateName = mm.StateName,
                                           RequestorID = x.RequestorID,
                                           RequestDesc = x.RequestDesc,
                                           UserName = dddd.FirstName + " " + dddd.LastName,
                                           // AgencyName = ass.AgencySiteName



                                       }).Distinct().ToList();
                        foreach (RequestViewModel rc in requestC)
                        {


                             bool alreadyExists = reqfinallist.Any(x => x.RequestID == rc.RequestID );

                             if (alreadyExists == false)
                             {


                                 reqfinallist.Add(rc);



                             }

                        }



                    }


                }



               

                return PartialView(reqfinallist);



            }
            //else if (userinfo.RoleBinID == 13)
            //{


            //    Guid uid = new Guid(userinfo.Id);

            //    var requests = (from x in reqlist
            //                    join ddf in db.RequestClients
            //                    on x.RequestID equals ddf.RequestID
            //                    into dddf
            //                    from ddddf in dddf.DefaultIfEmpty()
            //                    join dde in db.RequestScreenings
            //                    on x.RequestID equals dde.RequestID
            //                    into ddde
            //                    from dddde in ddde.DefaultIfEmpty()
            //                    //join aa in reqtypelist
            //                    //on x.RequestTypeID equals aa.RequestTypeID
            //                    //join rr in reqactionlist
            //                    //on x.RequestActionID equals rr.RequestActionID
            //                    join mm in statelist
            //                    on x.StateID equals mm.StateID
            //                    //join ddd in ganttlist
            //                    //on x.ProjectID equals ddd.ID
            //                    join dddd in userlist
            //                    on x.RequestorID equals dddd.UserName
            //                    //join srp in context.SiteRoleProgramUserProfiles
            //                    //on dddd.Id equals srp.Id
            //                    //join s in context.Sites
            //                    //on srp.SiteID equals s.SiteID
            //                    //join asp in context.AgencySiteProgramSites
            //                    //on srp.SiteID equals asp.SiteID
            //                    //join ass in context.AgencySites
            //                    //on asp.AgencySiteID equals ass.AgencySiteID
            //                    where x.UserID == uid

            //                    select new RequestViewModel()
            //                    {
            //                        RequestID = x.RequestID,
            //                        RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
            //                        DueDate = x.DueDate,
            //                        UserID = x.UserID,
            //                        // RequestTypeName = aa.RequestTypeName,
            //                        //  RequestActionName = rr.RequestActionName,
            //                        AssetCount = (from ba in db.Assets
            //                                      where ba.RequestID == x.RequestID
            //                                      select ba.AssetID).Count(),
            //                        RequestTitle = x.RequestTitle,
            //                        // ProjectName = ddd.Title,
            //                        StateName = mm.StateName,
            //                        RequestorID = x.RequestorID,
            //                        RequestDesc = x.RequestDesc,
            //                        UserName = dddd.FirstName + " " + dddd.LastName,
            //                       // AgencyName = ass.AgencySiteName
                                   
                                    

            //                    }).Distinct().ToList();





            //    return PartialView(requests);


            //}
            else
            {


                var reqfinallist = new List<RequestViewModel>();

                foreach (ScreeningViewModel ssc in screeninglist)
                {


                    var screencheck = (from x in edb.Screenings
                                       where x.ScreeningID == ssc.ScreeningID
                                       select x).Count();

                    if (screencheck > 0)
                    {

                        var request = (from x in reqlist
                                       //join ddf in db.RequestClients
                                       //on x.RequestID equals ddf.RequestID
                                       //into dddf
                                       //from ddddf in dddf.DefaultIfEmpty()
                                       join dde in db.RequestScreenings
                                       on x.RequestID equals dde.RequestID
                                       //into ddde
                                       //from dddde in ddde.DefaultIfEmpty()
                                       //join rr in reqactionlist
                                       //on x.RequestActionID equals rr.RequestActionID
                                       join mm in statelist
                                       on x.StateID equals mm.StateID
                                       //join ddd in ganttlist
                                       //on x.ProjectID equals ddd.ID
                                       join dddd in userlist
                                       on x.UserID equals dddd.Id
                                       //join srp in context.SiteRoleProgramUserProfiles
                                       //on dddd.Id equals srp.Id
                                       //join s in context.Sites
                                       //on srp.SiteID equals s.SiteID
                                       //join asp in context.AgencySiteProgramSites
                                       //on srp.SiteID equals asp.SiteID
                                       //join ass in context.AgencySites
                                       //on asp.AgencySiteID equals ass.AgencySiteID
                                       where x.RequestorID == UserName

                                       select new RequestViewModel()
                                       {
                                           RequestID = x.RequestID,
                                           RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                           DueDate = x.DueDate,
                                           UserID = x.UserID,
                                           RequestTrackerTypeName = "Screening",
                                           // DaysOpen = DbFunctions.DiffDays(Convert.ToDateTime(x.RequestDate), DateTime.Now).Value,
                                           // ScreeningID = dddde.ScreeningID,
                                           //ClientID = ddddf.ClientID,
                                           // RequestTypeName = aa.RequestTypeName,
                                           //  RequestActionName = rr.RequestActionName,
                                           //AssetCount = (from ba in db.Assets
                                           //              where ba.RequestID == x.RequestID
                                           //              select ba.AssetID).Count(),
                                           RequestTitle = x.RequestTitle,
                                           // ProjectName = ddd.Title,
                                           StateName = mm.StateName,
                                           RequestorID = x.RequestorID,
                                           RequestDesc = x.RequestDesc,
                                           UserName = dddd.FirstName + " " + dddd.LastName,
                                           // AgencyName = ass.AgencySiteName



                                       }).Distinct().ToList();
                        foreach (RequestViewModel r in request)
                        {

                            bool alreadyExists = reqfinallist.Any(x => x.RequestID == r.RequestID);

                             if (alreadyExists == false)
                             {



                                 reqfinallist.Add(r);



                             }

                        }



                    }


                }


                foreach (ClientViewModel ccc in clientlist)
                {


                    var clientcheck = (from x in edb.Clients
                                       where x.ClientID == ccc.ClientID
                                       select x).Count();

                    if (clientcheck > 0)
                    {

                        var requestC = (from x in reqlist
                                        join ddf in db.RequestClients
                                        on x.RequestID equals ddf.RequestID
                                        //into dddf
                                        //from ddddf in dddf.DefaultIfEmpty()
                                        // join dde in db.RequestScreenings
                                        //  on x.RequestID equals dde.RequestID
                                        //into ddde
                                        //from dddde in ddde.DefaultIfEmpty()
                                        //join rr in reqactionlist
                                        //on x.RequestActionID equals rr.RequestActionID
                                        join mm in statelist
                                        on x.StateID equals mm.StateID
                                        //join ddd in ganttlist
                                        //on x.ProjectID equals ddd.ID
                                        join dddd in userlist
                                        on x.UserID equals dddd.Id
                                        //join srp in context.SiteRoleProgramUserProfiles
                                        //on dddd.Id equals srp.Id
                                        //join s in context.Sites
                                        //on srp.SiteID equals s.SiteID
                                        //join asp in context.AgencySiteProgramSites
                                        //on srp.SiteID equals asp.SiteID
                                        //join ass in context.AgencySites
                                        //on asp.AgencySiteID equals ass.AgencySiteID
                                        where x.RequestorID == UserName

                                        select new RequestViewModel()
                                        {
                                            RequestID = x.RequestID,
                                            RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                            DueDate = x.DueDate,
                                            UserID = x.UserID,
                                            RequestTrackerTypeName = "Client",
                                            // ScreeningID = dddde.ScreeningID,
                                            //ClientID = ddddf.ClientID,
                                            // RequestTypeName = aa.RequestTypeName,
                                            //  RequestActionName = rr.RequestActionName,
                                            //AssetCount = (from ba in db.Assets
                                            //              where ba.RequestID == x.RequestID
                                            //              select ba.AssetID).Count(),
                                            RequestTitle = x.RequestTitle,
                                            // ProjectName = ddd.Title,
                                            StateName = mm.StateName,
                                            RequestorID = x.RequestorID,
                                            RequestDesc = x.RequestDesc,
                                            UserName = dddd.FirstName + " " + dddd.LastName,
                                            // AgencyName = ass.AgencySiteName



                                        }).Distinct().ToList();
                        foreach (RequestViewModel rc in requestC)
                        {

                            bool alreadyExists = reqfinallist.Any(x => x.RequestID == rc.RequestID);

                            if (alreadyExists == false)
                            {

                                reqfinallist.Add(rc);


                            }


                        }



                    }


                }




                return PartialView(reqfinallist);



            }

            return PartialView();

        }


        public ActionResult _RequestList(int? id)
        {
            string UserName = @User.Identity.Name.ToString();
            var userinfo = (from x in context.AspNetUsers
                            where x.UserName == UserName
                            select x).FirstOrDefault();

            ViewBag.RoleBinID = userinfo.RoleBinID;
            var reqlist = (from B in db.Requests select B).ToList();
            var reqclientlist = (from B in db.RequestClients select B).Distinct().ToList();
            var reqpersonlist = (from B in db.RequestPersons select B).Distinct().ToList();
            var clientlist = new List<ClientViewModel>();
            var clients = new List<int>();
            var personlist = new List<ClientViewModel>();
            var persons = new List<int>();


            // WWC
            foreach (RequestClient rc in reqclientlist)
            {

                var clientcheck = (from x in edb.Clients
                                   where x.ClientID == rc.ClientID
                                   select x).Count();

                if (clientcheck > 0)
                {


                    clients.Add(rc.ClientID);

                }


            }


            foreach (int c in clients)
            {


                ClientViewModel modelC = new ClientViewModel();
                modelC.ClientID = c;

                clientlist.Add(modelC);


            }

            // FP
            foreach (RequestPerson rc in reqpersonlist)
            {

                var personcheck = (from x in wdb.People
                                   where x.PersonID == rc.PersonID
                                   select x).Count();

                if (personcheck > 0)
                {


                    persons.Add(rc.PersonID);

                }


            }


            foreach (int c in persons)
            {


                ClientViewModel modelC = new ClientViewModel();
                modelC.ClientID = c;

                personlist.Add(modelC);


            }


            var reqscreeninglist = (from B in db.RequestScreenings select B).Distinct().ToList();
            // Build List of Valid Screenings
            var screeninglist = new List<ScreeningViewModel>();
            var screens = new List<int>();

            foreach (RequestScreening rs in reqscreeninglist)
            {

                var screencheck = (from x in edb.Screenings
                                   where x.ScreeningID == rs.ScreeningID
                                   select x).Count();

                if (screencheck > 0)
                {


                    screens.Add(rs.ScreeningID);

                }


            }


            foreach (int s in screens)
            {


                ScreeningViewModel model = new ScreeningViewModel();
                model.ScreeningID = s;

                screeninglist.Add(model);


            }



            var useridreqlist = (from B in db.UserIdRequests select B).Distinct().ToList();
            var reqtypelist = (from B in db.RequestTypes select B).ToList();
            var reqactionlist = (from B in db.RequestActions select B).ToList();
            // var reqtrackerlist = (from B in db.RequestTrackers select B).ToList();


            var prioritylist = (from B in db.Priorities select B).ToList();
            var statelist = (from B in db.States select B).ToList();
            var ganttlist = (from B in db.GanttTasks select B).ToList();
            var userlist = (from B in context.AspNetUsers select B).Distinct().ToList();
            var siteroleprogramlist = (from B in context.SiteRoleProgramUserProfiles select B).Distinct().ToList();
            var sitelist = (from B in context.Sites select B).Distinct().ToList();
            var agencysiteprogramlist = (from B in context.AgencySiteProgramSites select B).Distinct().ToList();
            var agencysitelist = (from B in context.AgencySites select B).Distinct().ToList();

            if (userinfo.RoleBinID == 1 || userinfo.RoleBinID == 13)
            {


                if (id == 20)
                {

                    var reqfinallist = new List<RequestViewModel>();

                    foreach (ScreeningViewModel ssc in screeninglist)
                    {


                        var screencheck = (from x in edb.Screenings
                                           where x.ScreeningID == ssc.ScreeningID
                                           select x).Count();

                        if (screencheck > 0)
                        {

                            var request = (from x in reqlist
                                           //join ddf in db.RequestClients
                                           //on x.RequestID equals ddf.RequestID
                                           //into dddf
                                           //from ddddf in dddf.DefaultIfEmpty()
                                           join dde in db.RequestScreenings
                                           on x.RequestID equals dde.RequestID
                                           //into ddde
                                           //from dddde in ddde.DefaultIfEmpty()
                                           //join rr in reqactionlist
                                           //on x.RequestActionID equals rr.RequestActionID
                                           join mm in statelist
                                           on x.StateID equals mm.StateID
                                           //join ddd in ganttlist
                                           //on x.ProjectID equals ddd.ID
                                           join dddd in userlist
                                           on x.RequestorID equals dddd.UserName
                                           //join srp in context.SiteRoleProgramUserProfiles
                                           //on dddd.Id equals srp.Id
                                           //join s in context.Sites
                                           //on srp.SiteID equals s.SiteID
                                           //join asp in context.AgencySiteProgramSites
                                           //on srp.SiteID equals asp.SiteID
                                           //join ass in context.AgencySites
                                           //on asp.AgencySiteID equals ass.AgencySiteID
                                           where x.ProjectID == 69 && x.ApplicationID == 6 && dde.ScreeningID == ssc.ScreeningID

                                           select new RequestViewModel()
                                           {
                                               RequestID = x.RequestID,
                                               RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                               DueDate = x.DueDate,
                                               UserID = x.UserID,
                                               RequestTrackerTypeName = "Screening",
                                               RequestTrackerComment = (from ba in db.RequestTrackers
                                                                        where x.RequestID == ba.RequestID
                                                                        select ba.RequestDesc).First(),
                                               // DaysOpen = DbFunctions.DiffDays(Convert.ToDateTime(x.RequestDate), DateTime.Now).Value,
                                               // ScreeningID = dddde.ScreeningID,
                                               //ClientID = ddddf.ClientID,
                                               // RequestTypeName = aa.RequestTypeName,
                                               //  RequestActionName = rr.RequestActionName,
                                               //AssetCount = (from ba in db.Assets
                                               //              where ba.RequestID == x.RequestID
                                               //              select ba.AssetID).Count(),
                                               RequestTitle = x.RequestTitle,
                                               // ProjectName = ddd.Title,
                                               StateName = mm.StateName,
                                               RequestorID = x.RequestorID,
                                               RequestDesc = x.RequestDesc,
                                               UserName = dddd.FirstName + " " + dddd.LastName,
                                               // AgencyName = ass.AgencySiteName



                                           }).Distinct().ToList();
                            foreach (RequestViewModel r in request)
                            {

                                bool alreadyExists = reqfinallist.Any(x => x.RequestID == r.RequestID);

                                if (alreadyExists == false)
                                {

                                    reqfinallist.Add(r);



                                }

                            }



                        }


                    }


                    foreach (ClientViewModel ccc in clientlist)
                    {


                        var clientcheck = (from x in edb.Clients
                                           where x.ClientID == ccc.ClientID
                                           select x).Count();

                        if (clientcheck > 0)
                        {

                            var requestC = (from x in reqlist
                                            join ddf in db.RequestClients
                                            on x.RequestID equals ddf.RequestID
                                            //into dddf
                                            //from ddddf in dddf.DefaultIfEmpty()
                                            // join dde in db.RequestScreenings
                                            //  on x.RequestID equals dde.RequestID
                                            //into ddde
                                            //from dddde in ddde.DefaultIfEmpty()
                                            //join rr in reqactionlist
                                            //on x.RequestActionID equals rr.RequestActionID
                                            join mm in statelist
                                            on x.StateID equals mm.StateID
                                            //join ddd in ganttlist
                                            //on x.ProjectID equals ddd.ID
                                            join dddd in userlist
                                            on x.RequestorID equals dddd.UserName
                                            //join srp in context.SiteRoleProgramUserProfiles
                                            //on dddd.Id equals srp.Id
                                            //join s in context.Sites
                                            //on srp.SiteID equals s.SiteID
                                            //join asp in context.AgencySiteProgramSites
                                            //on srp.SiteID equals asp.SiteID
                                            //join ass in context.AgencySites
                                            //on asp.AgencySiteID equals ass.AgencySiteID
                                            where x.ProjectID == 69 && x.ApplicationID == 6 && ddf.ClientID == ccc.ClientID

                                            select new RequestViewModel()
                                            {
                                                RequestID = x.RequestID,
                                                RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                                DueDate = x.DueDate,
                                                UserID = x.UserID,
                                                RequestTrackerTypeName = "Client",
                                                RequestTrackerComment = (from ba in db.RequestTrackers
                                                                         where x.RequestID == ba.RequestID
                                                                         select ba.RequestDesc).First(),
                                                RequestTitle = x.RequestTitle,
                                                // ProjectName = ddd.Title,
                                                StateName = mm.StateName,
                                                RequestorID = x.RequestorID,
                                                RequestDesc = x.RequestDesc,
                                                UserName = dddd.FirstName + " " + dddd.LastName,
                                                // AgencyName = ass.AgencySiteName



                                            }).Distinct().ToList();
                            foreach (RequestViewModel rc in requestC)
                            {


                                bool alreadyExists = reqfinallist.Any(x => x.RequestID == rc.RequestID);

                                if (alreadyExists == false)
                                {


                                    reqfinallist.Add(rc);



                                }

                            }



                        }


                    }

                    return PartialView(reqfinallist);

                }
                else
                {

                    var reqfinallist = new List<RequestViewModel>();

                    foreach (ClientViewModel ccc in personlist)
                    {


                        var personcheck = (from x in wdb.People
                                           where x.PersonID == ccc.ClientID
                                           select x).Count();

                        if (personcheck > 0)
                        {

                            var requestC = (from x in reqlist
                                            join ddf in db.RequestPersons
                                            on x.RequestID equals ddf.RequestID
                                            //into dddf
                                            //from ddddf in dddf.DefaultIfEmpty()
                                            // join dde in db.RequestScreenings
                                            //  on x.RequestID equals dde.RequestID
                                            //into ddde
                                            //from dddde in ddde.DefaultIfEmpty()
                                            //join rr in reqactionlist
                                            //on x.RequestActionID equals rr.RequestActionID
                                            join mm in statelist
                                            on x.StateID equals mm.StateID
                                            //join ddd in ganttlist
                                            //on x.ProjectID equals ddd.ID
                                            join dddd in userlist
                                            on x.RequestorID equals dddd.UserName
                                            //join srp in context.SiteRoleProgramUserProfiles
                                            //on dddd.Id equals srp.Id
                                            //join s in context.Sites
                                            //on srp.SiteID equals s.SiteID
                                            //join asp in context.AgencySiteProgramSites
                                            //on srp.SiteID equals asp.SiteID
                                            //join ass in context.AgencySites
                                            //on asp.AgencySiteID equals ass.AgencySiteID
                                            where x.ProjectID == 71 && x.ApplicationID == 2 && ddf.PersonID == ccc.ClientID

                                            select new RequestViewModel()
                                            {
                                                RequestID = x.RequestID,
                                                RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                                DueDate = x.DueDate,
                                                UserID = x.UserID,
                                                RequestTrackerTypeName = "Client",
                                                RequestTrackerComment = (from ba in db.RequestTrackers
                                                                         where x.RequestID == ba.RequestID
                                                                         select ba.RequestDesc).First(),
                                                RequestTitle = x.RequestTitle,
                                                // ProjectName = ddd.Title,
                                                StateName = mm.StateName,
                                                RequestorID = x.RequestorID,
                                                RequestDesc = x.RequestDesc,
                                                UserName = dddd.FirstName + " " + dddd.LastName,
                                                // AgencyName = ass.AgencySiteName



                                            }).Distinct().ToList();
                            foreach (RequestViewModel rc in requestC)
                            {


                                bool alreadyExists = reqfinallist.Any(x => x.RequestID == rc.RequestID);

                                if (alreadyExists == false)
                                {


                                    reqfinallist.Add(rc);



                                }

                            }



                        }


                    }



                    return PartialView(reqfinallist);

                }



                return PartialView();



            }
        
            else
            {


                var reqfinallist = new List<RequestViewModel>();

                foreach (ScreeningViewModel ssc in screeninglist)
                {


                    var screencheck = (from x in edb.Screenings
                                       where x.ScreeningID == ssc.ScreeningID
                                       select x).Count();

                    if (screencheck > 0)
                    {

                        var request = (from x in reqlist
                                       //join ddf in db.RequestClients
                                       //on x.RequestID equals ddf.RequestID
                                       //into dddf
                                       //from ddddf in dddf.DefaultIfEmpty()
                                       join dde in db.RequestScreenings
                                       on x.RequestID equals dde.RequestID
                                       //into ddde
                                       //from dddde in ddde.DefaultIfEmpty()
                                       //join rr in reqactionlist
                                       //on x.RequestActionID equals rr.RequestActionID
                                       join mm in statelist
                                       on x.StateID equals mm.StateID
                                       //join ddd in ganttlist
                                       //on x.ProjectID equals ddd.ID
                                       join dddd in userlist
                                       on x.UserID equals dddd.Id
                                       //join srp in context.SiteRoleProgramUserProfiles
                                       //on dddd.Id equals srp.Id
                                       //join s in context.Sites
                                       //on srp.SiteID equals s.SiteID
                                       //join asp in context.AgencySiteProgramSites
                                       //on srp.SiteID equals asp.SiteID
                                       //join ass in context.AgencySites
                                       //on asp.AgencySiteID equals ass.AgencySiteID
                                       where x.RequestorID == UserName

                                       select new RequestViewModel()
                                       {
                                           RequestID = x.RequestID,
                                           RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                           DueDate = x.DueDate,
                                           UserID = x.UserID,
                                           RequestTrackerTypeName = "Screening",
                                           // DaysOpen = DbFunctions.DiffDays(Convert.ToDateTime(x.RequestDate), DateTime.Now).Value,
                                           // ScreeningID = dddde.ScreeningID,
                                           //ClientID = ddddf.ClientID,
                                           // RequestTypeName = aa.RequestTypeName,
                                           //  RequestActionName = rr.RequestActionName,
                                           //AssetCount = (from ba in db.Assets
                                           //              where ba.RequestID == x.RequestID
                                           //              select ba.AssetID).Count(),
                                           RequestTitle = x.RequestTitle,
                                           // ProjectName = ddd.Title,
                                           StateName = mm.StateName,
                                           RequestorID = x.RequestorID,
                                           RequestDesc = x.RequestDesc,
                                           UserName = dddd.FirstName + " " + dddd.LastName,
                                           // AgencyName = ass.AgencySiteName



                                       }).Distinct().ToList();
                        foreach (RequestViewModel r in request)
                        {

                            bool alreadyExists = reqfinallist.Any(x => x.RequestID == r.RequestID);

                            if (alreadyExists == false)
                            {



                                reqfinallist.Add(r);



                            }

                        }



                    }


                }


                foreach (ClientViewModel ccc in clientlist)
                {


                    var clientcheck = (from x in edb.Clients
                                       where x.ClientID == ccc.ClientID
                                       select x).Count();

                    if (clientcheck > 0)
                    {

                        var requestC = (from x in reqlist
                                        join ddf in db.RequestClients
                                        on x.RequestID equals ddf.RequestID
                                        //into dddf
                                        //from ddddf in dddf.DefaultIfEmpty()
                                        // join dde in db.RequestScreenings
                                        //  on x.RequestID equals dde.RequestID
                                        //into ddde
                                        //from dddde in ddde.DefaultIfEmpty()
                                        //join rr in reqactionlist
                                        //on x.RequestActionID equals rr.RequestActionID
                                        join mm in statelist
                                        on x.StateID equals mm.StateID
                                        //join ddd in ganttlist
                                        //on x.ProjectID equals ddd.ID
                                        join dddd in userlist
                                        on x.UserID equals dddd.Id
                                        //join srp in context.SiteRoleProgramUserProfiles
                                        //on dddd.Id equals srp.Id
                                        //join s in context.Sites
                                        //on srp.SiteID equals s.SiteID
                                        //join asp in context.AgencySiteProgramSites
                                        //on srp.SiteID equals asp.SiteID
                                        //join ass in context.AgencySites
                                        //on asp.AgencySiteID equals ass.AgencySiteID
                                        where x.RequestorID == UserName

                                        select new RequestViewModel()
                                        {
                                            RequestID = x.RequestID,
                                            RequestDateActual = Convert.ToDateTime(x.RequestDateActual),
                                            DueDate = x.DueDate,
                                            UserID = x.UserID,
                                            RequestTrackerTypeName = "Client",
                                            // ScreeningID = dddde.ScreeningID,
                                            //ClientID = ddddf.ClientID,
                                            // RequestTypeName = aa.RequestTypeName,
                                            //  RequestActionName = rr.RequestActionName,
                                            //AssetCount = (from ba in db.Assets
                                            //              where ba.RequestID == x.RequestID
                                            //              select ba.AssetID).Count(),
                                            RequestTitle = x.RequestTitle,
                                            // ProjectName = ddd.Title,
                                            StateName = mm.StateName,
                                            RequestorID = x.RequestorID,
                                            RequestDesc = x.RequestDesc,
                                            UserName = dddd.FirstName + " " + dddd.LastName,
                                            // AgencyName = ass.AgencySiteName



                                        }).Distinct().ToList();
                        foreach (RequestViewModel rc in requestC)
                        {

                            bool alreadyExists = reqfinallist.Any(x => x.RequestID == rc.RequestID);

                            if (alreadyExists == false)
                            {

                                reqfinallist.Add(rc);


                            }


                        }



                    }


                }


                return PartialView(reqfinallist);



            }

            return PartialView();

        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create(int? id)
        {


            string UserName = @User.Identity.Name.ToString();
            ViewBag.UserName = UserName;

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();



            ViewBag.ApplicationID = id;
            ViewBag.ProjectID = new SelectList(db.GanttTasks.Where(x => x.ApplicationID == id), "ID", "Title");
            ViewBag.RequestProgramID = new SelectList(db.RequestPrograms, "RequestProgramID", "RequestProgramName");
            ViewBag.RequestTypeID = new SelectList(db.RequestTypes.Where(x => x.Active == 1 && x.RequestModeID == 1), "RequestTypeID", "RequestTypeName");
            ViewBag.RequestActionID = new SelectList(db.RequestActions.Where(x => x.Active == 1), "RequestActionID", "RequestActionName");
            ViewBag.PriorityID = new SelectList(db.Priorities, "PriorityID", "PriorityName");
            ViewBag.StateID = new SelectList(db.States, "StateID", "StateName");


            IEnumerable OptionF = context.AspNetUsers.Where(c => c.Active == true && c.Id != userid.Id).AsEnumerable().Select(c => new SelectListItem()

            {
                //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                Text = c.FirstName + " " + c.LastName,
                Value = c.Id.ToString(),
                Selected = true,
            });


            var NewtermsF = new SelectList(OptionF, "Value", "Text");
            ViewBag.OptionListF = NewtermsF;


       
            ViewBag.PossibleUserProfileBins = context.AspNetUsers.Where(x => x.Active == true);



            return PartialView();


        }


        public JsonResult _AddRequestF(Request request, string UploadedFiles)
        {

            if (ModelState.IsValid)
            {

                //if (request.Urgent == true)
                //{

                //    request.PriorityID = 8;

                //}


                //var assigneename = (from B in dbAdmin.UserProfiles
                //                    where B.UserID == request.UserID
                //                    select B).SingleOrDefault();

                //request.RequestorViewed = true;
                //request.AssigneeViewed = false;


                //db.Requests.Add(request);

                //int Id1 = request.RequestID;
                //string User = request.RequestorID;

                //string Assignee = assigneename.UserName;
                //string RequestTitle = request.RequestTitle;
                //string RequestDes = request.RequestDesc;
                //string DeveloperNote = request.TestLog;
                //DateTime Created = DateTime.Now;
                //DateTime Created1 = DateTime.Now;
                //int? Stat = request.StateID;
                //int? Pri = request.PriorityID;

                //RequestTracker RT = new RequestTracker
                //{

                //    RequestID = Id1,
                //    Assignee = assigneename.UserName,
                //    Status = Stat,
                //    Priority = Pri,
                //    DateUpdated = Created,
                //    UpdatedBy = User,
                //    DateCreated = Created1,
                //    CreatedBy = User,
                //    RequestDesc = RequestDes,
                //    DeveloperNotes = DeveloperNote

                //};

                //db.RequestTrackers.Add(RT);
                //db.SaveChanges();

                //int RID = RT.RequestID;
                //int RTID = RT.RequestTrackerID;
                //string CreatedBy = Membership.GetUser().UserName.ToString();
                //DateTime DateCreated = DateTime.Now;




                //if (UploadedFiles != "")
                //{

                //    // Program
                //    var stringToSplit = UploadedFiles;

                //    var query = from val in stringToSplit.Split(',')
                //                select val;
                //    foreach (string val in query)
                //    {

                //        // Add Assets to Request



                //        Asset a = new Asset
                //        {

                //            RequestID = RTID,
                //            AssetTitle = Path.GetFileNameWithoutExtension(val),
                //            AssetURL = val,
                //            DateCreated = DateCreated,
                //            CreatedBy = CreatedBy


                //        };



                //        try
                //        {
                //            db.Assets.Add(a);
                //            db.SaveChanges();
                //        }
                //        catch (DbEntityValidationException dbEx)
                //        {
                //            foreach (var validationErrors in dbEx.EntityValidationErrors)
                //            {
                //                foreach (var validationError in validationErrors.ValidationErrors)
                //                {
                //                    Debug.WriteLine("Property: {0} Error: {1}",
                //               validationError.PropertyName, validationError.ErrorMessage);
                //                }
                //            }
                //        }



                //    }


                //}



                //if (request.Participants != null)
                //{


                //    // Participants
                //    var stringToSplitS = request.Participants;

                //    var queryS = from valS in stringToSplitS.Split(',')
                //                 select valS;
                //    foreach (string valS in queryS)
                //    {

                //        Guid g = new Guid(valS);

                //        UserIdRequests uir = new UserIdRequests
                //        {


                //            UserId = g,
                //            RequestID = RID,

                //        };

                //        db.UserIdRequests.Add(uir);
                //        db.SaveChanges();


                //        var followerphone = (from B in dbAdmin.UserProfiles
                //                             where B.UserID == g
                //                             select B).SingleOrDefault();


                //        if (Pri == 8)
                //        {

                //            var twilioRestClientP = new TwilioRestClient(
                //            ConfigurationManager.AppSettings.Get("TwilioSid"),
                //            ConfigurationManager.AppSettings.Get("TwilioToken"));



                //            twilioRestClientP.SendSmsMessage(
                //                "+17209032285",
                //                followerphone.Phone,
                //                string.Format(
                //                    "CMT Alert: " +
                //                    request.RequestTitle)
                //                );


                //        }


                //        var ZZ = (from W in dbAdmin.aspnet_Membership
                //                  join WW in dbAdmin.aspnet_Users on W.UserId equals WW.UserId
                //                  where WW.UserName == UserName
                //                  select W.Email).Single();


                //        var YY = (from M in dbAdmin.aspnet_Membership
                //                  join MM in dbAdmin.aspnet_Users on M.UserId equals MM.UserId
                //                  where MM.UserId == g
                //                  select M.Email).Single();

                //        var TT = (from S in dbAdmin.UserProfiles
                //                  where S.UserName == UserName
                //                  select S.FirstName).Single();

                //        var JJ = (from K in dbAdmin.UserProfiles
                //                  where K.UserName == UserName
                //                  select K.LastName).Single();


                //        SendFollowEmail(User, ZZ, YY, RequestTitle, RequestDes, TT, JJ);




                //    }

                //}


                //var Z = (from W in dbAdmin.aspnet_Membership
                //         join WW in dbAdmin.aspnet_Users on W.UserId equals WW.UserId
                //         where WW.UserName == UserName
                //         select W.Email).Single();


                //var Y = (from M in dbAdmin.aspnet_Membership
                //         join MM in dbAdmin.aspnet_Users on M.UserId equals MM.UserId
                //         where MM.UserId == request.UserID
                //         select M.Email).Single();

                //var T = (from S in dbAdmin.UserProfiles
                //         where S.UserName == UserName
                //         select S.FirstName).Single();

                //var J = (from K in dbAdmin.UserProfiles
                //         where K.UserName == UserName
                //         select K.LastName).Single();


                //if (Pri == 8)
                //{

                //    var twilioRestClient = new TwilioRestClient(
                // ConfigurationManager.AppSettings.Get("TwilioSid"),
                // ConfigurationManager.AppSettings.Get("TwilioToken"));

                //    twilioRestClient.SendSmsMessage(
                //       "+17209032285",
                //       assigneename.Phone,
                //       string.Format(
                //           "CMT Alert: " +
                //           request.RequestTitle)
                //       );

                //}

                //SendEmail(User, Z, Y, RequestTitle, RequestDes, T, J);


                //return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);



        }


        // GET: Requests/Create
        public ActionResult Edit(int? id)
        {


            string UserName = @User.Identity.Name.ToString();
            ViewBag.UserName = UserName;

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();



            ViewBag.RequestID = id;


            // Get clientid and screeningid if it exists

            // start with checking for screening id in request

       var rscount = (from x in db.RequestScreenings
                          where x.RequestID == id
                          select x).Count();

       if (rscount > 0)
       {

           var rsinfo = (from x in db.RequestScreenings
                          where x.RequestID == id
                          select x).SingleOrDefault();
           
           ViewBag.ScreeningID = rsinfo.ScreeningID;

           var screeninfo = (from x in edb.Screenings
                             join y in edb.ClinicalCycles
                             on x.ClinicalCycleID equals y.ClinicalCycleID
                             where x.ScreeningID == rsinfo.ScreeningID
                         select y).SingleOrDefault();

           ViewBag.ClientID = screeninfo.ClientID;


       }
       else
       {

           var csinfo = (from x in db.RequestClients
                         where x.RequestID == id
                         select x).SingleOrDefault();

           ViewBag.ClientID = csinfo.ClientID;
           ViewBag.ScreeningID = 0;




       }


            return PartialView();


        }



        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "RequestID,RequestDate,RequestDateActual,RequestorID,RequestTitle,DivisionID,RequestProgramID,RequestTypeID,PriorityID,StateID,RequestDesc,UserID,DeveloperNotes,TestLog,TimeToComplete,ReleaseDate,BroadcastDate,TrainingIssue,DueDate,RequestorNotes,RequestedByID,Overdue,RequestModeID,ApplicationID,OverdueWarning,RequestorViewed,AssigneeViewed,Requestor,TestingMode,Participants,SubApplicationID,ProjectID,Urgent,RequestActionID")] Request request)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Requests.Add(request);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(request);
        //}

        //// GET: Requests/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Request request = db.Requests.Find(id);
        //    if (request == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(request);
        //}

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestID,RequestDate,RequestDateActual,RequestorID,RequestTitle,DivisionID,RequestProgramID,RequestTypeID,PriorityID,StateID,RequestDesc,UserID,DeveloperNotes,TestLog,TimeToComplete,ReleaseDate,BroadcastDate,TrainingIssue,DueDate,RequestorNotes,RequestedByID,Overdue,RequestModeID,ApplicationID,OverdueWarning,RequestorViewed,AssigneeViewed,Requestor,TestingMode,Participants,SubApplicationID,ProjectID,Urgent,RequestActionID")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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
