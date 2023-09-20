// call api len datatable nhan vien
$(document).ready(function () {
  $.fn.dataTableExt.sErrMode = "mute";
  var staffTable = $("#staff-table").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Employee/Get",
      dataType: "json",
      dataSrc: "",
    },
    columns: [
      {
        data: "employeeId",
        title: "STT",
        render: function (data, type, row, meta) {
          return meta.row + 1;
        },
      },
      { data: "phoneNumber", title: "Số điện thoại" },
      { data: "fullName", title: "Họ và tên" },
      {
        data: "gender",
        title: "Giới tính",
        render: function (data, type, row) {
          if (data == true) {
            return '<span class="badge badge-pill badge-primary" style="padding:10px;">Nam</span>';
          } else {
            return '<span class="badge badge-pill badge-danger" style="padding:10px;">Nữ</span>';
          }
        },
      },
      {
        data: "dateOfBirth",
        title: "Ngày sinh",
        render: function (data, type, full, meta) {
          var dateObj = new Date(data);
          var day = dateObj.getUTCDate();
          var month = dateObj.getUTCMonth() + 1;
          var year = dateObj.getUTCFullYear();
          var formattedDate = `${day}/${month}/${year}`;
          return formattedDate;
        },
      },
      {
        data: "status",
        title: "Trạng thái",
        render: function (data, type, row) {
          if (data == true) {
            return '<span class="badge badge-pill badge-primary" style="padding:10px;">Kích hoạt</span>';
          } else {
            return '<span class="badge badge-pill badge-danger" style="padding:10px;">Ngừng kích hoạt</span>';
          }
        },
      },
      {
        render: function () {
          return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
        },
        title: "Thao tác",
      },
    ],
    rowCallback: function (row, data) {
      $(row).find("td").css("vertical-align", "middle");
    },
    language: {
      sInfo: "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
      lengthMenu: "Hiển thị _MENU_ bản ghi",
      sSearch: "Tìm kiếm:",
      sInfoFiltered: "(lọc từ _MAX_ bản ghi)",
      sInfoEmpty: "Hiển thị 0 đến 0 trong 0 bản ghi",
      sZeroRecords: "Không có data cần tìm",
      sEmptyTable: "Không có data trong bảng",
      oPaginate: {
        sFirst: "Đầu",
        sLast: "Cuối",
        sNext: "Tiếp",
        sPrevious: "Trước",
      },
    },
  });

  // call api them nhan vien
  $("#add-employee-form").submit(function (event) {
    event.preventDefault();
    var formData = {
      email: $("#email").val(),
      snn: $("#snn").val(),
      fullName: $("#fullName").val(),
      phoneNumber: $("#phoneNumber").val(),
      dateOfBirth: $("#dateOfBirth").val(),
      gender: $("#gender").val(),
      homeTown: $("#homeTown").val(),
      status: true,
    };

    //convert nomal date to ISO 8601 date
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
    // add thong tin

    if (confirm(`Bạn có muốn thêm nhân viên ${formData.fullName} không?`)) {
      //add tk nhan vien

      const date1 = new Date();
      const date2 = new Date(formData.dateOfBirth);
      console.log(date1)
      console.log(date2)

      // Compare the two dates
      if (date1.getFullYear() - date2.getFullYear()<=14) {
        $("#date14-error").show();
        return
      } else {
        $("#date14-error").hide();
      }
      if (date1 < date2) {
        $("#date-error").show();
        return
      } else {
        $("#date-error").hide();
      }
      if (!isValidEmail(formData.email)) {
        return;
      }
      $.ajax({
        url: `https://localhost:44328/api/Authentication/CreateEmployeeAccount?email=${formData.email}&fullName=${formData.fullName}`,
        type: "POST",
        data: JSON.stringify(formData.phoneNumber),
        contentType: "application/json",
        success: function (response) {
          console.log(response.user.id);
          formData.appUserId = response.user.id;
          $.ajax({
            url: "https://localhost:44328/api/Employee",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
              $("#add-employee-modal").modal("hide");
              $("#success").toast("show");
              $("#add-employee-form")[0].reset();
              staffTable.ajax.reload();
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
  $.validator.addMethod("validateEmail", function (value, element) {
    // Regular expression for a valid email address
    var emailRegex = /^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
    return emailRegex.test(value);
  });
  function isValidEmail(value) {
    // Regular expression for a valid email address
    var emailRegex = /^[A-Za-z0-9._%-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
    return emailRegex.test(value);
  }
  // add validate
  $("#add-employee-form").validate({
    rules: {
      email: {
        required: true,
        validateEmail: true,
      },
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
      email: {
        required: "Bạn phải nhập email",
        validateEmail: "Email không hợp lệ",
      },
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
  //add event click datatable

  $("#staff-table tbody").on("click", "tr", function (e) {
    e.preventDefault();
    let staffId = $("#staff-table").DataTable().row(this).data().id;
    if (staffId !== null) {
      localStorage.setItem("staffId", staffId);
      console.log(staffId);
      window.location.href = `/frontend/admin/update-staff.html`;
    }
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
