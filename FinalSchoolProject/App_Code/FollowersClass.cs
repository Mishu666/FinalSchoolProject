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
    public int ID { get; private set; }
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

    public static FollowersClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        FollowersClass obj = new FollowersClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            FollowerID = Convert.ToInt32(dr["FollowerID"]),
            FollowedID = Convert.ToInt32(dr["FollowedID"]),
            FollowDate = Convert.ToDateTime(dr["FollowDate"])
        };
        return obj;
    }

    public static FollowersClass CreateNew(int FollowerID, int FollowedID)
    {
        FollowersClass follower = new FollowersClass
        {
            FollowerID = FollowerID,
            FollowedID = FollowedID,
            FollowDate = DateTime.Now
        };
        follower.Insert();
        return follower;
    }

    #endregion

    #region sql functions

    private void Insert()
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

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Followers] " +
            "SET [FollowerID] = {0}, [FollowedID] = {1}, [FollowDate] = {2}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.FollowerID, this.FollowedID, this.FollowDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Followers] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Followers]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Followers]";
        string surround;

        if (pairs.Length > 0) sql_str += " WHERE ";

        for (int i = 0; i < pairs.Length; i++)
        {
            surround = "";
            if (pairs[i].Value is string) surround = "'";
            if (pairs[i].Value is DateTime) surround = "#";
            sql_str += "[{1}] = {0}{2}{0}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, surround, pairs[i].Key, pairs[i].Value);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }

    #endregion
}