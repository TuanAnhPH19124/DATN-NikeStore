(function () {
    var homeController = function (e){
        e.name  = "product Controller";
        function constructor(){
            
        }   
        constructor();
    };
    homeController.$inject = ['$scope'];
    angular.module("app").controller("homeController", homeController);
}());