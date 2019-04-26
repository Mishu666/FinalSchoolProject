<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommentControl.ascx.cs" Inherits="User_Controls_CommentControl" ClassName="User_Controls_CommentControl" %>

<div class='card shadow-sm mb-3' id="comment_card" runat="server">
    <div class='card-body'>
        <div class='card-text' id="comment_text" runat="server"></div>
    </div>

    <div class="d-flex flex-column justify-content-between align-items-start text-gray-700 card-footer" id="comment_footer">
        <div class="d-flex flex-row justify-content-between align-items-center w-100" id="footer_row">
            <div class="mr-auto d-flex flex-row w-100">
                <span>comment by&nbsp;</span><a href="#" id="comment_author_name" runat="server"></a>
                <span>&nbsp;on&nbsp; </span>
                <div id="comment_creation_date" runat="server"></div>
            </div>
            <div class="vote-block d-flex flex-row">
                <a role="button" id="comment_upvote_button" runat="server" class="mr-1 p-0 text-gray-500 comment_upvote_space dismissable_popover"
                    data-placement="top" data-content="you must log in to vote">
                    <i tabindex="-1" class="comment_upvote fas fa-arrow-up"></i>
                </a>
                <span id="comment_upvote_counter" runat="server" class="comment_upvote_counter d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;"></span>
                <a role="button" id="comment_downvote_button" runat="server" class="mr-1 p-0 text-gray-500 comment_downvote_space dismissable_popover"
                    data-placement="top" data-content="you must log in to vote">
                    <i tabindex="-1" class="comment_downvote fas fa-arrow-down"></i>
                </a>
                <span id="comment_downvote_counter" runat="server" class="comment_downvote_counter d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;"></span>
            </div>
        </div>

        <div id="child_comments_space" runat="server" class="w-100 mt-2"></div>

    </div>


</div>
