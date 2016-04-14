using System;

namespace NetworkSellFood
{
	public class WebUserGroup
	{
		/// <summary>
		/// The visit and search.
		///  使用购物车的权限
		/// </summary>
		public static readonly uint VisitAndSearch = 0x0001U;

		/// <summary>
		/// The submit and roll back.
		/// 下单以及撤回订单
		/// </summary>
		public static readonly uint SubmitAndRollBack = 0x0002U;

		/// <summary>
		/// The change indent status.
		/// 更改订单状态
		/// </summary>
		public static readonly uint ChangeIndentStatus = 0x0004U;

		/// <summary>
		/// The post word.
		/// 发表评论
		/// </summary>
		public static readonly uint PostWord = 0x0008U;

		/// <summary>
		/// The delete word.
		/// 删除评论
		/// </summary>
		public static readonly uint DeleteWord = 0x0010U;

		/// <summary>
		/// The add menu.
		/// 增加菜单，商品
		/// </summary>
		public static readonly uint AddMenu = 0x0020U;

		/// <summary>
		/// The delete menu.
		/// 删除商品
		/// </summary>
		public static readonly uint DeleteMenu = 0x0040U;

		/// <summary>
		/// The type of the add.
		/// 增加分类
		/// </summary>
		public static readonly uint AddType = 0x0080U;

		/// <summary>
		/// The type of the delete.
		/// 删除分类
		/// </summary>
		public static readonly uint DeleteType = 0x0100U;

		/// <summary>
		/// The user Manage.
		/// 用户管理
		/// </summary>
		public static readonly uint UserManage = 0x0200U;

		/// <summary>
		/// The permission invoke.
		/// 权限授予
		/// </summary>
		public static readonly uint PermissionInvoke = 0x0400U;

		/// <summary>
		/// The permission deny.
		/// 权限撤销
		/// </summary>
		public static readonly uint PermissionDeny = 0x0800U;

		public WebUserGroup ()
		{
		}

		/// <summary>
		/// Gets or sets the GID.
		/// </summary>
		/// <value>The GID.</value>
		public uint GID{ get; set; }

		/// <summary>
		/// Gets or sets the name of the Group.
		/// </summary>
		/// <value>The name of the Group.</value>
		public string GName{ get; set; }

		/// <summary>
		/// Gets or sets the permission.
		/// </summary>
		/// <value>The permission.</value>
		public uint Permission{ get; set; }

		/// <summary>
		/// Gets or sets the commit.
		/// </summary>
		/// <value>The commit.</value>
		public string Commit{ get; set; }
	}
}

