using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ModeratorsClass
/// </summary>
public class ModeratorsClass
{
    public int ID { get; private set; }
    public int PageID, ModeratorID;

    #region constructors

    private ModeratorsClass()
    {

    }

    private ModeratorsClass(int ID, int PageID, int ModeratorID)
    {
        this.ID = ID;
        this.PageID = PageID;
        this.ModeratorID = ModeratorID;
    }

    private ModeratorsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.PageID = Convert.ToInt32(dr["PageID"]);
        this.ModeratorID = Convert.ToInt32(dr["ModeratorID"]);
    }

    public static ModeratorsClass CreateNew(int PageID, int ModeratorID)
    {
        ModeratorsClass mod = new ModeratorsClass
        {
            PageID = PageID,
            ModeratorID = ModeratorID
        };
        mod.Insert();
        return mod;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Moderators] " +
            "([PageID], [ModeratorID]) " +
            "VALUES ({0}, {1}) ";

        sql_str = string.Format(sql_str, this.PageID, this.ModeratorID);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Moderators] " +
            "SET [PageID] = {0}, [ModeratorID] = {1}";
        sql_str = string.Format(sql_str, this.PageID, this.ModeratorID);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Moderators]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [CommentVotes]";
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