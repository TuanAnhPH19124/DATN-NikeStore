(function () {
    var orderController = function (
        voucherService,
        e, r, l, orderFactory, productService, authService, jwtHelper, cartService, apiUrl, addressService, ghnServices,
        guidService,
        cloudFlareService,
        vnpayService,
        $window,
        orderService
    ) {
        // const uuid = require('uuid');
        // e.uuid = require('uuid');
        e.searchKeyWord = '';
        e.vouchers = [];
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
        e.selectedShippingServiceIndex = -1;
        e.cdn_cgi_trace = null;
        e.selectedVoucher = '';
        e.voucherValue = 0;
        e.showDiscountList = -1;


        e.freeShip = function () {
            if (e.subtotal() >= 5000000 && e.avalibleShippingService.length > 0 && e.selectedShippingServiceIndex !== -1)
                return e.avalibleShippingService[e.selectedShippingServiceIndex].totalFee * -1;
            else
                return 0;
        }

        e.setSelectedVoucher = function (id) {
            e.selectedVoucher = id;
            if (e.vouchers.length > 0) {
                e.vouchers.forEach(item => {
                    if (item.id === id) {
                        e.voucherValue = e.subtotal() * item.value / 100 * -1;
                        e.searchKeyWord = item.code;
                    }
                })
            }
            console.log(e.selectedVoucher);
        }

        e.searchOnChange = function () {
            if (e.searchKeyWord === '') {
                voucherService.getVouchers()
                    .then(function (response) {
                        e.vouchers = response.data;
                    }, function (response) {
                        console.error(response.data);
                    })
            } else {
                voucherService.getVoucherByCode(e.searchKeyWord)
                    .then(function (response) {
                        e.vouchers = response.data;
                    }, function (response) {
                        console.error(response.data);
                    })
            }
        }

        e.calculateDate = function tinhKhoangThoiGian(ngayCanTinh) {
            // Ngày hiện tại
            const ngayHienTai = new Date();

            // Ngày cần tính khoảng thời gian
            const ngayCanTinhDate = new Date(ngayCanTinh);

            // Tính khoảng thời gian
            const khoangThoiGian = ngayCanTinhDate - ngayHienTai;

            // Chuyển khoảng thời gian thành giờ, phút, giây
            const millisecondsInSecond = 1000;
            const secondsInMinute = 60;
            const minutesInHour = 60;
            const hoursInDay = 24;

            const milliseconds = khoangThoiGian % millisecondsInSecond;
            const totalSeconds = Math.floor(khoangThoiGian / millisecondsInSecond);
            const seconds = totalSeconds % secondsInMinute;
            const totalMinutes = Math.floor(totalSeconds / secondsInMinute);
            const minutes = totalMinutes % minutesInHour;
            const totalHours = Math.floor(totalMinutes / minutesInHour);
            const hours = totalHours % hoursInDay;
            const days = Math.floor(totalHours / hoursInDay);

            // return {
            //     days,
            //     hours,
            //     minutes,
            //     seconds,
            //     milliseconds,
            // };
            return `${days > 0 ? days + ' ngày, ': ''}${hours > 0 ? hours + ' giờ, ': ''}${minutes > 0? minutes + ' phút, ': ''}`;
        }
 

        e.getExpiredDate = function (futureTime) {
            let currentDate = new Date();
            let futureDate = new Date(futureTime);

            let timeDiff = futureDate.getTime() - currentDate.getTime();
            let dayDiff = Math.floor(timeDiff / (1000 * 3600 * 24));
            return dayDiff;
        }

        e.selectShippingService = function (service) {
            e.avalibleShippingService.forEach(item => {
                item.selected = false;
            })
            e.selectedShippingServiceIndex = e.avalibleShippingService.indexOf(service);
            e.avalibleShippingService[e.selectedShippingServiceIndex].selected = true;
        }

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
            let provinceName = e.provices.filter(item => item.ProvinceID === parseInt(newAddress.cityCode)).map(item => item.NameExtension[1]);
            let districtName = e.districts.filter(item => item.DistrictID === parseInt(newAddress.provinceCode)).map(item => item.NameExtension[0]);
            let wardName = e.wards.filter(item => item.WardCode === newAddress.wardCode).map(item => item.NameExtension[0]);

            newAddress.userId = tokenDecode.Id;

            newAddress.addressLine += `, ${wardName}, ${districtName}, ${provinceName}`;
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
            e.selectedShippingServiceIndex = -1;
            ghnServices.getAvalibleServices(data)
                .then(function (response) {
                    e.avalibleShippingService = response.data.data;
                    e.avalibleShippingService.forEach(item => {
                        let data = e.getAvaliableServiceFeeData(item.service_type_id);
                        ghnServices.getServiceFee(data)
                            .then(function (response) {
                                item.totalFee = response.data.data.total;
                            }, function (response) {
                                console.log(response);
                            })
                        data = e.getLeadTimeData(item.service_id);
                        ghnServices.getLeadTime(data)
                            .then(function (response) {
                                item.leadTime = response.data.data.leadtime;
                            }, function (response) {
                                console.log(response.data);
                            })
                        item.selected = false;
                    })
                }, function (response) {
                    console.log(response.data);
                })
        }

        e.convertTimestampToDateString = function (timestamp) {
            const daysOfWeek = ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'];

            const date = new Date(timestamp * 1000); // Lưu ý: timestamp được biểu diễn bằng mili giây nên nhân với 1000
            const year = date.getFullYear();
            const month = (date.getMonth() + 1).toString().padStart(2, ''); // Tháng bắt đầu từ 0, nên cộng thêm 1
            const day = date.getDate().toString().padStart(2, '');
            const dayOfWeek = daysOfWeek[date.getDay()];
            // Trả về định dạng ngày tháng
            return `${dayOfWeek}, Ngày ${day} tháng ${month} năm ${year}`;
        }

        e.getAvaliableServiceFeeData = function (service_type_id) {
            var data = null;
            if (service_type_id !== undefined && e.avalibleShippingService.length !== 0) {
                data = {
                    "service_type_id": service_type_id,
                    "from_district_id": e.shop.shops[0].district_id,
                    "from_ward_code": e.shop.shops[0].ward_code,
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
            }
            return data;
        }

        e.getLeadTimeData = function (service_id) {
            var data = {
                "from_district_id": e.shop.shops[0].district_id,
                "from_ward_code": e.shop.shops[0].ward_code,
                "to_district_id": e.addressCustomer[e.selectedIndex].provinceCode,
                "to_ward_code": e.addressCustomer[e.selectedIndex].wardCode,
                "service_id": service_id
            }
            return data;

        }

        function getCurrentDateTimeInGMT7Format() {
            // const timeZoneOffSet = 7 * 60;
            const now = new Date();

            // now.setMinutes(now.getMinutes() + timeZoneOffSet);
            const year = now.getFullYear();
            const month = (now.getMonth() + 1).toString().padStart(2, '0');
            const day = now.getDate().toString().padStart(2, '0');
            const hours = now.getHours().toString().padStart(2, '0');
            const minutes = now.getMinutes().toString().padStart(2, '0');
            const seconds = now.getSeconds().toString().padStart(2, '0');
            const formatDatetime = `${year}${month}${day}${hours}${minutes}${seconds}`;
            return formatDatetime;
        }

        function getExpireDateInGMT7Format() {
            const now = new Date();

            // Thêm 15 phút
            now.setMinutes(now.getMinutes() + 15);

            // Cộng thêm múi giờ  
            const year = now.getFullYear();
            const month = (now.getMonth() + 1).toString().padStart(2, '0');
            const day = now.getDate().toString().padStart(2, '0');
            const hours = now.getHours().toString().padStart(2, '0');
            const minutes = now.getMinutes().toString().padStart(2, '0');
            const seconds = now.getSeconds().toString().padStart(2, '0');

            const expireDatetime = `${year}${month}${day}${hours}${minutes}${seconds}`;
            return expireDatetime;
        }

        e.createOrder = function (paymethod) {
            let token = authService.getToken();
            let tokenDecode = jwtHelper.decodeToken(token);
            let oItems = e.carts.map(item => {
                return {
                    productId: item.product.id,
                    colorId: item.colorId,
                    sizeId: item.sizeId,
                    unitPrice: item.product.discountRate,
                    quantity: item.quantity
                }
            });
            let data = {
                addressId: e.addressCustomer[e.selectedIndex].id,
                userId: tokenDecode.Id,
                note: null,
                voucherId: null,
                employeeId: null,
                paymentMethod: paymethod,
                amount: e.subtotal() + e.avalibleShippingService[e.selectedShippingServiceIndex].totalFee,
                orderItems: oItems
            }

            console.log(data);
            console.log(e.carts);
            orderService.createOrder(data)
                .then(function (response) {
                    cartService.clearCart(tokenDecode.Id)
                        .then(function (response) {
                            l.path('/order');
                        }, function (response) {
                            console.log(response.data);
                        })
                }, function (response) {
                    console.log(response.data);
                })
        }

        e.vnpay = function () {
            var vnp_Amount = (e.subtotal() + e.avalibleShippingService[e.selectedShippingServiceIndex].totalFee) * 100;
            var vnp_Command = 'pay';
            var vnp_CreateDate = getCurrentDateTimeInGMT7Format();
            var vnp_CurrCode = 'VND';
            var vnp_IpAddr = e.cdn_cgi_trace.ip;
            var vnp_Locale = 'vn';
            var vnp_OrderInfo = "Thanh toan hoa don " + (vnp_Amount / 100) + " VND";
            var vnp_OrderType = 'other';
            var vnp_ReturnUrl = 'https://sandbox.vnpayment.vn/tryitnow/Home/VnPayReturn';
            var vnp_TmnCode = 'S29P0U7A';
            var vnp_TxnRef = guidService.generateGuid().toString();
            var vnp_Version = '2.1.0';
            var vnp_SecureHash = 'VXTCXWAGUTDABTTTUYINYCNPQPXJLLAS';
            var vnp_ExpireDate = getExpireDateInGMT7Format();

            var urlPayRedirect = 'https://sandbox.vnpayment.vn/paymentv2/vpcpay.html' +
                '?vnp_Amount=' + vnp_Amount +
                '&vnp_Command=' + vnp_Command +
                '&vnp_CreateDate=' + vnp_CreateDate +
                '&vnp_CurrCode=' + vnp_CurrCode +
                '&vnp_IpAddr=' + vnp_IpAddr +
                '&vnp_Locale=' + vnp_Locale +
                '&vnp_OrderInfo=' + encodeURIComponent(vnp_OrderInfo) +
                '&vnp_OrderType=' + vnp_OrderType +
                '&vnp_ReturnUrl=' + encodeURIComponent(vnp_ReturnUrl) +
                '&vnp_TmnCode=' + vnp_TmnCode +
                '&vnp_TxnRef=' + vnp_TxnRef +
                '&vnp_SecureHash=' + vnp_SecureHash +
                '&vnp_ExpireDate=' + vnp_ExpireDate +
                '&vnp_Version=' + vnp_Version;

            $window.open(urlPayRedirect, '_blank');
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
                cloudFlareService.getIPAddr()
                    .then(function (response) {
                        e.cdn_cgi_trace = response.data.trim().split('\n').reduce(function (obj, pair) {
                            pair = pair.split('=');
                            return obj[pair[0]] = pair[1], obj;
                        }, {});

                    }, function (response) {
                        console.log(response);
                    })
                voucherService.getVouchers()
                    .then(function (response) {
                        e.vouchers = response.data;
                        console.log(e.vouchers);
                    }, function (response) {
                        console.error(response.data);
                    })
            } else {
                l.path('/signin');
            }

        }

        constructor();
    }
    orderController.$inject = ['voucherService', '$scope', '$routeParams', '$location', 'orderFactory', 'productService', 'authService', 'jwtHelper', 'cartService', 'apiUrl', 'addressService', 'ghnServices', 'guidService', 'cloudFlareService', 'vnpayService', '$window', 'orderService'];
    angular.module("app").controller("orderController", orderController);
}());