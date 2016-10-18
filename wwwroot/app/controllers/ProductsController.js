"use strict";

app.controller("ProductCtrl", function ($scope, ProductsFactory) {

  $scope.products = [];

  $scope.getProducts = function() {
    ProductsFactory.getProducts()
    .then((responseProducts) => {
    $scope.products = responseProducts;
    });
  };

  $scope.postProduct = function() {
      var newProduct = {
          "description": $scope.newProdDescription,
          "price": $scope.newProdPrice,
          "productId": $scope.newProdID
      };
      console.log(newProduct);
      ProductsFactory.postProduct(newProduct)
      .then((response) => {
          console.log(response)
          $scope.getProducts()
      });
  };

  $scope.putProduct = function () {
      var editProduct = {
          "description": $scope.editProdDescription,
          "price": $scope.editProdPrice,
          "productId": $scope.editProdID
      };

      var id = $scope.editProdID;

      console.log(editProduct);

      ProductsFactory.putProduct(editProduct, id)
      .then((response) => {
          console.log(response)
          $scope.getProducts()
      });
  };

  $scope.deleteProduct = function () {
      var deleteKey = this.product.productId;

      ProductsFactory.deleteProducts(deleteKey)
      .then((response) => {
          console.log(response)
          $scope.getProducts();
      });
  };

});
