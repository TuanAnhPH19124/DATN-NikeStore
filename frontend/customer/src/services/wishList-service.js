(function () {
    var wishListService = function ($http, apiUrl) {
        this.getWishLists = function (id) {
            let uri = apiUrl + '/api/wishlists/' + id;
            return $http({
                method: 'GET',
                url: uri
            });
        };
    }
    wishListService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("wishListService", wishListService);
}());