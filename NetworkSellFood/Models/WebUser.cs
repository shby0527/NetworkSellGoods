using System;

namespace NetworkSellFood
{
	public class WebUser
	{
		public WebUser ()
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
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public byte[] Password{ get; set; }

		/// <summary>
		/// Gets or sets the name of the nick.
		/// </summary>
		/// <value>The name of the nick.</value>
		public string NickName{ get; set; }

		/// <summary>
		/// Gets or sets the avator.
		/// </summary>
		/// <value>The avator.</value>
		public string Avator{ get; set; }

		/// <summary>
		/// Gets or sets the register time.
		/// </summary>
		/// <value>The register time.</value>
		public DateTime RegisterTime{ get; set; }

		/// <summary>
		/// Gets or sets the last login.
		/// </summary>
		/// <value>The last login.</value>
		public DateTime LastLogin{ get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public byte Status{ get; set; }

		/// <summary>
		/// Gets or sets the level.
		/// </summary>
		/// <value>The level.</value>
		public byte Level{ get; set; }

	}
}

