$("#modal-add-camera").on("shown.bs.modal", function () {
  // Khởi tạo quét mã vạch ở đây
  Quagga.init(
    {
      inputStream: {
        name: "Live",
        type: "LiveStream",
        target: document.querySelector("#scanner"), // Chọn phần tử để hiển thị camera
      },
      decoder: {
        readers: ["code_128_reader"], // Chọn loại mã vạch cần quét (ở đây là Code 128)
      },
    },
    function (err) {
      if (err) {
        console.error(err);
        return;
      }
      Quagga.start();
    }
  );
});

$("#modal-add-camera").on("hidden.bs.modal", function () {
  // Dừng quét mã vạch khi đóng modal
  Quagga.stop();
});
function myFunction() {
  var x = document.getElementById("customer-info");
  console.log(x.style.visibility);
  if (x.style.visibility == "hidden") {
    x.style.visibility = "visible";
  } else {
    x.style.visibility = "hidden";
  }
}
function CallGiaoHang() {
  // call tỉnh
  var option_province = [];
  var startedProvince = 269;
  var startedDistrict = 2264;
  var startedWard = 90816;
  $.ajax({
    url: "https://online-gateway.ghn.vn/shiip/public-api/master-data/province",
    type: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
    dataType: "json",
    success: function (result) {
      console.log(result.data);
      for (var i = 0; i < result.data.length; i++) {
        option_province.push(
          '<option value="',
          result.data[i].ProvinceID, // Use the correct property name for ProvinceID
          '">',
          result.data[i].ProvinceName, // Use the correct property name for ProvinceName
          "</option>"
        );
      }
      $("#province").html(option_province.join(""));
    },
    error: function (error) {
      console.error("Error fetching provinces:", error);
    },
  });
  var option_district = [];
  $.ajax({
    url: `https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${startedProvince}`,
    type: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
    dataType: "json",
    success: function (result) {
      console.log(result.data[0]);
      for (var i = 0; i < result.data.length; i++) {
        option_district.push(
          '<option value="',
          result.data[i].DistrictID, // Use the correct property name for ProvinceID
          '">',
          result.data[i].DistrictName, // Use the correct property name for ProvinceName
          "</option>"
        );
      }
      $("#district").html(option_district.join(""));
    },
    error: function (error) {
      console.error("Error fetching provinces:", error);
    },
  });
  var option_ward = [];
  $.ajax({
    url: `https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${startedDistrict}`,
    type: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
    dataType: "json",
    success: function (result) {
      console.log(result.data[0]);
      for (var i = 0; i < result.data.length; i++) {
        option_ward.push(
          '<option value="',
          result.data[i].WardCode, // Use the correct property name for ProvinceID
          '">',
          result.data[i].WardName, // Use the correct property name for ProvinceName
          "</option>"
        );
      }
      $("#ward").html(option_ward.join(""));
    },
    error: function (error) {
      console.error("Error fetching provinces:", error);
    },
  });
  $("#province").on("change", function () {
    var selectedValue = $(this).val();
    console.log(selectedValue);
    var option_district = [];
    $.ajax({
      url: `https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${selectedValue}`,
      type: "GET",
      headers: {
        token: "d73043b1-2777-11ee-b394-8ac29577e80e",
      },
      dataType: "json",
      success: function (result) {
        console.log(result.data[0]);
        for (var i = 0; i < result.data.length; i++) {
          option_district.push(
            '<option value="',
            result.data[i].DistrictID, // Use the correct property name for ProvinceID
            '">',
            result.data[i].DistrictName, // Use the correct property name for ProvinceName
            "</option>"
          );
        }
        $("#district").html(option_district.join(""));
        var option_ward = [];
        $.ajax({
          url: `https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${result.data[0].DistrictID}`,
          type: "GET",
          headers: {
            token: "d73043b1-2777-11ee-b394-8ac29577e80e",
          },
          dataType: "json",
          success: function (result) {
            console.log(result.data);
            for (var i = 0; i < result.data.length; i++) {
              option_ward.push(
                '<option value="',
                result.data[i].WardCode, // Use the correct property name for ProvinceID
                '">',
                result.data[i].WardName, // Use the correct property name for ProvinceName
                "</option>"
              );
            }
            $("#ward").html(option_ward.join(""));
          },
          error: function (error) {
            console.error("Error fetching provinces:", error);
          },
        });
      },
      error: function (error) {
        console.error("Error fetching provinces:", error);
      },
    });

    fetchAllMoneyShip($("#district").val(), $("#ward").val()).then((data) => {
      if ($("#delivery").prop("checked")) {
        $("#ship-fee").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data.data.total)
        );
      } else {
        $("#ship-fee").text(0);
      }
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var shippingFee = parseFloat(
        $("#ship-fee")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var discountFee = parseFloat(
        $("#discount-price")
          .text()
          .replace(/[^0-9]/g, "")
      );
      $("#sum").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(parseFloat(productFee - discountFee + shippingFee))
      );
    });
    fetchAllDayShip($("#district").val(), $("#ward").val()).then((data) => {
      if ($("#delivery").prop("checked")) {
        console.log(data.data.leadtime);
        // Create a new Date object and pass the timestamp as milliseconds
        var date = new Date(data.data.leadtime * 1000);

        // Extract the components of the date
        var year = date.getFullYear();
        var month = ("0" + (date.getMonth() + 1)).slice(-2); // Correctly format the month
        var day = ("0" + date.getDate()).slice(-2); // Correctly format the day

        // Create a formatted date string in the desired format (day/month/year)
        var formattedDate = day + "/" + month + "/" + year;
        $("#delivery-date").text(formattedDate);
        console.log(formattedDate);
      }
    });
  });
  $("#district").on("change", function () {
    var selectedValue = $(this).val();
    console.log(selectedValue);

    var option_ward = [];

    // Create a promise for the AJAX request
    var ajaxPromise = new Promise(function (resolve, reject) {
      $.ajax({
        url: `https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${selectedValue}`,
        type: "GET",
        headers: {
          token: "d73043b1-2777-11ee-b394-8ac29577e80e",
        },
        dataType: "json",
        success: function (result) {
          console.log(result.data);
          for (var i = 0; i < result.data.length; i++) {
            option_ward.push(
              '<option value="',
              result.data[i].WardCode, // Use the correct property name for ProvinceID
              '">',
              result.data[i].WardName, // Use the correct property name for ProvinceName
              "</option>"
            );
          }
          $("#ward").html(option_ward.join(""));
          resolve(); // Resolve the promise when the AJAX request is successful
        },
        error: function (error) {
          console.error("Error fetching provinces:", error);
          reject(error); // Reject the promise if there is an error
        },
      });
    });

    // Wait for the AJAX request to complete, then execute the following code
    ajaxPromise.then(function () {
      fetchAllMoneyShip($("#district").val(), $("#ward").val()).then((data) => {
        if ($("#delivery").prop("checked")) {
          $("#ship-fee").text(
            Intl.NumberFormat("vi-VN", {
              style: "currency",
              currency: "VND",
            }).format(data.data.total)
          );
        } else {
          $("#ship-fee").text(0);
        }
        var productFee = parseFloat(
          $("#total")
            .text()
            .replace(/[^0-9]/g, "")
        );
        var shippingFee = parseFloat(
          $("#ship-fee")
            .text()
            .replace(/[^0-9]/g, "")
        );
        var discountFee = parseFloat(
          $("#discount-price")
            .text()
            .replace(/[^0-9]/g, "")
        );
        $("#sum").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(parseFloat(productFee - discountFee + shippingFee))
        );
      });
      fetchAllDayShip($("#district").val(), $("#ward").val()).then((data) => {
        if ($("#delivery").prop("checked")) {
          console.log(data.data.leadtime);
          // Create a new Date object and pass the timestamp as milliseconds
          var date = new Date(data.data.leadtime * 1000);

          // Extract the components of the date
          var year = date.getFullYear();
          var month = ("0" + (date.getMonth() + 1)).slice(-2); // Correctly format the month
          var day = ("0" + date.getDate()).slice(-2); // Correctly format the day

          // Create a formatted date string in the desired format (day/month/year)
          var formattedDate = day + "/" + month + "/" + year;
          $("#delivery-date").text(formattedDate);
          console.log(formattedDate);
        }
      });
    });
  });

  $("#ward").on("change", function () {
    var selectedValue = $(this).val();
    console.log(selectedValue);
    fetchAllMoneyShip($("#district").val(), $("#ward").val()).then((data) => {
      if ($("#delivery").prop("checked")) {
        $("#ship-fee").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data.data.total)
        );
      } else {
        $("#ship-fee").text(0);
      }
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var shippingFee = parseFloat(
        $("#ship-fee")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var discountFee = parseFloat(
        $("#discount-price")
          .text()
          .replace(/[^0-9]/g, "")
      );
      $("#sum").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(parseFloat(productFee - discountFee + shippingFee))
      );
    });
    fetchAllDayShip($("#district").val(), $("#ward").val()).then((data) => {
      if ($("#delivery").prop("checked")) {
        console.log(data.data.leadtime);
        // Create a new Date object and pass the timestamp as milliseconds
        var date = new Date(data.data.leadtime * 1000);

        // Extract the components of the date
        var year = date.getFullYear();
        var month = ("0" + (date.getMonth() + 1)).slice(-2); // Correctly format the month
        var day = ("0" + date.getDate()).slice(-2); // Correctly format the day

        // Create a formatted date string in the desired format (day/month/year)
        var formattedDate = day + "/" + month + "/" + year;
        $("#delivery-date").text(formattedDate);
        console.log(formattedDate);
      }
    });
  });
}
// ngày ship hàng
function fetchAllDayShip(to_district_id, to_ward_code) {
  const url =
    `https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/leadtime` +
    `?from_district_id=1485&from_ward_code=1A0607&to_district_id=${to_district_id}&to_ward_code=${to_ward_code}&service_id=53320`;

  return fetch(url, {
    method: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
      shop_id: "4374133",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      return data;
    });
}

