(function () {
    var authService = function ($http, apiUrl) {
        const enumEvent = {
            WISHLIST: 0,
            CART: 1
        }

        var session  = {
            login: false,
            token: "",
        }
        var eAfterSignIn = [];

        this.clearEvent = function () {
            eAfterSignIn = [];
        }
        this.eventAfterSignedIn = function (){
            var event = eAfterSignIn.length;
            if (event > 0){
                return true;
            }
            return false;
        }
        this.getEventAfterSignedIn = function () {
            return eAfterSignIn;
        }
        this.setEventAfterLogin = function(newE){
            eAfterSignIn.push(newE);
        }
        this.getEnum = function (){
            return enumEvent;
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