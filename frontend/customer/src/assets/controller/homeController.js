(function () {
    var homeController = function (e, l,jwtHelper, productService, authService, wishListService, headerFactory){
        e.products = [];

        e.addToWishListE = function (productId){
            if (!authService.isLoggedIn()){
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.WISHLIST,
                    data: {
                        productsId: productId,
                        appUserId: "",
                    }
                }
                authService.setEventAfterLogin(newE);
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let data = {
                    productsId: productId,
                    appUserId: tokenDecode.Id,
                };
                wishListService.addNewWishList(data)
                .then(function (response){
                    console.log(response.status)
                })
                .catch(function (data){
                    console.log(data);
                });
            }
        }

        function constructor(){
            let isLoggedIn = authService.isLoggedIn();
            if (isLoggedIn){
                debugger;
                var token = authService.getToken();
                var tokenDecode = jwtHelper.decodeToken(token);
                let eventAfterLoggedIn = authService.eventAfterSignedIn();
                if (eventAfterLoggedIn){
                    let events = authService.getEventAfterSignedIn();
                    events.forEach(event => {
                        if (event.enum === 0){ // add to wish list
                            event.data.appUserId = tokenDecode.Id;
                            wishListService.addNewWishList(event.data)
                            .then(function (response){
                                console.log(response.status)
                            })
                            .catch(function (data){
                                console.log(data);
                            });
                        }else{ // add to cart

                        }
                    });
                    authService.clearEvent();
                }
            }
            productService.getProducts()
            .then(function (response) {
                e.products = response.data;
            })
            .catch(function (data){
                console.log(data);
            });
        }   

        constructor();
    };
    homeController.$inject = ['$scope', '$location', 'jwtHelper','productService', 'authService', 'wishListService', 'headerFactory'];
    angular.module("app").controller("homeController", homeController);
}());