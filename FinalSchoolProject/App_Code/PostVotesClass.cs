using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for PostVotesClass
/// </summary>
public class PostVotesClass
{
    public readonly int ID;
    public int VoterID, VotedPostID, VoteValue;

    #region constructors

    private PostVotesClass()
    {

    }

    private PostVotesClass(int ID, int VoterID,
        int VotedPostID, int VoteValue)
    {
        this.ID = ID;
        this.VoterID = VoterID;
        this.VotedPostID = VotedPostID;
        this.VoteValue = VoteValue;
    }

    private PostVotesClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.VoterID = Convert.ToInt32(dr["VoterID"]);
        this.VotedPostID = Convert.ToInt32(dr["VotedPostID"]);
        this.VoteValue = Convert.ToInt32(dr["VoteValue"]);
    }

    public PostVotesClass(int VoterID, int VotedPostID, int VoteValue)
    {
        this.VoterID = VoterID;
        this.VotedPostID = VotedPostID;
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

        string sql_str = "INSERT INTO [PostVotes] " +
            "([VoterID], [VotedPostID], [VoteValue]) " +
            "VALUES ({0}, {1}, {2}) ";

        sql_str = string.Format(sql_str, this.VoterID, this.VotedPostID, this.VoteValue);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        return Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "Update [PostVotes] " +
            "SET [VoterID] = {0}, [VotedPostID] = {1}, " +
            "[VoteValue] = {2}";
        sql_str = string.Format(sql_str, this.VoterID, this.VotedPostID, this.VoteValue);
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [PostVotes]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;
    }

    public static DataTable GetByProperty(string property, object val)
    {
        string surround = "";
        if (val is string) surround = "'";
        if (val is DateTime) surround = "#";

        string sql_str = "SELECT * FROM [PostVotes] WHERE [" + property + "] = {0}{1}{0}";
        DataTable user_dt = Dbase.SelectFromTable(surround, sql_str);
        return user_dt;
    }

    #endregion
}