// call api len datatable nhan vien
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
$(document).ready(function () {
  $.fn.dataTableExt.sErrMode = "mute";
  var voucherTable = $("#voucher-table").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Voucher/Get",
      dataType: "json",
      dataSrc: "",
    },
    columns: [
      {
        data: "id",
        title: "STT",
        render: function (data, type, row, meta) {
          return meta.row + 1;
        },
      },
      { data: "code", title: "Mã" },
      {
        data: "value",
        title: "Giá trị",
        render: function (data, type, full, meta) {
          return data + " %";
        },
      },
      {
        data: "expression",
        title: "Đơn hàng tối thiểu",
        render: function (data, type, full, meta) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
      {
        data: null,
        title: "Thời gian",
        render: function (data, type, full, meta) {
          var dateObj1 = new Date(full.startDate);
          var day1 = dateObj1.getUTCDate();
          var month1 = dateObj1.getUTCMonth() + 1;
          var year1 = dateObj1.getUTCFullYear();
          var formattedDate = `${day1}/${month1}/${year1}`;

          var dateObj2 = new Date(full.endDate);
          var day2 = dateObj2.getUTCDate();
          var month2 = dateObj2.getUTCMonth() + 1;
          var year2 = dateObj2.getUTCFullYear();
          var formattedDate2 = `${day2}/${month2}/${year2}`;
          if (full.status === true) {
            return `<span class="badge badge-pill badge-success" style="padding:10px;">${
              formattedDate + "-" + formattedDate2
            }</span>`;
          } else {
            return `<span class="badge badge-pill badge-danger" style="padding:10px;">${
              formattedDate + "-" + formattedDate2
            }</span>`;
          }
        },
      },
      // {
      //   data: "createdDate",
      //   title: "Ngày tạo",
      //   render: function (data, type, full, meta) {
      //     var dateObj = new Date(data);
      //     var day = dateObj.getUTCDate();
      //     var month = dateObj.getUTCMonth() + 1;
      //     var year = dateObj.getUTCFullYear();
      //     var formattedDate = `${day}/${month}/${year}`;
      //     return formattedDate;
      //   },
      // },
      {
        data: "status",
        title: "Trạng thái",
        render: function (data, type, row) {
          if (data == true) {
            return '<span class="badge badge-pill badge-primary" style="padding:10px;background-color: #1967d2;border-color: #1967d2;">Kích hoạt</span>';
          } else {
            return '<span class="badge badge-pill badge-danger" style="padding:10px;">Không kích hoạt</span>';
          }
        },
      },
      {
        render: function () {
          return '<td><a class="btn btn-primary" style="background-color: #1967d2;border-color: #1967d2;" id="btn"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
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
  $("#add-voucher-form").submit(function (event) {
    event.preventDefault();
    var formData = {
      code: $("#code").val().trim(),
      value: $("#value").val(),
      expression: parseInt($("#expression").val().replace(/[^\d]/g, ""), 10),
      description: $("#description").val(),
      startDate: $("#startDate").val(),
      endDate: $("#endDate").val(),
      status: true,
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
      startDate.setMinutes(
        startDate.getMinutes() - startDate.getTimezoneOffset()
      );
      endDate.setMinutes(endDate.getMinutes() - endDate.getTimezoneOffset());

      formData.startDate = startDate.toISOString();
      formData.endDate = endDate.toISOString();
    } catch (error) {
      formData.startDate = "";
      formData.endDate = "";
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
    if (
      /^\d+$/.test(formData.value) &&
      parseInt(formData.value, 10) >= 1 &&
      parseInt(formData.value, 10) <= 100
    ) {
      // Validation passed
    } else {
      return;
    }

    if (confirm(`Bạn có muốn thêm voucher ${formData.code} không?`)) {
      $.ajax({
        url: "https://localhost:44328/api/Voucher",
        type: "POST",
        data: JSON.stringify(formData),
        contentType: "application/json",
        success: function (response) {
          console.log(response);

          $("#modal-add-voucher").modal("hide");
          $("#success").toast("show");
          $("#add-voucher-form")[0].reset();
          voucherTable.ajax.reload();
        },
        error: function (xhr, status, error) {
          if (
            xhr.responseText ===
            "Code Voucher with the same Name already exists"
          ) {
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
  $.validator.addMethod("value100", function (value, element) {
    return (
      /^\d+$/.test(value) &&
      parseInt(value, 10) >= 1 &&
      parseInt(value, 10) <= 100
    );
  });
  // add validate
  $("#add-voucher-form").validate({
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
        required: "Bạn phải nhập giá trị giảm giá",
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

$("#voucher-table tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let voucherId = $("#voucher-table").DataTable().row(this).data().id;
  if (voucherId !== null) {
    localStorage.setItem("voucherId", voucherId);
    window.location.href = `/frontend/admin/update-voucher.html`;
  }
});
const id_user = localStorage.getItem("user-id");
$.ajax({
  url: "https://localhost:44328/api/AppUser/Get/" + id_user,
  type: "GET",
  contentType: "application/json",
  success: function (data) {
    console.log(data.fullName);
    $("#fullName").text(data.fullName);
  },
  error: function () {},
});
