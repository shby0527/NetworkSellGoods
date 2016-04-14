(function (){
	var regPhone = /^\+?[0-9]{5,}$/;
	$(document).ready(function(){
		$('[data-action=change]').click(function(){
			var aid = $(this).attr('data-info');
			var txtcommit = $('#txtcommit'+aid).val();
			var txtinfo = $('#txtinfo'+aid).val();
			var txtperson = $('#txtperson'+aid).val();
			var txtphone = $('#txtphone'+aid).val();
			if (txtcommit == "") {
				alert('标题备注不应为空');
				return;
			}
			if (txtinfo == "") {
				alert('详细信息不应为空');
				return;
			}
			if (txtperson == "") {
				alert('联系人不应为空');
				return;
			}
			if (!regPhone.test(txtphone)) {
				alert('不合法的电话号码');
				return;
			}
			var header = {
				aid:aid,
				commit:txtcommit,
				information:txtinfo,
				callperson:txtperson,
				callphone:txtphone
			};
			$.post('/ShopOpt/ChangeAddress',header,function (data) {
				if (data.IsSuccess == false) {
					alert('修改失败');
					return;
				}
				alert('修改成功');
				window.location.href="/ShopOpt/AddressInfo";
			},'json');
		});
		$('[data-action=delete]').click(function () {
			var aid = $(this).attr('data-info');
			var header = {
				aid:aid
			};
				$.post('/ShopOpt/DeleteAddress',header,function (data) {
				if (data.IsSuccess == false) {
					alert('删除失败');
					return;
				}
				alert('删除成功');
				window.location.href="/ShopOpt/AddressInfo";
			},'json');
		});
		$('#btnAdd').click(function () {
			var txtcommit = $('#txtcommit').val();
			var txtinfo = $('#txtinfo').val();
			var txtperson = $('#txtperson').val();
			var txtphone = $('#txtphone').val();
			if (txtcommit == "") {
				alert('标题备注不应为空');
				return;
			}
			if (txtinfo == "") {
				alert('详细信息不应为空');
				return;
			}
			if (txtperson == "") {
				alert('联系人不应为空');
				return;
			}
			if (!regPhone.test(txtphone)) {
				alert('不合法的电话号码');
				return;
			}
			$('#frmAddr').submit();
		});
	});
})();﻿