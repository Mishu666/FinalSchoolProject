using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using TreeViewBindingTest;

public partial class ViewPost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object post_id = Request.QueryString["post-id"];
            if (post_id == null) Response.Redirect("Home.aspx");
            else
            {
                int id = Convert.ToInt32(post_id);
                DataTable post_dt = PostsClass.GetByProperties(new KeyValuePair<string, object>("ID", id));
                (Master as PostPagesMasterPage).BindPostRepeater(post_dt);
                DisplayPostComments(id);
            }
        }
    }

    private void DisplayPostComments(int PostID)
    {
        DataTable original_comments = CommentsClass.GetByProperties(
            new KeyValuePair<string, object>("ParentPostID", PostID),
            new KeyValuePair<string, object>("ParentCommentID", 0));

        foreach (DataRow comment_dr in original_comments.Rows)
        {
            DisplayCommentHierarchy(CommentsClass.FromDataRow(comment_dr));
        }

    }

    private void DisplayCommentHierarchy(CommentsClass comment)
    {

        DisplayComment(comment);

        DataTable child_comments = CommentsClass.GetByProperties(new KeyValuePair<string, object>("ParentCommentID", comment.ID));

        foreach (DataRow comment_dr in child_comments.Rows)
        {
            DisplayCommentHierarchy(CommentsClass.FromDataRow(comment_dr));
        }

    }

    private void DisplayComment(CommentsClass comment)
    {
        //HtmlGenericControl comment_div = new HtmlGenericControl("div");
        //comment_div.Attributes["class"] = "card shadow-sm mb-3";
        //comment_div.Style["margin-left"] = comment.Depth * 40 + "px";

        //HtmlGenericControl comment_body = new HtmlGenericControl("div");
        //comment_body.Attributes["class"] = "card-body";

        //HtmlGenericControl comment_text = new HtmlGenericControl("div");
        //comment_body.Attributes["class"] = "card-text";



        //HtmlGenericControl comment_title = new HtmlGenericControl("h5");
        //comment_title.Attributes["class"] = "card-title text-gray-900";



        //comment_title.InnerText = comment.Title;

        //comment_body.Controls.Add(comment_title);
        //comment_div.Controls.Add(comment_body);

        //comment_view.Controls.Add(comment_div);

        string html_str = @"
            <div class='card shadow-sm mb-3' style='margin-left:{0}px;'>
                <div class='card-body'>
                    <h5 class='card-title'>{1}</h5>
                    <div class='card-text'>{2}</div>
                </div>
            </div>";

        html_str = string.Format(html_str, comment.Depth * 40, comment.Title, comment.Body);

        comment_view.InnerHtml += html_str;

    }


    protected void PostViewRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
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