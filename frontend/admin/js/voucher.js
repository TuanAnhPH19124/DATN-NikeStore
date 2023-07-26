// call api len datatable nhan vien
$(document).ready(function () {
    $('#voucher-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Voucher",
            "dataType": "json",
            "dataSrc": "",
        },
        "columns": [
            { "data": 'id', "title": "ID", "visible": false, },
            { "data": 'code', "title": "Mã" },
            { "data": 'value', "title": "Giá trị" },
            { "data": 'startDate', "title": "Ngày bắt đầu" },
            { "data": 'endDate', "title": "Ngày kết thúc" },
            { "data": 'createdDate', "title": "Ngày tạo" },
            {
                "data": 'status', "title": "Trạng thái", "render": function (data, type, row) {
                    if (data == true) {
                        return '<span class="badge badge-pill badge-primary">Kích hoạt</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger">Đã hủy</span>';
                    }
                }
            },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Sửa</a></td>';
                },
                "title": "Thao tác"
            },
        ],
    });
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

    $('#voucher-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let id = $('#voucher-table').DataTable().row(this).data().employeeId;
        if (id !== null) {
            localStorage.setItem("id", id);
            window.location.href = `/frontend/admin/update-staff.html`;
        }
    });

});
$(function () {
    $('#datepicker').datepicker({
        format: 'dd/mm/yyyy',
    });
});

