(function () {
    var categoryService = function (http, apiUrl) {
        this.getCategories = function () {
            let uri = apiUrl + '/api/Categories';
            return http({
                method: 'GET',
                url: uri
            })
        };
    };
    categoryService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("categoryService",categoryService);
}());