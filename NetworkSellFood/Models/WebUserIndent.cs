/********************************************
 * 用户订单，对应表 info_indent
 * ******************************************/

using System;

namespace NetworkSellFood
{
	public class WebUserIndent
	{
		public WebUserIndent ()
		{
		}

		public string BID{ get; set; }

		/// <summary>
		/// Gets or sets the create time.
		/// </summary>
		/// <value>The create time.</value>
		public DateTime CreateTime{ get; set; }

		/// <summary>
		/// Gets or sets the commit.
		/// </summary>
		/// <value>The commit.</value>
		public string Commit{ get; set; }

		/// <summary>
		/// Gets or sets the piace.
		/// </summary>
		/// <value>The piace.</value>
		public uint Piace{ get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{ get; set; }

		/// <summary>
		/// Gets or sets the star.
		/// </summary>
		/// <value>The star.</value>
		public byte Star{ get; set; }

		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public WebUserAddress Address{ get; set; }

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public WebUser User{ get; set; }

		/// <summary>
		/// Gets or sets the indent goods.
		/// 订单中的所有货物
		/// </summary>
		/// <value>The indent goods.</value>
		public WebIndentGoodsCollection IndentGoods{ get; set; }
	}
}

