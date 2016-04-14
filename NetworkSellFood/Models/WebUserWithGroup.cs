using System;

namespace NetworkSellFood
{
	public class WebUserWithGroup
	{
		public WebUserWithGroup ()
		{
		}

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public WebUser User{ get; set; }

		/// <summary>
		/// Gets or sets the groups.
		/// </summary>
		/// <value>The groups.</value>
		public WebGroupCollection Groups{ get; set; }
	}
}

