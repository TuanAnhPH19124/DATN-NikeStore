$(document).ready(function () {
  let tabCount = 0;
  const maxTabs = 5;

  // Khôi phục danh sách hoá đơn từ Local Storage khi tải lại trang
  restoreInvoicesFromLocalStorage();

  $("#addInvoiceBtn").on("click", function () {
    if (tabCount >= maxTabs) {
      return;
    }

    tabCount++;
    const newTabId = `invoice${tabCount}`;
    const newTabContent = `
      <div id="${newTabId}" class="container tab-pane">
        <ul id="productsList${tabCount}" class="list-group">                 
        </ul>
      </div>
    `;

    $("#invoiceTabs .nav-item").removeClass("active");
    $("#invoiceTabs .tab-content .tab-pane").removeClass("active");

    $("#invoiceTabs").append(`
      <li class="nav-item">
        <a class="nav-link" data-toggle="tab" href="#${newTabId}">Hoá Đơn <span class="close-tab" data-index="${tabCount}">✕</span></a>
      </li>
    `);

    $("#invoiceTabContent").append(newTabContent);
    $('[data-toggle="tooltip"]').tooltip();

    // Lưu danh sách hoá đơn vào Local Storage sau khi thêm hoá đơn
    saveInvoicesToLocalStorage();

    // Kích hoạt hoá đơn vừa tạo
    $(`#${newTabId}`).addClass("active");
    $(`#invoiceTabs a[href="#${newTabId}"]`).parent().addClass("active");
  });

  $("#invoiceTabs").on("click", "a.nav-link", function () {
    $("#invoiceTabs .nav-item").removeClass("active");
    $(this).parent().addClass("active");
    $("#invoiceTabContent .tab-pane").removeClass("active");
    $($(this).attr("href")).addClass("active");
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
    if (confirm(`Bạn có muốn xoá hoá đơn không?`)) {
      $(`#invoice${tabIndex}`).remove();
      $(this).parent().parent().remove();
      tabCount--;

      // Lưu danh sách hoá đơn vào Local Storage sau khi xoá hoá đơn
      saveInvoicesToLocalStorage();
    }
  });

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

  // Gọi API và xử lý dữ liệu
  fetch("https://localhost:44328/api/Product/active")
    .then((response) => response.json())
    .then((data) => {
      // Lặp qua dữ liệu từ API và cập nhật bảng HTML
      const tableBody = document.querySelector("#product-table tbody");

      data.forEach((product) => {
        const row = document.createElement("tr");

        row.innerHTML = `
                    <td>${product.id}</td>
                    <td><img src="${product.productImages}" 
        }" class="product-image"></td>
                    <td>${product.name}</td>
                    <td>${product.discountRate}</td>
                    <td><span class="status-badge ${
                      product.status === "out-of-stock"
                        ? "out-of-stock"
                        : "in-stock"
                    }">${
          product.status === "out-of-stock" ? "Ngừng kinh doanh" : "Kinh doanh"
        }</span></td>
                    <td>
                        <button type="button" class="btn btn-warning">
                            Chọn
                        </button>
                    </td>
                `;

        tableBody.appendChild(row);
      });
    })
    .catch((error) => {
      console.error("Lỗi khi gọi API:", error);
    });
});
