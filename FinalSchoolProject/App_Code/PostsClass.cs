﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PostsClass
/// </summary>
public class PostsClass
{
    public int ID { get; protected set; }
    public int ConsultPageID, AuthorID, UpvoteCount, DownvoteCount, CommentCount;
    public string Title, Body;
    public bool IsRemoved, IsDeleted, IsEdited;
    public DateTime CreationDate;


    #region constructors

    protected PostsClass()
    {

    }

    protected PostsClass(int ID, int ConsultPageID, int AuthorID, int UpvoteCount, int DownvoteCount, int CommentCount,
        string Title, string Body, bool IsRemoved, bool IsDeleted, bool IsEdited, DateTime CreationDate)
    {
        this.ID = ID;
        this.ConsultPageID = ConsultPageID;
        this.AuthorID = AuthorID;
        this.UpvoteCount = UpvoteCount;
        this.DownvoteCount = DownvoteCount;
        this.CommentCount = CommentCount;
        this.Title = Title;
        this.Body = Body;
        this.IsRemoved = IsRemoved;
        this.IsDeleted = IsDeleted;
        this.IsEdited = IsEdited;
        this.CreationDate = CreationDate;
    }

    public static PostsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        PostsClass obj = new PostsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            ConsultPageID = Convert.ToInt32(dr["ConsultPageID"]),
            UpvoteCount = Convert.ToInt32(dr["UpvoteCount"]),
            DownvoteCount = Convert.ToInt32(dr["DownvoteCount"]),
            CommentCount = Convert.ToInt32(dr["CommentCount"]),
            Title = dr["Title"].ToString(),
            Body = dr["Body"].ToString(),
            IsRemoved = Convert.ToBoolean(dr["IsRemoved"]),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
            IsEdited = Convert.ToBoolean(dr["IsEdited"]),
            CreationDate = Convert.ToDateTime(dr["CreationDate"])
        };


        obj.AuthorID = Convert.IsDBNull(dr["AuthorID"]) ? 0 : Convert.ToInt32(dr["AuthorID"]);

        return obj;
    }

    public static PostsClass CreateNew(int ConsultPageID, int AuthorID, string Title, string Body)
    {
        PostsClass post = new PostsClass
        {
            ConsultPageID = ConsultPageID,
            AuthorID = AuthorID,
            UpvoteCount = 0,
            DownvoteCount = 0,
            CommentCount = 0,
            Title = Title,
            Body = Body,
            IsDeleted = false,
            IsEdited = false,
            IsRemoved = false,
            CreationDate = DateTime.Now
        };
        post.Insert();
        return post;
    }

    #endregion

    #region sql functions

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string AuthorID_str = this.AuthorID == 0 ? "Null" : this.AuthorID.ToString();

        string sql_str = "INSERT INTO [Posts] " +
            "([ConsultPageID], [AuthorID], [UpvoteCount], [DownvoteCount], [CommentCount], [Title], [Body], " +
            "[IsRemoved],[IsDeleted],[IsEdited], [CreationDate]) " +
            "VALUES ({0}, {1}, {2}, {3}, {4}, '{5}', '{6}', {7}, {8},{9}, #{10}#) ";

        sql_str = string.Format(sql_str, this.ConsultPageID, AuthorID_str,
            this.UpvoteCount, this.DownvoteCount, this.CommentCount, this.Title, this.Body,
            this.IsRemoved, this.IsDeleted, this.IsEdited, this.CreationDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {

        string AuthorID_str = this.AuthorID == 0 ? "Null" : this.AuthorID.ToString();

        string sql_str = "UPDATE [Posts] " +
            "SET [ConsultPageID] = {0}, [AuthorID] = {1}, " +
            "[UpvoteCount] = {2}, [DownvoteCount] = {3}, [CommentCount] = {4}, [Title] = '{5}', [Body] = '{6}', " +
            " [IsRemoved] = {7}, [IsDeleted]={8},[IsEdited] = {9}, [CreationDate] = #{10}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.ConsultPageID, AuthorID_str,
            this.UpvoteCount, this.DownvoteCount, this.CommentCount, this.Title, this.Body,
            this.IsRemoved, this.IsDeleted, this.IsEdited, this.CreationDate);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Posts] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }
    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Posts]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static PostsClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }

    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Posts]";
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

    public static string GetAuthorUsername(int ID)
    {
        PostsClass post = GetByID(ID);
        UsersClass author = UsersClass.GetByID(post.AuthorID);
        return author.Username;
    }

    public static string GetPageName(int ID)
    {

        PostsClass post = GetByID(ID);
        ConsultPagesClass page = ConsultPagesClass.GetByID(post.ConsultPageID);
        return page.PageName;
    }

    public DataTable GetPostComments()
    {
        DataTable comments = CommentsClass.GetByProperties(
            new KeyValuePair<string, object>("ParentPostID", this.ID)
            );
        return comments;
    }

    #endregion
}