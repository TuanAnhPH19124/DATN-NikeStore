// call api len datatable nhan vien
$(document).ready(function () {
    var customerTable = $('#customer-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/AppUser/GetUsersWithUserRole",
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
            { "data": 'email', "title": "Email", },
            { "data": 'phoneNumber', "title": "SĐT", },
            { "data": 'fullName', "title": "Họ và tên", },
            { "data": 'modifiedDate', "title": "Ngày tạo",
            "render": function (data, type, full, meta) {
                var dateObj = new Date(data);
                var day = dateObj.getUTCDate();
                var month = dateObj.getUTCMonth() + 1;
                var year = dateObj.getUTCFullYear();
                var formattedDate = `${day}/${month}/${year}`;
                return formattedDate;
            } },
            {
                "data": 'status', "title": "Trạng thái",
                "render": function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge badge-pill badge-primary" style="padding:10px;background-color: #1967d2;border-color: #1967d2;">Kích hoạt</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger" style="padding:10px;">Đã hủy</span>';
                    }
                }
            },
            {
                "data": 'status',
                "render": function (data) {
                    if (data == 1) {
                        return '<td><button class="btn btn-danger" id="btn" ><i class="fa fa-times" aria-hidden="true"></i></button></td>';
                    } else {
                        return '<td><button class="btn btn-primary" id="btn" style="padding-right: 8px;padding-left: 8px;"><i class="fa fa-retweet" aria-hidden="true"></i></button></td>';
                    }
                },
                "title": "Thao tác"
            },
        ],
        rowCallback: function(row, data) {
            $(row).find('td').css('vertical-align', 'middle');
          },
          "language": {
            "sInfo": "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
            "lengthMenu": "Hiển thị _MENU_ bản ghi",
            "sSearch": "Tìm kiếm:",
            "sInfoFiltered": "(lọc từ _MAX_ bản ghi)",
            "sInfoEmpty": "Hiển thị 0 đến 0 trong 0 bản ghi",
            "sZeroRecords": "Không có data cần tìm",
            "sEmptyTable": "Không có data trong bảng",
            "oPaginate": {
                "sFirst": "Đầu",
                "sLast": "Cuối",
                "sNext": "Tiếp",
                "sPrevious": "Trước"
            },
          }
    });
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
                    
                },
            });


        }
    });
});

const id_user = localStorage.getItem("user-id")
$.ajax({
    url: "https://localhost:44328/api/AppUser/Get/"+id_user,
    type: "GET",
    contentType: "application/json",
    success: function (data) {
        console.log(data.fullName)
        $("#fullName").text(data.fullName)
    },
    error: function () {

    },
});


