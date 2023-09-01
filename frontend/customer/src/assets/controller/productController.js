(function () {
    var productController = function (e, l, productService, categoryService, apiUrl) {
        e.products = [];
        e.categories = [];
        e.sale = false;
        e.sortBy = "";
        e.genders = [
            {id: 0, name: "Nam", selected: false},
            {id: 1, name: "Nữ", selected: false},
            {id: 2, name: "Unisex", selected: false},
        ]
        e.onSelectChange = function() {
            console.log('run filter');
            getProductsByFilter();
        };

        function getProductsByFilter(){
            let params = {};
            params.sortBy = e.sortBy;
            params.sale = true;
            params.categories = e.categories.filter(item => item.selected).map(selected => selected.id);
            productService.getProductsByParams(params)
            .then(function (response){
                e.products = response.data;
            })
            .catch(function (data){
                console.log(data);
            });
        }

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
                })
                .catch(function (data){
                    console.log(data);
                });
            
            categoryService.getCategories()
            .then(function (response){
                e.categories = response.data;
                e.categories.forEach(function (item){
                    item.selected = false;
                })
            })
            .catch(function (data){
                console.log(data);
            });
        }


        constructor();
    };

    productController.$inject = ['$scope', '$location', 'productService', 'categoryService','apiUrl'];
    angular.module("app").controller("productController", productController);
}())