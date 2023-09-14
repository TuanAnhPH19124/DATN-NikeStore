(function (){
    var userController = function (
        s, l,
        authService,
        userService,
        jwtHelper
    ){

        s.userInfor = {};
        s.ischange = function (){

        }
        

        s.updateInfor = function (){
            console.log(s.userInfor.fullName);
            if (s.userInfor.fullName === ''){
                alert("Không được để trống họ tên");
                return;

            }
            if (s.userInfor.userName === ''){
                alert("Không được để trống tên tài khoản");
                return;
                
            }
            if (s.userInfor.phoneNumber === ''){
                alert("Không được để trống số điện thoại");
                return;

            }
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            let data = {
                fullName: s.userInfor.fullName,
                userName: s.userInfor.userName,
                phoneNumber: s.userInfor.phoneNumber,
                id: s.userInfor.id
            };

            userService.updateUserInfo(tokenDecode.Id, data)
            .then(function(response){
                userService.getUserInfomation(tokenDecode.Id)
                .then(function (response){
                    s.userInfor = response.data;
                    alert("Cập nhật thàng công!")
                }, function (response){
                    console.error(response.data);
                })
            }, function (response){
                console.error(response.data);
            })



        }

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