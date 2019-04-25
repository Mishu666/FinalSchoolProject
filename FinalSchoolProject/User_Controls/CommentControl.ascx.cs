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
    public int Depth { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CommentsClass comment = CommentsClass.GetByID(CommentID);
        comment_text.InnerText = comment.Body;
        comment_card.Attributes["data-id"] = comment.ID.ToString();
        comment_card.Attributes["data-date"] = comment.CreationDate.ToString("dd/MM/yyyy HH:mm:ss");
        comment_card.Attributes["data-rating"] = (comment.UpvoteCount - comment.DownvoteCount).ToString();
        comment_text.InnerText = comment.Body;
        comment_author_name.InnerText = UsersClass.GetByID(comment.CommentorID).Username;
        comment_author_name.HRef = "#";
        comment_creation_date.InnerText = comment.CreationDate.ToString("dd/MM/yyyy");
        comment_upvote_counter.InnerText = comment.UpvoteCount.ToString();
        comment_upvote_counter.Attributes["data-comment-id"] = comment.ID.ToString();
        comment_upvote_button.Attributes["data-comment-id"] = comment.ID.ToString();
        comment_downvote_counter.InnerText = comment.DownvoteCount.ToString();
        comment_downvote_counter.Attributes["data-comment-id"] = comment.ID.ToString();
        comment_downvote_button.Attributes["data-comment-id"] = comment.ID.ToString();
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
            child.Depth = Depth + 1;
            child_comments_space.Controls.Add(child);
        }
    }

}