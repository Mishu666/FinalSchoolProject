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
    public readonly int ID;
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

    public ModeratorsClass(int PageID, int ModeratorID)
    {
        this.PageID = PageID;
        this.ModeratorID = ModeratorID;
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

        string sql_str = "INSERT INTO [Moderators] " +
            "([PageID], [ModeratorID]) " +
            "VALUES ({0}, {1}) ";

        sql_str = string.Format(sql_str, this.PageID, this.ModeratorID);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
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

    public DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Moderators]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Moderators] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}