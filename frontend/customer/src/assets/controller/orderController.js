(function () {
    var orderController = function (e, r, l, orderFactory, productService, authService, jwtHelper, cartService,apiUrl) {
        e.step = 1;
        e.carts = [];
        e.deliveryFee = function (){
            if (e.subtotal() > 5000000)
                return 0;
            else
                return 250000;
        }
        
        e.getImgUrl = function (path){
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        e.subtotal = function () {
            let total = 0;
            e.carts.forEach(item => {
                total += item.quantity * item.product.discountRate;
            });
            return total;
        }

        function constructor() {

            if (authService.isLoggedIn()) {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                // lấy dữ liệu giỏ hàng
                cartService.getCarts(tokenDecode.Id)
                    .then(function (response) {
                        e.carts = response.data;
                    })
                    .catch(function (data) {
                        console.log(data);
                    });

            } else {
                l.path('/signin');
            }

        }

        constructor();
    }
    orderController.$inject = ['$scope', '$routeParams', '$location', 'orderFactory', 'productService', 'authService', 'jwtHelper', 'cartService', 'apiUrl'];
    angular.module("app").controller("orderController", orderController);
}());