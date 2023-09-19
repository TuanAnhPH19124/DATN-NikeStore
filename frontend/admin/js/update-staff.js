const id = localStorage.getItem("staffId");
console.log(id); //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
  $.ajax({
    url: "https://localhost:44328/api/Employee/Get/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(JSON.stringify(data));
      $("#fullName").val(data.fullName);
      var dateObj = new Date(data.dateOfBirth);
      var day = padZero(dateObj.getUTCDate());
      var month = padZero(dateObj.getUTCMonth() + 1);
      var year = dateObj.getUTCFullYear();
      var formattedDate = `${day}/${month}/${year}`;
      
      function padZero(value) {
        return value < 10 ? `0${value}` : value;
      }
      $("#dateOfBirth").val(formattedDate);
      $("#snn").val(data.snn);
      $("#phoneNumber").val(data.phoneNumber);
      $("#status").prop("checked", data.status);
      $("#homeTown").val(data.homeTown);
      $("#gender").val(data.gender);
      localStorage.setItem("updated-appUser", data.appUserId);

      $.ajax({
        url: "https://localhost:44328/api/AppUser/Get/" + data.appUserId,
        type: "GET",
        dataType: "json",
        success: function (data) {
          console.log(JSON.stringify(data));
          $("#email").val(data.email);
        },
        error: function () {
          console.log("Error retrieving data.");
        },
      });
    },
    error: function () {
      console.log("Error retrieving data.");
    },
  });
  $("#update-employee-form").submit(function (event) {
    event.preventDefault();
    var formData = {
      id: id,
      snn: $("#snn").val(),
      fullName: $("#fullName").val(),
      phoneNumber: $("#phoneNumber").val(),
      dateOfBirth: $("#dateOfBirth").val(),
      gender: $("#gender").val(),
      homeTown: $("#homeTown").val(),
      status: $("#status").prop("checked"),
    };
    var startComponents = formData.dateOfBirth.split("/");
    try {
      var startDate = new Date(
        `${startComponents[2]}-${startComponents[1]}-${startComponents[0]}`
      );
  
      // Adjust for local time zone offset
      startDate.setMinutes(startDate.getMinutes() - startDate.getTimezoneOffset());
  
      formData.dateOfBirth = startDate.toISOString();
      
    } catch (error) {
      formData.dateOfBirth = "";
    }

    if (confirm(`Bạn có muốn cập nhật nhân viên không?`)) {
      const date1 = new Date();
      const date2 = new Date(formData.dateOfBirth);
      console.log(date1);
      console.log(date2);
      if (date1.getFullYear() - date2.getFullYear()<=14) {
        $("#date14-error").show();
        return
      } else {
        $("#date14-error").hide();
      }
      // Compare the two dates
      if (date1 < date2) {
        $("#date-error").show();
        return;
      } else {
        $("#date-error").hide();
      }
      $.ajax({
        url: "https://localhost:44328/api/Employee/" + id,
        type: "PUT",
        data: JSON.stringify(formData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          const id = localStorage.getItem("updated-appUser");
          var formData2 = {
            status: formData.status == true ? 1 : 0,
            id: id,
          };
          $.ajax({
            url: `https://localhost:44328/api/AppUser/${id}/UpdateUserByAdmin`,
            type: "PUT",
            data: JSON.stringify(formData2),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
              window.location.href = "/frontend/admin/staff.html";
            },
          });
        },
      });
    } else {
      return;
    }
  });
  // custom validate
  $.validator.addMethod("nameContainOnlyChar", function (value, element) {
    return value.match(/^[a-zA-ZÀ-ỹ\s]+$/) != null;
  });
  $.validator.addMethod("idContainOnlyNum", function (value, element) {
    return value.match(/[^0-9]/) == null;
  });
  $.validator.addMethod("phoneNumContainOnlyNum", function (value, element) {
    return value.match(/[^0-9]/) == null;
  });
  $.validator.addMethod("onlyContain10Char", function (value, element) {
    return value.match(/^\w{10}$/) != null;
  });
  $.validator.addMethod("onlyContain12Char", function (value, element) {
    return value.match(/^\w{12}$/) != null;
  });
  // add validate
  $("#update-employee-form").validate({
    rules: {
      dateOfBirth: {
        required: true,
      },
      homeTown: {
        required: true,
      },
      fullName: {
        required: true,
        maxlength: 30,
        nameContainOnlyChar: true,
      },
      snn: {
        required: true,
        idContainOnlyNum: true,
        onlyContain12Char: true,
      },
      phoneNumber: {
        required: true,
        phoneNumContainOnlyNum: true,
        onlyContain10Char: true,
      },
      role: {
        required: true,
      },
    },
    messages: {
      dateOfBirth: {
        required: "Bạn phải nhập ngày sinh",
      },
      homeTown: {
        required: "Bạn phải nhập Quê quán",
      },
      fullName: {
        required: "Bạn phải nhập họ và tên",
        maxlength: "Hãy nhập tối đa 30 ký tự",
        nameContainOnlyChar: "Họ tên không được chứa số hay ký tự",
      },
      snn: {
        required: "Bạn phải nhập số căn cước",
        idContainOnlyNum: "Số căn cước không được chưa ký tự",
        onlyContain12Char: "Độ dài của số căn cước là 12",
      },
      phoneNumber: {
        required: "Bạn phải nhập số điện thoại",
        phoneNumContainOnlyNum: "Số điện thoại không được chứa ký tự",
        onlyContain10Char: "Số điện thoại chứa 10 ký tự",
      },
      role: {
        required: "Bạn phải nhập tên vai trò",
      },
    },
  });
});
$(function () {
  $(".date").datepicker({
    format: "dd/mm/yyyy",
  });
});
const id_user = localStorage.getItem("user-id");
$.ajax({
  url: "https://localhost:44328/api/AppUser/Get/" + id_user,
  type: "GET",
  contentType: "application/json",
  success: function (data) {
    console.log(data.fullName);
    $("#userName").text(data.fullName);
  },
  error: function () {},
});
