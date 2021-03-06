﻿using System;
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

    private void Abort()
    {
        HttpResponse res = HttpContext.Current.Response;
        res.StatusCode = 401;
        res.End();
    }

    //------------------------------------------------------------------------------------------------------------

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
            if (username.Length > 15)
            {
                warnings.Add(new Warning("signupInputUsername", "username cannot be longer than 15 characters"));
                valid = false;
            }

            if (UsersClass.UsernameTaken(username))
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

    //------------------------------------------------------------------------------------------------------------

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public bool UsernameTaken(string username)
    {
        return UsersClass.UsernameTaken(username);
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

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int Upvote(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        if (post.IsDeleted)
        {
            Abort();
        }

        if (post.IsRemoved)
        {
            Abort();
        }

        UsersClass PostAuthor = UsersClass.GetByID(post.AuthorID);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, 1);
            post.UpvoteCount += 1;
            PostAuthor.Points += 5;
            PostAuthor.Update();
            post.Update();
            return post.UpvoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == 1)
        {
            vote.Delete();
            post.UpvoteCount -= 1;
            PostAuthor.Points -= 5;
            PostAuthor.Update();
            post.Update();

        }
        else if (vote.VoteValue == -1)
        {
            vote.VoteValue = 1;
            vote.Update();
            post.UpvoteCount += 1;
            post.DownvoteCount -= 1;
            PostAuthor.Points += 10;
            PostAuthor.Update();
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
            Abort();
        }
        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        if (post.IsDeleted)
        {
            Abort();
        }

        if (post.IsRemoved)
        {
            Abort();
        }

        UsersClass PostAuthor = UsersClass.GetByID(post.AuthorID);

        DataTable vote_dt = PostVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedPostID", PostID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            PostVotesClass.CreateNew(userID, PostID, -1);
            post.DownvoteCount += 1;
            PostAuthor.Points -= 5;
            PostAuthor.Update();
            post.Update();
            return post.DownvoteCount;
        }

        PostVotesClass vote = PostVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == -1)
        {
            vote.Delete();
            post.DownvoteCount -= 1;
            PostAuthor.Points += 5;
            PostAuthor.Update();
            post.Update();

        }
        else if (vote.VoteValue == 1)
        {
            vote.VoteValue = -1;
            vote.Update();
            post.DownvoteCount += 1;
            post.UpvoteCount -= 1;
            PostAuthor.Points -= 10;
            PostAuthor.Update();
            post.Update();

        }
        return post.DownvoteCount;

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public int CommentUpvote(int CommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        if (comment == null)
        {
            Abort();
        }

        if (comment.IsDeleted)
        {
            Abort();
        }

        if (comment.IsRemoved)
        {
            Abort();
        }

        UsersClass CommentAuthor = UsersClass.GetByID(comment.CommentorID);

        DataTable vote_dt = CommentVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedCommentID", CommentID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            CommentVotesClass.CreateNew(userID, CommentID, 1);
            comment.UpvoteCount += 1;
            CommentAuthor.Points += 1;
            CommentAuthor.Update();
            comment.Update();
            return comment.UpvoteCount;
        }

        CommentVotesClass vote = CommentVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == 1)
        {
            vote.Delete();
            comment.UpvoteCount -= 1;
            CommentAuthor.Points -= 1;
            CommentAuthor.Update();
            comment.Update();

        }
        else if (vote.VoteValue == -1)
        {
            vote.VoteValue = 1;
            vote.Update();
            comment.UpvoteCount += 1;
            comment.DownvoteCount -= 1;
            CommentAuthor.Points += 2;
            CommentAuthor.Update();
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
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        if (comment == null)
        {
            Abort();
        }

        if (comment.IsDeleted)
        {
            Abort();
        }

        if (comment.IsRemoved)
        {
            Abort();
        }

        UsersClass CommentAuthor = UsersClass.GetByID(comment.CommentorID);

        DataTable vote_dt = CommentVotesClass.GetByProperties(
                new KeyValuePair<string, object>("VotedCommentID", CommentID),
                new KeyValuePair<string, object>("VoterID", userID)
            );

        if (vote_dt == null || vote_dt.Rows.Count == 0)
        {
            CommentVotesClass.CreateNew(userID, CommentID, -1);
            comment.DownvoteCount += 1;
            CommentAuthor.Points -= 1;
            CommentAuthor.Update();
            comment.Update();
            return comment.DownvoteCount;
        }

        CommentVotesClass vote = CommentVotesClass.FromDataRow(vote_dt.Rows[0]);

        if (vote.VoteValue == -1)
        {
            vote.Delete();
            comment.DownvoteCount -= 1;
            CommentAuthor.Points += 1;
            CommentAuthor.Update();
            comment.Update();

        }
        else if (vote.VoteValue == 1)
        {
            vote.VoteValue = -1;
            vote.Update();
            comment.DownvoteCount += 1;
            comment.UpvoteCount -= 1;
            CommentAuthor.Points -= 2;
            CommentAuthor.Update();
            comment.Update();

        }
        return comment.DownvoteCount;

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void ReportPost(int PostID, string reportBody)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);
        UsersClass author = UsersClass.GetByID(post.AuthorID);

        author.Points -= 20;
        author.Flags += 1;
        author.Update();

        PostReportsClass.CreateNew(userID, PostID, reportBody);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndReportPost(int PostID, string reportBody)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        bool valid = true;

        if (string.IsNullOrWhiteSpace(reportBody))
        {
            warnings.Add(new Warning("ReportPostTextArea", "Report cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            ReportPost(PostID, reportBody);
        }

        return warnings;
    }


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void ReportComment(int CommentID, string reportBody)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass comment = CommentsClass.GetByID(CommentID);
        UsersClass author = UsersClass.GetByID(comment.CommentorID);

        author.Points -= 20;
        author.Flags += 1;
        author.Update();

        CommentReportsClass.CreateNew(userID, CommentID, reportBody);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndReportComment(int CommentID, string reportBody)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        if (comment == null)
        {
            Abort();
        }

        bool valid = true;

        if (string.IsNullOrWhiteSpace(reportBody))
        {
            warnings.Add(new Warning("ReportCommentTextArea", "Report cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            ReportComment(CommentID, reportBody);
        }

        return warnings;
    }



    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeletePost(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        if (post.AuthorID != userID)
        {
            Abort();
        }

        post.AuthorID = 0;
        post.IsDeleted = true;
        post.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void RemovePost(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        UsersClass current_user = UsersClass.GetByID(userID);
        bool IsMod = current_user.IsModeratorFor(post.ConsultPageID);

        if (post.AuthorID != userID && current_user.IsAdmin == false && !IsMod)
        {
            Abort();
        }

        post.AuthorID = 0;
        post.IsRemoved = true;
        post.Update();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeleteComment(int CommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        if (comment == null)
        {
            Abort();
        }

        if (comment.CommentorID != userID)
        {
            Abort();
        }

        comment.CommentorID = 0;
        comment.IsDeleted = true;
        comment.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void RemoveComment(int CommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        if (comment == null)
        {
            Abort();
        }

        UsersClass current_user = UsersClass.GetByID(userID);
        PostsClass ParentPost = PostsClass.GetByID(comment.ParentPostID);
        bool IsPageMod = current_user.IsModeratorFor(ParentPost.ConsultPageID);

        if (comment.CommentorID != userID && current_user.IsAdmin == false && !IsPageMod)
        {
            Abort();
        }

        comment.CommentorID = 0;
        comment.IsRemoved = true;
        comment.Update();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void Subscribe(int PageID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        UsersClass current_user = UsersClass.GetByID(userID);

        if (current_user.IsSubscribedTo(PageID))
        {
            DataTable sub_dt = SubscriptionsClass.GetByProperties(
                new KeyValuePair<string, object>("PageID", PageID),
                new KeyValuePair<string, object>("SubscriberID", current_user.ID)
                );
            SubscriptionsClass sub = SubscriptionsClass.FromDataRow(sub_dt.Rows[0]);
            sub.Delete();
            page.SubscriberCount -= 1;
            page.Update();
        }
        else
        {
            SubscriptionsClass.CreateNew(current_user.ID, PageID);
            page.SubscriberCount += 1;
            page.Update();
        }

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void SavePost(int PostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(PostID);

        if (post == null)
        {
            Abort();
        }

        UsersClass PostAuthor = UsersClass.GetByID(post.AuthorID);

        DataTable save_dt = SavedPostsClass.GetByProperties(
                new KeyValuePair<string, object>("SavedPostID", PostID),
                new KeyValuePair<string, object>("SaverID", userID)
            );

        if (save_dt == null || save_dt.Rows.Count == 0)
        {
            PostAuthor.Points += 10;
            SavedPostsClass.CreateNew(userID, PostID);
            PostAuthor.Update();
            return;
        }

        SavedPostsClass save = SavedPostsClass.FromDataRow(save_dt.Rows[0]);
        save.Delete();
        PostAuthor.Points -= 10;
        PostAuthor.Update();


    }

    //------------------------------------------------------------------------------------------------------------

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
            Abort();
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

    //------------------------------------------------------------------------------------------------------------

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreateNewComment(string Body, int ParentPostID)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        PostsClass post = PostsClass.GetByID(ParentPostID);
        CommentsClass.CreateNew(Body, userID, ParentPostID, 0);
        post.CommentCount += 1;
        post.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreateComment(string Body, int ParentPostID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        PostsClass post = PostsClass.GetByID(ParentPostID);
        bool valid = true;

        if (post == null)
        {
            Abort();
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

    //------------------------------------------------------------------------------------------------------------

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreateCommentReply(string Body, int ParentCommentID)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        CommentsClass parent_comment = CommentsClass.GetByID(ParentCommentID);
        PostsClass parent_post = PostsClass.GetByID(parent_comment.ParentPostID);
        CommentsClass.CreateNew(Body, userID, ParentCommentID, parent_post.ID);
        parent_post.CommentCount += 1;
        parent_post.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreateCommentReply(string Body, int ParentCommentID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        CommentsClass comment = CommentsClass.GetByID(ParentCommentID);
        bool valid = true;

        if (comment == null)
        {
            Abort();
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

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void UpdateUserInfo(string Username, string Bio, string NewPassword)
    {

        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass user = UsersClass.GetByID(userID);

        user.Username = Username;
        user.Bio = Bio;
        user.Password = NewPassword;
        user.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndUpdateUserInfo(string Username, string Bio, string NewPassword, string NewPasswordConfirm, string PasswordConfirm)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
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
            if (UsersClass.UsernameTaken(Username) && Username != user.Username)
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

        if (!string.IsNullOrWhiteSpace(NewPassword) || !string.IsNullOrWhiteSpace(NewPasswordConfirm))
        {
            if (NewPassword != NewPasswordConfirm)
            {
                Warning w = new Warning();
                w.Text = "Passwords do not match";
                w.WarnControls = new List<string>();
                w.WarnControls.Add("EditNewPasswordInput");
                w.WarnControls.Add("EditConfirmNewPasswordInput");
                warnings.Add(w);
                valid = false;
            }
        }
        else
        {
            NewPassword = user.Password;
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
            UpdateUserInfo(Username, Bio, NewPassword);
        }

        return warnings;

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void EditPost(int PostID, string Body)
    {
        PostsClass post = PostsClass.GetByID(PostID);

        post.Body = Body;
        post.IsEdited = true;
        post.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndEditPost(int PostID, string Body)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        PostsClass post = PostsClass.GetByID(PostID);
        bool valid = true;

        if (post == null)
        {
            Abort();
        }

        if (post.IsDeleted)
        {
            Abort();
        }

        if (post.IsRemoved)
        {
            Abort();
        }

        if (string.IsNullOrWhiteSpace(Body))
        {
            warnings.Add(new Warning("EditableArea", "Post cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            EditPost(PostID, Body);
        }

        return warnings;

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void EditComment(int CommentID, string Body)
    {
        CommentsClass comment = CommentsClass.GetByID(CommentID);

        comment.Body = Body;
        comment.IsEdited = true;
        comment.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndEditComment(int CommentID, string Body)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        CommentsClass comment = CommentsClass.GetByID(CommentID);
        bool valid = true;

        if (comment == null)
        {
            Abort();
        }

        if (comment.IsDeleted)
        {
            Abort();
        }

        if (comment.IsRemoved)
        {
            Abort();
        }

        if (string.IsNullOrWhiteSpace(Body))
        {
            warnings.Add(new Warning("EditableArea", "Comment cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            EditComment(CommentID, Body);
        }

        return warnings;

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string SendMessage(int RecipientID, string Message)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        int userID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass user = UsersClass.GetByID(userID);

        if (string.IsNullOrWhiteSpace(Message)) return "empty message";

        MessagesClass msg = MessagesClass.CreateNew(userID, RecipientID, Message);
        return "sent " + Message;

    }

    //------------------------------------------------------------------------------------------------------------


    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeleteUser(int UserID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        UsersClass user = UsersClass.GetByID(UserID);

        if (user == null) Abort();

        DataTable posts_dt = user.GetUserPosts();
        foreach (DataRow dt in posts_dt.Rows)
        {
            DeletePost((int)dt["ID"]);
        }

        DataTable comments_dt = user.GetUserComments();
        foreach (DataRow dt in comments_dt.Rows)
        {
            DeleteComment((int)dt["ID"]);
        }

        user.Delete();

        if (UserID == (int)Session["CurrentUserID"])
            LogOutUser();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void KickUser(int UserID, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsModeratorFor(PageID))
        {
            Abort();
        }

        UsersClass UserToEdit = UsersClass.GetByID(UserID);

        if (UserToEdit == null)
        {
            Abort();
        }

        if (UserToEdit.IsSubscribedTo(PageID))
        {
            DataTable sub_dt = SubscriptionsClass.GetByProperties(
                new KeyValuePair<string, object>("PageID", PageID),
                new KeyValuePair<string, object>("SubscriberID", UserID)
                );

            SubscriptionsClass sub = SubscriptionsClass.FromDataRow(sub_dt.Rows[0]);
            sub.Delete();
            page.SubscriberCount -= 1;
            page.Update();
        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void IncludeUser(int UserID, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsModeratorFor(PageID))
        {
            Abort();
        }

        UsersClass UserToEdit = UsersClass.GetByID(UserID);

        if (UserToEdit == null)
        {
            Abort();
        }

        if (!UserToEdit.IsSubscribedTo(PageID))
        {
            SubscriptionsClass.CreateNew(UserID, PageID);
            page.SubscriberCount += 1;
            page.Update();
        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void IncludeMultipleUsers(int[] UserIDs, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsModeratorFor(PageID))
        {
            Abort();
        }

        UsersClass UserToEdit;

        foreach (int ID in UserIDs)
        {

            UserToEdit = UsersClass.GetByID(ID);

            if (UserToEdit == null)
            {
                continue;
            }

            if (!UserToEdit.IsSubscribedTo(PageID))
            {
                SubscriptionsClass.CreateNew(ID, PageID);
                page.SubscriberCount += 1;
            }

        }

        page.Update();


    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void KickMultipleUsers(int[] UserIDs, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsModeratorFor(PageID))
        {
            Abort();
        }

        UsersClass UserToEdit;

        foreach (int ID in UserIDs)
        {

            UserToEdit = UsersClass.GetByID(ID);

            if (UserToEdit == null)
            {
                continue;
            }

            if (UserToEdit.IsSubscribedTo(PageID))
            {
                DataTable sub_dt = SubscriptionsClass.GetByProperties(
                    new KeyValuePair<string, object>("PageID", PageID),
                    new KeyValuePair<string, object>("SubscriberID", ID)
                    );

                SubscriptionsClass sub = SubscriptionsClass.FromDataRow(sub_dt.Rows[0]);
                sub.Delete();
                page.SubscriberCount -= 1;
            }

        }

        page.Update();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void UpdateAdminStatus(bool IsAdmin, int UserID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);
        UsersClass EditedUser = UsersClass.GetByID(UserID);

        if (EditedUser == null)
        {
            Abort();
        }

        if (CurrentUserID == UserID)
        {
            Abort();
        }

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        EditedUser.IsAdmin = IsAdmin;
        EditedUser.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void UpdateSuspendedStatus(bool IsSuspended, int UserID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);
        UsersClass EditedUser = UsersClass.GetByID(UserID);

        if (EditedUser == null)
        {
            Abort();
        }

        if (CurrentUserID == UserID)
        {
            Abort();
        }

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        EditedUser.IsSuspended = IsSuspended;
        EditedUser.Update();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DeletePage(int PageID)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null) Abort();

        page.Delete();

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DemoteUser(int UserID, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        UsersClass UserToEdit = UsersClass.GetByID(UserID);

        if (UserToEdit == null)
        {
            Abort();
        }

        if (UserToEdit.IsModeratorFor(PageID))
        {
            DataTable mod_dt = ModeratorsClass.GetByProperties(
                new KeyValuePair<string, object>("PageID", PageID),
                new KeyValuePair<string, object>("ModeratorID", UserID)
                );

            ModeratorsClass mod = ModeratorsClass.FromDataRow(mod_dt.Rows[0]);
            mod.Delete();

            UserToEdit.ModeratedPagesCount -= 1;
            if(UserToEdit.ModeratedPagesCount == 0)
            {
                UserToEdit.IsMod = false;
            }
            UserToEdit.Update();

        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void PromoteUser(int UserID, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        UsersClass UserToEdit = UsersClass.GetByID(UserID);

        if (UserToEdit == null)
        {
            Abort();
        }

        if (!UserToEdit.IsMod ||  !UserToEdit.IsModeratorFor(PageID))
        {
            ModeratorsClass.CreateNew(PageID, UserID);
            UserToEdit.IsMod = true;
            UserToEdit.ModeratedPagesCount += 1;
            UserToEdit.Update();
        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void PromoteMultipleUsers(int[] UserIDs, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        UsersClass UserToEdit;

        foreach (int ID in UserIDs)
        {

            UserToEdit = UsersClass.GetByID(ID);

            if (UserToEdit == null)
            {
                continue;
            }

            if (!UserToEdit.IsMod || !UserToEdit.IsModeratorFor(PageID))
            {
                ModeratorsClass.CreateNew(PageID, ID);

                UserToEdit.IsMod = true;
                UserToEdit.ModeratedPagesCount += 1;
                UserToEdit.Update();
            }

        }

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public void DemoteMultipleUsers(int[] UserIDs, int PageID)
    {

        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        ConsultPagesClass page = ConsultPagesClass.GetByID(PageID);

        if (page == null)
        {
            Abort();
        }

        int CurrentUserID = Convert.ToInt32(Session["CurrentUserID"]);
        UsersClass current_user = UsersClass.GetByID(CurrentUserID);

        if (!current_user.IsAdmin)
        {
            Abort();
        }

        UsersClass UserToEdit;

        foreach (int ID in UserIDs)
        {

            UserToEdit = UsersClass.GetByID(ID);

            if (UserToEdit == null)
            {
                continue;
            }

            if (UserToEdit.IsModeratorFor(PageID))
            {
                DataTable mod_dt = ModeratorsClass.GetByProperties(
                    new KeyValuePair<string, object>("PageID", PageID),
                    new KeyValuePair<string, object>("ModeratorID", ID)
                    );

                ModeratorsClass mod = ModeratorsClass.FromDataRow(mod_dt.Rows[0]);
                mod.Delete();

                UserToEdit.ModeratedPagesCount -= 1;
                if (UserToEdit.ModeratedPagesCount == 0)
                {
                    UserToEdit.IsMod = false;
                }
                UserToEdit.Update();

            }

        }

    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void CreatePage(string PageName)
    {
        ConsultPagesClass.CreateNew(PageName);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndCreatePage(string PageName)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(PageName))
        {
            warnings.Add(new Warning("NewConsultPageInsertButton", "Title cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            CreatePage(PageName);
        }

        return warnings;
    }

    //------------------------------------------------------------------------------------------------------------

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    private void UpdatePageDescription(int PageID, string Description)
    {

        ConsultPagesClass cp = ConsultPagesClass.GetByID(PageID);
        cp.Description = Description;
        cp.Update();

    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public List<Warning> ValidateAndUpdatePageDescription(int PageID, string Description)
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false)
        {
            Abort();
        }

        List<Warning> warnings = new List<Warning>();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(Description))
        {
            warnings.Add(new Warning("NewConsultPageInsertButton", "Title cannot be empty"));
            valid = false;
        }

        if (valid)
        {
            UpdatePageDescription(PageID, Description);
        }

        return warnings;
    }


}
