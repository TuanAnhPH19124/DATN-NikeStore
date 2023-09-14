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
        
        this.updateUserInfo = function (id,data){
            let uri = apiUrl + '/api/AppUser/' + id;
            return http({
                method: 'PUT',
                url: uri,
                data : data
            });
        };


    }

    userService.$inject = [
        'apiUrl',
        '$http'
    ];
    angular.module("app").service("userService", userService);
}())