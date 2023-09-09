$(document).ready(function () {
    $("#reset-password").on("click",function(){
        var formData = {
            email: $("#email").val(),
        };
        $.ajax({
            url: "https://localhost:44328/api/Authentication/forgot-password",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                $(".toast")
                .find(".toast-body")
                .text("Mời bạn kiểm tra email, mật khẩu mới đã được gửi cho bạn");
              $(".toast").toast("show");
              var form = $(".user")[0];
              form.reset();
            },
            error: function (jqXHR, textStatus, errorThrown) {

            }
        });
    })
});