// call api len datatable nhan vien
$(document).ready(function () {
    var sizeTable = $('#category-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Sole",
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
            { "data": 'name', "title": "Loại đế giày" },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" style="background-color: #1967d2;border-color: #1967d2;" id="btn"><i class="fa fa-wrench" aria-hidden="true"></i></a></td>';
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
    $('#add-sole-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            name: $("#name").val().trim(),
            description: ""
        };
        if(formData.name.trim(" ")==""){
            return
        }
        if (confirm(`Bạn có muốn thêm đế ${formData.name} không?`)) {
            if(formData.name.trim()==""){
                return
            }
            $.ajax({
                url: "https://localhost:44328/api/Sole",
                type: "POST",
                data: JSON.stringify(formData),
                contentType: "application/json",
                success: function (response) {
                    $('#success').toast('show')
                },
                error: function () {
                    $('#fail').toast('show')
                },
            });
        } else {
            return
        }
    });
    $('#category-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let soleId = $('#category-table').DataTable().row(this).data().id;
        if (soleId !== null) {
            localStorage.setItem("soleId", soleId);
            window.location.href = `/frontend/admin/update-sole.html`;
        }
    });
    $("#add-sole-form").validate({
        rules: {
            name: {
                required: true,
            },
        },
        messages: {
            "name": {
                required: "Mời bạn nhập loại đế",
            },
        },
    });
});


