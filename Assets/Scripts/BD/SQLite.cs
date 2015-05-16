//----------------------------------------------
// SQLiter
// Copyright © 2014 OuijaPaw Games LLC
//----------------------------------------------

using UnityEngine;

using UnityEngine.UI;
using System.Data;
using System.IO;
using Mono.Data.SqliteClient;

/// <summary>
/// The idea is that here is a bunch of the basics on using SQLite
/// Nothing is some advanced course on doing joins and unions and trying to make your infinitely normalized schema work
/// SQLite is simple.  Very simple.  
/// Pros:
/// - Very simple to use
/// - Very small memory footprint
/// 
/// Cons:
/// - It is a flat file database.  You can change the settings to make it run completely in memory, which will make it even
/// faster; however, you cannot have separate threads interact with it -ever-, so if you plan on using SQLite for any sort
/// of multiplayer game and want different Unity instances to interact/read data... they absolutely cannot.
/// - Doesn't offer as many bells and whistles as other DB systems
/// - It is awfully slow.  I mean dreadfully slow.  I know "slow" is a relative term, but unless the DB is all in memory, every
/// time you do a write/delete/update/replace, it has to write to a physical file - since SQLite is just a file based DB.
/// If you ever do a write and then need to read it shortly after, like .5 to 1 second after... there's a chance it hasn't been
/// updated yet... and this is local.  So, just make sure you use a coroutine or whatever to make sure data is written before
/// using it.
/// 
/// SQLite is nice for small games, high scores, simple saved, etc.  It is not very secure and not very fast, but it's cheap,
/// simple, and useful at times.
/// 
/// Here are some starting tools and information.  Go explore.
/// </summary>
public class SQLite : MonoSingleton<SQLite>
{

	public static SQLite Instance = null;
	public bool DebugMode = false;
	
	/// <summary>
	/// Table name and DB actual file location
	/// </summary>
	private const string SQL_DB_NAME = "PlayersLocal";
	
	// feel free to change where the DBs are stored
	// this file will show up in the Unity inspector after a few seconds of running it the first time
	private static string SQL_DB_LOCATION;
	
	// table name
	private const string SQL_TABLE_NAME = "Player";
	
	/// <summary>
	/// predefine columns here to there are no typos
	/// </summary>
	private const string COL_NAME = "name";  // using name as example of primary, unique, key
	private const string COL_LEVEL = "level";
	private const string COL_XP = "xp";
	private const string COL_STR = "str";
	private const string COL_SP = "sp";
	private const string COL_DEX = "dex";
	private const string COL_HEAL = "heal";
	private const string COL_STA = "sta";
	private const string COL_MSTR = "Mstr";
	private const string COL_MSP = "Msp";
	private const string COL_MDEX = "Mdex";
	private const string COL_MHEAL = "Mheal";
	private const string COL_MSTA = "Msta";
	
	/// <summary>
	/// DB objects
	/// </summary>
	private IDbConnection mConnection = null;
	private IDbCommand mCommand = null;
	private IDataReader mReader = null;
	private string mSQLString;
	
	private bool mCreateNewTable = false;

	public bool reset = false;


	void Start () {
		//SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
		//	+ "Databases" + Path.DirectorySeparatorChar
		//	+ SQL_DB_NAME + ".db";
		
		#if UNITY_STANDALONE_WIN
		//SQL_DB_LOCATION = "URI=file:C:/Users/Alex/Documents/SanctuaryGame/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + "/"
			+ SQL_DB_NAME + ".db";
		#endif
		
		#if UNITY_STANDALONE_OSX
		//SQL_DB_LOCATION = "URI=file:/Users/jTalavera/Desktop/Sanctuary/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
			//+ "Databases" + Path.DirectorySeparatorChar
			+ SQL_DB_NAME + ".db";
		#endif  
		
		Debug.Log(SQL_DB_LOCATION);
		Instance = this;
		SQLiteInit();
	}


