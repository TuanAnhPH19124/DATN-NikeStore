// call api len datatable nhan vien
$(document).ready(function () {
    var staffTable = $('#staff-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Employee",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                "data": 'employeeId', "title": "STT", render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": 'fullName', "title": "Họ và tên" },
            { "data": 'snn', "title": "Số căn cước" },
            { "data": 'phoneNumber', "title": "Số điện thoại" },
            {
                "data": 'modifiedDate', "title": "Ngày thay đổi",
                "render": function (data, type, full, meta) {
                    var dateObj = new Date(data);
                    var day = dateObj.getUTCDate();
                    var month = dateObj.getUTCMonth() + 1;
                    var year = dateObj.getUTCFullYear();
                    var formattedDate = `${day}/${month}/${year}`;
                    return formattedDate;
                }
            },
            { "data": 'role', "title": "Vai trò" },
            {
                "data": 'status', "title": "Trạng thái", "render": function (data, type, row) {
                    if (data == true) {
                        return '<span class="badge badge-pill badge-primary" style="padding:10px;">Kích hoạt</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger" style="padding:10px;">Ngừng kích hoạt</span>';
                    }
                }
            },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
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
    setInterval(function () {
        staffTable.ajax.reload();
    }, 2500);
    // call api them nhan vien
    $('#add-employee-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            fullName: $("#fullName").val(),
            snn: $("#snn").val(),
            phoneNumber: $("#phoneNumber").val(),
            role: $("#role").val(),
            password: "1",
            modifiedDate: new Date,
            status: true,
        };
        $.ajax({
            url: "https://localhost:44328/api/Employee",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                $('.toast').toast('show')
            },
        });
    });
    // custom validate 
    $.validator.addMethod("nameContainOnlyChar", function (value, element) {
        return value.match(/^[a-zA-ZÀ-ỹ\s]+$/) != null;
    });
    $.validator.addMethod("idContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("phoneNumContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("onlyContain10Char", function (value, element) {
        return value.match(/^\w{10}$/) != null;
    });
    $.validator.addMethod("onlyContain12Char", function (value, element) {
        return value.match(/^\w{12}$/) != null;
    });
    // add validate
    $("#add-employee-form").validate({
        rules: {
            "fullName": {
                required: true,
                maxlength: 30,
                nameContainOnlyChar: true,
            },
            "snn": {
                required: true,
                idContainOnlyNum: true,
                onlyContain12Char: true,
            },
            "phoneNumber": {
                required: true,
                phoneNumContainOnlyNum: true,
                onlyContain10Char: true,
            },
            "role": {
                required: true,
            }
        },
        messages: {
            "fullName": {
                required: "Bạn phải nhập họ và tên",
                maxlength: "Hãy nhập tối đa 30 ký tự",
                nameContainOnlyChar: "Họ tên không được chứa số hay ký tự",
            },
            "snn": {
                required: "Bạn phải nhập số căn cước",
                idContainOnlyNum: "Số căn cước không được chưa ký tự",
                onlyContain12Char: "Độ dài của số căn cước là 12"
            },
            "phoneNumber": {
                required: "Bạn phải nhập số điện thoại",
                phoneNumContainOnlyNum: "Số điện thoại không được chứa ký tự",
                onlyContain10Char: "Số điện thoại chứa 10 ký tự"
            },
            "role": {
                required: "Bạn phải nhập tên vai trò",
            }
        },
    });
    //add event click datatable

    $('#staff-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let staffId = $('#staff-table').DataTable().row(this).data().employeeId;
        if (staffId !== null) {
            localStorage.setItem("staffId", staffId);
            window.location.href = `/frontend/admin/update-staff.html`;
        }
    });
});


