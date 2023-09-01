(function(){
    var cartsController = function (e, l, authService, cartService, jwtHelper, orderFactory, apiUrl){
        e.carts = [];
        e.totalAmount = function () {
            var total = 0
            e.carts.forEach(cart => {
                total += cart.product.discountRate;
            });
            return total;
        };

        e.deliveryFee = function (){
            if (e.totalAmount() > 5000000){
                return 0;
            }else{
                return 100000;
            }
        }

        e.getImgUrl = function (path){
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        function constructor() {
            if (!authService.isLoggedIn() && e.carts !== []){
                l.path('/signin');
            }else{
                let token =  authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                cartService.getCarts(tokenDecode.Id)
                .then(function(response){
                    e.carts = response.data;

                })
                .catch(function(data){
                    console.log(data);
                });
            }
        }
        constructor();
    }
    cartsController.$inject = ['$scope', '$location', 'authService', 'cartService', 'jwtHelper', 'orderFactory', 'apiUrl'];
    angular.module("app").controller("cartsController", cartsController);
}());