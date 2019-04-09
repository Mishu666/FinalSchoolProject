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
    public int ID { get; private set; }
    public int SubscriberCount;
    public string PageName, Status; //active - closed

    #region constructors

    private ConsultPagesClass()
    {

    }

    private ConsultPagesClass(int ID, int SubscriberCount, string PageName, string Status)
    {
        this.ID = ID;
        this.SubscriberCount = SubscriberCount;
        this.PageName = PageName;
        this.Status = Status;
    }

    private ConsultPagesClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.SubscriberCount = Convert.ToInt32(dr["SubscriberCount"]);
        this.PageName = dr["PageName"].ToString();
        this.Status = dr["Status"].ToString();
    }

    public static ConsultPagesClass CreateNew(string PageName)
    {
        ConsultPagesClass page = new ConsultPagesClass
        {
            PageName = PageName,
            SubscriberCount = 0,
            Status = "active"
        };
        page.Insert();
        return page;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [ConsultPages] " +
            "([SubscriberCount], [PageName], [Status]) " +
            "VALUES ({0}, '{1}', '{2}') ";

        sql_str = string.Format(sql_str, this.SubscriberCount, this.PageName, this.Status);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [ConsultPages] " +
            "SET [SubscriberCount] = {0}, [PageName] = '{1}', [Status] = '{2}'";
        sql_str = string.Format(sql_str, this.SubscriberCount, this.PageName, this.Status);
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