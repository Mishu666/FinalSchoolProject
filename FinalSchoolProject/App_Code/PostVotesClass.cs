﻿using System;
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

    private PostVotesClass(DataRow dr)
    {
        this.ID = Convert.ToInt32(dr["ID"]);
        this.VoterID = Convert.ToInt32(dr["VoterID"]);
        this.VotedPostID = Convert.ToInt32(dr["VotedPostID"]);
        this.VoteValue = Convert.ToInt32(dr["VoteValue"]);
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

    public static DataTable GetByProperty(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [PostVotes]";
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