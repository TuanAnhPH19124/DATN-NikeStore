(function () {
    var authController = function (e, l, authService, headerFactory, jwtHelper, wishListService, cartService) {
        e.loggedInStatus = false;
        e.emailConfirm = '';
        e.errorMsgEmail = '';
        e.errorMsgUserAccount = '';
        e.errorMsgUserPassword = '';
      


        e.signInE = function (user) {
            e.errorMsgUserAccount = '';
            e.errorMsgUserPassword = '';

            if (user.account === undefined || user.account === '') {
                e.errorMsgUserAccount = "Không được bỏ trống email";
                return;
            }

            if (user.password === undefined || user.password === '') {
                e.errorMsgUserPassword = "Không được bỏ trống mật khẩu";
                return;
            }

            authService.signIn(user)
                .then(function (response) {
                    authService.setLoggedIn(!e.loggedInStatus);
                    authService.setToken(response.data.token);
                    authService.setUserName(response.data.user);
                    // get wish list counter
                    let tokenDecode = jwtHelper.decodeToken(response.data.token);
                    wishListService.getWishLists(tokenDecode.Id)
                        .then(function (response) {
                            headerFactory.setWishListCounter(response.data.length);

                        })
                        .catch(function (data) {
                            console.log(data);
                        });
                    // get cart counter
                    cartService.getCarts(tokenDecode.Id)
                        .then(function (response) {
                            console.log(response.data);
                            headerFactory.setCartCounter(response.data.length);
                        })
                        .catch(function (data) {
                            console.log(data);
                        });
                    l.path('/');
                })
                .catch(function (data, status, header, configuration) {
                    console.log(status);
                });

        };

        e.signUpE = function (newUser) {
            if (newUser !== null) {
                console.log(newUser);
                authService.signUp(newUser)
                    .then(function (response) {
                        authService.setLoggedIn(!e.loggedInStatus);
                        authService.setToken(response.data.token);
                        authService.setUserName(response.data.user);
                        console.log(response);
                        l.path('/');
                    })
                    .catch(function (data, status, header, configuration) {
                        console.log(data);
                    });
            } else {
                console.log('Wrong Credential!');
            }
        };
        e.signOutE = function () {
            authService.setLogOut();
            authService.clearSession();
            l.path('/');
        }

        e.forgottenPassword = function () {
            if (kiemTraEmail(e.emailConfirm)) {
                let data = { email: e.emailConfirm };
                authService.forgotPassword(data)
                    .then(function (response) {
                        l.path('/signin');
                    }, function (response) {
                        console.error(response.data);
                    })
            } else {
                e.emailConfirm = '';
                e.errorMsgEmail = 'Email không hợp lệ';
            }
        }

        function kiemTraEmail(email) {

            const regex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;


            return regex.test(email);
        }

        function constructor() {
            e.loggedInStatus = authService.isLoggedIn();
            let path = l.path();
            if (path === "/accountDetail") {
                if (!authService.isLoggedIn())
                    l.path('/signin');
            }
        };
        constructor();
    }
    authController.$inject = ['$scope', '$location', 'authService', 'headerFactory', 'jwtHelper', 'wishListService', 'cartService'];
    angular.module("app").controller("authController", authController);

}());

