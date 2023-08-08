$(document).ready(function () {
    $('#login-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            email: $("#email").val(),
            password: $("#password").val(),
        };
        $.ajax({
            url: "https://localhost:44328/api/Authentication/SignIn",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                window.location.href = `/frontend/admin/index.html`;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error")
                    $('.toast').toast('show')

            }
        });
    });
    $("#login-form").validate({
        rules: {
            "email": {
                required: true,
            },
            "password": {
                required: true,
            },
        },
        messages: {
            "email": {
                required: "Mời bạn nhập email",
            },
            "password": {
                required: "Mời bạn nhập mật khẩu",
            },
        },
    });
});