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
    public void Upvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostVotesClass upvote = PostVotesClass.CreateNew(userID, PostID, 1);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void Downvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            HttpContext current = HttpContext.Current;
            current.Response.StatusCode = 401;
            current.Response.End();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostVotesClass upvote = PostVotesClass.CreateNew(userID, PostID, -1);
    }

}
