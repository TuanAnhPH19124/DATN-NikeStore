(function () {
    var orderService = function (http, apiUrl) {
        this.order = function () {
            let uri = apiUrl + '';
        };
    };
    orderService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("orderService",orderService);
}());