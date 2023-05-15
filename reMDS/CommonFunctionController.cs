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
            public int DeptID { set; get; }
            public string GradeGradeName { set; get; }
            public int GradeID { set; get; }
            public int GradeLevel { set; get; }
            public int RQChangeMRRoleID { set; get; }
            public int RQChangePORoleID { set; get; }
            public int RQChangeTroubleRoleID { set; get; }
            public int RQChangeDieLaunchRoleID { set; get; }
            public int RQChangeTransferDieRoleID { set; get; }
            public int RQChangeDSUMRoleID { set; get; }
            public int RQChangeDCFRoleID { set; get; }
            public string MRRole { set; get; }
            public string PORole { set; get; }
            public string TroubleRole { set; get; }
            public string DieLaunchRole { set; get; }
            public string TransferDieRole { set; get; }
            public string DSUMRole { set; get; }
            public string DCFRole { set; get; }
            public int MRRoleID { set; get; }
            public int PORoleID { set; get; }
            public int TroubleRoleID { set; get; }
            public int DieLaunchRoleID { set; get; }
            public int TransferDieRoleID { set; get; }
            public int DSUMRoleID { set; get; }
            public int DCFRoleID { set; get; }
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
            var DCFRoleRecieptedID = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true && x.isCancel == false && x.GiveFromDate <= today && x.GiveToDate >= today && x.GiveRoleFunction == "DCF").FirstOrDefault()?.GiveRoleID ?? 0;
            userInformation output = new userInformation
            {
                UserID = user.UserID,
                Name = user.UserName,
                Code = user.UserCode,
                Email = user.Email,
                Password = user.Password,
                Dept = user.Department.DeptName,
                DeptID = user.DeptID,
                GradeGradeName = user.UserGradeCatergory.GradeName,
                GradeID = user.GradeID,
                GradeLevel = user.UserGradeCatergory.GradeLevel,
                MRRoleID = user.MRRoleID == null ? 0 : (int)user.MRRoleID,
                PORoleID = user.PORoleID == null ? 0 : (int)user.PORoleID,
                TroubleRoleID = user.TroubleRoleID == null ? 0 : (int)user.TroubleRoleID,
                DSUMRoleID = user.DSUMRoleID == null ? 0 : (int)user.DSUMRoleID,
                DieLaunchRoleID = user.DieLaunchRoleID == null ? 0 : (int)user.DieLaunchRoleID,
                TransferDieRoleID = user.TransferDieRoleID == null ? 0 : (int)user.TransferDieRoleID,
                DCFRoleID = user.DCFRoleID == null ? 0 : (int)user.DCFRoleID,
                // RQ Change Role from USer
                RQChangeMRRoleID = user.RQChangeMRRoleToID == null ? 0 : (int)user.RQChangeMRRoleToID,
                RQChangePORoleID = user.RQChangePORoleToID == null ? 0 : (int)user.RQChangePORoleToID,
                RQChangeTroubleRoleID = user.RQChangeTroubleRoleToID == null ? 0 : (int)user.RQChangeTroubleRoleToID,
                RQChangeDieLaunchRoleID = user.RQChangeDieLaunchRoleToID == null ? 0 : (int)user.RQChangeDieLaunchRoleToID,
                RQChangeTransferDieRoleID = user.RQChangeTransferDieRoleToID == null ? 0 : (int)user.RQChangeTransferDieRoleToID,
                RQChangeDSUMRoleID = user.RQChangeDSUMRoleToID == null ? 0 : (int)user.RQChangeDSUMRoleToID,
                RQChangeDCFRoleID = user.RQChangeDCFRoleToID == null ? 0 : (int)user.RQChangeDCFRoleToID,


                //Final Role
                MRRole = MRRoleRecieptedID != 0 ? db.DMSRoles.Find(MRRoleRecieptedID).RoleName : user.MRRoleID != null ? db.DMSRoles.Find(user.MRRoleID).RoleName: db.DMSRoles.Find(1).RoleName,
                PORole = PORoleRecieptedID != 0 ? db.DMSRoles.Find(PORoleRecieptedID).RoleName : user.PORoleID != null ? db.DMSRoles.Find(user.PORoleID).RoleName: db.DMSRoles.Find(1).RoleName,
                TroubleRole = TroubleRoleRecieptedID != 0 ? db.DMSRoles.Find(TroubleRoleRecieptedID).RoleName : user.TroubleRoleID != null ? db.DMSRoles.Find(user.TroubleRoleID).RoleName: db.DMSRoles.Find(1).RoleName,
                DieLaunchRole = DieLaunchRoleRecieptedID != 0 ? db.DMSRoles.Find(DieLaunchRoleRecieptedID).RoleName : user.DieLaunchRoleID != null ? db.DMSRoles.Find(user.DieLaunchRoleID).RoleName: db.DMSRoles.Find(1).RoleName,
                TransferDieRole = TransferDieRoleRecieptedID != 0 ? db.DMSRoles.Find(TransferDieRoleRecieptedID).RoleName : user.TransferDieRoleID != null ? db.DMSRoles.Find(user.TransferDieRoleID).RoleName : db.DMSRoles.Find(1).RoleName,
                DSUMRole = DSUMRoleRecieptedID != 0 ? db.DMSRoles.Find(DSUMRoleRecieptedID).RoleName : user.DSUMRoleID != null ? db.DMSRoles.Find(user.DSUMRoleID).RoleName : db.DMSRoles.Find(1).RoleName,
                DCFRole = DCFRoleRecieptedID != 0 ? db.DMSRoles.Find(DCFRoleRecieptedID).RoleName : user.DCFRoleID != null ? db.DMSRoles.Find(user.DCFRoleID).RoleName : db.DMSRoles.Find(1).RoleName,

                // Role Đã và Đang asign
                ListAsignment = db.UserGivePermitions.Where(x => x.GiverUserID == userID && x.isActive == true).ToList(),
                ListRecieptAsignment = db.UserGivePermitions.Where(x => x.ReciepterUserID == userID && x.isActive == true).ToList(),

                IsAdmin = user.isAdmin == true ? true : false ,
                IsLock = user.isLock == true ? true : false,
                IsNewRQ = user.isNewRQ == true ? true : false
            };

            return output;

        }
    }
}
