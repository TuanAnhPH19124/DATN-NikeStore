const id = localStorage.getItem("productId");
console.log(id)

var loadFile = function (event) {
  var image = document.getElementById('output');
  image.src = URL.createObjectURL(event.target.files[0]);
};
function increment() {
  const input = document.getElementById("number");
  let newValue = parseInt(input.value) + 1;
  if (isNaN(newValue)) {
    newValue = 0;
  }
  input.value = newValue;
}

function decrement() {
  const input = document.getElementById("number");
  let newValue = parseInt(input.value) - 1;
  if (newValue < 0 || isNaN(newValue)) {
    newValue = 0;
  }
  input.value = newValue;
}
// call api for multiple input
document.addEventListener('DOMContentLoaded', function () {
  var selectElement = document.getElementById('category-select');
  fetch('https://localhost:44328/api/Categories')
      .then(response => response.json())
      .then(data => {
          console.log(data);
          data.forEach(function (country) {
              var optionElement = document.createElement('option');
              optionElement.value = country.id; 
              optionElement.textContent = country.name;
              selectElement.appendChild(optionElement);
          });
          new MultiSelectTag('category-select')
      })
      .catch(error => {
          console.error('Error fetching data:', error);
      });
});
document.addEventListener('DOMContentLoaded', function () {
  var selectElement = document.getElementById('color-select');
  fetch('https://localhost:44328/api/Color/Get')
      .then(response => response.json())
      .then(data => {
          console.log(data);
          data.forEach(function (country) {
              var optionElement = document.createElement('option');
              optionElement.value = country.id;
              optionElement.textContent = country.name;
              selectElement.appendChild(optionElement);
          });
          new MultiSelectTag('color-select')  // id
      })
      .catch(error => {
          console.error('Error fetching data:', error);
      });
});
document.addEventListener('DOMContentLoaded', function () {
  var selectElement = document.getElementById('size-select');
  fetch('https://localhost:44328/api/Size/Get')
      .then(response => response.json())
      .then(data => {
          console.log(data);
          data.forEach(function (country) {
              var optionElement = document.createElement('option');
              optionElement.value = country.id;
              optionElement.textContent = country.numberSize;
              selectElement.appendChild(optionElement);
          });
          new MultiSelectTag('size-select')
      })
      .catch(error => {
          console.error('Error fetching data:', error);
      });
});

$(document).ready(function () {
  // hien thi product
  $.ajax({
    url: "https://localhost:44328/api/Product/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(JSON.stringify(data));
      $('#name').val(data.name);
      $('#description').val(data.description);
      $('#retailPrice').val(data.retailPrice);
      $('#costPrice').val(data.costPrice);
      $('#status').val(data.status);
      $('#output').attr('src', `/backend/.NET/Webapi/wwwroot/Images/${id}.jpg`);
    },
    error: function () {
      console.log("Error retrieving data.");
    }
  });
  // lay id cate và stock
  $.ajax({
    url: "https://localhost:44328/api/CategoryProduct/" + id,
    type: "GET",
    dataType: "json"
  })
  .done(function(data) {
    console.log(JSON.stringify(data[0].categoryId));
    $('#category-select').val(data[0].categoryId);
  })
  .fail(function() {
    console.log("Error retrieving data for category.");
  })
  .always(function() {
    $.ajax({
      url: "https://localhost:44328/api/Stock/" + id,
      type: "GET",
      dataType: "json"
    })
    .done(function(data) {
      console.log(JSON.stringify(data));
      $('#unitInStock').val(data.unitInStock);
      $('#color-select').val(data.colorId);
      $('#size-select').val(data.sizeId);
    })
    .fail(function() {
      console.log("Error retrieving data for stock.");
    });
  });

// submit
  $('#update-product-form').submit(function (event) {
    event.preventDefault()
    var formData = {
      id: id,
      name: $("#name").val(),
      description: $("#description").val(),
      retailPrice: $("#retailPrice").val(),
      costPrice: $("#costPrice").val(),
      colorId: $("#color-select").val(),
      sizeId: $("#size-select").val(),
      status: Number($("#status").val()),

    };
    //api update product
    $.ajax({
      url: "https://localhost:44328/api/Product/" + id,
      type: "PUT",
      data: JSON.stringify(formData),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (e) {
        window.location.href = `/frontend/admin/product-page.html`;
      },
    });
    // category product
    $.ajax({
      url: "https://localhost:44328/api/CategoryProduct/" + id,
      type: "DELETE",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
    })
    .then(function() {
      var categoryformData = {
        productId: id,
        categoryId: $("#category-select").val(),
      };
      
      return $.ajax({
        url: "https://localhost:44328/api/CategoryProduct",
        type: "POST",
        data: JSON.stringify(categoryformData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
      })
    })
    .then(function(response) {
    })
    .catch(function(error) {
    });
        // api delete Stock
        $.ajax({
          url: "https://localhost:44328/api/Stock/" + id,
          type: "DELETE",
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (e) {
    
          },
        });
        //api add Stock
        var stockformData = {
          productId: id,
          colorId: $("#color-select").val(),
          sizeId: $("#size-select").val(),
          unitInStock: Number($("#unitInStock").val()),
        };
        $.ajax({
          url: "https://localhost:44328/api/Stock",
          type: "POST",
          data: JSON.stringify(stockformData),
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (response) {
          },
        });
        // upload anh
        var fileInput = document.getElementById('image');
        var formData = new FormData();
        formData.append('image', fileInput.files[0]);
          $.ajax({  
            url: 'https://localhost:44328/api/ProductImg/'+id, 
            method: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function(response){
              console.log('Image uploaded successfully!');
            },
            error: function(xhr, status, error){
              console.error(error);
            }
          });
  });
});

$.validator.addMethod("compare2Price", function (value, element) {
  var parts1 = Number($("#retailPrice").val());
  var parts2 = Number($("#costPrice").val());
  return parts1 > parts2

});
$("#update-product-form").validate({
  rules: {
      "name": {
          required: true,
      },
      "retailPrice": {
        required: true,
    },
    "costPrice": {
      required: true,
  },
      "retailPrice": {
        required: true,
        compare2Price: true,
    },
  },
  messages: {
      "name": {
          required: "Mời bạn nhập Tên sản phẩm",
      },
    "retailPrice": {
      required: "Mời bạn nhập giá bán",
      compare2Price: "Tiền nhập không được lớn hơn tiền bán",
  },
  "costPrice": {
    required: "Mời bạn nhập giá nhập",
},
  },
});
