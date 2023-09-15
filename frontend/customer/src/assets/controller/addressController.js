(function (){
    var addressController = function (
        s, l,
        addressService,
        authService,
        jwtHelper,
        ghnServices
    ){
        s.addresses = [];
        s.visible = -1;
        s.address = null;
        s.districts = [];
        s.wards = [];
        s.provices = [];


        s.showAddressDetail = function (id){
            s.addresses.forEach(element => {
                if (element.id === id){
                    s.address = element;
                }
            });
            console.log(s.address);
            s.visible = 0;
        }

        s.getDistricts = function (provinceId) {
            if (provinceId !== undefined) {
                console.log(provinceId)
                let data = {
                    province_id: parseInt(provinceId)
                }
                ghnServices.getDistrict(data)
                    .then(function (response) {
                        s.districts = response.data.data;
                    }, function (response) {
                        console.log(response.data);
                    });
            } else {
                s.districts = [];
            }

        }

        s.getWards = function (districtId) {
            if (districtId !== undefined) {
                console.log(districtId)
                let data = {
                    district_id: parseInt(districtId)
                }
                ghnServices.getWards(data)
                    .then(function (response) {
                        s.wards = response.data.data;
                        console.log(e.wards);
                    }, function (response) {
                        console.log(response.data);
                    });
            } else {
                s.wards = [];
            }
        }

        function constructor(){
            if (!authService.isLoggedIn()){
                l.path('/signin');
            }else{
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                addressService.getAdresses(tokenDecode.Id)
                .then(function (response){
                    s.addresses = response.data;
                    console.log(s.addresses)
                }, function (response){
                    console.error(response.data);
                });

                ghnServices.getProvinces()
                .then(function (response) {
                    s.provices = response.data.data;
                }, function (response) {
                    console.log(response.data);
                });
            }
        }
        constructor();
    }
    addressController.$inject = [
        '$scope',
        '$location',
        'addressService',
        'authService',
        'jwtHelper',
        'ghnServices'
    ]
    angular.module("app").controller("addressController", addressController);
}())