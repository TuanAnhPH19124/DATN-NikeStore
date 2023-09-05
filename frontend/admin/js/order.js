const icon = document.querySelectorAll(".icon");
const text = document.querySelectorAll(".text");
const progress = document.querySelectorAll(".progres");
let index = 0;

function update() {
  console.log(index)
  icon[index].style.visibility = "visible";
  text[index].style.visibility = "visible";
  progress[index].classList.add("active");
  index++;
}

const id = localStorage.getItem("billId");
console.log(id);
$(document).ready(function () {
  $.ajax({
    url: "https://localhost:44328/api/Orders/Get/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      $("#customerName").val(data.customerName);
      $("#phoneNumber").val(data.phoneNumber);
      $("#address").val(data.address);
      $("#amount").val(
        Intl.NumberFormat("vi-VN", {
          style: "currency",
          currency: "VND",
        }).format(data.amount)
      );

      var dateObj1 = new Date(data.dateCreated);
      var day1 = dateObj1.getUTCDate();
      var month1 = dateObj1.getUTCMonth() + 1;
      var year1 = dateObj1.getUTCFullYear();
      var formattedDate = `${day1}/${month1}/${year1}`;
      $("#dateOfBirth").val(formattedDate);
      $("#note").val(data.note);

      $.ajax({
        url: "https://localhost:44328/api/AppUser/Get/" + data.employeeId,
        type: "GET",
        dataType: "json",
        success: function (data) {
          $("#employeeId").val(data.fullName);
        },
        error: function () {
          console.log("Error retrieving data.");
        },
      });

      $.ajax({
        url: "https://localhost:44328/api/Voucher/Get/" + data.voucherId,
        type: "GET",
        dataType: "json",
        success: function (data) {
          console.log(JSON.stringify(data));
          $("#voucherId").val(data.code);
        },
        error: function () {
          console.log("Error retrieving data.");
        },
      });
      var tableBody = $("#orderItemsTable tbody");
      tableBody.empty(); // Clear the table body before rendering

      var count = 1; // Initialize the count

      data.orderItems.forEach(function (item) {
        $.ajax({
          url: "https://localhost:44328/api/Product/" + item.productId,
          type: "GET",
          dataType: "json",
          success: function (productData) {
            // Thực hiện ajax call để lấy thông tin về màu sắc (color)
            $.ajax({
              url: "https://localhost:44328/api/Color/Get/" + item.colorId,
              type: "GET",
              dataType: "json",
              success: function (colorData) {
                // Thực hiện ajax call để lấy thông tin về kích thước (size)
                $.ajax({
                  url: "https://localhost:44328/api/Size/Get/" + item.sizeId,
                  type: "GET",
                  dataType: "json",
                  success: function (sizeData) {
                    var row =
                      "<tr>" +
                      "<td>" +
                      count +
                      "</td>" +
                      "<td>" +
                      productData.name +
                      "</td>" +
                      "<td>" +
                      colorData.name +
                      "</td>" +
                      "<td>" +
                      sizeData.numberSize +
                      "</td>" + // Sử dụng giá trị đã cập nhật
                      "<td>" +
                      item.quantity +
                      "</td>" +
                      "<td>" +
                      Intl.NumberFormat("vi-VN", {
                        style: "currency",
                        currency: "VND",
                      }).format(item.unitPrice * item.quantity) +
                      "</td>" +
                      "</tr>";

                    tableBody.append(row);

                    count++; // Increment the count for the next item
                  },
                  error: function () {
                    console.log("Error retrieving size data.");
                  },
                });
              },
              error: function () {
                console.log("Error retrieving color data.");
              },
            });
          },
          error: function () {
            console.log("Error retrieving product data.");
          },
        });
      });

      console.log(data.orderStatuses.length);
      const itemsWithStatus3 = data.orderStatuses.filter(
        (item) => item.status === 3
      );
      console.log(itemsWithStatus3);
      if (itemsWithStatus3.length != 0&&data.orderStatuses.length==1) {
        console.log("Thành công");
        const listItems = document.querySelectorAll("#status-list li");

        // Loop through each list item
        listItems.forEach((item) => {
          const textContent = item.querySelector(".text").textContent;
        
          // Check if the text content is not "Thành công" or "Đã thanh toán"
          if (textContent !== "Thành công" && textContent !== "Đã thanh toán") {
            // Hide the list item
            item.style.display = "none";
        
            // Remove ::after pseudo-element
            const afterElement = window.getComputedStyle(item, '::after');
            if (afterElement) {
              item.style.content = "none";
            }
          }
        });
        index = 0
        update()
        index = 3
        update()
        $("#confirm-outer-btn").hide()
        $("#cancel").hide()
      } else {
        console.log("Thanh toán chưa xong");
        data.orderStatuses.forEach((orderStatus) => {
          index = orderStatus.status
          update()
      })}
      console.log(data.orderStatuses[data.orderStatuses.length-1].status)
      $("#update-status").click(function(event) {
        event.preventDefault()
        var currentDate = new Date();

        // Format the date in ISO 8601 format
        var iso8601DateString = currentDate.toISOString();
    
        // Now, you can use iso8601DateString to fill the time field in your formData
        var formData = {
            "orderId": id,
            "status": index - 1,
            "time": iso8601DateString, // Fill with ISO 8601 formatted timestamp
            "note": $("#note").val()
        };
            $.ajax({
                url: "https://localhost:44328/api/OrderStatus",
                type: "POST",
                data: JSON.stringify(formData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                  //location.reload();
                },
                error: function () {
                },
            });
        });
    },
    error: function () {
      console.log("Error retrieving data.");
    },
  });

});
