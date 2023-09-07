(function(){
    var cloudFlareService = function (http){
        this.getIPAddr = function (){
            let uri = 'https://www.cloudflare.com/cdn-cgi/trace';
            return http({
                method: 'GET',
                url: uri
            });
        }
    }
    cloudFlareService.$inject = ['$http'];
    angular.module("app").service("cloudFlareService", cloudFlareService);
}())