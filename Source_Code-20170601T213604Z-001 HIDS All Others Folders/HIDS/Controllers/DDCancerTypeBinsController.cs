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
    public class DDCancerTypeBinsController : Controller
    {
        private eCaSTContextEntities db = new eCaSTContextEntities();
        private ApplicationDbContext portal = new ApplicationDbContext();
        // GET: DDCancerTypeBins
        public ActionResult Index()
        {
            return View(db.DDCancerTypeBins.ToList());
        }

        public ActionResult _CancerTypeList(int? id, int? id2)
        {

            ViewBag.RandomValue = id + id2;

            List<SelectListItem> items = new List<SelectListItem>();
            List<CancerTypeViewModel> CancerTypeItems = new List<CancerTypeViewModel>();
            var CancerType = (from P in portal.SubContractorClinics
                              where P.SiteID == id && P.ProgramID == id2
                              select P.CancerTypeBinID).ToList();


            foreach (int ct in CancerType)
            {

                var CT = (from P in db.DDCancerTypeBins
                          where P.CancerTypeBinID == ct
                          select new
                          {
                              CancerTypeBinID = P.CancerTypeBinID,
                              CancerTypeBinName = P.CancerTypeBinName

                          }).Distinct();


                int CancerTypeBinID = CT.FirstOrDefault().CancerTypeBinID;
                string CTID = CancerTypeBinID.ToString();
                string CancerTypeBinName = CT.FirstOrDefault().CancerTypeBinName;

                bool alreadyExists = items.Any(x => x.Value == CTID);

                if (alreadyExists == false)
                {

                    items.Add(new SelectListItem { Text = CancerTypeBinName, Value = CTID, Selected = false });


                }




            }

            foreach (SelectListItem num in items)
            {

                int CTID = Convert.ToInt32(num.Value);

                var cancertypes = (from c in db.DDCancerTypeBins
                                   where c.CancerTypeBinID == CTID
                                   select new CancerTypeViewModel()
                                   {

                                       CancerTypeBinID = c.CancerTypeBinID,
                                       CancerTypeBinName = c.CancerTypeBinName


                                   }).Distinct();



                if (cancertypes.Count() > 0)
                {
                    int CancerTypeBinID = cancertypes.FirstOrDefault().CancerTypeBinID;

                    string CancerTypeBinName = cancertypes.FirstOrDefault().CancerTypeBinName;


                    bool alreadyExists = CancerTypeItems.Any(x => x.CancerTypeBinID == CancerTypeBinID);

                    if (alreadyExists == false)
                    {

                        CancerTypeItems.Add(new CancerTypeViewModel() { CancerTypeBinID = CancerTypeBinID, CancerTypeBinName = CancerTypeBinName, SiteID = id, ProgramID = id2 });

                    }
                }



            }



            return PartialView(CancerTypeItems);

        }



        // GET: DDCancerTypeBins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DDCancerTypeBin dDCancerTypeBin = db.DDCancerTypeBins.Find(id);
            if (dDCancerTypeBin == null)
            {
                return HttpNotFound();
            }
            return View(dDCancerTypeBin);
        }

        // GET: DDCancerTypeBins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DDCancerTypeBins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CancerTypeBinID,CancerTypeBinName,Active")] DDCancerTypeBin dDCancerTypeBin)
        {
            if (ModelState.IsValid)
            {
                db.DDCancerTypeBins.Add(dDCancerTypeBin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dDCancerTypeBin);
        }

        // GET: DDCancerTypeBins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DDCancerTypeBin dDCancerTypeBin = db.DDCancerTypeBins.Find(id);
            if (dDCancerTypeBin == null)
            {
                return HttpNotFound();
            }
            return View(dDCancerTypeBin);
        }

        // POST: DDCancerTypeBins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CancerTypeBinID,CancerTypeBinName,Active")] DDCancerTypeBin dDCancerTypeBin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dDCancerTypeBin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dDCancerTypeBin);
        }

        // GET: DDCancerTypeBins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DDCancerTypeBin dDCancerTypeBin = db.DDCancerTypeBins.Find(id);
            if (dDCancerTypeBin == null)
            {
                return HttpNotFound();
            }
            return View(dDCancerTypeBin);
        }

        // POST: DDCancerTypeBins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DDCancerTypeBin dDCancerTypeBin = db.DDCancerTypeBins.Find(id);
            db.DDCancerTypeBins.Remove(dDCancerTypeBin);
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
