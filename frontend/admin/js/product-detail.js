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

var option_category = [];
$.getJSON("https://localhost:44328/api/Categories", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_category.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#category-select").html(option_category.join(''));
});

var option_color = [];
$.getJSON("https://localhost:44328/api/Color/Get", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_color.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#color-select").html(option_color.join(''));
});

var option_size = [];
$.getJSON("https://localhost:44328/api/Size/Get", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_size.push('<option value="', result[i].id, '">', result[i].numberSize, '</option>');
  }
  $("#size-select").html(option_size.join(''));
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
    },
    error: function () {
      console.log("Error retrieving data.");
    }
  });
  // lay id cate
  $.ajax({
    url: "https://localhost:44328/api/CategoryProduct/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(JSON.stringify(data[0].categoryId));
      $('#category-select').val(data[0].categoryId);
    },
    error: function () {
      console.log("Error retrieving data.");
    }
  });
      // lay id color size và so luong
      $.ajax({
        url: "https://localhost:44328/api/Stock/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
          console.log(JSON.stringify(data));
        $('#unitInStock').val(data.unitInStock);
        $('#color-select').val(data.colorId);
        $('#size-select').val(data.sizeId);
        },
        error: function () {
          console.log("Error retrieving data.");
        }
      });

// submit
  $('#update-product-form').submit(function (event) {
    event.preventDefault()
    var formData = {
      id: id,
      name: $("#name").val(),
      description: $("#description").val(),
      retailPrice: $("#retailPrice").val(),
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

      },
    });
    // api delete categoryProduct
    $.ajax({
      url: "https://localhost:44328/api/CategoryProduct/" + id,
      type: "DELETE",
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (e) {

      },
    });
    //api add categoryProduct
    var categoryformData = {
      productId: id,
      categoryId: $("#category-select").val(),
    };
    $.ajax({
      url: "https://localhost:44328/api/CategoryProduct",
      type: "POST",
      data: JSON.stringify(categoryformData),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (e) {

      },
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
  });
});