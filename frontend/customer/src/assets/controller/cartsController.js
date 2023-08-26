(function(){
    var cartsController = function (e, l, authService, cartService, jwtHelper, orderFactory){
        e.carts = [];
        e.totalAmount = function () {
            var total = 0
            for (let i = 0; i < e.carts.length; i++) {
                if (e.carts[i].isSelected)
                    total += (e.carts[i].product.retailPrice * e.carts[i].product.discountRate / 100) * e.carts[i].quantity;
            }
            return total;
        };

        e.goToPayment = function () {
            let selectedItems = e.carts.filter(item => item.isSelected === true);
            orderFactory.setSelectedItems(selectedItems);
            l.path('/pay/1');
        }

        function constructor() {
            if (!authService.isLoggedIn() && e.carts !== []){
                l.path('/signin');
            }else{
                let token =  authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                cartService.getCarts(tokenDecode.Id)
                .then(function(response){
                    debugger;
                    e.carts = response.data;
                    e.carts.forEach(function(item) {
                        item.isSelected = false;
                    });
                    console.log(e.carts);
                })
                .catch(function(data){
                    console.log(data);
                });
            }
        }
        constructor();
    }
    cartsController.$inject = ['$scope', '$location', 'authService', 'cartService', 'jwtHelper', 'orderFactory'];
    angular.module("app").controller("cartsController", cartsController);
}());