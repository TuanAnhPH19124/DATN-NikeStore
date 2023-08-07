(function () {
    var authService = function ($http, apiUrl) {
        this.signIn = function (user) {
            let uri = apiUrl + '/api/Authentication/SignIn';
            return $http({
                method: 'POST',
                url: uri,
                data: user
            });
        };
        this.signUp = function (newUser){
            let uri = apiUrl + '/api/Authentication/SignUp';
            return $http({
                method: 'POST',
                url: uri,
                data: newUser
            });
        };
    }
    authService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("authService", authService);
}());