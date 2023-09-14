(function () {
    var voucherService = function (http, apiUrl){
        this.getVouchers = function () {
            let uri = apiUrl + '/api/Voucher/Get';
            return http({
                method: 'GET',
                url: uri
            })
        }
    }
    voucherService.$inject = ['$http', 'apiUrl']
    angular.module("app").service("voucherService", voucherService);
}());