(function (){
    var orderDetailController = function (
            s, r, l,
            orderService,
            authService,
            priceFactory,
            apiUrl,
            stockService,
            jwtHelper,
            cartService
        ){

        s.order = [];

        constructor();


        s.formatPrice = function (price){
            return priceFactory.formatVNDPrice(price);
        }

        s.sale = function (){
            if (s.order.length !== 0){
                if (s.order[0].voucherValue > 0){
                    return s.totalAmount() * s.order[0].voucherValue / 100;
                }else{
                    return 0;
                }
            }
        }

        s.totalAmount = function (){
            var total = 0;
            if(s.order.length !== 0){
                s.order[0].orderItems.forEach(element => {
                    total += element.discountRate * element.quantity;
                });
            }
            return total;
        }

        s.reBuy = function(){
            console.log('run');
            if (s.order.length !== 0){
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let data = [];
                s.order[0].orderItems.forEach(item =>{
                    let apiData = {
                        productId: item.productId,
                        colorId: item.colorId,
                        sizeId: item.sizeId
                    }
                    stockService.getStockId(apiData)
                    .then(function (response){
                        data.push({
                            appUserId: tokenDecode.Id,
                            stockId: response.data,
                            quantity: item.quantity
                        })
                    }, function(response){
                        console.error(response.data);
                    })
                })
                cartService.addRangeToCard(data)
                .then(function (response){
                    l.path('/cart');
                }, function (response){
                    console.error(response.data);
                })
                console.log(data);
            }
            
        }

        s.total = function (){
            return s.totalAmount() + s.sale() * -1;
        }

        s.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        s.redirectToOrder = function (){
            l.path('/order');
        }
        function constructor(){
            if (!authService.isLoggedIn()){
                l.path('/signin');
                return;
            }
            let id = String(r.id);
            orderService.getOrderDetail(id)
            .then(function (response){
                s.order = response.data;
                console.log(s.order);
            }, function(response) {
                console.error(response.data);
            })
        }
    }
    orderDetailController.$inject = [
        '$scope',
        '$routeParams',
        '$location',
        'orderService',
        'authService',
        'priceFactory',
        'apiUrl',
        'stockService',
        'jwtHelper',
        'cartService'
    ];
    angular.module("app").controller("orderDetailController", orderDetailController);
}())