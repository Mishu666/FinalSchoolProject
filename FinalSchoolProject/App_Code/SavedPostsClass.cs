using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for SavedPosts
/// </summary>
public class SavedPostsClass
{
    public readonly int ID;
    public int SaverID, SavedPostID;
    public DateTime SaveDate;

    #region constructors

    private SavedPostsClass()
    {

    }

    private SavedPostsClass(int ID, int SaverID,
        int SavedPostID, DateTime SaveDate)
    {
        this.ID = ID;
        this.SaverID = SaverID;
        this.SavedPostID = SavedPostID;
        this.SaveDate = SaveDate;
    }

    private SavedPostsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.SaverID = Convert.ToInt32(dr["SaverID"]);
        this.SavedPostID = Convert.ToInt32(dr["SavedPostID"]);
        this.SaveDate = Convert.ToDateTime(dr["SaveDate"]);
    }

    public SavedPostsClass(int SaverID, int SavedPostID)
    {
        this.SaverID = SaverID;
        this.SavedPostID = SavedPostID;
        this.SaveDate = DateTime.Now;
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

        string sql_str = "INSERT INTO [SavedPosts] " +
            "([SaverID], [SavedPostID], [SaveDate]) " +
            "VALUES ({0}, {1}, #{3}#) ";

        sql_str = string.Format(sql_str, this.SaverID, this.SavedPostID, this.SaveDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [SavedPosts] " +
            "SET [SaverID] = {0}, [SavedPostID] = {1}, [SaveDate] = #{3}#";
        sql_str = string.Format(sql_str, this.SaverID, this.SavedPostID, this.SaveDate);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [SavedPosts]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [SavedPosts] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}