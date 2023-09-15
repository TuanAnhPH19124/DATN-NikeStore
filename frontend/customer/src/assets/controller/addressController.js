(function (){
    var addressController = function (
        s, l,
        addressService,
        authService,
        jwtHelper
    ){
        s.addresses = [];
        function constructor(){
            if (!authService.isLoggedIn()){
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                addressService.getAdresses(tokenDecode.Id)
                .then(function (response){
                    s.addresses = response.data;
                }, function (response){
                    console.error(response.data);
                })
            }
        }
        constructor();
    }
    addressController.$inject = [
        '$scope',
        '$location',
        'addressService',
        'authService',
        'jwtHelper'
    ]
    angular.module("app").controller("addressController", addressController);
}())