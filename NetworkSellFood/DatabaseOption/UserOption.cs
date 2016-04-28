using System;
using System.Data;
using System.Text;
using System.Web.Configuration;
using AbPasswdPlugin;
using PluginLoader.Loader;
using DatabaseVisited.MySqlHelper;
using MySql.Data.MySqlClient;

namespace NetworkSellFood
{
	public class UserOption
	{
		private WebSessionUser m_Session = null;

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password{ get; set; }


		/// <summary>
		/// Gets the session status.
		/// this Session Status is used to save to the Session Object
		/// </summary>
		/// <value>The session status.</value>
		public WebSessionUser SessionStatus {
			get {
				return this.m_Session;
			}
		}


		public static string WebDomain {
			get {
				return WebConfigurationManager.AppSettings.Get ("WebDomain");
			}
		}

		/// <summary>
		/// Users the login.
		/// 用户登录
		/// </summary>
		/// <returns><c>true</c>, if login was usered, <c>false</c> otherwise.</returns>
		public bool UserLogin ()
		{
			IPluginArray<AbsPassword> passwdarr = PluginLoader<AbsPassword>.Load ();
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = "select `uid`,`username`,`password`,`nick` from info_user where username = ?Username";
			MySql.Data.MySqlClient.MySqlParameter par = new MySql.Data.MySqlClient.MySqlParameter ("?Username", MySql.Data.MySqlClient.MySqlDbType.VarChar);
			par.Value = this.UserName;
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, par);
			if (count == 0)
				return false;
			byte[] password = (byte[])ds.Tables [0].Rows [0] ["password"];
			if (passwdarr.PluginCount == 0)
				throw new Exception ("Cypto Plugin Count is too small");
			AbsPassword cypto = passwdarr [AbsPassword.GetCryptoVersion (password)];
			cypto.Password = this.Password;
			if (!cypto.Check (password))
				return false;
			//验证成功，创建所有的用户，及用户组信息
			// Session Saving Data create
			this.m_Session = new WebSessionUser ();
			this.m_Session.UID = (uint)ds.Tables [0].Rows [0] ["uid"];
			this.m_Session.UserName = (string)ds.Tables [0].Rows [0] ["username"];
			this.m_Session.NickName = (string)ds.Tables [0].Rows [0] ["nick"];
			this.m_Session.LoginSign = true;
			SQL = string.Format ("update info_user set lastLogin = now() where uid = {0}", this.m_Session.UID);
			database.ExecuteSQLWithoutResult (SQL);
			return true;
		}

		/// <summary>
		/// Avas the image.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="uid">Uid.</param>
		public static string AvaImage (uint uid)
		{
			string SQL = string.Format ("select `avator` from info_user where `uid`={0}", uid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count == 0)
				return null;
			string path = ds.Tables [0].Rows [0] [0] as string;
			return path;
		}

		/// <summary>
		/// Gets the user with group.
		/// </summary>
		/// <returns>The user with group.</returns>
		/// <param name="user">User.</param>
		public static WebUserWithGroup GetUserWithGroup (WebSessionUser user)
		{
			WebUser baseinfo = GetUserBase (user);
			if (baseinfo == null)
				return null;
			string SQL = string.Format (
				             "select info_userGroup.* from info_userGroup,rel_uglink where info_userGroup.gid = rel_uglink.gid and rel_uglink.uid={0}",
				             baseinfo.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count == 0)
				return null;
			WebUserWithGroup rtl = new WebUserWithGroup ();
			rtl.User = baseinfo;
			rtl.Groups = new WebGroupCollection ();
			foreach (DataRow dr in ds.Tables[0].Rows) {
				WebUserGroup grp = new WebUserGroup ();
				grp.GID = (uint)dr ["gid"];
				grp.GName = (string)dr ["gname"];
				grp.Permission = (uint)dr ["permission"];
				grp.Commit = (string)dr ["commit"];
				rtl.Groups.Add (grp);
			}
			return rtl;
		}

