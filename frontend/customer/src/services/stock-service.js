(function (){
    var stockService = function (http, apiUrl){
        this.getStockByProductId = function (id){
            let uri = apiUrl + '/api/Stock/' + id;
            return http({
                method: 'GET',
                url: uri
            });
        };

        this.getStockId = function (data){
            let uri = apiUrl + '/api/Stock/getStockId';
            return http({
                method: 'POST',
                url: uri,
                data: data
            })
        }
    };
    stockService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("stockService", stockService);
}())