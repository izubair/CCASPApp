using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

using Microsoft.AspNet.Identity;

namespace WebApplication3.Controllers
{
    public class TicketsController : Controller
    {
        private ServiceRequestDBEntities db = new ServiceRequestDBEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.IssueAddInfo).Include(t => t.IssueDetail).Include(t => t.Issue);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            //ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName");
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo");
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details");
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description");
            return View();
        }

        public ActionResult SetTicketData(string Latitude, string Longitude, string Address, string ParcelNo, string CrossSt1, string CrossSt2, string Jurisdiction)
        {
            Session["Message"] = "Lat: " + Latitude + ", Lng: " + Longitude + ", Address: " + Address +
                ", ParcelNo: " + ParcelNo + ", CrossSt1: " + CrossSt1 + ", CrossSt2: " + CrossSt2 + ", Jurisdiction: " + Jurisdiction;
            Session["Lat"] = Latitude;
            Session["Lng"] = Longitude;
            Session["Address"] = Address;
            Session["ParcelNo"] = ParcelNo;
            Session["CrossSt1"] = CrossSt1;
            Session["CrossSt2"] = CrossSt2;
            Session["Jurisdiction"] = Jurisdiction;

            return Json("OK", JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetIssueDetail(int id)
        {
            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem() { Text = "Sub Item 1", Value = "1" });
            //items.Add(new SelectListItem() { Text = "Sub Item 2", Value = "8" });
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails.Where(o => o.IssueID == id).ToList()
                , "IssueDetailId", "Details");
            //return View();
            // you may replace the above code with data reading from database based on the id

            return Json(ViewBag.IssueDetailId, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetIssueAddInfo(int id, int detailId)
        {
            //List<SelectListItem> items = new List<SelectListItem>();
            //items.Add(new SelectListItem() { Text = "Sub Item 1", Value = "1" });
            //items.Add(new SelectListItem() { Text = "Sub Item 2", Value = "8" });
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes.Where(o => o.IssueID == id && o.IssueDetailID == detailId).ToList()
                , "IssueAddInfoId", "AdditionalInfo");
            //return View();
            // you may replace the above code with data reading from database based on the id

            return Json(ViewBag.IssueAddInfoId, JsonRequestBehavior.AllowGet);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketId,ConstituentID,Service,IssueId,IssueDetailId,IssueAddInfoId,DateReported,TimeReported,Subject,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                TicketLocation ticketLoc = new TicketLocation();

                //ticketLoc.jurisdiction = Convert.ToString(Session["Lat"]);
                ticket.ConstituentID = User.Identity.GetUserId();
                DateTime dttm = DateTime.Now;
                ticket.DateReported = dttm.Date;
                ticket.TimeReported = dttm.TimeOfDay;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                ticketLoc.Latitude = Convert.ToDouble(Session["Lat"]);
                ticketLoc.Longitude = Convert.ToDouble(Session["Lng"]);
                ticketLoc.Location = Convert.ToString(Session["Address"]);
                ticketLoc.ParcelNo = Convert.ToString(Session["ParcelNo"]);
                ticketLoc.CrossSt1 = Convert.ToString(Session["CrossSt1"]);
                ticketLoc.CrossSt2 = Convert.ToString(Session["CrossSt2"]);
                if (ticketLoc.Location.Contains("Henderson"))
                    ticketLoc.City = "Henderson";
                else
                    ticketLoc.City = "Las Vegas";
                ticketLoc.State = "NV";
                // Now use the identity of created ticket 
                ticketLoc.TicketId = ticket.TicketId;
                db.TicketLocations.Add(ticketLoc);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo", ticket.IssueAddInfoId);
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details", ticket.IssueDetailId);
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description", ticket.IssueId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            //ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo", ticket.IssueAddInfoId);
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details", ticket.IssueDetailId);
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description", ticket.IssueId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TicketId,ConstituentID,Service,IssueId,IssueDetailId,IssueAddInfoId,DateReported,TimeReported,Subject,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo", ticket.IssueAddInfoId);
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details", ticket.IssueDetailId);
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description", ticket.IssueId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
