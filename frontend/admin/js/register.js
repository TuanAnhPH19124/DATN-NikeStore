$(document).ready(function () {
    $('#register-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            email: $("#email").val(),
            password: $("#password").val(),
            phoneNumber: $("#phoneNumber").val(),
            userName: $("#userName").val(),
        };
        $.ajax({
            url: "https://localhost:44328/api/Authentication/SignUp",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                window.location.href = `/frontend/admin/index.html`;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("Fail")

            }
        });
    });
    $.validator.addMethod("validEmail", function (value, element) {
        return value.match(/^\w+([.-]?\w+)*@\w+([.-]?\w+)*(\.\w{2,3})+$/) != null;
    });
    $.validator.addMethod("validatePassword", function (value, element) {
        return value.match(/^(?=.*[a-z])(?=.*[0-9])(?=.*[@*]).{8,}$/) != null;
    });
    $.validator.addMethod("validatePhoneNum", function (value, element) {
        return value.match(/^\d{10}$/) != null;
    });
    $("#register-form").validate({
        rules: {
            "email": {
                required: true,
                validEmail: true,
            },
            "password": {
                required: true,
                minlength: 8,
                maxlength: 12,
                validatePassword: true,
            },
            "phoneNumber": {
                required: true,
                validatePhoneNum: true,
            },
            "userName": {
                required: true,
            },
        },
        messages: {
            "email": {
                required: "Mời bạn nhập email",
                validEmail: "Định dạng email không hợp lệ"
            },
            "password": {
                required: "Mời bạn nhập mật khẩu",
                minlength: "Mật khẩu dài ít nhất 8 ký tự đến 12 ký tự",
                validatePassword: "Mật khẩu chứa ít nhất một chữ cái thường, hoa, một số và một kí tự đặc biệt",
                maxlength: "Mật khẩu dài ít nhất 8 ký tự đến 12 ký tự",
            },
            "phoneNumber": {
                required: "Mời bạn nhập số điện thoại",
                validatePhoneNum: "Số điện thoại phải đủ 10 số, không chứa ký tự"
            },
            "userName": {
                required: "Mời bạn nhập tên tài khoản",
            },
        },
    });
});