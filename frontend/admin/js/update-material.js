const id = localStorage.getItem("materialId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Material/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#name').val(data.name);
            $('#description').val(data.description);
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-material-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id: id,
            name: $("#name").val(),
            description: $("#description").val(),
        };
        if (confirm(`Bạn có muốn sửa chất liệu ${formData.name} không?`)) {
            $.ajax({
                url: "https://localhost:44328/api/Material/" + id,
                type: "PUT",
                data: JSON.stringify(formData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    window.location.href = "/frontend/admin/material.html";
                },
            });
        } else {
            return
        }
    });
    $("#update-material-form").validate({
        rules: {
            "name": {
                required: true,
            },
        },
        messages: {
            "name": {
                required: "Mời bạn nhập Chất liệu",
            },
        },
    });
});


