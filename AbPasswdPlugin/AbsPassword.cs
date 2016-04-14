using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PluginLoader.Plugins;
using PluginLoader.Loader;

namespace AbPasswdPlugin
{
	/// <summary>
	/// Abs plugin for Passwd Secret.
	/// </summary>
	public abstract class AbsPassword : IPlugin
	{
		protected AbsPassword (string password, params object[] oargs)
		{
			this.Password = password;
			this.setOtherArgs (oargs);
		}

		public abstract void setOtherArgs (params object[] args);

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password{ get; set; }
		#region IPlugin implementation
		/// <summary>
		/// Loading this instance.
		/// </summary>
		public abstract bool Loading ();

		/// <summary>
		/// Uns the loading.
		/// </summary>
		/// <returns><c>true</c>, if loading was uned, <c>false</c> otherwise.</returns>
		public abstract bool UnLoading ();
		#endregion
		/// <summary>
		/// Gets the serect.
		/// </summary>
		/// <returns>The serect.</returns>
		protected abstract PasswdBase Secret ();

		/// <summary>
		/// Gets the get binary.
		/// </summary>
		/// <value>The get binary.</value>
		public byte[] GetBinary {
			get {
				BinaryFormatter bf = new BinaryFormatter ();
				using (MemoryStream ms = new MemoryStream ()) {
					bf.Serialize (ms, this.Secret ());
					byte[] ret = ms.ToArray ();
					ms.Close ();
					return ret;
				}
			}
		}

		/// <summary>
		/// Checks the secret.
		/// </summary>
		/// <returns><c>true</c>, if secret was checked, <c>false</c> otherwise.</returns>
		/// <param name="info">Info.</param>
		protected abstract bool CheckSecret (PasswdBase info);

		/// <summary>
		/// Slows the compose.
		/// </summary>
		/// <returns><c>true</c>, if compose was slowed, <c>false</c> otherwise.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		protected bool SlowCompose (byte[] a, byte[] b)
		{
			int tmp = a.Length ^ b.Length;
			for (int i=0; i<a.Length && i<b.Length; i++) {
				tmp |= a [i] ^ b [i];
			}
			return(tmp == 0);
		}

		/// <summary>
		/// Gets the crypto version.
		/// </summary>
		/// <returns>The crypto version.</returns>
		/// <param name="Date">Date.</param>
		public static string GetCryptoVersion (byte[] Date)
		{
			using (MemoryStream ms = new MemoryStream()) {
				BinaryFormatter bf = new BinaryFormatter ();
				ms.Write (Date, 0, Date.Length);
				ms.Position = 0;
				PasswdBase tmp = bf.Deserialize (ms) as PasswdBase;
				if (tmp == null)
					return null;
				return tmp.VersionHash;
			}
		}

		/// <summary>
		/// Check the specified Date.
		/// the date is from disk or datebase
		/// </summary>
		/// <param name="Date">Date.</param>
		public bool Check (byte[] Date)
		{
			using (MemoryStream ms = new MemoryStream()) {
				BinaryFormatter bf = new BinaryFormatter ();
				ms.Write (Date, 0, Date.Length);
				ms.Position = 0;
				PasswdBase tmp = bf.Deserialize (ms) as PasswdBase;
				if (tmp == null)
					return false;
				//show this password is not in this check
				if (tmp.VersionHash != this.GetGUID ())
					return false;
				return this.CheckSecret (tmp);
			}
		}
	}
}

