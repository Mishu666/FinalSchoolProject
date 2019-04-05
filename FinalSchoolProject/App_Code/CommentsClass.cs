using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CommentsClass
/// </summary>
public class CommentsClass
{
    public readonly int ID;
    public int CommentorID, ParentPostID, ParentCommentID;
    public string Title, Body;
    public DateTime CreationDate;
    public bool IsRemoved, IsDeleted;


    #region constructors

    private CommentsClass()
    {

    }

    private CommentsClass(int ID, string Title, string Body,
        int CommentorID, int ParentPostID, int ParentCommentID,
        DateTime CreationDate, bool IsRemoved, bool IsDeleted)
    {
        this.ID = ID;
        this.Title = Title;
        this.Body = Body;
        this.CommentorID = CommentorID;
        this.ParentPostID = ParentPostID;
        this.ParentCommentID = ParentCommentID;
        this.CreationDate = CreationDate;
        this.IsRemoved = IsRemoved;
        this.IsDeleted = IsDeleted;
    }

    private CommentsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.CommentorID = Convert.ToInt32(dr["CommentorID"]);
        this.ParentPostID = Convert.ToInt32(dr["ParentPostID"]);
        this.ParentCommentID = Convert.ToInt32(dr["ParentCommentID"]);
        this.Title = dr["Title"].ToString();
        this.Body = dr["Body"].ToString();
        this.CreationDate = Convert.ToDateTime(dr["CreationDate"]);
        this.IsRemoved = Convert.ToBoolean(dr["IsRemoved"]);
        this.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
    }

    public CommentsClass(string Title, string Body,
        int CommentorID, int ParentPostID, int ParentCommentID)
    {
        this.Title = Title;
        this.Body = Body;
        this.CommentorID = CommentorID;
        this.ParentPostID = ParentPostID;
        this.ParentCommentID = ParentCommentID;
        this.CreationDate = DateTime.Now;
        this.IsRemoved = false;
        this.IsDeleted = false;
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

        string sql_str = "INSERT INTO [Comments] " +
            "([Title], [Body], [CommentorID], [ParentPostID], [ParentCommentID], " +
            "[CreationDate]. [IsRemoved], [IsDeleted]) " +
            "VALUES ('{0}','{1}',{2}, {3}, {4}, #{5}#, {6}, {7}) ";

        sql_str = string.Format(sql_str, this.Title, this.Body, this.CommentorID, this.ParentPostID,
            this.ParentCommentID, this.CreationDate, this.IsRemoved, this.IsDeleted);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Comments] " +
            "SET [Title] = '{0}', [Body] = '{1}', [CommentorID] = {2}, " +
            "[ParentPostID] = {3}, [ParentCommentID] = {4}, [CreationDate] = #{5}#," +
            "[IsRemoved = {6}, [IsDeleted] = {7}";
        sql_str = string.Format(sql_str, this.Title, this.Body, this.CommentorID, this.ParentPostID,
            this.ParentCommentID, this.CreationDate, this.IsRemoved, this.IsDeleted);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Comment]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Comments] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }
    #endregion
}