		/// <summary>
		/// Gets the user base.
		/// </summary>
		/// <returns>The user base.</returns>
		/// <param name="user">User.</param>
		public static WebUser GetUserBase (WebSessionUser user)
		{
			if (user == null)
				return null;
			else if (!user.LoginSign)
				return null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			WebUser rtl = new WebUser ();
			string SQL = string.Format (
				             "select `uid`,`username`,`nick`,`avator`,`regTime`,`lastLogin`,`status`,`level` from info_user where `uid` = {0}",
				             user.UID);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count == 0)
				return null;
			DataRow dr = ds.Tables [0].Rows [0];
			rtl.UID = (uint)dr ["uid"];
			rtl.UserName = (string)dr ["username"];
			rtl.NickName = (string)dr ["nick"];
			rtl.Avator = dr ["avator"] as string;
			rtl.RegisterTime = (DateTime)dr ["regTime"];
			rtl.LastLogin = (DateTime)dr ["lastLogin"];
			rtl.Status = (byte)dr ["status"];
			rtl.Level = (byte)dr ["level"];
			return rtl;
		}

		/// <summary>
		/// Determines if is baned the specified user.
		/// </summary>
		/// <returns><c>true</c> if is baned the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User.</param>
		public static bool IsBaned (WebSessionUser user)
		{
			if (user == null)
				return false;
			else if (!user.LoginSign)
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format (
				             "select `status` from info_user where `uid` = {0}",
				             user.UID);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count == 0)
				return false;
			byte status = (byte)ds.Tables [0].Rows [0] ["status"];
			return (status == 1);
		}

		/// <summary>
		/// Changes the name of the nick.
		/// </summary>
		/// <returns><c>true</c>, if nick name was changed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="changednick">Changednick.</param>
		public static bool ChangeNickName (WebSessionUser user, string changednick)
		{
			if (user == null)
				return false;
			else if (!user.LoginSign)
				return false;
			//user 段存在且为已登录状态，则认为该用户已经登录
			string SQL = "update info_user set `nick`=?nick where `uid`=?uid";
			MySql.Data.MySqlClient.MySqlParameter paramNick = new MySql.Data.MySqlClient.MySqlParameter ("?nick", MySql.Data.MySqlClient.MySqlDbType.VarChar);
			paramNick.Value = changednick;
			MySql.Data.MySqlClient.MySqlParameter paramUid = new MySql.Data.MySqlClient.MySqlParameter ("?uid", MySql.Data.MySqlClient.MySqlDbType.UInt32);
			paramUid.Value = user.UID;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, paramNick, paramUid);
			if (count != 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Changes the password.
		/// </summary>
		/// <returns><c>true</c>, if password was changed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="oldpassword">Oldpassword.</param>
		/// <param name="newpassword">Newpassword.</param>
		public static bool ChangePassword (WebSessionUser user, string oldpassword, string newpassword)
		{
			if (user == null)
				return false;
			else if (!user.LoginSign)
				return false;
			IPluginArray<AbsPassword> passarr = PluginLoader<AbsPassword>.Load ();
			string SQL = string.Format ("select `password` from info_user where `uid`={0}", user.UID);
			int count = 0;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			byte[] oldpasswddata = (byte[])ds.Tables [0].Rows [0] ["password"];
			ds.Dispose ();
			AbsPassword passplugin = passarr [AbsPassword.GetCryptoVersion (oldpasswddata)];
			//首先验证原先密码的正误
			passplugin.Password = oldpassword;
			if (!passplugin.Check (oldpasswddata))
				return false;
			//验证通过后
			passplugin.Password = newpassword;
			byte[] newpasswddata = passplugin.GetBinary;
			SQL = string.Format ("update info_user set `password`=?password where `uid`={0}", user.UID);
			MySqlParameter paramPwd = new MySqlParameter ("?password", MySqlDbType.VarBinary);
			paramPwd.Value = newpasswddata;
			count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, paramPwd);
			if (count == 0)
				return false;
			//更新成功，检查密码保护邮箱，发生送提示邮件
			SQL = string.Format ("select `address` from info_email where uid={0} and sign=1", user.UID);
			ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count != 0) {
				string EMail = (string)ds.Tables [0].Rows [0] ["address"];
				AutoSendEmail.SendEmail ("账户信息变更通知",
					string.Format (
						"尊敬的{0}:\n您已经与{1}更改了您的密码，请确认为您本人操作.\n\t\t\t网上订餐系统",
						user.NickName, DateTime.Now.ToString ()), EMail);
			}
			ds.Dispose ();
			return true;
		}

		/// <summary>
		/// Changes the avator.
		/// </summary>
		/// <returns><c>true</c>, if avator was changed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="avatorPath">Avator path.</param>
		public static bool ChangeAvator (WebSessionUser user, string avatorPath)
		{
			if (user == null)
				return false;
			else if (!user.LoginSign)
				return false;
			string SQL = null;
			int count = 0;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (avatorPath == null) {
				SQL = string.Format ("update info_user set `avator`=null where `uid`={0}", user.UID);
				count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text);
			} else {
				SQL = string.Format ("update info_user set `avator`=?avator where `uid`={0}", user.UID);
				MySqlParameter paramAva = new MySqlParameter ("?avator", MySqlDbType.VarChar);
				paramAva.Value = avatorPath;
				count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, paramAva);
			}
			if (count != 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Checks the user.
		/// 检查是否存在这个用户
		/// </summary>
		/// <returns><c>true</c>, if user was checked,用户存在 <c>false</c> otherwise.</returns>
		/// <param name="username">Username.</param>
		public static bool CheckUser (string username)
		{
			string SQL = "select count(`uid`) from info_user where `username`=?username";
			MySqlParameter paraUsr = new MySqlParameter ("?username", MySqlDbType.VarChar);
			paraUsr.Value = username;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, paraUsr);
			if (count != 0) {
				long result = (long)ds.Tables [0].Rows [0] [0];
				if (result != 0)
					return true; //用户存在
				else
					return false; //用户不存在
			}
			return true;		  //错误情况和 用户存在一样处理
		}

		/// <summary>
		/// Registers the user.
		/// 用户注册
		/// </summary>
		/// <returns><c>true</c>, if user was registered, <c>false</c> otherwise.</returns>
		/// <param name="username">Username.</param>
		/// <param name="nickname">Nickname.</param>
		/// <param name="passwd">Passwd.</param>
		public static bool RegisterUser (string username, string passwd, string nickname = "天香用户")
		{
			if (CheckUser (username))
				return false;
			if (username == null || passwd == null)
				return false;
			if (username.Length < 6 || passwd.Length < 6)
				return false;
			IPluginArray<AbsPassword> passwdArr = PluginLoader<AbsPassword>.Load ();
			AbsPassword plugin = passwdArr [0];//选择默认加密插件
			string SQL = "insert into info_user (`username`,`password`,`nick`,`regTime`,`lastLogin`) values(?username,?passwd,?nickname,now(),now())";
			MySqlParameter paramUsr = new MySqlParameter ("?username", MySqlDbType.VarChar);
			paramUsr.Value = username;
			plugin.Password = passwd;
			byte[] passwdData = plugin.GetBinary;
			MySqlParameter paramPasswd = new MySqlParameter ("?passwd", MySqlDbType.VarBinary);
			paramPasswd.Value = passwdData;
			MySqlParameter paramNick = new MySqlParameter ("?nickname", MySqlDbType.VarChar);
			paramNick.Value = nickname;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, paramUsr, paramPasswd, paramNick);
			//添加用户默认权限组 Default
			SQL = "select `gid` from info_userGroup where gname='Default'";
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			uint gid = (uint)ds.Tables [0].Rows [0] ["gid"];
			SQL = "select `uid` from info_user where `username`=?username";
			ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, paramUsr);
			uint uid = (uint)ds.Tables [0].Rows [0] ["uid"];
			SQL = string.Format ("insert into rel_uglink values({0},{1})", uid, gid);
			count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Determines if is user email exists the specified user.
		/// 判断用户密保信息是否存在
		/// 存在为 true
		/// 不存在为false
		/// </summary>
		/// <returns><c>true</c> if is user email exists the specified user; otherwise, <c>false</c>.</returns>
		/// <param name="user">User.</param>
		public static bool IsUserEmailExists (WebSessionUser user)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("select count(*) from info_email where uid={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count != 0) {
					long sets = (long)ds.Tables [0].Rows [0] [0];
					if (sets == 0)
						return false;
					else
						return true;
				}
			}
			throw new Exception ("Database Connetcion Fail");
		}

		/// <summary>
		/// Makes the vail code to email.
		/// 产生验证码
		/// </summary>
		/// <returns>The vail code to email.</returns>
		private static string MakeVailCodeToEmail ()
		{
			char[] arrSel = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray ();
			StringBuilder sb = new StringBuilder ();
			Random rnd = new Random ();
			for (int i = 0; i < 64; i++) {
				sb.Append (arrSel [rnd.Next (arrSel.Length)]);
			}
			return sb.ToString ();
		}

		public static bool ResendEmail (WebSessionUser user)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			WebUserEmail ue = GetEmail (user);
			if ((DateTime.Now - ue.MakeTime).Minutes > 30) {
				return false;
			}
			AutoSendEmail.SendEmail (string.Format ("{0}的邮箱验证码", user.NickName),
				string.Format ("您的邮箱验证码为{0}。\n请点击 http://{1}/UserOpt/VailEmail/{0}\n进行验证。", ue.EmailVailCode,
					WebDomain), ue.EmailAddress);
			return true;
		}

		/// <summary>
		/// 变更或添加邮箱认证
		/// </summary>
		/// <value><c>true</c> if change user email; otherwise, <c>false</c>.</value>
		public static bool ChangeUserEmail (WebSessionUser user, string EmailAddress)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (IsUserEmailExists (user)) {
				SQL = string.Format (
					"update info_email set `address`=?address,`vailcode`=?code,`maketime`=now(),`sign`=0 where `uid`={0}", 
					user.UID);
			} else {
				SQL = string.Format (
					"insert into info_email values({0},?address,?code,now(),0)",
					user.UID);
			}
			MySqlParameter parAddr = new MySqlParameter ("?address", MySqlDbType.VarChar);
			MySqlParameter parCode = new MySqlParameter ("?code", MySqlDbType.VarChar);
			parAddr.Value = EmailAddress;
			string code = MakeVailCodeToEmail ();
			parCode.Value = code;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parAddr, parCode);
			if (count != 0) {
				AutoSendEmail.SendEmail (string.Format ("{0}的邮箱验证码", user.NickName),
					string.Format ("您的邮箱验证码为{0}。\n请点击 http://{1}/UserOpt/VailEmail/{0}\n进行验证。", code,
						WebDomain), EmailAddress);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the email.
		/// </summary>
		/// <returns>The email.</returns>
		/// <param name="user">User.</param>
		public static WebUserEmail GetEmail (WebSessionUser user)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			string SQL = string.Format ("select * from info_email where `uid`={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return null;
				WebUserEmail email = new WebUserEmail ();
				email.User = new WebUser (){ UID = user.UID };
				email.EmailAddress = (string)ds.Tables [0].Rows [0] ["address"];
				email.EmailVailCode = (string)ds.Tables [0].Rows [0] ["vailcode"];
				email.MakeTime = (DateTime)ds.Tables [0].Rows [0] ["maketime"];
				email.Sign = (byte)ds.Tables [0].Rows [0] ["sign"];
				return email;
			}
		}

		/// <summary>
		/// Gets the vail status.
		/// 获取邮箱认证状态
		/// </summary>
		/// <returns><c>true</c>, if vail status was gotten, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		public static bool GetVailStatus (WebSessionUser user)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("select `sign` from info_email where `uid`={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return false;
				return ((byte)ds.Tables [0].Rows [0] ["sign"]) == 1;
			}
		}

		/// <summary>
		/// Emails the vail check.
		/// </summary>
		/// <returns><c>true</c>, if vail check was emailed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="InputCode">Input code.</param>
		public static bool EmailVailCheck (WebSessionUser user, string InputCode)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			WebUserEmail email = new WebUserEmail ();
			string SQL = string.Format ("select * from info_email where `uid`={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return false;
				email.EmailAddress = (string)ds.Tables [0].Rows [0] ["address"];
				email.EmailVailCode = (string)ds.Tables [0].Rows [0] ["vailcode"];
				email.MakeTime = (DateTime)ds.Tables [0].Rows [0] ["maketime"];
				email.Sign = (byte)ds.Tables [0].Rows [0] ["sign"];
			}
			if (email.Sign != 0)
				return false;
			if ((DateTime.Now - email.MakeTime).Minutes > 30)
				return false;
			if (email.EmailVailCode != InputCode)
				return false;
			//修改数据库认证状态
			SQL = string.Format ("update info_email set `sign`=1 where `uid`={0}", user.UID);
			count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Changes the real info.
		/// 修改用户密保信息（用以标识用户真实信息）
		/// </summary>
		/// <returns><c>true</c>, if real info was changed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		public static bool ChangeRealInfo (WebSessionUser user,
		                                   string Name,
		                                   byte Sex,
		                                   byte LicenseType,
		                                   string LicenseID,
		                                   string CallPhoneNumber)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("select count(`uid`) from info_realinfo where `uid`={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			bool flag = false;//false 表示没有
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count != 0) {
					if (((long)ds.Tables [0].Rows [0] [0]) != 0)
						flag = true;
					else
						flag = false;
				} else
					throw new Exception ("Database Error");
			}
			if (flag)
				SQL = string.Format (
					"update info_realinfo set `name`=?name,`sex`={0},`licensetype`={1},`licenseid`=?id,`phonenumber`=?call where `uid`={2}", 
					Sex, LicenseType, 
					user.UID);
			else
				SQL = string.Format (
					"insert into info_realinfo values({0},?name,{1},{2},?id,?call)", user.UID, Sex,
					LicenseType);
			MySqlParameter parName = new MySqlParameter ("?name", MySqlDbType.VarChar);
			MySqlParameter parID = new MySqlParameter ("?id", MySqlDbType.VarChar);
			MySqlParameter parCall = new MySqlParameter ("?call", MySqlDbType.VarChar);
			parName.Value = Name;
			parID.Value = LicenseID;
			parCall.Value = CallPhoneNumber;
			count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parName, parID, parCall);
			return true;
		}



		/// <summary>
		/// Adds the address info.
		/// 增加联系地址
		/// </summary>
		/// <returns><c>true</c>, if address info was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="commit">Commit.</param>
		/// <param name="information">Information.</param>
		/// <param name="callperson">Callperson.</param>
		/// <param name="callnumber">Callnumber.</param>
		public static bool AddAddressInfo (WebSessionUser user,
		                                   string commit, string information, 
		                                   string callperson, string callnumber)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format (
				             "insert into info_address (`commit`,`information`,`callperson`,`callnumber`,`uid`) values (?commit,?information,?callperson,?callnumber,{0})", 
				             user.UID);
			MySqlParameter parCommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
			MySqlParameter parInformation = new MySqlParameter ("?information", MySqlDbType.VarChar);
			MySqlParameter parCallPerson = new MySqlParameter ("?callperson", MySqlDbType.VarChar);
			MySqlParameter parCallNumber = new MySqlParameter ("?callnumber", MySqlDbType.VarChar);
			parCommit.Value = commit;
			parInformation.Value = information;
			parCallPerson.Value = callperson;
			parCallNumber.Value = callnumber;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, 
				            parCommit, parInformation, parCallPerson, parCallNumber);
			if (count == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Removes the address info.
		/// 移除指定用户信息
		/// </summary>
		/// <returns><c>true</c>, if address info was removed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="aid">Aid.</param>
		public static bool RemoveAddressInfo (WebSessionUser user, uint aid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("update info_address set `status`=1 where `aid`={0} and `uid`={1}", aid, user.UID);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Modifies the address info.
		/// 修改联系地址信息
		/// </summary>
		/// <returns><c>true</c>, if address info was modifyed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="aid">Aid.</param>
		/// <param name="Commit">Commit.</param>
		/// <param name="Information">Information.</param>
		/// <param name="CallPerson">Call person.</param>
		/// <param name="CallNumber">Call number.</param>
		public static bool ModifyAddressInfo (WebSessionUser user, uint aid,
		                                      string Commit, string Information,
		                                      string CallPerson, string CallNumber)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format (
				             "update info_address set `commit`=?commit,`information`=?information,`callperson`=?callperson,`callnumber`=?callnumber where `aid`={0} and `uid`={1}", 
				             aid, user.UID);
			MySqlParameter parCommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
			MySqlParameter parInformation = new MySqlParameter ("?information", MySqlDbType.VarChar);
			MySqlParameter parCallPerson = new MySqlParameter ("?callperson", MySqlDbType.VarChar);
			MySqlParameter parCallNumber = new MySqlParameter ("?callnumber", MySqlDbType.VarChar);
			parCommit.Value = Commit;
			parInformation.Value = Information;
			parCallPerson.Value = CallPerson;
			parCallNumber.Value = CallNumber;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, 
				            parCommit, parInformation, parCallPerson, parCallNumber);
			if (count == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Gets all address info.
		/// 获取用户所有的通信地址
		/// </summary>
		/// <returns>The all address info.</returns>
		/// <param name="user">User.</param>
		public static WebUserAddressCollection GetAllAddressInfo (WebSessionUser user)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("select* from info_address where `uid`={0} and `status` = 0", user.UID);

			WebUserAddressCollection rtl = new WebUserAddressCollection ();
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return null;
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebUserAddress ua = new WebUserAddress ();
					ua.AID = (uint)dr ["aid"];
					ua.Commit = (string)dr ["commit"];
					ua.Information = (string)dr ["information"];
					ua.CallPerson = (string)dr ["callperson"];
					ua.CallNumber = (string)dr ["callnumber"];
					ua.Status = 0;
					ua.UID = user.UID;
					rtl.Add (ua);
				}
			}
			return rtl;
		}

		/// <summary>
		/// Gets the real info.
		/// </summary>
		/// <returns>The real info.</returns>
		/// <param name="user">User.</param>
		public static WebUserRealInfomation GetRealInfo (WebSessionUser user)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("select* from info_realinfo where `uid`={0}", user.UID);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return null;
				WebUserRealInfomation rtl = new WebUserRealInfomation ();
				rtl.User = new WebUser (){ UID = user.UID };
				rtl.Name = (string)ds.Tables [0].Rows [0] ["name"];
				rtl.Sex = (byte)ds.Tables [0].Rows [0] ["sex"];
				rtl.LicenseType = (byte)ds.Tables [0].Rows [0] ["licensetype"];
				rtl.LicenseID = (string)ds.Tables [0].Rows [0] ["licenseid"];
				rtl.PhoneNumber = (string)ds.Tables [0].Rows [0] ["phonenumber"];
				return rtl;
			}
		}
	}
}