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

        this.getOrderByUserId = function (id, type){
            let uri = apiUrl + '/api/orders/getByUserId/' + id;
            let param = {};
            if (type !== null && type !== undefined){
                param.type = type;
            }
            return http({
                method: 'GET',
                url: uri,
                params: param
            });
        };

        this.getOrderDetail = function (id){
            let uri = apiUrl + '/api/orders/getOrderDetail/' + id;
            return http({
                method: 'GET',
                url: uri
            });
        };
    };
    orderService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("orderService",orderService);
}());