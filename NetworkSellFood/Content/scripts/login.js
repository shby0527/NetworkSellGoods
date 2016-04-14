(function(){
	var changeVcode = function () {
		$('#imgvcode').attr('src','/Home/VailCode?'+Math.random());
	};
	$(document).ready(function () {
		$('#imgvcode').click(changeVcode);
		$('#btnLogin').click(function () {
				var username = $('#txtuser').val();
				var passwd = $('#txtpwd').val();
				var vcode = $('#txtvail').val();
				var isrem = $('#cbReMe').is(':checked');
				if (username=='') {
					alert('用户名不应为空');
					return;
				}
				if (passwd=='') {
					alert('密码不应为空');
					return;
				}
				if (vcode=='') {
					alert('验证码不应为空');
					return;
				}
				var posted={
					username:username,
					passwd:passwd,
					vcode:vcode				
				};
				if(isrem){
					posted.rememberme = true;
				}
				$.post('/UserOpt/CheckVCode',{'vcode':vcode},function (data) {
						if (data.IsPassed) {
											$.post('/UserOpt/UserLogin',posted,function (data) {
												if (data.IsPassed) {
													window.location.href=data.Href;
													return;
												}	
												alert('用户名或密码错误，登录失败');
											},'json');
											return;
						}	
						alert('验证码错误');
				},'json');
			});
		});
})();