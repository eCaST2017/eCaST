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
    public class SupportTicketsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        //
        // GET: /SupportTickets/

        public ViewResult Index()
        {
            return View(context.SupportTickets.ToList());
        }

        //
        // GET: /SupportTickets/Details/5

        public ViewResult Details(int id)
        {
            SupportTicket supportticket = context.SupportTickets.Single(x => x.SupportTicketID == id);
            return View(supportticket);
        }

        //
        // GET: /SupportTickets/Create

        public ActionResult Create()
        {
            return PartialView();
        } 

        //
        // POST: /SupportTickets/Create

        [HttpPost]
        public ActionResult Create(SupportTicket supportticket)
        {
            if (ModelState.IsValid)
            {
                context.SupportTickets.Add(supportticket);
                context.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(supportticket);
        }
        
        //
        // GET: /SupportTickets/Edit/5
 
        public ActionResult Edit(int id)
        {
            SupportTicket supportticket = context.SupportTickets.Single(x => x.SupportTicketID == id);
            return View(supportticket);
        }

        //
        // POST: /SupportTickets/Edit/5

        [HttpPost]
        public ActionResult Edit(SupportTicket supportticket)
        {
            if (ModelState.IsValid)
            {
                context.Entry(supportticket).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supportticket);
        }

        //
        // GET: /SupportTickets/Delete/5
 
        public ActionResult Delete(int id)
        {
            SupportTicket supportticket = context.SupportTickets.Single(x => x.SupportTicketID == id);
            return View(supportticket);
        }

        //
        // POST: /SupportTickets/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SupportTicket supportticket = context.SupportTickets.Single(x => x.SupportTicketID == id);
            context.SupportTickets.Remove(supportticket);
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