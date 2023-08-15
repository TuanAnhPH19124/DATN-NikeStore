const id = localStorage.getItem("voucherId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Voucher/Get/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#code').val(data.code);
            $('#value').val(data.value);
            $('#description').val(data.description);
            var dateObj1 = new Date(data.startDate);
            var day1 = dateObj1.getUTCDate();
            var month1 = dateObj1.getUTCMonth() + 1;
            var year1 = dateObj1.getUTCFullYear();
            var formattedDate = `${day1}/${month1}/${year1}`;

            var dateObj2 = new Date(data.endDate);
            var day2 = dateObj2.getUTCDate();
            var month2 = dateObj2.getUTCMonth() + 1;
            var year2 = dateObj2.getUTCFullYear();
            var formattedDate2 = `${day2}/${month2}/${year2}`;
            $('#startDate').val(formattedDate);
            $('#endDate').val(formattedDate2);
            $('#status').prop('checked', data.status);
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-voucher-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id : id,
            code: $("#code").val(),
            value: Number($("#value").val()),
            description: $("#description").val(),
            startDate: $("#startDate").val(),
            endDate: $("#endDate").val(),
            status: $("#status").prop('checked'),
            
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
            url: "https://localhost:44328/api/Voucher/" + id,
            type: "PUT",
            data: JSON.stringify(formData),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                window.location.href = "/frontend/admin/voucher.html";

            },
        });
    });
});

$(function () {
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
    });
});