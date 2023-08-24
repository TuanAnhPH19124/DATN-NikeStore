
var option_category = [];
$.getJSON("https://localhost:44328/api/Categories", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_category.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#category-select").html(option_category.join(''));
});

$('#add-category-now').click(function () {
  var formData = {
    name: $("#category-name").val(),
  };

  if (confirm(`Bạn có muốn thêm danh mục ${formData.name} không?`)) {
    $.ajax({
      url: "https://localhost:44328/api/Categories",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        $('#success').toast('show');
        $('#add-category').modal('hide');
        var option_category = [];
      $.getJSON("https://localhost:44328/api/Categories", function (result) {
        for (var i = 0; i < result.length; i++) {
          option_category.push('<option value="', result[i].id, '">', result[i].name, '</option>');
        }
        $("#category-select").html(option_category.join(''));
      });
      },

    });
  }
});


var option_sole = [];
$.getJSON("https://localhost:44328/api/Sole", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_sole.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#sole-select").html(option_sole.join(''));
});

$('#add-material-now').click(function () {
  var formData = {
    name: $("#material-name").val(),
    description: "",
  };

  if (confirm(`Bạn có muốn thêm chất liệu ${formData.name} không?`)) {
    $.ajax({
      url: "https://localhost:44328/api/Material",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        $('#success').toast('show');
        $('#add-material').modal('hide');
        var option_material = [];
$.getJSON("https://localhost:44328/api/Material", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_material.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#material-select").html(option_material.join(''));
});
      },
    });
  }
});

var option_material = [];
$.getJSON("https://localhost:44328/api/Material", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_material.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#material-select").html(option_material.join(''));
});

$('#add-sole-now').click(function () {
  var formData = {
    name: $("#sole-name").val(),
    description: "",
  };

  if (confirm(`Bạn có muốn thêm đế ${formData.name} không?`)) {
    $.ajax({
      url: "https://localhost:44328/api/Sole",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        $('#success').toast('show');
        $('#add-sole').modal('hide');
        var option_sole = [];
$.getJSON("https://localhost:44328/api/Sole", function (result) {
  for (var i = 0; i < result.length; i++) {
    option_sole.push('<option value="', result[i].id, '">', result[i].name, '</option>');
  }
  $("#sole-select").html(option_sole.join(''));
});
      },
    });
  }
});

var product = {
  "retailPrice": 0,
  "description": "",
  "status": 1,
  "brand": 1,
  "discountRate": 0,
  "soleId": 0,
  "materialId": 0,
  "name": "",
  "Categories": [],
  "Colors": []
}

var selectedColor = -1;

function objectToFormData(obj) {
  var formData = new FormData();

  for (var key in obj) {
    if (obj.hasOwnProperty(key)) {
      formData.append(key, obj[key]);
    }
  }

  return formData;
}
// call api len datatable nhan vien
$(document).ready(function () {
  
  // call api them nhan vien
  $('#add-product-form').submit(function (event) {
    event.preventDefault()
  
    let productFormData = new FormData();
    productFormData.append('name', $("#name").val());
    productFormData.append('description', $("#description").val());
    productFormData.append('retailPrice', $("#retailPrice").val());
    productFormData.append('discountRate', $("#discountRate").val());
    productFormData.append('soleId', $("#sole-select").val());
    productFormData.append('materialId', $("#material-select").val());
    productFormData.append('status', 1);
    productFormData.append('brand', 1);
    
    if (product.Colors.length !== 0){
      for (let i = 0; i < product.Colors.length; i++) {
        productFormData.append(`colors[${i}].id`, product.Colors[i].id);
        for (let j = 0; j < product.Colors[i].Images.length; j++) {
          productFormData.append(`colors[${i}].images[${j}].image`, product.Colors[i].Images[j].file);
          productFormData.append(`colors[${i}].images[${j}].setAsDefault`, product.Colors[i].Images[j].setAsDefault);
        }
        for (let y = 0; y < product.Colors[i].Sizes.length; y++) {
          productFormData.append(`colors[${i}].sizes[${y}].id`, product.Colors[i].Sizes[y].id);
          productFormData.append(`colors[${i}].sizes[${y}].unitInStock`, product.Colors[i].Sizes[y].unitInStock);
        }
      }
      
    }

    var categories = $("#category-select").val();
    if (categories.length !== 0){
      for (let i = 0; i < categories.length; i++) {
        productFormData.append(`categories[${i}].id`, categories[i]);
      }
    }
    
    // console.log($("#category-select").val());
    for (var pair of productFormData.entries()) {
      console.log(pair[0] + ': ' + pair[1]);
    }
    
    if (confirm(`Bạn có muốn thêm sản phẩm này không?`)) {
      $.ajax({
        url: "https://localhost:44328/api/Product",
        type: "POST",
        data: productFormData,
        processData: false,
        contentType: false,
        success: function (response) {
          // localStorage.setItem("productId", response.id);
          // console.log(response.id)
          $('#success').toast('show');
          //window.location.href = "/frontend/admin/product-detail.html";
          var form = $("#add-product-form")[0]; 
          form.reset();
        },
        error: function (response){
          $('#fail').toast('show');
          
        }
      });
  } else {
      return
  }



  });
});

