const icon = document.querySelectorAll(".icon")
const text = document.querySelectorAll(".text")
const progress = document.querySelectorAll(".progres")
let index = 0;

function update(){
    icon[index].style.visibility="visible";
    text[index].style.visibility="visible";
    progress[index].classList.add("active");
    index++
}
const id = localStorage.getItem("billId");
console.log(id)
$(document).ready(function () {
  $.ajax({
      url: "https://localhost:44328/api/Orders/Get/" + id,
      type: "GET",
      dataType: "json",
      success: function (data) {
          console.log((data.orderStatuses));
          $("#customerName").val(data.customerName)
          $("#phoneNumber").val(data.phoneNumber)
          $("#address").val(data.address)
          $("#amount").val(Intl.NumberFormat("vi-VN", {
            style: "currency",
            currency: "VND",
          }).format(data.amount))

          var dateObj1 = new Date(data.dateCreated);
          var day1 = dateObj1.getUTCDate();
          var month1 = dateObj1.getUTCMonth() + 1;
          var year1 = dateObj1.getUTCFullYear();
          var formattedDate = `${day1}/${month1}/${year1}`;
          $('#dateOfBirth').val(formattedDate);
          $('#note').val(data.note);

          $.ajax({
            url: "https://localhost:44328/api/AppUser/Get/" + data.employeeId,
            type: "GET",
            dataType: "json",
            success: function (data) {
            $("#employeeId").val(data.fullName)
            },
            error: function () {
                console.log("Error retrieving data.");
            }
        });

        $.ajax({
          url: "https://localhost:44328/api/Voucher/Get/" + data.voucherId,
          type: "GET",
          dataType: "json",
          success: function (data) {
            console.log(JSON.stringify(data));
            $("#voucherId").val(data.code)
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
                      var row = "<tr>" +
                          "<td>" + count + "</td>" +
                          "<td>" + productData.name + "</td>" +
                          "<td>" + colorData.name + "</td>" +
                          "<td>" + sizeData.numberSize + "</td>" + // Sử dụng giá trị đã cập nhật
                          "<td>" + item.quantity + "</td>" +
                          "<td>" + Intl.NumberFormat("vi-VN", {
                            style: "currency",
                            currency: "VND",
                          }).format( item.unitPrice * item.quantity )+ "</td>" +
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
        
      },
      error: function () {
          console.log("Error retrieving data.");
      }
  });
  
});
