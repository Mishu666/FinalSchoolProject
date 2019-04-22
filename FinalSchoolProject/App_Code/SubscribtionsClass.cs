using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Subscribtions
/// </summary>
public class SubscribtionsClass
{
    public int ID { get; private set; }
    public int SubscriberID, SubscribedPageID;
    public DateTime SubscribtionDate;

    #region constructors

    private SubscribtionsClass()
    {

    }

    private SubscribtionsClass(int ID, int SubscriberID,
        int SubscribedPageID, DateTime SubscribtionDate)
    {
        this.ID = ID;
        this.SubscriberID = SubscriberID;
        this.SubscribedPageID = SubscribedPageID;
        this.SubscribtionDate = SubscribtionDate;
    }

    public static SubscribtionsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        SubscribtionsClass obj = new SubscribtionsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            SubscriberID = Convert.ToInt32(dr["SubscriberID"]),
            SubscribedPageID = Convert.ToInt32(dr["SubscribedPageID"]),
            SubscribtionDate = Convert.ToDateTime(dr["SubscribtionDate"])
        };
        return obj;
    }

    public static SubscribtionsClass CreateNew(int SubscriberID, int SubscribedPageID)
    {
        SubscribtionsClass sub = new SubscribtionsClass
        {
            SubscriberID = SubscriberID,
            SubscribedPageID = SubscribedPageID,
            SubscribtionDate = DateTime.Now
        };
        sub.Insert();
        return sub;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Subscribtions] " +
            "([SubscriberID], [SubscribedPageID], [SubscribtionDate]) " +
            "VALUES ({0}, {1}, '{3}') ";

        sql_str = string.Format(sql_str, this.SubscriberID, this.SubscribedPageID, this.SubscribtionDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Subscribtions] " +
            "SET [SubscriberID] = {0}, [SubscribedPageID] = {1}, [SubscribtionDate] = '{3}'";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.SubscriberID, this.SubscribedPageID, this.SubscribtionDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Subscribtions] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Subscribtions]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static SubscribtionsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Subscribtions]";
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
}