(function () {
    var productService = function ($http, apiUrl) {
        this.getProducts = function () {
            let uri = apiUrl + '/api/Product';
            return $http({
                method: 'GET',
                url: uri
            });
        };

        this.getProduct = function (Id){
            let uri = apiUrl + '/api/Product/' + Id;
            return $http({
                method: 'GET',
                url: uri
            });
        };

        this.getProductsByParams = function (paramsF){
            let uri = apiUrl + '/api/Product/filter';
            return $http({
                method: 'GET',
                url: uri,
                params: paramsF
            });
        };

    };
    productService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("productService", productService);
}());