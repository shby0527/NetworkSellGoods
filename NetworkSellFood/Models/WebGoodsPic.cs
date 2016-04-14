using System;

namespace NetworkSellFood
{
	public class WebGoodsPic
	{
		public WebGoodsPic ()
		{
		}

		/// <summary>
		/// Gets or sets the MII.
		/// 更多图片对应图ID
		/// </summary>
		/// <value>The MII.</value>
		public uint MIID{ get; set;}

		/// <summary>
		/// Gets or sets the MIPI.
		/// 图片地址
		/// </summary>
		/// <value>The MIPI.</value>
		public string MIPIC{get;set;}

		/// <summary>
		/// Gets or sets the GI.
		/// 对应的GID
		/// </summary>
		/// <value>The GI.</value>
		public uint GID{get;set;}
	}
}

