(function () {
    var authService = function ($http, apiUrl) {
        var session  = {
            login: false,
            token: "",
        }
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
        this.isLoggedIn = function () {
            return session.login;
        }
        this.getToken = function () {
            return session.token;
        }
        this.setLoggedIn = function (status) {
            session.login = status;
        }
        this.setToken = function (token) {
            session.token = token;
        }
    }
    authService.$inject = ['$http', 'apiUrl'];
    angular.module("app").service("authService", authService);
}());