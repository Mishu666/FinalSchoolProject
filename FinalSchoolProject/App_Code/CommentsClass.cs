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
    public int ID { get; private set; }
    public int CommentorID, ParentPostID, ParentCommentID, UpvoteCount, DownvoteCount;
    public string Title, Body;
    public string CreationDate;
    public bool IsRemoved, IsDeleted;


    #region constructors

    private CommentsClass()
    {

    }

    private CommentsClass(int ID, string Title, string Body,
        int CommentorID, int ParentPostID, int ParentCommentID,
        string CreationDate, bool IsRemoved, bool IsDeleted)
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

    public static CommentsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        CommentsClass obj = new CommentsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            CommentorID = Convert.ToInt32(dr["CommentorID"]),
            ParentPostID = Convert.ToInt32(dr["ParentPostID"]),
            ParentCommentID = Convert.ToInt32(dr["ParentCommentID"]),
            Title = dr["Title"].ToString(),
            Body = dr["Body"].ToString(),
            CreationDate = Convert.ToString(dr["CreationDate"]),
            IsRemoved = Convert.ToBoolean(dr["IsRemoved"]),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"])
        };
        return obj;
    }

    public static CommentsClass CreateNew(string Title, string Body,
        int CommentorID, int ParentPostID, int ParentCommentID)
    {
        CommentsClass comment = new CommentsClass
        {
            Title = Title,
            Body = Body,
            CommentorID = CommentorID,
            ParentPostID = ParentPostID,
            ParentCommentID = ParentCommentID,
            CreationDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            IsRemoved = false,
            IsDeleted = false
        };

        comment.Insert();
        return comment;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Comments] " +
            "([Title], [Body], [CommentorID], [ParentPostID], [ParentCommentID], " +
            "[UpvoteCount], [DownvoteCount], [CreationDate], [IsRemoved], [IsDeleted]) " +
            "VALUES ('{0}','{1}',{2}, {3}, {4}, {5} {6}, '{7}', {8}, {9}) ";

        sql_str = string.Format(sql_str, this.Title, this.Body, this.CommentorID, this.ParentPostID,
            this.ParentCommentID, this.UpvoteCount, this.DownvoteCount, this.CreationDate, this.IsRemoved, this.IsDeleted);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT last_insert_rowid() AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Comments] " +
            "SET [Title] = '{0}', [Body] = '{1}', [CommentorID] = {2}, [UpvoteCount] = {3}, [DownvoteCount] = {4}," +
            "[ParentPostID] = {5}, [ParentCommentID] = {6}, [CreationDate] = '{7}'," +
            "[IsRemoved = {8}, [IsDeleted] = {9}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.Title, this.Body, this.CommentorID, this.UpvoteCount, this.DownvoteCount,
            this.ParentPostID, this.ParentCommentID, this.CreationDate, this.IsRemoved, this.IsDeleted);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Comments] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Comment]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }
    public static CommentsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Comment]";
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
                prepend = "date('";
                append = "')";
            }
            sql_str += "[{0}] = {1}{2}{3}";
            if (i < pairs.Length - 1) sql_str += " AND ";

            sql_str = string.Format(sql_str, pairs[i].Key, prepend, pairs[i].Value, append);

        }
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt;
    }
    #endregion

    #region utility functions

    public static DataTable GetCommentHeirarchy(int ID)
    {
        string sql = @"With CommentsAndParents (
                        ID,
	                    CommentText,
	                    ParentID,
	                    ParentCommentText,
	                    Depth
                    )
                    AS(
                    SELECT  ID,
                            Title,
                            ParentCommentID,
                            Title,
                            0
                    FROM Comments
                    WHERE ParentCommentID IS NULL AND ID = " + ID + @"

                    UNION ALL

                    SELECT Comments.ID,
                            Comments.Title,
                            Comments.ParentCommentID,
                            CommentsAndParents.CommentText,
                            CommentsAndParents.Depth + 1
                    FROM  Comments JOIN CommentsAndParents ON CommentsAndParents.ID = Comments.ParentCommentID
                    )

                    SELECT * FROM CommentsAndParents";

        DataTable dt = Dbase.SelectFromTable(sql);
        return dt;
    }

    #endregion

}