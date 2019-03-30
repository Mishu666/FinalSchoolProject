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
    public int FollowersCount, FollowingCount;
    public string Username, Password, Email;
    public DateTime DOB, CreationDate;
    public bool IsAdmin;

    #region constructors

    private UsersClass()
    {

    }

    private UsersClass(int ID, int FollowersCount, int FollowingCount, string Username, string Password,
        string Email, DateTime DOB, DateTime CreationDate, bool IsAdmin)
    {
        this.ID = ID;
        this.FollowersCount = FollowersCount;
        this.FollowingCount = FollowingCount;
        this.Username = Username;
        this.Password = Password;
        this.Email = Email;
        this.DOB = DOB;
        this.CreationDate = CreationDate;
        this.IsAdmin = IsAdmin;
    }

    public UsersClass(DataRow user_dr)
    {
        this.ID = Convert.ToInt32(user_dr["ID"]);
        this.FollowersCount = Convert.ToInt32(user_dr["FollowersCount"]);
        this.FollowingCount = Convert.ToInt32(user_dr["FollowingCount"]);
        this.Username = user_dr["Username"].ToString();
        this.Password = user_dr["Password"].ToString();
        this.Email = user_dr["Email"].ToString();
        this.DOB = Convert.ToDateTime(user_dr["DOB"]);
        this.CreationDate = Convert.ToDateTime(user_dr["CreationDate"]);
        this.IsAdmin = Convert.ToBoolean(user_dr["IsAdmin"]);
    }

    public UsersClass(string Username, string Password, string Email, DateTime DOB)
    {
        this.Username = Username;
        this.Password = Password;
        this.Email = Email;
        this.DOB = DOB;
        this.IsAdmin = false;
    }

    #endregion

    #region sql functions

    public void Insert()
    {
        DateTime CreationDate = DateTime.Now;
        string sql_str = "INSERT INTO [Users] " +
            "([Username], [Password], [Email], [DOB], [IsAdmin], [CreationDate]) " +
            "VALUES ('{0}','{1}','{2}',#{3}#,{4},#{5}#)";
        string.Format(sql_str, this.Username, this.Password, this.Email, this.DOB, this.IsAdmin, CreationDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Update()
    {
        string sql_str = "Update [Users] " +
            "SET [Username] = '{0}', [Password] = '{1}', [Email] = '{2}', " +
            "[FollowersCount] = {3}, [FollowingCount] = {4} " +
            "[DOB] = #{5}#, [IsAdmin] = {6}";
        string.Format(sql_str, this.Username, this.Password, this.Email,
            this.FollowersCount, this.FollowingCount, this.DOB, this.IsAdmin);
        Dbase.ChangeTable(sql_str);
    }

    #endregion
}