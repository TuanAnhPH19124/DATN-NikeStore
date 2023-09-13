$(document).ready(function () {
  $.fn.dataTableExt.sErrMode = "mute";
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
      {
        data: "customerName",
        title: "Họ và tên",
        render: function (data, type, full, meta) {
          if (full.addressId === null) {
            return data;
          } else {
            return full.user.fullName;
          }
        },
      },
      
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
  var billTable1 = $("#bill-table1").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders/GetLatestConfirmedOrders",
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
      { data: "customerName", title: "Họ và tên",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.fullName;
        }
      },},
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
  var billTable2 = $("#bill-table2").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders/GetLatestPendingShipOrders",
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
      { data: "customerName", title: "Họ và tên",
            render: function (data, type, full, meta) {
          if (full.addressId === null) {
            return data;
          } else {
            return full.user.fullName;
          }
        }, },
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
  var billTable3 = $("#bill-table3").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders/GetLatestShippingOrders",
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
      { data: "customerName", title: "Họ và tên",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.fullName;
        }
      }, },
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
  var billTable4 = $("#bill-table4").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders/GetLatestDeliveredOrders",
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
      { data: "customerName", title: "Họ và tên",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.fullName;
        }
      },},
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
  var billTable5 = $("#bill-table5").DataTable({
    ajax: {
      url: "https://localhost:44328/api/Orders/GetLatestCancelOrders",
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
      { data: "customerName", title: "Họ và tên",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.fullName;
        }
      }, },
      { data: "phoneNumber", title: "Số điện thoại",
      render: function (data, type, full, meta) {
        if (full.addressId === null) {
          return data;
        } else {
          return full.user.phoneNumber;
        }
      }, },
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
      {
        data: "amount",
        title: "Tổng tiền",
        render: function (data, type, row) {
          return Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data);
        },
      },
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
});
$("#bill-table tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
});
$("#bill-table1 tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table1").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
});
$("#bill-table2 tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table2").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
});
$("#bill-table3 tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table3").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
});
$("#bill-table4 tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table4").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
  }
});
$("#bill-table5 tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let billId = $("#bill-table5").DataTable().row(this).data().id;
  if (billId !== null) {
    localStorage.setItem("billId", billId);
    window.location.href = `/frontend/admin/order.html`;
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
