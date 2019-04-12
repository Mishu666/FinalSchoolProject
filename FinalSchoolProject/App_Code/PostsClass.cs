using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PostsClass
/// </summary>
public class PostsClass
{
    public int ID { get; private set; }
    public int ConsultPageID, AuthorID, VoteCount, CommentCount;
    public string Title, Body;
    public bool IsDeleted, IsRemoved;
    public DateTime CreationDate;


    #region constructors

    private PostsClass()
    {

    }

    private PostsClass(int ID, int ConsultPageID, int AuthorID, int VoteCount, int CommentCount,
        string Title, string Body, bool IsDeleted, bool IsRemoved, DateTime CreationDate)
    {
        this.ID = ID;
        this.ConsultPageID = ConsultPageID;
        this.AuthorID = AuthorID;
        this.VoteCount = VoteCount;
        this.CommentCount = CommentCount;
        this.Title = Title;
        this.Body = Body;
        this.IsDeleted = IsDeleted;
        this.IsRemoved = IsRemoved;
        this.CreationDate = CreationDate;
    }

    public static PostsClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        PostsClass obj = new PostsClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            ConsultPageID = Convert.ToInt32(dr["ConsultPageID"]),
            AuthorID = Convert.ToInt32(dr["AuthorID"]),
            VoteCount = Convert.ToInt32(dr["VoteCount"]),
            CommentCount = Convert.ToInt32(dr["CommentCount"]),
            Title = dr["Title"].ToString(),
            Body = dr["Body"].ToString(),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"]),
            IsRemoved = Convert.ToBoolean(dr["IsRemoved"]),
            CreationDate = Convert.ToDateTime(dr["CreationDate"])
        };
        return obj;
    }

    public static PostsClass CreateNew(int ConsultPageID, int AuthorID, string Title, string Body)
    {
        PostsClass post = new PostsClass
        {
            ConsultPageID = ConsultPageID,
            AuthorID = AuthorID,
            VoteCount = 0,
            CommentCount = 0,
            Title = Title,
            Body = Body,
            IsDeleted = false,
            IsRemoved = false,
            CreationDate = DateTime.Now
        };
        post.Insert();
        return post;
    }

    #endregion

    #region sql functions

    private void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("already inserted");
        }

        string sql_str = "INSERT INTO [Posts] " +
            "([ConsultPageID], [AuthorID], [VoteCount], [CommentCount], [Title], [Body], " +
            "[IsDeleted], [IsRemoved], [CreationDate]) " +
            "VALUES ({0}, {1}, {2}, {3}, '{4}', '{5}', {6}, {7}, #{8}#) ";

        sql_str = string.Format(sql_str, this.ConsultPageID, this.AuthorID,
            this.VoteCount, this.CommentCount, this.Title, this.Body, this.IsDeleted,
            this.IsRemoved, this.CreationDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Posts] " +
            "SET [ConsultPageID] = {0}, [AuthorID] = {1}, " +
            "[VoteCount] = {2}, [CommentCount] = {3}, [Title] = '{4}', [Body] = '{5}', " +
            "[IsDeleted] = {6}, [IsRemoved] = {7}, [CreationDate] = #{8}#";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.ConsultPageID, this.AuthorID,
            this.VoteCount, this.CommentCount, this.Title, this.Body, this.IsDeleted,
            this.IsRemoved, this.CreationDate);
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

    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Posts]";
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

    #region utility functions
    
    
    #endregion
}