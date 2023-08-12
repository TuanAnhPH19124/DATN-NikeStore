(function () {
    var authService = function ($http, $timeout, apiUrl) {
        const enumEvent = {
            WISHLIST: 0,
            CART: 1,
            BUYNOW: 2
        }

        var session  = {
            login: false,
            token: "",
        }
        var eAfterSignIn = [];
        var clearEventPromise = null;

        this.removeExistEnumType = function (i,newE) {
            eAfterSignIn.splice(i, 1, newE);
        }

        this.scheduleClearEvent = function (){
            if (clearEventPromise){
                $timeout.cancel(clearEventPromise);
            }
            clearEventPromise = $timeout(this.clearEvent, 60*1000);
        }

        this.clearEvent = function () {
            eAfterSignIn = [];
            if (clearEventPromise){
                $timeout.cancel(clearEventPromise);
            };
            console.log(`Clear events successfully!. Event = ${eAfterSignIn.length}`);
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
        this.getUserInfomation = function (Id){
            let uri = apiUrl + '/api/AppUser/Get/' + Id;
            return $http({
                method: 'GET',
                url: uri
            })
        }
    }
    authService.$inject = ['$http', '$timeout', 'apiUrl'];
    angular.module("app").service("authService", authService);
}());