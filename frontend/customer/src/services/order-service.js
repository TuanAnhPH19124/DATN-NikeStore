(function () {
    var orderService = function (http, apiUrl) {
        this.createOrder = function (data) {
            let uri = apiUrl + '/api/orders/payOneline';
            return http({
                method: 'POST',
                url: uri,
                data: data
            })
        };
    };
    orderService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("orderService",orderService);
}());