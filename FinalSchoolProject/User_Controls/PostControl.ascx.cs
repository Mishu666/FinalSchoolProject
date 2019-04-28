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
    protected void Page_Load(object sender, EventArgs e)
    {
        PostsClass post = PostsClass.GetByID(PostID);
        post_card.Attributes["data-id"] = post.ID.ToString();
        post_card.Attributes["data-date"] = post.CreationDate.ToString("dd/MM/yyyy HH:mm:ss");
        post_card.Attributes["data-rating"] = (post.UpvoteCount - post.DownvoteCount).ToString();
        post_title.InnerText = post.Title;
        post_text.InnerText = post.Body;
        post_comment_count.InnerText = post.CommentCount.ToString();
        post_author_name.InnerText = UsersClass.GetByID(post.AuthorID).Username;
        post_author_name.HRef = "#";
        post_creation_date.InnerText = post.CreationDate.ToString("dd/MM/yyyy");
        post_consult_page.HRef = "ConsultPage.aspx?page-id=" + post.ConsultPageID.ToString();
        post_consult_page.InnerText = ConsultPagesClass.GetByID(post.ConsultPageID).PageName;
        upvote_counter.InnerText = post.UpvoteCount.ToString();
        upvote_counter.Attributes["data-post-id"] = post.ID.ToString();
        upvote_button.Attributes["data-post-id"] = post.ID.ToString();
        downvote_counter.InnerText = post.DownvoteCount.ToString();
        downvote_counter.Attributes["data-post-id"] = post.ID.ToString();
        downvote_button.Attributes["data-post-id"] = post.ID.ToString();
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
                GlobalFunctions.AddCssClass(upvote_button, "active");
            }
            else if (vote_val == -1)
            {
                GlobalFunctions.AddCssClass(downvote_button, "active");

            }
        }
        else
        {
            GlobalFunctions.RemoveCssClass(upvote_button, "active");
            GlobalFunctions.RemoveCssClass(downvote_button, "active");
        }
    }
}