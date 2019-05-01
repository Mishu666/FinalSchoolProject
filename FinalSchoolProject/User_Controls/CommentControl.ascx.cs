using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Controls_CommentControl : System.Web.UI.UserControl
{
    public int CommentID { get; set; }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        SetVoteButtonColor();
        DisplayChildren();
        if (child_comments_space.Controls.Count == 0) child_comments_space.Visible = false;
    }

    private void SetVoteButtonColor()
    {
        if (Session["Logged"] == null || (bool)Session["Logged"] == false) return;

        int VoterID = Convert.ToInt32(Session["CurrentUserID"]);
        DataTable comment_votes = CommentVotesClass.GetByProperties(
            new KeyValuePair<string, object>("VotedcommentID", CommentID),
            new KeyValuePair<string, object>("VoterID", VoterID)
            );

        if (comment_votes != null && comment_votes.Rows.Count > 0)
        {
            CommentVotesClass vote = CommentVotesClass.FromDataRow(comment_votes.Rows[0]);
            int vote_val = vote.VoteValue;

            if (vote_val == 1)
            {
                GlobalFunctions.AddCssClass(comment_upvote_button, "active");
            }
            else if (vote_val == -1)
            {
                GlobalFunctions.AddCssClass(comment_downvote_button, "active");

            }
        }
        else
        {
            GlobalFunctions.RemoveCssClass(comment_upvote_button, "active");
            GlobalFunctions.RemoveCssClass(comment_downvote_button, "active");
        }
    }

    private void DisplayChildren()
    {
        DataTable child_comments = CommentsClass.GetByProperties(new KeyValuePair<string, object>("ParentCommentID", CommentID));

        foreach (DataRow comment_dr in child_comments.Rows)
        {
            User_Controls_CommentControl child = (User_Controls_CommentControl)Page.LoadControl("~/User_Controls/CommentControl.ascx");
            child.CommentID = Convert.ToInt32(comment_dr["ID"]);
            child_comments_space.Controls.Add(child);
        }
    }

}