
var app = angular.module("app", ["ngRoute", "angular-jwt"]);
app.constant("apiUrl", 'https://localhost:5001');
app.directive("headerPage", function () {
  return {
    restrict: 'E',
    templateUrl: '../directives/header-page.html',
    controller: "headerController"
    
}
})
app.directive("productPrimary", function(){
  return{
    restrict: 'E',
    templateUrl: '../directives/product-primary.html'
  }
})
app.config(function ($routeProvider, $locationProvider){
    $locationProvider.hashPrefix(""); 
  $routeProvider
    .when("/index", {
      templateUrl: "../page/index/index.html",
      controller: "homeController"
    })
    .when("/signin", {
      controller: "authController",
      templateUrl: "../page/login/login.html"
    })
    .when("/signup", {
      controller: "authController",
      templateUrl: "../page/login/register.html",
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
