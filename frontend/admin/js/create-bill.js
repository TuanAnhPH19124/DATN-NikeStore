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
// JS TẠO HOÁ ĐƠN
$(document).ready(function () {
  let tabCount = 0;
  const maxTabs = 4;

  // call tỉnh
  var option_province = [];
  $.ajax({
    url: "https://online-gateway.ghn.vn/shiip/public-api/master-data/province",
    type: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
    dataType: "json",
    success: function (result) {
      console.log(result.data.length);
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

    fetchAllProvince().then(data=>console.log(data))

    fetchAllProvinceDistricts(201).then((data) => {
      console.log(data);
    });

    fetchAllProvinceWard(1804).then(data=>{
      console.log(data);
    })

    // to_district_id, to_ward_code
    fetchAllMoneyShip(3695,90768).then((data) => {
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
    console.log(arr);
    tabCount++;
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
    console.log(selectedOrder);
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

        var colorIds = [
          ...new Set(data.productImages.map((item) => item.colorId)),
        ];
        var images = data.productImages.map((item) => ({
          colorId: item.colorId,
          imageUrl: item.imageUrl,
        }));
        console.log(images);

        // Fetch color data and process images for each color
        var colorPromises = colorIds.map((colorId) =>
          $.ajax({
            url: "https://localhost:44328/api/Color/Get/" + colorId,
            type: "GET",
            dataType: "json",
          }).then(function (colorData) {
            console.log(colorData);
            console.log(colorId);

            var colorEntry = {
              id: colorId,
              name: colorData.name,
              Images: [],
              Sizes: [],
            };

            product.Colors.push(colorEntry);
            console.log(product);

            var imagesForColor = images.filter(
              (image) => image.colorId === colorId
            );

            var imagePromises = imagesForColor.map((imageData) =>
              fetch(
                "https://localhost:44328/" +
                  imageData.imageUrl.replace(/\\/g, "/")
              )
                .then((response) => response.blob())
                .then((blob) => {
                  var newImage = new File([blob], "image.jpg", {
                    type: "image/jpeg",
                  });

                  colorEntry.Images.push({
                    file: newImage,
                    setAsDefault: false,
                  });

                  loadImageE();
                })
            );

            return Promise.all(imagePromises);
          })
        );

        // Fetch size data and update color sizes
        var sizePromises = data.stocks.map((size) =>
          $.ajax({
            url: "https://localhost:44328/api/Size/Get/" + size.sizeId,
            type: "GET",
            dataType: "json",
          }).then(function (sizeData) {
            console.log(sizeData);

            var selectedColorText = {
              numberSize: sizeData.numberSize,
              id: size.sizeId,
            };

            var colorIndex = product.Colors.findIndex(
              (color) => color.id === size.colorId
            );
            if (colorIndex !== -1) {
              selectedColorText.unitInStock = size.unitInStock;
              product.Colors[colorIndex].Sizes.push(selectedColorText);
            }
          })
        );

        // Wait for all AJAX requests to complete
        Promise.all([...colorPromises, ...sizePromises]).then(function () {
          loadColorE();
          loadSizeE();
          console.log(data);
          console.log(product);
          $("#product-instock").hide();
          $("#quantity-input").hide();
          $("#addToCart").hide();
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
        $("#customer-name").text(data.fullName);
        $("#customer-email").text(data.email);
        $("#customer-phone").text(data.phoneNumber);
        $(".hidden-info").show();
        $("#account-modal").modal("hide");
      },
    });
  }
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
    product.Colors.forEach((color) => {
      var newDiv = document.createElement("div");
      newDiv.className = "container-color";

      var newButton = document.createElement("button");
      newButton.type = "button";
      newButton.className = "btn btn-outline-dark color";
      newButton.id = color.id;
      newButton.textContent = color.name;
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
          $("#quantity-input").show();
          $("#addToCart").show();
          $("#quantity").val(1);
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

    var deleteButton = $("<button>")
      .addClass("delete-button")
      .text("Delete")
      .click(function () {
        $(this).closest("tr").remove();
      });

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
      $("<td>").append(deleteButton)
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
          unitPrice: $("#discountRate").text(),
          quantity: parseInt($("#quantity").val(), 10),
          name: $("#name").text(),
          productId: addToCartItem.id,
          colorId: addToCartItem.colorId,
          sizeId: addToCartItem.sizeId,
        });
      }
      console.log(arr);
    },
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

// giao hành nhanh
// tỉnh
function fetchAllProvince() {
  return fetch(
    "https://online-gateway.ghn.vn/shiip/public-api/master-data/province",
    {
      method: "GET",
      headers: {
        token: "d73043b1-2777-11ee-b394-8ac29577e80e",
      },
    }
  )
    .then((response) => response.json())
    .then((data) => {
      return data;
    });
}

//  quận huyện
function fetchAllProvinceDistricts(codeProvince) {
  const url = `https://online-gateway.ghn.vn/shiip/public-api/master-data/district?province_id=${codeProvince}`;

  return fetch(url, {
    method: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      return data;
    });
}

// Phường/Xã
function fetchAllProvinceWard(codeDistrict) {
  const url = `https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id=${codeDistrict}`;

  return fetch(url, {
    method: "GET",
    headers: {
      token: "d73043b1-2777-11ee-b394-8ac29577e80e",
    },
  })
    .then((response) => response.json())
    .then((data) => {
      return data;
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
