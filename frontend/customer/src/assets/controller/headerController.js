(function () {
    var headerController = function (e, wishListService, authService, jwtHelper, headerFactory){
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
    headerController.$inject = ['$scope', 'wishListService', 'authService', 'jwtHelper', 'headerFactory'];
    angular.module("app").controller("headerController", headerController);
}());