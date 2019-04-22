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
    public int ID { get; private set; }
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

    public static PostVotesClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        PostVotesClass obj = new PostVotesClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            VoterID = Convert.ToInt32(dr["VoterID"]),
            VotedPostID = Convert.ToInt32(dr["VotedPostID"]),
            VoteValue = Convert.ToInt32(dr["VoteValue"])
        };
        return obj;
    }

    public static PostVotesClass CreateNew(int VoterID, int VotedPostID, int VoteValue)
    {
        PostVotesClass postvote = new PostVotesClass
        {

            VoterID = VoterID,
            VotedPostID = VotedPostID,
            VoteValue = VoteValue
        };
        postvote.Insert();
        return postvote;
    }

    #endregion

    #region sql functions

    private void Insert()
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

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [PostVotes] " +
            "SET [VoterID] = {0}, [VotedPostID] = {1}, " +
            "[VoteValue] = {2}";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.VoterID, this.VotedPostID, this.VoteValue);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [PostVotes] WHERE [ID]=" + this.ID;
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

    public static PostVotesClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }
    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [PostVotes]";
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
}