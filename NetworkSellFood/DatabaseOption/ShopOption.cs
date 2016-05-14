using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using DatabaseVisited.MySqlHelper;

namespace NetworkSellFood
{
	public static class ShopOption
	{
		/// <summary>
		/// Gets all types.
		/// 获取分类信息
		/// </summary>
		/// <returns>The all types.</returns>
		/// <param name="Person">Person.</param>
		public static WebGoodsTypeCollection GetAllTypes (uint? Person)
		{
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			WebGoodsTypeCollection rtl = new WebGoodsTypeCollection ();
			string SQL = null;
			if (!Person.HasValue)
				SQL = string.Format ("select* from info_type where `status` = {0} and `ptid` is null", 0);
			else
				SQL = string.Format ("select* from info_type where `status` = {0} and  `ptid`={1}", 0, Person.Value);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return rtl;
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebGoodsTypes type = new WebGoodsTypes ();
					type.TID = (uint)dr ["tid"];
					type.TName = (string)dr ["tname"];
					type.Commit = (string)dr ["commit"];
					type.ChildTypes = null;
					type.Status = (byte)dr ["status"];
					rtl.Add (type);
				}
			}
			return rtl;
		}

		/// <summary>
		/// Gets all goods.
		/// 获取商品信息
		/// </summary>
		/// <param name="tid">tid</param>
		/// <returns>The all goods.</returns>
		/// <param name="Page">Page.</param>
		/// <param name="PageCount">Page count.</param>
		public static WebGoodsCollection GetAllGoods (uint tid, uint Page = 1, uint PerPage = 10)
		{
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			WebGoodsCollection rtl = new WebGoodsCollection ();
			string SQL = string.Format (
				             "select info_foods.`gid`,`gname`,`gpic`,`gcommit`,`gprice`,`status` from info_foods,rel_tflink where info_foods.`gid`=rel_tflink.`gid` and rel_tflink.`tid`={0} limit {1},{2}",
				             tid, (Page - 1) * PerPage + 1, Page * PerPage);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				if (count == 0)
					return rtl;
				rtl.Page = Page;
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebGoodsInfo goods = new WebGoodsInfo ();
					goods.GID = (uint)dr ["gid"];
					goods.GName = (string)dr ["gname"];
					goods.GPicture = (string)dr ["gpic"];
					goods.GCommit = (string)dr ["gcommit"];
					goods.GPrice = (uint)dr ["gprice"];
					goods.Status = (byte)dr ["status"];
					rtl.Add (goods);
				}
			}
			SQL = string.Format (
				"select count(*) from info_foods,rel_tflink where info_foods.`gid`=rel_tflink.`gid` and rel_tflink.`tid`={0}",
				tid);
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				long counts = (long)ds.Tables [0].Rows [0] [0];
				rtl.PageCount = Convert.ToUInt32 (counts % PerPage == 0 ? counts / PerPage : (counts / PerPage) + 1);
			}
			return rtl;
		}

		/// <summary>
		/// Searchs the goods.
		/// </summary>
		/// <returns>The goods.</returns>
		/// <param name="condint">Condint.搜索条件</param>
		/// <param name="Page">Page.</param>
		/// <param name="PerPage">Per page.</param>
		public static WebGoodsCollection SearchGoods (string condint, uint Page, uint PerPage)
		{
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			WebGoodsCollection rtl = new WebGoodsCollection ();
			string SQL = string.Format ("select * from info_foods where gname like ?condint limit {0},{1}",
				             (Page - 1) * PerPage + 1, Page * PerPage);
			MySqlParameter par = new MySqlParameter ("?condint", MySqlDbType.VarChar);
			par.Value = string.Format ("%{0}%", condint);
			int count = 0;
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count, CommandType.Text, par)) {
				if (count == 0)
					return rtl;
				rtl.Page = Page;
				foreach (DataRow dr in ds.Tables[0].Rows) {
					WebGoodsInfo goods = new WebGoodsInfo ();
					goods.GID = (uint)dr ["gid"];
					goods.GName = (string)dr ["gname"];
					goods.GPicture = (string)dr ["gpic"];
					goods.GCommit = (string)dr ["gcommit"];
					goods.GPrice = (uint)dr ["gprice"];
					goods.Status = (byte)dr ["status"];
					rtl.Add (goods);
				}
			}
			SQL = "select count(*) from info_foods where gname like ?condint";
			par = new MySqlParameter ("?condint", MySqlDbType.VarChar);
			par.Value = string.Format ("%{0}%", condint);
			using (DataSet ds = database.EexcuteSQLWithResult (SQL, out count)) {
				long counts = (long)ds.Tables [0].Rows [0] [0];
				rtl.PageCount = Convert.ToUInt32 (counts % PerPage == 0 ? counts / PerPage : (counts / PerPage) + 1);
			}
			return rtl;
		}

		/*****************************************************************************************************
		 * 用户订单操作
		 * ***************************************************************************************************/

		/// <summary>
		/// Adds the card goods.
		/// 向购物车添加商品
		/// 所需权限
		/// WebUserGroup.VisitAndSearch
		/// </summary>
		/// <returns><c>true</c>, if card goods was added, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="gid">Gid.</param>
		public static bool AddCardGoods (WebSessionUser user, uint gid, uint count)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!AdminOption.PermissionCheck (user, WebUserGroup.VisitAndSearch))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("insert into info_cart (`uid`,`gid`,`count`) values({0},{1},{2})", user.UID, gid, count);
			int c = database.ExecuteSQLWithoutResult (SQL);
			if (c == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Removes the card goods.
		/// 移除购物车中的商品
		/// 所需权限
		/// WebUserGroup.VisitAndSearch
		/// </summary>
		/// <returns><c>true</c>, if card goods was removed, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="gid">Gid.</param>
		public static bool RemoveCardGoods (WebSessionUser user, uint gid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!AdminOption.PermissionCheck (user, WebUserGroup.VisitAndSearch))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			string SQL = string.Format ("delete from info_cart where `gid`={0} and `uid`={1}", gid, user.UID);
			int c = database.ExecuteSQLWithoutResult (SQL);
			if (c == 0)
				return false;
			return true;
		}

		/// <summary>
		/// Creates the indent.
		/// 创建订单
		/// 所需权限
		/// WebUserGroup.SubmitAndRollBack
		/// </summary>
		/// <returns><c>true</c>, if indent was created, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="goods">Goods.所选商品</param>
		public static bool CreateIndent (WebSessionUser user, WebUserCartGoodsCollection goods, uint aid, string commit)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			if (!AdminOption.PermissionCheck (user, WebUserGroup.SubmitAndRollBack))
				return false;
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (!database.ExecuteWithTrans (cmd => {
				string bid = IndentIdCreater (user.UID);
				List<string> listSQL = new List<string> ();
				uint price = 0;
				foreach (WebUserCart cart in goods) {
					listSQL.Add (string.Format (
						"insert into info_indentgoods values('{0}',{1},{2})", bid, cart.Goods.GID, cart.Count));
					price += (cart.Count * cart.Goods.GPrice);
				}
				string SQL = string.Format (
					             "insert into info_indent (`bid`,`createtime`,`commit`,`piace`,`uid`,`aid`) values ('{0}',now(),?commit,{1},{2},{3})", 
					             bid, price, user.UID, aid);
				MySqlParameter parcommit = new MySqlParameter ("?commit", MySqlDbType.VarChar);
				parcommit.Value = commit;
				cmd.CommandText = SQL;
				cmd.Parameters.Add (parcommit);
				cmd.ExecuteNonQuery ();
				//_______________________________
				cmd.Parameters.Clear ();
				foreach (string sql in listSQL) {
					cmd.CommandText = sql;
					cmd.ExecuteNonQuery ();
				}
				return true;
			}))
				return false;
			foreach (WebUserCart cart in goods) {
				RemoveCardGoods (user, cart.Goods.GID);
			}
			return true;
		}

		/// <summary>
		/// Indents the identifier creater.
		/// 订单号生成
		/// </summary>
		/// <returns>The identifier creater.</returns>
		private static string IndentIdCreater (uint uid)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append (uid);
			DateTime now = DateTime.Now;
			sb.Append (now.ToString ("yyyyMMdd"));
			sb.Append (now.Millisecond);
			return sb.ToString ();
		}

		/// <summary>
		/// Determines whether this instance cancel indent the specified user bid.
		/// 取消订单
		/// </summary>
		/// <returns><c>true</c> if this instance cancel indent the specified user bid; otherwise, <c>false</c>.</returns>
		/// <param name="user">User.</param>
		/// <param name="bid">Bid.</param>
		public static bool CancelIndent (WebSessionUser user, string bid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("update info_indent set `status`=6 where `bid`='{0}' and `status`=1", bid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (database.ExecuteSQLWithoutResult (SQL) != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Payments the indent.
		/// 付款
		/// </summary>
		/// <returns><c>true</c>, if indent was paymented, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="bid">Bid.</param>
		public static bool PaymentIndent (WebSessionUser user, string bid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("update info_indent set `status`=2 where `bid`='{0}' and `status`=1", bid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (database.ExecuteSQLWithoutResult (SQL) != 0)
				return true;
			return false;
		}

		/// <summary>
		/// Recives the goods.
		/// </summary>
		/// <returns><c>true</c>, if goods was recived, <c>false</c> otherwise.</returns>
		/// <param name="user">User.</param>
		/// <param name="bid">Bid.</param>
		public static bool ReciveGoods (WebSessionUser user, string bid)
		{
			if (user == null)
				return false;
			if (!user.LoginSign)
				return false;
			string SQL = string.Format ("update info_indent set `status`=4 where `bid`='{0}' and `status`=3", bid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			if (database.ExecuteSQLWithoutResult (SQL) != 0)
				return true;
			return false;
		}

		public static string GetGoodsImage (uint gid)
		{
			string SQL = string.Format ("select `gpic` from info_foods where gid={0}", gid);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			if (count == 0)
				return null;
			return (string)ds.Tables [0].Rows [0] ["gpic"];
		}

		/// <summary>
		/// Gets the cart goods.
		/// </summary>
		/// <returns>The cart goods.</returns>
		public static WebUserCartGoodsCollection GetCartGoods (WebSessionUser user)
		{
			if (user == null)
				return null;
			if (!user.LoginSign)
				return null;
			string SQL = string.Format ("select info_cart.csign,info_cart.count,info_foods.* from info_cart,info_foods where info_cart.gid = info_foods.gid and info_cart.uid={0}", user.UID);
			MysqlHelper database = new MysqlHelper (DatabaseUser.DataServer, DatabaseUser.DataPort,
				                       DatabaseUser.DataUser, DatabaseUser.DataPasswd, DatabaseUser.Database);
			int count = 0;
			DataSet ds = database.EexcuteSQLWithResult (SQL, out count);
			WebUserCartGoodsCollection cart = new WebUserCartGoodsCollection ();
			foreach (DataRow i in ds.Tables[0].Rows) {
				cart.Add (new WebUserCart () { 
					CSIGN = (uint)i ["csign"],
					Count = (uint)i ["count"],
					Goods = new WebGoodsInfo () {
						GID = (uint)i ["gid"],
						GName = (string)i ["gname"],
						GPicture = (string)i ["gpic"],
						GCommit = (string)i ["gcommit"],
						GPrice = (uint)i ["gprice"],
						Status = (byte)i ["status"]
					}
				});
			}
			return cart;
		}
	}
}

