<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommentControl.ascx.cs" Inherits="User_Controls_CommentControl" ClassName="User_Controls_CommentControl" %>

<% CommentsClass comment = CommentsClass.GetByID(CommentID); %>

<div class='card shadow-sm mt-2 mb-3 comment' data-rating='<%= comment.UpvoteCount - comment.DownvoteCount %>'
    data-comment-id='<%= comment.ID %>' data-date='<%= comment.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") %>'>
    <div class='card-body'>
        <div class='card-text' id="comment_text"><%= comment.Body %></div>
    </div>

    <div class="d-flex flex-column justify-content-between align-items-start text-gray-700 card-footer w-100" id="comment_footer">
        <div class="d-flex flex-row justify-content-between align-items-center w-100">
            <div class="mr-auto d-flex flex-row w-100">
                <span>comment by&nbsp;</span><a href="ViewUser.aspx?user-id=<%= comment.CommentorID %>">
                    <%= UsersClass.GetByID(comment.CommentorID).Username %>
                </a>
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

                    <div class="input-group mb-3">
                        <textarea class="form-control addReplyBody" placeholder="Reply body"></textarea>
                    </div>

                    <div class="w-100 add_reply_warning_space"></div>
                    <div class="d-flex flex-row justify-content-end add_reply_footer">
                        <button class="btn btn-secondary mr-2 cancel_reply_button">Cancel</button>
                        <button class="btn btn-primary confirm_reply_button">Submit</button>
                    </div>
                </div>
            </div>

        </div>

    </div>
</div>
