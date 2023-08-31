(function () {
    var productDetailController = function (e, r, l, productService, authService, jwtHelper, cartService, apiUrl, headerFactory) {
        e.product = null;
        e.quantity = "1";
        e.colors = [];
        e.errorMg = "";
        e.selectedColor = 0;
        e.selectedImg = 0;
        e.selectedSize = -1;
        e.selectedSizeIndex = -1;
        e.addToCartE = function (productId) {
            if (e.selectedSizeIndex === -1 && e.selectedSize === -1) {
                e.errorMg = "Bạn phải chọn kích cỡ trước";
                return;
            } else {
                e.errorMg = "";
            }
            if (!authService.isLoggedIn()) {
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.CART,
                    data: {
                        appUserId: null,
                        stockId: e.colors[e.selectedColor].sizes[e.selectedSizeIndex].stockId,
                        quantity: parseInt(e.quantity)
                    }
                }
                authService.setEventAfterLogin(newE);
                authService.scheduleClearEvent();
                l.path('/signin');
            } else {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let data = {
                    appUserId: tokenDecode.Id,
                    stockId: e.colors[e.selectedColor].sizes[e.selectedSizeIndex].stockId,
                    quantity: parseInt(e.quantity)
                }
                cartService.addToCarts(data)
                    .then(function (response) {
                        let countCart = cartService.getCarts(tokenDecode.Id)
                        .then(function (response){
                            headerFactory.setCartCounter(response.data.length);
                            l.path('/cart');
                        })
                        .catch(function(data){ console.log(data);})
                    })
                    .catch(function (data) {
                        console.log(data);
                    });
            }

        }
        e.setSelectedSize = function (sizeId) {
            e.selectedSize = sizeId;
            e.errorMg = "";
            for (let i = 0; i < e.colors[e.selectedColor].sizes.length; i++) {
                if (e.colors[e.selectedColor].sizes[i].id === sizeId) {
                    e.selectedSizeIndex = i;
                    break;
                }
            }
        }
        e.setSelectedColor = function (colorId) {
            for (let i = 0; i < e.colors.length; i++) {
                if (e.colors[i].id === colorId) {
                    e.selectedColor = i;
                    break;
                }
            }
            e.selectedSizeIndex = -1;
            e.selectedSize = -1;
        }
        e.checkUnit = function () {
            let unit = e.colors[e.selectedColor].sizes[e.selectedSizeIndex].unit;
            let selectedUnit = parseInt(e.quantity);
            if (selectedUnit > unit)
                e.quantity = unit + "";
        }
        e.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }




        function constructor() {
            let productId = String(r.id);
            productService.getProduct(productId)
                .then(function (response) {
                    e.product = {
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
                        e.colors.push({ id: value, images: [], sizes: [] });
                    })
                    e.colors.forEach(color => {
                        color.images = response.data.productImages.filter(img => img.colorId === color.id).map(img => img.imageUrl);
                        color.sizes = response.data.stocks.filter(stock => stock.colorId === color.id).map(stock => {
                            return { id: stock.sizeId, stockId: stock.stockId, unit: stock.unitInStock, numberSize: stock.numberSize, selected: false }
                        })

                    });
                    console.log(e.colors);
                })
                .catch(function (data) {
                    console.log(data);
                });
        };

        constructor();
    }
    productDetailController.$inject = ['$scope', '$routeParams', '$location', 'productService', 'authService', 'jwtHelper', 'cartService', 'apiUrl', 'headerFactory'];
    angular.module("app").controller("productDetailController", productDetailController);
}());