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
});
