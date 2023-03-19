using DMS3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS3.Controllers
{
    public class CommonFunctionController : Controller
    {
        private DMSEntities db = new DMSEntities();

        // Get User Information
        public class userInformation 
        { 
            public int UserID { set; get; }
            public string Name { set; get; }
            public string Code { set; get; }
            public string Email { set; get; }
            public string Password { set; get; }
            public string Dept { set; get; }
            public string GradeGradeName { set; get; }
            public int GradeLevel { set; get; }
            public string RQChangeMRRoleTo { set; get; }
            public string RQChangePORoleTo { set; get; }
            public string RQChangeTroubleRoleTo { set; get; }
            public string RQChangeDieLaunchRoleTo { set; get; }
            public string RQChangeTransferDieRoleTo { set; get; }
            public string RQChangeDSUMRoleTo { set; get; }
            public string MRRole { set; get; }
            public string PORole { set; get; }
            public string TroubleRole { set; get; }
            public string DieLaunchRole { set; get; }
            public string TransferDieRole { set; get; }
            public string DSUMRole { set; get; }
            public List<UserGivePermition> ListAsignment { set; get; }
            public List<UserGivePermition> ListRecieptAsignment { set; get; }
            public bool IsAdmin { set; get; }
            public bool IsLock { set; get; }
            public bool IsNewRQ { set; get; }

        }
        public userInformation getUserInformation(int userID)
        {
            var user = db.Users.Find(userID);
            var today = DateTime.Now;

            // Role được asign
            var MRRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "MR").FirstOrDefault()?.GiveRoleID ?? 0;
            var PORoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "PO").FirstOrDefault()?.GiveRoleID ?? 0;
            var TroubleRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "Trouble").FirstOrDefault()?.GiveRoleID ?? 0;
            var DieLaunchRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "DieLaunch").FirstOrDefault()?.GiveRoleID ?? 0;
            var TransferDieRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "DieTransfer").FirstOrDefault()?.GiveRoleID ?? 0;
            var DSUMRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "DUSM").FirstOrDefault()?.GiveRoleID ?? 0;
             userInformation output = new userInformation
            {
                UserID = user.UserID,
                Name = user.UserName,
                Code = user.UserCode,
                Email = user.Email,
                Password = user.Password,
                Dept = user.Department.DeptName,
                GradeGradeName = user.UserGradeCatergory.GradeName,
                GradeLevel = user.UserGradeCatergory.GradeLevel,


                // RQ Change Role from USer
                RQChangeMRRoleTo = db.DMSRoles.Find(user.RQChangeMRRoleToID)?.RoleName,
                RQChangePORoleTo = db.DMSRoles.Find(user.RQChangePORoleToID)?.RoleName,
                RQChangeTroubleRoleTo = db.DMSRoles.Find(user.RQChangeTroubleRoleToID)?.RoleName,
                RQChangeDieLaunchRoleTo = db.DMSRoles.Find(user.RQChangeDieLaunchRoleToID)?.RoleName,
                RQChangeTransferDieRoleTo = db.DMSRoles.Find(user.RQChangeTransferDieRoleToID)?.RoleName,
                RQChangeDSUMRoleTo = db.DMSRoles.Find(user.RQChangeDSUMRoleToID)?.RoleName,

               

                //Final Role
                MRRole = MRRoleRecieptedID != 0 ? db.DMSRoles.Find(MRRoleRecieptedID).RoleName :  db.DMSRoles.Find(user.MRRoleID).RoleName,
                PORole = PORoleRecieptedID != 0 ? db.DMSRoles.Find(PORoleRecieptedID).RoleName : db.DMSRoles.Find(user.PORoleID).RoleName,
                TroubleRole = TroubleRoleRecieptedID != 0 ? db.DMSRoles.Find(TroubleRoleRecieptedID).RoleName : db.DMSRoles.Find(user.TroubleRoleID).RoleName,
                DieLaunchRole = DieLaunchRoleRecieptedID != 0 ? db.DMSRoles.Find(DieLaunchRoleRecieptedID).RoleName : db.DMSRoles.Find(user.DieLaunchRoleID).RoleName,
                TransferDieRole = TransferDieRoleRecieptedID != 0 ? db.DMSRoles.Find(TransferDieRoleRecieptedID).RoleName : db.DMSRoles.Find(user.TransferDieRoleID).RoleName,
                DSUMRole = DSUMRoleRecieptedID != 0 ? db.DMSRoles.Find(DSUMRoleRecieptedID).RoleName : db.DMSRoles.Find(user.DSUMRoleID).RoleName,


                // Role Đã và Đang asign
                ListAsignment = db.UserGivePermitions.Where(x => x.GiverUserID == userID && x.isActive == true).ToList(),
                ListRecieptAsignment = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true).ToList(),

                IsAdmin = (bool)user.isAdmin,
                IsLock = (bool)user.isLock,
                IsNewRQ = (bool)user.isNewRQ
            };

            return output;

        }
    }
}
