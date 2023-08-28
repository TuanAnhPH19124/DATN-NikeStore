const id = localStorage.getItem("voucherId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Voucher/Get/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#code').val(data.code);
            $('#value').val(data.value);
            $('#description').val(data.description);
            var dateObj1 = new Date(data.startDate);
            var day1 = dateObj1.getUTCDate();
            var month1 = dateObj1.getUTCMonth() + 1;
            var year1 = dateObj1.getUTCFullYear();
            var formattedDate = `${day1}/${month1}/${year1}`;

            var dateObj2 = new Date(data.endDate);
            var day2 = dateObj2.getUTCDate();
            var month2 = dateObj2.getUTCMonth() + 1;
            var year2 = dateObj2.getUTCFullYear();
            var formattedDate2 = `${day2}/${month2}/${year2}`;
            $('#startDate').val(formattedDate);
            $('#endDate').val(formattedDate2);
            $('#status').prop('checked', data.status);
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-voucher-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id : id,
            code: $("#code").val(),
            value: Number($("#value").val()),
            description: $("#description").val(),
            startDate: $("#startDate").val(),
            endDate: $("#endDate").val(),
            status: $("#status").prop('checked'),
            
          };
                  //convert nomal date to ISO 8601 date
        [startDay, startMonth, startYear] = formData.startDate.split('/');
        [endDay, endMonth, endYear] = formData.endDate.split('/');
        try {
            formData.startDate = new Date(`${startYear}-${startMonth}-${startDay}`).toISOString();
            formData.endDate = new Date(`${endYear}-${endMonth}-${endDay}`).toISOString();
        } catch (error) {
            formData.startDate = ""
            formData.endDate = ""
        }
        if (confirm(`Bạn có muốn cập nhật voucher không?`)) {
            $.ajax({
                url: "https://localhost:44328/api/Voucher/" + id,
                type: "PUT",
                data: JSON.stringify(formData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    window.location.href = "/frontend/admin/update-voucher.html";
                },
            });
        } else {
            return
        }
    });
      // custom validate
  $.validator.addMethod("nameContainOnlyChar", function (value, element) {
    return value.match(/^[a-zA-ZÀ-ỹ\s]+$/) != null;
  });
  $.validator.addMethod("valueContainOnlyNum", function (value, element) {
    return value.match(/[^0-9]/) == null;
  });
  $.validator.addMethod("value100", function (value, element) {
    return /^\d+$/.test(value) && parseInt(value, 10) >= 1 && parseInt(value, 10) <= 100;
  }, "Please enter a valid number between 1 and 100.");
  // add validate
  $("#update-voucher-form").validate({
    rules: {
      code: {
        required: true,
        maxlength: 15,
      },
      value: {
        required: true,
        value100: true
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
        value100: "Giá trị là sô nằm trong khoảng từ 1-100"
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
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
    });
});