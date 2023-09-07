(function(){
    var cartService = function (http, apiUrl) {

        this.getCarts = function (userId){
            let uri = apiUrl + "/api/ShoppingCarts/" + userId;
            return http({
                method: 'GET',
                url: uri
            })
        }
        this.addToCarts = function (data) {
            let uri = apiUrl + "/api/ShoppingCarts";
            return http({
                method: 'POST',
                url: uri,
                data: data
            }) 
        };

        this.clearCart = function (id){
            let uri = apiUrl + "/api/ShoppingCarts/clear/" + id;
            return http({
                method: 'DELETE',
                url: uri,
            }) 
        }
    }
    cartService.$inject = ['$http', 'apiUrl']
    angular.module("app").service("cartService", cartService);
}());