using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DMS3.Models;

namespace DMS3.Controllers
{
    public class DCFsController : Controller
    {
        private DMSEntities db = new DMSEntities();

        // GET: DCFs
        public ActionResult Index()
        {
            if(Session["UserID"] == null)
            {
                Session["URL"] = HttpContext.Request.Url.AbsolutePath;
                return Redirect("/Login");
            }
            var dCFs = db.DCFs.Include(d => d.DCFStatusCategory);
            return View(dCFs.ToList());
        }


        public JsonResult getCDF(int id)
        {
            var cdf = db.DCFs.Find(id);

            return Json(cdf, JsonRequestBehavior.AllowGet);
        }





























        // GET: DCFs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DCF dCF = db.DCFs.Find(id);
            if (dCF == null)
            {
                return HttpNotFound();
            }
            return View(dCF);
        }

        // GET: DCFs/Create
        public ActionResult Create()
        {
            ViewBag.DCFID = new SelectList(db.DCFStatusCategories, "DCFStatusID", "Status");
            return View();
        }

        // POST: DCFs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DCFID,POID,DCFStatusID,NoOfcomponent,DieComponentName,ReceiveDCFDate,ReceivePhotoDate,Attachment,PUCConfirmBy,PUCConfirmDate,PUCAppBy,PUCAppDate,SubmitDCFby,SubmitDCFDate,SubmitPhotoBy,SubmitPhotoDate,CreateDate,Remark")] DCF dCF)
        {
            if (ModelState.IsValid)
            {
                db.DCFs.Add(dCF);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DCFID = new SelectList(db.DCFStatusCategories, "DCFStatusID", "Status", dCF.DCFID);
            return View(dCF);
        }

        // GET: DCFs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DCF dCF = db.DCFs.Find(id);
            if (dCF == null)
            {
                return HttpNotFound();
            }
            ViewBag.DCFID = new SelectList(db.DCFStatusCategories, "DCFStatusID", "Status", dCF.DCFID);
            return View(dCF);
        }

        // POST: DCFs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DCFID,POID,DCFStatusID,NoOfcomponent,DieComponentName,ReceiveDCFDate,ReceivePhotoDate,Attachment,PUCConfirmBy,PUCConfirmDate,PUCAppBy,PUCAppDate,SubmitDCFby,SubmitDCFDate,SubmitPhotoBy,SubmitPhotoDate,CreateDate,Remark")] DCF dCF)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dCF).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DCFID = new SelectList(db.DCFStatusCategories, "DCFStatusID", "Status", dCF.DCFID);
            return View(dCF);
        }

        // GET: DCFs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DCF dCF = db.DCFs.Find(id);
            if (dCF == null)
            {
                return HttpNotFound();
            }
            return View(dCF);
        }

        // POST: DCFs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DCF dCF = db.DCFs.Find(id);
            db.DCFs.Remove(dCF);
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
