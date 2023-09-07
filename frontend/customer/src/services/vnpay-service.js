(function(){
    var vnpayService = function(http){
        this.vpcpay = function(data){
            let uri = 'https://sandbox.vnpayment.vn/paymentv2/vpcpay.html';
            return http({
                method: 'GET',
                url: uri,
                params: data
            });
        };
    };
    vnpayService.$inject = ['$http'];
    angular.module("app").service("vnpayService", vnpayService);
}())