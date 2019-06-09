using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class ViewUser : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object userid = Request.QueryString["user-id"];

        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));

        if (userid == null)
        {
            if (Session["Logged"] == null || (bool)Session["Logged"] == false)
            {
                Response.Redirect("All.aspx");
            }
            else
            {
                ViewState["ViewUserID"] = current_user.ID;
            }
        }
        else
        {
            UsersClass user = UsersClass.GetByID(Convert.ToInt32(userid));
            if (user == null)
            {
                Response.Redirect("All.aspx");
            }
            else
            {
                ViewState["ViewUserID"] = user.ID;
            }
        }

        if (current_user == null) //guest viewing user profile
        {
            ProfileMyPostsTab.Visible = true;
        }
        else
        {
            UsersClass view_user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"]));

            if (view_user.ID == current_user.ID) //User viewing his own profile
            {
                ProfileMyPostsTab.Visible = true;
                ProfileSavedPostsTab.Visible = true;
                ProfileDownvotedPostsTab.Visible = true;
                ProfileUpvotedPostsTab.Visible = true;
                ProfileMessagesTab.Visible = true;
            }
            else //user viewing other user profile
            {

                ProfileMyPostsTab.Visible = true;
                ProfileConvoTab.Visible = true;
                ProfileNewMessageTab.Visible = true;
            }
        }

        FillPostsRepeater("submitted");
        FillModeratedPagesRepeater();


    }

    public void FillPostsRepeater(string type)
    {
        UsersClass user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"]));
        DataTable posts_dt;

        if (type == "submitted")
        {
            posts_dt = user.GetUserPosts();
        }
        else if (type == "saved")
        {
            posts_dt = user.GetUserSavedPosts();
        }
        else if (type == "upvoted")
        {
            posts_dt = user.GetUserUpvotedPosts();
        }
        else if (type == "downvoted")
        {
            posts_dt = user.GetUserDownvotedPosts();
        }
        else
        {
            return;
        }

        ProfilePostsRepeater.DataSource = posts_dt;
        ProfilePostsRepeater.DataBind();


    }

    public void FillModeratedPagesRepeater()
    {

        UsersClass user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"]));
        DataTable pages = user.GetMyModeratedPages();

        ModeratedPagesRepeater.DataSource = pages;
        ModeratedPagesRepeater.DataBind();

    }

    public void FillMessagesRepeater()
    {
        UsersClass user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));

        if (user == null) Response.Redirect("All.aspx");

        DataTable user_messages = user.GetMessages();

        ProfileMessagesRepeater.DataSource = user_messages;
        ProfileMessagesRepeater.DataBind();

    }

    public void FillConvoRepeater()
    {

        UsersClass user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"]));
        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));


        if (current_user == null) Response.Redirect("All.aspx");

        DataTable conversation = current_user.GetConversationWith(user.ID);

        ProfileConvoRepeater.DataSource = conversation;
        ProfileConvoRepeater.DataBind();


    }

    public void ClearActiveTab()
    {

        GlobalFunctions.RemoveCssClass(ProfileMyPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileSavedPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileUpvotedPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileDownvotedPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileMessagesTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileConvoTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileNewMessageTab, "active");

    }

    protected void ProfilePostsTab_ServerClick(object sender, EventArgs e)
    {

        HtmlAnchor anchor = (HtmlAnchor)sender;
        string post_type = anchor.Attributes["data-post-type"];

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = false;
        user_posts_space.Visible = true;
        new_message_space.Visible = false;

        FillPostsRepeater(post_type);

    }

    protected void ProfileMessagesTab_ServerClick(object sender, EventArgs e)
    {

        HtmlAnchor anchor = (HtmlAnchor)sender;

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = true;
        user_posts_space.Visible = false;
        new_message_space.Visible = false;

        FillMessagesRepeater();

    }

    protected void ProfileConvoTab_ServerClick(object sender, EventArgs e)
    {

        HtmlAnchor anchor = (HtmlAnchor)sender;

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = true;
        user_messages_space.Visible = false;
        user_posts_space.Visible = false;
        new_message_space.Visible = false;

        FillConvoRepeater();

    }
    protected void NewMessageTab_ServerClick(object sender, EventArgs e)
    {

        HtmlAnchor anchor = (HtmlAnchor)sender;

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = false;
        user_posts_space.Visible = false;
        new_message_space.Visible = true;
    }

    protected void ProfileConvoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

    }


}