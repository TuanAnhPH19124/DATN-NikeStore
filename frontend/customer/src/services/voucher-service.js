(function () {
    var voucherService = function (http, apiUrl){
        this.confirmVoucher = function (code) {
            let uri = apiUrl + '';
        }
    }
    voucherService.$inject = ['$http', 'apiUrl']
}());