(function () {
    var homeController = function (e, l,jwtHelper, productService, authService, wishListService, cartService,headerFactory, orderFactory){
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
                authService.scheduleClearEvent();
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

        e.addToCartsE = function (productId){
            if (!authService.isLoggedIn()){
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.CART,
                    data: {
                        appUserId: "",
                        shoppingCartItemsDto: {
                            quantity: 1,
                            productId: productId,
                            isQuantity: true
                        }
                    }
                }
                authService.setEventAfterLogin(newE);
                authService.scheduleClearEvent();
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let data = {
                    appUserId: tokenDecode.Id,
                    shoppingCartItemsDto: {
                        quantity: 1,
                        productId: productId,
                        isQuantity: true
                    }
                };
                cartService.addToCarts(data)
                .then(function (response){
                    console.log(`Add to cart success: ${response.status}`)
                })
                .catch(function(data){
                    console.log(data);
                });
            }
        }

        e.buyNow = function (productId){
            let isLoggedIn = authService.isLoggedIn();
            orderFactory.setProductId(productId);
            if (isLoggedIn){
                l.path('/pay/0');
            }else{
                let enumType = authService.getEnum();
                let newE = {
                    enum: enumType.BUYNOW,
                    ridirectTo: '/pay/0',
                };
                authService.setEventAfterLogin(newE);
                authService.scheduleClearEvent();
                l.path('/signin');
            }
            
        }



        function constructor(){
            let isLoggedIn = authService.isLoggedIn();
            if (isLoggedIn){
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
                        }else if (event.enum === 1){ // add to cart
                            event.data.appUserId = tokenDecode.Id;
                            cartService.addToCarts(event.data)
                            .then(function(response){
                                console.log( `Add to cart success: ${response.status}`)
                            })
                            .catch(function(data){
                                console.log(data);
                            });
                        }else{ // buy now
                            l.path('/pay/0');
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
    homeController.$inject = ['$scope', '$location', 'jwtHelper','productService', 'authService', 'wishListService', 'cartService','headerFactory', 'orderFactory'];
    angular.module("app").controller("homeController", homeController);
}());