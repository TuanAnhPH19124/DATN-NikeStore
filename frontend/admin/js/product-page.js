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

