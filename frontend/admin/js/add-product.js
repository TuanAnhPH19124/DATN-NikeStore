
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

var product = {
  "costPrice": 123,
  "retailPrice": 123,
  "description": "123",
  "status": 1,
  "brand": 1,
  "discountRate": 1,
  "soleId": 1,
  "materialId": 1,
  "name": "123423",
  "Colors": [

  ]
}

var selectedColor = -1;
document.addEventListener("DOMContentLoaded", function () {
  // Đoạn mã AJAX ở đây
});
// call api len datatable nhan vien
$(document).ready(function () {
  // call api them nhan vien
  $('#add-product-form').submit(function (event) {
    event.preventDefault()

    // var formData = new FormData();
    // formData.append("costPrice",231)
    // formData.append("retailPrice",231)
    // formData.append("description","12312")
    // formData.append("status",1)
    // formData.append("brand",1)
    // formData.append("discountRate",1)
    // formData.append("name","1231")
    // formData.append("materialId",1)
    // formData.append("colors","2")

    $.ajax({
      url: "https://localhost:44328/api/Product",
      type: "POST",
      data: product,
      processData: false,
      contentType: false,
      success: function (response) {
        localStorage.setItem("productId", response.id);
        console.log(response.id)
        //window.location.href = "/frontend/admin/product-detail.html";
      },
    });


  });
});

// $.validator.addMethod("compare2Price", function (value, element) {
//   var parts1 = Number($("#retailPrice").val());
//   var parts2 = Number($("#costPrice").val());
//   return parts1 > parts2

// });
// $("#add-product-form").validate({
//   rules: {
//       "name": {
//           required: true,
//       },
//       "retailPrice": {
//         required: true,
//     },
//     "costPrice": {
//       required: true,
//   },
//       "retailPrice": {
//         required: true,
//         compare2Price: true,
//     },
//   },
//   messages: {
//       "name": {
//           required: "Mời bạn nhập Tên sản phẩm",
//       },
//     "retailPrice": {
//       required: "Mời bạn nhập giá bán",
//       compare2Price: "Tiền nhập không được lớn hơn tiền bán",
//   },
//   "costPrice": {
//     required: "Mời bạn nhập giá nhập",
// },
//   },
// });

// upload nhiều ảnh
// Hàm chuyển đổi hình ảnh thành base64
const getBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

// Chờ tài liệu HTML được tải xong
document.addEventListener("DOMContentLoaded", () => {
  const fileList = []; // Danh sách hình ảnh đã tải lên

  const uploadList = document.querySelector(".upload-list");
  const uploadButton = document.querySelector(".upload-button");

  // Xử lý sự kiện khi nhấn nút đóng trong modal
  const handleCancel = () => {
    document.querySelector(".preview-modal").style.display = "none";
  };

  // Xử lý sự kiện khi xóa hình ảnh
  const handleDelete = (file, previewContainer) => {
    const index = fileList.indexOf(file);
    if (index > -1) {
      fileList.splice(index, 1);
      uploadList.removeChild(previewContainer);

      if (fileList.length < 6) {
        uploadButton.style.display = "flex";
      }
    }
  };

  // Lắng nghe sự kiện click nút đóng trong modal
  const modalCloseButton = document.querySelector(".modal-close-button");
  modalCloseButton.addEventListener("click", () => {
    handleCancel();
  });

  // Lắng nghe sự kiện click overlay để đóng modal
  const modalOverlay = document.querySelector(".modal-overlay");
  modalOverlay.addEventListener("click", () => {
    handleCancel();
  });

  // Ngăn sự kiện click trên nội dung modal lan sang nền
  const modalContent = document.querySelector(".modal-content");
  modalContent.addEventListener("click", (event) => {
    event.stopPropagation();
  });

  // Lắng nghe sự kiện click nút tải lên
  uploadButton.addEventListener("click", () => {
    const fileInput = document.getElementById("file-input");
    fileInput.click();
  });

  // Lắng nghe sự kiện thay đổi tập tin tải lên
  const fileInput = document.getElementById("file-input");
  fileInput.addEventListener("change", (event) => {
    const files = event.target.files;
    for (const file of files) {
      if (fileList.length < 6 && file.type.startsWith("image/")) {
        fileList.push(file);
        // ảnh thêm vào api
        const newImage = { file: file, setAsDefault: false };
        //  product.Colors[0].Images.push(newImage);

        // Hiển thị hình ảnh tải lên trong giao diện
        const previewContainer = document.createElement("div");
        previewContainer.className = "preview-container";
        const previewImage = document.createElement("img");
        previewImage.src = URL.createObjectURL(file);
        previewImage.alt = "Preview";
        previewImage.className = "preview-image";

        // Tạo nút xóa và xử lý sự kiện khi click
        const deleteButton = document.createElement("button");
        deleteButton.textContent = "x";
        deleteButton.className = "delete-button";
        deleteButton.addEventListener("click", () => {
          handleDelete(file, previewContainer);
        });

        previewContainer.appendChild(previewImage);
        previewContainer.appendChild(deleteButton);
        uploadList.appendChild(previewContainer);

        // Kiểm tra nếu danh sách hình ảnh đã đạt đến giới hạn
        if (fileList.length === 6) {
          uploadButton.style.display = "none"; // Ẩn nút "Upload"
        }
      } else {
        alert("Please select valid image files (up to 6 images).");
      }
    }
    fileInput.value = ""; // Reset file input
  });
});

