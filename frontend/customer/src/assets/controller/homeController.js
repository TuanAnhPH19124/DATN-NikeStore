(function () {
    var homeController = function (e, jwtHelper, productService, authService, wishListService){
        e.name  = "product Controller";
        e.products = [];
        e.isLoggedIn = false;
        e.tokenDecode = {};
        e.wishListCounter = 0;
        function constructor(){
            e.isLoggedIn = authService.isLoggedIn();
            productService.getProducts()
            .then(function (response) {
                e.products = response.data;
            })
            .catch(function (data){
                console.log(data);
            });
        }   

        constructor();
    };
    homeController.$inject = ['$scope', 'jwtHelper','productService', 'authService', 'wishListService'];
    angular.module("app").controller("homeController", homeController);
}());