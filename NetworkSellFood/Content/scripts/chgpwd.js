(function () {
	$(document).ready(function (){
		$('#btnsubmit').click(function () {
			var pwdInfo = {
				oldpasswd:$('#txtoldpwd').val(),
				passwd:$('#txtpwd').val(),
				passwda:$('#txtpwda').val()
			};
			if (pwdInfo.oldpasswd == "") {
				alert('旧密码不能为空');
				return;
			}
			var regpwd = /^[0-9]+$/;
			var regpwd1 = /^[a-zA-Z]+$/;
			if (regpwd.test(pwdInfo.passwd)||regpwd1.test(pwdInfo.passwd)||pwdInfo.passwd.length<6) {
				alert('密码必须大于6位且必须包含数字和字母');
				return;
			}
			if (pwdInfo.passwd != pwdInfo.passwda) {
				alert('密码和确认密码不一致，请检查');
				return;
			}
			$.post('/UserOpt/PasswdChg',pwdInfo,function (data) {
				if (data.IsPassed == false) {
					alert('密码修改失败');
					$('#txtoldpwd').val("");
					$('#txtpwd').val("");
					$('#txtpwda').val("");
					return;
				}	
				alert('密码修改成功');
				$('#txtoldpwd').val("");
				$('#txtpwd').val("");
				$('#txtpwda').val("");
			},'json');
		});
	});
})();