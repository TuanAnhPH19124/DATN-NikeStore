(function (){
    var headerFactory = function () {
        var wishListCounter = 0;
        var cartCounter = 0;
        
        var factory = {};
        factory.getWishListCounter = function (){
            return wishListCounter;
        }

        factory.setWishListCounter = function (wishListLeng) {
            wishListCounter = wishListLeng;
        }

        factory.getCartCounter = function (){
            return cartCounter;
        }

        factory.setCartCounter = function (cartLength) {
            cartCounter = cartLength;
        }
        return factory;
    }
    angular.module("app").factory("headerFactory", headerFactory);
}());