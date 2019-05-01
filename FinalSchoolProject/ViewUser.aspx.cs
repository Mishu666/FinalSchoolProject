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
    public int UserID;
    protected void Page_Load(object sender, EventArgs e)
    {
        object userid = Request.QueryString["user-id"];
        if (userid == null)
        {
            if (Session["Logged"] == null || (bool)Session["Logged"] == false)
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                UserID = Convert.ToInt32(Session["CurrentUserID"]);
            }
        }
        else
        {
            UsersClass user = UsersClass.GetByID(Convert.ToInt32(userid));
            if (user == null)
            {
                Response.Redirect("Home.aspx");
            }
            else
            {
                UserID = user.ID;
            }
        }

        FillPostsRepeater("submitted");

    }

    public void FillPostsRepeater(string type)
    {
        UsersClass user = UsersClass.GetByID(Convert.ToInt32(UserID));
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

    public void ClearActiveTab()
    {

        GlobalFunctions.RemoveCssClass(ProfileMyPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileSavedPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileUpvotedPostsTab, "active");
        GlobalFunctions.RemoveCssClass(ProfileDownvotedPostsTab, "active");

    }

    protected void ProfilePostsTab_ServerClick(object sender, EventArgs e)
    {

        HtmlAnchor anchor = (HtmlAnchor)sender;
        string post_type = anchor.Attributes["data-post-type"];

        ClearActiveTab();
        GlobalFunctions.AddCssClass(anchor, "active");

        FillPostsRepeater(post_type);

    }

}