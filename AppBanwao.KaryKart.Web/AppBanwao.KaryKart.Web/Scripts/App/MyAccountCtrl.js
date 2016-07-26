
app.controller('MyAccountController', ['$scope', '$http', '$window', function ($scope, $http, $window) {
    $scope.loaddata = function () {
        $http({
            url: 'GetUser',
            method: 'Post'
        }).success(function (data) {
            if (data!= null) {
                $scope.UserID = data.UserID;
                $scope.Username = data.EmailAddress;
                $scope.Salutaions = data.salutations;
                $scope.fName = data.FirstName;
                $scope.lName = data.LastName;
                $scope.addressline1 = data.AddressLine1;
                $scope.addressline2 = data.AddressLine2;
                $scope.landmark = data.Landmark;
                $scope.mobile = data.Mobile;
                $scope.CityID = data.CityID;
                $scope.cities = data.cities;
                $scope.StateID = data.StateID;
                $scope.states = data.states;
                $scope.countryID = data.CountryID;
                $scope.countries = data.countries;
                $scope.pincode = data.Pincode;
                
            }

            if (data.messagetype == 'invaliduser') {
                $scope.user.showmessage = true;
                $scope.errortype = 'danger';
                $scope.user.message = data.message;
                //$scope.hidePwdSection = false;

            }

        }).error(function (data) {

        })
    }
}]);

