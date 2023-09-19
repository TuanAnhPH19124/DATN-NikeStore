(function () {
    var cartsController = function (priceFactory,$window,stockService,sizeService, e, l, authService, cartService, jwtHelper, orderFactory, apiUrl) {
        e.carts = [];
        e.sizes = [];
        e.cartQuantityMsgError = '';
     
        e.totalAmount = function () {
            var total = 0
            e.carts.forEach(cart => {
                total += cart.product.discountRate * cart.quantity;
            });
            return total;
        };

        e.deliveryFee = function () {
            if (e.totalAmount() > 5000000) {
                return 0;
            } else {
                return 100000;
            }
        }

        e.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        e.convertoString = function(data){
            return data + '';
        }

        e.updateCart = async function(id){
            try {
                let data = e.carts.filter(item => item.id === id).map(item => {
                    return { id: item.id, quantity: parseInt(item.quantity), productId: item.product.id, colorId: item.colorId, sizeId: item.sizeId  };  
                }) 
                console.log(data);
                const response = await cartService.updateCart(id, data[0]);
                console.log('Thành công');
            } catch (error) {
                var index = -1;
                for (let i = 0; i < e.carts.length; i++) {
                    if (e.carts[i].id === id){
                        index = i;
                        break;
                    }
                }
                e.carts[index].quantity = parseInt(e.carts[index].quantity) - 1;
                e.cartQuantityMsgError = 'Đã đạt số lượng tối đa';
                e.$apply();
            }
        }

        e.degree = function (id, plus){
            e.cartQuantityMsgError = '';
            var cart = e.carts.filter(item => item.id === id);
            var index = -1;
            for (let i = 0; i < e.carts.length; i++) {
                if (e.carts[i].id === id){
                    index = i;
                    break;
                }
            }
            if (plus){
                if (parseInt(e.carts[index].quantity) <= 9)
                    e.carts[index].quantity = parseInt(e.carts[index].quantity) + 1 + '';
            }else{
                if (parseInt(e.carts[index].quantity) > 1)
                    e.carts[index].quantity = parseInt(e.carts[index].quantity) - 1 + '';
            }
            e.updateCart(id);
        }

        e.pay = function(){
            
            l.path('/pay');
        }

        e.removeCart = function(id){
            var confirm = $window.confirm("Bạn có chắc chắn muốn xóa sản phẩm này không?");
            if (confirm){
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                cartService.deleteCart(id)
                .then(function(response){
                    cartService.getCarts(tokenDecode.Id)
                    .then(function (response){
                        e.carts = response.data;
                        e.carts.forEach(item => {
                            item.quantity += '';
                            sizeService.getSizeForProduct(item.product.id, item.colorId)
                            .then(function (response){
                                item.sizes = response.data;
                            },function(response){
                                console.error(response);
                            })
                        })
                    }, function (response){
                        console.error(response.data);
                    })
                }, function(response){
                    console.error(response.data);
                })
            }
            
        }

        e.formatPrice = function (price){
            return priceFactory.formatVNDPrice(price);
        }

        function constructor() {
            if (!authService.isLoggedIn() && e.carts !== null) {
                l.path('/signin');
            } else {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                cartService.getCarts(tokenDecode.Id)
                    .then(function (response) {
                        e.carts = response.data;
                        e.carts.forEach(item => {
                            item.quantity += '';
                            sizeService.getSizeForProduct(item.product.id, item.colorId)
                            .then(function (response){
                                item.sizes = response.data;
                            },function(response){
                                console.error(response);
                            })
                        })
                        console.log(e.carts);
                    })
                    .catch(function (data) {
                        console.log(data);
                });
            }
        }
        constructor();

    }
    cartsController.$inject = ['priceFactory','$window','stockService','sizeService','$scope', '$location', 'authService', 'cartService', 'jwtHelper', 'orderFactory', 'apiUrl'];
    angular.module("app").controller("cartsController", cartsController);
}());