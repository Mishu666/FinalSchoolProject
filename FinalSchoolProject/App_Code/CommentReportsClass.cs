using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CommentReportsClass
/// </summary>
public class CommentReportsClass
{
    public readonly int ID;
    public int ReporterID, ReportedCommentID;
    public string Body;
    public DateTime CreationDate;

    #region constructors

    private CommentReportsClass()
    {

    }

    private CommentReportsClass(int ID, int ReporterID,
        int ReportedCommentID, string Body, DateTime CreationDate)
    {
        this.ID = ID;
        this.ReporterID = ReporterID;
        this.ReportedCommentID = ReportedCommentID;
        this.Body = Body;
        this.CreationDate = CreationDate;
    }

    private CommentReportsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.ReporterID = Convert.ToInt32(dr["ReporterID"]);
        this.ReportedCommentID = Convert.ToInt32(dr["ReportedCommentID"]);
        this.Body = dr["Body"].ToString();
        this.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
    }

    public CommentReportsClass(int ReporterID, int ReportedCommentID, string Body)
    {
        this.ReporterID = ReporterID;
        this.ReportedCommentID = ReportedCommentID;
        this.Body = Body;
        this.CreationDate = DateTime.Now;
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

        string sql_str = "INSERT INTO [CommentReports] " +
            "([ReporterID], [ReportedCommentID], [Body], [CreationDate]) " +
            "VALUES ({0}, {1}, '{2}', #{3}#) ";

        sql_str = string.Format(sql_str, this.ReporterID, this.ReportedCommentID, this.Body, this.CreationDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [CommentReports] " +
            "SET [ReporterID] = {0}, [ReportedCommentID] = {1}, " +
            "[Body] = '{2}', [CreationDate] = #{3}#";
        sql_str = string.Format(sql_str, this.ReporterID, this.ReportedCommentID, this.Body, this.CreationDate);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [CommentReports]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [CommentReports] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}