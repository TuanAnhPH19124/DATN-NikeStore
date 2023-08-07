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
  $.ajax({
    url: "https://localhost:44328/api/Product/" + id,
    type: "GET",
    dataType: "json",
    success: function (data) {
      console.log(JSON.stringify(data));
      $('#name').val(data.name);
      $('#description').val(data.description);
      $('#retailPrice').val(data.retailPrice);
      $('#status').val(data.status);
    },
    error: function () {
      console.log("Error retrieving data.");
    }
  });
  $('#update-product-form').submit(function (event) {
    event.preventDefault()
    var formData = {
      id: id,
      name: $("#name").val(),
      description: $("#description").val(),
      retailPrice: $("#retailPrice").val(),
      colorId: $("#color-select").val(),
      sizeId: $("#size-select").val(),
    };
    $.ajax({
      url: "https://localhost:44328/api/Product/" + id,
      type: "PUT",
      data: JSON.stringify(formData),
      contentType: "application/json; charset=utf-8",
      dataType: "json",
      success: function (e) {

      },
    });
  });
});