using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CommentRepliesClass
/// </summary>
public class CommentRepliesClass : CommentsClass
{
    public int ParentCommentID;

    #region constructors

    protected CommentRepliesClass()
    {

    }

    protected CommentRepliesClass(int ID, string Body,
        int CommentorID, int ParentPostID, int ParentCommentID,
        DateTime CreationDate, bool IsRemoved, bool IsDeleted) : base(ID, Body, CommentorID, ParentPostID, CreationDate, IsRemoved, IsDeleted)
    {
        this.ParentCommentID = ParentCommentID;
    }

    public new static CommentRepliesClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        CommentRepliesClass obj = new CommentRepliesClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            CommentorID = Convert.ToInt32(dr["CommentorID"]),
            ParentPostID = Convert.ToInt32(dr["ParentPostID"]),
            Body = dr["Body"].ToString(),
            CreationDate = Convert.ToDateTime(dr["CreationDate"]),
            IsRemoved = Convert.ToBoolean(dr["IsRemoved"]),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
            DownvoteCount = Convert.ToInt32(dr["DownvoteCount"]),
            UpvoteCount = Convert.ToInt32(dr["UpvoteCount"])

        };

        if (dr["ParentCommentID"] is int) obj.ParentCommentID = Convert.ToInt32(dr["ParentCommentID"]);

        return obj;
    }

    public new static CommentRepliesClass CreateNew(string Body,
        int CommentorID, int ParentCommentID)
    {
        CommentsClass parent = CommentsClass.GetByID(ParentCommentID);

        CommentRepliesClass comment = new CommentRepliesClass
        {
            Body = Body,
            CommentorID = CommentorID,
            ParentPostID = parent.ParentPostID,
            ParentCommentID = ParentCommentID,
            CreationDate = DateTime.Now,
            IsRemoved = false,
            IsDeleted = false
        };

        comment.Insert();
        return comment;
    }

    #endregion

    #region sql functions

    protected override void Insert()
    {
        string sql_str;
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }


        sql_str = "INSERT INTO [Comments] " +
            "([Body], [CommentorID], [ParentPostID], [ParentCommentID], " +
            "[UpvoteCount], [DownvoteCount], [CreationDate], [IsRemoved], [IsDeleted]) " +
            "VALUES ('{0}',{1}, {2}, {3}, {4}, {5}, #{6}#, {7}, {8}) ";
        sql_str = string.Format(sql_str, this.Body, this.CommentorID, this.ParentPostID,
            this.ParentCommentID, this.UpvoteCount, this.DownvoteCount, this.CreationDate, this.IsRemoved, this.IsDeleted);


        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public override void Update()
    {
        string sql_str;
        sql_str = "UPDATE [Comments] " +
            "SET [Body] = '{0}', [CommentorID] = {1}, [UpvoteCount] = {2}, [DownvoteCount] = {3}," +
            "[ParentPostID] = {4}, [ParentCommentID] = {5}, [CreationDate] = #{6}#," +
            "[IsRemoved] = {7}, [IsDeleted] = {8}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.Body, this.CommentorID, this.UpvoteCount, this.DownvoteCount,
            this.ParentPostID, this.ParentCommentID, this.CreationDate, this.IsRemoved, this.IsDeleted);

        Dbase.ChangeTable(sql_str);
    }

    #endregion
}