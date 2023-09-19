(function () {
    var productController = function (jwtHelper,authService,wishListService,sizeService,colorService, e, l, productService, categoryService, apiUrl) {
        var searchKeyword = l.search().search_keyword;

        e.products = [];
        e.categories = [];
        e.sale = false;
        e.sortBy = "";
        e.genders = [
            { id: 0, name: "Nam", selected: false },
            { id: 1, name: "Nữ", selected: false },
            { id: 2, name: "Unisex", selected: false },
        ]
        e.colors = [];
        e.sizes = [];
        e.priceRanges = [
            {min: 0, max: 1000000, selected: false},
            {min: 1000000, max: 2000000, selected: false},
            {min: 2000000, max: 4999999, selected: false},
            {min: 5000000, max: 5000000 * 9999, selected: false}
        ]

        e.addToFavourite = function (productId){
            if (!authService.isLoggedIn()){
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.WISHLIST,
                    data: {
                        productsId: productId,
                        appUserId: "",
                    }
                }
                authService.setEventAfterLogin(newE);
                authService.scheduleClearEvent();
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);

                let data = {
                    productsId: productId,
                    appUserId: tokenDecode.Id
                }

                wishListService.addNewWishList(data)
                .then(function (response){
                    alert("Thêm sản phẩm yêu thích thành công");

                }, function (response){
                    console.error(response.data);
                })


            }
        }

        e.onSelectChange = function () {
            console.log('run filter');
            getProductsByFilter();
        };

        function getProductsByFilter() {
            var min = 5000000 * 9999;
            var max = 0;
            e.priceRanges.filter(item => {
                if (item.selected){
                    if (min > item.min)
                        min = item.min;
                    // 1. min = 0
                    // 2. item.min = 10
                    max = item.max;
                };
            });
            let params = {};
            params.keyword = searchKeyword;
            params.sortBy = e.sortBy;
            params.sale = true;
            params.categories = e.categories.filter(item => item.selected).map(selected => selected.id);
            params.colors = e.colors.filter(item => item.selected).map(selected => selected.id);
            params.sizes = e.sizes.filter(item => item.selected).map(selected => selected.id);
            params.min = min >= 0 ? min : null;
            params.max = max > 0 ? max : null;
            productService.getProductsByParams(params)
                .then(function (response) {
                    e.products = response.data;
                })
                .catch(function (data) {
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

        e.calculatePercenOff = function (discount, price) {
            return 100 - discount / price * 100;
        }

        e.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        function constructor() {
            if (searchKeyword){
                console.log('key word found: ' + searchKeyword);

            }

            getProductsByFilter();

            // productService.getProducts()
            //     .then(function (response) {
            //         e.products = response.data;
            //     })
            //     .catch(function (data) {
            //         console.log(data);
            //     });

            colorService.getAllColor()
            .then(function(response){
                e.colors = response.data;
                e.colors.forEach(item => {
                    item.selected = false;
                });
                console.log(e.colors);
            }, function(response){
                console.error(response.data);
            })

            sizeService.getAllSize()
            .then(function(response){
                e.sizes = response.data;
                e.sizes.forEach(item => {
                    item.selected = false;
                })
            },function(response){
                console.error(response.data);
            })

            categoryService.getCategories()
                .then(function (response) {
                    e.categories = response.data;
                    e.categories.forEach(function (item) {
                        item.selected = false;
                    })
                })
                .catch(function (data) {
                    console.log(data);
                });
        }


        constructor();
    };

    productController.$inject = ['jwtHelper','authService','wishListService','sizeService','colorService', '$scope', '$location', 'productService', 'categoryService', 'apiUrl'];
    angular.module("app").controller("productController", productController);
}())