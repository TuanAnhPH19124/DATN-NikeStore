(function () {
    var authController = function (e, l, authService, headerFactory, jwtHelper, wishListService, cartService) {
        e.loggedInStatus = false;
        e.emailConfirm = '';
        e.errorMsgEmail = '';
        e.errorMsgUserAccount = '';
        e.errorMsgUserPassword = '';
        e.errorMsgSignUPFN = '';
        e.errorMsgSignUPE = '';
        e.errorMsgSignUPPN = '';
        e.errorMsgSignUPP = '';
        e.errorMsgSignUPPC = '';
        e.newPassWordError = '';

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
                .catch(function (ressponse) {
                    console.log(ressponse);
                    if(ressponse.data.status === 401)
                        e.errorMsgUserPassword = "Mật khẩu của bạn chưa chính xác!";
                    if (ressponse.status === 404)
                        e.errorMsgUserAccount = "Tài khoản không tồn tại!"
                    if (ressponse.status === 400)
                        alert("Tài khoản này không thể truy cập");
                });

        };

        function kiemTraMatKhau(matKhau) {
            // Kiểm tra độ dài tối thiểu
            if (matKhau.length < 8) {
                e.newPassWordError.length = "Mật khẩu phải có ít nhất 8 ký tự.";
            }

            // Kiểm tra chữ cái thường từ a-z
            const chuCaiThuong = /[a-z]/;
            if (!chuCaiThuong.test(matKhau)) {
                e.newPassWordError.lowercase = "Mật khẩu phải chứa ít nhất một chữ cái thường (a-z).";
            }

            // Kiểm tra chữ cái in hoa từ A-Z
            const chuCaiInHoa = /[A-Z]/;
            if (!chuCaiInHoa.test(matKhau)) {
                e.newPassWordError.uppercase = "Mật khẩu phải chứa ít nhất một chữ cái in hoa (A-Z).";
            }

            // Kiểm tra chữ số từ 0-9
            const chuSo = /[0-9]/;
            if (!chuSo.test(matKhau)) {
                e.newPassWordError.digit = "Mật khẩu phải chứa ít nhất một chữ số (0-9).";
            }

            // Kiểm tra kí tự đặc biệt (có thể thay đổi dấu hiệu đặc biệt theo yêu cầu)
            const kiTuDacBiet = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/;
            if (!kiTuDacBiet.test(matKhau)) {
                e.newPassWordError.special = "Mật khẩu phải chứa ít nhất một kí tự đặc biệt.";
            }

            // Nếu không có lỗi nào, trả về null để biểu thị mật khẩu hợp lệ
            if (Object.keys(e.newPassWordError).length === 0) {
                return null;
            }

            return Object.keys(e.newPassWordError).length;
        }

        e.signUpE = function (newUser) {
            e.errorMsgSignUPFN = '';
            e.errorMsgSignUPE = '';
            e.errorMsgSignUPPN = '';
            e.errorMsgSignUPP = '';
            e.errorMsgSignUPPC = '';

            if (newUser.fullName === undefined || newUser.fullName === ''){
                e.errorMsgSignUPFN = "Không được bỏ trống họ và tên";
                return;
            }

            if (newUser.email === undefined || newUser.email === ''){
                e.errorMsgSignUPE = "Email không hợp lệ";
                // e.errorMsgSignUPE = "Không được bỏ trống email";
                newUser.email = ''
                return;
            }

            if (newUser.phoneNumber === undefined || newUser.phoneNumber === ''){
                e.errorMsgSignUPPN = "Không được bỏ trống số điện thoại";
                return;
            }

            if (newUser.password === undefined || newUser.password === ''){
                e.errorMsgSignUPP = "Không được bỏ trống mật khẩu";
                return;
            }
            

            if (newUser.passwordConfirm === undefined || newUser.passwordConfirm === ''){
                e.errorMsgSignUPPC = "Không được bỏ trống mật khẩu xác nhận";
                return;
            }

            if (newUser.passwordConfirm !== newUser.password){
                e.errorMsgSignUPPC = "Mật khẩu xác nhận không khớp với mật khẩu cài đặt";
                newUser.passwordConfirm = ''
                return;
            }

            if (kiemTraMatKhau(newUser.password) !== null) {
                return;
            }

            
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

