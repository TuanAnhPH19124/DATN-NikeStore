(function () {
    var headerController = function (l,e, wishListService, authService, jwtHelper, headerFactory){
        e.searchKeyword = '';

        e.getUserName = function () {
            return authService.getUserName();
        }

        e.isLogin = function () {
            return authService.isLoggedIn();
        };

        e.handleKeyPress  = function (event){
            if (event.which === 13){
                e.searchE();
            }   
        }
        
        e.searchE = function (){
          
                console.log(e.searchKeyword);
                l.path('/product').search({ search_keyword: e.searchKeyword});
            

        }
        
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
    headerController.$inject = ['$location', '$scope', 'wishListService', 'authService', 'jwtHelper', 'headerFactory'];
    angular.module("app").controller("headerController", headerController);
}());