using System;

namespace NetworkSellFood
{
	public class WebGroupWithUser
	{
		public WebGroupWithUser ()
		{
		}

		/// <summary>
		/// Gets or sets the group.
		/// </summary>
		/// <value>The group.</value>
		public WebUserGroup Group{ get; set; }

		/// <summary>
		/// Gets or sets the users.
		/// </summary>
		/// <value>The users.</value>
		public WebUserCollection Users{ get; set; }
	}
}

