const id = localStorage.getItem("id");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Employee/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#fullName').val(data.fullName);
            $('#snn').val(data.snn);
            $('#phoneNumber').val(data.phoneNumber);
            $('#password').val(data.password);
            $('#status').prop('checked', data.status);
            $('#role').val(data.role);
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-employee-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            employeeId: id,
            fullName: $("#fullName").val(),
            snn: $("#snn").val(),
            phoneNumber: $("#phoneNumber").val(),
            role: $("#role").val(),
            password: $("#password").val(),
            modifiedDate: new Date,
            status: $("#status").prop('checked'),
        };
        $.ajax({
            url: "https://localhost:44328/api/Employee/" + id,
            type: "PUT",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $('.toast').toast('show')
            },
        });
    });
});


