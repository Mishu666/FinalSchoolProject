using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UsersClass
/// </summary>
public class UsersClass
{
    public readonly int ID = 0;
    public int Flags, Points, MyFollowersCount, FollowingCount;
    public string Username, Password;
    public DateTime DOB, CreationDate;
    public bool IsAdmin, IsSuspended, IsPrivate, IsDeleted;

    #region constructors

    private UsersClass()
    {

    }

    private UsersClass(int ID, int MyFollowersCount, int FollowingCount, int Flags, int Points,
        string Username, string Password, 
        DateTime DOB, DateTime CreationDate, 
        bool IsAdmin, bool IsSuspended, bool IsPrivate, bool IsDeleted)
    {
        this.ID = ID;
        this.MyFollowersCount = MyFollowersCount;
        this.FollowingCount = FollowingCount;
        this.Flags = Flags;
        this.Points = Points;
        this.Username = Username;
        this.Password = Password;
        this.DOB = DOB;
        this.CreationDate = CreationDate;
        this.IsAdmin = IsAdmin;
        this.IsSuspended = IsSuspended;
        this.IsPrivate = IsPrivate;
        this.IsDeleted = IsDeleted;
    }

    private UsersClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.MyFollowersCount = Convert.ToInt32(dr["MyFollowersCount"]);
        this.FollowingCount = Convert.ToInt32(dr["FollowingCount"]);
        this.Flags = Convert.ToInt32(dr["Flags"]);
        this.Points = Convert.ToInt32(dr["Points"]);
        this.Username = dr["Username"].ToString();
        this.Password = dr["Password"].ToString();
        this.DOB = Convert.ToDateTime(dr["DOB"]);
        this.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
        this.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
        this.IsSuspended = Convert.ToBoolean(dr["IsSuspended"]);
        this.IsPrivate = Convert.ToBoolean(dr["IsPrivate"]);
        this.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
    }

    public UsersClass(string Username, string Password, DateTime DOB)
    {
        this.Username = Username;
        this.Password = Password;
        this.DOB = DOB;
        this.IsAdmin = false;
        this.MyFollowersCount = 0;
        this.FollowingCount = 0;
        this.Flags = 0;
        this.Points = 0;
        this.CreationDate = DateTime.Now;
        this.IsAdmin = false;
        this.IsSuspended = false;
        this.IsPrivate = false;
        this.IsDeleted = false;
        this.ID = this.Insert();
    }

    #endregion

    #region sql functions

    private int Insert()
    {
        if (ID != 0)
        {
            throw new Exception("user already inserted");
        }

        string sql_str = "INSERT INTO [Users] " +
            "([Username], [Password], [MyFollowersCount], [FollowingCount], [Flags], [Points], " +
            "[CreationDate], [DOB], [IsAdmin], [IsSuspended], [IsPrivate], [IsDeleted]) " +
            "VALUES ('{0}','{1}', {2}, {3}, {4}, {5}, #{6}#, #{7}#, {8}, {9}, {10}, {11}) ";

        sql_str = string.Format(sql_str, this.Username, this.Password, this.MyFollowersCount, this.FollowingCount,this.Flags, this.Points,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Users] " +
            "SET [Username] = '{0}', [Password] = '{1}', " +
            "[MyFollowersCount] = {2}, [FollowingCount] = {3} , [Flags] = {4}, [Points]= {5}, " +
            "[CreationDate] = #{6}#, [DOB] = #{7}#, [IsAdmin] = {8}, [IsSuspended] = {9}, " +
            "[IsPrivate] = {10}, [IsDeleted] = {11}";
        sql_str = string.Format(sql_str, this.Username, this.Password, this.MyFollowersCount, this.FollowingCount, this.Flags, this.Points,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Users]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;

    }
    
    public static DataTable GetByProperty(string property, object val)
    {

        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Users] WHERE [{0}] = {1}{2}{1}";
        sql_str = string.Format(sql_str, property, surround, val);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }
    
    public static DataRow GetByCredentials(string username, string password)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}' AND [Password]='{1}'";
        sql_str = sql_str = string.Format(sql_str, username, password);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        if (user_dt.Rows.Count == 0) return null;

        return user_dt.Rows[0];
    }

    #endregion

    #region utility functions

    public static bool UserNameTaken(string username)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}'";
        sql_str = string.Format(sql_str, username);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt.Rows.Count > 0;
    }
    
    public static bool UserExists(string username, string password)
    {
        DataRow user = GetByCredentials(username, password);
        return user != null;

    }

    public static string GetUserUsername(int id)
    {
        string username;
        DataTable user = GetByProperty("id", id);
        if (user == null) username = "null";
        else username = user.Rows[0]["Username"].ToString();
        return username;
        
    }
    #endregion

}