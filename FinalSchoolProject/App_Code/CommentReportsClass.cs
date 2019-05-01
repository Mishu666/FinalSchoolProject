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
    public int ID { get; protected set; }
    public int ReporterID, ReportedCommentID;
    public string Body;
    public DateTime CreationDate;

    #region constructors

    protected CommentReportsClass()
    {

    }

    protected CommentReportsClass(int ID, int ReporterID,
        int ReportedCommentID, string Body, DateTime CreationDate)
    {
        this.ID = ID;
        this.ReporterID = ReporterID;
        this.ReportedCommentID = ReportedCommentID;
        this.Body = Body;
        this.CreationDate = CreationDate;
    }

    public static CommentReportsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        CommentReportsClass obj = new CommentReportsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            ReporterID = Convert.ToInt32(dr["ReporterID"]),
            ReportedCommentID = Convert.ToInt32(dr["ReportedCommentID"]),
            Body = dr["Body"].ToString(),
            CreationDate = Convert.ToDateTime(dr["CreationDate"])
        };
        return obj;
    }

    public static CommentReportsClass CreateNew(int ReporterID, int ReportedCommentID, string Body)
    {
        CommentReportsClass commentreport = new CommentReportsClass
        {
            ReporterID = ReporterID,
            ReportedCommentID = ReportedCommentID,
            Body = Body,
            CreationDate = DateTime.Now
        };
        
        commentreport.Insert();
        return commentreport;
    }

    #endregion

    #region sql functions

    protected void Insert()
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

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [CommentReports] " +
            "SET [ReporterID] = {0}, [ReportedCommentID] = {1}, " +
            "[Body] = '{2}', [CreationDate] = #{3}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.ReporterID, this.ReportedCommentID, this.Body, this.CreationDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [CommentReports] WHERE [ID]=" + this.ID;
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

    public static CommentReportsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [CommentReports]";
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