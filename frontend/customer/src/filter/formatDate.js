(function () {
    var formatDate = function () {
        return function (input) {
            if (!input) return '';

            // Chuyển đổi input thành một đối tượng Date
            var date = new Date(input);

            // Lấy giờ và phút
            var hours = date.getHours();
            var minutes = date.getMinutes();

            // Lấy ngày, tháng và năm
            var day = date.getDate();
            var month = date.getMonth() + 1; // Tháng trong JavaScript bắt đầu từ 0
            var year = date.getFullYear();

            // Định dạng lại ngày giờ
            var formattedTime = (hours < 10 ? '0' : '') + hours + ':' + (minutes < 10 ? '0' : '') + minutes;
            var formattedDate = (day < 10 ? '0' : '') + day + '-' + (month < 10 ? '0' : '') + month + '-' + year;

            return formattedTime + ' ' + formattedDate;
        };
    };
    angular.module("app").filter("formatDate", formatDate);
}())