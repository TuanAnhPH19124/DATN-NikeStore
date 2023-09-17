(function () {
    var addressController = function (
        s, l,
        addressService,
        authService,
        jwtHelper,
        ghnServices
    ) {
        s.addresses = [];
        s.visible = -1;
        s.address = null;
        s.districts = [];
        s.wards = [];
        s.provices = [];
        s.isUpdate = -1;
        s.isAddNew = -1;
        s.addressLineInput = '';

        s.signOutE = function () {
            authService.setLogOut();
            authService.clearSession();
            l.path('/');
        }

        s.convertToInt = function (string) {
            return parseInt(string);
        }

        s.addNew = function () {
            s.address = {};
            s.isUpdate = -1;
            s.isAddNew = 0;
            s.addressLineInput = '';
            s.visible = 0;
        }

        s.showAddressLine = function (){
            console.log(s.addressLineInput);

        }

        s.updateAddress = function (id) {
     
    

            if (confirm("Bạn có muốn cập nhật địa chỉ mới không?")) {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                let provinceName = s.provices.filter(item => item.ProvinceID === parseInt(s.address.cityCode)).map(item => item.NameExtension[1]);
                let districtName = s.districts.filter(item => item.DistrictID === parseInt(s.address.provinceCode)).map(item => item.NameExtension[0]);
                let wardName = s.wards.filter(item => item.WardCode === s.address.wardCode).map(item => item.NameExtension[0]);
                s.address.addressLine = `${s.addressLineInput}, ${wardName}, ${districtName}, ${provinceName}`;

                let data = s.address;
                data.userId = tokenDecode.Id;
                addressService.updateAddress(id, data)
                    .then(function (response) {
                        alert('Cập nhật địa chỉ thành công!');
                        addressService.getAdresses(tokenDecode.Id)
                            .then(function (response) {
                                s.addresses = response.data;
                            }, function (response) {
                                console.error(response.data);
                            });
                    }, function (response) {
                        console.error(response.data);
                    })
            }

        }

        s.addNewAddress = function () {
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            let provinceName = s.provices.filter(item => item.ProvinceID === parseInt(s.address.cityCode)).map(item => item.NameExtension[1]);
            let districtName = s.districts.filter(item => item.DistrictID === parseInt(s.address.provinceCode)).map(item => item.NameExtension[0]);
            let wardName = s.wards.filter(item => item.WardCode === s.address.wardCode).map(item => item.NameExtension[0]);
            s.address.addressLine = `${s.addressLineInput}, ${wardName}, ${districtName}, ${provinceName}`;

            let data = s.address;
            data.userId = tokenDecode.Id;

            addressService.addAddress(data)
                .then(function (response) {
                    alert('Thêm địa chỉ thành công');
                    addressService.getAdresses(tokenDecode.Id)
                        .then(function (response) {
                            s.addresses = response.data;
                        }, function (response) {
                            console.error(response.data);
                        });
                    s.visible = -1;
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        s.cancel = function () {
            s.visible = -1;
        }

        s.showAddressDetail = function (id) {
            s.addresses.forEach(element => {
                if (element.id === id) {
                    s.address = element;
                }
            });
            s.getDistricts(s.address.cityCode);
            s.getWards(s.address.provinceCode);
            s.addressLineInput = splitAddressLine(s.address.addressLine);
            s.visible = 0;
            s.isUpdate = 0;
            s.isAddNew = -1;
        }

        function splitAddressLine(addLine) {
            let newAddress = '';
            let arr = addLine.split(', ');
            if (arr.length > 3)
                arr.splice(-3);

            for (let i = 0; i < arr.length; i++) {
                if (i === arr.length - 1) {
                    newAddress += arr[i];
                } else {
                    newAddress += arr[i] + ', ';
                }
            }

            return newAddress;
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
                        console.log(s.districts);
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
                        console.log(s.wards);
                    }, function (response) {
                        console.log(response.data);
                    });
            } else {
                s.wards = [];
            }
        }

        function constructor() {
            if (!authService.isLoggedIn()) {
                l.path('/signin');
            } else {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                addressService.getAdresses(tokenDecode.Id)
                    .then(function (response) {
                        s.addresses = response.data;
                        console.log(s.addresses)
                    }, function (response) {
                        console.error(response.data);
                    });

                ghnServices.getProvinces()
                    .then(function (response) {
                        s.provices = response.data.data;
                        console.log(s.provices);
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