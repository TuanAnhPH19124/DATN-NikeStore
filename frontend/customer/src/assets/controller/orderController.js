(function () {
    var orderController = function (e, r, l, orderFactory, productService, authService, jwtHelper, cartService, apiUrl, addressService, ghnServices) {
        e.step = 1;
        e.addressCustomer = [];
        e.userInformation = {};
        e.carts = [];
        e.showForm = false;
        e.agree = false;
        e.selectedIndex = -1;
        e.provices = [];
        e.districts = [];
        e.wards = [];
        e.shop = {};
        e.avalibleShippingService = [];
        e.totalServiceFee = 0;

        e.selectAddress = function (id) {
            for (let i = 0; i < e.addressCustomer.length; i++) {
                if (e.addressCustomer[i].id === id) {
                    e.addressCustomer[i].selected = true;
                    e.selectedIndex = i;
                    e.userInformation.phoneNumber = e.addressCustomer[i].phoneNumber;
                } else {
                    e.addressCustomer[i].selected = false;
                }
            }
        }

        e.updateAddress = function (event) {
            event.stopPropagation();
            console.log('b');
        }

        e.deliveryFee = function () {
            if (e.subtotal() > 5000000)
                return 0;
            else
                return 250000;
        }

        e.reset = function (form) {
            if (form) {
                form.$setPristine();
                form.$setUntouched();
            }
            e.user = {};
            console.log('tun')
        };

        e.getImgUrl = function (path) {
            const imgUrl = new URL(path, apiUrl);
            return imgUrl.href;
        }

        e.save = function (newAddress) {
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            newAddress.userId = tokenDecode.Id;
            addressService.addAddress(newAddress)
                .then(function (response) {
                    alert('Thêm địa chỉ thành công');
                    e.showForm = false;
                    e.getAddresses();
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        e.subtotal = function () {
            let total = 0;
            e.carts.forEach(item => {
                total += item.quantity * item.product.discountRate;
            });
            return total;
        }

        e.getAddresses = function () {
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            addressService.getAdresses(tokenDecode.Id)
                .then(function (response) {
                    e.addressCustomer = response.data;
                    e.addressCustomer.forEach(item => {
                        if (item.setAsDefault === true) {
                            item.selected = true;
                            e.selectedIndex = e.addressCustomer.indexOf(item);
                            e.userInformation.phoneNumber = item.phoneNumber;
                        } else {
                            item.selected = false;
                        }
                    });
                    console.log(e.addressCustomer);
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        e.getDistricts = function (provinceId) {
            if (provinceId !== undefined) {
                console.log(provinceId)
                let data = {
                    province_id: parseInt(provinceId)
                }
                ghnServices.getDistrict(data)
                    .then(function (response) {
                        e.districts = response.data.data;
                        console.log(e.districts);
                    }, function (response) {
                        console.log(response.data);
                    });
            } else {
                e.districts = [];
            }

        }

        e.getWards = function (districtId) {
            if (districtId !== undefined) {
                console.log(districtId)
                let data = {
                    district_id: parseInt(districtId)
                }
                ghnServices.getWards(data)
                    .then(function (response) {
                        e.wards = response.data.data;
                        console.log(e.wards);
                    }, function (response) {
                        console.log(response.data);
                    });
            } else {
                e.wards = [];
            }
        }

        e.getUserInformation = function (id) {
            authService.getUserInfomation(id)
                .then(function (response) {
                    e.userInformation = response.data;
                    if (e.addressCustomer.length !== 0)
                        e.userInformation.phoneNumber = e.addressCustomer[e.selectedIndex].phoneNumber;
                })
                .catch(function (data) {
                    console.log(data);
                });
        }

        e.nextToShippingService = function () {
            let data = {
                "shop_id": e.shop.shops[0]._id,
                "from_district": e.shop.shops[0].district_id,
                "to_district": e.addressCustomer[e.selectedIndex].provinceCode
            }
            ghnServices.getAvalibleServices(data)
                .then(function (response) {
                    e.avalibleShippingService = response.data.data;
                    e.avalibleShippingService.forEach(item => {
                        e.getAvaliableServiceFee(item.service_type_id);
                        item.totalFee = e.totalServiceFee;
                    })
                }, function (response) {
                    console.log(response.data);
                })
        }

        e.getAvaliableServiceFee = function (service_type_id) {
            return new Promise(function (resolve, reject){
                if (service_type_id !== undefined && e.avalibleShippingService.length !== 0) {
                    let data = {
                        "service_type_id": service_type_id,
                        "from_district_id": e.shop.shops[0].district_id,
                        "from_ward_code" : e.shop.shops[0].ward_code,
                        "to_district_id": e.addressCustomer[e.selectedIndex].provinceCode,
                        "to_ward_code": e.addressCustomer[e.selectedIndex].wardCode,
                        "height": 20,
                        "length": 30,
                        "weight": 2000,
                        "width": 40,
                        "insurance_value": 0,
                        "coupon": null,
                    };
                    if (service_type_id === 5) {
                        data.items = e.carts.map(item => {
                            return {
                                "name": item.product.name,
                                "quantity": item.quantity,
                                "height": 20,
                                "weight": 2000,
                                "length": 20,
                                "width": 20
                            }
                        });
                    }
                 
                    ghnServices.getServiceFee(data)
                    .then(function (response){
                        resolve(response.data.data.total);
                    }, function(response){
                        console.log(response);
                        reject(response);
                    })
                }
            });
        }

        function constructor() {
            if (authService.isLoggedIn()) {
                let token = authService.getToken();
                let tokenDecode = jwtHelper.decodeToken(token);
                // lấy dữ liệu giỏ hàng
                cartService.getCarts(tokenDecode.Id)
                    .then(function (response) {
                        e.carts = response.data;
                    })
                    .catch(function (data) {
                        console.log(data);
                    });
                e.getAddresses();
                e.getUserInformation(tokenDecode.Id);
                ghnServices.getProvinces()
                    .then(function (response) {
                        e.provices = response.data.data;
                    }, function (response) {
                        console.log(response.data);
                    });
                ghnServices.getShops()
                    .then(function (response) {
                        e.shop = response.data.data;
                    }, function (response) {
                        console.log(response.data);
                    })
            } else {
                l.path('/signin');
            }

        }

        constructor();
    }
    orderController.$inject = ['$scope', '$routeParams', '$location', 'orderFactory', 'productService', 'authService', 'jwtHelper', 'cartService', 'apiUrl', 'addressService', 'ghnServices'];
    angular.module("app").controller("orderController", orderController);
}());