function findIndexById(array, id) {
  for (var i = 0; i < array.length; i++) {
      if (array[i].id === id) {
          return i; // Trả về chỉ số khi tìm thấy phần tử có id tương ứng
      }
  }
  return -1; // Trả về -1 nếu không tìm thấy phần tử
}

// add color 
document.addEventListener("DOMContentLoaded", function () {
  var addColorButton = document.getElementById("addColorButton");
  var plusButtonContainer = document.getElementById("render-color");
  var selectedColorText = {};
  var selectedButtons = [];

  addColorButton.addEventListener("click", function () {
    if (selectedColorText === {} || selectedColorText === undefined) {
      return; // Ngăn việc thêm nút nếu màu chưa được chọn
    }

    if (selectedButtons.indexOf(selectedColorText.id) === -1) {
      var newButton = document.createElement("button");
      newButton.type = "button";
      newButton.className = 'btn btn-outline-dark';
      newButton.style.marginRight = "5%";
      newButton.id = selectedColorText.id;
      newButton.textContent = selectedColorText.text;
      newButton.addEventListener('click', function (e) {
        selectedColor = findIndexById(product.Colors, e.target.id);
        console.log(selectedColor);
        loadSizeE();
      })
      
      plusButtonContainer.parentNode.insertBefore(newButton, plusButtonContainer);

      plusButtonContainer.style.float = "left";
      product.Colors.push({ id: selectedColorText.id, Images: [], Sizes: [] });
      selectedColor = findIndexById(product.Colors, selectedColorText.id);
      loadSizeE();
      console.log(product);
      selectedButtons.push(selectedColorText);
      $('#exampleModalColor').modal('hide');

    }
    selectedColorText = {}; // Reset màu đã chọn

  });

  $('#exampleModalColor').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    selectedColorText = button.data('color');

    if (selectedButtons.indexOf(selectedColorText) !== -1) {
      selectedColorText = ""; // Reset màu đã chọn
    }
  });

  $('#exampleModalColor').on('hide.bs.modal', function () {
    selectedColorText = ""; // Reset màu đã chọn khi modal đóng
  });

  $.ajax({
    url: 'https://localhost:44328/api/Color/Get',
    method: 'GET',
    dataType: 'json',
    success: function (data) {
      var buttonContainer = $('#colorContainer');
      var addButton = buttonContainer.find('#add-now-btn'); // Get the "add-now-btn" button
      var buttonsToKeep = buttonContainer.find('.btn:not(#add-now-btn)'); // Get all buttons except "add-now-btn"

      buttonContainer.empty(); // Empty the container

      // Append the buttons you want to keep
      buttonsToKeep.each(function () {
        buttonContainer.append($(this));
      });

      data.forEach(function (item) {
        var button = $('<button></button>');
        button.attr('type', 'button');
        button.addClass('btn btn-outline-dark');
        button.css('margin-left', '3%');
        button.css('margin-top', '1%');
        button.text(item.name);
        button.attr('data-color', item.name);
        button.attr('id', item.id);
        button.click(function () {
          selectedColorText = { id: item.id, text: item.name };

        });
        buttonContainer.append(button);
      });

      buttonContainer.append(addButton); // Append the "add-now-btn" button back
    },
    error: function () {
      console.error('Error fetching data.');
    }
  });
  // add color nhanh
  $('#add-color-now').click(function () {
    // thêm nút ban đầu
    var buttonContainer = $('#colorContainer');
    var addButton = buttonContainer.find('#add-now-btn');
    buttonContainer.empty();
    buttonContainer.append(addButton);
    var formData = {
      name: $("#color-name").val(),
    };
    if (confirm(`Bạn có muốn thêm màu ${formData.name}?`)) {
      $.ajax({
        url: "https://localhost:44328/api/Color",
        type: "POST",
        data: JSON.stringify(formData),
        contentType: "application/json",
        success: function (response) {
          $('#exampleModalAddColor').modal('hide');
          $.ajax({
            url: 'https://localhost:44328/api/Color/Get',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
              var buttonContainer = $('#colorContainer');
              var addButton = buttonContainer.find('#add-now-btn'); // Get the "add-now-btn" button
              var buttonsToKeep = buttonContainer.find('.btn:not(#add-now-btn)'); // Get all buttons except "add-now-btn"

              buttonContainer.empty(); // Empty the container

              // Append the buttons you want to keep
              buttonsToKeep.each(function () {
                buttonContainer.append($(this));
              });

              data.forEach(function (item) {
                var button = $('<button></button>');
                button.attr('type', 'button');
                button.addClass('btn btn-outline-dark');
                button.css('margin-left', '3%');
                button.css('margin-top', '1%');
                button.text(item.name);
                button.attr('data-color', item.name);
                button.attr('id', item.id);
                button.click(function () {
                  selectedColorText = item.name;
                });
                buttonContainer.append(button);
              });
              buttonContainer.append(addButton); // Append the "add-now-btn" button back
              $("#color-name").val("")
            },
            error: function () {
              console.error('Error fetching data.');
            }
          });
          $('#exampleModalColor').modal('show');
        },
      });
    }
  });
});

