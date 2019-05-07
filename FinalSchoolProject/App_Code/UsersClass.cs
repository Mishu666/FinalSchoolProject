﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UsersClass
/// </summary>
public class UsersClass
{
    public int ID { get; protected set; }

    public int Flags, Points, MyFollowersCount, FollowingCount;
    public string Username, Password, ProfilePictureDir, Bio;
    DateTime DOB, CreationDate;
    public bool IsAdmin, IsSuspended, IsPrivate, IsDeleted;

    #region constructors

    protected UsersClass()
    {

    }

    protected UsersClass(int ID, int MyFollowersCount, int FollowingCount, int Flags, int Points,
        string Username, string Password, string ProfilePictureDir, string Bio,
        DateTime DOB, DateTime CreationDate,
        bool IsAdmin, bool IsSuspended, bool IsPrivate, bool IsDeleted)
    {
        this.ID = ID;
        this.MyFollowersCount = MyFollowersCount;
        this.FollowingCount = FollowingCount;
        this.Flags = Flags;
        this.Points = Points;
        this.Username = Username;
        this.Password = Password;
        this.Bio = Bio;
        this.DOB = DOB;
        this.CreationDate = CreationDate;
        this.IsAdmin = IsAdmin;
        this.IsSuspended = IsSuspended;
        this.IsPrivate = IsPrivate;
        this.IsDeleted = IsDeleted;
        this.ProfilePictureDir = ProfilePictureDir;
    }

    public static UsersClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        UsersClass obj = new UsersClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            MyFollowersCount = Convert.ToInt32(dr["MyFollowersCount"]),
            FollowingCount = Convert.ToInt32(dr["FollowingCount"]),
            Flags = Convert.ToInt32(dr["Flags"]),
            Points = Convert.ToInt32(dr["Points"]),
            Username = dr["Username"].ToString(),
            Password = dr["Password"].ToString(),
            Bio = dr["Bio"].ToString(),
            ProfilePictureDir = dr["ProfilePictureDir"].ToString(),
            DOB = Convert.ToDateTime(dr["DOB"]),
            CreationDate = Convert.ToDateTime(dr["CreationDate"]),
            IsAdmin = Convert.ToBoolean(dr["IsAdmin"]),
            IsSuspended = Convert.ToBoolean(dr["IsSuspended"]),
            IsPrivate = Convert.ToBoolean(dr["IsPrivate"]),
            IsDeleted = Convert.ToBoolean(dr["IsDeleted"])
        };
        return obj;
    }

    public static UsersClass CreateNew(string Username, string Password, DateTime DOB)
    {
        UsersClass user = new UsersClass
        {
            Username = Username,
            Password = Password,
            DOB = DOB,
            IsAdmin = false,
            MyFollowersCount = 0,
            FollowingCount = 0,
            Flags = 0,
            Points = 0,
            Bio = "Hello fellow consultants and consultees!",
            ProfilePictureDir = "",
            CreationDate = DateTime.Now,
            IsSuspended = false,
            IsPrivate = false,
            IsDeleted = false
        };
        user.Insert();
        return user;

    }

    #endregion

    #region sql functions

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("user already inserted");
        }

        string sql_str = "INSERT INTO [Users] " +
            "([Username], [Password], [MyFollowersCount], [FollowingCount], [Flags], [Points], " +
            "[CreationDate], [DOB], [IsAdmin], [IsSuspended], [IsPrivate], [IsDeleted], [ProfilePictureDir]. [Bio]) " +
            "VALUES ('{0}','{1}', {2}, {3}, {4}, {5}, #{6}#, #{7}#, {8}, {9}, {10}, {11}, '{12}', '{13}') ";

        sql_str = string.Format(sql_str, this.Username, this.Password, this.MyFollowersCount, this.FollowingCount, this.Flags, this.Points,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted, this.ProfilePictureDir, this.Bio);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Users] " +
            "SET [Username] = '{0}', [Password] = '{1}', " +
            "[MyFollowersCount] = {2}, [FollowingCount] = {3} , [Flags] = {4}, [Points]= {5}, " +
            "[CreationDate] = #{6}#, [DOB] = #{7}#, [IsAdmin] = {8}, [IsSuspended] = {9}, " +
            "[IsPrivate] = {10}, [IsDeleted] = {11}, [ProfilePictureDir] = '{12}', [Bio] = '{13}'";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.Username, this.Password, this.MyFollowersCount, this.FollowingCount, this.Flags, this.Points,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsSuspended, this.IsPrivate, this.IsDeleted, this.ProfilePictureDir, this.Bio);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Users] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }

    #endregion

    #region select functions

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Users]";
        DataTable all = Dbase.SelectFromTable(sql_str);

        return all;

    }

    public static UsersClass GetByID(int ID)
    {
        KeyValuePair<string, object> id_pair = new KeyValuePair<string, object>("ID", ID);
        DataTable obj = GetByProperties(id_pair);
        if (obj == null || obj.Rows.Count == 0) return null;
        else return FromDataRow(obj.Rows[0]);
    }

    public static DataTable GetByProperties(params KeyValuePair<string, object>[] pairs)
    {
        string sql_str = "SELECT * FROM [Users]";
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

    public static UsersClass GetByCredentials(string username, string password)
    {
        DataTable user_dt = GetByProperties(
            new KeyValuePair<string, object>("Username", username),
            new KeyValuePair<string, object>("Password", password)
            );

        if (user_dt == null || user_dt.Rows.Count == 0) return null;
        return FromDataRow(user_dt.Rows[0]);
    }

    #endregion

    #region utility functions

    public static bool UserNameTaken(string username)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [Username]='{0}'";
        sql_str = string.Format(sql_str, username);
        DataTable user_dt = Dbase.SelectFromTable(sql_str);
        return user_dt.Rows.Count > 0;
    }

    public static bool UserExists(string username, string password)
    {
        UsersClass user = GetByCredentials(username, password);
        return user != null;

    }

    public DataTable GetUserPosts()
    {
        string sql = "SELECT * FROM [Posts] WHERE [AuthorID]=" + this.ID;
        return Dbase.SelectFromTable(sql);
    }

    public DataTable GetUserSavedPosts()
    {
        string sql = "SELECT * FROM [Posts] WHERE [ID] IN (SELECT [SavedPostID] FROM [SavedPosts]  WHERE [SaverID]=" + this.ID + ")";
        return Dbase.SelectFromTable(sql);

    }

    public DataTable GetUserUpvotedPosts()
    {
        string sql = "SELECT * FROM [Posts] WHERE [ID] IN (SELECT [VotedPostID] FROM [PostVotes]  WHERE [VoterID]=" + this.ID + " AND [VoteValue]=1)";
        return Dbase.SelectFromTable(sql);

    }

    public DataTable GetUserDownvotedPosts()
    {
        string sql = "SELECT * FROM [Posts] WHERE [ID] IN (SELECT [VotedPostID] FROM [PostVotes]  WHERE [VoterID]=" + this.ID + " AND [VoteValue]=-1)";
        return Dbase.SelectFromTable(sql);

    }
    #endregion
}