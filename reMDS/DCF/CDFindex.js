
function showDetail(id) {
    $.ajax({
        url: "/DCFs/getCDF",
        type: 'POST',
        data: { 'id': id, },
        dataType: 'json',
        success: function (data) {
            if (1) {
                renderModalShowDetail(data.data)
            } else {
                alert('You do not have permition!')
            }
        },
        fail: function () {
            alert('occur error!!')
        }
    })
}


function renderModalShowDetail(data) {
    $('#modal_dcf_showdetail').modal()
}

