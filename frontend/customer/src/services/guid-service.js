(function(){
    var guidService = function (){
        this.generateGuid = function() {
            // Tạo một GUID ngẫu nhiên
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
            }
            
            return (
                s4() + s4() + '-' +
                s4() + '-' +
                s4() + '-' +
                s4() + '-' +
                s4() + s4() + s4()
            );
        };
    }
    guidService.$inject = [];
    angular.module("app").service("guidService", guidService);
}())