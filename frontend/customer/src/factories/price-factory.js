(function(){
    var priceFactory = function (){
        var factory = {};
        factory.formatVNDPrice = function (price){
            let numericValue = parseInt(price, 10);
            if (!isNaN(numericValue)) {
                const formattedValue = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND'
                }).format(numericValue);
                return formattedValue;
            }
        }
        return factory;
    }
    angular.module("app").factory("priceFactory", priceFactory);
}())