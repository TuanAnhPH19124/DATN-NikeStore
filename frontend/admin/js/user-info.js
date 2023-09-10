const id = localStorage.getItem("user-id");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/AppUser/Get/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $("#email").val(data.email)
            $("#fullName").val(data.fullName)
            $("#phoneNumber").val(data.phoneNumber)
            $("#status").val(data.status)

            $("#change-pass").on("click", function (event) {
                event.preventDefault(); // Prevent form submission
                validateForm()
                if($("#currentPassword").val()==""||$("#newPassword").val()==""||$("#newPassword2").val()==""){
                    return
                }
                if($("#newPassword").val()!=$("#newPassword2").val()){
                    return
                }
               if (confirm("Bạn có muốn đổi mật khẩu không?")) {
                   var formData = {
                       "userName": data.userName,
                       "currentPassword": $("#currentPassword").val(),
                       "newPassword": $("#newPassword").val(),
                   };
                   $.ajax({
                       url: "https://localhost:44328/api/Authentication/ChangePassword",
                       type: "POST",
                       data: JSON.stringify(formData),
                       contentType: "application/json",
                       success: function (response) {
                        $(".toast")
                        .find(".toast-body")
                        .text("Đổi mật khẩu thành công");
                      $(".toast").toast("show");
                      $('#passwordModal').modal('hide');
                        $("#currentPassword").val("")
                        $("#newPassword").val("")
                        $("#newPassword2").val("")
                       },
                       error: function (xhr,error,jqXHR) {
                        console.log(xhr.responseJSON.error)
                        $(".toast")
                        .find(".toast-body")
                        .text(xhr.responseJSON.error);
                      $(".toast").toast("show");
                       },
                   });
               } else {
                   return;
               }
           });
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
});
const id_user = localStorage.getItem("user-id")
$.ajax({
    url: "https://localhost:44328/api/AppUser/Get/"+id_user,
    type: "GET",
    contentType: "application/json",
    success: function (data) {
        console.log(data.fullName)
        $("#userName").text(data.fullName)
    },
    error: function () {

    },
});

function validateForm() {
    var currentPassword = document.getElementById("currentPassword").value;
    var newPassword = document.getElementById("newPassword").value;
    var newPassword2 = document.getElementById("newPassword2").value;

    var validatePass = document.getElementById("validatePass");
    var validateNewPass = document.getElementById("validateNewPass");
    var validateNewPass2 = document.getElementById("validateNewPass2");

    if (currentPassword.length == 0) {
        validatePass.style.display = "block";
      } else {
        validatePass.style.display = "none";
      }
      if (newPassword.length == 0) {
        validateNewPass.style.display = "block";
      } else {
        validateNewPass.style.display = "none";
      }
      if (newPassword2 != newPassword) {
        validateNewPass2.style.display = "block";
      } else {
        validateNewPass2.style.display = "none";
      }
}
