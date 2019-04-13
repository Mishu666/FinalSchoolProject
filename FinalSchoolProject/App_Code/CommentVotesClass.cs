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
    public int ID { get; private set; }
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

    public static CommentVotesClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        CommentVotesClass obj = new CommentVotesClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            VoterID = Convert.ToInt32(dr["VoterID"]),
            VotedCommentID = Convert.ToInt32(dr["VotedCommentID"]),
            VoteValue = Convert.ToInt32(dr["VoteValue"])
        };
        return obj;
    }

    public static CommentVotesClass CreateNew(int VoterID, int VotedCommentID, int VoteValue)
    {
        CommentVotesClass commentvote = new CommentVotesClass
        {
            VoterID = VoterID,
            VotedCommentID = VotedCommentID,
            VoteValue = VoteValue
        };
        commentvote.Insert();
        return commentvote;
    }

    #endregion

    #region sql functions

    private void Insert()
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

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [CommentVotes] " +
            "SET [VoterID] = {0}, [VotedCommentID] = {1}, " +
            "[VoteValue] = {2}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.VoterID, this.VotedCommentID, this.VoteValue);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [CommentVotes] WHERE [ID]=" + this.ID;
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

    public static CommentVotesClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [CommentVotes]";
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
}