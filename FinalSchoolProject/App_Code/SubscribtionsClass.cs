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
    public readonly int ID;
    public int SubscriberID, SubscribedPageID;
    public DateTime SubscribtionDate;

    #region constructors

    private SubscribtionsClass()
    {

    }

    private SubscribtionsClass(int ID, int SubscriberID,
        int SubscribedPageID , DateTime SubscribtionDate)
    {
        this.ID = ID;
        this.SubscriberID = SubscriberID;
        this.SubscribedPageID = SubscribedPageID;
        this.SubscribtionDate = SubscribtionDate;
    }

    private SubscribtionsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.SubscriberID = Convert.ToInt32(dr["SubscriberID"]);
        this.SubscribedPageID = Convert.ToInt32(dr["SubscribedPageID"]);
        this.SubscribtionDate = Convert.ToDateTime(dr["SubscribtionDate"]);
    }

    public SubscribtionsClass(int SubscriberID, int SubscribedPageID)
    {
        this.SubscriberID = SubscriberID;
        this.SubscribedPageID = SubscribedPageID;
        this.SubscribtionDate = DateTime.Now;
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

        string sql_str = "INSERT INTO [Subscribtions] " +
            "([SubscriberID], [SubscribedPageID], [SubscribtionDate]) " +
            "VALUES ({0}, {1}, #{3}#) ";

        sql_str = string.Format(sql_str, this.SubscriberID, this.SubscribedPageID, this.SubscribtionDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Subscribtions] " +
            "SET [SubscriberID] = {0}, [SubscribedPageID] = {1}, [SubscribtionDate] = #{3}#";
        sql_str = string.Format(sql_str, this.SubscriberID, this.SubscribedPageID, this.SubscribtionDate);
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

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Subscribtions] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}