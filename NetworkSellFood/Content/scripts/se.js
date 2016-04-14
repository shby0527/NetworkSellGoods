(function () {
	$(document).ready(function (){
		$('#btnOK').click(function () {
			var txt = $('#txtemail').val();
			var reg = /^\w+@(\w+\.)+\w+$/;
			if (!reg.test(txt)) {
				alert('邮箱格式不正确!')
				return;
			}
			$('#frmse').submit();
		});
		$('#btnResend').click(function(){
			$.get('/UserOpt/ResendEmail');
			$(this).button('loading').delay(20000).queue(function(){
				$(this).button('reset');
			});
		});
	});
})();