using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for CommentTree
/// </summary>
public class CommentTree
{
    int PostID;
    public CommentsClass Value;
    public List<CommentTree> Children;

    public CommentTree(int PostID, CommentsClass Value)
    {
        this.PostID = PostID;
        this.Value = Value;
        this.Children = new List<CommentTree>();
    }

    public void SortChildren()
    {

    }

    public bool InsertComment(CommentsClass comment)
    {
        if (comment.ParentPostID != this.PostID) return false;
        if (comment.ParentCommentID == 0) return false;
        if(comment.ParentCommentID == this.Value.ID)
        {
            this.Children.Add(new CommentTree(this.PostID, comment));
            return true;
        }
        foreach(CommentTree child in this.Children)
        {
            if (child.InsertComment(comment)) return true;
        }
        return false;

    }

    public List<CommentTree> GetPostCommentsAsTrees(int PostID)
    {
        List<CommentTree> CommentTreesList = new List<CommentTree>();
        CommentsClass current_comment;
        DataTable comments = CommentsClass.GetByProperties(new KeyValuePair<string, object>("ParentPostID", PostID));
        DataView commentsDV = comments.DefaultView;
        commentsDV.Sort = "ParentCommentID ASC";
        comments = commentsDV.ToTable();
        
        foreach(DataRow comment_dr in comments.Rows)
        {
            current_comment = CommentsClass.FromDataRow(comment_dr);
            if(current_comment.ParentCommentID == 0)
            {
                CommentTreesList.Add(new CommentTree(PostID, current_comment));
            }
            else
            {
                foreach(CommentTree tree in CommentTreesList)
                {
                    if (tree.InsertComment(current_comment)) break;
                }
            }
        }

        return CommentTreesList;

    }



}