// giá tiền ship
function fetchAllMoneyShip(to_district_id, to_ward_code) {
  const requestOptions = {
    method: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
      shop_id: "4374133",
    },
    // Construct the URL with query parameters
    url: `https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee?service_type_id=2&
    insurance_value=&coupon=&from_district_id=1485&to_district_id=${to_district_id}&to_ward_code=${to_ward_code}&height=15&length=15&weight=1000&width=15`,
  };

  return fetch(requestOptions.url, requestOptions)
    .then((response) => response.json())
    .then((data) => {
      return data;
    });
}

// JS TẠO HOÁ ĐƠN
$(document).ready(function () {
  let tabCount = 0;
  const maxTabs = 4;

  CallGiaoHang();

  function setFirstTabActive() {
    $("#invoiceTabs .nav-item").removeClass("active");
    $("#invoiceTabs .tab-content .tab-pane").removeClass("active show");

    const firstTabLink = $("#invoiceTabs .nav-item:first-child .nav-link");
    const firstTabContent = $(firstTabLink.attr("href"));

    firstTabLink.addClass("active");
    firstTabContent.addClass("show active");
  }

  // Call the function to set the first tab as active
  setFirstTabActive();
  // Khôi phục danh sách hoá đơn từ Local Storage khi tải lại trang
  restoreInvoicesFromLocalStorage();

  $("#addInvoiceBtn").on("click", function () {
    fetchAllMoneyShip(3695, 90768).then((data) => {
      console.log(data);
    });
    fetchAllDayShip(1452, 21012).then((data) => {
      console.log(data);
    });

    if (tabCount >= maxTabs) {
      alert("Bạn đã đạt tới số lượng tối đa của hoá đơn (5 hoá đơn).");
      return;
    }
    arr.push([]);
    customers.push({});
    console.log(arr);
    console.log(customers);
    tabCount++;
    selectedOrder = tabCount;
    const newTabId = `invoice${tabCount}`;
    const newTabContent = `
          <div id="${newTabId}" class="tab-pane"  role="tabpanel" aria-labelledby="invoice-tab">
          <table class="table">
          <thead>
              <tr>
                  <th>STT</th>
                  <th>TÊN SẢN PHẨM</th>
                  <th>MÀU</th>
                  <th>SIZE</th>
                  <th>GIÁ TIỀN</th>
                  <th>SỐ LƯỢNG</th>
                  <th>THÀNH TIỀN</th>
              </tr>
          </thead>
          <tbody id="productsList">
          </tbody>
          <tfoot>
              <tr>
                  <th></th>
                  <th></th>
                  <th></th>
                  <th></th>
                  <th></th>
                  <th>TỔNG TIỀN</th>
                  <th id="total-bill${tabCount}"></th>
              </tr>
          </tfoot>
      </table>
          </div>
      `;

    $("#invoiceTabs .nav-item").removeClass("active");
    $("#invoiceTabs .tab-content .tab-pane").removeClass("active show");

    $("#invoiceTabs").append(`
          <li class="nav-item">
              <a class="nav-link" data-toggle="tab" href="#${newTabId}">Hoá Đơn <span class="close-tab" data-index="${tabCount}">✕</span></a>
          </li>
      `);

    $("#invoiceTabContent").append(newTabContent);
    $('[data-toggle="tooltip"]').tooltip();
    // Lưu danh sách hoá đơn vào Local Storage sau khi thêm hoá đơn
    saveInvoicesToLocalStorage();
    $(`#invoiceTabs a[href="#${newTabId}"]`).tab("show");
    // Kích hoạt hoá đơn vừa tạo
    $(`#${newTabId}`).addClass("show active"); // "show" is important for Bootstrap 4
    $(`#invoiceTabs a[href="#${newTabId}"]`).parent().addClass("active");
  });

  $("#invoiceTabs").on("click", "a.nav-link", function () {
    $("#invoiceTabs .nav-item").removeClass("active");
    $(this).parent().addClass("active");
    $("#invoiceTabContent .tab-pane").removeClass("active");
    $($(this).attr("href")).addClass("active");
    var parts = this.href.split(/[\/#]/);
    var lastPart = parts[parts.length - 1];
    var matches = lastPart.match(/\d+/);
    var lastNumber = parseInt(matches[0]);
    selectedOrder = lastNumber;
    console.log(customers[selectedOrder]);
    if (customers[selectedOrder].id == undefined) {
      $("#customer-name").text("Khách lẻ");
      $("#customer-email").text();
      $("#customer-phone").text();
      $("#customer-id").text();
      $(".hidden-info").hide();
      $("#customer-id").hide();
      $("#re-select").hide();
    } else {
      $("#customer-name").text(customers[selectedOrder].fullName);
      $("#customer-email").text(customers[selectedOrder].email);
      $("#customer-phone").text(customers[selectedOrder].phoneNumber);
      $("#customer-id").text(customers[selectedOrder].id);
      $(".hidden-info").show();
      $("#customer-id").hide();
      $("#re-select").show();
    }
    // Recalculate the totalSum
    var totalSum = 0;
    $(`#invoice${selectedOrder} tbody tr`).each(function () {
      var rowTotalCell = $(this).find("td:eq(6)");
      var rowTotal = parseFloat(rowTotalCell.text().replace(/[.,₫]/g, ""));
      if (!isNaN(rowTotal)) {
        totalSum += rowTotal;
      }
    });

    $(`#total-bill${selectedOrder}`).text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(totalSum)
    );
    $(`#total`).text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(totalSum)
    );
    var option_voucher = [];
    // Add a default option with a value of -1 and "Select a voucher" text
    option_voucher.push('<option value="-1">Không áp dụng</option>');
    $.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
      for (var i = 0; i < result.length; i++) {
        var productFee = parseFloat(
          $("#total")
            .text()
            .replace(/[^0-9]/g, "")
        );
        if (result[i].expression <= productFee) {
          option_voucher.push(
            '<option value="',
            result[i].id,
            '">',
            result[i].code,
            "</option>"
          );
        }
      }
      $("#voucher-select").html(option_voucher.join(""));
    });
    if ($("#voucher-select").val() == -1) {
      $(`#dicscount-price`).text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(0)
      );
    } else {
      $.ajax({
        url:
          "https://localhost:44328/api/Voucher/Get/" +
          $("#voucher-select").val(),
        type: "GET",
        dataType: "json",
        success: function (data) {
          console.log(JSON.stringify(data));
          var dongAmountString = $("#total").text();
          var cleanedString = dongAmountString.replace(/[^0-9]/g, ""); // Remove non-numeric characters
          var dongAmountNumber = (data.value * parseFloat(cleanedString)) / 100; // Parse the cleaned string to a number

          $("#discount-price").text(
            Intl.NumberFormat("vi-VN", {
              style: "currency",
              currency: "VND",
            }).format(dongAmountNumber)
          );
          var shippingFee = parseFloat(
            $("#ship-fee")
              .text()
              .replace(/[^0-9]/g, "")
          );
          $("#sum").text(
            Intl.NumberFormat("vi-VN", {
              style: "currency",
              currency: "VND",
            }).format(
              parseFloat(
                cleanedString - (data.value * parseFloat(cleanedString)) / 100
              ) + shippingFee
            )
          );
        },
        error: function () {
          console.log("Error retrieving data.");
        },
      });
    }

    $(`#sum`).text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(totalSum)
    );

    console.log(arr);
    console.log(customers);
  });

  $("#invoiceTabs").on("mouseenter", "a.nav-link", function () {
    $(this).tooltip("show");
  });

  $("#invoiceTabs").on("mouseleave", "a.nav-link", function () {
    $(this).tooltip("hide");
  });

  $("#invoiceTabs").on("click", ".close-tab", function (e) {
    e.stopPropagation();
    const tabIndex = $(this).data("index");
    if (confirm("Bạn có muốn xoá hoá đơn không?")) {
      $(`#invoice${tabIndex}`).remove();
      $(this).parent().parent().remove();
      tabCount--;
      updateTabIndices(); // Update tab indices
      removeInvoiceFromLocalStorage(tabIndex);
      arr.splice(tabIndex - 1, 1); // Corrected index for array splice
      customers.splice(tabIndex - 1, 1); // Corrected index for array splice
      setFirstTabActive();
    }
  });

  function removeInvoiceFromLocalStorage(invoiceIndex) {
    const invoices = JSON.parse(localStorage.getItem("invoices"));
    if (invoices && invoices.length > 0) {
      invoices.splice(invoiceIndex - 1, 1); // Remove the specific invoice
      localStorage.setItem("invoices", JSON.stringify(invoices));
    }
  }

  function updateTabIndices() {
    $(".close-tab").each(function (index) {
      $(this).data("index", index + 1);
    });
  }

  $('[data-toggle="tooltip"]').tooltip({
    placement: "bottom",
    trigger: "hover",
  });

  function saveInvoicesToLocalStorage() {
    const invoices = [];
    for (let i = 1; i <= tabCount; i++) {
      invoices.push($(`#invoice${i}`).html());
    }

    localStorage.setItem("invoices", JSON.stringify(invoices));
  }

  function restoreInvoicesFromLocalStorage() {
    const invoices = JSON.parse(localStorage.getItem("invoices"));
    if (invoices && invoices.length > 0) {
      tabCount = invoices.length;
      invoices.forEach((invoiceContent, index) => {
        arr.push([]);
        customers.push({});
        const tabIndex = index + 1;
        const newTabId = `invoice${tabIndex}`;
        $("#invoiceTabs").append(`
                  <li class="nav-item">
                      <a class="nav-link" data-toggle="tab" href="#${newTabId}">Hoá Đơn<span class="close-tab" data-index="${tabIndex}">✕</span></a>
                  </li>
              `);
        $("#invoiceTabContent").append(`
                  <div id="${newTabId}" class="container tab-pane">${invoiceContent}</div>
              `);
      });

      // Kích hoạt tab hoá đơn đã tạo trước đó
      const activeTabIndex = $("#invoiceTabs .nav-item.active a.nav-link").attr(
        "href"
      );
      if (activeTabIndex) {
        $(activeTabIndex).addClass("active");
      }
    }
  }
});
// END JS TẠO HOÁ ĐƠN

