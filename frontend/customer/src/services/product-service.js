(function () {
    var productService = function ($http) {
        this.getProducts = function () {
            let uri = apiUrl + '/api/Authentication/SignIn';
            return $http({
                method: 'GET',
                url: uri
            });
        };
    }
    productService.$inject = ['$http'];
    angular.module("app").service("productService", productService);
}());