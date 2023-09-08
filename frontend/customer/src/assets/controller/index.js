var app = angular.module("app", ["ngRoute", "angular-jwt"]);
app.constant("apiUrl", 'https://localhost:44328');
app.directive("headerPage", function () {
  return {
    restrict: 'E',
    templateUrl: '../directives/header-page.html',
    controller: "headerController"

  }
})
app.directive("productPrimary", function () {
  return {
    restrict: 'E',
    templateUrl: '../directives/product-primary.html'
  }
})
app.config(function ($routeProvider, $locationProvider) {
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
    .when("/accountDetail", {
      templateUrl: "../page/login/accountinfomation.html",
      controller: "authController"
    })
    .when("/product", {
      templateUrl: "../page/product/product.html",
      controller: "productController"
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
    .when("/21", {
      templateUrl: "../page/login/addressinfomation.html"
    })
    .when("/productDetail/:id", {
      templateUrl: "../page/productdetails/productdetail.html",
      controller: "productDetailController"
    })
    .when("/cart", {
      templateUrl: "../page/cart/cart.html",
      controller: "cartsController"
    })
    .when("/wishlist", {
      templateUrl: "../page/index/favoriteproduct.html",
      controller: "wishListController"
    })
    .when("/pay", {
      templateUrl: "../page/cart/pay.html",
      controller: "orderController"
    })
    .otherwise({
      templateUrl: "../pages/index/index.html",
    });
});
