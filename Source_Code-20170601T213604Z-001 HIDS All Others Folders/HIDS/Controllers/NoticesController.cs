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
    public class NoticesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Notices
        public ActionResult Index(int? id)
        {

            ViewBag.ProgramID = id;

            return View(db.Notices.ToList());
        }

        // GET: Notices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var defaultnoticecount = (from x in db.Notices
                                 join y in db.DDNoticeTypeBins
                                 on x.NoticeTypeBinID equals y.NoticeTypeBinID
                                 where x.ProgramID == id && x.Default == true
                                 select x).Count();
            
            
            ViewBag.NoticeCount = defaultnoticecount;


            if (defaultnoticecount > 0)
            {


                var defaultnotice = (from x in db.Notices
                                     join y in db.DDNoticeTypeBins
                                     on x.NoticeTypeBinID equals y.NoticeTypeBinID
                                     where x.ProgramID == id && x.Default == true
                                     select new NoticeViewModel
                                     {

                                         NoticeID = x.NoticeID,
                                         NoticeDescription = x.NoticeDescription,
                                         NoticeName = x.NoticeName,
                                         NoticeTypeBinID = y.NoticeTypeBinID,
                                         NoticeTypeBinName = y.NoticeTypeBinName,
                                         Alert = y.Alert,
                                         Icon = y.Icon,
                                         Default = x.Default,
                                         Expires = x.Expires,
                                         ExpirationDate = x.ExpirationDate



                                     }).First();

                return PartialView(defaultnotice);


            }


            return PartialView();
        }

        // GET: Notices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Notices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoticeID,NoticeName,NoticeDescription,Active,ProgramID,NoticeTypeBinID,Default,Expires,ExpirationDate,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Notices.Add(notice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notice);
        }

        // GET: Notices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoticeID,NoticeName,NoticeDescription,Active,ProgramID,NoticeTypeBinID,Default,Expires,ExpirationDate,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Notices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
            db.Notices.Remove(notice);
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
