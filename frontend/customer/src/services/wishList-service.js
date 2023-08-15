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
    }
    wishListService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("wishListService", wishListService);
}());