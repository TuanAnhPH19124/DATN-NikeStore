(function () {
    var orderController = function (e, r, l, orderFactory, productService, authService, jwtHelper) {
        e.products = [];
        e.userInfo = {};
        e.location = {
            address: null,
            city: null,
            district: null,
            ward: null
        }
        e.total = {
            cost: 0,
            shipfee: 20000,
            voucer: 0
        }
       
        function calculatCost(){
            e.total.cost = e.products.reduce(function (acc, product){
                return acc + product.retailPrice;
            }, 0);
        }

        function constructor(){
            if (authService.isLoggedIn()){
                var type = parseInt(r.type);
                if (type === 0){ // buy now
                    let productId = orderFactory.getProductId();
                    if (productId !== null){
                        productService.getProduct(productId)
                        .then(function(response){
                            debugger;
                            console.log(response.data);
                            e.products.push(response.data);
                            calculatCost();
                        })
                        .catch(function(data){
                            console.log(data);
                        });ư
                    }
                }else{
                    e.products = orderFactory.getSelectedItems();
                }

                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                authService.getUserInfomation(tokenDecode.Id)
                .then(function(response){
                    e.userInfo = response.data;
                })
                .catch(function(data){
                    console.log(data);
                });
            }else{
                l.path('/signin');
            }
            
        }

        constructor();
    }
    orderController.$inject = ['$scope', '$routeParams', '$location','orderFactory', 'productService', 'authService', 'jwtHelper'];
    angular.module("app").controller("orderController", orderController);
}());