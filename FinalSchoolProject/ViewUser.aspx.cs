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
                    Response.Redirect("Home.aspx");
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
                    Response.Redirect("Home.aspx");
                }
                else
                {
                    ViewState["ViewUserID"] = user.ID;
                }
            }

            if(current_user == null)
            {
                FollowUserButton.Visible = true;
            }
            else
            {
                UsersClass view_user = UsersClass.GetByID(Convert.ToInt32(ViewState["ViewUserID"]));

                if (view_user.ID == current_user.ID)
                {
                    EditUserButton.Visible = true;
                }
                else
                {
                    KeyValuePair<string, object> follower_id_pair = new KeyValuePair<string, object>("FollowerID", current_user.ID);
                    KeyValuePair<string, object> followed_id_pair = new KeyValuePair<string, object>("FollowedID", view_user.ID);
                    DataTable followers_dt = FollowersClass.GetByProperties(follower_id_pair, followed_id_pair);

                    if(followers_dt == null || followers_dt.Rows.Count ==0)
                    {
                        FollowUserButton.Visible = true;
                    }
                    else
                    {
                        UnfollowUserButton.Visible = true;
                    }
                }
            }

            FillPostsRepeater("submitted");
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