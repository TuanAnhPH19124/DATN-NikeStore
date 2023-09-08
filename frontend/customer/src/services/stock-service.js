(function (){
    var stockService = function (http, apiUrl){
        this.getStockByProductId = function (id){
            let uri = apiUrl + '/api/Stock/' + id;
            return http({
                method: 'GET',
                url: uri
            });
        };
    };
    stockService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("stockService", stockService);
}())