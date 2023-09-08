const icon = document.querySelectorAll(".icon");
const text = document.querySelectorAll(".text");
const progress = document.querySelectorAll(".progres");
let index = 0;

function update() {
  icon[index].style.visibility = "visible";
  text[index].style.visibility = "visible";
  progress[index].classList.add("active");
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
        // Create an array to keep track of which indices are present
        const indicesPresent = [];

        data.orderStatuses.forEach((orderStatus) => {
          const index = orderStatus.status;

          // Add the index to the array of present indices
          indicesPresent.push(index);

          if (index === 4||index===3) {
            $("#confirm-outer-btn").hide()
            $("#cancel").hide()
            for (let i = 0; i < 5; i++) {
              progress[i].classList.add("active");
            }
          }
        });

        // Now, loop through the list items and hide those with missing indices
        const statusList = document.getElementById("status-list");
        const listItems = statusList.querySelectorAll("li");

        for (let i = 0; i < listItems.length; i++) {
          const listItem = listItems[i];
          const listItemIndex = i;
          index=i
          update()
          // If the index is not present in the array, hide the list item
          if (!indicesPresent.includes(listItemIndex)) {
            listItem.style.display = "none";
          }
        }
      console.log(data.orderStatuses[data.orderStatuses.length-1].status)
      //cập nhật trạng thái
      $("#update-status").click(function(event) {
        console.log(index)
        event.preventDefault()
        var currentDate = new Date()
        var iso8601DateString = currentDate.toISOString()
        var formData = {
            "orderId": id,
            "status": data.orderStatuses.length,
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
                  location.reload();
                },
                error: function () {
                },
            });
        });
        $("#cancel").click(function(event) {
          console.log(index)
          event.preventDefault()
          var currentDate = new Date()
          var iso8601DateString = currentDate.toISOString()
          var formData = {
              "orderId": id,
              "status": 4,
              "time": iso8601DateString, // Fill with ISO 8601 formatted timestamp
              "note": "Hủy"
          };
              $.ajax({
                  url: "https://localhost:44328/api/OrderStatus",
                  type: "POST",
                  data: JSON.stringify(formData),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (response) {
                    location.reload();
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

