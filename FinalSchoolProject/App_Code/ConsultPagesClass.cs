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
    public readonly int ID;
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

    public ConsultPagesClass(string PageName)
    {
        this.PageName = PageName;
        this.SubscriberCount = 0;
        this.Status = "active";
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

        string sql_str = "INSERT INTO [ConsultPages] " +
            "([SubscriberCount], [PageName], [Status]) " +
            "VALUES ({0}, '{1}', '{2}') ";

        sql_str = string.Format(sql_str, this.SubscriberCount, this.PageName, this.Status);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
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

    public DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [ConsultPages]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [ConsultPages] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}