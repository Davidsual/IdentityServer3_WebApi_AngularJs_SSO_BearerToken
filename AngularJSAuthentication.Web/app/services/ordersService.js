'use strict';
app.factory('ordersService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var ordersServiceFactory = {};

    var _getOrders = function () {

        return $http.get(serviceBase + 'api/orders').then(function (results) {
            return results;
        });
    };
    var _getOrders2 = function () {

        return $http.get('http://localhost:14870/api/orders').then(function (results) {
            return results;
        });
    };
    ordersServiceFactory.getOrders = _getOrders;
    ordersServiceFactory.getOrders2 = _getOrders2;

    return ordersServiceFactory;

}]);