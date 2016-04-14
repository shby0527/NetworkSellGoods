/*******************************************
 * 订单中的货物
 * 对应表 info_indentgoods
 * ****************************************/

using System;

namespace NetworkSellFood
{
	public class WebIndentGoods
	{
		public WebIndentGoods ()
		{
		}

		/// <summary>
		/// Gets or sets the BI.
		/// </summary>
		/// <value>The BI.</value>
		public string BID{ get; set; }

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

