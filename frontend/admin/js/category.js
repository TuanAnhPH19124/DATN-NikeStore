// call api len datatable nhan vien
$(document).ready(function () {
    var sizeTable = $('#category-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Categories",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            {
                "data": 'id', "title": "STT",
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": 'name', "title": "Danh mục" },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
                },
                "title": "Thao tác"
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
    setInterval(function () {
        sizeTable.ajax.reload();
    }, 2500);
    $('#add-category-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            name: $("#name").val(),
        };

        if (confirm(`Bạn có muốn thêm danh mục ${formData.name} không?`)) {
            $.ajax({
                url: "https://localhost:44328/api/Categories",
                type: "POST",
                data: JSON.stringify(formData),
                contentType: "application/json",
                success: function (response) {
                    $('.toast').toast('show')
                },
            });
        } else {
            return
        }
    });
    $('#category-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let categoryId = $('#category-table').DataTable().row(this).data().id;
        if (categoryId !== null) {
            localStorage.setItem("categoryId", categoryId);
            window.location.href = `/frontend/admin/update-category.html`;
        }
    });
    $("#add-category-form").validate({
        rules: {
            "name": {
                required: true,
            },
        },
        messages: {
            "name": {
                required: "Mời bạn nhập Tên danh mục",
            },
        },
    });
});


