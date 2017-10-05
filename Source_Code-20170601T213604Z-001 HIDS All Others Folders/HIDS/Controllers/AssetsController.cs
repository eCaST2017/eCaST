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
    public class AssetsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /Assets/

        public ViewResult Index()
        {
            return View(context.Assets.ToList());
        }


        [Authorize]
        public ActionResult _AssetList()
        {
            return PartialView(context.Assets.ToList());
        }

        //
        // GET: /Assets/Details/5

        public ViewResult Details(int id)
        {
            Asset asset = context.Assets.Single(x => x.AssetID == id);
            return View(asset);
        }

        //
        // GET: /Assets/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Assets/Create

        [HttpPost]
        public ActionResult Create(Asset asset)
        {
            if (ModelState.IsValid)
            {
                context.Assets.Add(asset);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(asset);
        }
        
        //
        // GET: /Assets/Edit/5
 
        public ActionResult Edit(int id)
        {
            Asset asset = context.Assets.Single(x => x.AssetID == id);
            return View(asset);
        }

        //
        // POST: /Assets/Edit/5

        [HttpPost]
        public ActionResult Edit(Asset asset)
        {
            if (ModelState.IsValid)
            {
                context.Entry(asset).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(asset);
        }

        //
        // GET: /Assets/Delete/5
 
        public ActionResult Delete(int id)
        {
            Asset asset = context.Assets.Single(x => x.AssetID == id);
            return View(asset);
        }

        //
        // POST: /Assets/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = context.Assets.Single(x => x.AssetID == id);
            context.Assets.Remove(asset);
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