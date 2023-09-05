(function(){
    var ghnServices = function (http){
        const token = '5f70a241-4a7f-11ee-af43-6ead57e9219a';
        this.getProvinces = function (){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/province';
            return http({
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri
            });
        };

        this.getDistrict = function(data){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/district';
            return http({
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                params: data
            });
        };

        this.getWards = function(data){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/master-data/ward?district_id';
            return http({
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                data: data
            });
        };

        this.getShops = function (){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/v2/shop/all';
            return http({
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                data: {
                    "offset": 0,
                    "limit": 1,
                    "client_phone": ""
                }
            });
        };

        this.getAvalibleServices = function (data){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services';
            return http({
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                data: data
            });
        };

        this.getServiceFee = function(data){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee';
            return http({
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                data: data
            });
        };

        this.getLeadTime = function (data){
            let uri = 'https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/leadtime';
            return http({
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Token': token
                },
                url: uri,
                data: data
            });
        };

    }
    ghnServices.$inject = ['$http']
    angular.module("app").service("ghnServices", ghnServices);
}())