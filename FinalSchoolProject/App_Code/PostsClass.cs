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
    public readonly int ID;
    public int ConsultPageID, AuthorID, VoteCount;
    public string Title, Body;
    public bool IsDeleted, IsRemoved;
    public DateTime CreationDate;


    #region constructors

    private PostsClass()
    {

    }

    private PostsClass(int ID, int ConsultPageID, int AuthorID, int VoteCount,
        string Title, string Body, bool IsDeleted, bool IsRemoved, DateTime CreationDate)
    {
        this.ID = ID;
        this.ConsultPageID = ConsultPageID;
        this.AuthorID = AuthorID;
        this.VoteCount = VoteCount;
        this.Title = Title;
        this.Body = Body;
        this.IsDeleted = IsDeleted;
        this.IsRemoved = IsRemoved;
        this.CreationDate = CreationDate;
    }

    private PostsClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.ConsultPageID = Convert.ToInt32(dr["ConsultPageID"]);
        this.AuthorID = Convert.ToInt32(dr["AuthorID"]);
        this.VoteCount = Convert.ToInt32(dr["VoteCount"]);
        this.Title = dr["Title"].ToString();
        this.Body = dr["Body"].ToString();
        this.IsDeleted = Convert.ToBoolean(dr["IsDeleted"]);
        this.IsRemoved = Convert.ToBoolean(dr["IsRemoved"]); ;
        this.CreationDate = Convert.ToDateTime(dr["CreationDate"]); ;
    }

    public PostsClass(int ConsultPageID, int AuthorID, string Title, string Body)
    {
        this.ConsultPageID = ConsultPageID;
        this.AuthorID = AuthorID;
        this.VoteCount = 0;
        this.Title = Title;
        this.Body = Body;
        this.IsDeleted = false;
        this.IsRemoved = false;
        this.CreationDate = DateTime.Now; ;
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

        string sql_str = "INSERT INTO [Posts] " +
            "([ConsultPageID], [AuthorID], [VoteCount], [Title], [Body], " +
            "[IsDeleted], [IsRemoved], [CreationDate]) " +
            "VALUES ({0}, {1}, {2},'{3}', '{4}', {5}, {6}, #{7}#) ";

        sql_str = string.Format(sql_str, this.ConsultPageID, this.AuthorID,
            this.VoteCount, this.Title, this.Body, this.IsDeleted,
            this.IsRemoved, this.CreationDate);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [Posts] " +
            "SET [ConsultPageID] = {0}, [AuthorID] = {1}, " +
            "[VoteCount] = {2}, [Title] = '{3}', [Body] = '{4}', " +
            "[IsDeleted] = {5}, [IsRemoved] = {6}, [CreationDate] = #{7}#";
        sql_str = string.Format(sql_str, this.ConsultPageID, this.AuthorID,
            this.VoteCount, this.Title, this.Body, this.IsDeleted,
            this.IsRemoved, this.CreationDate);
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

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [Posts] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}