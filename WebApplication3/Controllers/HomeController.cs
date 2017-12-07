using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{    
    public class HomeController : Controller
    {
        private ServiceRequestDBEntities db = new ServiceRequestDBEntities();        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetHtml(string path)
        {
            return File(Server.MapPath("../Content/") + "index.html", "text/html");

            //return File(Server.MapPath(path) + "index.html", "text/html");
        }
        //[ChildActionOnly]
        //public ActionResult GetHtmlPage(string path)
        //{
        //    return new FilePathResult(path, "text/html");
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your about page";

            return View();
        }

        [Authorize]
        public ActionResult Location()
        {
            ViewBag.Message = "Select Address, Cross Streets or Parcel and enter required data or click (Drop pin) on map to select location";

            return View();
        }       

        public ActionResult MapTickets()
        {

            //var ticketsLoc = db.TicketLocations;

            var ticketsLoc = from ticket in db.Tickets
            join ticLoc in db.TicketLocations
            on ticket.TicketId equals ticLoc.TicketId
            orderby ticket.DateReported descending, ticket.TimeReported descending
            select ticLoc;
            /*
            var sortedReadings = ticketsLoc.OrderBy(x => x..TimeOfDay)
                .ThenBy(x => x.DateTimeOfReading.Date)
                .ThenBy(x => x.DateTimeOfReading.Year);*/


            //var json = new JavaScriptSerializer().Serialize(ticketsLatLng);
            var ticketsLatLng = ticketsLoc.Select(o => new { o.Latitude, o.Longitude, o.Location });
            Session["MapTicketsJson"] = ticketsLatLng;

            ViewBag.searchDisplay = "block";

            return View(ticketsLoc.ToList());
           
        }

        public ActionResult Top5Catg()
        {

            //>>>>>>>>> Need to implement this to show top 5 categories for tickets
            // Might neet to implement as separate view or sub view
            //>>>>>>>>>>>>>>>>
            
            var ticketsLoc = from ticket in db.Tickets
                             join ticLoc in db.TicketLocations
                             on ticket.TicketId equals ticLoc.TicketId
                             orderby ticket.DateReported descending, ticket.TimeReported descending
                             select ticLoc;
            /*
            var sortedReadings = ticketsLoc.OrderBy(x => x..TimeOfDay)
                .ThenBy(x => x.DateTimeOfReading.Date)
                .ThenBy(x => x.DateTimeOfReading.Year);*/


            //var json = new JavaScriptSerializer().Serialize(ticketsLatLng);
            var ticketsLatLng = ticketsLoc.Select(o => new { o.Latitude, o.Longitude, o.Location });
            Session["MapTicketsJson"] = ticketsLatLng;

            ViewBag.searchDisplay = "none";

            return View("MapTickets", ticketsLoc.ToList());

        }

        public ActionResult Last5Requests()
        {

            //var ticketsLoc = db.TicketLocations;

            var ticketsLoc = (from ticket in db.Tickets
                             join ticLoc in db.TicketLocations
                             on ticket.TicketId equals ticLoc.TicketId
                             orderby ticket.DateReported descending, ticket.TimeReported descending
                             select ticLoc).Take(5);
            /*
            var sortedReadings = ticketsLoc.OrderBy(x => x..TimeOfDay)
                .ThenBy(x => x.DateTimeOfReading.Date)
                .ThenBy(x => x.DateTimeOfReading.Year);*/


            //var json = new JavaScriptSerializer().Serialize(ticketsLatLng);
            var ticketsLatLng = ticketsLoc.Select(o => new { o.Latitude, o.Longitude, o.Location });
            Session["MapTicketsJson"] = ticketsLatLng;

            ViewBag.searchDisplay = "none";

            return View("MapTickets", ticketsLoc.ToList());

        }

        public ActionResult MapFilteredTickets()
        {
            var days = Request.Form["optradio1"];

            DateTime ticketDate = DateTime.Now.Date;
            ticketDate = ticketDate.AddDays(-Convert.ToInt32(days));


            string strIssueValue = "";
            int issueID = 1; // First Value
            if (Request.Form["selectIssue"] != null)
            {
                strIssueValue = Request.Form["selectIssue"].ToString();
                issueID = Convert.ToInt32(strIssueValue);
                if (issueID == 0)
                    issueID = 1;
            }

            string strDetailValue;
            int detailID = 0;
            if (Request.Form["selectDetails"] != null)
            {
                strDetailValue = Request.Form["selectDetails"].ToString();
                detailID = Convert.ToInt32(strDetailValue);
            }



            string straddInfoValue; 
            int addInfoID = 0;
            if (Request.Form["selectAddInfo"] != null)
            {
                straddInfoValue = Request.Form["selectAddInfo"].ToString();
                addInfoID = Convert.ToInt32(straddInfoValue);
            }

            
            var ticketStatus = Request.Form["optradio2"];



            var ticketsFiltered = db.Tickets.Where(o => o.DateReported > ticketDate && o.IssueId == issueID); ;
            if (detailID != 0 && addInfoID != 0)
            {
                ticketsFiltered = ticketsFiltered.Where(o => o.IssueDetailId == detailID && o.IssueAddInfoId == addInfoID);
            }
            else if (detailID != 0)
            {
                // only Issue selected
                ticketsFiltered = ticketsFiltered.Where(o => o.IssueDetailId == detailID);
            }
            
            
            var ticketsLoc = from ticket in ticketsFiltered
                             join ticLoc in db.TicketLocations
                         on ticket.TicketId equals ticLoc.TicketId
                         select ticLoc;

            


            //var ticketsLoc = db.TicketLocations;
            //var json = new JavaScriptSerializer().Serialize(ticketsLatLng);
            var ticketsLatLng = ticketsLoc.Select(o => new { o.Latitude, o.Longitude, o.Location });
            Session["MapTicketsJson"] = ticketsLatLng;

            ViewBag.searchDisplay = "block";

            return View("MapTickets", ticketsLoc.ToList());

        }

        public ActionResult GetTicketsLatLng()
        {            
            var ticLatLng = Session["MapTicketsJson"];
            return Json(ticLatLng, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Done(string address, string loc)
        {
            TempData["Message"] = address; // msg; 
            return new RedirectResult(@"~\Tickets\Create");
        }

        


        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult  Contact(string textBoxStringData, int textBoxIntData, string checkboxData)
        {
            //Do something
            SendEmail(this, null);
            ViewBag.Message = "Email submitted.";

            return View();
        }

        protected void SendEmail(object sender, EventArgs e)
        {
            string txtEmail = "asmithaabb@gmail.com";
            string txtPassword = "kzq1968AA!";
            string txtSubject = "Test email";
            string txtBody = "This is just a test email";
            string txtTo = "irfanzub@gmail.com";
            using (MailMessage mm = new MailMessage(txtEmail, txtTo))
            {
                mm.Subject = txtSubject;
                mm.Body = txtBody;
                //if (fuAttachment.HasFile)
                //{
                //    string FileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
                //    mm.Attachments.Add(new Attachment(fuAttachment.PostedFile.InputStream, FileName));
                //}
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(txtEmail, txtPassword);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                //ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Email sent.');", true);
            }
        }
    }
}