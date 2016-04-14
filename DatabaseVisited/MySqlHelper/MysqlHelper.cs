using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DatabaseVisited.MySqlHelper
{
	/// <summary>
	/// Translant proc.
	/// </summary>
	public delegate bool TranslantProc (MySqlCommand cmd);
	/// <summary>
	/// Mysql helper.
	/// </summary>
	public sealed class MysqlHelper
	{
		/// <summary>
		/// Gets or sets the server address.
		/// </summary>
		/// <value>The server address.</value>
		public string ServerAddr{ get; set; }

		/// <summary>
		/// Gets or sets the server port.
		/// </summary>
		/// <value>The server port.</value>
		public string ServerPort{ get; set; }

		/// <summary>
		/// Gets or sets the server user.
		/// </summary>
		/// <value>The server user.</value>
		public string ServerUser{ get; set; }

		/// <summary>
		/// Gets or sets the server password.
		/// </summary>
		/// <value>The server password.</value>
		public string ServerPasswd{ get; set; }

		/// <summary>
		/// Gets or sets the server database.
		/// </summary>
		/// <value>The server database.</value>
		public string ServerDatabase{ get; set; }

		/// <summary>
		/// Gets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString {
			get {
				return string.Format ("server={0},{1};uid={2};pwd={3};database={4};charset=utf8",
					this.ServerAddr,
					this.ServerPort,
					this.ServerUser,
					this.ServerPasswd,
					this.ServerDatabase);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseVisited.MySqlHelper.MysqlHelper"/> class.
		/// </summary>
		public MysqlHelper ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseVisited.MySqlHelper.MysqlHelper"/> class.
		/// </summary>
		/// <param name="addr">Address.</param>
		/// <param name="port">Port.</param>
		/// <param name="uid">Uid.</param>
		/// <param name="pwd">Pwd.</param>
		/// <param name="database">Database.</param>
		public MysqlHelper (string addr, string port, 
		                    string uid, string pwd, 
		                    string database)
		{
			this.ServerAddr = addr;
			this.ServerPort = port;
			this.ServerUser = uid;
			this.ServerPasswd = pwd;
			this.ServerDatabase = database;
		}

		/// <summary>
		/// Eexcutes the SQL with result.
		/// </summary>
		/// <returns>The SQL with result.</returns>
		/// <param name="SqlString">Sql string.</param>
		/// <param name="type">Type.</param>
		/// <param name="Params">Parameters.</param>
		public DataSet EexcuteSQLWithResult (string SqlString,
		                                     out int ReturnLine,
		                                     CommandType type = CommandType.Text,
		                                     params MySqlParameter[] Params)
		{
			ReturnLine = 0;
			using (MySqlConnection conn = new MySqlConnection (this.ConnectionString)) {
				try {
					conn.Open ();
					using (MySqlCommand cmd = conn.CreateCommand ()) {
						cmd.CommandText = SqlString;
						cmd.CommandType = type;
						cmd.Parameters.AddRange (Params);
						using (MySqlDataAdapter da = new MySqlDataAdapter (cmd)) {
							DataSet ds = new DataSet ();
							ReturnLine = da.Fill (ds);
							return ds;
						}
					}
				} catch (MySqlException) {
					return null;
				} finally {
					if (conn.State == ConnectionState.Open)
						conn.Close ();
				}
			}
		}

		/// <summary>
		/// Executes the SQL without result.
		/// </summary>
		/// <returns>The SQL without result.</returns>
		/// <param name="SqlString">Sql string.</param>
		/// <param name="type">Type.</param>
		/// <param name="Params">Parameters.</param>
		public int ExecuteSQLWithoutResult (string SqlString,
		                                    CommandType type = CommandType.Text,
		                                    params MySqlParameter[] Params)
		{
			using (MySqlConnection conn = new MySqlConnection (this.ConnectionString)) {
				try {
					conn.Open ();
					using (MySqlCommand cmd = conn.CreateCommand ()) {
						cmd.CommandText = SqlString;
						cmd.CommandType = type;
						cmd.Parameters.AddRange (Params);
						return cmd.ExecuteNonQuery ();
					}
				} catch (MySqlException) {
					return 0;
				} finally {
					if (conn.State == ConnectionState.Open)
						conn.Close ();
				}
			}
		}

		/// <summary>
		/// Executes the with trans.
		/// </summary>
		/// <returns><c>true</c>, if with trans was executed, <c>false</c> otherwise.</returns>
		/// <param name="proc">Proc.</param>
		public bool ExecuteWithTrans (TranslantProc proc,
		                              IsolationLevel level = IsolationLevel.ReadCommitted)
		{
			using (MySqlConnection conn = new MySqlConnection (this.ConnectionString)) {
				try {
					conn.Open ();
					using (MySqlTransaction trans = conn.BeginTransaction (level)) {
						using (MySqlCommand cmd = conn.CreateCommand ()) {
							cmd.Transaction = trans;
							try {
								if (!proc (cmd)) {
									trans.Rollback ();
									return false;
								}
							} catch (Exception) {
								trans.Rollback ();
								return false;
							}
							trans.Commit ();
							return true;
						}
					}
				} catch (MySqlException) {
					return false;
				} finally {
					if (conn.State == ConnectionState.Open)
						conn.Close ();
				}
			}
		}
	}
}

