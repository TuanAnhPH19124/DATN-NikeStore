
let app = angular.module("demo_routing", ["ngRoute"]);
app.config(function ($routeProvider, $locationProvider){
    $locationProvider.hashPrefix(""); //Xóa khoảng thừa của URL
  //Giống switch case
  $routeProvider
    .when("/1", {
      templateUrl: "../pages/index/index.html",
    })
    .when("/2", {
      templateUrl: "../page/login/login.html",
    })
    .when("/3", {
      templateUrl: "../page/login/register.html",
    })
    .when("/4", {
      templateUrl: "../page/index/index.html",
    })
    .when("/5", {
      templateUrl: "../page/login/forgotpassword.html",
    })
    .when("/6", {
      templateUrl: "../page/product/product.html",
    })
    .when("/7", {
      templateUrl: "../page/promotionalproducts/promotionalproducts.html",
    })
    .when("/8", {
      templateUrl: "../page/blog/blog.html",
    })
    .when("/9", {
      templateUrl: "../page/contact/contact.html",
    })
    .when("/11", {
      templateUrl: "../page/productdetails/productdetail.html",
    })
    .when("/10", {
      templateUrl: "../page/cart/cart.html",
    })
    .when("/12", {
      templateUrl: "../page/index/favoriteproduct.html",
    })
    .when("/13", {
      templateUrl: "../page/cart/pay.html",
    })
    .when("/14", {
      templateUrl: "../pages/deatails/deatail1.html",
    })
    .when("/15", {
      templateUrl: "../pages/deatails/deatail2.html",
    })
    .when("/16", {
      templateUrl: "../pages/qlsp_add/qlsp_add.html",
      controller: "qlsp_add",
    })
    .when("/17", {
      templateUrl: "../pages/qlsp_del/qlsp_del.html",
      controller: "qlsp_del",
    })
    .when("/18", {
      templateUrl: "../pages/giohang/giohang.html",
    })
    .otherwise({
      templateUrl: "../pages/index/index.html",
    });
});
