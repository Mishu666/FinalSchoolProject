using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;

/// <summary>
/// Summary description for NotificationsClass
/// </summary>
public class NotificationsClass
{

    public int ID { get; protected set; }
    public int BannedUserID,PageID;
    public DateTime BanDate;

    #region constructors

    protected NotificationsClass()
    {

    }

    protected NotificationsClass(int ID, int BannedUserID,
        int PageID, DateTime BanDate)
    {
        this.ID = ID;
        this.BannedUserID = BannedUserID;
        this.PageID = PageID;
        this.BanDate = BanDate;
    }

    public static NotificationsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        NotificationsClass obj = new NotificationsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            BannedUserID = Convert.ToInt32(dr["UserID"]),
            PageID = Convert.ToInt32(dr["PageID"]),
            BanDate = Convert.ToDateTime(dr["BanDate"])
        };
        return obj;
    }

    public static NotificationsClass CreateNew(int UserID, int PageID)
    {
        NotificationsClass post = new NotificationsClass
        {
            BannedUserID = UserID,
            PageID = PageID,
            BanDate = DateTime.Now
        };

        post.Insert();
        return post;
    }

    #endregion

    #region sql functions

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [SavedPosts] " +
            "([BannedUserID], [PageID], [BanDate]) " +
            "VALUES ({0}, {1}, #{2}#)";

        sql_str = string.Format(sql_str, this.BannedUserID, this.PageID, this.BanDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [SavedPosts] " +
            "SET [BannedUserID]={0}, [PageID]={1}, [BanDate]=#{2}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.BannedUserID, this.PageID, this.BanDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [SavedPosts] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [SavedPosts]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static NotificationsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [SavedPosts]";
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
                prepend = "#";
                append = "#";
            }
            sql_str += "[{0}]={1}{2}{3}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, pairs[i].Key, prepend, pairs[i].Value, append);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }

    #endregion
}