using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class TicketsController : Controller
    {
        private ServiceRequestDBEntities db = new ServiceRequestDBEntities();

        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.Constituent).Include(t => t.IssueAddInfo).Include(t => t.IssueDetail).Include(t => t.Issue);
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
            ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName");
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo");
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details");
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description");

            
            return View();
        }
        /*
        [Route("Tickets/CreateTicket")]
        public ActionResult CreateTicket() //string Latitude, string Longitude, string Address, string ParcelNo, string CrossSt1, string CrossSt2, string Jurisdiction)
        {
            
            ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName");
            ViewBag.IssueAddInfoId = new SelectList(db.IssueAddInfoes, "IssueAddInfoId", "AdditionalInfo");
            ViewBag.IssueDetailId = new SelectList(db.IssueDetails, "IssueDetailId", "Details");
            ViewBag.IssueId = new SelectList(db.Issues, "IssueId", "Description");

            
            //TempData["Message"] = Latitude + " " + Longitude;
            //Session["Message"] = "Lat: " + Longitude + ", Lng: " + Longitude + ", Address: " + Address + 
            //    ", ParcelNo: " + ParcelNo + ", CrossSt1: " + CrossSt1 + ", CrossSt2: " + CrossSt2 + ", Jurisdiction: " + Jurisdiction;
            return View("Create");
        }*/

        
        public ActionResult SetTicketData(string Latitude, string Longitude, string Address, string ParcelNo, string CrossSt1, string CrossSt2, string Jurisdiction)
        {
            Session["Message"] = "Lat: " + Longitude + ", Lng: " + Longitude + ", Address: " + Address +
                ", ParcelNo: " + ParcelNo + ", CrossSt1: " + CrossSt1 + ", CrossSt2: " + CrossSt2 + ", Jurisdiction: " + Jurisdiction;

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
        public ActionResult Create([Bind(Include = "TicketId,ConstituentID,Service,IssueId,IssueDetailId,IssueAddInfoId,DateReported,TimeReported")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
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
            ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
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
        public ActionResult Edit([Bind(Include = "TicketId,ConstituentID,Service,IssueId,IssueDetailId,IssueAddInfoId,DateReported,TimeReported")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ConstituentID = new SelectList(db.Constituents, "ConstituentId", "FirstName", ticket.ConstituentID);
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
