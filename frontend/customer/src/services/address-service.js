(function (){
    var addressService = function (http, url){
        this.getAdresses = function (userId){
            let uri = url + "/api/Address" + userId;
            return http({
                method: 'GET',
                url: uri
            });
        };

        this.addAddress = function (address){
            let uri = url + "/api/Address";
            return http({
                method: 'POST',
                url: uri,
                data: address
            });
        };
    }
    addressService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("addressService", addressService);
}())