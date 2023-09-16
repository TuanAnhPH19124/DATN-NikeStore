(function () {
    var wishListService = function ($http, apiUrl) {
        this.getWishLists = function (id) {
            let uri = apiUrl + '/api/wishlists/' + id;
            return $http({
                method: 'GET',
                url: uri
            });
        };

        this.addNewWishList = function (data){
            let uri = apiUrl + '/api/wishlists/';
            return $http({
                method: 'POST',
                url: uri,
                data : data
            })
        }
        this.removeWish = function (appUserId, productId){
            let uri = apiUrl + '/api/wishlists/' + appUserId + '/' + productId;
            return $http({
                method: 'DELETE',
                url: uri
            })
        }
    }
    wishListService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("wishListService", wishListService);
}());