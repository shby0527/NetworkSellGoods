(function () {
	$(document).ready(function () {
		$('#btnPwdReset').click(function (){
			if (!confirm('确认要重置该用户的密码吗？')) {
				return;
			}
		var uid = $(this).attr('data-uid');
		$.post('/UserManage/ResetPasswd',{'id':uid},function (data) {
			if (data.IsSuccess == false) {
				alert(data.Reason);
				window.location.href = data.Redict;
			}	else {
				alert(data.Reason);
				window.location.href = data.Redict;			
			}	
		},'json');
	} );
	});	
})();