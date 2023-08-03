function myFunction() {
    console.log("xóa thành công");
}
// call api with jquery
$(document).ready(function () {
    $('#productData').DataTable({
        "ajax": {
            "url": "https://6447750e50c253374425338d.mockapi.io/fake",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": 'image' },
            { "data": 'name' },
            { "data": 'price' },
            { "data": 'createdPrice' },
            { "data": 'status' },
            {
                "render": function () {
                    return '<td><a class="btn btn-primary" id="btn" onclick="myFunction()">Xóa</a></td>';
                }
            },
        ],
    });
});
//add event click datatable

$('#productData tbody').on('click', 'tr', function (event) {
    if ($(event.target).is('td')) {
        window.location.href = "/frontend/admin/product-detail.html";
    }
});

var options = [];
$.getJSON("https://localhost:44328/api/Categories", function (result) {
    for (var i = 0; i < result.length; i++) {
        options.push('<option value="', result[i].id, '">', result[i].name, '</option>');
    }
    $("#category-select").html(options.join(''));
});
var options2 = [];
$.getJSON("https://localhost:44328/api/Color/Get", function (result) {
    for (var i = 0; i < result.length; i++) {
        options2.push('<option value="', result[i].id, '">', result[i].name, '</option>');
    }
    $("#color-select").html(options2.join(''));
});

