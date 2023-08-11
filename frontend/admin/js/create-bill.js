function closeModal(modalId) {
    $(modalId).modal('hide');
  }
  function selectButton(button) {
    // Deselect all buttons in the group
    var buttons = button.parentElement.children;
    for (var i = 0; i < buttons.length; i++) {
      buttons[i].classList.remove("selected");
    }

    // Select the clicked button
    button.classList.add("selected");
  }
  function increment() {
    const input = document.getElementById("number");
    let newValue = parseInt(input.value) + 1;
    if(isNaN(newValue)){
      newValue=0;
    }
    input.value = newValue;
  }

  function decrement() {
    const input = document.getElementById("number");
    let newValue = parseInt(input.value) - 1;
    if(newValue<0||isNaN(newValue)){
      newValue=0;
    }
    input.value = newValue;
  }
  $('#productModal').on('shown.bs.modal', function() {
    $(this).find('#productModal').focus();
 });
 function myFunction() {
  var x = document.getElementById('customer-info');
  console.log(x.style.visibility)
  if (x.style.visibility == 'hidden') {
    x.style.visibility = 'visible';
  } else {
    x.style.visibility = 'hidden';
  }
}
//call api
var option_voucher = [];
$.getJSON("https://localhost:44328/api/Voucher/Get", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_voucher.push('<option value="', result[i].id, '">', result[i].code, '</option>');
  }
  $("#voucher-select").html(option_voucher.join(''));
});
$('#voucher-select').on('change', function() {
  const id = $(this).val();
  $.ajax({
    url: "https://localhost:44328/api/Voucher/Get/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
        console.log(JSON.stringify(data));
        console.log($("#total").text())
        if($("#total").text()!="0 đ"){
          var discount_price = $('#discount-price').text(((data.value*$("#total").text())/100));
          $('#sum').text((($("#total").text())-discount_price[0].innerHTML)+" đ");
          
          document.getElementById("discount-price").innerHTML ="- " +discount_price[0].innerHTML+' đ';
        }

    },
    error: function () {
        console.log("Error retrieving data.");
    }
});
});

