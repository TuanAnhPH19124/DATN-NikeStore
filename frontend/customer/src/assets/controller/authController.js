(function () {
    var authController = function (e,l,authService,headerFactory,jwtHelper,wishListService){
        e.loggedInStatus = false;
        e.signInE = function (user) {
            if (user !== null){
                authService.signIn(user)
                .then(function (response){
                    authService.setLoggedIn(!e.loggedInStatus);
                    authService.setToken(response.data.token);
                    // get wish list counter
                    let tokenDecode = jwtHelper.decodeToken(response.data.token);
                    wishListService.getWishLists(tokenDecode.Id)
                    .then(function (response){
                        headerFactory.setWishListCounter(response.data.length);
                    })
                    .catch(function (data){
                        console.log(data);
                    });
                    // get cart counter
                    
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
            e.loggedInStatus = authService.isLoggedIn();
        };
        constructor();
    }
    authController.$inject = ['$scope', '$location','authService', 'headerFactory', 'jwtHelper', 'wishListService'];
    angular.module("app").controller("authController", authController);

}());

