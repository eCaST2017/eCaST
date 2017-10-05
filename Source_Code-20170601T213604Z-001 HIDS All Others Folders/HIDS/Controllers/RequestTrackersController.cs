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

namespace CTL.Controllers
{
    public class RequestTrackersController : Controller
    {
        private PortalRequestEntitiesEntities db = new PortalRequestEntitiesEntities();
        private ApplicationDbContext context = new ApplicationDbContext();
        // GET: RequestTrackers
        public ActionResult Index()
        {
            return View(db.RequestTrackers.ToList());
        }


        public ActionResult _TrackerList(int? id, int? id2, int? id3)
        {


            string UserName = @User.Identity.Name.ToString();
            ViewBag.UserName = UserName;

            var userid = (from x in context.AspNetUsers
                          where x.UserName == UserName
                          select x).FirstOrDefault();

            ViewBag.UserID = userid.Id;

            ViewBag.ClientID = id;
            ViewBag.ScreeningID = id2;

            // on screening
            if (id2 > 0)
            {


                var trackers = (from x in db.RequestTrackers
                                join ff in db.Requests
                                on x.RequestID equals ff.RequestID
                                into fff
                                from ffff in fff.DefaultIfEmpty()
                                join dd in db.RequestScreenings
                                on x.RequestID equals dd.RequestID
                                into ddd
                                from dddd in ddd.DefaultIfEmpty()
                                where ffff.RequestID == id3
                                select new RequestTrackerViewModel
                                {

                                    RequestTrackerID = x.RequestTrackerID,
                                    RequestID = x.RequestID,
                                    CreatedBy = x.CreatedBy,
                                    DateCreated = x.DateCreated,
                                    RequestDesc = x.RequestDesc




                                }).ToList();




                return PartialView(trackers);



            }
            else
            {


                var trackers = (from x in db.RequestTrackers
                                join ff in db.Requests
                                on x.RequestID equals ff.RequestID
                                into fff
                                from ffff in fff.DefaultIfEmpty()
                                join dd in db.RequestClients
                                on x.RequestID equals dd.RequestID
                                into ddd
                                from dddd in ddd.DefaultIfEmpty()
                                where ffff.RequestID == id3
                                select new RequestTrackerViewModel
                                {

                                    RequestTrackerID = x.RequestTrackerID,
                                    RequestID = x.RequestID,
                                    CreatedBy = x.CreatedBy,
                                    DateCreated = x.DateCreated,
                                    RequestDesc = x.RequestDesc




                                }).ToList();




                return PartialView(trackers);



            }

            return PartialView();
        }


        // GET: RequestTrackers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTracker requestTracker = db.RequestTrackers.Find(id);
            if (requestTracker == null)
            {
                return HttpNotFound();
            }
            return View(requestTracker);
        }

        // GET: RequestTrackers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequestTrackers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequestTrackerID,RequestID,Assignee,Status,Priority,DateUpdated,UpdatedBy,DateCreated,CreatedBy,RequestDesc,DeveloperNotes")] RequestTracker requestTracker)
        {
            if (ModelState.IsValid)
            {
                db.RequestTrackers.Add(requestTracker);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requestTracker);
        }

        // GET: RequestTrackers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTracker requestTracker = db.RequestTrackers.Find(id);
            if (requestTracker == null)
            {
                return HttpNotFound();
            }
            return View(requestTracker);
        }

        // POST: RequestTrackers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequestTrackerID,RequestID,Assignee,Status,Priority,DateUpdated,UpdatedBy,DateCreated,CreatedBy,RequestDesc,DeveloperNotes")] RequestTracker requestTracker)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestTracker).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(requestTracker);
        }

        // GET: RequestTrackers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestTracker requestTracker = db.RequestTrackers.Find(id);
            if (requestTracker == null)
            {
                return HttpNotFound();
            }
            return View(requestTracker);
        }

        // POST: RequestTrackers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequestTracker requestTracker = db.RequestTrackers.Find(id);
            db.RequestTrackers.Remove(requestTracker);
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
