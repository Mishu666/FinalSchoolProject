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
    public int ID { get; protected set; }
    public int CommentorID, ParentPostID, UpvoteCount, DownvoteCount;
    public string Body;
    public DateTime CreationDate;
    public bool IsRemoved, IsDeleted, IsEdited;


    #region constructors

    protected CommentsClass()
    {

    }

    protected CommentsClass(int ID, string Body,
        int CommentorID, int ParentPostID,
        DateTime CreationDate, bool IsRemoved, bool IsDeleted, bool IsEdited)
    {
        this.ID = ID;
        this.Body = Body;
        this.CommentorID = CommentorID;
        this.ParentPostID = ParentPostID;
        this.CreationDate = CreationDate;
        this.IsRemoved = IsRemoved;
        this.IsDeleted = IsDeleted;
        this.IsEdited = IsEdited;
    }

    public static CommentsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        CommentsClass obj = new CommentsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            CommentorID = Convert.ToInt32(dr["CommentorID"]),
            ParentPostID = Convert.ToInt32(dr["ParentPostID"]),
            Body = dr["Body"].ToString(),
            CreationDate = Convert.ToDateTime(dr["CreationDate"]),
            IsRemoved = Convert.ToBoolean(dr["IsRemoved"]),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
            IsEdited = Convert.ToBoolean(dr["IsEdited"]),
            DownvoteCount = Convert.ToInt32(dr["DownvoteCount"]),
            UpvoteCount = Convert.ToInt32(dr["UpvoteCount"])

        };

        return obj;
    }

    public static CommentsClass CreateNew(string Body,
        int CommentorID, int ParentPostID)
    {
        CommentsClass comment = new CommentsClass
        {
            Body = Body,
            CommentorID = CommentorID,
            ParentPostID = ParentPostID,
            CreationDate = DateTime.Now,
            IsRemoved = false,
            IsDeleted = false,
            IsEdited = false
        };

        comment.Insert();
        return comment;
    }

    #endregion

    #region sql functions

    protected virtual void Insert()
    {
        string sql_str;
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }


        sql_str = "INSERT INTO [Comments] " +
            "([Body], [CommentorID], [ParentPostID], " +
            "[UpvoteCount], [DownvoteCount], [CreationDate], [IsRemoved], [IsDeleted], [IsEdited]) " +
            "VALUES ('{0}',{1}, {2}, {3}, {4}, #{5}#, {6}, {7}, {8}) ";
        sql_str = string.Format(sql_str, this.Body, this.CommentorID, this.ParentPostID,
            this.UpvoteCount, this.DownvoteCount, this.CreationDate, this.IsRemoved, this.IsDeleted, this.IsEdited);

        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public virtual void Update()
    {
        string sql_str;

        sql_str = "UPDATE [Comments] " +
            "SET [Body] = '{0}', [CommentorID] = {1}, [UpvoteCount] = {2}, [DownvoteCount] = {3}," +
            "[ParentPostID] = {4},[CreationDate] = #{5}#," +
            "[IsRemoved] = {6}, [IsDeleted] = {7}, [IsEdited] = {8}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.Body, this.CommentorID, this.UpvoteCount, this.DownvoteCount,
            this.ParentPostID, this.CreationDate, this.IsRemoved, this.IsDeleted, this.IsEdited);

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
        string sql_str = "SELECT * FROM [Comments]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetOriginalComments(int PostID)
    {
        string sql_str = "SELECT * FROM [Comments] WHERE [ParentCommentID] IS NULL AND [ParentPostID] = " + PostID;
        return Dbase.SelectFromTable(sql_str);

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
        string sql_str = "SELECT * FROM [Comments]";
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

    #region utility functions

    #endregion

}