	/// <summary>
	/// Awake will initialize the connection.  
	/// RunAsyncInit is just for show.  You can do the normal SQLiteInit to ensure that it is
	/// initialized during the AWake() phase and everything is ready during the Start() phase
	/// </summary>
	public override void Init()
	{
		//SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
		//	+ "Databases" + Path.DirectorySeparatorChar
		//	+ SQL_DB_NAME + ".db";
		
		#if UNITY_STANDALONE_WIN
		//SQL_DB_LOCATION = "URI=file:C:/Users/Alex/Documents/SanctuaryGame/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + "/"
			+ SQL_DB_NAME + ".db";
		#endif
		
		#if UNITY_STANDALONE_OSX
		//SQL_DB_LOCATION = "URI=file:/Users/jTalavera/Desktop/Sanctuary/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
			//+ "Databases" + Path.DirectorySeparatorChar
			+ SQL_DB_NAME + ".db";
		#endif  
		
		Debug.Log(SQL_DB_LOCATION);
		Instance = this;
		SQLiteInit();
	}

	/// <summary>
	/// Awake will initialize the connection.  
	/// RunAsyncInit is just for show.  You can do the normal SQLiteInit to ensure that it is
	/// initialized during the AWake() phase and everything is ready during the Start() phase
	/// </summary>
	/*void Awake()
	{
		//SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
		//	+ "Databases" + Path.DirectorySeparatorChar
		//	+ SQL_DB_NAME + ".db";

		#if UNITY_STANDALONE_WIN
		//SQL_DB_LOCATION = "URI=file:C:/Users/Alex/Documents/SanctuaryGame/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + "/"
			+ SQL_DB_NAME + ".db";
		#endif
		
		#if UNITY_STANDALONE_OSX
		//SQL_DB_LOCATION = "URI=file:/Users/jTalavera/Desktop/Sanctuary/Assets/Databases/PlayersLocal.db";
		SQL_DB_LOCATION = "URI=file:" + Application.dataPath + Path.DirectorySeparatorChar
			//+ "Databases" + Path.DirectorySeparatorChar
			+ SQL_DB_NAME + ".db";
		#endif  

		Debug.Log(SQL_DB_LOCATION);
		Instance = this;
		SQLiteInit();
	}*/
	
	/// <summary>
	/// Uncomment if you want to see the time it takes to do things
	/// </summary>
	//void Update()
	//{
	//    Debug.Log(Time.time);
	//}
	
	/// <summary>
	/// Clean up SQLite Connections, anything else
	/// </summary>
	void OnDestroy()
	{
		SQLiteClose();
	}
	
	/// <summary>
	/// Example using the Loom to run an asynchronous method on another thread so SQLite lookups
	/// do not block the main Unity thread
	/// </summary>
	public void RunAsyncInit()
	{
		LoomManager.Loom.QueueOnMainThread(() =>
		                                   {
			SQLiteInit();
		});
	}
	
