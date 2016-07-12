var app=angular.module("karrykartApp", []);
app.controller("RegisterController", ['$scope', '$http', function ($scope, $http) {

    $scope.register = function () {
        var user = {
            UserIdentifier: $scope.user.UserIdentifier,
            UserPwd: $scope.user.pwd
        };

        $http({
            url: 'Account/Register',
            method: 'Post',
            data:user
        }).success(function (data) {

        }).error(function(data){
        
        })
    }


}])
