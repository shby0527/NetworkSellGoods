using System;
using System.Data;
using MySql.Data.MySqlClient;
using DatabaseVisited.MySqlHelper;

namespace NetworkSellFood
{
	public static class AdminOption
	{
		/// <summary>
		/// Searchs the user with I.
		/// 使用 用户ID搜索用户
		/// </summary>
		/// <returns>The user with I.</returns>
		/// <param name="user">User.</param>
		/// <param name="Uid">Uid.</param>
		public static WebUser SearchUserByID (WebSessionUser user, uint Uid)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			if (!PermissionCheck (user, WebUserGroup.UserManage))
				return null;
			return UserOption.GetUserBase (new WebSessionUser () {UID = Uid,
				LoginSign = true
			});
		}

		/// <summary>
		/// Permissions the check.
		/// 权限检查
		/// </summary>
		/// <returns><c>true</c>, if check was permissioned, 有所需权限<c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="NeedPermission">Need permission.所需权限</param>
		public static bool PermissionCheck (WebSessionUser user, uint NeedPermission)
		{
			WebUserWithGroup ugroup = UserOption.GetUserWithGroup (user);
			//权限检查
			//需要使用的权限 WebUserGroup.UserManage
			foreach (WebUserGroup grp in ugroup.Groups) {
				if ((grp.Permission & NeedPermission) == NeedPermission)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Searchs the name of the user by user.
		/// 根据用户名搜索用户
		/// </summary>
		/// <returns>The user by user name.</returns>
		/// <param name="user">User.</param>
		/// <param name="Username">Username.</param>
		public static WebUser SearchUserByUserName (WebSessionUser user, string Username)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			if (!PermissionCheck (user, WebUserGroup.UserManage))
				return null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = "select `uid`,`username`,`nick`,`avator`,`regTime`,`lastLogin`,`status`,`level` from info_user where `username`=?username";
			MySqlParameter parUname = new MySqlParameter ("?username", MySqlDbType.VarChar);
			parUname.Value = Username;
			WebUser rtl = new WebUser ();
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, parUname);
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
		/// Searchs the user by nick.
		/// All Can search
		/// </summary>
		/// <returns>The user by nick.</returns>
		/// <param name="NickName">Nick name.</param>
		public static WebUserCollection SearchUserByNick (string NickName, uint Page = 1, uint PerPage = 10)
		{
			string SQL = string.Format (
				             "select `uid`,`username`,`nick`,`avator`,`regTime`,`lastLogin`,`status`,`level` from info_user where `nick`=?nick limit {0},{1}",
				             (Page - 1) * PerPage + 1, Page * PerPage);
			MySqlParameter parNick = new MySqlParameter ("?nick", MySqlDbType.VarChar);
			parNick.Value = NickName;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			WebUserCollection rtc = new WebUserCollection ();
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, parNick)) {
				if (count == 0)
					return null;

				rtc.Page = Page;
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebUser rtl = new WebUser ();
					rtl.UID = (uint)dr ["uid"];
					rtl.UserName = (string)dr ["username"];
					rtl.NickName = (string)dr ["nick"];
					rtl.Avator = dr ["avator"] as string;
					rtl.RegisterTime = (DateTime)dr ["regTime"];
					rtl.LastLogin = (DateTime)dr ["lastLogin"];
					rtl.Status = (byte)dr ["status"];
					rtl.Level = (byte)dr ["level"];
					rtc.Add (rtl);
				}
			}
			SQL = "select count(*) from info_user where `nick`=?nick";
			parNick = new MySqlParameter ("?nick", MySqlDbType.VarChar);
			parNick.Value = NickName;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, parNick)) {
				long counts = (long)ds.Tables [0].Rows [0] [0];
				rtc.PageCount = Convert.ToUInt32 (counts % PerPage == 0 ? counts / PerPage : (counts / PerPage) + 1);
			}
			return rtc;
		}

		/// <summary>
		/// Users the status change.
		/// 更改用户状态
		/// </summary>
		/// <returns><c>true</c>, if status change was usered, <c>false</c> otherwise.</returns>
		/// <param name="user">User. （有权限的用户）</param>
		/// <param name="UID">User interface.(要更改的用户ID)</param>
		/// <param name="Status">Status.(目标状态)</param>
		public static bool UserStatusChange (WebSessionUser user, uint UID, uint Status)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.UserManage))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("update info_user set `status`={1} where `uid`={0}", UID, Status);
			return database.ExecuteSQLWithoutResult (SQL) != 0;
		}

		/// <summary>
		/// Adds the group.
		/// 所需权限
		/// WebUserGroup.PermissionInvoke
		/// </summary>
		/// <returns><c>true</c>, if group was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="GroupName">Group name.</param>
		/// <param name="GroupCommit">Group commit.</param>
		/// <param name="Permission">Permission.</param>
		public static bool AddGroup (WebSessionUser user,
		                             string GroupName,
		                             string GroupCommit,
		                             uint Permission)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.PermissionInvoke))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("insert into info_userGroup (`gname`,`permission`,`commit`) values (?gname,{0},?commit)", Permission);
			MySqlParameter parName = new MySqlParameter ("?gname", MySqlDbType.VarChar);
			MySqlParameter parCommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
			parName.Value = GroupName;
			parCommit.Value = GroupCommit;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parName, parCommit);
			if (count != 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Dels the group.
		/// 删除用户组
		/// 所需权限
		/// WebUserGroup.PermissionDeny
		/// 删除用户组将导致所有该组用户移动到默认组
		/// 若该用户不止加入该组则直接删除，
		/// 若用户没有其他组，则移动到默认组
		/// </summary>
		/// <returns><c>true</c>, if group was deled, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="gid">Gid.</param>
		public static bool DelGroup (WebSessionUser user,
		                             uint gid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.PermissionDeny))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (database.ExecuteWithTrans (cmd => {
				string SQL =
					string.Format (
						"select `uid`,count(`gid`) as `JoinGroup` from rel_uglink where `uid` in (select `uid` from rel_uglink where `gid`={0}) group by `uid` having `JoinGroup` =1",
						gid);
				cmd.CommandText = SQL;
				MySqlDataReader dr = cmd.ExecuteReader ();
				while (dr.Read ()) {
					uint uid = dr.GetUInt32 ("uid");
					SQL = string.Format ("update rel_uglink set `gid`=2 where `uid`={0}", uid);
					cmd.CommandText = SQL;
					cmd.ExecuteNonQuery ();
				} 
				dr.Close ();
				SQL = string.Format ("delete info_userGroup where `gid`={0}", gid);
				cmd.CommandText = SQL;
				cmd.ExecuteNonQuery ();
				return true;
			}))
				return true;
			else
				return false;
		}

		/// <summary>
		/// Modiflies the group.
		/// 修改组权限
		/// 所需权限
		///	WebUserGroup.PermissionInvoke;
		/// WebUserGroup.PermissionDeny
		/// </summary>
		/// <returns><c>true</c>, if group was modiflyed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="gid">Gid.</param>
		/// <param name="permission">Permission.</param>
		public static bool ModiflyGroup (WebSessionUser user, uint gid, uint permission)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.PermissionInvoke | WebUserGroup.PermissionDeny))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("update info_userGroup set `permission`={0} where `gid`={1}", permission, gid);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;			
		}

		/// <summary>
		/// Alls the group.
		/// 所有列表
		/// 需要权限
		///	WebUserGroup.PermissionInvoke;
		/// 或
		/// WebUserGroup.PermissionDeny
		/// </summary>
		/// <returns>The group.</returns>
		public static WebGroupCollection AllGroup (WebSessionUser user)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			if (!(PermissionCheck (user, WebUserGroup.PermissionInvoke) || PermissionCheck (user, WebUserGroup.PermissionDeny)))
				return null;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = "select * from info_userGroup";
			int count;
			WebGroupCollection rtl = new WebGroupCollection ();
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebUserGroup ug = new WebUserGroup ();
					ug.GID = (uint)dr ["gid"];
					ug.GName = (string)dr ["gname"];
					ug.Permission = (uint)dr ["permission"];
					ug.Commit = (string)dr ["commit"];
					rtl.Add (ug);
				}
			}
			return rtl;
		}

		/// <summary>
		/// Adds the type.
		/// 添加分类
		/// 所需权限
		/// WebUserGroup.AddType
		/// </summary>
		/// <returns><c>true</c>, if type was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="TName">T name.</param>
		/// <param name="Commit">Commit.</param>
		/// <param name="ParerntId">Parernt identifier.</param>
		public static bool AddType (WebSessionUser user, string TName, string Commit, uint ParerntId = 0)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.AddType))
				return false;
			string SQL = null;
			if (ParerntId == 0) {
				SQL = "insert into info_type (`tname`,`commit`) values(?name,?commit)";
			} else {
				SQL = string.Format ("insert into info_type (`tname`,`commit`,`ptid`) values (?name,?commit,{0})", ParerntId);
			}
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			MySqlParameter parName = new MySqlParameter ("?name", MySqlDbType.VarChar);
			MySqlParameter parCommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
			parName.Value = TName;
			parCommit.Value = Commit;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parName, parCommit);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Deletes the type.
		/// 删除分类
		/// 所需权限
		/// WebUserGroup.DeleteType
		/// </summary>
		/// <returns><c>true</c>, if type was deleted, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="tid">Tid.</param>
		public static bool DeleteType (WebSessionUser user, uint tid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.DeleteType))
				return false;
			string SQL = string.Format ("update info_type set `status`=1 where `tid`={0}", tid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Modifies the type.
		/// 修改类别信息（注释）
		/// </summary>
		/// <returns><c>true</c>, if type was modifyed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="tid">Tid.</param>
		/// <param name="commit">Commit.</param>
		public static bool ModifyType (WebSessionUser user, uint tid,
		                               string commit)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.AddType | WebUserGroup.DeleteType))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("update info_type set `commit`=?commit where `tid`={0}", tid);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Adds the menu.
		/// 所需权限
		/// WebUserGroup.AddMenu
		/// </summary>
		/// <returns><c>true</c>, if menu was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="GName">G name.</param>
		/// <param name="GPicture">G picture.</param>
		/// <param name="GCommit">G commit.</param>
		/// <param name="GPrice">G price.</param>
		public static bool AddMenu (WebSessionUser user,
		                            string GName, string GPicture, 
		                            string GCommit, uint GPrice)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.AddMenu))
				return false;
			string SQL = string.Format (
				             "insert into info_foods (`gname`,`gpic`,`gcommit`,`gprice`) values (?name,?gpic,?gcommit,{0})",
				             GPrice);
			MySqlParameter parName = new MySqlParameter ("?name", MySqlDbType.VarChar);
			parName.Value = GName;
			MySqlParameter parGPic = new MySqlParameter ("?gpic", MySqlDbType.VarChar);
			parGPic.Value = GPicture;
			MySqlParameter parCommit = new MySqlParameter ("?gcommit", MySqlDbType.VarChar);
			parCommit.Value = GCommit;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parName, parGPic, parCommit);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Updates the menu data.
		/// 更新商品上架状态
		/// 所需权限
		/// WebUserGroup.DeleteMenu
		/// WebUserGroup.AddMenu
		/// </summary>
		/// <returns><c>true</c>, if menu data was updated, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="gid">Gid.</param>
		/// <param name="status">Status.</param>
		public static bool UpdateMenuStatus (WebSessionUser user, uint gid, byte status)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.DeleteMenu | WebUserGroup.AddMenu))
				return false;
			string SQL = string.Format ("update info_foods set `status`={1} where `gid`={0}", gid, status);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Modifies the menu data.
		/// 修改商品信息
		/// 所需权限
		/// WebUserGroup.AddMenu
		/// WebUserGroup.DeleteMenu
		/// </summary>
		/// <returns><c>true</c>, if menu data was modifyed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="Name">Name.</param>
		/// <param name="Picture">Picture.</param>
		/// <param name="Commit">Commit.</param>
		/// <param name="Price">Price.</param>
		public static bool ModifyMenuData (WebSessionUser user, uint gid, string Name,
		                                   string Picture, string Commit, uint Price)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.AddMenu | WebUserGroup.DeleteMenu))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("update info_foods set `gname`=?name,`gpic`=?pic,`gcommit`=?commit,`gprice`={1} where `gid`={0}", gid, Price);
			MySqlParameter parName = new MySqlParameter ("?name", MySqlDbType.VarChar);
			MySqlParameter parPic = new MySqlParameter ("?pic", MySqlDbType.VarChar);
			MySqlParameter parCommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
			parName.Value = Name;
			parPic.Value = Picture;
			parCommit.Value = Commit;
			int count = database.ExecuteSQLWithoutResult (SQL, CommandType.Text, parName, parPic, parCommit);
			if (count != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Adds the user to group.
		/// 添加用户到用户组
		/// 所需权限
		/// WebUserGroup.PermissionInvoke
		/// </summary>
		/// <returns><c>true</c>, if user to group was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="uid">Uid.</param>
		/// <param name="gid">Gid.</param>
		public static bool AddUserToGroup (WebSessionUser user, uint uid, uint gid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.PermissionInvoke))
				return false;
			string SQL = string.Format ("insert into rel_uglink values({0},{1})", uid, gid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = database.ExecuteSQLWithoutResult (SQL);
			if (count == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Removes the user from group.
		/// 从用户组中移除用户
		/// 所需权限
		/// WebUserGroup.PermissionDeny
		/// </summary>
		/// <returns><c>true</c>, if user from group was removed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="uid">Uid.</param>
		/// <param name="gid">Gid.</param>
		public static bool RemoveUserFromGroup (WebSessionUser user, uint uid, uint gid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.PermissionDeny))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("select count(*) from rel_uglink where `uid`={0}", uid);//是否可以安全的移除用户
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return false;
				long c = (long)ds.Tables [0].Rows [0] [0];
				if (c <= 1)
					return false;
				SQL = string.Format ("delete from rel_uglink where `uid`={0} and `gid`={1}", uid, gid);
				count = database.ExecuteSQLWithoutResult (SQL);
				if (count == 0)
					return false;
				return true;
			}
		}

		/// <summary>
		/// Changes the indent status.
		/// 修改订单状态
		/// 所需权限
		/// WebUserGroup.ChangeIndentStatus
		/// </summary>
		/// <returns><c>true</c>, if indent status was changed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="status">Status.</param>
		/// <param name="bid">Bid.</param>
		public static bool ChangeIndentStatus (WebSessionUser user, byte status, string bid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!PermissionCheck (user, WebUserGroup.ChangeIndentStatus))
				return false;
			string SQL = string.Format ("update info_indent set `status`={1} where `bid`='{0}'", bid, status);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (database.ExecuteSQLWithoutResult (SQL) != 0)
				return true;
			return false;
		}

	}
}

