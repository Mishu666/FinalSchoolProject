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
    public int ID { get; protected set; }
    public int SaverID, SavedPostID;
    public DateTime SaveDate;

    #region constructors

    protected SavedPostsClass()
    {

    }

    protected SavedPostsClass(int ID, int SaverID,
        int SavedPostID, DateTime SaveDate)
    {
        this.ID = ID;
        this.SaverID = SaverID;
        this.SavedPostID = SavedPostID;
        this.SaveDate = SaveDate;
    }

    public static SavedPostsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        SavedPostsClass obj = new SavedPostsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            SaverID = Convert.ToInt32(dr["SaverID"]),
            SavedPostID = Convert.ToInt32(dr["SavedPostID"]),
            SaveDate = Convert.ToDateTime(dr["SaveDate"])
        };
        return obj;
    }

    public static SavedPostsClass CreateNew(int SaverID, int SavedPostID)
    {
        SavedPostsClass post = new SavedPostsClass
        {
            SaverID = SaverID,
            SavedPostID = SavedPostID,
            SaveDate = DateTime.Now
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
            "([SaverID], [SavedPostID], [SaveDate]) " +
            "VALUES ({0}, {1}, #{2}#)";

        sql_str = string.Format(sql_str, this.SaverID, this.SavedPostID, this.SaveDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [SavedPosts] " +
            "SET [SaverID]={0}, [SavedPostID]={1}, [SaveDate]=#{2}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.SaverID, this.SavedPostID, this.SaveDate);
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

    public static SavedPostsClass GetByID(int ID)
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