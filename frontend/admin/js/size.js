// call api len datatable nhan vien
$(document).ready(function () {
    $.fn.dataTableExt.sErrMode = 'mute';
    var sizeTable = $('#size-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Size/Get",
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
            { "data": 'numberSize', "title": "Kích cỡ" },
            { "data": 'description', "title": "Mô tả" },
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
    // call api them nhan vien
    $('#add-size-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            numberSize: $("#numberSize").val(),
            description: ""
        };
        if(formData.numberSize.trim(" ")==""){
            return
        }
        if (confirm(`Bạn có muốn thêm size ${formData.numberSize} không?`)) {
            $.ajax({
                url: "https://localhost:44328/api/Size",
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
    $('#size-table tbody').on('click', 'tr', function (e) {
        e.preventDefault();
        let sizeId = $('#size-table').DataTable().row(this).data().id;
        if (sizeId !== null) {
            localStorage.setItem("sizeId", sizeId);
            window.location.href = `/frontend/admin/update-size.html`;
        }
    });
    $.validator.addMethod("onlyContaiNum", function (value, element) {
        return value.match(/^[0-9]+$/) != null;
    });
    $("#add-size-form").validate({
        rules: {
            "numberSize": {
                required: true,
                onlyContaiNum: true
            },
        },
        messages: {
            "numberSize": {
                required: "Mời bạn nhập Số size",
                onlyContaiNum: "Size là số không chứa ký tự"
            },
        },
    });
});


