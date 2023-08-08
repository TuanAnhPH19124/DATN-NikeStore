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
});


