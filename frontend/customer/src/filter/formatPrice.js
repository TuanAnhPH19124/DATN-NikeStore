(function (){
    var formatVNDPrice = function (){
        return function (input){
            let numericValue = parseInt(input, 10);
            if (!isNaN(numericValue)) {
                const formattedValue = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND'
                }).format(numericValue);
                return formattedValue;
            }
        }
    }
    angular.module("app").filter("formatVNDPrice", formatVNDPrice);
}())