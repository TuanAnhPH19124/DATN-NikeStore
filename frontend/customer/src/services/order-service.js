(function () {
    var orderService = function (http, apiUrl) {
        this.createOrder = function (data) {
            let uri = apiUrl + '/api/orders/payOnline';
            return http({
                method: 'POST',
                url: uri,
                data: data
            })
        };

        this.getOrderByUserId = function (id){
            let uri = apiUrl + '/api/orders/getByUserId/' + id;
            return http({
                method: 'GET',
                url: uri
            })
        }

    };
    orderService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("orderService",orderService);
}());