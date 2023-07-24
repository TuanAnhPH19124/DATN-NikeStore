// call api len datatable nhan vien
$(document).ready(function () {
    $('#staff-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Employee",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": 'fullName' },
            { "data": 'snn' },
            { "data": 'phoneNumber' },
            { "data": 'modifiedDate' },
            { "data": 'role' },
            { "data": 'status' },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Xóa</a></td>';
                }
            },
        ],
    });
    // call api them nhan vien
    $('#add-employee-form').submit(function (event) {
        event.preventDefault();
        var formData = {
            fullName: $("#fullName").val(),
            snn: $("#snn").val(),
            phoneNumber: $("#phoneNumber").val(),
            role: $("#role").val(),
            password: "1",
            modifiedDate: "2023-07-18T08:44:53.134Z",
            status: true,
        };
        $.ajax({
            url: "https://localhost:44328/api/Employee",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                console.log("Thêm nhân viên thành công")
            },
            error: function (xhr, status, error) {
                console.log("Fail")
                console.log(formData);
            }
        });
    });
    // custom validate 
    $.validator.addMethod("nameContainOnlyChar", function (value, element) {
        return value.match(/[^a-zA-Z]/) == null;
    });
    $.validator.addMethod("idContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("phoneNumContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
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
                require: true,
                phoneNumContainOnlyNum: true,
            },
            "role": {
                require: true,
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
                phoneNumContainOnlyNum: "Số điện thoại không được chứa ký tự"
            },
            "role": {
                required: "Bạn phải nhập tên vai trò",
            }
        },
    });
});