$('#create-bill').click(function (event) {
  event.preventDefault()
  var formData = {
      address: $("#address").val(),
      phoneNumber: $("#phoneNumber").val(),
      note: $("#note").val(),
      customerName: $("#customerName").val(),
      voucherId: $("#voucher-select").val(),
  };
  var formData = {
    "address": $("#address").val(),
    "phoneNumber": $("#phoneNumber").val(),
    "note": $("#note").val(),
    "paymentMethod": 0,
    "amount": 0,
    "customerName":  $("#customerName").val(),
    "voucherId": $("#voucher-select").val(),
    "orderItems": JSON.parse(localStorage.getItem("cart")),
  };
  $.ajax({
      url: "https://localhost:44328/api/Orders/pay",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
         // window.location.href = `/frontend/admin/order.html`;
      },
  });
});
$(document).ready(function () {
  $('#productData').DataTable({
      "ajax": {
          "url": "https://localhost:44328/api/Product",
          "dataType": "json",
          "dataSrc": ""
      },
      "columns": [
          {
              "data": 'id', 'title': 'STT', render: function (data, type, row, meta) {
                  return meta.row + 1;
              }
          },
          { "data": 'id', 'title': 'Ảnh',
          "render": function (data, type, row) {
                  return `<img src="/backend/.NET/Webapi/wwwroot/Images/${data}.jpg" alt="" style="border-radius: 10%;" width=120px height=110px>`;
          }},
          { "data": 'name', 'title': 'Tên sản phẩm' },
          { "data": 'costPrice', 'title': 'Giá nhập',
          "render": function (data, type, row) {
                  return data+" đ";
          } },
          { "data": 'retailPrice', 'title': 'Giá bán' ,
          "render": function (data, type, row) {
            return data+" đ";
    }
        },
          {
              "data": 'status', "title": "Trạng thái",
              "render": function (data, type, row) {
                  if (data == 1) {
                      return '<span class="badge badge-pill badge-primary" style="padding:10px;">Kinh doanh</span>';
                  } else {
                      return '<span class="badge badge-pill badge-danger" style="padding:10px;">Ngừng kinh doanh</span>';
                  }
              }
          },
          {
              "title": "Thao tác",
              "render": function () {
                  return '<td><button type="button" class="btn btn-outline-primary" data-toggle="modal" data-target="#productModal">Chọn</button></td>';
              }
          },
      ],
      rowCallback: function(row, data) {
          $(row).find('td').css('vertical-align', 'middle');
        },
        "language": {
          "sInfo": "Hiển thị _START_ đến _END_ của _TOTAL_ bản ghi",
          "lengthMenu": "Hiển thị _MENU_ bản ghi",
          "sSearch": "Tìm kiếm:",
          "sInfoFiltered": "(lọc từ _MAX_ bản ghi)",
          "sInfoEmpty": "Hiển thị 0 đến 0 trong 0 bản ghi",
          "sZeroRecords": "Không có data cần tìm",
          "sEmptyTable": "Không có data trong bảng",
          "oPaginate": {
              "sFirst": "Đầu",
              "sLast": "Cuối",
              "sNext": "Tiếp",
              "sPrevious": "Trước"
          },
        }
  });
});
$('#productData tbody').on('click', 'tr', function (e) {
  let selectedProduct = $('#productData').DataTable().row(this).data().id;
  if (selectedProduct !== null) {
      localStorage.setItem("selectedProduct", selectedProduct);
      const id = localStorage.getItem("selectedProduct");
      console.log(id) 
      $.ajax({
        url: "https://localhost:44328/api/Product/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
          console.log(JSON.stringify(data));
          $('#name').text(data.name);
          $('#description').val(data.description);
          $('#retailPrice').text(data.retailPrice+" đ");
          $('#costPrice').text(data.costPrice+ " đ");
          $('#status').val(data.status);
          $('#output').attr('src', `/backend/.NET/Webapi/wwwroot/Images/${id}.jpg`);
          
        },
        error: function () {
          console.log("Error retrieving data.");
        }
      });
  }
});
function addToCart(){
  const id = localStorage.getItem("selectedProduct");
  const oldCart = localStorage.getItem("cart");
  $('#productModal').modal('hide');
  const obj = JSON.parse(oldCart);

  const arr = [];
  for (const key in obj) {
    arr.push(obj[key]);
  }
  arr.push({
    "productId": id,
    "unitPrice": $("#retailPrice").text(),
    "quantity": $("#number").val(),
    "name": $("#name").text(),
  });
  var cartJson = JSON.stringify(arr)
  localStorage.setItem("cart",cartJson)
  // đẩy ra html
  pushHTML();
  // Đăng ký sự kiện click cho các nút xóa sản phẩm
  document.addEventListener('click', function(event) {
      if (event.target && event.target.matches('.btn-danger')) {
          var productId = event.target.getAttribute('data-product-id');
          deleteItem(productId);
      }
  });



}
function deleteItem(id){
  console.log(id)
  const oldCart = localStorage.getItem("cart");
  const obj = JSON.parse(oldCart);
  console.log(obj)
  
  const productIdToDelete = id;
  
  const newArray = obj.filter(item => item.productId !== productIdToDelete);
  localStorage.removeItem('cart');
  var cartJson = JSON.stringify(newArray)
  localStorage.setItem("cart",cartJson)
  pushHTML();
  console.log(newArray);
}
function pushHTML(){
  const data = JSON.parse(localStorage.getItem("cart"));
  console.log(data)
  var html = '';
  var totalPrice=0;
  var html = '';
  
  for (var i = 0; i < data.length; i++) {
      html += '<tr>';
      html += `<td><img src="https://localhost:44328/Images/${data[i].productId}.jpg" alt="" style="border-radius: 10%;" width=120px height=110px>  </td>`;
      html += `<td style="vertical-align: middle;">${data[i].name}</td>`;
      html += '<td style="vertical-align: middle;">' + data[i].quantity+" đôi" + '</td>';
      html += '<td style="vertical-align: middle;">' + (Number(data[i].quantity) * Number(data[i].unitPrice)) +" đ" + '</td>';
      html += `<td style="vertical-align: middle;"><button class="btn btn-danger" id="btn${data[i].productId}" data-product-id="${data[i].productId}"><i class="fa fa-times" aria-hidden="true"></i></button></td>`;
      html += '</tr>';
      totalPrice += data[i].quantity * data[i].unitPrice;
  }
  
  var total = `<tr>
    <th scope="row"></th>
    <td></td>
    <td>Tổng tiền:</td>
    <td>${totalPrice+" đ"}</td>
  </tr>`
  $('#myTable tbody').html(html); 
  $('tfoot').html(total); 
  $('#total').html(totalPrice);
}