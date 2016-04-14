/************************************************
 * 对应表 info_indentrep
 * *********************************************/

using System;

namespace NetworkSellFood
{
	public class WebIndentReplay
	{
		public WebIndentReplay ()
		{
		}

		/// <summary>
		/// Gets or sets the RI.
		/// </summary>
		/// <value>The RI.</value>
		public uint RID{ get; set; }

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public string Content{ get; set; }

		/// <summary>
		/// Gets or sets the R time.
		/// </summary>
		/// <value>The R time.</value>
		public DateTime RTime{ get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{ get; set; }

		/// <summary>
		/// Gets or sets the user interface.
		/// </summary>
		/// <value>The user interface.</value>
		public WebUser User{ get; set; }
	}
}

