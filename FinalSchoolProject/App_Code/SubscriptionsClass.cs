using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for Subscriptions
/// </summary>
public class SubscriptionsClass
{
    public int ID { get; protected set; }
    public int SubscriberID, PageID;
    public DateTime SubscriptionDate;

    #region constructors

    protected SubscriptionsClass()
    {

    }

    protected SubscriptionsClass(int ID, int SubscriberID,
        int PageID, DateTime SubscriptionDate)
    {
        this.ID = ID;
        this.SubscriberID = SubscriberID;
        this.PageID = PageID;
        this.SubscriptionDate = SubscriptionDate;
    }

    public static SubscriptionsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        SubscriptionsClass obj = new SubscriptionsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            SubscriberID = Convert.ToInt32(dr["SubscriberID"]),
            PageID = Convert.ToInt32(dr["PageID"]),
            SubscriptionDate = Convert.ToDateTime(dr["SubscriptionDate"])
        };
        return obj;
    }

    public static SubscriptionsClass CreateNew(int SubscriberID, int PageID)
    {
        SubscriptionsClass sub = new SubscriptionsClass
        {
            SubscriberID = SubscriberID,
            PageID = PageID,
            SubscriptionDate = DateTime.Now
        };
        sub.Insert();
        return sub;
    }

    #endregion

    #region sql functions

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Subscriptions] " +
            "([SubscriberID], [PageID], [SubscriptionDate]) " +
            "VALUES ({0}, {1}, #{2}#) ";

        sql_str = string.Format(sql_str, this.SubscriberID, this.PageID, this.SubscriptionDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Subscriptions] " +
            "SET [SubscriberID] = {0}, [PageID] = {1}, [SubscriptionDate] = #{2}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.SubscriberID, this.PageID, this.SubscriptionDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Subscriptions] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Subscriptions]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static SubscriptionsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Subscriptions]";
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