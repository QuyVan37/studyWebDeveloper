using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS3.Controllers
{
    public class SendEmailController : Controller
    {
        // GET: SendEmail
        public void sendEmailToAdmin(string content)
        {
            // code send email to admin
        }
        public void sendEmailToUser(int userID, string content, string remark)
        {
            // code send email to user
        }
    }
}