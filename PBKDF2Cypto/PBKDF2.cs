using System;
using System.Text;
using System.Security.Cryptography;
using PluginLoader.PluginAttribute;
using PluginLoader.Loader;
using PluginLoader.Configure;

namespace AbPasswdPlugin
{
	/// <summary>
	/// Crypt.
	/// </summary>
	[DingerInfo(Address = "null",EMail = "umi@hayama.cf",Name = "umi",Phone = "null")]
	[PluginExtraInfo(Commit = "It is Default Plugin , Prority is 0",Company = "null",ReleaseTime = "2015.11",UpdateTime = "2015.11")]
	[PluginInfo("202B40094A34793BBE521B1CFB8AA651A4242B499A8704E9053549F29048716E",0,Name = "PBKDF2", Version = "1.0.0",Author = "umi")]
	public class PBKDF2:AbsPassword
	{
		/// <summary>
		/// The date.
		/// </summary>
		private PasswdBase date;

		/// <summary>
		/// Initializes a new instance of the <see cref="AbPasswdPlugin.Crypt"/> class.
		/// </summary>
		public PBKDF2 ()
			:base("",100,200)
		{
			this.date = new PasswdBase ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AbPasswdPlugin.Crypt"/> class.
		/// </summary>
		/// <param name="passwd">Passwd.</param>
		public PBKDF2 (string passwd)
			:base(passwd)
		{
			this.date = new PasswdBase ();
		}
		#region implemented abstract members of AbsPlugin
		public override bool Loading ()
		{
			//the version field is the Plugin GUID
			this.date.VersionHash = this.GetGUID ();
			ConfigureManager cfg = new ConfigureManager (this);
			int min = 100;
			int max = 200;
			if (cfg.IsConfigKeyExists ("MinCount") && cfg.IsConfigKeyExists ("MaxCount")) {
				min = Convert.ToInt32 (cfg ["MinCount"]);
				max = Convert.ToInt32 (cfg ["MaxCount"]);
			} else {
				cfg ["MinCount"] = 100.ToString ();
				cfg ["MaxCount"] = 200.ToString ();
				cfg.SaveAllConfig ();
			}
			this.setOtherArgs (min, max);
			return true;
		}

		public override bool UnLoading ()
		{
			return true;
		}

		/// <summary>
		/// Gets or sets the minimum count.
		/// </summary>
		/// <value>The minimum count.</value>
		public int MinCount{ get; set; }

		/// <summary>
		/// Gets or sets the max count.
		/// </summary>
		/// <value>The max count.</value>
		public int MaxCount{ get; set; }
		#region implemented abstract members of AbsPlugin
		/// <summary>
		/// set two args with Stram
		/// </summary>
		/// <param name="args">Arguments.</param>
		public override void setOtherArgs (params object[] args)
		{
			if (args.Length < 2)
				throw new ArgumentException ("the args count is too small");
			this.MinCount = (int)args [0];
			this.MaxCount = (int)args [1];
		}
		#endregion
		protected override PasswdBase Secret ()
		{
			if (this.date.VersionHash == "")
				throw new Exception ("the VERSION is not set");
			using (HMACSHA512 hash512 = new HMACSHA512()) {
				using (RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider()) {
					Random cntRnd = new Random ();
					CryExtInfo extra = new CryExtInfo ();
					extra.Count = cntRnd.Next (this.MinCount, this.MaxCount);
					extra.Salt = new byte[64];
					//fill the Randomize salt ,the Salt's length is 
					//512bit
					rnd.GetNonZeroBytes (extra.Salt);
					using (SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider()) {
						byte[] KeyBuffer = new byte[64];
						rnd.GetNonZeroBytes (KeyBuffer);
						KeyBuffer = sha512.ComputeHash (KeyBuffer);
						extra.Key = KeyBuffer;
						hash512.Key = KeyBuffer;
						this.date.ExtraInfo = extra;
						byte[] pwdBuffer;
						byte[] pwdDate = Encoding.UTF8.GetBytes (this.Password);
						//now we add the salt to the string
						byte[] pwdWithSalt = new byte[pwdDate.Length + extra.Salt.Length];
						Array.Copy (pwdDate, pwdWithSalt, pwdDate.Length);
						Array.Copy (extra.Salt, 0, pwdWithSalt, pwdDate.Length, extra.Salt.Length);
						pwdBuffer = hash512.ComputeHash (pwdWithSalt);
						for (int i=0; i<= extra.Count; i++) {
							pwdBuffer = hash512.ComputeHash (pwdBuffer);
						}
						this.date.Password = pwdBuffer;
					}
				}
			}
			return this.date;
		}

		protected override bool CheckSecret (PasswdBase info)
		{
			CryExtInfo infos = info.ExtraInfo as CryExtInfo;
			if (infos == null)
				return false;
			using (HMACSHA512 hash512 = new HMACSHA512(infos.Key)) {
				byte[] pwdDate = Encoding.UTF8.GetBytes (this.Password);
				byte[] pwdWithSalt = new byte[pwdDate.Length + infos.Salt.Length];
				Array.Copy (pwdDate, pwdWithSalt, pwdDate.Length);
				Array.Copy (infos.Salt, 0, pwdWithSalt, pwdDate.Length, infos.Salt.Length);
				byte[] pwdBuffer = hash512.ComputeHash (pwdWithSalt);
				for (int i =0; i<=infos.Count; i++) {
					pwdBuffer = hash512.ComputeHash (pwdBuffer);
				}
				return this.SlowCompose (info.Password, pwdBuffer);
			}
		}
		#endregion
	}

	/// <summary>
	/// Cry extra info.
	/// </summary>
	[Serializable]
	public class CryExtInfo
		:System.Runtime.Serialization.ISerializable
	{

		public CryExtInfo ()
		{
		}

		protected CryExtInfo (System.Runtime.Serialization.SerializationInfo info, 
		                      System.Runtime.Serialization.StreamingContext context)
		{
			this.Count = info.GetInt32 ("Count");
			this.Key = (byte[])info.GetValue ("Key", typeof(byte[]));
			this.Salt = (byte[])info.GetValue ("Salt", typeof(byte[]));
		}

		/// <summary>
		/// Gets or sets the count.
		/// </summary>
		/// <value>The count.</value>
		public int Count{ get; set; }

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>The key.</value>
		public byte[] Key{ get; set; }

		/// <summary>
		/// Gets or sets the salt.
		/// </summary>
		/// <value>The salt.</value>
		public byte[] Salt{ get; set; }
		#region ISerializable implementation
		public void GetObjectData (System.Runtime.Serialization.SerializationInfo info, 
		                           System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue ("Key", this.Key);
			info.AddValue ("Count", this.Count);
			info.AddValue ("Salt", this.Salt);
		}
		#endregion
	}
}

