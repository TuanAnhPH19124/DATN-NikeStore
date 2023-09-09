(function (){
    var purchaseController = function (apiUrl,priceFactory,l,orderService,jwtHelper,authService,$s){
        $s.orders = [];

        $s.formatPrice = function (price){
            return priceFactory.formatVNDPrice(price);
        }

        $s.getToTalAmount = function (id){
            var order = null;
            var total = 0;
            for (let i = 0; i < $s.orders.length; i++) {
                if ($s.orders[i].orderId === id){
                    order = $s.orders[i];
                    break;
                }                
            }

            order.orderItems.forEach(item => {
                total += item.discountRate * item.quantity;
            });

            return total;
           
        }

        $s.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        function constructor(){
            if (!authService.isLoggedIn()) {
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                orderService.getOrderByUserId(tokenDecode.Id)
                .then(function(response){
                    $s.orders = response.data;
                    console.log($s.orders);
                }, function(response){
                    console.log(response.data);
                })
            }
        }
        constructor();
    }
    purchaseController.$inject = ['apiUrl','priceFactory','$location','orderService','jwtHelper','authService', '$scope'];
    angular.module("app").controller("purchaseController", purchaseController);
}())