(function () {
    var wishListController = function (e, wishListService, authService, jwtHelper, headerFactory){
        e.wishListCounter = function () {
            return headerFactory.getWishListCounter();
        };
        e.cartCounter = function (){
            return headerFactory.getCartCounter();
        }
        
        function constructor(){
           
        };
        constructor();
    }
    wishListController.$inject = ['$scope', 'wishListService', 'authService', 'jwtHelper', 'headerFactory'];
    angular.module("app").controller("wishListController", wishListController);
}());