using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Controls_PostControl : System.Web.UI.UserControl
{
    public int PostID { get; set; }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        SetVoteButtonColor();
    }

    private void SetVoteButtonColor()
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false) return;

        int VoterID = Convert.ToInt32(Session["CurrentUserID"]);
        DataTable post_votes = PostVotesClass.GetByProperties(
            new KeyValuePair<string, object>("VotedPostID", PostID),
            new KeyValuePair<string, object>("VoterID", VoterID)
            );

        if (post_votes != null && post_votes.Rows.Count > 0)
        {
            PostVotesClass vote = PostVotesClass.FromDataRow(post_votes.Rows[0]);
            int vote_val = vote.VoteValue;

            if (vote_val == 1)
            {
                GlobalFunctions.AddCssClass(upvote_button, "active_vote");
            }
            else if (vote_val == -1)
            {
                GlobalFunctions.AddCssClass(downvote_button, "active_vote");

            }
        }
        else
        {
            GlobalFunctions.RemoveCssClass(upvote_button, "active_vote");
            GlobalFunctions.RemoveCssClass(downvote_button, "active_vote");
        }
    }
}