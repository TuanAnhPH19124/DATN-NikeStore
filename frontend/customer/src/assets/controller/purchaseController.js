(function (){
    var purchaseController = function (l,orderService,jwtHelper,authService,$s){
        $s.orders = [];
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
    purchaseController.$inject = ['$location','orderService','jwtHelper','authService', '$scope'];
    angular.module("app").controller("purchaseController", purchaseController);
}())