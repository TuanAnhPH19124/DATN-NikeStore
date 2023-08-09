// call api len datatable nhan vien
$(document).ready(function () {
    var voucherTable = $('#voucher-table').DataTable({
        "ajax": {
            "url": "https://localhost:44328/api/Voucher/Get",
            "dataType": "json",
            "dataSrc": "",
        },
        "columns": [
            {
                "data": 'id', "title": "ID", render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": 'code', "title": "Mã" },
            { "data": 'value', "title": "Giá trị" },
            {
                "data": null,
                "title": "Thời gian",
                "render": function (data, type, full, meta) {
                    var dateObj1 = new Date(full.startDate);
                    var day1 = dateObj1.getUTCDate();
                    var month1 = dateObj1.getUTCMonth() + 1;
                    var year1 = dateObj1.getUTCFullYear();
                    var formattedDate = `${day1}/${month1}/${year1}`;

                    var dateObj2 = new Date(full.endDate);
                    var day2 = dateObj2.getUTCDate();
                    var month2 = dateObj2.getUTCMonth() + 1;
                    var year2 = dateObj2.getUTCFullYear();
                    var formattedDate2 = `${day2}/${month2}/${year2}`;
                    return formattedDate + '-' + formattedDate2;
                }
            },
            {
                "data": 'createdDate', "title": "Ngày tạo",
                "render": function (data, type, full, meta) {
                    var dateObj = new Date(data);
                    var day = dateObj.getUTCDate();
                    var month = dateObj.getUTCMonth() + 1;
                    var year = dateObj.getUTCFullYear();
                    var formattedDate = `${day}/${month}/${year}`;
                    return formattedDate;
                }
            },
            {
                "data": 'status', "title": "Trạng thái",
                "render": function (data, type, row) {
                    if (data == true) {
                        return '<span class="badge badge-pill badge-primary">Kích hoạt</span>';
                    } else {
                        return '<span class="badge badge-pill badge-danger">Đã hủy</span>';
                    }
                }
            },
            {
                "render": function () {
                    return '<td><a class="btn btn-danger" id="btn" onclick="myFunction()">Hủy kích hoạt</a></td>';
                },
                "title": "Thao tác"
            },
        ],
    });
    setInterval(function () {
        customerTable.ajax.reload();
    }, 2500);
    // call api them nhan vien
    $('#add-voucher-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            code: $("#code").val(),
            value: $("#value").val(),
            description: $("#description").val(),
            startDate: $("#startDate").val(),
            endDate: $("#endDate").val(),
            createdDate: new Date,
            status: true,
        };

        //convert nomal date to ISO 8601 date
        [startDay, startMonth, startYear] = formData.startDate.split('/');
        [endDay, endMonth, endYear] = formData.endDate.split('/');
        try {
            formData.startDate = new Date(`${startYear}-${startMonth}-${startDay}`).toISOString();
            formData.endDate = new Date(`${endYear}-${endMonth}-${endDay}`).toISOString();
        } catch (error) {
            formData.startDate = ""
            formData.endDate = ""
        }

        $.ajax({
            url: "https://localhost:44328/api/Voucher",
            type: "POST",
            data: JSON.stringify(formData),
            contentType: "application/json",
            success: function (response) {
                console.log(response)
                $('.toast').toast('show')
            },
        });
    });
    // custom validate 
    $.validator.addMethod("nameContainOnlyChar", function (value, element) {
        return value.match(/^[a-zA-ZÀ-ỹ\s]+$/) != null;
    });
    $.validator.addMethod("valueContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("compare2Date", function (value, element) {
        var parts1 = $("#startDate").val().split("/");
        var parts2 = $("#endDate").val().split("/");

        var year1 = parseInt(parts1[2], 10) + 2000; // Convert 2-digit year to 4-digit year
        var year2 = parseInt(parts2[2], 10) + 2000;

        var month1 = parseInt(parts1[1], 10) - 1; // JavaScript months are zero-indexed
        var month2 = parseInt(parts2[1], 10) - 1;

        var day1 = parseInt(parts1[0], 10);
        var day2 = parseInt(parts2[0], 10);

        var jsDate1 = new Date(year1, month1, day1);
        var jsDate2 = new Date(year2, month2, day2);
        return jsDate1 <= jsDate2

    });

    // add validate
    $("#add-voucher-form").validate({
        rules: {
            "code": {
                required: true,
                maxlength: 15,
            },
            "value": {
                required: true,
                valueContainOnlyNum: true,
            },
            "description": {
                maxlength: 20
            },
            "startDate": {
                required: true,
                compare2Date: true,
            },
            "endDate": {
                required: true,
                compare2Date: true,
            },
        },
        messages: {
            "code": {
                required: "Bạn phải nhập Mã giảm giá",
                maxlength: "Mã giảm giá không quá 15 ký tự",
            },
            "value": {
                required: "Bạn phải nhập số căn cước",
                valueContainOnlyNum: "Giá trị là số, không chứa ký tự",
            },
            "description": {
                maxlength: "Mô tả không được quá 20 ký tự"
            },
            "startDate": {
                required: "Nhập ngày bắt đầu",
                compare2Date: "Ngày bắt đầu không thể lớn hơn ngày kết thúc",
            },
            "endDate": {
                required: "Nhập ngày kết thúc",
                compare2Date: "Ngày bắt đầu không thể lớn hơn ngày kết thúc",
            },
        },
    });

});
$(function () {
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
    });
});

