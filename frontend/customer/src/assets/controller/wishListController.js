

(function () {
    var wishListController = function (
        s, l,
        authService,
        jwtHelper,
        wishListService,
        apiUrl,
        productService,
        cartService,
        headerFactory
    ) {


        s.wishLists = [];
        s.selectedColor = 0;
        s.productDetail = {};
        s.selectedSizeIndex = -1;
        s.colors = [];
        s.errorMg = "";
        s.selectedSizeId = '';
        s.selectedSize = -1;

        s.setSelectedSize = function (sizeId) {
            s.selectedSize = sizeId;
            s.selectedSizeId = '';
            s.errorMg = "";
            for (let i = 0; i < s.colors[s.selectedColor].sizes.length; i++) {
                if (s.colors[s.selectedColor].sizes[i].id === sizeId) {
                    s.selectedSizeIndex = i;
                    break;
                }
            }
        }

        s.addToCart = function () {
            if (s.selectedSizeIndex === -1 && s.selectedSize === -1) {
                s.errorMg = "Bạn phải chọn kích cỡ trước";
                return;
            } else {
                s.errorMg = "";
            }


            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            let data = {
                appUserId: tokenDecode.Id,
                colorId: s.colors[s.selectedColor].id,
                productId: s.productDetail.id,
                sizeId: s.colors[s.selectedColor].sizes[s.selectedSizeIndex].id,
                quantity: 1
            }

            cartService.addToCarts(data)
                .then(function (response) {
                    let countCart = cartService.getCarts(tokenDecode.Id)
                        .then(function (response) {
                            headerFactory.setCartCounter(response.data.length);
                            l.path('/cart');
                        })
                        .catch(function (data) { console.log(data); })
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        s.setSelectedColor = function (colorId) {
            for (let i = 0; i < s.colors.length; i++) {
                if (s.colors[i].id === colorId) {

                    s.selectedColor = i;

                    break;
                }
            }
            s.selectedSizeIndex = -1;
            s.selectedSize = -1;
            s.selectedSizeId = '';

        }

        s.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        s.getProductDetail = function (productId) {
            s.selectedColor = 0;
            s.productDetail = {};
            s.selectedSizeIndex = -1;
            s.colors = [];
            s.errorMg = "";
            s.selectedSizeId = '';

            productService.getProduct(productId)
                .then(function (response) {
                    s.productDetail = {
                        "id": response.data.id,
                        "barCode": response.data.barCode,
                        "name": response.data.name,
                        "status": response.data.status,
                        "retailPrice": response.data.retailPrice,
                        "description": response.data.description,
                        "discountRate": response.data.discountRate,
                        "discountType": response.data.discountType,
                        "soleId": response.data.soleId,
                        "materialId": response.data.materialId,
                    };
                    let countColor = new Set();
                    response.data.productImages.forEach(element => {
                        countColor.add(element.colorId);
                    });
                    countColor.forEach(function (value) {
                        s.colors.push({ id: value, images: [], sizes: [] });
                    })
                    s.colors.forEach(color => {
                        color.images = response.data.productImages.filter(img => img.colorId === color.id).map(img => img.imageUrl);
                        color.sizes = response.data.stocks.filter(stock => stock.colorId === color.id).map(stock => {
                            return { id: stock.sizeId, stockId: stock.stockId, unit: stock.unitInStock, numberSize: stock.numberSize, selected: false }
                        })

                    });
                    console.log(s.colors);
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        s.removeWish = function (productId) {
            if (confirm("Bạn có muốn xóa sản phẩm này!")) {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                wishListService.removeWish(tokenDecode.Id, productId)
                    .then(function (response) {
                        alert("Xóa thành công");
                        wishListService.getWishLists(tokenDecode.Id)
                            .then(function (response) {
                                s.wishLists = response.data;
                                console.log(s.wishLists);
                            }, function (response) {
                                console.error(response.data);
                            })
                    }, function (response) {
                        console.error(response.data);
                    })
            }
        }

        function constructor() {
            if (!authService.isLoggedIn()) {
                l.path('/signin');
            } else {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                wishListService.getWishLists(tokenDecode.Id)
                    .then(function (response) {
                        s.wishLists = response.data;
                        console.log(s.wishLists);
                    }, function (response) {
                        console.error(response.data);
                    })

            }
        }
        constructor();
    }

    wishListController.$inject = [
        '$scope',
        '$location',
        'authService',
        'jwtHelper',
        'wishListService',
        'apiUrl',
        'productService',
        'cartService',
        'headerFactory'
    ];
    angular.module("app").controller("wishListController", wishListController);
}())