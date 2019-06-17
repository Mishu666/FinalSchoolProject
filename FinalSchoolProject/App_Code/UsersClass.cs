using System;
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

    public int Flags, Points, ModeratedPagesCount;
    public string Username, Password, Bio;
    public DateTime DOB, CreationDate;
    public bool IsAdmin, IsMod, IsSuspended;

    protected UsersClass()
    {

    }

    protected UsersClass(int ID, int Flags, int Points, int ModeratedPagesCount, string Username, string Password,
                         string Bio, DateTime DOB, DateTime CreationDate, bool IsAdmin, bool IsMod, bool IsSuspended)
    {
        this.ID = ID;
        this.Flags = Flags;
        this.Points = Points;
        this.ModeratedPagesCount = ModeratedPagesCount;
        this.Username = Username;
        this.Password = Password;
        this.Bio = Bio;
        this.DOB = DOB;
        this.CreationDate = CreationDate;
        this.IsAdmin = IsAdmin;
        this.IsMod = IsMod;
        this.IsSuspended = IsSuspended;
    }

    public static UsersClass FromDataRow(DataRow dr)
    {
        if (dr == null) return null;
        UsersClass obj = new UsersClass
        {
            ID = Convert.ToInt32(dr["ID"]),
            Flags = Convert.ToInt32(dr["Flags"]),
            ModeratedPagesCount = Convert.ToInt32(dr["ModeratedPagesCount"]),
            Points = Convert.ToInt32(dr["Points"]),
            Username = dr["Username"].ToString(),
            Password = dr["Password"].ToString(),
            Bio = dr["Bio"].ToString(),
            DOB = Convert.ToDateTime(dr["DOB"]),
            CreationDate = Convert.ToDateTime(dr["CreationDate"]),
            IsAdmin = Convert.ToBoolean(dr["IsAdmin"]),
            IsMod = Convert.ToBoolean(dr["IsMod"]),
            IsSuspended = Convert.ToBoolean(dr["IsSuspended"]),
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
            IsMod = false,
            Flags = 0,
            Points = 0,
            ModeratedPagesCount = 0,
            Bio = "Hello fellow consultants and consultees!",
            CreationDate = DateTime.Now,
            IsSuspended = false,
        };
        user.Insert();
        return user;

    }

    protected void Insert()
    {
        if (ID != 0)
        {
            throw new Exception("user already inserted");
        }

        string sql_str = "INSERT INTO [Users] " +
            "([Username], [Password], [Flags], [Points], [ModeratedPagesCount] " +
            "[CreationDate], [DOB], [IsAdmin], [IsMod], [IsSuspended], [ProfilePictureDir], [Bio]) " +
            "VALUES ('{0}','{1}', {2}, {3}, {4}, #{5}#, #{6}#, {7}, {8}, {9}, '{10}') ";

        sql_str = string.Format(sql_str, this.Username, this.Password, this.Flags, this.Points,
            this.ModeratedPagesCount, this.CreationDate, this.DOB, this.IsAdmin, this.IsMod, this.IsSuspended, this.Bio);
        Dbase.ChangeTable(sql_str);

        string get_id = "SELECT @@IDENTITY AS ID";

        DataTable dt = Dbase.SelectFromTable(get_id);

        this.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
    }

    public void Update()
    {
        string sql_str = "UPDATE [Users] " +
            "SET [Username] = '{0}', [Password] = '{1}', " +
            "[Flags] = {2}, [Points]= {3}, [ModeratedPagesCount] = {4}, " +
            "[CreationDate] = #{5}#, [DOB] = #{6}#, " +
            "[IsAdmin] = {7}, [IsMod] = {8}, [IsSuspended] = {9}, " +
            "[Bio] = '{10}'";
        sql_str += " WHERE [ID]=" + this.ID;
        sql_str = string.Format(sql_str, this.Username, this.Password, this.Flags, this.Points, this.ModeratedPagesCount,
            this.CreationDate, this.DOB, this.IsAdmin, this.IsMod, this.IsSuspended, this.Bio);
        Dbase.ChangeTable(sql_str);
    }

    public void Delete()
    {
        string sql_str = "DELETE FROM [Users] WHERE [ID]=" + this.ID;
        Dbase.ChangeTable(sql_str);
    }

    public static DataTable GetAll()
    {
        string sql_str = "SELECT * FROM [Users]";
        return Dbase.SelectFromTable(sql_str);

    }

    public static DataTable GetAllExcept(int id)
    {
        string sql_str = "SELECT * FROM [Users] WHERE [ID]<>" + id;
        return Dbase.SelectFromTable(sql_str);

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

    public static bool UsernameTaken(string username)
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

    public bool IsSubscribedTo(int PageID)
    {
        string sql = "SELECT * FROM [Subscriptions] WHERE [PageID]=" + PageID + " AND [SubscriberID]=" + this.ID;
        DataTable dt = Dbase.SelectFromTable(sql);
        return dt.Rows.Count > 0;
    }

    public bool IsModeratorFor(int PageID)
    {
        if (!this.IsMod) return false;

        string sql = "SELECT * FROM [Moderators] WHERE [PageID]=" + PageID + " AND [ModeratorID]=" + this.ID;
        DataTable dt = Dbase.SelectFromTable(sql);
        return dt.Rows.Count > 0;
    }

    public DataTable GetMyModeratedPages()
    {
        string sql = "SELECT * FROM [ConsultPages] WHERE [ID] IN (SELECT [PageID] FROM [Moderators] WHERE [ModeratorID]=" + this.ID + ")";
        DataTable dt = Dbase.SelectFromTable(sql);
        return dt;
    }

    public DataTable GetMySubscibedPages()
    {

        string sql = "SELECT * FROM [ConsultPages] WHERE [ID] IN (SELECT [PageID] FROM [Subscriptions] WHERE [SubscriberID]=" + this.ID + ")";
        return Dbase.SelectFromTable(sql);

    }

    public DataTable GetPostsInSubscibedPages()
    {

        string sql = "SELECT * FROM [Posts] WHERE [ConsultPageID] IN (SELECT [PageID] FROM [Subscriptions] WHERE [SubscriberID]=" + this.ID + ")";
        return Dbase.SelectFromTable(sql);

    }

    public DataTable GetUserComments()
    {
        string sql = "SELECT * FROM [Comments] WHERE [CommentorID]=" + this.ID;
        return Dbase.SelectFromTable(sql);
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
        string sql = "SELECT * FROM [Posts] WHERE [ID] IN (SELECT [VotedPostID] FROM [PostVotes] WHERE [VoterID]=" + this.ID + " AND [VoteValue]=-1)";
        return Dbase.SelectFromTable(sql);

    }

    public DataTable GetMessages()
    {
        string sql = "SELECT Messages.*, Users.* FROM [Messages] INNER JOIN [Users] ON Messages.SenderID = Users.ID WHERE Messages.RecipientID=" + this.ID;

        return Dbase.SelectFromTable(sql);
    }

    public DataTable GetConversationWith(int UserID)
    {

        string sql_str = "SELECT [Messages.*], [Users.Username] FROM [Messages] WHERE ([SenderID] = {0} AND [RecipientID] = {1})" +
            " OR ([SenderID] = {1} AND [RecipientID] = {0}) INNER JOIN [Users] ON [Messages.SenderID]=[Users.ID] ORDER BY [SendDate]";

        sql_str = string.Format(sql_str, this.ID, UserID);

        return Dbase.SelectFromTable(sql_str);

    }

}