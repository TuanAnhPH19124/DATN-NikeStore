$(function () {
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
    });
});
const id_user = localStorage.getItem("user-id")
$.ajax({
    url: "https://localhost:44328/api/AppUser/Get/"+id_user,
    type: "GET",
    contentType: "application/json",
    success: function (data) {
        console.log(data.fullName)
        $("#fullName").text(data.fullName)
    },
    error: function () {

    },
});

