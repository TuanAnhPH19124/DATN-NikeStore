const id = localStorage.getItem("voucherId");
console.log(id); //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
  $.ajax({
    url: "https://localhost:44328/api/Voucher/Get/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(JSON.stringify(data));
      $("#code").val(data.code);
      $("#value").val(data.value);
      $("#description").val(data.description);
      $("#expression").val(Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(data.expression));
      var dateObj1 = new Date(data.startDate);
      var day1 = padZero(dateObj1.getUTCDate());
      var month1 = padZero(dateObj1.getUTCMonth() + 1);
      var year1 = dateObj1.getUTCFullYear();
      var formattedDate = `${day1}/${month1}/${year1}`;
      
      function padZero(value) {
        return value < 10 ? `0${value}` : value;
      }
      
      var dateObj2 = new Date(data.endDate);
      var day2 = padZero(dateObj2.getUTCDate());
      var month2 = padZero(dateObj2.getUTCMonth() + 1);
      var year2 = dateObj2.getUTCFullYear();
      var formattedDate2 = `${day2}/${month2}/${year2}`;
      
      function padZero(value) {
        return value < 10 ? `0${value}` : value;
      }
      
      $("#startDate").val(formattedDate);
      $("#endDate").val(formattedDate2);
      $("#status").prop("checked", data.status);
    },
    error: function () {
      console.log("Error retrieving data.");
    },
  });
  $("#update-voucher-form").submit(function (event) {
    event.preventDefault();
    var formData = {
      id: id,
      code: $("#code").val().trim(),
      value: Number($("#value").val()),
      expression: parseInt($("#expression").val().replace(/[^\d]/g, ''), 10),
      description: $("#description").val(),
      startDate: $("#startDate").val(),
      endDate: $("#endDate").val(),
      status: $("#status").prop("checked"),
    };

    var startComponents = formData.startDate.split("/");
    var endComponents = formData.endDate.split("/");
  
    try {
      var startDate = new Date(
        `${startComponents[2]}-${startComponents[1]}-${startComponents[0]}`
      );
      var endDate = new Date(
        `${endComponents[2]}-${endComponents[1]}-${endComponents[0]}`
      );
  
      // Adjust for local time zone offset
      startDate.setMinutes(startDate.getMinutes() - startDate.getTimezoneOffset());
      endDate.setMinutes(endDate.getMinutes() - endDate.getTimezoneOffset());
  
      formData.startDate = startDate.toISOString();
      formData.endDate = endDate.toISOString();
      
    } catch (error) {
      formData.startDate = "";
      formData.endDate = "";
    }
    if (/^\d+$/.test(formData.value) && parseInt(formData.value, 10) >= 1 && parseInt(formData.value, 10) <= 100) {
      // Validation passed
    } else {
      return
    }
    const dateObj = new Date(formData.startDate);

    const day = String(dateObj.getUTCDate()).padStart(2, "0");
    const month = String(dateObj.getUTCMonth() + 1).padStart(2, "0"); // Note: Month is zero-based
    const year = dateObj.getUTCFullYear();

    const formattedDate = `${day}-${month}-${year}`;

    console.log(formattedDate);
    const today = new Date();

    const day1 = String(today.getDate()).padStart(2, "0");
    const month1 = String(today.getMonth() + 1).padStart(2, "0"); // Note: Month is zero-based
    const year1 = today.getFullYear();

    const formattedDate1 = `${day1}-${month1}-${year1}`;

    console.log(formattedDate1);

    // Parse the date strings into Date objects
    const date1Parts = formattedDate.split("-");
    const date1 = new Date(
      parseInt(date1Parts[2], 10),
      parseInt(date1Parts[1], 10) - 1, // Subtract 1 because months are zero-based
      parseInt(date1Parts[0], 10)
    );

    const date2Parts = formattedDate1.split("-");
    const date2 = new Date(
      parseInt(date2Parts[2], 10),
      parseInt(date2Parts[1], 10) - 1, // Subtract 1 because months are zero-based
      parseInt(date2Parts[0], 10)
    );

    // Compare the two dates
    if (date1.getTime() < date2.getTime()) {
      $("#startDate-error").show();
      $("#date-error").hide();
      return
    }else {
      $("#startDate-error").hide();
    }
    if (startDate > endDate) {
      $("#date-error").show();
      $("#startDate-error").hide();
      return;
    } else {
      $("#date-error").hide();
    }
    if (confirm(`Bạn có muốn cập nhật voucher không?`)) {
      $.ajax({
        url: "https://localhost:44328/api/Voucher/" + id,
        type: "PUT",
        data: JSON.stringify(formData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
          window.location.href = "/frontend/admin/voucher.html";
        },
        error: function (xhr, status, error) {
          if (xhr.status === 500) {
            $("#fail").toast("show");
          }
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
  $.validator.addMethod("valueContainOnlyNum", function (value, element) {
    return value.match(/[^0-9]/) == null;
  });
  $.validator.addMethod(
    "value100",
    function (value, element) {
      return (
        /^\d+$/.test(value) &&
        parseInt(value, 10) >= 1 &&
        parseInt(value, 10) <= 100
      );
    },
    "Please enter a valid number between 1 and 100."
  );
  // add validate
  $("#update-voucher-form").validate({
    rules: {
      code: {
        required: true,
        maxlength: 15,
      },
      value: {
        required: true,
        value100: true,
      },
      expression: {
        required: true,
      },
      description: {
        maxlength: 20,
      },
      startDate: {
        required: true,
      },
      endDate: {
        required: true,
      },
    },
    messages: {
      code: {
        required: "Bạn phải nhập Mã giảm giá",
        maxlength: "Mã giảm giá không quá 15 ký tự",
      },
      value: {
        required: "Bạn phải nhập số căn cước",
        value100: "Giá trị là sô nằm trong khoảng từ 1-100",
      },
      expression: {
        required: "Bạn phải nhập giá trị tối thiểu",
      },
      description: {
        maxlength: "Mô tả không được quá 20 ký tự",
      },
      startDate: {
        required: "Nhập ngày bắt đầu",
      },
      endDate: {
        required: "Nhập ngày kết thúc",
      },
    },
  });
});

$(function () {
  $(".date").datepicker({
    format: "dd/mm/yyyy",
  });
});
function formatCurrency(input, type) {
  let value = input.value.replace(/[^\d]/g, ""); // Loại bỏ các ký tự không phải s

  if (type === 1) {
    let basePrice = parseInt($("#retailPrice").val().replace(/[^\d]/g, ""));
    if (parseInt(value) > basePrice) {
      value = 0;
    }
  }

  let numericValue = parseInt(value, 10);
  if (!isNaN(numericValue)) {
    const formattedValue = new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(numericValue);
    input.value = formattedValue;
  } else {
    input.value = "";
  }
}
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