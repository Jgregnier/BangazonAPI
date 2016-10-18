"use strict";

app.factory("CustomerFactory", function ($q, $http) {

    let getCustomers = () => {
        return $q((resolve, reject) => {
            $http.get("http://localhost:5000/Customers")
            .success((CustomersObj) => {
                console.log("in the factory hello", CustomersObj);
                resolve(CustomersObj);
            })
            .error((error) => {
                reject(error);
            });
        });
    };

    let postCustomer = (newCustomer) => {
        return $q((resolve, reject) => {
            $http.post("http://localhost:5000/Customers", angular.toJson(newCustomer))
          .success((CustomersObj) => {
              resolve(CustomersObj);
          })
          .error((error) => {
              reject(error);
          });
        });
    };

    let putCustomer = (editCustomer, id) => {
        return $q((resolve, reject) => {
            $http.put(`http://localhost:5000/Customers/${id}`, angular.toJson(editCustomer))
          .success((customersObj) => {
              resolve(customersObj);
          })
          .error((error) => {
              reject(error);
          });
        });
    };

    let deleteCustomer = (id) => {
        return $q((resolve, reject) => {
            $http.delete(`http://localhost:5000/Customers/${id}`)
          .success((customersObj) => {
              resolve(customersObj);
          })
          .error((error) => {
              reject(error);
          });
        });
    };

    return {
        getCustomers,
        postCustomer,
        deleteCustomer,
        putCustomer
    };

});