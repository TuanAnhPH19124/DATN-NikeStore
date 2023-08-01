// call api len datatable nhan vien
$(document).ready(function () {
    var sizeTable = $('#size-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Size/Get",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                "data": 'id', "title": "STT",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": 'numberSize', "title": "Kích cỡ size" },
            { "data": 'description', "title": "Mô tả" },
            { "data": 'stocks', "title": "Tồn kho" },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Sửa</a></td>';
                },
                "title": "Thao tác"
            },
        ],
    });
    setInterval(function () {
        sizeTable.ajax.reload();
    }, 5000);
    // call api them nhan vien
    $('#add-size-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            numberSize: $("#numberSize").val(),
            description: ""
        };

        if (confirm(`Bạn có muốn thêm size ${formData.numberSize} không?`)) {
            $.ajax({
                url: "https://localhost:44328/api/Size",
                type: "POST",
                data: JSON.stringify(formData),
                contentType: "application/json",
                success: function (response) {
                    $('.toast').toast('show')
                },
            });
        } else {
            return
        }
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