	/// <summary>
	/// Basic initialization of SQLite
	/// </summary>
	private void SQLiteInit()
	{
		Debug.Log("SQLiter - Opening SQLite Connection");
		mConnection = new SqliteConnection(SQL_DB_LOCATION);
		mCommand = mConnection.CreateCommand();
		mConnection.Open();
		
		// WAL = write ahead logging, very huge speed increase
		mCommand.CommandText = "PRAGMA journal_mode = WAL;";
		mCommand.ExecuteNonQuery();
		
		// journal mode = look it up on google, I don't remember
		mCommand.CommandText = "PRAGMA journal_mode";
		mReader = mCommand.ExecuteReader();
		if (DebugMode && mReader.Read())
			Debug.Log("SQLiter - WAL value is: " + mReader.GetString(0));
		mReader.Close();
		
		// more speed increases
		mCommand.CommandText = "PRAGMA synchronous = OFF";
		mCommand.ExecuteNonQuery();
		
		// and some more
		mCommand.CommandText = "PRAGMA synchronous";
		mReader = mCommand.ExecuteReader();
		if (DebugMode && mReader.Read())
			Debug.Log("SQLiter - synchronous value is: " + mReader.GetInt32(0));
		mReader.Close();
		
		// here we check if the table you want to use exists or not.  If it doesn't exist we create it.
		mCommand.CommandText = "SELECT name FROM sqlite_master WHERE name='" + SQL_TABLE_NAME + "'";
		mReader = mCommand.ExecuteReader();
		if (!mReader.Read())
		{
			Debug.Log("SQLiter - Could not find SQLite table " + SQL_TABLE_NAME);
			mCreateNewTable = true;
		}
		mCommand.CommandText = "SELECT name FROM sqlite_master WHERE name='" + "Inventory" + "'";
		mReader = mCommand.ExecuteReader();
		if (!mReader.Read())
		{
			Debug.Log("SQLiter - Could not find SQLite table " + "Inventory");
			mCreateNewTable = true;
		}
		mReader.Close();
		
		// create new table if it wasn't found
		if (mCreateNewTable || reset)
		{
			Debug.Log("SQLiter - Creating new SQLite table " + SQL_TABLE_NAME);
			
			// insurance policy, drop table
			mCommand.CommandText = "DROP TABLE IF EXISTS " + SQL_TABLE_NAME;
			mCommand.ExecuteNonQuery();
			
			// create new - SQLite recommendation is to drop table, not clear it
			mSQLString = "CREATE TABLE IF NOT EXISTS " + SQL_TABLE_NAME + " (" +
				COL_NAME + " TEXT UNIQUE, " +
				COL_LEVEL + " INTEGER, " +
				COL_XP + " INTEGER, " + 
				"gold INTEGER, " +
				COL_MSTR + " INTEGER, " +
				COL_MSP + " INTEGER, " +
				COL_MDEX + " INTEGER, " +
				COL_MHEAL + " INTEGER, " +
				COL_MSTA + " INTEGER, " +
				"mStrExp INTEGER, " +
				"mSpExp INTEGER, " +
				"mDexExp INTEGER, " +
				"mHealExp INTEGER, " +
				"mStaExp INTEGER, " +
				"PRIMARY KEY ("+COL_NAME+"))";

			mCommand.CommandText = mSQLString;
			mCommand.ExecuteNonQuery();

			mCommand.CommandText = "DROP TABLE IF EXISTS Inventory";
			mCommand.ExecuteNonQuery();

			mSQLString = "CREATE TABLE IF NOT EXISTS Inventory (" +
				"player TEXT," +
				"position INTEGER," +
				"type INTEGER," +
				"subType INTEGER," +
				"name TEXT," +
				"rarity INTEGER," +
				"level INTEGER," + 
				"damage INTEGER," +
				"armor INTEGER," +
				"str INTEGER," +
				"sp INTEGER," +
				"dex INTEGER," +
				"heal INTEGER," + 
				"sta INTEGER," +
				"st1 INTEGER," +
				"st2 INTEGER," +
				"element INTEGER," +
				"affinity INTEGER," +
				"PRIMARY KEY (player, position))";
			mCommand.CommandText = mSQLString;
			mCommand.ExecuteNonQuery();

			mCommand.CommandText = "DROP TABLE IF EXISTS Equip";
			mCommand.ExecuteNonQuery();
			
			mSQLString = "CREATE TABLE IF NOT EXISTS Equip (" +
				"player TEXT," +
				"position INTEGER," +
				"type INTEGER," +
				"subType INTEGER," +
				"name TEXT," +
				"rarity INTEGER," +
				"level INTEGER," + 
				"damage INTEGER," +
				"armor INTEGER," +
				"str INTEGER," +
				"sp INTEGER," +
				"dex INTEGER," +
				"heal INTEGER," + 
				"sta INTEGER," +
				"st1 INTEGER," +
				"st2 INTEGER," +
				"element INTEGER," +
				"affinity INTEGER," +
				"PRIMARY KEY (player, position))";
			mCommand.CommandText = mSQLString;
			mCommand.ExecuteNonQuery();

			
			/*mCommand.CommandText = "DROP TABLE IF EXISTS Skills";
			mCommand.ExecuteNonQuery();
			
			mSQLString = "CREATE TABLE IF NOT EXISTS Skills ("
				+ "id INTEGER,"
				+ "name TEXT,"
				+ "prefab TEXT,"
				+ "icon, TEXT,"
				+ "cd INTEGER)";
			mCommand.CommandText = mSQLString;
			mCommand.ExecuteNonQuery();*/
		}
		else
		{
			if (DebugMode)
				Debug.Log("SQLiter - SQLite table " + SQL_TABLE_NAME + " was found");
		}
		mConnection.Close();
	}
	
