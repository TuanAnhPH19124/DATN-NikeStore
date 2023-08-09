// call api len datatable nhan vien
$(document).ready(function () {
    var customerTable = $('#customer-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/AppUser/Get",
            "dataType": "json",
            "dataSrc": "",
        },
        "columns": [
            {
                "data": 'id', "title": "STT",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": 'avatarUrl', "title": "Ảnh", },
            { "data": 'userName', "title": "Tên tài khoản", },
            { "data": 'email', "title": "Email", },
            { "data": 'fullName', "title": "Họ và tên", },
            { "data": 'modifiedDate', "title": "Ngày tạo", },
            { "data": 'loyaltyPoint', "title": "Điểm", },
            { "data": 'rank', "title": "Hạng", },
            {
                "data": 'status', "title": "Trạng thái",
                "render": function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge badge-pill badge-primary">Kích hoạt</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger">Đã hủy</span>';
                    }
                }
            },
            {
                "data": 'status',
                "render": function (data) {
                    if (data == 1) {
                        return '<td><button class="btn btn-danger" id="btn" ><i class="fa fa-times" aria-hidden="true"></i></button></td>';
                    } else {
                        return '<td><button class="btn btn-danger" id="btn" style="padding-right: 8px;padding-left: 8px;"><i class="fa fa-retweet" aria-hidden="true"></i></button></td>';
                    }
                },
                "title": "Thao tác"
            },
        ],
    });
    setInterval(function () {
        customerTable.ajax.reload();
    }, 2500);
    //add event click datatable

    $('#customer-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let customerId = $('#customer-table').DataTable().row(this).data().id;
        if (customerId !== null) {
            $.ajax({
                url: `https://localhost:44328/api/AppUser/Get/${customerId}`,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.status == 1) {
                        var changedStatus = 0
                    } else {
                        var changedStatus = 1
                    }

                    var formData = {
                        status: changedStatus,
                        id: customerId
                    };
                    if (confirm(`Bạn có muốn thay đổi trạng thái của tài khoản ${data.userName} không?`)) {
                        $.ajax({
                            url: `https://localhost:44328/api/AppUser/${customerId}/UpdateUserByAdmin`,
                            type: "PUT",
                            data: JSON.stringify(formData),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                $('.toast').toast('show')
                            },
                        });
                    } else {
                        return
                    }

                },
            });


        }
    });
});


