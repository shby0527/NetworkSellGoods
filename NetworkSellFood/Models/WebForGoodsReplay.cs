/********************************************
 * 对应数据库info_repforgood
 * *****************************************/
using System;

namespace NetworkSellFood
{
	/// <summary>
	/// For goods replay.
	/// 对货物的评论
	/// </summary>
	public class WebForGoodsReplay
	{
		public WebForGoodsReplay ()
		{
		}

		public uint RID{ get; set; }

		/// <summary>
		/// Gets or sets the content of the R.
		/// 对照数据库脚本
		/// </summary>
		/// <value>The content of the R.</value>
		public string RContent{ get; set; }

		/// <summary>
		/// Gets or sets the RS time.
		/// </summary>
		/// <value>The RS time.</value>
		public DateTime RSTime{ get; set; }

		/// <summary>
		/// Gets or sets the R status.
		/// </summary>
		/// <value>The R status.</value>
		public byte RStatus{ get; set; }

		/// <summary>
		/// Gets or sets the GI.
		/// </summary>
		/// <value>The GI.</value>
		public uint GID{ get; set; }

		/// <summary>
		/// Gets or sets the user interface.
		/// </summary>
		/// <value>The user interface.</value>
		public uint UID{ get; set; }
	}
}

