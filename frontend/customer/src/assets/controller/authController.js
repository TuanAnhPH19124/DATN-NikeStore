(function () {
    var authController = function (e,l,authService){
        e.signInE = function (user) {
            if (user !== null){
                console.log(user);
                authService.signIn(user)
                .then(function (response){
                    console.log(response);
                    l.path('/');
                })
                .catch(function (data, status, header, configuration){
                    console.log(status);
                });
            }else {
                console.log('Wrong Credential!');
            }
        };
        e.signUpE = function (newUser){
            if (newUser !== null){
                console.log(newUser);
                authService.signUp(newUser)
                .then(function (response){
                    console.log(response);
                    l.path('/');
                })
                .catch(function (data, status, header, configuration){
                    console.log(data);
                });
            }else {
                console.log('Wrong Credential!');
            }
        };
        function constructor(){
        };
        constructor();
    }
    authController.$inject = ['$scope', '$location','authService'];
    angular.module("app").controller("authController", authController);

}());