function loadSizeE() {
  var plusButtonContainer = document.getElementById("render-size");
  var containerParent = plusButtonContainer.parentNode;
  // Xóa tất cả các phần tử con trong containerParent
  while (containerParent.firstChild !== plusButtonContainer) {
    containerParent.removeChild(containerParent.firstChild);
  }

  if (product.Colors.length !== 0) {
    if (product.Colors[selectedColor].Sizes.length !== 0) {
      product.Colors[selectedColor].Sizes.forEach(element => {
        var newButton = document.createElement("button");
        newButton.type = "button";
        newButton.className = 'btn btn-outline-dark';
        newButton.style.marginRight = "5%";
        newButton.textContent = element.numberSize;
        plusButtonContainer.parentNode.insertBefore(newButton, plusButtonContainer);
      });
    }
  }
}


// add size 
document.addEventListener("DOMContentLoaded", function () {
  var addColorButton = document.getElementById("addSizeButton");
  var plusButtonContainer = document.getElementById("render-size");
  var selectedColorText = {};
  var selectedButtons = [];

 

  addColorButton.addEventListener("click", function () {
    if (selectedColorText === {} || selectedColorText === undefined) {
      return; // Ngăn việc thêm nút nếu màu chưa được chọn
    }

    if (selectedButtons.indexOf(selectedColorText) === -1) {
    
      product.Colors[selectedColor].Sizes.push(selectedColorText);
      loadSizeE();
      $('#exampleModalSize').modal('hide');
    }
    plusButtonContainer.style.float = "left";
    selectedColorText = ""; // Reset màu đã chọn
  });

  $('#exampleModalSize').on('show.bs.modal', function (event) {
    if (selectedColor === -1) {
      alert('Bạn phải chọn màu trước');
      // Ngăn modal từ việc hiển thị bằng cách gọi e.preventDefault()
      event.preventDefault();
    }
    var button = $(event.relatedTarget);
    selectedColorText = button.data('color');

    if (selectedButtons.indexOf(selectedColorText) !== -1) {
      selectedColorText = ""; // Reset màu đã chọn
    }

  });

  $('#exampleModalSize').on('hide.bs.modal', function () {
    selectedColorText = {}; // Reset màu đã chọn khi modal đóng
  });

  $.ajax({
    url: 'https://localhost:44328/api/Size/Get',
    method: 'GET',
    dataType: 'json',
    success: function (data) {
      var buttonContainer = $('#sizeContainer');
      var addButton = buttonContainer.find('#add-size-now'); // Get the "add-now-btn" button
      var buttonsToKeep = buttonContainer.find('.btn:not(#add-size-now)'); // Get all buttons except "add-now-btn"

      buttonContainer.empty(); // Empty the container

      // Append the buttons you want to keep
      buttonsToKeep.each(function () {
        buttonContainer.append($(this));
      });

      data.forEach(function (item) {
        var button = $('<button></button>');
        button.attr('type', 'button');
        button.addClass('btn btn-outline-dark');
        button.css('margin-left', '3%');
        button.css('margin-top', '1%');
        button.text(item.numberSize);
        button.attr('data-color', item.numberSize);
        button.click(function () {
          selectedColorText = { id: item.id, numberSize: item.numberSize, unitInStock: 0 };
        });
        buttonContainer.append(button);
      });

      buttonContainer.append(addButton); // Append the "add-now-btn" button back
    },
    error: function () {
      console.error('Error fetching data.');
    }
  });
  // add color nhanh
  $('#add-size-now-btn').click(function () {
    // thêm nút ban đầu
    var buttonContainer = $('#sizeContainer');
    var addButton = buttonContainer.find('#add-size-now');
    buttonContainer.empty();
    buttonContainer.append(addButton);
    var formData = {
      numberSize: $("#numberSize").val(),
    };
    if (confirm(`Bạn có muốn thêm màu ${formData.numberSize}?`)) {
      $.ajax({
        url: "https://localhost:44328/api/Size",
        type: "POST",
        data: JSON.stringify(formData),
        contentType: "application/json",
        success: function (response) {
          $('#exampleModalAddSize').modal('hide');
          $.ajax({
            url: 'https://localhost:44328/api/Size/Get',
            method: 'GET',
            dataType: 'json',
            success: function (data) {
              var buttonContainer = $('#sizeContainer');
              var addButton = buttonContainer.find('#add-size-now'); // Get the "add-now-btn" button
              var buttonsToKeep = buttonContainer.find('.btn:not(#add-size-now)'); // Get all buttons except "add-now-btn"

              buttonContainer.empty(); // Empty the container

              // Append the buttons you want to keep
              buttonsToKeep.each(function () {
                buttonContainer.append($(this));
              });

              data.forEach(function (item) {
                var button = $('<button></button>');
                button.attr('type', 'button');
                button.addClass('btn btn-outline-dark');
                button.css('margin-left', '3%');
                button.css('margin-top', '1%');
                button.text(item.numberSize);
                button.attr('data-color', item.numberSize);
                button.click(function () {
                  selectedColorText = item.numberSize;
                });
                buttonContainer.append(button);
              });

              buttonContainer.append(addButton); // Append the "add-now-btn" button back
              $("#numberSize").val("")
            },
            error: function () {
              console.error('Error fetching data.');
            }
          });

          $('#exampleModalSize').modal('show');
        },
      });
    }
  });
});










