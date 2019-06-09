<%@ Control Language="C#" AutoEventWireup="true" CodeFile="~/UserControls/CommentControl.ascx.cs" Inherits="CommentControl" %>


<% CommentsClass comment = CommentsClass.GetByID(CommentID); %>

<div class='card shadow-sm mt-2 mb-3 comment border-top-0 border-bottom-0 border-right-0 border-primary' data-rating='<%= comment.UpvoteCount - comment.DownvoteCount %>'
    data-comment-id='<%= comment.ID %>' data-date='<%= comment.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") %>'
    data-isremoved="<%= comment.IsRemoved %>" data-isdeleted="<%= comment.IsDeleted %>" style="border-width: 3px;">
    <div class='card-body d-flex flex-row justify-content-between align-items-center'>

        <%
            if (!comment.IsDeleted && !comment.IsRemoved)
            {
        %>

        <div class="default_comment_view card-text comment_text">
            <%= comment.Body %>
        </div>

        <%
            }
            else if (comment.IsDeleted)
            {
        %>

        <div class="default_comment_view card-text text-danger comment_text">
            [deleted]
        </div>

        <%
            }
            else if (comment.IsRemoved)
            {
        %>

        <div class="default_comment_view card-text text-danger comment_text">
            [removed]
        </div>

        <%
            }
        %>

        <%
            if (Session["Logged"] != null && (bool)Session["Logged"] == true && !comment.IsDeleted && !comment.IsRemoved)
            {

                UsersClass current_user = UsersClass.GetByID(Convert.ToInt32(Session["CurrentUserID"]));
                PostsClass post = PostsClass.GetByID(comment.ParentPostID);
                bool IsMod = current_user.IsModeratorFor(post.ConsultPageID);
        %>

        <div class="comment_menu dropdown no-arrow">
            <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400 comment_dropdown"></i>
            </a>
            <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in" aria-labelledby="dropdownMenuLink"
                x-placement="bottom-end" style="position: absolute; will-change: transform; top: 0px; left: 0px; transform: translate3d(17px, 19px, 0px);">

                <%
                    if ((IsMod || current_user.IsAdmin) && current_user.ID != comment.CommentorID)
                    {
                %>

                <button class="RemoveCommentButton dropdown-item"><i class="fas fa-times mr-2 text-gray-400"></i>Remove</button>

                <%
                    }
                %>

                <%
                    if (current_user.ID == comment.CommentorID)
                    {
                %>

                <button class="EditCommentButton dropdown-item"><i class="fas fa-pencil-alt mr-2 text-gray-400"></i>Edit</button>
                <button class="DeleteCommentButton dropdown-item"><i class="fas fa-trash-alt mr-2 text-gray-400"></i>Delete</button>

                <%
                    }
                %>

                <button class="ReportCommentButton dropdown-item"><i class="fas fa-flag mr-2 text-gray-400"></i>Report</button>
            </div>
        </div>

        <%
            }
        %>

        <div class="edit_comment_view" style="display: none;">
            <div class="EditableAreaSpace mb-3">
                <div class="EditableArea"><%= comment.Body %></div>
            </div>
            <button type="button" class="CancelCommentEditButton btn btn-secondary">Cancel</button>
            <button type="button" class="ConfirmCommentEditButton btn btn-primary">Confirm</button>
            <span class="EditCommentWarningSpace text-danger ml-3" style="display: none;"></span>
        </div>

    </div>
    <div class="d-flex flex-column justify-content-between align-items-start text-gray-700 card-footer w-100" id="comment_footer">
        <div class="d-flex flex-row justify-content-between align-items-center w-100">
            <div class="mr-auto d-flex flex-row w-100">
                <span>comment by&nbsp;</span>
                <%  if (comment.IsDeleted)
                    {
                %>

                <span class="text-danger comment_author_name">[deleted]</span>

                <%
                    }
                    else if (comment.IsRemoved)
                    {
                %>

                <span class="text-danger comment_author_name">[removed]</span>

                <%
                    }
                    else
                    {
                %>

                <a href="ViewUser.aspx?user-id=<%= comment.CommentorID %>" class="comment_author_name"><%= UsersClass.GetByID(comment.CommentorID).Username %></a>

                <%
                    }
                %>
                <span>&nbsp;on&nbsp;</span>
                <div class="comment_creation_date"><%= comment.CreationDate.ToString("dd/MM/yyyy") %></div>
            </div>
            <div class="action-block d-flex flex-row">
                <a role="button" class="mr-5 p-0 text-gray-500 action_space replyButton dismissable_popover"
                    data-placement="top" data-content="you must log in to reply">
                    <i tabindex="-1" class="reply_button action fas fa-reply"></i>
                </a>
                <a role="button" class="mr-5 p-0 text-gray-500 action_space collapse_children_space">
                    <i tabindex="-1" class="collapse_children action fas fa-chevron-up"></i>
                </a>
                <a runat="server" role="button" id="comment_upvote_button" class="mr-1 p-0 text-gray-500 action_space comment_upvote_space dismissable_popover"
                    data-placement="top" data-content="you must log in to vote">
                    <i tabindex="-1" class="comment_upvote action fas fa-arrow-up"></i>
                </a>
                <span class="comment_upvote_counter d-flex flex-row justify-content-center align-items-center mx-3"
                    style="max-width: 4rem;"><%= comment.UpvoteCount %></span>
                <a runat="server" role="button" id="comment_downvote_button" class="mr-1 p-0 text-gray-500 action_space comment_downvote_space dismissable_popover"
                    data-placement="top" data-content="you must log in to vote" data-comment-id='<%= comment.ID %>'>
                    <i tabindex="-1" class="comment_downvote action fas fa-arrow-down"></i>
                </a>
                <span class="comment_downvote_counter d-flex flex-row justify-content-center align-items-center mx-3"
                    style="max-width: 4rem;"><%= comment.DownvoteCount %></span>
            </div>
        </div>

        <div id="child_comments_space" runat="server" class="child_comments_space w-100">

            <div class="card shadow-sm w-100 mt-2 mb-3 hidden_reply" style="display: none;">
                <div class="card-body">

                    <div class="EditableAreaSpace mb-3">
                        <div class="EditableArea addReplyBody"></div>
                    </div>

                    <div class="d-flex flex-row justify-content-start add_reply_footer">
                        <button class="btn btn-secondary mr-2 cancel_reply_button">Cancel</button>
                        <button class="btn btn-primary confirm_reply_button">Submit</button>
                        <span class="add_reply_warning_space text-danger ml-3" style="display: none;"></span>
                    </div>
                </div>
            </div>

        </div>

    </div>
</div>
