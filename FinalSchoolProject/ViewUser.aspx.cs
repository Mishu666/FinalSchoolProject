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
        if (!IsPostBack)
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

                    if(current_user.IsAdmin) //Admin viewing his own profile
                    {
                        ProfilePostReportsTab.Visible = true;
                        ProfileCommentReportsTab.Visible = true;
                    }

                }
                else //user viewing other user profile
                {

                    ProfileMyPostsTab.Visible = true;
                    ProfileConvoTab.Visible = true;
                    ProfileNewMessageTab.Visible = true;
                }
            }

            FillModeratedPagesRepeater();

            string page = Request.Params["Page"];

            if (!string.IsNullOrEmpty(page))
            {
                Session["Page"] = page;
            }
            else
            {
                if (Session["Page"] == null || Session["Page"].ToString() == "")
                {
                    page = "submitted";
                    Session["Page"] = page;
                }
                else
                {
                    page = Session["Page"].ToString();
                }
            }

            if (string.IsNullOrEmpty(page))
            {
                FillPostsRepeater("submitted");
            }
            else
            {
                if(page == "submitted")
                {
                    ProfilePostsTab_Show(ProfileMyPostsTab);
                }
                else if(page == "saved")
                {
                    ProfilePostsTab_Show(ProfileSavedPostsTab);
                }
                else if (page == "upvoted")
                {
                    ProfilePostsTab_Show(ProfileUpvotedPostsTab);
                }
                else if (page == "downvoted")
                {
                    ProfilePostsTab_Show(ProfileDownvotedPostsTab);
                }
                else if (page == "messages")
                {
                    ProfileMessagesTab_Show(ProfileMessagesTab);
                }
                else if (page == "conversation")
                {
                    ProfileConvoTab_Show(ProfileConvoTab);
                }
                else if (page == "new_message")
                {
                    NewMessageTab_Show(ProfileNewMessageTab);
                }
                else if (page == "post_reports")
                {
                    ProfileReports_Show(ProfilePostReportsTab);
                }
                else if (page == "comment_reports")
                {
                    ProfileReports_Show(ProfileCommentReportsTab);
                }
                else
                {
                    Response.Redirect("All.aspx");
                }
            }

        }

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

    public void FillPostReportsRepeater(string post_type)
    {

        UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));

        if (current_user == null | !current_user.IsAdmin) Response.Redirect("All.aspx");

        DataTable reports = null;

        if(post_type == "post_reports")
            reports = PostReportsClass.GetAllWithUserData();

        if (post_type == "comment_reports")
            reports = CommentReportsClass.GetAllWithUserData();

        reports_repeater.DataSource = reports;
        reports_repeater.DataBind();
    }

    //-----------------------------------------------------------------------------------------------------

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

    protected void ProfilePostsTab_Show(HtmlAnchor anchor)
    {
        string post_type = anchor.Attributes["data-post-type"];

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = false;
        user_posts_space.Visible = true;
        new_message_space.Visible = false;
        reports_space.Visible = false;

        FillPostsRepeater(post_type);

    }

    protected void ProfileMessagesTab_Show(HtmlAnchor anchor)
    {

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = true;
        user_posts_space.Visible = false;
        new_message_space.Visible = false;
        reports_space.Visible = false;

        FillMessagesRepeater();

    }

    protected void ProfileConvoTab_Show(HtmlAnchor anchor)
    {

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = true;
        user_messages_space.Visible = false;
        user_posts_space.Visible = false;
        new_message_space.Visible = false;
        reports_space.Visible = false;

        FillConvoRepeater();

    }

    protected void NewMessageTab_Show(HtmlAnchor anchor)
    {

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = false;
        user_posts_space.Visible = false;
        new_message_space.Visible = true;
        reports_space.Visible = false;
    }

    protected void ProfileReports_Show(HtmlAnchor anchor)
    {

        string post_type = anchor.Attributes["data-post-type"];

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        user_convo_space.Visible = false;
        user_messages_space.Visible = false;
        user_posts_space.Visible = false;
        new_message_space.Visible = false;
        reports_space.Visible = true;

        FillPostReportsRepeater(post_type);

    }

    protected void reports_repeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView rowView = (DataRowView)e.Item.DataItem;
        HtmlAnchor anchor = (HtmlAnchor)e.Item.FindControl("ViewReportedLink");

        if (rowView.Row.Table.Columns.Contains("ParentPostID"))
        {
            anchor.HRef += rowView["ParentPostID"];
        }
        else
        {
            anchor.HRef += rowView["Posts.ID"];
        }
    }
}