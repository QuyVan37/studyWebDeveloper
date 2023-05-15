

$('#btn-register').on('click', function (e) {
    e.preventDefault()
    let result = validateform()
    if (result) {
        $('form').submit()
    }
})
$('select').on('change', getPermitionGenarate)

getPermitionGenarate()
function getPermitionGenarate() {
    let deptID = $('select[name=DeptID]').val()
    let gradeID = $('select[name=GradeID]').val()
    if (deptID && gradeID) {
        $.ajax({
            url: "/login/getPermition",
            type: 'POST',
            data: { 'deptID': deptID, 'gradeID': gradeID },
            dataType: 'json',
            success: function (data) {
                renderPermition(data)
            },
            fail: function () {
                alert('occur error!!')
            }
        })
    }
}


function renderPermition(dataGenarate) {
    var data = dataGenarate
    
    $('.tableRenderRoleGenarade').html('')
    content = ''
    for (let func in data) {
        let role = data[func]

        content +=
            `
    <tr>
        <td>${func}</td>
        
        <td><i class="fa-solid fa-check"></i></td>
        <td>${role.isIssue ? '<i class="fa-solid fa-check"></i>' : '<i class="fa-solid fa-xmark"></i>'}</td>
        <td>${role.isCheck ? '<i class="fa-solid fa-check"></i>' : '<i class="fa-solid fa-xmark"></i>'}</td>
        <td>${role.isApprove ? '<i class="fa-solid fa-check"></i>' : '<i class="fa-solid fa-xmark"></i>'}</td>
        <td>${role.currentRole == null ? '' : role.currentRole }</td>
        <td><select name="rqChangeRole${func}" class="rqchangeRole">
            <option value=''></option>
                <option value='0'>View</option>
                    <option value='1'>Issue</option>
                        <option value='2'>Check</option>
                            <option value='3'>Approve</option>
                    </select></td>

                </tr>
                            `

    }
    $('.tableRenderRoleGenarade').html(content)
}
function validateform() {
    let code = $('input[name=code]').val()
    let pass = $('input[name=password]').val()
    let name = $('input[name=name]').val()
    let email = $('input[name=email]').val()
    let deptID = $('select[name=DeptID]').val()
    let gradeID = $('select[name=GradeID]').val()

    // 1.check code da ton tai chua
    isExistAcc(function (status) {
        if (status) {
            $('.verify_warning').text('This Code already register!')
            return false
        }
    })

    // 2.check da dien du form chua
    if (!code) {
        $('.verify_warning').text('Pls input employee code!')
        return false
    }
    if (pass.length < 6) {
        $('.verify_warning').text('Password at least 6 character!')
        return false
    }
    if (!name) {
        $('.verify_warning').text('Pls input Name!')
        return false
    }
    if (!email) {
        $('.verify_warning').text('Pls input Email!')
        return false
    }
    if (!deptID) {
        $('.verify_warning').text('Pls select your department belong!')
        return false
    }
    if (!gradeID) {
        $('.verify_warning').text('Pls select your grade to genarate you permition!')
        return false
    }
    $('.verify_warning').text('')
    return true
}
function isExistAcc(callback) {
    // callback return true || false
    let code = $('input[name=code]').val()
    $.ajax({
        url: "/login/isExistAcc",
        type: 'POST',
        data: { 'code': code },
        dataType: 'json',
        success: function (data) {
            callback(data)
        },
        fail: function () {
            alert('occur error!!')
        }
    })
}

