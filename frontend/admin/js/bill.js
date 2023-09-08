$(document).ready(function () {
  var billTable = $("#bill-table").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders",
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
      { data: "customerName", title: "Họ và tên" },
      { data: "phoneNumber", title: "Số điện thoại" },
      {
        data: "dateCreated",
        title: "Ngày tạo",
        render: function (data, type, full, meta) {
          var dateObj = new Date(data);
          var day = dateObj.getUTCDate();
          var month = dateObj.getUTCMonth() + 1;
          var year = dateObj.getUTCFullYear();
          var formattedDate = `${day}/${month}/${year}`;
          return formattedDate;
        },
      },
      { data: "amount", title: "Tổng tiền",
      render: function (data, type, row) {
        return Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(data);
      }, },
      {
        data: "status",
        title: "Trạng thái",
        render: function (data, type, row) {
          return `<a href="/frontend/admin/bill-detail.html" class="btn btn-info">Xem</a>`;
        },
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
      fullName: $("#fullName").val(),
      snn: $("#snn").val(),
      phoneNumber: $("#phoneNumber").val(),
      role: $("#role").val(),
      password: "1",
      modifiedDate: new Date(),
      status: true,
    };
    $.ajax({
      url: "https://localhost:44328/api/Employee",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        window.location.href = `/frontend/admin/staff.html`;
      },
    });
  });
  //add event click datatable
});
$("#bill-table tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
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

