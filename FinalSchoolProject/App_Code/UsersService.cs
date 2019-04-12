using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Services;
using System.Web.Script.Services;

/// <summary>
/// Summary description for UsersService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[ScriptService]
public class UsersService : System.Web.Services.WebService
{

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

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Upvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);

        DataTable post_dt = PostsClass.GetByProperties(new KeyValuePair<string, object>("ID", PostID));
        PostsClass post = PostsClass.FromDataRow(post_dt.Rows[0]);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, 1);
            post.VoteCount += 1;
            post.Update();
            return post.VoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);

        
        if (vote.VoteValue == 1)
        {
            vote.Delete();
            post.VoteCount -= 1;
            post.Update();

        }
        else if (vote.VoteValue == -1)
        {
            vote.VoteValue = 1;
            vote.Update();
            post.VoteCount += 2;
            post.Update();

        }

        return post.VoteCount;
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

        DataTable post_dt = PostsClass.GetByProperties(new KeyValuePair<string, object>("ID", PostID));
        PostsClass post = PostsClass.FromDataRow(post_dt.Rows[0]);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if(vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, -1);
            post.VoteCount -= 1;
            post.Update();
            return post.VoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == -1)
        {
            vote.Delete();
            post.VoteCount += 1;
            post.Update();

        }
        else if (vote.VoteValue == 1)
        {
            vote.VoteValue = -1;
            vote.Update();
            post.VoteCount -= 2;
            post.Update();

        }
        return post.VoteCount;

    }

}