	#region Insert
	/// <summary>
	/// Inserts a player into the database
	/// http://www.sqlite.org/lang_insert.html
	/// name must be unique, it's our primary key
	/// </summary>
	/// <param name="name"></param>
	/// <param name="raceType"></param>
	/// <param name="classType"></param>
	/// <param name="gold"></param>
	/// <param name="login"></param>
	/// <param name="level"></param>
	/// <param name="xp"></param>
	public void UpdatePlayerTable(string name, int level, int xp, int gold,
	                         int mstr, int msp, int mdex, int mheal, int msta,
	                         int mstre, int mspe, int mdexe, int mheale, int mstae)
	{

		// note - this will replace any item that already exists, overwriting them.  
		// normal INSERT without the REPLACE will throw an error if an item already exists
		mSQLString = "INSERT OR REPLACE INTO " + SQL_TABLE_NAME
			+ " ("
				+ COL_NAME + ","
				+ COL_LEVEL + ","
				+ COL_XP + ","
				+ "gold,"
				+ COL_MSTR + ","
				+ COL_MSP + ","
				+ COL_MDEX + ","
				+ COL_MHEAL + ","
				+ COL_MSTA + ","
				+ "mStrExp,"
				+ "mSpExp,"
				+ "mDexExp,"
				+ "mHealExp,"
				+ "mStaExp"
				+ ") VALUES ('"
				+ name + "',"  // note that string values need quote or double-quote delimiters
				+ level + ","
				+ xp + ","
				+ gold + ","
				+ mstr + ","
				+ msp + ","
				+ mdex + ","
				+ mheal + ","
				+ msta + ","
				+ mstre + ","
				+ mspe + ","
				+ mdexe + ","
				+ mheale + ","
				+ mstae
				+ ");";
		
		if (DebugMode)
			Debug.Log(mSQLString);
		ExecuteNonQuery(mSQLString);
	}

	public void UpdateInventoryTable(string player, int position, int type, int subType,
	                              string name, int rarity, int level,
	                              int damage, int armor, int str, int sp, int dex, int heal, int sta,
	                              int st1, int st2, int element, int affinity)
	{
		// note - this will replace any item that already exists, overwriting them.  
		// normal INSERT without the REPLACE will throw an error if an item already exists
		mSQLString = "INSERT OR REPLACE INTO " + "Inventory"
			+ " ( "
				+ "player" + ","
				+ "position" + ","
				+ "type" + ","
				+ "subType" + ","
				+ "name" + ","
				+ "rarity" + ","
				+ "level" + ","
				+ "damage" + ","
				+ "armor" + ","
				+ "str" + ","
				+ "sp" + ","
				+ "dex" + ","
				+ "heal" + ","
				+ "sta" + ","
				+ "st1" + ","
				+ "st2" + ","
				+ "element" + ","
				+ "affinity"
				+ ") VALUES ('"
				+ player + "',"  // note that string values need quote or double-quote delimiters
				+ position + ","
				+ type + ","
				+ subType + ",'"
				+ name + "',"
				+ rarity + ","
				+ level + ","
				+ damage + ","
				+ armor + ","
				+ str + ","
				+ sp + ","
				+ dex + ","
				+ heal + ","
				+ sta + ","
				+ st1 + ","
				+ st2 + ","
				+ element + ","
				+ affinity
				+ ");";
		
		if (DebugMode)
			Debug.Log(mSQLString);
		ExecuteNonQuery(mSQLString);
	}

	public void UpdateEquipTable(string player, int position, int type, int subType,
	                                 string name, int rarity, int level,
	                                 int damage, int armor, int str, int sp, int dex, int heal, int sta,
	                                 int st1, int st2, int element, int affinity)
	{
		// note - this will replace any item that already exists, overwriting them.  
		// normal INSERT without the REPLACE will throw an error if an item already exists
		mSQLString = "INSERT OR REPLACE INTO " + "Equip"
			+ " ( "
				+ "player" + ","
				+ "position" + ","
				+ "type" + ","
				+ "subType" + ","
				+ "name" + ","
				+ "rarity" + ","
				+ "level" + ","
				+ "damage" + ","
				+ "armor" + ","
				+ "str" + ","
				+ "sp" + ","
				+ "dex" + ","
				+ "heal" + ","
				+ "sta" + ","
				+ "st1" + ","
				+ "st2" + ","
				+ "element" + ","
				+ "affinity"
				+ ") VALUES ('"
				+ player + "',"  // note that string values need quote or double-quote delimiters
				+ position + ","
				+ type + ","
				+ subType + ",'"
				+ name + "',"
				+ rarity + ","
				+ level + ","
				+ damage + ","
				+ armor + ","
				+ str + ","
				+ sp + ","
				+ dex + ","
				+ heal + ","
				+ sta + ","
				+ st1 + ","
				+ st2 + ","
				+ element + ","
				+ affinity
				+ ");";
		
		if (DebugMode)
			Debug.Log(mSQLString);
		ExecuteNonQuery(mSQLString);
	}
	#endregion
	
