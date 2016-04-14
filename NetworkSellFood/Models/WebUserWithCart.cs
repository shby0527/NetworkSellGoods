/*****************************************
 * 用户信息以及其所有购物车
 * *************************************/

using System;

namespace NetworkSellFood
{
	public class WebUserWithCart
	{
		public WebUserWithCart ()
		{
		}

		public WebUser User{ get; set; }

		public WebUserCartGoodsCollection Gart{ get; set; }
	}
}

