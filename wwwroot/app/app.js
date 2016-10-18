"use strict";

var app = angular.module("BangazonApp", ["ngRoute"]);

app.config(function ($routeProvider) {
    $routeProvider.
    when('/products', {
        templateUrl: 'partials/productsPartial.html',
        controller: 'ProductCtrl'
    }).
    when('/customers', {
        templateUrl: 'partials/customersPartial.html',
        controller: 'CustomerCtrl',
    }).
    otherwise('/products');
});