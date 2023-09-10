// Set new default font family and font color to mimic Bootstrap's default styling
(Chart.defaults.global.defaultFontFamily = "Nunito"),
  '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = "#858796";

// Pie Chart Example
function fetchData1() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (error) {
    // Handle the error here, e.g., log it or perform some other action
    return 0;
  });
}

function fetchData2() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/GetLatestConfirmedOrders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (data) {
    return 0;
  });
}

function fetchData3() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/GetLatestPendingShipOrders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (data) {
    return 0;
  });
}

function fetchData4() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/GetLatestShippingOrders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (data) {
    return 0;
  });
}

function fetchData5() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/GetLatestDeliveredOrders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (data) {
    return 0;
  });
}

function fetchData6() {
  return new Promise(function (resolve, reject) {
    $.ajax({
      url: "https://localhost:44328/api/Orders/GetLatestCancelOrders",
      type: "GET",
      contentType: "application/json",
      success: function (data) {
        resolve(data.length);
      },
      error: function (error) {
        reject(error);
      },
    });
  }).catch(function (data) {
    return 0;
  });
}

// Example usage to fetch data from both endpoints concurrently
Promise.all([fetchData1(), fetchData2(),fetchData3(),fetchData4(),fetchData5(),fetchData6()])
  .then(function (results) {
    console.log("Tổng đơn hàng:", results[0]);
    console.log("Tổng đơn xác nhận:", results[1]);
    console.log("Tổng đơn chuẩn bị hàng:", results[2]);
    console.log("Tổng đơn đang vận chuyển:", results[3]);
    console.log("Tổng đơn thành công:", results[4]);
    console.log("Tổng đơn hủy:", results[5]);
    var confirmPer = Math.floor((results[1]/results[0])*100) 
    var preparePer = Math.floor((results[2]/results[0])*100)
    var shippingPer = Math.floor((results[3]/results[0])*100)
    var successPer = Math.floor((results[4]/results[0])*100)
    var cancelPer = Math.floor((results[5]/results[0])*100)
    console.log(confirmPer+preparePer+shippingPer+successPer+cancelPer)

    var ctx = document.getElementById("myPieChart");
    var myPieChart = new Chart(ctx, {
      type: "doughnut",
      data: {
        labels: [
          "Chờ xác nhận",
          "Chuẩn bị hàng",
          "Đang vận chuyển",
          "Thành công",
          "Đã hủy",
        ],
        datasets: [
          {
            data: [confirmPer, preparePer, shippingPer, successPer, cancelPer],
            backgroundColor: [
              "#28a745",
              "#ffc107",
              "#17a2b8",
              "#007bff",
              "#dc3545",
            ],
            hoverBorderColor: "rgba(234, 236, 244, 1)",
          },
        ],
      },
      options: {
        maintainAspectRatio: false,
        tooltips: {
          backgroundColor: "rgb(255,255,255)",
          bodyFontColor: "#858796",
          borderColor: "#dddfeb",
          borderWidth: 1,
          xPadding: 15,
          yPadding: 15,
          displayColors: false,
          caretPadding: 10,
        },
        legend: {
          display: false,
        },
        cutoutPercentage: 80,
      },
    });
  })
  .catch(function (error) {
    console.error("Error fetching data:", error);
  });
