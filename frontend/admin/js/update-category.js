const id = localStorage.getItem("categoryId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Categories/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#name').val(data.name);
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-category-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id: id,
            name: $("#name").val(),
        };
        $.ajax({
            url: "https://localhost:44328/api/Categories/" + id,
            type: "PUT",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                window.location.href = "/frontend/admin/category.html";
            },
        });
    });
    $("#update-category-form").validate({
        rules: {
            "name": {
                required: true,
            },
        },
        messages: {
            "name": {
                required: "Mời bạn nhập Tên danh mục",
            },
        },
    });
});


