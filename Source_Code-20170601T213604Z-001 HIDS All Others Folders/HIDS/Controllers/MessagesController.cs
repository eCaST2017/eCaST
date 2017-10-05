using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.Models;

namespace CTL.Controllers
{
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            return View(db.Messages.ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create(string id)
        {

            ViewBag.Id = id;

            return PartialView();
        }



        public JsonResult _AddMessageF(Message message, string Id)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserName = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

                var name = (from x in db.AspNetUsers
                            where x.UserName == UserName
                            select x).FirstOrDefault();

                var email = (from x in db.AspNetUsers
                            where x.Id == Id
                            select x).FirstOrDefault();


                Message message1 = new Message
                {

                    MessageEmail = email.Email,
                    MessageTitle = message.MessageTitle,
                    MessageContent = message.MessageContent,
                    Active = true,
                    DateCreated = CreatedInit,
                    CreatedBy = name.Id,
                    DateUpdated = CreatedInit,
                    UpdatedBy = name.FirstName + " " + name.LastName,

                };

                db.Messages.Add(message1);
                db.SaveChanges();


                SendMessageEmail(email.Email, message.MessageTitle, message.MessageContent, name.FirstName + " " + name.LastName);


                return Json(new { Status = "Success", Modified = name.Id }, JsonRequestBehavior.AllowGet);


            }


            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);


        }


        public void SendMessageEmail(string eEmail, string eTitle, string eMessage, string Sender)
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
            mail.From = new MailAddress("cdphe_healthinformatics@state.co.us");
            mail.To.Add(eEmail);
            //mail.CC.Add(eEmail);
            mail.Priority = MailPriority.Normal;
            mail.Subject = "Colorado Family Leadership Registry Message from" + " " + Sender + ": " + eTitle;

            //// SET THE BODY //
            mail.Body = mail.Body + "<TABLE VALIGN=TOP WIDTH='65%'>";
            mail.Body = mail.Body + "<TR>";
            mail.Body = mail.Body + "<TD>";
            mail.Body = mail.Body + "<p> " + Sender + " sent you this message:" + "</p>";
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


        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageID,MessageEmail,MessageTitle,MessageContent,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageID,MessageEmail,MessageTitle,MessageContent,Active,DateUpdated,UpdatedBy,DateCreated,CreatedBy")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
