
var app = angular.module("karrykartApp", ['ngSanitize']);

app.controller("RegisterController", ['$scope', '$http', function ($scope, $http) {
    $scope.errortype = 'warning';
    $scope.hidePwdSection = false;
    $scope.waitActive = false;
   
    $scope.register = function () {
        
        var user = {
            UserIdentifier: $scope.user.UserIdentifier,
            UserPwd: $scope.user.pwd
        };
        $scope.waitActive = true;
        $scope.user.showmessage = true;
        
        $http({
            url: 'Account/Register',
            method: 'Post',
            data:user
        }).success(function (data) {
            if (data.messagetype == 'emailwitherror' || data.messagetype == 'email')
            {
                $scope.errortype = data.messagetype == 'email'?'success':'info';
                $scope.user.message = data.message;
                $scope.hidePwdSection = true;
            }
            if (data.messagetype == 'userexist') {
                $scope.errortype = 'danger';
                $scope.user.message = data.message;
                $scope.hidePwdSection = false;

            }
            
        }).error(function(data){
        
        })

        $scope.waitActive = false;
    }

    $scope.verifyotp = function () {
        $scope.waitActive = true;
        var otpdetails = {
            UserIdentifier: $scope.user.UserIdentifier,
            Userotp: $scope.user.otp
        };
        $scope.errortype = 'warning';
        $scope.user.message = GetPleaseWait();
        $http({
            url: 'Account/Verifyotp',
            method: 'Post',
            data: otpdetails
        }).success(function (data) {
            if (data.messagetype = 'success') {
                $scope.errortype = 'success';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = false;
                
                $scope.user.UserIdentifier = '';
                $scope.user.otp = '';
                $scope.user.pwd = '';
                $scope.user.cnfpwd = '';
            } else if (data.messagetype = 'error') {
                $scope.errortype = 'danger';
                $scope.user.message = data.message;
                $scope.user.showmessage = true;
                $scope.hidePwdSection = false;
               
                $scope.user.UserIdentifier = '';
                $scope.user.otp = '';
                $scope.user.pwd = '';
                $scope.user.cnfpwd = '';
            }
            

        }).error(function (data) {

        })

        $scope.waitActive = false;
    }

    $scope.switchtologin = function () {
        SwitchToTab('Login');
        $scope.user.message = '';

    }
}])
