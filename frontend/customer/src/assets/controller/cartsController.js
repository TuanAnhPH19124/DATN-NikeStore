(function () {
    var cartsController = function (priceFactory,$window,stockService,sizeService, e, l, authService, cartService, jwtHelper, orderFactory, apiUrl) {
        e.carts = [];
        e.sizes = [];
     
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
                let data = await Promise.all(e.carts.filter(item => item.id === id).map(async (item) =>{
                    try {
                        const response = await stockService.getStockByProductId(item.product.id);
                        const matchingStock = response.data.find(stock => stock.sizeId === item.sizeId && stock.colorId === item.colorId);
                        if (matchingStock === null){
                            throw new Error("Có gì đó không đúng size này không tồn tại");
                        }
                        return { id: item.id, quantity: parseInt(item.quantity), stockId: matchingStock.stockId }; 
                    } catch (error) {
                        console.error(error);
                    }
                }));
                console.log(data);
                const response = await cartService.updateCart(id, data[0]);
                console.log('Thành công');
            } catch (error) {
                console.error(error.response.data);
            }
        }

        e.degree = function (id, plus){
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
            var confirm = $window.confirm("Bạn có chắc chắn muốn xóa mục nảy");
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