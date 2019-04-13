using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PostReportsClass
/// </summary>
public class PostReportsClass
{
    public int ID { get; private set; }
    public int ReporterID, ReportedPostID;
    public string Body;
    public DateTime CreationDate;

    #region constructors

    private PostReportsClass()
    {

    }

    private PostReportsClass(int ID, int ReporterID,
        int ReportedPostID, string Body, DateTime CreationDate)
    {
        this.ID = ID;
        this.ReporterID = ReporterID;
        this.ReportedPostID = ReportedPostID;
        this.Body = Body;
        this.CreationDate = CreationDate;
    }

    public static PostReportsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        PostReportsClass obj = new PostReportsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            ReporterID = Convert.ToInt32(dr["ReporterID"]),
            ReportedPostID = Convert.ToInt32(dr["ReportedPostID"]),
            Body = dr["Body"].ToString(),
            CreationDate = Convert.ToDateTime(dr["CreationDate"])
        };
        return obj;
    }

    public static PostReportsClass CreateNew(int ReporterID, int ReportedPostID, string Body)
    {
        PostReportsClass report = new PostReportsClass
        {
            ReporterID = ReporterID,
            ReportedPostID = ReportedPostID,
            Body = Body,
            CreationDate = DateTime.Now
        };
        report.Insert();
        return report;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [PostReports] " +
            "([ReporterID], [ReportedPostID], [Body], [CreationDate]) " +
            "VALUES ({0}, {1}, '{2}', #{3}#) ";

        sql_str = string.Format(sql_str, this.ReporterID, this.ReportedPostID, this.Body, this.CreationDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [PostReports] " +
            "SET [ReporterID] = {0}, [ReportedPostID] = {1}, " +
            "[Body] = '{2}', [CreationDate] = #{3}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.ReporterID, this.ReportedPostID, this.Body, this.CreationDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [PostReports] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [PostReports]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static PostReportsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
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