(function () {
    var userController = function (
        s, l,
        authService,
        userService,
        jwtHelper
    ) {

        s.userInfor = {};
        s.changePasswords = {};
        s.erreMsg1 = '';
        s.erreMsg2 = '';
        s.erreMsg3 = '';
        s.newPassWordError = {};
        s.errorMsgEmail = '';
        s.emailConfirm = '';

        s.signOutE = function () {
            authService.setLogOut();
            authService.clearSession();
            l.path('/');
        }

        function kiemTraMatKhau(matKhau) {
            // Kiểm tra độ dài tối thiểu
            if (matKhau.length < 8) {
                s.newPassWordError.length = "Mật khẩu phải có ít nhất 8 ký tự.";
            }

            // Kiểm tra chữ cái thường từ a-z
            const chuCaiThuong = /[a-z]/;
            if (!chuCaiThuong.test(matKhau)) {
                s.newPassWordError.lowercase = "Mật khẩu phải chứa ít nhất một chữ cái thường (a-z).";
            }

            // Kiểm tra chữ cái in hoa từ A-Z
            const chuCaiInHoa = /[A-Z]/;
            if (!chuCaiInHoa.test(matKhau)) {
                s.newPassWordError.uppercase = "Mật khẩu phải chứa ít nhất một chữ cái in hoa (A-Z).";
            }

            // Kiểm tra chữ số từ 0-9
            const chuSo = /[0-9]/;
            if (!chuSo.test(matKhau)) {
                s.newPassWordError.digit = "Mật khẩu phải chứa ít nhất một chữ số (0-9).";
            }

            // Kiểm tra kí tự đặc biệt (có thể thay đổi dấu hiệu đặc biệt theo yêu cầu)
            const kiTuDacBiet = /[!@#$%^&*()_+{}\[\]:;<>,.?~\\-]/;
            if (!kiTuDacBiet.test(matKhau)) {
                s.newPassWordError.special = "Mật khẩu phải chứa ít nhất một kí tự đặc biệt.";
            }

            // Nếu không có lỗi nào, trả về null để biểu thị mật khẩu hợp lệ
            if (Object.keys(s.newPassWordError).length === 0) {
                return null;
            }

            return Object.keys(s.newPassWordError).length;
        }

      

      

        s.changPassword = function () {
            s.newPassWordError = {};
            s.erreMsg1 = s.erreMsg2 = s.erreMsg3 = '';
            if (s.changePasswords.oldchangePassword === undefined || s.changePasswords.oldchangePassword === '') {
                s.erreMsg1 = "Không được bỏ trống mật khẩu cũ";
                return;
            }

            if (s.changePasswords.newchangePassword === undefined || s.changePasswords.newchangePassword === '') {
                s.erreMsg2 = "Không được bỏ trống mật khẩu mới";
                return;
            }

            if (s.changePasswords.cofirmchangePassword === undefined || s.changePasswords.cofirmchangePassword === '') {
                s.erreMsg3 = "Không được bỏ trống xác nhận mật khẩu";
                return;
            }

            if (s.changePasswords.newchangePassword === s.changePasswords.oldchangePassword) {
                console.log('Mật khẩu mới phải khác mật khâu cữ');
                s.changePasswords.newchangePassword = '';
                s.changePasswords.cofirmchangePassword = '';

                s.erreMsg2 = 'Mật khẩu mới phải khác mật khâu cữ';
                return;
            }
            if (s.changePasswords.newchangePassword !== s.changePasswords.cofirmchangePassword) {
                console.log('Mật khẩu xác nhận không khớp mật khẩu mới');
                s.changePasswords.cofirmchangePassword = '';
                s.erreMsg3 = 'Mật khẩu xác nhận không khớp mật khẩu mới';
                return;
            }
            if (kiemTraMatKhau(s.changePasswords.newchangePassword) !== null) {
                return;
            }

            let data = {
                userName: authService.getUserName(),
                currentPassword: s.changePasswords.oldchangePassword,
                newPassword: s.changePasswords.newchangePassword
            }
            authService.changePassword(data)
                .then(function (response) {
                    alert('Đổi mật khẩu thành công')
                    authService.setLogOut();
                    authService.clearSession();
                    l.path('/signin');
                }, function (response) {

                })


        }

        s.updateInfor = function () {
            console.log(s.userInfor.fullName);
            if (s.userInfor.fullName === '') {
                alert("Không được để trống họ tên");
                return;

            }
            if (s.userInfor.userName === '') {
                alert("Không được để trống tên tài khoản");
                return;

            }
            if (s.userInfor.phoneNumber === '') {
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

            s.changePassword1 = function () {
                if (s.changePassword.oldchangePassword === "") {
                    alert("Nhập mật khẩu cũ");
                    return;
                }
                if (s.changePassword.newchangePassword === "") {
                    alert("Nhập mật khẩu mới");
                    return;
                }
                if (s.changePassword.cofirmchangePassword === "") {
                    alert("Xác nhận mật khẩu mới");
                    return;
                }
            }


            userService.updateUserInfo(tokenDecode.Id, data)
                .then(function (response) {
                    userService.getUserInfomation(tokenDecode.Id)
                        .then(function (response) {
                            s.userInfor = response.data;
                            alert("Cập nhật thành công!")
                        }, function (response) {
                            console.error(response.data);
                        })
                }, function (response) {
                    console.error(response.data);
                })



        }

        function constructor() {
            if (!authService.isLoggedIn()) {
                l.path('/signin');
                return;
            };
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            userService.getUserInfomation(tokenDecode.Id)
                .then(function (response) {
                    s.userInfor = response.data;
                    console.log(s.userInfor);
                }, function (response) {
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