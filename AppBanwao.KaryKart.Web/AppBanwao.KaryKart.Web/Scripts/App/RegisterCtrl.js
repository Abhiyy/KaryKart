var app = angular.module("karrykartApp", []);

app.controller("RegisterController", ['$scope', '$http', function ($scope, $http) {

    $scope.hidePwdSection = false;
    $scope.waitactive = false;
    $scope.register = function () {
        $scope.waitactive = true;
        var user = {
            UserIdentifier: $scope.user.UserIdentifier,
            UserPwd: $scope.user.pwd
        };
       
        $http({
            url: 'Account/Register',
            method: 'Post',
            data:user
        }).success(function (data) {
            if (data.messagetype == 'emailwitherror' || data.messagetype == 'email')
            {
                $scope.user.errortype = data.messagetype == 'email'?'success':'info';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = true;
            }
            if (data.messagetype == 'userexist') {
                $scope.user.errortype = 'danger';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = false;

            }
            
        }).error(function(data){
        
        })

        $scope.waitactive = false;
    }

    $scope.verifyotp = function () {
        var otpdetails = {
            UserIdentifier: $scope.user.UserIdentifier,
            Userotp: $scope.user.otp
        };

        $http({
            url: 'Account/Verifyotp',
            method: 'Post',
            data: otpdetails
        }).success(function (data) {
            if (data.messagetype = 'success') {
                $scope.user.errortype = 'success';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = true;
            } else if (data.messagetype = 'error') {
                $scope.user.errortype = 'danger';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = true;
            }
            

        }).error(function (data) {

        })
    }

}])
