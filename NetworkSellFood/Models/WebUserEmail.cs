using System;

namespace NetworkSellFood
{
	public class WebUserEmail
	{
		public WebUserEmail ()
		{
		}

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public WebUser User{ get; set;}

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		/// <value>The email address.</value>
		public string EmailAddress{ get; set;}

		/// <summary>
		/// Gets or sets the email vail code.
		/// </summary>
		/// <value>The email vail code.</value>
		public string EmailVailCode{ get; set;}

		/// <summary>
		/// Gets or sets the make time.
		/// </summary>
		/// <value>The make time.</value>
		public DateTime MakeTime{get;set;}

		/// <summary>
		/// Gets or sets the sign.
		/// </summary>
		/// <value>The sign.</value>
		public byte Sign{ get; set;}
	}
}

