using System;
using System.Runtime.Serialization;

namespace AbPasswdPlugin
{
	[Serializable]
	public class PasswdBase : ISerializable
	{

		public PasswdBase ()
		{
		}

		protected PasswdBase (SerializationInfo info, StreamingContext context)
		{
			this.VersionHash = info.GetString ("Version");
			this.Password = (byte[])info.GetValue ("Password", typeof(byte[]));
			Type type = (Type)info.GetValue ("ExtraType", typeof(Type));
			this.ExtraInfo = info.GetValue ("Extra", type);
		}

		/// <summary>
		/// Gets or sets the version hash.
		/// what's Secrey you used
		/// </summary>
		/// <value>The version hash.</value>
		public string VersionHash { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public byte[] Password{ get; set; }

		/// <summary>
		/// Gets or sets the extra info.
		/// </summary>
		/// <value>The extra info.</value>
		public object ExtraInfo{ get; set; }
		#region ISerializable implementation
		/// <Docs>To be added: an object of type 'SerializationInfo'</Docs>
		/// <summary>
		/// To be added
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			info.AddValue ("Version", this.VersionHash);
			info.AddValue ("Password", this.Password);
			info.AddValue ("Extra", this.ExtraInfo);
			info.AddValue ("ExtraType", this.ExtraInfo.GetType ());
		}
		#endregion
	}
}
