using System;

namespace NetworkSellFood
{
	/// <summary>
	/// I limit data.
	/// </summary>
	public interface ILimitData
	{
		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>The page.</value>
		uint Page{ get;  }

		/// <summary>
		/// Gets or sets the page count.
		/// </summary>
		/// <value>The page count.</value>
		uint PageCount{ get; }

	}
}

