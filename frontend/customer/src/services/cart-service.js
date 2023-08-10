(function(){
    var cartService = function (http, apiUrl) {
        this.getCarts = function (userId){
            let uri = apiUrl + "/api/ShoppingCarts/" + userId;
            return http({
                method: 'GET',
                url: uri
            })
        }
    }
    cartService.$inject = ['$http', 'apiUrl']
    angular.module("app").service("cartService", cartService);
}());