const decrementButton = document.getElementById("decrement");
const incrementButton = document.getElementById("increment");
const quantityInput = document.getElementById("quantity");
quantityInput.addEventListener("change", function () {
  const inputValue = parseFloat(quantityInput.value);
  if (isNaN(inputValue) || inputValue <= 1) {
    quantityInput.value = 1;
  }
  if (inputValue >= instock) {
    quantityInput.value = instock;
  }
});
decrementButton.addEventListener("click", function () {
  let inputValue = parseFloat(quantityInput.value);
  inputValue--;
  if (inputValue <= 1) {
    inputValue = 1;
  }
  if (inputValue >= instock) {
    quantityInput.value = instock;
  }
  quantityInput.value = inputValue;
});

incrementButton.addEventListener("click", function () {
  let inputValue = parseFloat(quantityInput.value);
  inputValue++;
  quantityInput.value = inputValue;
  if (inputValue >= instock) {
    quantityInput.value = instock;
  }
});

// // Gọi API và xử lý dữ liệu
// fetch("https://localhost:44328/api/Product/active")
//   .then((response) => response.json())
//   .then((data) => {
//     // Lặp qua dữ liệu từ API và cập nhật bảng HTML
//     const tableBody = document.querySelector("#product-table tbody");

//     data.forEach((product) => {
//       const row = document.createElement("tr");

