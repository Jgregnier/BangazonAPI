"use strict";

app.factory("ProductsFactory", function ($q, $http) {

    let getProducts = () => {
      return $q((resolve, reject) => {
        $http.get("http://localhost:5000/Products")
        .success((productsObj) => {
            console.log("in the factory hello", productsObj);
          resolve(productsObj);
        })
        .error((error) => {
          reject(error);
        });
      });
    };

    let postProduct = (newProduct) => {
      return $q((resolve, reject) => {
          $http.post("http://localhost:5000/Products",angular.toJson(newProduct))
        .success((productsObj) => {
          resolve(productsObj);
        })
        .error((error) => {
          reject(error);
        });
      });
    };

    let putProduct = (editProduct, id) => {
        return $q((resolve, reject) => {
            $http.put(`http://localhost:5000/Products/${id}` ,angular.toJson(editProduct))
          .success((productsObj) => {
              resolve(productsObj);
          })
          .error((error) => {
              reject(error);
          });
        });
    };

    let deleteProducts = (id) => {
      return $q((resolve, reject) => {
          $http.delete(`http://localhost:5000/Products/${id}`)
        .success((productsObj) => {
          resolve(productsObj);
        })
        .error((error) => {
          reject(error);
        });
      });
    };

  return {
    getProducts,
    postProduct,
    deleteProducts,
    putProduct
  };

});
