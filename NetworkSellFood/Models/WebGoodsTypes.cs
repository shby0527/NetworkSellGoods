using System;

namespace NetworkSellFood
{
	public class WebGoodsTypes
	{
		public WebGoodsTypes ()
		{
		}

		/// <summary>
		/// Gets or sets the TID
		/// 类型ID
		/// </summary>
		/// <value>The TI.</value>
		public uint TID{ get; set; }

		/// <summary>
		/// Gets or sets the name of the T.
		/// 类型名称
		/// </summary>
		/// <value>The name of the T.</value>
		public string TName{ get; set; }

		/// <summary>
		/// Gets or sets the commit.
		/// 描述
		/// </summary>
		/// <value>The commit.</value>
		public string Commit{ get; set; }

		/// <summary>
		/// Gets or sets the child types.
		/// 说有子分类
		/// </summary>
		/// <value>The child types.</value>
		public WebGoodsTypeCollection ChildTypes{ get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{ get; set; }

	}
}

