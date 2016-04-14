using System;

namespace NetworkSellFood
{
	public class WebGoodsInfo
	{
		public WebGoodsInfo ()
		{
		}

		/// <summary>
		/// Gets or sets the GID.
		/// </summary>
		/// <value>The GI.</value>
		public uint GID{ get; set;}

		/// <summary>
		/// Gets or sets the name of the G.
		/// 商品名字
		/// </summary>
		/// <value>The name of the G.</value>
		public string GName{ get; set;}

		/// <summary>
		/// Gets or sets the G picture.
		/// 图片信息
		/// </summary>
		/// <value>The G picture.</value>
		public string GPicture{ get; set; }

		/// <summary>
		/// Gets or sets the G commit.
		/// 商品介绍
		/// </summary>
		/// <value>The G commit.</value>
		public string GCommit{ get; set; }

		/// <summary>
		/// Gets or sets the G price.
		/// 单价
		/// </summary>
		/// <value>The G price.</value>
		public uint GPrice{ get; set;}

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{get;set;}
	}
}

