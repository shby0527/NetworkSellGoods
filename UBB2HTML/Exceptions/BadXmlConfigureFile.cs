using System;

namespace UBB2HTML
{
	public class BadXmlConfigureFile:Exception
	{
		/// <summary>
		/// Gets or sets the name of the node.
		/// </summary>
		/// <value>The name of the node.</value>
		public string NodeName{ get; private set; }

		/// <summary>
		/// Gets the fail reason.
		/// </summary>
		/// <value>The fail reason.</value>
		public string FailReason{ get; private set;}

		/// <summary>
		/// Initializes a new instance of the <see cref="UBB2HTML.BadXmlConfigureFile"/> class.
		/// </summary>
		public BadXmlConfigureFile ()
			:base("Bad File to the root")
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="UBB2HTML.BadXmlConfigureFile"/> class.
		/// </summary>
		/// <param name="msg">Message.</param>
		/// <param name="NodeName">Node name.</param>
		public BadXmlConfigureFile(string msg,string NodeName,string reason = "Bad Node")
			:base(msg)
		{
			this.NodeName = NodeName;
			this.FailReason = reason;
		}

	}
}

