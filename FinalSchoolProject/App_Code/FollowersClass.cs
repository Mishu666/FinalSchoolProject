using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for FollowersClass
/// </summary>
public class FollowersClass
{
    public readonly int ID;
    public int FollowerID, FollowedID;
    public DateTime FollowDate;

    #region constructors

    private FollowersClass()
    {

    }

    private FollowersClass(int ID, int FollowerID, int FollowedID, DateTime FollowDate)
    {
        this.ID = ID;
        this.FollowerID = FollowerID;
        this.FollowedID = FollowedID;
        this.FollowDate = FollowDate;
    }

    private FollowersClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.FollowerID = Convert.ToInt32(dr["FollowerID"]);
        this.FollowedID = Convert.ToInt32(dr["FollowedID"]);
        this.FollowDate = Convert.ToDateTime(dr["FollowDate"]);
    }

    public FollowersClass(int FollowerID, int FollowedID)
    {
        this.FollowerID = FollowerID;
        this.FollowedID = FollowedID;
        this.FollowDate = DateTime.Now;
        this.ID = this.Insert();
    }

    #endregion

    #region sql functions

    private int Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Followers] " +
            "([FollowerID], [FollowedID], [FollowDate]) " +
            "VALUES ({0}, {1}, #{2}#) ";

        sql_str = string.Format(sql_str, this.FollowerID, this.FollowedID, this.FollowDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Followers] " +
            "SET [FollowerID] = {0}, [FollowedID] = {1}, [FollowDate] = {2}";
        sql_str = string.Format(sql_str, this.FollowerID, this.FollowedID, this.FollowDate);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Followers]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Followers] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}