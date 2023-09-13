(function (){
    var userController = function (
        s, l,
        authService,
        userService,
        jwtHelper
    ){

        s.userInfor = {};

        function constructor(){
            if (!authService.isLoggedIn()){
                l.path('/signin');
                return;
            };
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            userService.getUserInfomation(tokenDecode.Id)
            .then(function (response){
                s.userInfor = response.data;
                console.log(s.userInfor);
            }, function (response){
                console.error(response.data);
            })
        }
        constructor();
    }
    userController.$inject = [
        '$scope',
        '$location',
        'authService',
        'userService',
        'jwtHelper'
    ];
    angular.module("app").controller("userController", userController);
}())