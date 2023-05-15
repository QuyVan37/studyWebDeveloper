
function showUserDetail(userID) {
    $.ajax({
        url: "/login/getUserInfor",
        type: 'POST',
        data: { 'userID': userID, },
        dataType: 'json',
        success: function (data) {
            if (data.status) {
                renderModalVerifyUser(data.user)
            } else {
                alert('You do not have permition!')
            }
        },
        fail: function () {
            alert('occur error!!')
        }
    })
}

function renderModalVerifyUser(user) {
    console.log(user)
    $('#userID').val(user.UserID)
    $('#name').val(user.Name)
    $('#code').val(user.Code)
    $('#email').val(user.Email)
    $('#GradeID').val(user.GradeID)
    $('#DeptID').val(user.DeptID)
    $('#isAdmin').val(user.IsAdmin?"true" :"false")
    $('#isLock').val(user.IsLock ? "true" : "false")
    $('#isNewRQ').val(user.IsNewRQ ? "true" : "false")
    $('#MRRoleID').val(user.MRRoleID)
    $('#PORoleID').val(user.PORoleID)
    $('#DieLaunchRoleID').val(user.DieLaunchRoleID)
    $('#TroubleRoleID').val(user.TroubleRoleID)
    $('#DSUMRoleID').val(user.DSUMRoleID)
    $('#TransferDieRoleID').val(user.TransferDieRoleID)
    $('#RQChangeMRroleID').val(user.RQChangeMRRoleID)
    $('#RQChangePOroleID').val(user.RQChangePORoleID)
    $('#RQChangeTroubleRoleID').val(user.RQChangeTroubleRoleID)
    $('#RQChangeDSUMRoleID').val(user.RQChangeDSUMRoleID)
    $('#RQChangeDieLaunchRoleID').val(user.RQChangeDieLaunchRoleID)
    $('#RQChangeTransferDieRoleID').val(user.RQChangeTransferDieRoleID)
    


    $('#modal_Admin_verify_user').modal('show');
}

$('.btn_user_action').click(function () {
    let action = $(this).val()
    let formdata = new FormData()
    formdata.append('userID', $('#userID').val())
    formdata.append('name', $('#name').val())
    formdata.append('code', $('#code').val())
    formdata.append('email', $('#email').val())
    formdata.append('gradeID', $('#GradeID').val())
    formdata.append('deptID', $('#DeptID').val())
    formdata.append('isAdmin', $('#isAdmin').val())
    formdata.append('isLock', $('#isLock').val())
    formdata.append('MRRoleID', $('#MRRoleID').val())
    formdata.append('PORoleID', $('#PORoleID').val())
    formdata.append('TroubleRoleID', $('#TroubleRoleID').val())
    formdata.append('DSUMRoleID', $('#DSUMRoleID').val())
    formdata.append('DieLaunchRoleID', $('#DieLaunchRoleID').val())
    formdata.append('TransferDieRoleID', $('#TransferDieRoleID').val())
    formdata.append('action', action)
    formdata.append('reasonRefuse', $('#reasonRefuse').val())
    if (action == 'refuse' && $('#reasonRefuse').val().length == 0) {
        alert("Please input reason refuse!")
        return
    } else {
        $.ajax({
            url: "/login/AdminVerifyUser",
            type: 'POST',
            data: formdata,
            processData: false,
            contentType: false,
            success: function (data) {
                if (data.status) {
                    alert('Success!')
                } else {
                    alert('You do not have permition!')
                }
            },
            fail: function () {
                alert('occur error!!')
            }
        })
    }
    
})