$("#add-product-form").validate({
  rules: {
      "name": {
          required: true,
      },
      "description": {
        required: true,
    },
    "retailPrice": {
      required: true,
  },
  "discountRate": {
    required: true,
},
  },
  messages: {
      "name": {
          required: "Mời bạn nhập Tên sản phẩm",
      },
      "description": {
        required: "Mời bạn nhập mô tả",
    },
    "retailPrice": {
      required: "Mời bạn nhập giá bán",
  },
  "discountRate": {
    required: "Mời bạn nhập giảm giá",
},
  },
});


// Chờ tài liệu HTML được tải xong
document.addEventListener("DOMContentLoaded", () => {
  const uploadList = document.querySelector(".upload-list");
  const uploadButton = document.getElementById('upload-button');

  // Xử lý sự kiện khi nhấn nút đóng trong modal
  const handleCancel = () => {
    document.querySelector(".preview-modal").style.display = "none";
  };

  // Xử lý sự kiện khi xóa hình ảnh


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
    console.log('run');
    if (selectedColor === -1) {
      alert('Bạn phải chọn màu trước');
    } else {
      fileInput.click();
    }
  });



  // Lắng nghe sự kiện thay đổi tập tin tải lên
  const fileInput = document.getElementById("file-input");
  fileInput.addEventListener("change", (event) => {
    const files = event.target.files;
    for (const file of files) {
      if (file.type.startsWith("image/")) {
        // fileList.push(file);
        // ảnh thêm vào api
        const newImage = { file: file, setAsDefault: false };
        product.Colors[selectedColor].Images.push(newImage);
        //  product.Colors[0].Images.push(newImage);
        loadImageE();
      }
    }
    fileInput.value = ""; // Reset file input
  });
});

function handleDelete(file) {
  const uploadButton = document.getElementById("upload-button");
  const index = product.Colors[selectedColor].Images.findIndex(p => p.file === file);
  if (index > -1) {
    product.Colors[selectedColor].Images.splice(index, 1);
    if (product.Colors[selectedColor].Images.length < 6) {
      uploadButton.style.display = "flex";
    }
    loadImageE();
  }
}

function loadImageE() {
  const uploadList = document.querySelector(".upload-list");
  const dynamicDivs = uploadList.querySelectorAll(".preview-container");
  const uploadButton = document.getElementById("upload-button");

  dynamicDivs.forEach(dy => {
    uploadList.removeChild(dy);
  });

  if (selectedColor !== -1) {
    // Kiểm tra nếu danh sách hình ảnh đã đạt đến giới hạn
    if (product.Colors[selectedColor].Images.length === 6) {
      uploadButton.style.display = "none"; // Ẩn nút "Upload"
    }

    if (product.Colors[selectedColor].Images.length !== 0) {
      product.Colors[selectedColor].Images.forEach(img => {
        const previewContainer = document.createElement("div");
        previewContainer.className = "preview-container";
        const previewImage = document.createElement("img");
        previewImage.src = URL.createObjectURL(img.file);
        previewImage.alt = "Preview";
        previewImage.className = "preview-image";

        // Tạo nút xóa và xử lý sự kiện khi click
        const deleteButton = document.createElement("button");
        deleteButton.textContent = "x";
        deleteButton.className = "delete-button";
        deleteButton.addEventListener("click", () => {
          handleDelete(img.file);
        });

        previewContainer.appendChild(previewImage);
        previewContainer.appendChild(deleteButton);
        uploadList.appendChild(previewContainer);
      });
    }

  }

}

function findIndexById(array, id) {
  for (var i = 0; i < array.length; i++) {
    if (array[i].id === id) {
      return i; // Trả về chỉ số khi tìm thấy phần tử có id tương ứng
    }
  }
  return -1; // Trả về -1 nếu không tìm thấy phần tử
}

function clearE() {
  const uploadList = document.querySelector(".upload-list");
  const dynamicDivs = uploadList.querySelectorAll(".preview-container");
  const plusButtonContainer = document.getElementById("render-color");
  const sizeContainer = document.getElementById("render-size");

  plusButtonContainer.innerHTML = "";

  while (sizeContainer.firstChild) {
    sizeContainer.removeChild(sizeContainer.firstChild);
  }

  dynamicDivs.forEach(dy => {
    uploadList.removeChild(dy);
  });
}

