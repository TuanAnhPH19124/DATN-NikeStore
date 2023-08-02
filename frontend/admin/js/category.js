// call api len datatable nhan vien
$(document).ready(function () {
    var sizeTable = $('#category-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Categories/Get",
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
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Sửa</a></td>';
                },
                "title": "Thao tác"
            },
        ],
    });
    setInterval(function () {
        sizeTable.ajax.reload();
    }, 5000);
});


