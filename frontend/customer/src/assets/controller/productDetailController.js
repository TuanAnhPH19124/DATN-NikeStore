(function () {
    var productDetailController = function (e, r, l, productService, authService, jwtHelper, cartService) {
        e.product = null;
        e.quantity = 1;

        e.addToCartE = function (productId) {
            if (e.quantity <= 0){
                return;
            }
            if (!authService.isLoggedIn())
            {
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.CART,
                    data: {
                        appUserId: null,
                        shoppingCartItemsDto: {
                            quantity: e.quantity,
                            productId: productId,
                            isQuantity: true
                        }
                    }
                }
                authService.setEventAfterLogin(newE);
                authService.scheduleClearEvent();
                l.path('/signin');
            }else{
                let token =  authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let data = {
                    appUserId: tokenDecode.Id,
                    shoppingCartItemsDto: {
                        quantity: e.quantity,
                        productId: productId,
                        isQuantity: true
                    }
                }
                cartService.addToCarts(data)
                .then(function(response){
                    l.path('/cart');
                })
                .catch(function(data){
                    console.log(data);
                });
            }
        
        }

        function constructor() {
            debugger;
            let productId = String(r.id);
            console.log(productId);
            productService.getProduct(productId)
                .then(function (response) {
                    e.product = response.data;
                })
                .catch(function (data) {
                    console.log(data);
                });
        };

        constructor();
    }
    productDetailController.$inject = ['$scope', '$routeParams', '$location','productService', 'authService', 'jwtHelper', 'cartService'];
    angular.module("app").controller("productDetailController", productDetailController);
}());