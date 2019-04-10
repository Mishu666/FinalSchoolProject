using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindRepeater();
    }

    private void BindRepeater()
    {
        DataTable dt = PostsClass.GetAll();
        HomePostsRepeater.DataSource = dt;
        HomePostsRepeater.DataBind();
    }
    

    protected void HomePostsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        KeyValuePair<string, object> voterID = new KeyValuePair<string, object>("VoterID", Convert.ToInt32(Session["CurrentUserID"]));
        KeyValuePair<string, object> PostID = new KeyValuePair<string, object>("VotedPostID", drv["ID"]);

        DataTable user_votes = PostVotesClass.GetByProperty(PostID,voterID);
        if (user_votes != null && user_votes.Rows.Count > 0)
        {
            int vote_val = Convert.ToInt32(user_votes.Rows[0]["VoteValue"]);
            WebControl upvote = (WebControl)FindControl("upvote_button");
            WebControl downvote = (WebControl)FindControl("downvote_button");
            if (vote_val == 1)
            {
                GlobalFunctions.AddCssClass(upvote, "active");
            }
            else if (vote_val == -1)
            {
                GlobalFunctions.AddCssClass(downvote, "active");

            }
            else
            {
                throw new Exception("invalid vote value");
            }
        }
    }
}