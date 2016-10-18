"use strict";

app.controller("CustomerCtrl", function ($scope, CustomerFactory) {

    $scope.customers = [];

    $scope.getCustomers = function () {
        CustomerFactory.getCustomers()
        .then((responseCustomers) => {
            $scope.customers = responseCustomers;
        });
    };

    $scope.postCustomer = function () {
        var newCustomer = {
            "firstName": $scope.newCustomerFirstName,
            "lastName": $scope.newCustomerLastName
        };
        console.log(newCustomer);
        CustomerFactory.postCustomer(newCustomer)
        .then((response) => {
            console.log(response)
            $scope.getCustomers()
        });
    };

    $scope.putCustomer = function () {
        var editCustomer = {
            "firstName": $scope.editCustomerFirstName,
            "lastName": $scope.editCustomerLastName,
            "customerId": $scope.editCustomerID
        };

        var id = $scope.editCustomerID;

        console.log(editCustomer);

        CustomerFactory.putCustomer(editCustomer, id)
        .then((response) => {
            console.log(response)
            $scope.getCustomers()
        });
    };

    $scope.deleteCustomer = function () {
        var deleteKey = this.customer.customerId;

        CustomerFactory.deleteCustomer(deleteKey)
        .then((response) => {
            console.log(response)
            $scope.getCustomers();
        });
    };

});