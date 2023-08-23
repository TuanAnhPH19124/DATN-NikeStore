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
                    return `<img src="https://localhost:44328/Images/${data}.jpg" alt="" style="border-radius: 10%;" width=120px height=110px>`;
            }},
            { "data": 'name', 'title': 'Tên sản phẩm' },
            { "data": 'costPrice', 'title': 'Giá nhập',
            "render": function (data, type, row) {
                    return data+" VND";
            } },
            { "data": 'retailPrice', 'title': 'Giá bán',
            "render": function (data, type, row) {
                return data+" VND";
        }  },
            {
                "data": 'status', "title": "Trạng thái",
                "render": function (data, type, row) {
                    if (data == 1) {
                        return '<span class="badge badge-pill badge-primary" style="padding:10px;background-color: #1967d2;border-color: #1967d2;" >Kinh doanh</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger" style="padding:10px;">Ngừng kinh doanh</span>';
                    }
                }
            },
            {
                "title": "Thao tác",
                "render": function () {
                    return '<td><a class="btn btn-primary" style="background-color: #1967d2;border-color: #1967d2;" id="btn"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
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

