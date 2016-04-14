(function () {
	$(document).ready(function () {
		$('#btnOK').click(function () {
			var txtname = $('#txtname').val();
			if (txtname == "") {
				alert('姓名不应为空');
				return;
			}
			var txtid = $('#txtid').val();
			if (txtid == "") {
				alert('证件号码不应为空');
				return;
			}
			var regphone = /^\+?[0-9]{5,}$/;
			var txtphone = $('#txtphone').val();
			if (!regphone.test(txtphone)) {
				alert('电话号码非法');
				return;	
			}
			$('#frmrel').submit();
		});	
	});
})();﻿