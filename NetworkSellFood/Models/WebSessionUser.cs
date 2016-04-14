using System;

namespace NetworkSellFood
{
	public class WebSessionUser
	{
		public WebSessionUser ()
		{
		}

		/// <summary>
		/// Gets or sets the user interface.
		/// </summary>
		/// <value>The user interface.</value>
		public uint UID{ get; set; }

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName{ get; set; }

		/// <summary>
		/// Gets or sets the name of the nick.
		/// </summary>
		/// <value>The name of the nick.</value>
		public string NickName{ get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="NetworkSellFood.WebSessionUser"/> login sign.
		/// </summary>
		/// <value><c>true</c> if login sign; otherwise, <c>false</c>.</value>
		public bool LoginSign{ get; set; }
	}
}

