﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class ViewPost : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        object post_id = Request.QueryString["post-id"];
        if (post_id == null) Response.Redirect("All.aspx");
        else
        {
            int id = Convert.ToInt32(post_id);
            PostsClass post = PostsClass.GetByID(id);

            if (post == null) Response.Redirect("All.aspx");

            ViewPostPostControl.PostID = id;
            ViewState["PostID"] = id;
            DisplayPostComments(id);
        }
    }

    private void DisplayPostComments(int PostID)
    {
        DataTable original_comments = CommentsClass.GetOriginalComments(PostID);
        foreach (DataRow comment_dr in original_comments.Rows)
        {
            DisplayComment(CommentsClass.FromDataRow(comment_dr));
        }

    }

    private void DisplayComment(CommentsClass comment)
    {
        CommentControl comment_control = (CommentControl)LoadControl("~/UserControls/CommentControl.ascx");
        comment_control.CommentID = comment.ID;
        comment_space.Controls.Add(comment_control);

    }

}