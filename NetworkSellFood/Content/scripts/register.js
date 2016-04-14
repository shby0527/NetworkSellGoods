var changeVcode = function () {
		$('#imgvcode').attr('src','/Home/VailCode?'+Math.random());
	}; 
(function () {
	$(document).ready(function (){
		$('#imgvcode').click(changeVcode);
		$('#btnRegister').click(function () {
					var UserInfo = {
					    username:$('#txtuser').val(),
					    passwd:$('#txtpwd').val(),
					    passwda:$('#txtpwdagn').val(),
					    vcode:$('#txtvail').val()
					};
					var regpwd = /^[0-9]+$/;
					var regpwd1 = /^[a-zA-Z]+$/;
					if (UserInfo.username.length<6) {
						alert('用户名不能小于6位');
						return;
					}
					if (regpwd.test(UserInfo.passwd)||regpwd1.test(UserInfo.passwd)||UserInfo.passwd.length<6) {
						alert('密码必须大于6位且必须包含数字和字母');
						return;
					}
					if (UserInfo.passwd != UserInfo.passwda) {
						alert('密码和确认密码不同，请核对');
						return;
					}
					$.post('/UserOpt/CheckVCode',{'vcode':UserInfo.vcode},function(data){
						if (data.IsPassed == false) {
							alert('验证码错误！');
							return;
						}
						$.post('/UserOpt/CheckUser',{'user':UserInfo.username},function (data) {
								if (data.IsExists == true) {
									alert('用户名已经存在！');
									return;
								}	
								$.post('/UserOpt/PostRegister',UserInfo,function (data) {
									if (data.IsRegister == false) {
										alert('注册失败!');
										return;
									}
									alert('注册成功!');
									window.location.href = data.Href;
								},'json');				
						},'json');
					}	,'json');
		});
	} );
})();