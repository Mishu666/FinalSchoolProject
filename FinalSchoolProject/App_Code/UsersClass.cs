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
    public readonly int ID;
    public int Flags;
    public int MyFollowersCount, FollowingCount;
    public string Username, Password, Email;
    public DateTime DOB, CreationDate;
    public bool IsAdmin, IsSuspended, IsPrivate, IsDeleted;

    #region constructors

    private UsersClass()
    {

    }

    private UsersClass(int ID, int MyFollowersCount, int FollowingCount, int Flags, 
        string Username, string Password, string Email, 
        DateTime DOB, DateTime CreationDate, 
        bool IsAdmin, bool IsSuspended, bool IsPrivate, bool IsDeleted)
    {
        this.ID = ID;
        this.MyFollowersCount = MyFollowersCount;
        this.FollowingCount = FollowingCount;
        this.Flags = Flags;
        this.Username = Username;
        this.Password = Password;
        this.Email = Email;
        this.DOB = DOB;
        this.CreationDate = CreationDate;
        this.IsAdmin = IsAdmin;
        this.IsSuspended = IsSuspended;
        this.IsPrivate = IsPrivate;
        this.IsDeleted = IsDeleted;
    }

    public UsersClass(DataRow user_dr)
    {
        this.ID = Convert.ToInt32(user_dr["ID"]);
        this.MyFollowersCount = Convert.ToInt32(user_dr["MyFollowersCount"]);
        this.FollowingCount = Convert.ToInt32(user_dr["FollowingCount"]);
        this.Flags = Convert.ToInt32(user_dr["Flags"]);
        this.Username = user_dr["Username"].ToString();
        this.Password = user_dr["Password"].ToString();
        this.Email = user_dr["Email"].ToString();
        this.DOB = Convert.ToDateTime(user_dr["DOB"]);
        this.CreationDate = Convert.ToDateTime(user_dr["CreationDate"]);
        this.IsAdmin = Convert.ToBoolean(user_dr["IsAdmin"]);
        this.IsSuspended = Convert.ToBoolean(user_dr["IsSuspended"]);
        this.IsPrivate = Convert.ToBoolean(user_dr["IsPrivate"]);
        this.IsDeleted = Convert.ToBoolean(user_dr["IsDeleted"]);
    }

    public UsersClass(string Username, string Password, string Email, DateTime DOB)
    {
        this.Username = Username;
        this.Password = Password;
        this.Email = Email;
        this.DOB = DOB;
        this.IsAdmin = false;
        this.MyFollowersCount = 0;
        this.FollowingCount = 0;
        this.Flags = 0;
        this.CreationDate = DateTime.Now;
        this.IsAdmin = false;
        this.IsSuspended = false;
        this.IsPrivate = false;
        this.IsDeleted = false;
    }

    #endregion

    #region sql functions

    public void Insert()
    {
        DateTime CreationDate = DateTime.Now;
        string sql_str = "INSERT INTO [Users] " +
            "([Username], [Password], [Email], [MyFollowersCount], [FollowingCount],[Flags], " +
            "[CreationDate], [DOB], [IsAdmin], [IsSuspended], [IsPrivate], [IsDeleted]) " +
            "VALUES ('{0}','{1}','{2}', {3}, {4}, {5}, #{6}#, #{7}#, {8}, {9}, {10}, {11})";
        string.Format(sql_str, this.Username, this.Password, this.Email, this.MyFollowersCount, this.FollowingCount,this.Flags,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted);
        Dbase.ChangeTable(sql_str);
    }

    public void Update()
    {
        string sql_str = "Update [Users] " +
            "SET [Username] = '{0}', [Password] = '{1}', [Email] = '{2}', " +
            "[MyFollowersCount] = {3}, [FollowingCount] = {4} , [Flags] = {5}, " +
            "[CreationDate] = #{6}#, [DOB] = #{7}#, [IsAdmin] = {8}, [IsSuspended] = {9}, " +
            "[IsPrivate] = {10}, [IsDeleted] = {11}";
        string.Format(sql_str, this.Username, this.Password, this.Email, this.MyFollowersCount, this.FollowingCount, this.Flags,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public List<UsersClass> GetAll()
    {
        string sql_str = "SELECT * FROM [Users]";
        DataTable all = Dbase.SelectFromTable(sql_str);
        int count = all.Rows.Count;

        List<UsersClass> result = new List<UsersClass>();

        for (int i = 0; i < count; i++)
        {
            result.Add(new UsersClass(all.Rows[i]));
        }

        return result;
    }
    
    public static UsersClass GetByProperty(string property, object val)
    {

        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Users] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        if (user_dt.Rows.Count == 0) return null;

        UsersClass user = new UsersClass(user_dt.Rows[0]);
        return user;
    }
    
    public static UsersClass GetByCredentials(string username, string password)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}' AND [Password]='{1}'";
        string.Format(sql_str, username, password);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        if (user_dt.Rows.Count == 0) return null;

        UsersClass user = new UsersClass(user_dt.Rows[0]);
        return user;
    }
    
    #endregion

    #region utility functions
    
    public static bool UserNameTaken(string username)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}'";
        string.Format(sql_str, username);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt.Rows.Count > 0;
    }
    
    public static bool UserExists(string username, string password)
    {
        UsersClass user = GetByCredentials(username, password);
        return user != null;

    }
    #endregion

}