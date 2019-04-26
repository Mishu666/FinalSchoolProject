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
    public List<string> ValidateAndLogin(string username, string password)
    {

        List<string> warnings = new List<string>();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(username))
        {
            warnings.Add("username cannot be empty");
            valid = false;
        }
        if(string.IsNullOrWhiteSpace(password))
        {
            warnings.Add("password cannot be empty");
            valid = false;
        }
        if(valid)
        {
            if(!UsersClass.UserExists(username, password))
            {
                warnings.Add("username and password do not match");
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
    public List<string> ValidateAndSignup(string username, string password, string pass_confirm, string DOBstr)
    {
        List<string> warnings = new List<string>();
        bool valid = true;
        DateTime DOB;
        bool DOB_parsed = DateTime.TryParseExact(DOBstr, "dd/MM/yyyy", null, DateTimeStyles.None, out DOB);

        if (string.IsNullOrWhiteSpace(username))
        {
            warnings.Add("username cannot be empty");
            valid = false;
        }
        else
        {
            if(UsersClass.UserNameTaken(username))
            {
                warnings.Add("username taken");
                valid = false;
            }
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            warnings.Add("password cannot be empty");
            valid = false;
        }
        else
        {
            if (!password.Equals(pass_confirm))
            {
                warnings.Add("passwords do not match");
                valid = false;
            }
        }
        if (string.IsNullOrWhiteSpace(DOB.ToString()))
        {
            warnings.Add("birth date cannot be empty");
            valid = false;
        }

        if (valid)
        {
            if (!DOB_parsed || DOB.CompareTo(DateTime.Today) > 0)
            {
                warnings.Add("invalid date of birth");
                valid = false;
            }
            else
            {
                SignUpUser(username, password, DOB);
            }
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

        if(vote_dt == null || vote_dt.Rows.Count == 0)
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
            post.UpvoteCount   -= 1;
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
    public void ReportPost(int PostID, string reportBody)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);



    }

}
