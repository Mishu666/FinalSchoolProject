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
    public string FollowDate;

    #region constructors

    private FollowersClass()
    {

    }

    private FollowersClass(int ID, int FollowerID, int FollowedID, string FollowDate)
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
            FollowDate = Convert.ToString(dr["FollowDate"])
        };
        return obj;
    }

    public static FollowersClass CreateNew(int FollowerID, int FollowedID)
    {
        FollowersClass follower = new FollowersClass
        {
            FollowerID = FollowerID,
            FollowedID = FollowedID,
            FollowDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
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
            "VALUES ({0}, {1}, '{2}') ";

        sql_str = string.Format(sql_str, this.FollowerID, this.FollowedID, this.FollowDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT last_insert_rowid() AS ID";

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

    public static FollowersClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Followers]";
        string prepend, append;

        if (pairs.Length > 0) sql_str += " WHERE ";

        for (int i = 0; i < pairs.Length; i++)
        {
            prepend = "";
            append = "";
            if (pairs[i].Value is string)
            {
                prepend = "'";
                append = "'";
            }
            if (pairs[i].Value is DateTime)
            {
                prepend = "date('";
                append = "')";
            }
            sql_str += "[{0}] = {1}{2}{3}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, pairs[i].Key, prepend, pairs[i].Value, append);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }

    #endregion
}