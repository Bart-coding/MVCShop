using MVCShop.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

namespace MVCShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult SendNewsletters()
        {
            ViewBag.Confirmed = false;
            ViewBag.Message = "Jesteś pewien, że chcesz wysłać Newsletter?";
            return View();
        }

        [HttpPost, ActionName("SendNewsletters")]
        [ValidateAntiForgeryToken]
        public ActionResult SendNewslettersConfirmed()
        {
            try
            {
                var dateInThePast = DateTime.Now.AddDays(-7);
                var news = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Date > dateInThePast)
                                                .OrderByDescending(p => p.Date)
                                                .Take(10)
                                                .ToList();

                StringBuilder sb = new StringBuilder();
                if (news.Count() > 0)
                {
                    sb.AppendLine("<h2>NOWOŚCI</h2>");
                    foreach (var product in news)
                    {
                        sb.Append("<p>" + product.Name + " - " + product.Price + "</p>");
                    }
                    sb.AppendLine("<br>");
                }

                var sales = db.Products.Where(p => p.Deleted == false && p.Visible == true && p.Discount != 0).ToList();
                if (sales.Count() > 0)
                {
                    sb.AppendLine("<h2>PROMOCJE</h2>");
                    foreach (var product in sales)
                    {
                        var priceAfterDiscount = decimal.Round(product.Price * (100 - product.Discount) * (decimal)0.01,2);
                        sb.AppendFormat("<p> {0} - {1} (<s>{2}</s>)</p>", product.Name, priceAfterDiscount, product.Price);
                    }
                    sb.AppendLine("<br>");
                }

                var body = sb.ToString();
                var subject = "Newsletter MVC SHOP";
                MailMessage mail = new MailMessage()
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // wysyłamy dla każdego kto ma taką opcję ustawioną w profilu
                var usersWithNewsleter = db.Users.Where(u => u.Newsletter).ToList();
                foreach (var user in usersWithNewsleter)
                {
                    var receiverEmail = new MailAddress(user.Email);
                    mail.To.Add(receiverEmail);
                }

                var senderEmail = new MailAddress("mvc.shop.123@gmail.com", "MVC SHOP");
                mail.From = senderEmail;
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, "admin@admin")
                };
                smtp.Send(mail);

                ViewBag.Confirmed = true;
                ViewBag.Message = "Newsletter został pomyślnie wysłany";
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Confirmed = true;
                ViewBag.Message = e.Message;
                return View();
            }
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