	#region Query Values
	
	/// <summary>
	/// Quick method to show how you can query everything.  Expland on the query parameters to limit what you're looking for, etc.
	/// </summary>
	public Utils.PlayerData[] GetAllPlayers()
	{
		mConnection.Open();

		Utils.PlayerData[] players = new Utils.PlayerData[3];
		int i = 0;
		
		// if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
		mCommand.CommandText = "SELECT * FROM " + SQL_TABLE_NAME;
		mReader = mCommand.ExecuteReader();
		while (mReader.Read())
		{
			players[i] = new Utils.PlayerData(mReader.GetString(0), mReader.GetInt32(1));
			i ++;
		}
		mReader.Close();
		mConnection.Close();
		return players;
	}

	public Item[] AddAllItems(string name, string table, int length)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		
		mConnection.Open();
		
		// if you have a bunch of stuff, this is going to be inefficient and a pain.  it's just for testing/show
		mCommand.CommandText = "SELECT * FROM " + table + " WHERE player = '" + name+ "'";
		mReader = mCommand.ExecuteReader();
		Item[] inventory = new Item [length];
		while (mReader.Read())
		{
			Item i = null;
			if (mReader.GetInt32(2) == 0) {
				i = new Weapon(mReader.GetInt32(3),
				               mReader.GetString(4),
				               mReader.GetInt32(5),
				               mReader.GetInt32(6),
				               mReader.GetInt32(7),
				               mReader.GetInt32(8),
				               mReader.GetInt32(9),
				               mReader.GetInt32(10),
				               mReader.GetInt32(11),
				               mReader.GetInt32(12),
				               mReader.GetInt32(13),
				               mReader.GetInt32(14),
				               mReader.GetInt32(15),
				               mReader.GetInt32(16),
				               mReader.GetInt32(17));
			} else {
				switch(mReader.GetInt32(3)) {
				case 0: i = new Chest (mReader.GetString(4),
					                       mReader.GetInt32(5),
				                       mReader.GetInt32(6),
				                       mReader.GetInt32(7),
				                       mReader.GetInt32(8),
				                       mReader.GetInt32(9),
				                       mReader.GetInt32(10),
				                       mReader.GetInt32(11),
				                       mReader.GetInt32(12),
				                       mReader.GetInt32(13),
				                       mReader.GetInt32(14),
				                       mReader.GetInt32(15),
				                       mReader.GetInt32(16),
				                       mReader.GetInt32(17))
					                       ;break;
				case 1: i = new Legs (mReader.GetString(4),
					                       mReader.GetInt32(5),
					                       mReader.GetInt32(6),
					                       mReader.GetInt32(7),
					                       mReader.GetInt32(8),
					                       mReader.GetInt32(9),
					                       mReader.GetInt32(10),
					                       mReader.GetInt32(11),
					                       mReader.GetInt32(12),
					                       mReader.GetInt32(13),
					                       mReader.GetInt32(14),
					                       mReader.GetInt32(15),
					                       mReader.GetInt32(16),
					                       mReader.GetInt32(17))
						;break;
				case 2: i = new Boots (mReader.GetString(4),
					                       mReader.GetInt32(5),
					                       mReader.GetInt32(6),
					                       mReader.GetInt32(7),
					                       mReader.GetInt32(8),
					                       mReader.GetInt32(9),
					                       mReader.GetInt32(10),
					                       mReader.GetInt32(11),
					                       mReader.GetInt32(12),
					                       mReader.GetInt32(13),
					                       mReader.GetInt32(14),
					                       mReader.GetInt32(15),
					                       mReader.GetInt32(16),
					                       mReader.GetInt32(17))
						;break;
					//case 1: i = new Legs(); break;
				//case 2: i = new Boots(); break;
				}
			}
			inventory[mReader.GetInt32(1)] = i;

			sb.Length = 0;
			sb.Append(mReader.GetString(0)).Append(" ");
			sb.Append(mReader.GetInt32(1)).Append(" ");
			sb.Append(mReader.GetInt32(2)).Append(" ");
			sb.Append(mReader.GetInt32(3)).Append(" ");
			sb.Append(mReader.GetString(4)).Append(" ");
			sb.Append(mReader.GetInt32(5)).Append(" ");
			sb.Append(mReader.GetInt32(6)).Append(" ");
			sb.Append(mReader.GetInt32(7)).Append(" ");
			sb.Append(mReader.GetInt32(8)).Append(" ");
			sb.Append(mReader.GetInt32(9)).Append(" ");
			sb.Append(mReader.GetInt32(10)).Append(" ");
			sb.Append(mReader.GetInt32(11)).Append(" ");
			sb.Append(mReader.GetInt32(12)).Append(" ");
			sb.Append(mReader.GetInt32(13)).Append(" ");
			sb.Append(mReader.GetInt32(14)).Append(" ");
			sb.Append(mReader.GetInt32(15)).Append(" ");
			sb.Append(mReader.GetInt32(16)).Append(" ");
			sb.Append(mReader.GetInt32(17)).Append(" ");
			sb.AppendLine();
			
			// view our output
			if (DebugMode)
				Debug.Log(sb.ToString());
		}
		mReader.Close();
		mConnection.Close();
		return inventory;
	}
	
	/// <summary>
	/// Supply the column and the value you're trying to find, and it will use the primary key to query the result
	/// </summary>
	/// <param name="column"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public string QueryString(string column, string value)
	{
		string text = "";
		mConnection.Open();
		mCommand.CommandText = "SELECT " + column + " FROM " + SQL_TABLE_NAME + " WHERE " + COL_NAME + "='" + value + "'";
		mReader = mCommand.ExecuteReader();
		if (mReader.Read())
			text = mReader.GetString(0);
		else
			Debug.Log("QueryString - nothing to read...");
		mReader.Close();
		mConnection.Close();
		return text;
	}
	
	/// <summary>
	/// Supply the column and the value you're trying to find, and it will use the primary key to query the result
	/// </summary>
	/// <param name="column"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public int QueryInt(string column, string value)
	{
		int sel = -1;
		mConnection.Open();
		mCommand.CommandText = "SELECT " + column + " FROM " + SQL_TABLE_NAME + " WHERE " + COL_NAME + "='" + value + "'";
		mReader = mCommand.ExecuteReader();
		if (mReader.Read())
			sel = mReader.GetInt32(0);
		else
			Debug.Log("QueryInt - nothing to read...");
		mReader.Close();
		mConnection.Close();
		return sel;
	}
	
	/// <summary>
	/// Supply the column and the value you're trying to find, and it will use the primary key to query the result
	/// </summary>
	/// <param name="column"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public short QueryShort(string column, string value)
	{
		short sel = -1;
		mConnection.Open();
		mCommand.CommandText = "SELECT " + column + " FROM " + SQL_TABLE_NAME + " WHERE " + COL_NAME + "='" + value + "'";
		mReader = mCommand.ExecuteReader();
		if (mReader.Read())
			sel = mReader.GetInt16(0);
		else
			Debug.Log("QueryShort - nothing to read...");
		mReader.Close();
		mConnection.Close();
		return sel;
	}
	#endregion
	
	#region Update / Replace Values
	/// <summary>
	/// A 'Set' method that will set a column value for a specific player, using their name as the unique primary key
	/// to some value.  This currently just uses 'int' types, but you could modify this to use/do most anything.
	/// Remember strings need single/double quotes around their values
	/// </summary>
	/// <param name="value"></param>
	public void SetValue(string column, int value, string name)
	{
		ExecuteNonQuery("UPDATE OR REPLACE " + SQL_TABLE_NAME + " SET " + column + "=" + value + " WHERE " + COL_NAME + "='" + name + "'");
	}
	
	#endregion
	
	#region Delete
	
	/// <summary>
	/// Basic delete, using the name primary key for the 
	/// </summary>
	/// <param name="nameKey"></param>
	public void DeletePlayer(string nameKey)
	{
		ExecuteNonQuery("DELETE FROM " + SQL_TABLE_NAME + " WHERE " + COL_NAME + "='" + nameKey + "'");
		ExecuteNonQuery("DELETE FROM Inventory WHERE player ='" + nameKey + "'");
		ExecuteNonQuery("DELETE FROM Equip WHERE player ='" + nameKey + "'");
	}
	#endregion
	
	/// <summary>
	/// Basic execute command - open, create command, execute, close
	/// </summary>
	/// <param name="commandText"></param>
	public void ExecuteNonQuery(string commandText)
	{
		mConnection.Open();
		mCommand.CommandText = commandText;
		mCommand.ExecuteNonQuery();
		mConnection.Close();
	}
	
	/// <summary>
	/// Clean up everything for SQLite
	/// </summary>
	private void SQLiteClose()
	{
		if (mReader != null && !mReader.IsClosed)
			mReader.Close();
		mReader = null;
		
		if (mCommand != null)
			mCommand.Dispose();
		mCommand = null;
		
		if (mConnection != null && mConnection.State != ConnectionState.Closed)
			mConnection.Close();
		mConnection = null;
	}
	
	public void updatePlayer () {
		Debug.Log ("MIAU");
		string name = Utils.playerName;

		DeletePlayer (name);

		Attributtes atr = Utils.player.GetComponent<Attributtes> ();
		generateSlots gs = GameObject.Find ("InventoryPanel").GetComponent<generateSlots> ();
		UpdatePlayerTable (name,
		              atr.level,
		              atr.expActual,
		              gs.Gold,
		              atr.masterys [(int)Utils.Stat.FUERZA],
		              atr.masterys [(int)Utils.Stat.MAGIA],
		              atr.masterys [(int)Utils.Stat.DESTREZA],
		              atr.masterys [(int)Utils.Stat.CURA],
		              atr.masterys [(int)Utils.Stat.AGUANTE],
	                  atr.masteryExp [(int)Utils.Stat.FUERZA],
	                  atr.masteryExp [(int)Utils.Stat.MAGIA],
	                  atr.masteryExp [(int)Utils.Stat.DESTREZA],
	          	 	  atr.masteryExp [(int)Utils.Stat.CURA],
		              atr.masteryExp [(int)Utils.Stat.AGUANTE]);


		generateSlots inv = GameObject.Find ("InventoryPanel").GetComponent<generateSlots> ();
		for (int i = 0; i < inv.total_slots; i++ ) {
			if (inv.inventory[i] != null) {
				Item it = inv.inventory[i];
				UpdateInventoryTable(name, i, it.type, it.subType, it.name, it.rarity, it.level, it.damage, it.armor,
				                     it.stats[0], it.stats[1], it.stats[2], it.stats[3], it.stats[4],
				                     it.isStat1, it.isStat2, (int)it.isAffinity, it.affinity);
			}

		}
		PlayerEquip eq = GameObject.Find ("EquipPanel").GetComponent<PlayerEquip> ();

			if (eq.equipment.weapon != null) {
				Item it = eq.equipment.weapon;
				UpdateEquipTable(name, 0, it.type, it.subType, it.name, it.rarity, it.level, it.damage, it.armor,
				                     it.stats[0], it.stats[1], it.stats[2], it.stats[3], it.stats[4],
				                     it.isStat1, it.isStat2, (int)it.isAffinity, it.affinity);
			}
			if (eq.equipment.chest != null) {
				Item it = eq.equipment.chest;
				UpdateEquipTable(name, 1, it.type, it.subType, it.name, it.rarity, it.level, it.damage, it.armor,
				                 it.stats[0], it.stats[1], it.stats[2], it.stats[3], it.stats[4],
				                 it.isStat1, it.isStat2, (int)it.isAffinity, it.affinity);
			}
			if (eq.equipment.legs != null) {
				Item it = eq.equipment.legs;
				UpdateEquipTable(name, 2, it.type, it.subType, it.name, it.rarity, it.level, it.damage, it.armor,
				                 it.stats[0], it.stats[1], it.stats[2], it.stats[3], it.stats[4],
				                 it.isStat1, it.isStat2, (int)it.isAffinity, it.affinity);
			}
			if (eq.equipment.boots != null) {
				Item it = eq.equipment.boots;
				UpdateEquipTable(name, 3, it.type, it.subType, it.name, it.rarity, it.level, it.damage, it.armor,
				                 it.stats[0], it.stats[1], it.stats[2], it.stats[3], it.stats[4],
				                 it.isStat1, it.isStat2, (int)it.isAffinity, it.affinity);
			}
		
	}
}
