/*******************************************
 * 用户购物车 对应表 info_cart
 * **************************/

using System;

namespace NetworkSellFood
{
	public class WebUserCart
	{
		public WebUserCart ()
		{
		}

		/// <summary>
		/// Gets or sets the CSIG.
		/// </summary>
		/// <value>The CSIG.</value>
		public uint CSIGN { get; set; }

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public uint UID{ get; set; }

		/// <summary>
		/// Gets or sets the goods.
		/// </summary>
		/// <value>The goods.</value>
		public WebGoodsInfo Goods{ get; set; }

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public uint Count{ get; set; }
	}
}

