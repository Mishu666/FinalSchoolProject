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
        object post_id = Request.QueryString["post-id"];
        if (post_id == null) Response.Redirect("Home.aspx");
        else
        {
            int id = Convert.ToInt32(post_id);
            PostsClass post = PostsClass.GetByID(id);

            if (post == null) Response.Redirect("Home.aspx");

            ViewState["PostID"] = post.ID;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    private void DisplayPostComments(int PostID)
    {
        DataTable original_comments = CommentsClass.GetOriginalComments(PostID);
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


    }

}