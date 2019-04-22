using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class PostPagesMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void BindPostRepeater(DataTable dt)
    {
        PostsRepeater.DataSource = dt;
        PostsRepeater.DataBind();
    }

    protected void PostsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        DataRowView post_drv = (DataRowView)e.Item.DataItem;
        int VoterID = Convert.ToInt32(Session["CurrentUserID"]);
        int PostID = Convert.ToInt32(post_drv["ID"]);
        DataTable user_votes_dt = PostVotesClass.GetByProperties(
            new KeyValuePair<string, object>("VotedPostID", PostID),
            new KeyValuePair<string, object>("VoterID", VoterID)
            );

        HtmlControl upvote = (HtmlControl)e.Item.FindControl("upvote_button");
        HtmlControl downvote = (HtmlControl)e.Item.FindControl("downvote_button");

        if (user_votes_dt != null && user_votes_dt.Rows.Count > 0)
        {
            PostVotesClass vote = PostVotesClass.FromDataRow(user_votes_dt.Rows[0]);
            int vote_val = vote.VoteValue;

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
        else
        {
            GlobalFunctions.RemoveCssClass(upvote, "active");
            GlobalFunctions.RemoveCssClass(downvote, "active");
        }
    }

}
