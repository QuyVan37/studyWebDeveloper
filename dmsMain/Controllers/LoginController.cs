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
    public class LoginController : Controller
    {
        private DMSEntities db = new DMSEntities();
        Controllers.CommonFunctionController commonFunction = new CommonFunctionController();

        // GET: Login
        public ActionResult index()
        {
            return View();
        }
        // post: Login
        [HttpPost]
        public ActionResult index(string code, string password)
        {
            var user = db.Users.Where(x => x.UserCode == code && x.Password == password).FirstOrDefault();
            if (user != null)
            {
                if (user.isLock == true) // not allow login => send massage lock reason.
                {

                }
                else
                {
                    // get session
                    createUserSession(user.UserID);
                    return Redirect("/home");
                }
            }
            ViewBag.err = "Password or Code not correct!";
            ViewBag.code = code;
            ViewBag.pass = password;
            return View();
        }


        public void createUserSession(int userID)
        {

            var d = commonFunction.getUserInformation(userID);
            Session["UserID"] = d.UserID;
            Session["Name"] = d.Name;
            Session["Code"] = d.Code;
            Session["Password"] = d.Password;
            Session["Dept"] = d.Dept;
            Session["GradeGradeName"] = d.GradeGradeName;
            Session["GradeLevel"] = d.GradeLevel;
            Session["MRRole"] = d.MRRole;
            Session["PORole"] = d.PORole;
            Session["TroubleRole"] = d.TroubleRole;
            Session["DieLaunchRole"] = d.DieLaunchRole;
            Session["TransferDieRole"] = d.TransferDieRole;
            Session["DSUMRole"] = d.DSUMRole;
            Session["IsAdmin"] = d.IsAdmin;
            Session["IsLock"] = d.IsLock;
        }


        public ActionResult register()
        {
            ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName");
            ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName");
            return View();
        }

        public JsonResult isExistAcc(string code)
        {
            var user = db.Users.Where(x => x.UserCode == code).FirstOrDefault();
            bool output = false;
            if (user == null)
            {
                output = false;
            }
            else
            {
                output = true;
            }
            return Json(output, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getPermition(int deptID, int gradeID)
        {
           var output = GenaratePermition(deptID, gradeID);
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        public class permtion
        {
            public bool isIssue { get; set; }
            public bool isCheck { get; set; }
            public bool isApprove { get; set; }
        }
        public class listPermition
        {
            public permtion MR { set; get; }
            public permtion PO { set; get; }
            public permtion Trouble { set; get; }
            public permtion Die_Launching { set; get; }
            public permtion DSUM { set; get; }
            public permtion Die_Transfer { set; get; }
        }
        public listPermition GenaratePermition(int deptID, int gradeID)
        {
            var dept = db.Departments.Find(deptID);
            var gradeLevel = db.UserGradeCatergories.Find(gradeID).GradeLevel;
            permtion mRRole = new permtion
            {
                isIssue = dept.IssueMRfromGradeLevel != 0 && dept.IssueMRfromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckMRFromGradeLevel != 0 && dept.CheckMRFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveMRFromGradeLevel != 0 && dept.ApproveMRFromGradeLevel <= gradeLevel ? true : false,
            };
            permtion poRole = new permtion
            {
                isIssue = dept.IssuePOFromGradeLevel != 0 && dept.IssuePOFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckPOFromGradeLevel != 0 && dept.CheckPOFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApprovePOFromGradeLevel != 0 && dept.ApprovePOFromGradeLevel <= gradeLevel ? true : false,
            };
            permtion troubleRole = new permtion
            {
                isIssue = dept.IssueTroubleFromGradeLevel != 0 && dept.IssueTroubleFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckTroubleFromGradeLevel != 0 && dept.CheckTroubleFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveTroubleFromGradeLevel != 0 && dept.ApproveTroubleFromGradeLevel <= gradeLevel ? true : false,
            };
            permtion dieLaunchRole = new permtion
            {
                isIssue = dept.IssueDielaunchFromGradeLevel != 0 && dept.IssueDielaunchFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDieLaunchFromGradeLevel != 0 && dept.CheckDieLaunchFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDieLaunchFromGradeLevel != 0 && dept.ApproveDieLaunchFromGradeLevel <= gradeLevel ? true : false,
            };
            permtion dsumRole = new permtion
            {
                isIssue = dept.IssueDSUMFromGradeLevel != 0 && dept.IssueDSUMFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDSUMFromGradeLevel != 0 && dept.CheckDSUMFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDSUMFromGradeLevel != 0 && dept.ApproveDSUMFromGradeLevel <= gradeLevel ? true : false,
            };
            permtion dieTransferRole = new permtion
            {
                isIssue = dept.IssueDieTransferFromGradeLevel != 0 && dept.IssueDieTransferFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDieTransferFromGradeLevel != 0 && dept.CheckDieTransferFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDieTransferFromGradeLevel != 0 && dept.ApproveDieTransferFromGradeLevel <= gradeLevel ? true : false,
            };

            listPermition output = new listPermition
            {
                MR = mRRole,
                PO = poRole,
                Trouble = troubleRole,
                Die_Launching = dieLaunchRole,
                DSUM = dsumRole,
                Die_Transfer = dieTransferRole,
            };
            return output;
        }























    // GET: Login/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        User user = db.Users.Find(id);
        if (user == null)
        {
            return HttpNotFound();
        }
        return View(user);
    }

    // GET: Login/Create
    public ActionResult Create()
    {
        ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName");
        return View();
    }

    // POST: Login/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "UserID,UserCode,UserName,Password,DeptID,GradeID,MRRole,TroubleRole,PORole,TranferDieRole,DieLaunchRole,isAdmin,isLock,isNew,CreateDate")] User user)
    {
        if (ModelState.IsValid)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName", user.GradeID);
        return View(user);
    }

    // GET: Login/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        User user = db.Users.Find(id);
        if (user == null)
        {
            return HttpNotFound();
        }
        ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName", user.GradeID);
        return View(user);
    }

    // POST: Login/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
    // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "UserID,UserCode,UserName,Password,DeptID,GradeID,MRRole,TroubleRole,PORole,TranferDieRole,DieLaunchRole,isAdmin,isLock,isNew,CreateDate")] User user)
    {
        if (ModelState.IsValid)
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName", user.GradeID);
        return View(user);
    }

    // GET: Login/Delete/5
    public ActionResult Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        User user = db.Users.Find(id);
        if (user == null)
        {
            return HttpNotFound();
        }
        return View(user);
    }

    // POST: Login/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        User user = db.Users.Find(id);
        db.Users.Remove(user);
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
