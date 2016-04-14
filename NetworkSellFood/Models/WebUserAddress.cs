/*******************************************
 * 对应数据库表 info_address
 * ****************************************/
using System;

namespace NetworkSellFood
{
	public class WebUserAddress
	{
		public WebUserAddress ()
		{
		}

		/// <summary>
		/// Gets or sets the AI.
		/// </summary>
		/// <value>The AI.</value>
		public uint AID{ get; set; }

		/// <summary>
		/// Gets or sets the commit.
		/// </summary>
		/// <value>The commit.</value>
		public string Commit{ get; set; }

		/// <summary>
		/// Gets or sets the information.
		/// </summary>
		/// <value>The information.</value>
		public string Information{ get; set; }

		/// <summary>
		/// Gets or sets the call person.
		/// </summary>
		/// <value>The call person.</value>
		public string CallPerson { get; set; }

		/// <summary>
		/// Gets or sets the call number.
		/// </summary>
		/// <value>The call number.</value>
		public string CallNumber{ get; set; }

		/// <summary>
		/// Gets or sets the user interface.
		/// </summary>
		/// <value>The user interface.</value>
		public uint UID{ get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{get;set;}
	}
}

