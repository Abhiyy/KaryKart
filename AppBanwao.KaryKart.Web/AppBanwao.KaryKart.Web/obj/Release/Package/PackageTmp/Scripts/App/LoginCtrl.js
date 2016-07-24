

app.controller("LoginController", ['$scope', '$http','$window', function ($scope, $http,$window) {
    $scope.emailExpression = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;
    $scope.errortype = 'warning';
    
    $scope.login = function () {
        if ($scope.loginForm.$valid) {
            var user = {
                UserID: $scope.user.UserID,
                UserPwd: $scope.user.loginPwd
            };
            $scope.user.showmessage = true;
            $scope.errortype = 'warning';
            $scope.user.message = GetPleaseWait();

            $http({
                url: 'Account/Login',
                method: 'Post',
                data: user
            }).success(function (data) {
                if (data.messagetype == 'validuser') {
                   $window.location.reload();
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
    }
}])