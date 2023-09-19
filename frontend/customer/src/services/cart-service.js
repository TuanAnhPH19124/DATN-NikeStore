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
            });
        };

        this.addRangeToCard = function (data){
            let uri = apiUrl + '/api/ShoppingCarts/addrange';
            return http({
                method: 'POST',
                url: uri,
                data: data
            })
        }

        this.updateCart = function (id,data){
            let uri = apiUrl + '/api/ShoppingCarts/' + id;
            return http({
                method: 'PUT',
                url: uri,
                data: data
            });
        };

        

        this.deleteCart = function (id){
            let uri = apiUrl + '/api/ShoppingCarts/' + id;
            return http({
                method: 'DELETE',
                url: uri
            });
        };

        this.clearCart = function (data){
            let uri = apiUrl + "/api/ShoppingCarts/clear";
            return http({
                method: 'POST',
                url: uri,
                data: data
            }) 
        }
    }
    cartService.$inject = ['$http', 'apiUrl']
    angular.module("app").service("cartService", cartService);
}());