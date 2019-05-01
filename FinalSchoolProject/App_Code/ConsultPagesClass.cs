using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for ConsultPagesClass
/// </summary>
public class ConsultPagesClass
{
    public int ID { get; protected set; }
    public int SubscriberCount;
    public string PageName, Describtion;
    public bool IsLocked;

    #region constructors

    protected ConsultPagesClass()
    {

    }

    protected ConsultPagesClass(int ID, int SubscriberCount, string PageName,string Describtion, bool IsLocked)
    {
        this.ID = ID;
        this.SubscriberCount = SubscriberCount;
        this.Describtion = Describtion;
        this.PageName = PageName;
        this.IsLocked = IsLocked;
    }

    public static ConsultPagesClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        ConsultPagesClass obj = new ConsultPagesClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            SubscriberCount = Convert.ToInt32(dr["SubscriberCount"]),
            PageName = dr["PageName"].ToString(),
            Describtion = dr["Describtion"].ToString(),
            IsLocked = Convert.ToBoolean(dr["IsLocked"])
        };
        return obj;
    }

    public static ConsultPagesClass CreateNew(string PageName)
    {
        ConsultPagesClass page = new ConsultPagesClass
        {
            PageName = PageName,
            SubscriberCount = 0,
            Describtion = "",
            IsLocked = false
        };
        page.Insert();
        return page;
    }

    #endregion

    #region sql functions

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [ConsultPages] " +
            "([SubscriberCount], [PageName], [Describtion], [IsLocked]) " +
            "VALUES ({0}, '{1}', '{2}', {3}) ";

        sql_str = string.Format(sql_str, this.SubscriberCount, this.PageName, this.Describtion, this.IsLocked);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [ConsultPages] " +
            "SET [SubscriberCount] = {0}, [PageName] = '{1}', [IsLocked] = {2}, [Describtion] = '{3}'";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.SubscriberCount, this.PageName, this.IsLocked, this.Describtion);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [ConsultPages] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [ConsultPages]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static ConsultPagesClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [ConsultPages]";
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
            sql_str += "[{0}] = {1}{2}{3}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, pairs[i].Key, prepend, pairs[i].Value, append);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }

    #endregion

    #region tility methods


    #endregion
}