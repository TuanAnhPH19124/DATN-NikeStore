(function (){
    var orderFactory = function (){
        var productId = null;
            
        var selectedItems = [];


        var factory = {};
        factory.setProductId = function (Id){
            productId = Id;
        }

        factory.getProductId = function () {
            return productId;
        }

        factory.getSelectedItems = function () {
            return selectedItems;
        }

        factory.setSelectedItems = function (data){
            selectedItems = data;
        }

        return factory;
    }
    angular.module("app").factory("orderFactory",orderFactory);
}());