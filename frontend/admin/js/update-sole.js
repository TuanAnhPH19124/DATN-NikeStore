const id = localStorage.getItem("soleId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Sole/" + id,
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
    $('#update-sole-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id: id,
            name: $("#name").val(),
            description: $("#description").val(),
        };
        if (confirm(`Bạn có muốn sửa đế ${formData.name} không?`)) {
            if(formData.name.trim()==""){
                return
            }
            $.ajax({
                url: "https://localhost:44328/api/Sole/" + id,
                type: "PUT",
                data: JSON.stringify(formData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    window.location.href = "/frontend/admin/sole.html";
                },
            });
        } else {
            return
        }
    });
    $("#update-sole-form").validate({
        rules: {
            "name": {
                required: true,
            },
        },
        messages: {
            "name": {
                required: "Mời bạn nhập Loại Đế",
            },
        },
    });
});


