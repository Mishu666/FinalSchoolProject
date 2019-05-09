using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;
using System.Globalization;

/// <summary>
/// Summary description for UsersService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class UsersService : System.Web.Services.WebService
{

    [WebMethod(EnableSession = true)]
    public void LogOutUser()
    {
        Session["Logged"] = false;
        Session["CurrentUserID"] = null;
        Session["CurrentUserIsAdmin"] = null;

    }

    [WebMethod(EnableSession = true)]
    private void LogInUser(string username, string password)
    {
        UsersClass user = UsersClass.GetByCredentials(username, password);
        Session["Logged"] = true;
        Session["CurrentUserID"] = user.ID;
        Session["CurrentUserIsAdmin"] = user.IsAdmin;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void SignUpUser(string username, string password, DateTime DOB)
    {
        UsersClass.CreateNew(username, password, DOB);
        LogInUser(username, password);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndLogin(string username, string password)
    {

        List<Warning> warnings = new List<Warning>();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(username))
        {
            warnings.Add(new Warning("loginInputUsername", "username cannot be empty"));
            valid = false;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            warnings.Add(new Warning("loginInputPassword", "password cannot be empty"));
            valid = false;
        }
        if (valid)
        {
            if (!UsersClass.UserExists(username, password))
            {
                Warning w = new Warning();
                w.Text = "username and password do not match";
                w.WarnControls.Add("loginInputUsername");
                w.WarnControls.Add("loginInputPassword");
                warnings.Add(w);
                valid = false;
            }
            else
            {
                LogInUser(username, password);
            }
        }

        return warnings;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndSignup(string username, string password, string pass_confirm, string DOBstr)
    {
        List<Warning> warnings = new List<Warning>();
        bool valid = true;
        DateTime DOB;
        bool DOB_parsed = DateTime.TryParseExact(DOBstr, "dd/MM/yyyy", null, DateTimeStyles.None, out DOB);

        if (string.IsNullOrWhiteSpace(username))
        {
            warnings.Add(new Warning("signupInputUsername", "username cannot be empty"));
            valid = false;
        }
        else
        {
            if (UsersClass.UserNameTaken(username))
            {
                warnings.Add(new Warning("signupInputUsername", "username taken"));
                valid = false;
            }
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            warnings.Add(new Warning("signupInputPassword", "password cannot be empty"));
            valid = false;
        }
        else
        {
            if (!password.Equals(pass_confirm))
            {
                Warning w = new Warning();
                w.Text = "passwords do not match";
                w.WarnControls.Add("signupInputPassword");
                w.WarnControls.Add("signupInputPasswordConfirm");
                warnings.Add(w);
                valid = false;
            }
        }
        if (string.IsNullOrWhiteSpace(DOBstr))
        {
            warnings.Add(new Warning("signupInputDOB", "birth date cannot be empty"));
            valid = false;
        }

        if (!DOB_parsed || DOB > DateTime.Today)
        {
            warnings.Add(new Warning("signupInputDOB", "invalid date of birth"));
            valid = false;
        }

        if (valid)
        {
            SignUpUser(username, password, DOB);

        }

        return warnings;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UsernameTaken(string username)
    {
        return UsersClass.UserNameTaken(username);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UserExists(string username, string password)
    {
        return UsersClass.UserExists(username, password);

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UserLoggedIn()
    {
        if (Session["Logged"] != null && (bool)Session["Logged"] == true) return true;
        return false;
    }

    #region voting

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Upvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Status = "invalid login information";
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        PostsClass post = PostsClass.GetByID(PostID);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, 1);
            post.UpvoteCount += 1;
            post.Update();
            return post.UpvoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);


        if (vote.VoteValue == 1)
        {
            vote.Delete();
            post.UpvoteCount -= 1;
            post.Update();

        }
        else if (vote.VoteValue == -1)
        {
            vote.VoteValue = 1;
            vote.Update();
            post.UpvoteCount += 1;
            post.DownvoteCount -= 1;
            post.Update();

        }

        return post.UpvoteCount;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Downvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        PostsClass post = PostsClass.GetByID(PostID);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, -1);
            post.DownvoteCount += 1;
            post.Update();
            return post.DownvoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == -1)
        {
            vote.Delete();
            post.DownvoteCount -= 1;
            post.Update();

        }
        else if (vote.VoteValue == 1)
        {
            vote.VoteValue = -1;
            vote.Update();
            post.DownvoteCount += 1;
            post.UpvoteCount -= 1;
            post.Update();

        }
        return post.DownvoteCount;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int CommentUpvote(int CommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.Status = "invalid login information";
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        CommentsClass comment = CommentsClass.GetByID(CommentID);

        DataTable vote_dt = CommentVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedCommentID", CommentID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            CommentVotesClass.CreateNew(userID, CommentID, 1);
            comment.UpvoteCount += 1;
            comment.Update();
            return comment.UpvoteCount;
        }

        CommentVotesClass vote = CommentVotesClass.FromDataRow(vote_dt.Rows[0]);


        if (vote.VoteValue == 1)
        {
            vote.Delete();
            comment.UpvoteCount -= 1;
            comment.Update();

        }
        else if (vote.VoteValue == -1)
        {
            vote.VoteValue = 1;
            vote.Update();
            comment.UpvoteCount += 1;
            comment.DownvoteCount -= 1;
            comment.Update();

        }

        return comment.UpvoteCount;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int CommentDownvote(int CommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        CommentsClass comment = CommentsClass.GetByID(CommentID);

        DataTable vote_dt = CommentVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedCommentID", CommentID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            CommentVotesClass.CreateNew(userID, CommentID, -1);
            comment.DownvoteCount += 1;
            comment.Update();
            return comment.DownvoteCount;
        }

        CommentVotesClass vote = CommentVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == -1)
        {
            vote.Delete();
            comment.DownvoteCount -= 1;
            comment.Update();

        }
        else if (vote.VoteValue == 1)
        {
            vote.VoteValue = -1;
            vote.Update();
            comment.DownvoteCount += 1;
            comment.UpvoteCount -= 1;
            comment.Update();

        }
        return comment.DownvoteCount;

    }

    #endregion

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void ReportPost(int PostID, string reportBody)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        //CONTINUE

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreatePost(string title, string Body, int PageID)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        PostsClass.CreateNew(PageID, userID, title, Body);

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreatePost(string title, string Body, int PageID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        List<Warning> warnings = new List<Warning>();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(title))
        {
            warnings.Add(new Warning("addPostTitle", "Title cannot be empty"));
            valid = false;
        }
        if (string.IsNullOrWhiteSpace(Body))
        {
            warnings.Add(new Warning("addPostBody", "Post cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            CreatePost(title, Body, PageID);
        }

        return warnings;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreateNewComment(string Body, int ParentPostID)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        CommentsClass.CreateNew(Body, userID, ParentPostID);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreateCommentReply(string Body, int ParentCommentID)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        CommentRepliesClass.CreateNew(Body, userID, ParentCommentID);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreateComment(string Body, int ParentPostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        List<Warning> warnings = new List<Warning>();
        PostsClass post = PostsClass.GetByID(ParentPostID);
        bool valid = true;

        if (post == null)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(Body))
        {
            warnings.Add(new Warning("addCommentBody", "Comment cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            CreateNewComment(Body, ParentPostID);
        }

        return warnings;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreateCommentReply(string Body, int ParentCommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        List<Warning> warnings = new List<Warning>();
        CommentsClass comment = CommentsClass.GetByID(ParentCommentID);
        bool valid = true;

        if (comment == null)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
            valid = false;

        }

        if (string.IsNullOrWhiteSpace(Body))
        {
            warnings.Add(new Warning("addCommentBody", "Comment cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            CreateCommentReply(Body, ParentCommentID);
        }

        return warnings;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void UpdateUserInfo(string Username, string Bio, bool IsPrivate)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass user = UsersClass.GetByID(userID);

        user.Username = Username;
        user.Bio = Bio;
        user.IsPrivate = IsPrivate;
        user.Update();

    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndUpdateUserInfo(string Username, string Bio, bool IsPrivate, string PasswordConfirm)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        List<Warning> warnings = new List<Warning>();
        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass user = UsersClass.GetByID(userID);
        bool valid = true;


        if (string.IsNullOrWhiteSpace(Username))
        {
            warnings.Add(new Warning("EditUsernameInput", "Username cannot be empty"));
            valid = false;
        }
        else
        {
            if (UsersClass.UserNameTaken(Username) && Username != user.Username)
            {
                warnings.Add(new Warning("EditUsernameInput", "username taken"));
                valid = false;
            }
        }

        if (string.IsNullOrWhiteSpace(Bio))
        {
            warnings.Add(new Warning("EditBioInput", "Bio cannot be empty"));
            valid = false;
        }

        if (string.IsNullOrWhiteSpace(PasswordConfirm))
        {

            warnings.Add(new Warning("EditConfirmPasswordInput", "Enter password to confirm changes"));
            valid = false;
        }
        else
        {
            if (user.Password != PasswordConfirm)
            {
                warnings.Add(new Warning("EditConfirmPasswordInput", "Wrong Password"));
                valid = false;
            }
        }

        if (valid)
        {
            UpdateUserInfo(Username, Bio, IsPrivate);
        }

        return warnings;

    }
}
