(function () {
    var wishListController = function (
        s, l,
        authService,
        jwtHelper,
        wishListService,
        apiUrl
    ) {

        s.wishLists = [];

        s.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        s.removeWish = function (productId) {
            if (confirm("Bạn có muốn xóa sản phẩm này!")) {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                wishListService.removeWish(tokenDecode.Id, productId)
                    .then(function (response) {
                        alert("Xóa thành công");
                        wishListService.getWishLists(tokenDecode.Id)
                            .then(function (response) {
                                s.wishLists = response.data;
                                console.log(s.wishLists);
                            }, function (response) {
                                console.error(response.data);
                            })
                    }, function (response) {
                        console.error(response.data);
                    })
            }
        }

        function constructor() {
            if (!authService.isLoggedIn()) {
                l.path('/signin');
            } else {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                wishListService.getWishLists(tokenDecode.Id)
                    .then(function (response) {
                        s.wishLists = response.data;
                        console.log(s.wishLists);
                    }, function (response) {
                        console.error(response.data);
                    })

            }
        }
        constructor();
    }

    wishListController.$inject = [
        '$scope',
        '$location',
        'authService',
        'jwtHelper',
        'wishListService',
        'apiUrl'
    ];
    angular.module("app").controller("wishListController", wishListController);
}())