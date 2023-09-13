(function (){
    var userService = function (
        apiUrl,
        http
    ){
        this.getUserInfomation = function (id){
            let uri = apiUrl + '/api/AppUser/Get/' + id;
            return http({
                method: 'GET',
                url: uri
            });
        };
    }

    userService.$inject = [
        'apiUrl',
        '$http'
    ];
    angular.module("app").service("userService", userService);
}())