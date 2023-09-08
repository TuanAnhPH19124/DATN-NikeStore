$(document).ready(function () {
    $('#login-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            account: $("#userName").val(),
            password: $("#password").val(),
        };
        $.ajax({
            url: "https://localhost:44328/api/Authentication/SignIn",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                const token = response.token;
                const parts = token.split('.');
                const header = JSON.parse(window.atob(parts[0]));
                const payload = JSON.parse(window.atob(parts[1]));
                const signature = parts[2];
                console.log(response);
                console.log(header);
                console.log(payload);
                console.log(signature);
                if(payload.role=="Admin"){
                    localStorage.setItem("user-id", payload.Id);
                    const id = localStorage.getItem("user-id");
                    console.log(id)
                    //window.location.href = `/frontend/admin/index.html`;
                }else{
                    $('.toast').toast('show')
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (textStatus == "error")
                    $('.toast').toast('show')

            }
        });
    });
    $("#login-form").validate({
        rules: {
            "userName": {
                required: true,
            },
            "password": {
                required: true,
            },
        },
        messages: {
            "userName": {
                required: "Mời bạn nhập tài khoản",
            },
            "password": {
                required: "Mời bạn nhập mật khẩu",
            },
        },
    });
});