function loadColorE() {
  const plusButtonContainer = document.getElementById("render-color");
  plusButtonContainer.innerHTML = "";

  if (product.Colors.length !== 0) {
    product.Colors.forEach(color => {
      var newDiv = document.createElement('div');
      newDiv.className = "container-color";


      var newButton = document.createElement("button");
      newButton.type = "button";
      newButton.className = 'btn btn-outline-dark';
      newButton.id = color.id;
      newButton.textContent = color.name;
      newButton.addEventListener('click', function (e) {
        selectedColor = findIndexById(product.Colors, e.target.id);
        loadSizeE();
        loadImageE();
      });

      var newRemoveBtn = document.createElement("button");
      newRemoveBtn.className = "close-button";
      newRemoveBtn.textContent = " x ";
      newRemoveBtn.id = color.id;
      newRemoveBtn.addEventListener('click', function (e) {
        debugger;
        let index = findIndexById(product.Colors, e.target.id);
        product.Colors.splice(index, 1);
        console.log(product);
        if (product.Colors.length === 0) {
          selectedColor = -1;
          clearE();
        } else {
          selectedColor = 0;
          loadColorE();
          loadSizeE();
          loadImageE();
        }
      });

      newDiv.appendChild(newButton);
      newDiv.appendChild(newRemoveBtn);

      plusButtonContainer.appendChild(newDiv);
    });
  }
}

function isIdExists(id, array) {
  return array.some(function (item) {
    return item.id === id;
  });
}
// add color 
document.addEventListener("DOMContentLoaded", function () {
  var addColorButton = document.getElementById("addColorButton");
  var selectedColorText = {};

  addColorButton.addEventListener("click", function () {
    if (selectedColorText === {} || selectedColorText === undefined) {
      return; // Ngăn việc thêm nút nếu màu chưa được chọn
    }

    if (!isIdExists(selectedColorText.id, product.Colors)) {
      product.Colors.push({ id: selectedColorText.id, name: selectedColorText.text, Images: [], Sizes: [] });
    } else {
      alert(`Màu ${selectedColorText.text} đã tồn tại!`);
      selectedColorText = {};
      return;
    }

    selectedColor = findIndexById(product.Colors, selectedColorText.id);
    loadColorE();
    loadSizeE();
    loadImageE();
    console.log(product);


    selectedColorText = {};
    $('#exampleModalColor').modal('hide');

  });

  $('#exampleModalColor').on('show.bs.modal', function (event) {
    var button = $(event.relatedTarget);
    selectedColorText = button.data('color');
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
          $('#exampleModalColor').modal('show');
        },
      });
    }
  });
});

function loadSizeE() {
  var plusButtonContainer = document.getElementById("render-size");
  // Xóa tất cả các phần tử con trong containerParent
  while (plusButtonContainer.firstChild) {
    plusButtonContainer.removeChild(plusButtonContainer.firstChild);
  }

  if (product.Colors.length !== 0) {
    if (product.Colors[selectedColor].Sizes.length !== 0) {
      product.Colors[selectedColor].Sizes.forEach(element => {
        var container = document.createElement("div");
        container.className = "container-unit";

        // thêm ô hiển thị size
        var newButton = document.createElement("button");
        newButton.className = 'btn btn-dark';
        newButton.textContent = element.numberSize;

        // thêm ô điền số lượng
        var newInput = document.createElement("input");
        newInput.className = 'input-unit';
        newInput.placeholder = "Điền số lượng"
        newInput.value = element.unitInStock >= 0 ? element.unitInStock : '';
        newInput.min = 0;
        newInput.addEventListener('change', function () {
          if (parseInt(newInput.value) < 0) {
            newInput.value = 0;
          } else {
            let index = product.Colors[selectedColor].Sizes.findIndex(p => p.id === element.id);
            product.Colors[selectedColor].Sizes[index].unitInStock = parseInt(newInput.value);
          }
        });

        // thêm nút x bỏ
        var newXButton = document.createElement("button");
        newXButton.type = "button";
        newXButton.className = 'btn btn-danger';
        newXButton.textContent = 'x';
        newXButton.addEventListener('click', function () {
          if (confirm("Bạn có muốn xóa thuộc tính này?")) {
            let index = product.Colors[selectedColor].Sizes.findIndex(p => p.id === element.id);
            product.Colors[selectedColor].Sizes.splice(index, 1);
            loadSizeE();
          }
        });

        container.appendChild(newButton);
        container.appendChild(newInput);
        container.appendChild(newXButton);
        plusButtonContainer.appendChild(container);
      });
    }
  }
}

// add size 
document.addEventListener("DOMContentLoaded", function () {
  var addColorButton = document.getElementById("addSizeButton");
  var selectedColorText = {};

  addColorButton.addEventListener("click", function () {
    if (selectedColorText === {} || selectedColorText === undefined) {
      return; // Ngăn việc thêm nút nếu màu chưa được chọn
    }

    if (selectedColor !== -1 && !isIdExists(selectedColorText.id, product.Colors[selectedColor].Sizes)){
      product.Colors[selectedColor].Sizes.push(selectedColorText);
    }else{
      alert(`Size ${selectedColorText.numberSize} đã tồn tại`);
      selectedColorText = {};
      return;
    }
    loadSizeE();
    $('#exampleModalSize').modal('hide');
    selectedColorText = {};
  });

  $('#exampleModalSize').on('show.bs.modal', function (event) {
    if (selectedColor === -1) {
      alert('Bạn phải chọn màu trước');
      event.preventDefault();
    }
    var button = $(event.relatedTarget);
    selectedColorText = button.data('color');
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

          $('#exampleModalSize').modal('show');
        },
      });
    }
  });
});










