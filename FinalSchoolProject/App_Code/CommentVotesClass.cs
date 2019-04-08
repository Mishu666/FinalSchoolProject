using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CommentVotesClass
/// </summary>
public class CommentVotesClass
{
    public readonly int ID;
    public int VoterID, VotedCommentID, VoteValue;

    #region constructors

    private CommentVotesClass()
    {

    }

    private CommentVotesClass(int ID, int VoterID,
        int VotedCommentID, int VoteValue)
    {
        this.ID = ID;
        this.VoterID = VoterID;
        this.VotedCommentID = VotedCommentID;
        this.VoteValue = VoteValue;
    }

    private CommentVotesClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.VoterID = Convert.ToInt32(dr["VoterID"]);
        this.VotedCommentID = Convert.ToInt32(dr["VotedCommentID"]);
        this.VoteValue = Convert.ToInt32(dr["VoteValue"]);
    }

    public CommentVotesClass(int VoterID, int VotedCommentID, int VoteValue)
    {
        this.VoterID = VoterID;
        this.VotedCommentID = VotedCommentID;
        this.VoteValue = VoteValue;
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

        string sql_str = "INSERT INTO [CommentVotes] " +
            "([VoterID], [VotedCommentID], [VoteValue]) " +
            "VALUES ({0}, {1}, {2}) ";

        sql_str = string.Format(sql_str, this.VoterID, this.VotedCommentID, this.VoteValue);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [CommentVotes] " +
            "SET [VoterID] = {0}, [VotedCommentID] = {1}, " +
            "[VoteValue] = {2}";
        sql_str = string.Format(sql_str, this.VoterID, this.VotedCommentID, this.VoteValue);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [CommentVotes]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [CommentVotes] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}