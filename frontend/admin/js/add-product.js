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

// call api len datatable nhan vien
$(document).ready(function () {
  // call api them nhan vien
  $('#add-product-form').submit(function (event) {
    event.preventDefault()
    var formData = {
      "name": $("#name").val(),
      "retailPrice": $("#retailPrice").val(),
      "costPrice": $("#costPrice").val(),
      "description": $("#description").val(),
      "brand": 0,
      "status": Number($("#status").val()),
      "discountRate": 0,
    };

    $.ajax({
      url: "https://localhost:44328/api/Product",
      type: "POST",
      data: JSON.stringify(formData),
      contentType: "application/json",
      success: function (response) {
        localStorage.setItem("productId", response.id);
        console.log(response.id)
        window.location.href = "/frontend/admin/product-detail.html";
      },
    });
  });

});
$.validator.addMethod("compare2Price", function (value, element) {
  var parts1 = Number($("#retailPrice").val());
  var parts2 = Number($("#costPrice").val());
  return parts1 > parts2

});
$("#add-product-form").validate({
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

// upload nhiều ảnh
const getBase64 = (file) =>
  new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = (error) => reject(error);
  });

document.addEventListener("DOMContentLoaded", () => {
  const fileList = [
    // ... Add other file items here ...
  ];

  const uploadList = document.querySelector('.upload-list');

  const handleCancel = () => {
    document.querySelector('.preview-modal').style.display = 'none';
  };

  const handlePreview = async (file) => {
    if (!file.url && !file.preview) {
      file.preview = await getBase64(file.originFileObj);
    }
    const modal = document.querySelector('.preview-modal');
    modal.querySelector('.preview-image').src = file.url || file.preview;
    modal.querySelector('.modal-title').textContent = file.name || file.url.substring(file.url.lastIndexOf('/') + 1);
    modal.style.display = 'flex';
  };

  fileList.forEach((file) => {
    const previewContainer = document.createElement('div');
    previewContainer.className = 'preview-container';
    const previewImage = document.createElement('img');
    previewImage.src = file.url;
    previewImage.alt = 'Preview';
    previewImage.className = 'preview-image';
    previewImage.addEventListener('click', () => {
      handlePreview(file);
    });
    previewContainer.appendChild(previewImage);
    uploadList.appendChild(previewContainer);
  });

  const modalCloseButton = document.querySelector('.modal-close-button');
  modalCloseButton.addEventListener('click', () => {
    handleCancel();
  });

  const modalOverlay = document.querySelector('.modal-overlay');
  modalOverlay.addEventListener('click', () => {
    handleCancel();
  });

  const modalContent = document.querySelector('.modal-content');
  modalContent.addEventListener('click', (event) => {
    event.stopPropagation();
  });

  const uploadButton = document.querySelector('.upload-button');
  const fileInput = document.getElementById('file-input');
  uploadButton.addEventListener('click', () => {
    fileInput.click();
  });

  fileInput.addEventListener('change', (event) => {
    const file = event.target.files[0];
    if (file) {
      fileList.push(file);
      const previewContainer = document.createElement('div');
      previewContainer.className = 'preview-container';
      const previewImage = document.createElement('img');
      previewImage.src = URL.createObjectURL(file);
      previewImage.alt = 'Preview';
      previewImage.className = 'preview-image';
      previewImage.addEventListener('click', () => {
        handlePreview(file);
      });
previewContainer.appendChild(previewImage);
      uploadList.appendChild(previewContainer);
    }
  });
});