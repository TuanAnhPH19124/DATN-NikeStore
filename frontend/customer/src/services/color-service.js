(function(){
    var colorService = function (http, apiUrl){
        this.getAllColor = function (){
            let uri = apiUrl + '/api/Color/Get';
            return http({
                method: 'GET',
                url: uri
            })
        }
    }
    colorService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("colorService", colorService);
}())