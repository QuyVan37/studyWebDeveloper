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
        Controllers.SendEmailController sendEmailJob = new SendEmailController();


        // GET: Login
        public ActionResult index()
        {
            if (Session["UserID"] != null)
            {
                logout();
            }
            return View();
        }
        // post: Login
        [HttpPost]
        public ActionResult index(string code, string password)
        {
            var user = db.Users.Where(x => x.UserCode == code && x.Password == password).FirstOrDefault();
            var msg = "Password or Code not correct!";
            if (user != null)
            {
                if (user.isLock == true) // not allow login => send massage lock reason.
                {
                    msg = "Your account is locked, if you just register, plz wait until reciept email announcement";
                    goto exit;
                }
                else
                {
                    // get session
                    createUserSession(user.UserID);
                    user.LastOnlineDate = DateTime.Now;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    if (Session["URL"] != null)
                    {
                        return Redirect(Session["URL"].ToString());
                    }

                    return Redirect("/home");
                }
            }
        exit:
            ViewBag.err = msg;
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
        [HttpPost]
        public ActionResult register(string code, string password, string name, string buyerCode, string email, string deptID, string gradeID,
            string rqChangeRoleMR, string rqChangeRolePO, string rqChangeRoleTrouble, string rqChangeRoleDieLaunch, string rqChangeRoleDieTransfer,
            string rqChangeRoleDUSM, string rqChangeRoleDCF, string action)
        {
            var msg = "";
            if (String.IsNullOrWhiteSpace(code))
            {
                msg = "Pls input employee code!";
                goto exit;
            }
            if (isExistAcc(code).Data.Equals(true))
            {
                msg = "This Code already register!";
                goto exit;
            }
            if (password?.Trim().Length < 6)
            {
                msg = "Password at least 6 character!";
                goto exit;
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                msg = "Pls input Name!";
                goto exit;
            }
            if (String.IsNullOrWhiteSpace(email))
            {
                msg = "Pls input Email!";
                goto exit;
            }
            if (String.IsNullOrWhiteSpace(deptID))
            {
                msg = "Pls select your department belong!";
                goto exit;
            }
            if (String.IsNullOrWhiteSpace(gradeID))
            {
                msg = "Pls select your grade to genarate you permition!";
                goto exit;
            }
            if (db.Departments.Find(int.Parse(deptID)).DeptName.Contains("PUR"))
            {
                if (String.IsNullOrWhiteSpace(buyerCode))
                {
                    msg = "If you is buyer, Plz input buyer code, if not input buyer code '-'";
                    goto exit;
                }
            }
            // 2. Register Acc
            User newUser = new User();
            newUser.UserCode = code;
            newUser.Password = password;
            newUser.UserName = name;
            newUser.BuyerCode = String.IsNullOrWhiteSpace(buyerCode) ? null : buyerCode;
            newUser.Email = email;
            newUser.DeptID = int.Parse(deptID);
            newUser.GradeID = int.Parse(gradeID);
            var genarateRole = GenaratePermition(newUser.DeptID, newUser.GradeID);
            newUser.MRRoleID = genarateRole.MR.isApprove ? 4 : genarateRole.MR.isCheck ? 3 : genarateRole.MR.isIssue ? 2 : 1;
            newUser.PORoleID = genarateRole.PO.isApprove ? 4 : genarateRole.PO.isCheck ? 3 : genarateRole.PO.isIssue ? 2 : 1;
            newUser.TroubleRoleID = genarateRole.Trouble.isApprove ? 4 : genarateRole.Trouble.isCheck ? 3 : genarateRole.Trouble.isIssue ? 2 : 1;
            newUser.DieLaunchRoleID = genarateRole.Die_Launching.isApprove ? 4 : genarateRole.Die_Launching.isCheck ? 3 : genarateRole.Die_Launching.isIssue ? 2 : 1;
            newUser.TransferDieRoleID = genarateRole.Die_Transfer.isApprove ? 4 : genarateRole.Die_Transfer.isCheck ? 3 : genarateRole.Die_Transfer.isIssue ? 2 : 1;
            newUser.DSUMRoleID = genarateRole.DSUM.isApprove ? 4 : genarateRole.DSUM.isCheck ? 3 : genarateRole.DSUM.isIssue ? 2 : 1;
            newUser.DCFRoleID = genarateRole.DCF.isApprove ? 4 : genarateRole.DCF.isCheck ? 3 : genarateRole.DCF.isIssue ? 2 : 1;
            newUser.RQChangeMRRoleToID = String.IsNullOrWhiteSpace(rqChangeRoleMR) ? newUser.RQChangeMRRoleToID : int.Parse(rqChangeRoleMR);
            newUser.RQChangePORoleToID = String.IsNullOrWhiteSpace(rqChangeRolePO) ? newUser.RQChangePORoleToID : int.Parse(rqChangeRolePO);
            newUser.RQChangeTroubleRoleToID = String.IsNullOrWhiteSpace(rqChangeRoleMR) ? newUser.RQChangeMRRoleToID : int.Parse(rqChangeRoleMR);
            newUser.RQChangeMRRoleToID = String.IsNullOrWhiteSpace(rqChangeRoleMR) ? newUser.RQChangeMRRoleToID : int.Parse(rqChangeRoleMR);
            newUser.RQChangeMRRoleToID = String.IsNullOrWhiteSpace(rqChangeRoleMR) ? newUser.RQChangeMRRoleToID : int.Parse(rqChangeRoleMR);
            newUser.RQChangeMRRoleToID = String.IsNullOrWhiteSpace(rqChangeRoleMR) ? newUser.RQChangeMRRoleToID : int.Parse(rqChangeRoleMR);
            newUser.isAdmin = false;
            newUser.isLock = true;
            newUser.isNewRQ = true;
            newUser.LastOnlineDate = DateTime.Now;
            newUser.CreateDate = DateTime.Now;
            if (action == "update")
            {
                userProfile();
            }
            db.Users.Add(newUser);
            db.SaveChanges();
            // send email to admin để phê duyệt
            sendEmailJob.sendEmailToAdmin("New Register DMS with code: " + code);
            return Redirect("/home");
        exit:
            ViewBag.err = msg;
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


        public ActionResult forgotpw()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgotpw(string code)
        {
            var msg = "";
            var u = db.Users.Where(x => x.UserCode == code).FirstOrDefault();
            if (u == null)
            {
                msg = "This code is not register DMS, Plz re-check it";
                goto exit;
            }
            if (u.isLock == true)
            {
                msg = "Your account is locked, plz contact DMS admin!";
            }
            else
            {
                var newPass = u.UserName.Substring(0, 1) + u.UserCode;
                u.Password = newPass;
                db.Entry(u).State = EntityState.Modified;
                db.SaveChanges();
                sendEmailJob.sendEmailToUser(u.UserID, "New Password: " + newPass, "");
                msg = "Your password was sent to your email!";
            }
        exit:
            ViewBag.err = msg;
            return View();
        }

        public JsonResult getUserInfor(int userID)
        {
            if (Session["IsAdmin"].Equals(true))
            {
                var user = commonFunction.getUserInformation(userID);
                return Json(new { status = true, user = user }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = false, user = "" }, JsonRequestBehavior.AllowGet);
        }


        public class permtion
        {
            public bool isIssue { get; set; }
            public bool isCheck { get; set; }
            public bool isApprove { get; set; }
            public string currentRole { get; set; }
        }
        public class listPermition
        {
            public permtion MR { set; get; }
            public permtion PO { set; get; }
            public permtion Trouble { set; get; }
            public permtion Die_Launching { set; get; }
            public permtion DSUM { set; get; }
            public permtion Die_Transfer { set; get; }
            public permtion DCF { set; get; }
        }
        public listPermition GenaratePermition(int deptID, int gradeID)
        {
            var user = new CommonFunctionController.userInformation();
            if(Session["UserID"] != null)
            {
                user = commonFunction.getUserInformation(int.Parse(Session["UserID"].ToString()));
            }
            var dept = db.Departments.Find(deptID);
            var gradeLevel = db.UserGradeCatergories.Find(gradeID).GradeLevel;
            permtion mRRole = new permtion
            {
                isIssue = dept.IssueMRfromGradeLevel != 0 && dept.IssueMRfromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckMRFromGradeLevel != 0 && dept.CheckMRFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveMRFromGradeLevel != 0 && dept.ApproveMRFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.MRRole
            };
            permtion poRole = new permtion
            {
                isIssue = dept.IssuePOFromGradeLevel != 0 && dept.IssuePOFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckPOFromGradeLevel != 0 && dept.CheckPOFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApprovePOFromGradeLevel != 0 && dept.ApprovePOFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.PORole
            };
            permtion troubleRole = new permtion
            {
                isIssue = dept.IssueTroubleFromGradeLevel != 0 && dept.IssueTroubleFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckTroubleFromGradeLevel != 0 && dept.CheckTroubleFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveTroubleFromGradeLevel != 0 && dept.ApproveTroubleFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.TroubleRole
                 
            };
            permtion dieLaunchRole = new permtion
            {
                isIssue = dept.IssueDielaunchFromGradeLevel != 0 && dept.IssueDielaunchFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDieLaunchFromGradeLevel != 0 && dept.CheckDieLaunchFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDieLaunchFromGradeLevel != 0 && dept.ApproveDieLaunchFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.DieLaunchRole

            };
            permtion dsumRole = new permtion
            {
                isIssue = dept.IssueDSUMFromGradeLevel != 0 && dept.IssueDSUMFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDSUMFromGradeLevel != 0 && dept.CheckDSUMFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDSUMFromGradeLevel != 0 && dept.ApproveDSUMFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.DSUMRole

            };
            permtion dieTransferRole = new permtion
            {
                isIssue = dept.IssueDieTransferFromGradeLevel != 0 && dept.IssueDieTransferFromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDieTransferFromGradeLevel != 0 && dept.CheckDieTransferFromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDieTransferFromGradeLevel != 0 && dept.ApproveDieTransferFromGradeLevel <= gradeLevel ? true : false,
                currentRole = user?.TransferDieRole

            };
            permtion DCFRole = new permtion
            {
                isIssue = dept.IssueDCFfromGradeLevel != 0 && dept.IssueDCFfromGradeLevel <= gradeLevel ? true : false,
                isCheck = dept.CheckDCFfromGradeLevel != 0 && dept.CheckDCFfromGradeLevel <= gradeLevel ? true : false,
                isApprove = dept.ApproveDCFfromGrafeLevel != 0 && dept.ApproveDCFfromGrafeLevel <= gradeLevel ? true : false,
                currentRole = user?.DCFRole

            };

            listPermition output = new listPermition
            {
                MR = mRRole,
                PO = poRole,
                Trouble = troubleRole,
                Die_Launching = dieLaunchRole,
                DSUM = dsumRole,
                Die_Transfer = dieTransferRole,
                DCF = DCFRole
            };
            return output;
        }

        public ActionResult userProfile()
        {
            if (Session["UserID"] == null)
            {
                return Redirect("/Login");
            }
            var user = db.Users.Find(int.Parse(Session["UserID"].ToString()));
            ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName", user.GradeID);
            ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName", user.DeptID);
            return View(user);
        }

        public ActionResult UserAdminControl()
        {
            if (Session["IsAdmin"].Equals(false))
            {
                return Redirect("/home");
            }
            var users = db.Users.ToList();
            ViewBag.QtyIsLock = users.Where(x => x.isLock == true).Count();
            ViewBag.QtyIsNewRQ = users.Where(x => x.isNewRQ == true).Count();
            ViewBag.GradeID = new SelectList(db.UserGradeCatergories, "GradeID", "GradeName");
            ViewBag.DeptID = new SelectList(db.Departments, "DeptID", "DeptName");
            ViewBag.DMSroleID = new SelectList(db.DMSRoles, "RoleID", "RoleName");
            return View(users.OrderByDescending(x => x.CreateDate).Take(50));
        }

        public ActionResult logout()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            return Redirect("/login");
        }

        public JsonResult AdminVerifyUser(int userID, string name, string code, string email, int gradeID, int deptID, string isAdmin, string isLock,
            int MRRoleID, int PORoleID, int TroubleRoleID, int DSUMRoleID, int DieLaunchRoleID, int TransferDieRoleID, string action, string reasonRefuse)
        {
            bool status = false;
            if (Session["IsAdmin"].Equals(true))
            {
                var u = db.Users.Find(userID);
                if (action == "save" || action == "verify")
                {
                    u.UserName = !String.IsNullOrWhiteSpace(name) ? name : u.UserName;
                    u.UserCode = !String.IsNullOrWhiteSpace(code) ? code : u.UserCode;
                    u.Email = !String.IsNullOrWhiteSpace(email) ? email : u.Email;
                    u.GradeID = gradeID;
                    u.DeptID = deptID;
                    u.isAdmin = isAdmin == "true" ? true : false;
                    u.isLock = isLock == "true" ? true : false;
                    u.isNewRQ = false;
                    u.MRRoleID = MRRoleID;
                    u.PORoleID = PORoleID;
                    u.TroubleRoleID = TroubleRoleID;
                    u.DSUMRoleID = DSUMRoleID;
                    u.DieLaunchRoleID = DieLaunchRoleID;
                    u.TransferDieRoleID = TransferDieRoleID;
                    if (action == "verify")
                    {
                        u.isLock = false;
                        u.RQChangeDCFRoleToID = null;
                        u.RQChangeDieLaunchRoleToID = null;
                        u.RQChangeDSUMRoleToID = null;
                        u.RQChangeMRRoleToID = null;
                        u.RQChangePORoleToID = null;
                        u.RQChangeTransferDieRoleToID = null;
                        u.RQChangeTroubleRoleToID = null;
                        sendEmailJob.sendEmailToUser(u.UserID, "Your DMS Account created successful!", "");
                    }
                    db.Entry(u).State = EntityState.Modified;
                    db.SaveChanges();
                    status = true;
                }
                if (action == "refuse")
                {
                    db.Entry(u).State = EntityState.Deleted;
                    db.SaveChanges();
                    sendEmailJob.sendEmailToUser(u.UserID, "Your request create DMS Account was refuse, plz register again and provide correct information !", reasonRefuse);
                    status = true;
                }

            }

            return Json(new { status = status }, JsonRequestBehavior.AllowGet);
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
