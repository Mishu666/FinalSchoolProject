<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PostControl.ascx.cs" Inherits="User_Controls_PostControl" ClassName="User_Controls_PostControl" %>

<% PostsClass post = PostsClass.GetByID(PostID); %>

<div class="card shadow-sm w-100 mb-3 post" id="post_card" data-id='<%= post.ID %>' data-rating='<%= post.UpvoteCount - post.DownvoteCount %>'
    data-date='<%= post.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") %>'>
    <div class="card-body">
        <h5 class="card-title text-gray-900" id="post_title"><%= post.Title %></h5>
        <p class="card-text" id="post_text"><%= post.Body %></p>
    </div>
    <div class="card-footer d-flex flex-row justify-content-between align-items-center text-gray-700" id="post_footer">
        <div class="comments-block d-flex flex-row justify-content-center align-items-center">
            <i class="fas fa-comment mr-1"></i>
            <span class="comment_count d-flex flex-row justify-content-center align-items-center mx-3" style="max-width: 4rem;" id="post_comment_count"><%= post.CommentCount %></span>
        </div>
        <div class="mr-auto d-flex flex-row">
            <span>potsed by </span><a href="ViewUser.aspx?user-id='<%= post.AuthorID %>'" id="post_author_name"><%= UsersClass.GetByID(post.AuthorID).Username %></a>
            <span> on </span>
            <div id="post_creation_date"><%= post.CreationDate.ToString("dd/MM/yyyy") %></div>
            <span> - </span><a href="ConsultPage.aspx?page-id=<%= post.ConsultPageID %>" id="post_consult_page">
                <%= ConsultPagesClass.GetByID(post.ConsultPageID).PageName %>
            </a>
        </div>
        <div class="vote-block d-flex flex-row">
            <a runat="server" role="button" id="upvote_button" class="mr-1 p-0 text-gray-500 action_space upvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote" data-post-id='<%= post.ID %>'>
                <i tabindex="-1" class="upvote action fas fa-arrow-up"></i>
            </a>
            <span id="upvote_counter" class="upvote_counter d-flex flex-row justify-content-center align-items-center mx-3" 
                style="max-width: 4rem;" data-post-id='<%= post.ID %>'><%= post.UpvoteCount %></span>
            <a runat="server" role="button" id="downvote_button" class="mr-1 p-0 text-gray-500 action_space downvote_space dismissable_popover"
                data-placement="top" data-content="you must log in to vote"  data-post-id='<%= post.ID %>'>
                <i tabindex="-1" class="downvote action fas fa-arrow-down"></i>
            </a>
            <span id="downvote_counter" class="downvote_counter d-flex flex-row justify-content-center align-items-center mx-3" 
                style="max-width: 4rem;" data-post-id='<%= post.ID %>'><%= post.DownvoteCount %></span>
        </div>
    </div>
</div>
