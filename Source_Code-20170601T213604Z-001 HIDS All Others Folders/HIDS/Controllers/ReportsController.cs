using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CTL.Models;

namespace CTL.Controllers
{   
    public class ReportsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Reports/

        public ViewResult Index()
        {
            return View(context.Reports.ToList());
        }

        [Authorize]
        public ActionResult _ActiveReports()
        {

            var reportCount = (from ast in context.Reports
                             where ast.Active == true
                             select ast.ReportID).Count();

            ViewBag.ReportCount = reportCount;


            return PartialView(context.Reports.ToList());
        }

        //
        // GET: /Reports/Details/5

        public ViewResult Details(int id)
        {
            Report report = context.Reports.Single(x => x.ReportID == id);
            return View(report);
        }

        //
        // GET: /Reports/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Reports/Create

        [HttpPost]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                context.Reports.Add(report);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(report);
        }
        
        //
        // GET: /Reports/Edit/5
 
        public ActionResult Edit(int id)
        {
            Report report = context.Reports.Single(x => x.ReportID == id);
            return View(report);
        }

        //
        // POST: /Reports/Edit/5

        [HttpPost]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                context.Entry(report).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        //
        // GET: /Reports/Delete/5
 
        public ActionResult Delete(int id)
        {
            Report report = context.Reports.Single(x => x.ReportID == id);
            return View(report);
        }

        //
        // POST: /Reports/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = context.Reports.Single(x => x.ReportID == id);
            context.Reports.Remove(report);
            context.SaveChanges();
            return RedirectToAction("Index");
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