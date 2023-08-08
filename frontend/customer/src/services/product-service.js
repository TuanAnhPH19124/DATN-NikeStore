(function () {
    var productService = function ($http, apiUrl) {
        this.getProducts = function () {
            let uri = apiUrl + '/api/Product';
            return $http({
                method: 'GET',
                url: uri
            });
        };
    }
    productService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("productService", productService);
}());