//       row.innerHTML = `
//                   <td>${product.id}</td>
//                   <td><img src="${product.productImages}"
//       }" class="product-image"></td>
//                   <td>${product.name}</td>
//                   <td>${product.discountRate}</td>
//                   <td><span class="status-badge ${
//                     product.status === "out-of-stock"
//                       ? "out-of-stock"
//                       : "in-stock"
//                   }">${
//         product.status === "out-of-stock" ? "Ngừng kinh doanh" : "Kinh doanh"
//       }</span></td>
//                   <td>
//                       <button type="button" class="btn btn-warning">
//                           Chọn
//                       </button>
//                   </td>
//               `;

//       tableBody.appendChild(row);
//     });
//   })
//   .catch((error) => {
//     console.error("Lỗi khi gọi API:", error);
//   });
$("#productData").DataTable({
  ajax: {
    url: "https://localhost:44328/api/Product/active",
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
      data: "productImages[0].imageUrl",
      title: "Ảnh",
      render: function (data, type, row) {
        return `<img src="https://localhost:44328/${data}" alt="" style="border-radius: 10%;" width=120px height=110px>`;
      },
    },
    { data: "name", title: "Tên sản phẩm" },
    {
      data: "retailPrice",
      title: "Giá gốc",
      render: function (data, type, row) {
        return Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(data);
      },
    },
    {
      data: "discountRate",
      title: "Giá bán",
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
        if (data == 1) {
          return '<span class="badge badge-pill badge-primary" style="padding:10px;background-color: #1967d2;border-color: #1967d2;" >Kinh doanh</span>';
        } else {
          return '<span class="badge badge-pill badge-danger" style="padding:10px;">Ngừng kinh doanh</span>';
        }
      },
    },
    {
      title: "Thao tác",
      render: function () {
        return '<td><button type="button" class="btn btn-outline-primary">Chọn</button></td>';
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
$("#productData tbody").on("click", "tr", function (e) {
  let selectedProduct = $("#productData").DataTable().row(this).data().id;
  if (selectedProduct !== null) {
    localStorage.setItem("selectedProduct", selectedProduct);
    const id = localStorage.getItem("selectedProduct");
    console.log(id);
    $("#productDetailModal").modal("show");
    $.ajax({
      url: "https://localhost:44328/api/Product/" + id,
      type: "GET",
      dataType: "json",
      success: function (data) {
        console.log(JSON.stringify(data));
        $("#name").text(data.name);
        $("#discountRate").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data.discountRate)
        );
        // $('#costPrice').text(data.costPrice+ "VND");
        // $('#status').val(data.status);
        // $('#output').attr('src', `/backend/.NET/Webapi/wwwroot/Images/${id}.jpg`);

        // hiển thị màu
        var colorIds = [
          ...new Set(
            data.productImages.map(function (item) {
              return item.colorId;
            })
          ),
        ];
        var images = data.productImages.map(function (item) {
          return {
            colorId: item.colorId,
            imageUrl: item.imageUrl,
          };
        });
        console.log(images);
        // Assuming product is an object with a Colors property
        colorIds.forEach(function (colorId) {
          $.ajax({
            url: "https://localhost:44328/api/Color/Get/" + colorId,
            type: "GET",
            dataType: "json",
            success: function (data) {
              console.log(data.id);
              console.log(colorId);
              product.Colors.push({
                id: colorId,
                name: data.name,
                Images: [],
                Sizes: [],
              });
              loadColorE();
              for (let i = 0; i < product.Colors.length; i++) {
                if (product.Colors[i].id === colorId) {
                  const imagesForColor = images.filter(
                    (image) => image.colorId === colorId
                  );

                  imagesForColor.forEach((imageData) => {
                    const imageLink =
                      "https://localhost:44328/" +
                      imageData.imageUrl.replace(/\\/g, "/");

                    fetch(imageLink)
                      .then((response) => response.blob())
                      .then((blob) => {
                        const newImage = new File([blob], "image.jpg", {
                          type: "image/jpeg",
                        });

                        product.Colors[i].Images.push({
                          file: newImage,
                          setAsDefault: false,
                        });

                        loadImageE();
                      });
                  });
                }
              }
            },
            error: function () {
              console.log("Error retrieving data.");
            },
          });
        });
        // load Size
        var sizeData = data.stocks.map(function (item) {
          return {
            sizeId: item.sizeId,
            unitInStock: item.unitInStock,
            colorId: item.colorId,
          };
        });
        var promises = [];

        sizeData.forEach(function (size) {
          var promise = $.ajax({
            url: "https://localhost:44328/api/Size/Get/" + size.sizeId,
            type: "GET",
            dataType: "json",
          })
            .then(function (data) {
              console.log(data);

              var selectedColorText = {
                numberSize: data.numberSize,
                id: size.sizeId,
              };
              console.log(sizeData.length);
              // Find the correct color index based on colorId
              for (let i = 0; i < sizeData.length; i++) {
                if (product.Colors[i].id === size.colorId) {
                  selectedColorText.unitInStock = size.unitInStock;
                  product.Colors[i].Sizes.push(selectedColorText);
                }
              }
              // Push selectedColorText to the Sizes array of the corresponding color
            })
            .catch(function () {
              console.log("Error retrieving data.");
            });

          promises.push(promise);
        });

        // Wait for all AJAX requests to complete
        Promise.all(promises).then(function () {
          loadSizeE(); // This will be called after all requests are finished
          // Assuming you have an imageLink as you mentioned earlier

          console.log(data);
          console.log(product);
        });
        addToCartItem.id = data.id;
        addToCartItem.name = data.name;
        addToCartItem.price = data.discountRate;
      },
      error: function () {
        console.log("Error retrieving data.");
      },
    });
  }
});
var customerTable = $("#customer-table").DataTable({
  ajax: {
    url: "https://localhost:44328/api/AppUser/GetUsersWithUserRole",
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
    { data: "email", title: "Email" },
    { data: "phoneNumber", title: "SĐT" },
    { data: "fullName", title: "Họ và tên" },
    {
      data: "modifiedDate",
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
      data: "status",
      title: "Trạng thái",
      render: function (data, type, row) {
        if (data == 1) {
          return '<span class="badge badge-pill badge-primary" style="padding:10px;background-color: #1967d2;border-color: #1967d2;">Kích hoạt</span>';
        } else {
          return '<span class="badge badge-pill badge-danger" style="padding:10px;">Đã hủy</span>';
        }
      },
    },
    {
      title: "Thao tác",
      render: function () {
        return '<td><button type="button" class="btn btn-outline-primary">Chọn</button></td>';
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
customers = [{}];
$("#customer-table tbody").on("click", "tr", function (e) {
  e.preventDefault();
  let customerId = $("#customer-table").DataTable().row(this).data().id;
  if (customerId !== null) {
    $.ajax({
      url: `https://localhost:44328/api/AppUser/Get/${customerId}`,
      type: "GET",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (data) {
        console.log(data);
        customers[selectedOrder] = {
          id: data.id,
          fullName: data.fullName,
          email: data.email,
          phoneNumber: data.phoneNumber,
        };
        console.log(customers);
        $("#customer-name").text(data.fullName);
        $("#customer-email").text(data.email);
        $("#customer-phone").text(data.phoneNumber);
        $("#customer-id").text(data.id);
        $(".hidden-info").show();
        $("#customer-id").hide();
        $("#re-select").show();
        $("#account-modal").modal("hide");
      },
    });
  }
});
$("#re-select").on("click", function (e) {
  $("#customer-name").text("Khách lẻ");
  $("#customer-email").text();
  $("#customer-phone").text();
  $("#customer-id").text();
  $(".hidden-info").hide();
  $("#customer-id").hide();
  $("#re-select").hide();
  customers[selectedOrder] = {}
});
var product = {
  retailPrice: 0,
  description: "",
  status: 1,
  brand: 1,
  discountRate: 0,
  soleId: 0,
  materialId: 0,
  name: "",
  Categories: [],
  Colors: [],
};
var addToCartItem = {
  id: "",
  name: "",
  colorId: "",
  color: "",
  sizeId: "",
  size: 0,
  price: 0,
  amount: 0,
  total: 0,
};

var selectedColor = 0;
var selectedOrder = 0;
var instock = 0;
const arr = [[]];

function findIndexById(array, id) {
  for (var i = 0; i < array.length; i++) {
    if (array[i].id === id) {
      return i; // Trả về chỉ số khi tìm thấy phần tử có id tương ứng
    }
  }
  return -1; // Trả về -1 nếu không tìm thấy phần tử
}
function loadColorE() {
  const plusButtonContainer = document.getElementById("render-color");
  plusButtonContainer.innerHTML = "";

  if (product.Colors.length !== 0) {
    product.Colors.forEach((color, index) => {
      var newDiv = document.createElement("div");
      newDiv.className = "container-color";

      var newButton = document.createElement("button");
      newButton.type = "button";
      newButton.className = "btn btn-outline-dark color";
      newButton.id = color.id;
      newButton.textContent = color.name;

      // Add "active" class to the first button
      if (index === 0) {
        newButton.classList.add("active");
        selectedColor = findIndexById(product.Colors, color.id);
        addToCartItem.color = color.name;
        addToCartItem.colorId = color.id;
        loadSizeE();
        loadImageE();
        $("#product-instock").hide();
        $("#quantity-input").hide();
        $("#addToCart").hide();
      }

      newButton.addEventListener("click", function (e) {
        var buttons = document.getElementsByClassName("btn-outline-dark color");
        for (var i = 0; i < buttons.length; i++) {
          buttons[i].classList.remove("active");
        }

        // Thêm lớp active cho nút được bấm
        newButton.classList.add("active");

        selectedColor = findIndexById(product.Colors, e.target.id);
        addToCartItem.color = color.name;
        addToCartItem.colorId = color.id;
        loadSizeE();
        loadImageE();
        $("#product-instock").hide();
        $("#quantity-input").hide();
        $("#addToCart").hide();
      });
      newDiv.appendChild(newButton);
      plusButtonContainer.appendChild(newDiv);
    });
  }
}

function loadSizeE() {
  var plusButtonContainer = document.getElementById("render-size");
  // Xóa tất cả các phần tử con trong containerParent
  while (plusButtonContainer.firstChild) {
    plusButtonContainer.removeChild(plusButtonContainer.firstChild);
  }

  if (product.Colors.length !== 0) {
    if (product.Colors[selectedColor].Sizes.length !== 0) {
      product.Colors[selectedColor].Sizes.forEach((element) => {
        var container = document.createElement("div");
        container.className = "container-unit";

        var newButton = document.createElement("button");
        newButton.className = "btn btn-outline-dark size";
        newButton.textContent = element.numberSize;

        newButton.addEventListener("click", function (event) {
          event.preventDefault();
          event.stopPropagation();
          var buttons = document.getElementsByClassName(
            "btn-outline-dark size"
          );
          for (var i = 0; i < buttons.length; i++) {
            buttons[i].classList.remove("active");
          }

          // Thêm lớp active cho nút được bấm
          newButton.classList.add("active");
          instock = element.unitInStock;
          addToCartItem.size = element.numberSize;
          addToCartItem.sizeId = element.id;
          console.log(instock);
          $("#product-instock").show();
          $("#product-instock").text(`Còn ${element.unitInStock} sản phẩm`);
          if (element.unitInStock !== 0) {
            $("#quantity-input").show();
            $("#addToCart").show();
            $("#quantity").val(1);
          } else {
            $("#quantity-input").hide();
            $("#addToCart").hide();
            $("#quantity").val(0);
          }
        });
        container.appendChild(newButton);
        plusButtonContainer.appendChild(container);
      });
    }
  }
}
function loadImageE() {
  const uploadList = document.querySelector(".upload-list");
  const dynamicDivs = uploadList.querySelectorAll(".preview-container");

  dynamicDivs.forEach((dy) => {
    uploadList.removeChild(dy);
  });

  if (selectedColor !== -1) {
    if (product.Colors[selectedColor].Images.length !== 0) {
      product.Colors[selectedColor].Images.forEach((img) => {
        const previewContainer = document.createElement("div");
        previewContainer.className = "preview-container";
        const previewImage = document.createElement("img");
        previewImage.src = URL.createObjectURL(img.file);
        previewImage.alt = "Preview";
        previewImage.className = "preview-image";
        previewImage.style = " width: 150px;height: 150px;border-radius: 8px;";

        previewContainer.appendChild(previewImage);
        uploadList.appendChild(previewContainer);
      });
    }
  }
}
var option_voucher = [];
// Add a default option with a value of -1 and "Select a voucher" text
option_voucher.push('<option value="-1">Không áp dụng</option>');
$.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
  for (var i = 0; i < result.length; i++) {
    var productFee = parseFloat(
      $("#total")
        .text()
        .replace(/[^0-9]/g, "")
    );
    if (result[i].expression <= productFee) {
      option_voucher.push(
        '<option value="',
        result[i].id,
        '">',
        result[i].code,
        "</option>"
      );
    }
  }
  $("#voucher-select").html(option_voucher.join(""));
});

$("#voucher-select").on("change", function () {
  if (this.value == -1) {
    var dongAmountString = $("#total").text();
    var cleanedString = dongAmountString.replace(/[^0-9]/g, ""); // Remove non-numeric characters
    $("#discount-price").text("0 ₫");
    var shippingFee = parseFloat(
      $("#ship-fee")
        .text()
        .replace(/[^0-9]/g, "")
    );
    $("#sum").text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(parseFloat(cleanedString) + shippingFee)
    );
  }
  if ($("#total").text() !== "0") {
    const id = $(this).val();
    $.ajax({
      url: "https://localhost:44328/api/Voucher/Get/" + id,
      type: "GET",
      dataType: "json",
      success: function (data) {
        console.log(JSON.stringify(data));
        var dongAmountString = $("#total").text();
        var cleanedString = dongAmountString.replace(/[^0-9]/g, ""); // Remove non-numeric characters
        var dongAmountNumber = (data.value * parseFloat(cleanedString)) / 100; // Parse the cleaned string to a number

        $("#discount-price").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(dongAmountNumber)
        );
        var shippingFee = parseFloat(
          $("#ship-fee")
            .text()
            .replace(/[^0-9]/g, "")
        );
        $("#sum").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(
            parseFloat(
              cleanedString - (data.value * parseFloat(cleanedString)) / 100
            ) + shippingFee
          )
        );
      },
      error: function () {
        console.log("Error retrieving data.");
      },
    });
  }
});
$("#addToCart").click(function () {
  addToCartItem.amount = Number($("#quantity").val());
  addToCartItem.total = addToCartItem.amount * addToCartItem.price;
  var data = addToCartItem;
  console.log(addToCartItem);
  var tbody = $(`#invoice${selectedOrder} tbody`);

  // Generate a unique identifier for the item (combination of ID, color, and size)
  var itemIdentifier = data.id + "_" + data.color + "_" + data.size;

  // Find existing item with the same identifier in the cart
  var existingItem = tbody.find("tr[data-id='" + itemIdentifier + "']");

  if (existingItem.length > 0) {
    // Update existing item's quantity and total
    var existingAmount = parseInt(existingItem.find("td:eq(5)").text());
    var newAmount = parseInt(data.amount);

    var totalAmount = existingAmount + newAmount;
    //check so luong
    if (existingAmount >= instock) {
      totalAmount = instock;
    }
    existingItem.find("td:eq(5)").text(totalAmount);

    // Calculate the updated "Thành tiền" value
    var updatedRowTotal = totalAmount * data.price;
    existingItem.find("td:eq(6)").text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(updatedRowTotal)
    );
  } else {
    // Create a new row
    var newRow = $("<tr>");

    // Set data-id attribute on the row for matching
    newRow.attr("data-id", itemIdentifier);

    // Calculate the "Thành tiền" for the new row
    var newRowTotal = data.amount * data.price;
    // Create and append td cells with data values

    newRow.append(
      $("<td>").text(tbody.children().length + 1), // STT (Auto-increment)
      $("<td>").text(data.name), // Tên sản phẩm
      $("<td>").text(data.color), // Màu sắc
      $("<td>").text(data.size), // Kích thước
      $("<td>").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(data.price)
      ), // Kích thước
      $("<td>").text(data.amount), // Số lượng
      $("<td>").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(newRowTotal)
      ), // Thành tiền for the new row
      $("<button>").text("X").addClass("btn btn-danger delete-item").css({
        marginTop: "5px", // Add a 5px margin-top
      })
    );
    tbody.append(newRow);
  }
  var totalSum = 0;

  $(`#invoice${selectedOrder} tbody tr`).each(function () {
    var rowTotalCell = $(this).find("td:eq(6)");
    console.log(rowTotalCell.text());

    var rowTotal = parseFloat(rowTotalCell.text().replace(/[.,₫]/g, ""));
    if (!isNaN(rowTotal)) {
      totalSum += rowTotal;
    }
  });
  console.log(totalSum);
  $(`#total-bill${selectedOrder}`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );

  $(`#total`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );
  var option_voucher = [];
  // Add a default option with a value of -1 and "Select a voucher" text
  option_voucher.push('<option value="-1">Không áp dụng</option>');
  $.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
    for (var i = 0; i < result.length; i++) {
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      if (result[i].expression <= productFee) {
        option_voucher.push(
          '<option value="',
          result[i].id,
          '">',
          result[i].code,
          "</option>"
        );
      }
    }
    $("#voucher-select").html(option_voucher.join(""));
  });
  if ($("#voucher-select").val() == -1) {
    $(`#dicscount-price`).text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(0)
    );
  } else {
    $.ajax({
      url:
        "https://localhost:44328/api/Voucher/Get/" + $("#voucher-select").val(),
      type: "GET",
      dataType: "json",
      success: function (data) {
        console.log(JSON.stringify(data));
        var dongAmountString = $("#total").text();
        var cleanedString = dongAmountString.replace(/[^0-9]/g, ""); // Remove non-numeric characters
        var dongAmountNumber = (data.value * parseFloat(cleanedString)) / 100; // Parse the cleaned string to a number

        $("#discount-price").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(dongAmountNumber)
        );
        var shippingFee = parseFloat(
          $("#ship-fee")
            .text()
            .replace(/[^0-9]/g, "")
        );
        $("#sum").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(
            parseFloat(
              cleanedString - (data.value * parseFloat(cleanedString)) / 100
            ) + shippingFee
          )
        );
      },
      error: function () {
        console.log("Error retrieving data.");
      },
    });
  }

  $(`#sum`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );

  $.ajax({
    url: "https://localhost:44328/api/Product/" + addToCartItem.id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(data.stocks);
      $("#productDetailModal").modal("hide");
      console.log(arr);

      var existingItemIndex = arr[selectedOrder].findIndex(function (item) {
        return (
          item.productId === addToCartItem.id &&
          item.sizeId === addToCartItem.sizeId &&
          item.colorId === addToCartItem.colorId
        );
      });
      console.log(existingItemIndex);
      if (existingItemIndex !== -1) {
        arr[selectedOrder][existingItemIndex].quantity += parseInt(
          $("#quantity").val(),
          10
        );
        // Kiểm tra số lượng tồn kho và cập nhật số lượng nếu cần
        if (arr[selectedOrder][existingItemIndex].quantity >= instock) {
          arr[selectedOrder][existingItemIndex].quantity = instock;
        }
      } else {
        arr[selectedOrder].push({
          itemIdentifier: itemIdentifier, // Add itemIdentifier to the object
          unitPrice: parseFloat(
            $("#discountRate")
              .text()
              .replace(/[^0-9.]/g, "")
          ),
          quantity: parseInt($("#quantity").val(), 10),
          name: $("#name").text(),
          productId: addToCartItem.id,
          colorId: addToCartItem.colorId,
          sizeId: addToCartItem.sizeId,
          unitPrice: addToCartItem.price,
        });
      }
      console.log(arr);
    },
  });
});
$(document).on("click", ".delete-item", function () {
  var row = $(this).closest("tr");
  var itemIdentifier = row.attr("data-id");

  // Remove the item from the UI
  row.remove();

  // Ensure arr[selectedOrder] is defined and initialized as an array
  if (!Array.isArray(arr[selectedOrder])) {
    arr[selectedOrder] = [];
  }

  // Create a new inner array excluding the item to delete
  arr[selectedOrder] = arr[selectedOrder].filter(function (item) {
    return item.itemIdentifier !== itemIdentifier;
  });

  // Recalculate the totalSum
  var totalSum = 0;
  $(`#invoice${selectedOrder} tbody tr`).each(function () {
    var rowTotalCell = $(this).find("td:eq(6)");
    var rowTotal = parseFloat(rowTotalCell.text().replace(/[.,₫]/g, ""));
    if (!isNaN(rowTotal)) {
      totalSum += rowTotal;
    }
  });

  $(`#total-bill${selectedOrder}`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );
  $(`#total`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );
  var option_voucher = [];
  // Add a default option with a value of -1 and "Select a voucher" text
  option_voucher.push('<option value="-1">Không áp dụng</option>');
  $.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
    for (var i = 0; i < result.length; i++) {
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      if (result[i].expression <= productFee) {
        option_voucher.push(
          '<option value="',
          result[i].id,
          '">',
          result[i].code,
          "</option>"
        );
      }
    }
    $("#voucher-select").html(option_voucher.join(""));
  });
  if ($("#voucher-select").val() == -1) {
    $(`#dicscount-price`).text(
      Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND",
      }).format(0)
    );
  } else {
    $.ajax({
      url:
        "https://localhost:44328/api/Voucher/Get/" + $("#voucher-select").val(),
      type: "GET",
      dataType: "json",
      success: function (data) {
        console.log(JSON.stringify(data));
        var dongAmountString = $("#total").text();
        var cleanedString = dongAmountString.replace(/[^0-9]/g, ""); // Remove non-numeric characters
        var dongAmountNumber = (data.value * parseFloat(cleanedString)) / 100; // Parse the cleaned string to a number

        $("#discount-price").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(dongAmountNumber)
        );
        var shippingFee = parseFloat(
          $("#ship-fee")
            .text()
            .replace(/[^0-9]/g, "")
        );
        $("#sum").text(
          Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(
            parseFloat(
              cleanedString - (data.value * parseFloat(cleanedString)) / 100
            ) + shippingFee
          )
        );
      },
      error: function () {
        console.log("Error retrieving data.");
      },
    });
  }

  $(`#sum`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );

  console.log(arr);
});

