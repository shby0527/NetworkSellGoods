using System;

namespace NetworkSellFood
{
	public class WebUserRealInfomation
	{
		public WebUserRealInfomation ()
		{
			
		}

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>The user.</value>
		public WebUser User{ get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name{ get; set; }

		/// <summary>
		/// Gets or sets the sex.
		/// </summary>
		/// <value>The sex.</value>
		public byte Sex{ get; set; }

		/// <summary>
		/// Gets or sets the type of the license.
		/// </summary>
		/// <value>The type of the license.</value>
		public byte LicenseType{ get; set; }

		/// <summary>
		/// Gets or sets the license I.
		/// </summary>
		/// <value>The license I.</value>
		public string LicenseID{ get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>The phone number.</value>
		public string PhoneNumber{ get; set; }
	}
}

