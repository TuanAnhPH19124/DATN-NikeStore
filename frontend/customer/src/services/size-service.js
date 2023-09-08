(function(){
    var sizeService = function (http, apiUrl){
        this.getAllSize = function (){
            let uri = apiUrl + '/api/Size/Get';
            return http({
                method: 'GET',
                url: uri
            })
        }

        this.getSizeForProduct = function (productId, colorId){
            let uri = apiUrl + '/api/Size/GetSizeForProduct/' + productId + '/' + colorId;
            return http({
                method: 'GET',
                url: uri
            })
        }
    }
    sizeService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("sizeService", sizeService);
}())