function clearTableAndData() {
  // Clear the table by removing all rows
  $(`#invoice${selectedOrder} tbody tr`).remove();

  // Reset the total sum to 0
  var totalSum = 0;
  $(`#total-bill${selectedOrder}`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );

  // Reset the 'total', and 'sum' elements if needed
  $(`#total`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );
  var option_voucher = [];
  // Add a default option with a value of -1 and "Select a voucher" text
  option_voucher.push('<option value="-1">Không áp dụng</option>');
  $.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
    for (var i = 0; i < result.length; i++) {
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      if (result[i].expression <= productFee) {
        option_voucher.push(
          '<option value="',
          result[i].id,
          '">',
          result[i].code,
          "</option>"
        );
      }
    }
    $("#voucher-select").html(option_voucher.join(""));
  });
  $(`#sum`).text(
    Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(totalSum)
  );

  // Clear the 'arr' variable
  arr[selectedOrder] = [];
  customers[selectedOrder] = {};
}

// You can call this function whenever you need to clear the table and data
// For example, you can call it when the user clicks a "Clear" button or performs a specific action.
// clearTableAndData();

$("#create-bill").click(function (event) {
  event.preventDefault();
  const outputJSON = JSON.stringify(arr[selectedOrder], null, 2);
  console.log(outputJSON);
  var formData = {
    userId: $("#customer-id").text(),
    employeeId: localStorage.getItem("user-id"),
    address: "",
    phoneNumber: $("#customer-phone").text(),
    customerName: $("#customer-name").text(),
    voucherId: $("#voucher-select").val(),
    orderItems: JSON.parse(outputJSON),
    amount: parseInt($("#sum").text().replace(/\D/g, "")),
  };
  if ($("#delivery").prop("checked") === true) {
    formData = {
      userId: $("#customer-id").text(),
      employeeId: localStorage.getItem("user-id"),
      address:
        $("#address").val() +
        ", " +
        $("#ward option:selected").text() +
        ", " +
        $("#district option:selected").text() +
        ", " +
        $("#province option:selected").text(),
      phoneNumber: $("#phoneNumber").val(),
      customerName: $("#customerName").val(),
      voucherId: $("#voucher-select").val(),
      note: $("#note").val(),
      orderItems: JSON.parse(outputJSON),
      amount: parseInt($("#sum").text().replace(/\D/g, "")),
    };
    if($("#customer-name").text()=="Khách lẻ"){
      formData.customerName = "Khách lẻ"
    }
    fetchAllDayShip($("#district").val(), $("#ward").val()).then((data) => {
      console.log(data);
    });
    fetchAllMoneyShip($("#district").val(), $("#ward").val()).then((data) => {
      console.log(data.data.total);
    });
  }
  validateForm();
  if (formData.voucherId === "-1") {
    formData.voucherId = null;
  }
  if (formData.userId === "") {
    formData.userId = null;
  }
  if ($("#delivery").prop("checked") == true) {
    formData.shipping = true;
  } else {
    formData.shipping = false;
  }
  // var checkBox = document.getElementById("delivery");
  // var giaTriBoolean = checkBox.checked;
  // console.log(giaTriBoolean)
  if (formData.customerName === "") {
    return;
  }
  if (formData.phoneNumber === "" && $("#delivery").prop("checked") == true) {
    return;
  }
  if (
    !idContainOnlyNum(formData.phoneNumber) ||
    (!onlyContain10Char(formData.phoneNumber) &&
      $("#delivery").prop("checked") == true)
  ) {
    return;
  }
  if (confirm(`Bạn có muốn thanh toán hóa đơn này không?`)) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/PayAtStore",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        //window.location.href = `/frontend/admin/bill.html`;
        clearTableAndData();
        $("#customer-name").text("Khách lẻ");
        $("#customer-email").text("");
        $("#customer-phone").text("");
        $("#customer-id").text("");
        $(".hidden-info").hide();
        $("#customer-id").text("");
        //
        $("#delivery-field")[0].reset();
        $("#delivery").prop("checked", false);

        var x = document.getElementById("customer-info");
        x.style.visibility = "hidden";
        $("#delivery-field").hide();
        $("#delivery-info").hide();

        $("#voucher-select").val(-1);
        $("#total").text("0 ₫");
        $("#ship-fee").text("0 ₫");
        $("#discount-price").text("0 ₫");
        $("#sum").text("0 ₫");
        console.log(arr);
      },
    });
  } else {
    return;
  }
});
$("#delivery").on("change", function () {
  console.log($("#delivery").prop("checked"));
  $("#delivery-field").show();
  fetchAllMoneyShip($("#district").val(), $("#ward").val()).then((data) => {
    if ($("#delivery").prop("checked")) {
      $("#ship-fee").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(data.data.total)
      );
      var shippingFee = parseFloat(
        $("#ship-fee")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var discountFee = parseFloat(
        $("#discount-price")
          .text()
          .replace(/[^0-9]/g, "")
      );
      $("#sum").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(parseFloat(productFee - discountFee + shippingFee))
      );
    } else {
      $("#ship-fee").text("0 ₫");
      var productFee = parseFloat(
        $("#total")
          .text()
          .replace(/[^0-9]/g, "")
      );
      var discountFee = parseFloat(
        $("#discount-price")
          .text()
          .replace(/[^0-9]/g, "")
      );
      $("#sum").text(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(parseFloat(productFee - discountFee))
      );
      $("#delivery-info").hide();
    }
  });
  fetchAllDayShip($("#district").val(), $("#ward").val()).then((data) => {
    if ($("#delivery").prop("checked")) {
      console.log(data.data.leadtime);
      // Create a new Date object and pass the timestamp as milliseconds
      var date = new Date(data.data.leadtime * 1000);

      // Extract the components of the date
      var year = date.getFullYear();
      var month = ("0" + (date.getMonth() + 1)).slice(-2); // Correctly format the month
      var day = ("0" + date.getDate()).slice(-2); // Correctly format the day

      // Create a formatted date string in the desired format (day/month/year)
      $("#delivery-info").show();
      var formattedDate = day + "/" + month + "/" + year;
      $("#delivery-date").text(formattedDate);
      console.log(formattedDate);
    }
  });
});
// reset product khi đóng modal
$("#productDetailModal").on("hide.bs.modal", function () {
  $("#modal-add-product").css("overflow-y", "auto");
  $("#product-instock").hide();
  $("#quantity-input").hide();
  product = {
    retailPrice: 0,
    description: "",
    status: 1,
    brand: 1,
    discountRate: 0,
    soleId: 0,
    materialId: 0,
    name: "",
    Categories: [],
    Colors: [],
  };
});

function idContainOnlyNum(value) {
  return value.match(/[^0-9]/) === null;
}

function onlyContain10Char(value) {
  return value.match(/^\w{10}$/) !== null;
}

function validateForm() {
  // Get form inputs
  var validateName = document.getElementById("validateName");
  var customerName = document.getElementById("customerName");

  var validatePhone = document.getElementById("validatePhone");
  var phoneNumber = document.getElementById("phoneNumber");

  if (customerName.value.length == 0) {
    validateName.style.display = "block";
  } else {
    validateName.style.display = "none";
  }
  if (phoneNumber.value.length == 0) {
    validatePhone.style.display = "block";
  } else {
    validatePhone.style.display = "none";
  }
  if (
    idContainOnlyNum(phoneNumber.value) == true &&
    onlyContain10Char(phoneNumber.value) == true
  ) {
    validatePhone.style.display = "none";
  } else {
    validatePhone.style.display = "block";
    validatePhone.innerText = "Số điện thoại không hợp lệ";
  }
}

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
