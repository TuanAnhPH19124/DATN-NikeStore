(function () {
    var productController = function (e, l, productService, apiUrl) {
        e.products = [];
        e.countColors = function (array) {
            let countColor = new Set();
            array.forEach(img => {
                countColor.add(img.colorId);
            });
            return countColor.size;
        };
        e.formatPrice = function (price) {
            let numericValue = parseInt(price, 10);
            if (!isNaN(numericValue)) {
                const formattedValue = new Intl.NumberFormat('vi-VN', {
                    style: 'currency',
                    currency: 'VND'
                }).format(numericValue);
                return formattedValue;
            }
        }

        e.calculatePercenOff = function (discount, price){
            return discount/price * 100;
        }

        e.getImgUrl = function (path){
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        function constructor() {
            productService.getProducts()
                .then(function (response) {
                    e.products = response.data;
                    console.log(response.data);
                })
                .catch();
        }


        constructor();
    };

    productController.$inject = ['$scope', '$location', 'productService', 'apiUrl'];
    angular.module("app").controller("productController", productController);
}())