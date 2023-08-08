function myFunction() {
    console.log("xóa thành công");
}
// call api with jquery
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
                    return `<img src="/backend/.NET/Webapi/wwwroot/Images/7b7429a0-423c-4d4f-8ef7-b586b15e2b01/902812a7-85e4-4f58-b496-37c91358e7f6.jpg" alt="">`;
            }},
            { "data": 'name', 'title': 'Tên sản phẩm' },
            { "data": 'costPrice', 'title': 'Giá nhập' },
            { "data": 'retailPrice', 'title': 'Giá bán' },
            {
                "data": 'status', "title": "Trạng thái",
                "render": function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge badge-pill badge-primary">Kinh doanh</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger">Ngừng kinh doanh</span>';
                    }
                }
            },
            { "data": 'brand', 'title': 'Hãng' },
            {
                "title": "Thao tác",
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Sửa</a></td>';
                }
            },
        ],
    });
    $('#productData tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let productId = $('#productData').DataTable().row(this).data().id;
        if (productId !== null) {
            localStorage.setItem("productId", productId);
            window.location.href = `/frontend/admin/product-detail.html`;
        }
    });
});
//add event click datatable

$('#productData tbody').on('click', 'tr', function (event) {
    if ($(event.target).is('td')) {
        window.location.href = "/frontend/admin/product-detail.html";
    }
});

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