const id = localStorage.getItem("staffId");
console.log(id) //lay id nhan vien
// call api chi tiet 1 nhan vien
$(document).ready(function () {
    $.ajax({
        url: "https://localhost:44328/api/Employee/Get/" + id,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(JSON.stringify(data));
            $('#fullName').val(data.fullName);
            var dateObj1 = new Date(data.dateOfBirth);
            var day1 = dateObj1.getUTCDate();
            var month1 = dateObj1.getUTCMonth() + 1;
            var year1 = dateObj1.getUTCFullYear();
            var formattedDate = `${day1}/${month1}/${year1}`;
            $('#dateOfBirth').val(formattedDate);
            $('#snn').val(data.snn);
            $('#phoneNumber').val(data.phoneNumber);
            $('#status').prop('checked', data.status);
            $('#homeTown').val(data.homeTown);

            $.ajax({
                url: "https://localhost:44328/api/AppUser/Get/" + data.appUserId,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    console.log(JSON.stringify(data));
                    $("#email").val(data.email)
                },
                error: function () {
                    console.log("Error retrieving data.");
                }
            });
        },
        error: function () {
            console.log("Error retrieving data.");
        }
    });
    $('#update-employee-form').submit(function (event) {
        event.preventDefault()
        var formData = {
            id : id,
            "snn": $("#snn").val(),
            "fullName": $("#fullName").val(),
            "phoneNumber": $("#phoneNumber").val(),
            "dateOfBirth": $("#dateOfBirth").val(),
            "gender": $("#gender").val(),
            "homeTown":  $("#homeTown").val(),
            "status":  $("#status").prop('checked'),
        };
                  //convert nomal date to ISO 8601 date
                  [startDay, startMonth, startYear] = formData.dateOfBirth.split('/');
                  try {
                      formData.dateOfBirth = new Date(`${startYear}-${startMonth}-${startDay}`).toISOString();
                  } catch (error) {
                      formData.dateOfBirth = ""
                  }
                  if (confirm(`Bạn có muốn cập nhật nhân viên không?`)) {
                    $.ajax({
                        url: "https://localhost:44328/api/Employee/" + id,
                        type: "PUT",
                        data: JSON.stringify(formData),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            window.location.href = "/frontend/admin/staff.html";
                        },
                    });
                  } else {
                    return;
                  }
    });
    // custom validate 
    $.validator.addMethod("nameContainOnlyChar", function (value, element) {
        return value.match(/^[a-zA-ZÀ-ỹ\s]+$/) != null;
    });
    $.validator.addMethod("idContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("phoneNumContainOnlyNum", function (value, element) {
        return value.match(/[^0-9]/) == null;
    });
    $.validator.addMethod("onlyContain10Char", function (value, element) {
        return value.match(/^\w{10}$/) != null;
    });
    $.validator.addMethod("onlyContain12Char", function (value, element) {
        return value.match(/^\w{12}$/) != null;
    });
    // add validate
    $("#update-employee-form").validate({
        rules: {
            "dateOfBirth": {
                required: true,
            },
            "homeTown": {
                required: true,
            },
            "fullName": {
                required: true,
                maxlength: 30,
                nameContainOnlyChar: true,
            },
            "snn": {
                required: true,
                idContainOnlyNum: true,
                onlyContain12Char: true,
            },
            "phoneNumber": {
                required: true,
                phoneNumContainOnlyNum: true,
                onlyContain10Char: true,
            },
            "role": {
                required: true,
            }
        },
        messages: {
            "dateOfBirth": {
                required: "Bạn phải nhập ngày sinh",
            },
            "homeTown": {
                required: "Bạn phải nhập Quê quán",
            },
            "fullName": {
                required: "Bạn phải nhập họ và tên",
                maxlength: "Hãy nhập tối đa 30 ký tự",
                nameContainOnlyChar: "Họ tên không được chứa số hay ký tự",
            },
            "snn": {
                required: "Bạn phải nhập số căn cước",
                idContainOnlyNum: "Số căn cước không được chưa ký tự",
                onlyContain12Char: "Độ dài của số căn cước là 12"
            },
            "phoneNumber": {
                required: "Bạn phải nhập số điện thoại",
                phoneNumContainOnlyNum: "Số điện thoại không được chứa ký tự",
                onlyContain10Char: "Số điện thoại chứa 10 ký tự"
            },
            "role": {
                required: "Bạn phải nhập tên vai trò",
            }
        },
    });
});
$(function () {
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
    });
});